﻿using Backend3DForge.Models;
using Backend3DForge.Requests;
using Backend3DForge.Responses;
using Backend3DForge.Services.FileStorage;
using Backend3DForge.Services.ModelCalculator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend3DForge.Controllers
{
	[ApiController]
	[Route("api/cart")]
	public class CartController : BaseController
	{
        protected readonly IFileStorage fileStorage;
        protected readonly IModelCalculator modelCalculator;
		protected readonly IConfiguration configuration;

		private float MinCost => configuration.GetValue<float>("MinCost");
		private float ValueAdded => configuration.GetValue<float>("ValueAdded");

		public CartController(DbApp db, IFileStorage fileStorage, IModelCalculator modelCalculator, IConfiguration configuration) : base(db)
		{
			this.fileStorage = fileStorage;
			this.modelCalculator = modelCalculator;
			this.configuration = configuration;
		}

		[Authorize]
		[HttpGet("getItems")]
		public async Task<IActionResult> GetItems()
		{
			var cart = await DB.Carts
				.Include(p => p.OrderedModels)
				.ThenInclude(p => p.PrintMaterialColor)
				.FirstOrDefaultAsync(x => x.UserId == AuthorizedUser.Id);

			if (cart is null)
			{
				return Ok(new BaseResponse.SuccessResponse("No items in the cart", new OrderedModelResponse.View[] { }));
			}

			return Ok(new CartResponse(cart));
		}

		[Authorize]
		[HttpPut("AddItem")]
		public async Task<IActionResult> AddItem([FromForm] CartAddRequest addRequest)
		{
			CatalogModel? catalogModel = null;
			ModelCalculatorResult? printParameters = null;
			string? printExtenstion = null;
			double price = 0;
			long fileSize = 0;

			if (addRequest.CatalogModelId is not null)
			{
				catalogModel = await DB.CatalogModels.FirstOrDefaultAsync(x => x.Id == addRequest.CatalogModelId);

				if (catalogModel is null)
				{
					return BadRequest(new BaseResponse.ErrorResponse("Model with chosen ID does not exists"));
				}

				printExtenstion = catalogModel.PrintExtensionId;
				price = catalogModel.MinPrice;
				fileSize = catalogModel.PrintFileSize;
            }
			else if (addRequest.File is not null)
			{
				printExtenstion = Path.GetExtension(addRequest.File.FileName).Replace(".", "");

				if (addRequest.File.Length > 1024 * 1024 * 50)
				{
					return BadRequest(new BaseResponse.ErrorResponse("Print file is too large. Max size: 50MB"));
				}

				var printExtension = await DB.PrintExtensions.SingleOrDefaultAsync(p => p.Id == printExtenstion);

				if (printExtension is null)
				{
					return BadRequest(new BaseResponse.ErrorResponse("Invalid file format for printing"));
				}

				try
				{
					using var fs = addRequest.File.OpenReadStream();
					printParameters = modelCalculator.CalculateSurfaceArea(fs, Path.GetExtension(addRequest.File.FileName).Replace(".", ""));
				}
				catch (Exception ex)
				{
					return BadRequest(new BaseResponse.ErrorResponse(ex.Message));
				}

				var cheapestMaterialCost = await DB.PrintMaterials
					.MinAsync(x => x.Cost);

				price = cheapestMaterialCost * (printParameters.Volume * Math.Pow(addRequest.Scale, 3) / 1000f);

				price = price * (1 + (addRequest.Depth / 100f));

				if (price < MinCost)
				{
					price = MinCost;
				}
				else
				{
					price += ValueAdded;
				}

                fileSize = addRequest.File.Length;

            }
			else
			{
				return BadRequest(new BaseResponse.ErrorResponse("Chose something one!"));
			}

			var printTypeName = await DB.PrintTypes.FirstOrDefaultAsync(x => x.Id == addRequest.PrintTypeName);

			if (printTypeName is null)
			{
				return BadRequest(new BaseResponse.ErrorResponse("This print type does not exists"));
			}

			var printMaterialName = await DB.PrintMaterials.FirstOrDefaultAsync(x => x.Id == addRequest.PrintMaterialName);

			if (printMaterialName is null)
			{ 
				return BadRequest(new BaseResponse.ErrorResponse("This print material does not exists"));
            }

            var color = await DB.PrintMaterialColors.SingleOrDefaultAsync(p => p.PrintMaterialId == printMaterialName.Id && p.Id == addRequest.ColorId);

            if (color is null)
            {
                return BadRequest(new BaseResponse.ErrorResponse($"This color [{addRequest.ColorId}] does not exists in material [{printMaterialName.Id}]"));
            }

            var cart = await DB.Carts.FirstOrDefaultAsync(x => x.UserId == AuthorizedUser.Id);

			if (cart is null)
			{
				cart = new Cart()
				{
					CreatedAt = DateTime.UtcNow,
					UserId = AuthorizedUser.Id
				};
				cart = (await DB.Carts.AddAsync(cart)).Entity;
				await DB.SaveChangesAsync();
			}

			var orderedModel = new OrderedModel()
			{
				CartId = cart.Id,
				CatalogModelId = catalogModel is null ? null : catalogModel.Id,
				PrintExtensionId = printExtenstion,
				PricePerPiece = price,
				Pieces = addRequest.Pieces,
				XSize = catalogModel?.XSize ?? printParameters?.X ?? 0f,
				YSize = catalogModel?.YSize ?? printParameters?.Y ?? 0f,
				ZSize = catalogModel?.ZSize ?? printParameters?.Z ?? 0f,
				FileSize = fileSize,
				Depth = addRequest.Depth,
				Scale = addRequest.Scale,
				PrintMaterialColorId = color.Id,
				PrintMaterialColor = color,
				PrintTypeId = printTypeName.Id,
				PrintMaterialId = printMaterialName.Id,
			};

			await DB.OrderedModels.AddAsync(orderedModel);

			await DB.SaveChangesAsync();

			if (addRequest.File is not null)
			{
				Task.WaitAll(new[]
				{
					fileStorage.UploadPrintFile(orderedModel, addRequest.File.OpenReadStream())
				});
			}

			return Ok(new OrderedModelResponse(orderedModel));
		}

		[Authorize]
		[HttpPut]
		public async Task<IActionResult> UpdateItem([FromForm] CartUpdateRequest request)
		{
			var cart = await DB.Carts
				.Include(p => p.OrderedModels)
				.FirstOrDefaultAsync(x => x.UserId == AuthorizedUserId);

			if (cart is null)
			{
				return BadRequest(new BaseResponse.SuccessResponse("Cart not found"));
			}

			var orderedModel = cart.OrderedModels.FirstOrDefault(x => x.Id == request.OrderedModelId);

			if (orderedModel is null)
			{
				return BadRequest(new BaseResponse.ErrorResponse("Selected ordered model does not exists"));
			}

			var printTypeName = await DB.PrintTypes.FirstOrDefaultAsync(x => x.Id == request.PrintTypeName);

			if (printTypeName is null)
			{
				return BadRequest(new BaseResponse.ErrorResponse("This print type does not exists"));
			}

			var printMaterialName = await DB.PrintMaterials.FirstOrDefaultAsync(x => x.Id == request.PrintMaterialName);

			if (printMaterialName is null)
			{
				return BadRequest(new BaseResponse.ErrorResponse("This print material does not exists"));
			}

			var color = await DB.PrintMaterialColors.SingleOrDefaultAsync(p => p.PrintMaterialId == printMaterialName.Id && p.Id == request.ColorId);

			if (color is null)
			{
				return BadRequest(new BaseResponse.ErrorResponse($"This color [{request.ColorId}] does not exists in material [{printMaterialName.Id}]"));
			}

			orderedModel.Pieces = request.Pieces;
			orderedModel.Depth = request.Depth;
			orderedModel.Scale = request.Scale;
			orderedModel.PrintMaterialColor = color;
			orderedModel.PrintTypeId = printTypeName.Id;
			orderedModel.PrintMaterialId = printMaterialName.Id;

			DB.OrderedModels.Update(orderedModel);

			await DB.SaveChangesAsync();

			return Ok(new BaseResponse.SuccessResponse("Model has been updated"));
		}

		[Authorize]
		[HttpDelete]
		public async Task<IActionResult> RemoveItem(int orderedModelId)
		{
			var cart = await DB.Carts
				.Include(p => p.OrderedModels)
				.FirstOrDefaultAsync(x => x.UserId == AuthorizedUserId);

			if (cart is null)
			{
				return BadRequest(new BaseResponse.SuccessResponse("No items in the cart"));
			}

			var orderedModel = cart.OrderedModels.FirstOrDefault(x => x.Id == orderedModelId);

			if (orderedModel is null)
			{
				return BadRequest(new BaseResponse.ErrorResponse("Selected ordered model does not exists"));
			}

			DB.OrderedModels.Remove(orderedModel);

			await DB.SaveChangesAsync();

			return Ok(new BaseResponse.SuccessResponse("Model has been removed from the cart"));
		}
	}
}
