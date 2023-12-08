using Backend3DForge.Attributes;
using Backend3DForge.Models;
using Backend3DForge.Requests.Administrate;
using Backend3DForge.Responses.Administrate;
using Microsoft.AspNetCore.Mvc;

namespace Backend3DForge.Controllers
{
    [Route("api/administration/print-type")]
    [ApiController]
    [CanAdministrateSystem]
    public class AdministratePrintTypeController : BaseAdministrateController<PrintType, string, PrintTypeRequest, PrintTypeResponse>
    {
        public AdministratePrintTypeController(DbApp db) : base(db)
        {
        }

        protected override PrintTypeResponse ConvertToResponse(PrintType entity) => entity;

        protected override PrintType ConvertToTable(PrintTypeRequest request) => new PrintType(request.Name);

        protected override void UpdateTable(PrintType entity, PrintTypeRequest request)
        {
            entity.Id = request.Name;
        }
    }
}
