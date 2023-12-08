using Backend3DForge.Attributes;
using Backend3DForge.Models;
using Backend3DForge.Requests.Administrate;
using Backend3DForge.Responses.Administrate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend3DForge.Controllers
{
    [Route("api/administration/print-material")]
    [ApiController]
    [CanAdministrateSystem]
    public class AdministratePrintMaterialController : BaseAdministrateController<PrintMaterial, string, PrintMaterialRequest, PrintMaterialResponse>
    {
        public AdministratePrintMaterialController(DbApp db) : base(db)
        {
        }

        protected override PrintMaterialResponse ConvertToResponse(PrintMaterial entity) => entity;

        protected override PrintMaterial ConvertToTable(PrintMaterialRequest request) => new PrintMaterial()
        {
            Id = request.MaterialName,
            PrintTypeId = request.TechnologyName,
            Cost = request.Cost,
            Density = request.Density
        };

        protected override void UpdateTable(PrintMaterial entity, PrintMaterialRequest request)
        {
            entity.Id = request.MaterialName;
            entity.Cost = request.Cost;
            entity.Density = request.Density;
            entity.PrintTypeId = request.TechnologyName;
        }

        protected override void BeforePost(DbSet<PrintMaterial> table, ref PrintMaterial record)
        {
            var printTypeName = record.PrintTypeId;
            var printType = db.PrintTypes.SingleOrDefault(p => p.Id == printTypeName);
            if (printType is null)
            {
                throw new Exception("PrintType not found");
            }
            record.PrintType = printType;
        }
    }
}
