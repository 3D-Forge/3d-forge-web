using Backend3DForge.Attributes;
using Backend3DForge.Models;
using Backend3DForge.Requests.Administrate;
using Backend3DForge.Responses.Administrate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend3DForge.Controllers
{
    [Route("api/administration/print-material-color")]
    [ApiController]
    [CanAdministrateSystem]
    public class AdministratePrintMaterialColorController : BaseAdministrateController<PrintMaterialColor, int, PrintMaterialColorRequest, PrintMaterialColorResponse>
    {
        public AdministratePrintMaterialColorController(DbApp db) : base(db)
        {
        }

        protected override IQueryable<PrintMaterialColor> LoadDate(DbSet<PrintMaterialColor> table)
        {
            return table.Include(p => p.PrintMaterial);
        }

        protected override PrintMaterialColorResponse ConvertToResponse(PrintMaterialColor entity) => entity;

        protected override PrintMaterialColor ConvertToTable(PrintMaterialColorRequest request) => new PrintMaterialColor()
        {
            Name = request.Name,
            Red = request.Red,
            Green = request.Green,
            Blue = request.Blue,
            Cost = request.Cost,
            PrintMaterialId = request.PrintMaterialId
        };

        protected override void UpdateTable(PrintMaterialColor entity, PrintMaterialColorRequest request)
        {
            var printMaterial = db.PrintMaterials.SingleOrDefault(p => p.Id == request.PrintMaterialId);

            if(printMaterial is null)
            {
                throw new Exception("PrintMaterial not found");
            }
            entity.PrintMaterial = printMaterial;
            entity.PrintMaterialId = printMaterial.Id;
            entity.Red = request.Red;
            entity.Green = request.Green;
            entity.Blue = request.Blue;
            entity.Cost = request.Cost;
        }

        protected override void BeforePost(DbSet<PrintMaterialColor> table, ref PrintMaterialColor record)
        {
            var printMaterialId = record.PrintMaterialId;
            var printMaterial = db.PrintMaterials.SingleOrDefault(p => p.Id == printMaterialId);
            if (printMaterial is null)
            {
                throw new Exception("PrintMaterial not found");
            }
            record.PrintMaterial = printMaterial;
        }
    }
}
