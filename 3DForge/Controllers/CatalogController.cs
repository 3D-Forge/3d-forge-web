using Backend3DForge.Models;
using Backend3DForge.Requests;
using Backend3DForge.Responses;
using Backend3DForge.Services.FileStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace Backend3DForge.Controllers
{
    [Route("api/catalog")]
    [ApiController]
    public class CatalogController : BaseController
    {

        protected readonly IFileStorage fileStorage;

        public CatalogController(DbApp db, IFileStorage fileStorage) : base(db)
        {
            this.fileStorage = fileStorage;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddNewModel([FromBody] Publish3DModelRequest request, IFormFile print, IFormFile model)
        {
            var printExtension = await DB.PrintExtensions.SingleOrDefaultAsync(p => p.PrintExtensionName == print.ContentType);

            if (printExtension is null)
            {
                return BadRequest(new BaseResponse.ErrorResponse("Invalid file format for printing"));
            }

            var modelExtension = await DB.ModelExtensions.SingleOrDefaultAsync(p => p.ModelExtensionName == model.ContentType);
            if (modelExtension is null)
            {
                return BadRequest(new BaseResponse.ErrorResponse("Invalid 3D model file format"));
            }

            HashSet<Keyword> keywords = new HashSet<Keyword>();

            if(request.Keywords is not null)
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

            HashSet<ModelCategory> categories = new HashSet<ModelCategory>();

            foreach(var category in request.Categories)
            {
                ModelCategory? categoryObj = await DB.ModelCategories.SingleOrDefaultAsync(c => c.Id == category);
                if(categoryObj is null)
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
                Height = request.Height,
                Width = request.Width,
                Depth = request.Depth,
                Color = request.Color,
                ModelExtensionId = modelExtension.Id,
                PrintExtensionId = printExtension.Id,
                ModelFileSize = model.Length,
                PrintFileSize = print.Length,
                User = AuthorizedUser,
                Uploaded = DateTime.UtcNow,
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
    }
}
