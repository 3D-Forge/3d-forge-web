using Backend3DForge.Attributes;
using Backend3DForge.Models;
using Backend3DForge.Requests.Administrate;
using Backend3DForge.Responses.Administrate;
using Microsoft.AspNetCore.Mvc;

namespace Backend3DForge.Controllers
{
    [Route("api/administration/print-extension")]
    [ApiController]
    [CanAdministrateSystem]
    public class AdministratePrintExtensionController : BaseAdministrateController<PrintExtension, string, PrintExtensionRequest, PrintExtensionResponse>
    {
        public AdministratePrintExtensionController(DbApp db) : base(db)
        {
        }

        protected override PrintExtensionResponse ConvertToResponse(PrintExtension entity) => entity;

        protected override PrintExtension ConvertToTable(PrintExtensionRequest request) => new PrintExtension(request.Name);

        protected override void UpdateTable(PrintExtension entity, PrintExtensionRequest request)
        {
            entity.Id = request.Name;
        }
    }
}
