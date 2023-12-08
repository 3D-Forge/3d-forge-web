using Backend3DForge.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Backend3DForge.Controllers
{
    [Route("api/print")]
    [ApiController]
    public class PrintController : BaseController
    {
        protected readonly IMemoryCache memoryCache;
        public PrintController(DbApp db, IMemoryCache memoryCache) : base(db)
        {
            this.memoryCache = memoryCache;
        }

        [HttpGet("types")]
        public async Task<IActionResult> GetPrintTypes()
        {
            PrintTypeResponse? response = this.memoryCache.Get<PrintTypeResponse>("GET /api/print/types");

            if(response is null)
            {
                response = new PrintTypeResponse(await DB.PrintTypes.ToListAsync());
                this.memoryCache.Set("printTypes", response, TimeSpan.FromSeconds(30));
            }

            return Ok(response);
        }

        [HttpGet("types/{type}")]
        public async Task<IActionResult> GetPrintMaterials(string type)
        {
            PrintMaterialResponse? response = this.memoryCache.Get<PrintMaterialResponse>($"GET api/print/types/{type}");

            if (response is null)
            {
                if (!await DB.PrintTypes.AnyAsync(p => p.Id == type))
                {
                    return NotFound(new BaseResponse.ErrorResponse("Selected print type does not found."));
                }

                var printMaterials = await DB.PrintMaterials
                    .Where(p => p.PrintTypeId == type)
                    .ToListAsync();
                response = new PrintMaterialResponse(printMaterials);
                this.memoryCache.Set($"GET api/print/types/{type}", response, TimeSpan.FromSeconds(30));
            }
  
            return Ok(response);
        }

        [HttpGet("types/{type}/{material}")]
        public async Task<IActionResult> GetPrintMaterialColors(string type, string material)
        {
            PrintMaterialColorResponse? response = this.memoryCache.Get<PrintMaterialColorResponse>($"GET api/print/types/{type}/{material}");

            if (response is null)
            {
                if (!await DB.PrintTypes.AnyAsync(p => p.Id == type))
                {
                    return NotFound(new BaseResponse.ErrorResponse("Selected print type does not found."));
                }

                if (!await DB.PrintMaterials.AnyAsync(p => p.Id == material))
                {
                    return NotFound(new BaseResponse.ErrorResponse("Selected print material does not found."));
                }

                var printMaterialColors = await DB.PrintMaterialColors
                    .Include(p => p.PrintMaterial)
                    .Where(p => p.PrintMaterialId == material && p.PrintMaterial.PrintTypeId == type)
                    .ToListAsync();
                response = new PrintMaterialColorResponse(printMaterialColors);
                this.memoryCache.Set($"GET api/print/types/{type}/{material}", response, TimeSpan.FromSeconds(30));
            }

            return Ok(response);
        }
    }
}
