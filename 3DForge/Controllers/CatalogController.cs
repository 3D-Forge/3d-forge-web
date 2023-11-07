using Backend3DForge.Attributes;
using Backend3DForge.Models;
using Backend3DForge.Requests;
using Backend3DForge.Responses;
using Backend3DForge.Services.Email;
using Backend3DForge.Services.FileStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NuGet.Packaging;
using CatalogModelResponse = Backend3DForge.Responses.CatalogModelResponse;

namespace Backend3DForge.Controllers
{
	[Route("api/catalog")]
	[ApiController]
	public class CatalogController : BaseController
	{

		protected readonly IFileStorage fileStorage;
		protected readonly IMemoryCache memoryCache;
		protected readonly IEmailService emailService;

		public CatalogController(DbApp db, IFileStorage fileStorage, IMemoryCache memoryCache, IEmailService emailService) : base(db)
		{
			this.fileStorage = fileStorage;
			this.memoryCache = memoryCache;
			this.emailService = emailService;
		}

		[HttpGet("categories")]
		public async Task<IActionResult> GetCategories()
		{
			ModelCategoryResponse? response;

			if (!memoryCache.TryGetValue("GET api/catalog/categories", out response))
			{
				var categories = await DB.ModelCategories.ToArrayAsync();
				response = new ModelCategoryResponse(categories);
				memoryCache.Set("GET api/catalog/categories", response, TimeSpan.FromMinutes(10));
			}

			return Ok(response);
		}

		[HttpGet("keywords")]
		public async Task<IActionResult> SearchKeywords([FromQuery(Name = "q")] string q)
		{
			q = q.ToLower();
			var result = await DB.Keywords
				.Where(p => p.Name.ToLower().Contains(q))
				.Take(20)
				.ToArrayAsync();
			return Ok(new KeywordResponse(result));
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> AddNewModel([FromForm] Publish3DModelRequest request, [FromForm] IFormFileCollection files)
		{
			if (files.Count != 2)
			{
				return BadRequest(new BaseResponse.ErrorResponse("Please select 2 files"));
			}

			var model = files[0];
			var modelEx = Path.GetExtension(model.FileName).Replace(".", "");
			var print = files[1];
			var printEx = Path.GetExtension(print.FileName).Replace(".", "");

			var printExtension = await DB.PrintExtensions.SingleOrDefaultAsync(p => p.Name == printEx);

			if (printExtension is null)
			{
				return BadRequest(new BaseResponse.ErrorResponse("Invalid file format for printing"));
			}

			var modelExtension = await DB.ModelExtensions.SingleOrDefaultAsync(p => p.Name == modelEx);
			if (modelExtension is null)
			{
				return BadRequest(new BaseResponse.ErrorResponse("Invalid 3D model file format"));
			}

			HashSet<Keyword> keywords = new HashSet<Keyword>();

			if (request.Keywords is not null)
			{
				foreach (var keyword in request.Keywords)
				{
					Keyword? keywordObj = await DB.Keywords.SingleOrDefaultAsync(k => k.Name == keyword);
					if (keywordObj is null)
					{
						keywords.Add((await DB.Keywords.AddAsync(new Keyword
						{
							Name = keyword
						})).Entity);
					}
					else
					{
						keywords.Add(keywordObj);
					}
				}
			}

			await DB.SaveChangesAsync();

			HashSet<ModelCategory> categories = new HashSet<ModelCategory>();

			foreach (var category in request.Categories)
			{
				ModelCategory? categoryObj = await DB.ModelCategories.SingleOrDefaultAsync(c => c.Id == category);
				if (categoryObj is null)
				{
					return BadRequest(new BaseResponse.ErrorResponse($"'{category}' not found"));
				}
				else
				{
					categories.Add(categoryObj);
				}
			}


			var newModel = (await DB.CatalogModels.AddAsync(new CatalogModel
			{
				Name = request.Name,
				Description = request.Description,
				Height = 0,
				Width = 0,
				Depth = request.Depth,
				ModelExtensionName = modelExtension.Name,
				PrintExtensionName = printExtension.Name,
				ModelFileSize = model.Length,
				PrintFileSize = print.Length,
				User = AuthorizedUser,
				Uploaded = DateTime.UtcNow
			})).Entity;

			newModel.Keywords.AddRange(keywords);
			newModel.ModelCategoryes.AddRange(categories);

			await DB.SaveChangesAsync();

			Task.WaitAll(new[]
			{
				this.fileStorage.UploadPreviewModel(newModel, model.OpenReadStream()),
				this.fileStorage.UploadPrintFile(newModel, print.OpenReadStream())
			});
			return Ok(new Responses.CatalogModelResponse(newModel));
		}

		[HttpGet("{modelId}")]
		public async Task<IActionResult> GetModel([FromRoute] int modelId)
		{
			BaseResponse? response;

			if (!this.memoryCache.TryGetValue($"GET api/catalog/{modelId}", out response))
			{
				var model = await DB.CatalogModels
				 .Include(p => p.User)
				 .Include(p => p.ModelCategoryes)
				 .Include(p => p.Keywords)
				 .Include(p => p.ModelExtension)
				 .Include(p => p.PrintExtension)
				 .SingleOrDefaultAsync(p => p.Id == modelId);
				if (model is null)
				{
					response = new BaseResponse.ErrorResponse("Model not found");
				}
				else
				{
					response = new CatalogModelResponse(model);
				}
				this.memoryCache.Set($"GET api/catalog/{modelId}", response, TimeSpan.FromSeconds(10));
			}

			if (response.Success)
				return Ok(response);
			else
				return NotFound(response);
		}

		[HttpGet("{modelId}/preview")]
		public async Task<IActionResult> GetModelPreview([FromRoute] int modelId)
		{
			Stream fileStream;

			var model = await DB.CatalogModels
				.Include(p => p.User)
				.SingleOrDefaultAsync(p => p.Id == modelId);

			if (model is null)
				return NotFound(new BaseResponse.ErrorResponse($"Model '{modelId}' not found"));

			try
			{
				fileStream = await fileStorage.DownloadPreviewModel(model);
			}
			catch (Exception ex)
			{
				return BadRequest(new BaseResponse.ErrorResponse(ex.Message, ex));
			}
			HttpContext.Response.ContentLength = model.ModelFileSize;
			HttpContext.Response.Headers.Add("Content-Disposition", $"attachment; filename={model.Name}_{model.User.Login}.{model.ModelExtensionName}");
			return new FileStreamResult(fileStream, "application/octet-stream");
		}

		[Authorize]
		[HttpGet("unaccepted")]
		public async Task<IActionResult> GetUnacceptedModels()
		{
			if (!AuthorizedUser.CanModerateCatalog)
			{
				return StatusCode(403);
			}

			var models = await DB.CatalogModels.Where(x => x.Publicized == null).ToListAsync();

			return Ok(new CatalogModelResponse(models));
		}

		[Authorize]
		[CanModerateCatalog]
		[HttpPost("{modelId}/accept")]
		public async Task<IActionResult> AcceptModel([FromRoute] int modelId, [FromBody] AcceptRequest acceptRequest)
		{
			var model = await DB.CatalogModels
				.Include(p => p.User)
				.SingleOrDefaultAsync(p => p.Id == modelId);

			if (model is null)
			{
				return NotFound("Model not found");
			}

			var owner = model.User;

			if (!AuthorizedUser.CanModerateCatalog && owner != AuthorizedUser)
			{
				return StatusCode(403);
			}

			if (owner != AuthorizedUser)
			{
				try
				{
					await emailService.SendEmailUseTemplateAsync(
						email: owner.Email,
						templateName: "model_acception_notification.html",
						parameters: new Dictionary<string, string>
						{
							{ "login", owner.Login },
							{ "action", acceptRequest.Accepted ? "accepted" : "not accepted" },
							{ "message", acceptRequest.Message ?? "No message provided."},
						}
					);
				}
				catch (Exception ex)
				{
					return BadRequest(new BaseResponse.ErrorResponse(ex.Message));
				}
			}

			if (acceptRequest.Accepted)
			{
				model.Publicized = DateTime.Now;
			}
			else
			{
				DB.Remove(model);
			}

			await DB.SaveChangesAsync();

			return Ok(new BaseResponse.SuccessResponse("Model reviewed"));
		}

		[Authorize]
		[HttpPut("{modelId}/updateInfo")]
		public async Task<IActionResult> UpdateModelDescription([FromRoute] int modelId, [FromForm] Update3DModelRequest request)
		{

			bool different = false;

			HashSet<Keyword> keywords = new HashSet<Keyword>();

			if (request.Keywords is not null)
			{
				foreach (var keyword in request.Keywords)
				{
					Keyword? keywordObj = await DB.Keywords.SingleOrDefaultAsync(k => k.Name == keyword);
					if (keywordObj is null)
					{
						keywords.Add((await DB.Keywords.AddAsync(new Keyword
						{
							Name = keyword
						})).Entity);
					}
					else
					{
						keywords.Add(keywordObj);
					}
				}
			}

			await DB.SaveChangesAsync();

			HashSet<ModelCategory> categories = new HashSet<ModelCategory>();
			if (request.Categories is not null)
			{
				foreach (var category in request.Categories)
				{
					ModelCategory? categoryObj = await DB.ModelCategories.SingleOrDefaultAsync(c => c.Id == category);
					if (categoryObj is null)
					{
						return BadRequest(new BaseResponse.ErrorResponse($"'{category}' not found"));
					}
					else
					{
						categories.Add(categoryObj);
					}
				}
			}

			var catalogModel = await DB.CatalogModels
				.Include(p => p.Keywords)
				.Include(p => p.ModelCategoryes)
				.Include(p => p.User)
				.SingleOrDefaultAsync(p => p.Id == modelId);

			if (catalogModel is null)
			{
				return NotFound(new BaseResponse.ErrorResponse("Model not found"));
			}


			if (catalogModel.Name != (request.Name ?? catalogModel.Name))
			{
				catalogModel.Name = request.Name;
				different = true;
			}
			if (catalogModel.Description != (request.Description ?? catalogModel.Description))
			{
				catalogModel.Description = request.Description;
				different = true;
			}
			if (request.Depth.HasValue)
			{
				catalogModel.Depth = request.Depth.Value;
				different = true;
			}
			if (keywords.Except(catalogModel.Keywords).Count() != catalogModel.Keywords.Count)
			{
				catalogModel.Keywords = keywords;
				different = true;
			}
			if (categories.Except(catalogModel.ModelCategoryes).Count() != catalogModel.Keywords.Count)
			{
				catalogModel.ModelCategoryes = categories;
				different = true;
			}

			if (!different)
			{
				return BadRequest(new BaseResponse.ErrorResponse("Data are identical"));
			}

			await DB.SaveChangesAsync();

			return Ok(new CatalogModelResponse(catalogModel));
		}

		[Authorize]
		[HttpPut("{modelId}/updateFiles")]
		public async Task<IActionResult> UpdateModelFiles([FromRoute] int modelId, [FromForm] IFormFileCollection files)
		{
			if (files.Count != 2)
			{
				return BadRequest(new BaseResponse.ErrorResponse("Please select 2 files"));
			}

			var model = files[0];
			var modelEx = Path.GetExtension(model.FileName).Replace(".", "");
			var print = files[1];
			var printEx = Path.GetExtension(print.FileName).Replace(".", "");

			var printExtension = await DB.PrintExtensions.SingleOrDefaultAsync(p => p.Name == printEx);

			if (printExtension is null)
			{
				return BadRequest(new BaseResponse.ErrorResponse("Invalid file format for printing"));
			}

			var modelExtension = await DB.ModelExtensions.SingleOrDefaultAsync(p => p.Name == modelEx);
			if (modelExtension is null)
			{
				return BadRequest(new BaseResponse.ErrorResponse("Invalid 3D model file format"));
			}

			var catalogModel = await DB.CatalogModels
				.Include(p => p.Keywords)
				.Include(p => p.ModelCategoryes)
				.SingleOrDefaultAsync(p => p.Id == modelId);

			if (catalogModel is null)
			{
				return NotFound(new BaseResponse.ErrorResponse("Model not found"));
			}

			catalogModel.ModelExtensionName = modelExtension.Name;
			catalogModel.PrintExtensionName = printExtension.Name;
			catalogModel.ModelFileSize = model.Length;
			catalogModel.PrintFileSize = print.Length;

			await DB.SaveChangesAsync();

			Task.WaitAll(new[]
			{
				this.fileStorage.UploadPreviewModel(catalogModel, model.OpenReadStream()),
				this.fileStorage.UploadPrintFile(catalogModel, print.OpenReadStream())
			});
			return Ok(new Responses.CatalogModelResponse(catalogModel));
		}

		[Authorize]
		[HttpDelete("{modelId}")]
		public async Task<IActionResult> DeleteModel([FromRoute] int modelId, [FromBody] string reason)
		{
			var model = await DB.CatalogModels
				.Include(p => p.User)
				.SingleOrDefaultAsync(p => p.Id == modelId);

			if (model is null)
			{
				return NotFound("Model not found");
			}

			var owner = model.User;

			if (!AuthorizedUser.CanModerateCatalog && owner != AuthorizedUser)
			{
				return StatusCode(403);
			}

			if (owner != AuthorizedUser)
			{
				try
				{
					await emailService.SendEmailUseTemplateAsync(
						email: owner.Email,
						templateName: "model_deletion_notification.html",
						parameters: new Dictionary<string, string>
						{
							{ "login", owner.Login },
							{ "reason", reason }
						}
					);
				}
				catch (Exception ex)
				{
					return BadRequest(new BaseResponse.ErrorResponse(ex.Message));
				}
			}

			DB.Remove(model);

			await DB.SaveChangesAsync();

			return Ok(new BaseResponse.SuccessResponse("Model deleted"));
		}
	}
}
