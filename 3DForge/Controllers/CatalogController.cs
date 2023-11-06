﻿using Backend3DForge.Models;
using Backend3DForge.Requests;
using Backend3DForge.Responses;
using Backend3DForge.Services.FileStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NuGet.Packaging;
using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Controllers
{
    [Route("api/catalog")]
    [ApiController]
    public class CatalogController : BaseController
    {

        protected readonly IFileStorage fileStorage;
        protected readonly IMemoryCache memoryCache;

        public CatalogController(DbApp db, IFileStorage fileStorage, IMemoryCache memoryCache) : base(db)
        {
            this.fileStorage = fileStorage;
            this.memoryCache = memoryCache;
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
                return BadRequest(new BaseResponse.ErrorResponse("Please select files"));
            }

            var model = files[0];
            var modelEx = Path.GetExtension(model.FileName).Replace(".","");
            var print = files[1];
            var printEx = Path.GetExtension(print.FileName).Replace(".", "");

            var printExtension = await DB.PrintExtensions.SingleOrDefaultAsync(p => p.PrintExtensionName == printEx);

            if (printExtension is null)
            {
                return BadRequest(new BaseResponse.ErrorResponse("Invalid file format for printing"));
            }

            var modelExtension = await DB.ModelExtensions.SingleOrDefaultAsync(p => p.ModelExtensionName == modelEx);
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
                Height = 0,
                Width = 0,
                Depth = request.Depth,
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
