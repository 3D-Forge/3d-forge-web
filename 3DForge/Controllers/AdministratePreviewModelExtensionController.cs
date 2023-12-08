using Backend3DForge.Attributes;
using Backend3DForge.Models;
using Backend3DForge.Requests.Administrate;
using Backend3DForge.Responses.Administrate;
using Microsoft.AspNetCore.Mvc;

namespace Backend3DForge.Controllers
{
    [Route("api/administration/preview-model-extension")]
    [ApiController]
    [CanAdministrateSystem]
    public class AdministratePreviewModelExtensionController : BaseAdministrateController<ModelExtension, string, PreviewModelExtensionRequest, PreviewModelExtensionResponse>
    {
        public AdministratePreviewModelExtensionController(DbApp db) : base(db)
        {
        }

        protected override PreviewModelExtensionResponse ConvertToResponse(ModelExtension entity) => entity;

        protected override ModelExtension ConvertToTable(PreviewModelExtensionRequest request) => new ModelExtension(request.Name);

        protected override void UpdateTable(ModelExtension entity, PreviewModelExtensionRequest request)
        {
            entity.Id = request.Name;
        }
    }
}
