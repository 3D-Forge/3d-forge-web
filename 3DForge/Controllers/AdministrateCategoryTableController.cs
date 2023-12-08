using Backend3DForge.Attributes;
using Backend3DForge.Models;
using Backend3DForge.Requests.Administrate;
using Backend3DForge.Responses.Administrate;
using Microsoft.AspNetCore.Mvc;

namespace Backend3DForge.Controllers
{
    [Route("api/administration/category-table")]
    [ApiController]
    [CanAdministrateSystem]
    public class AdministrateCategoryTableController : BaseAdministrateController<ModelCategory, int, ModelCategoryRequest, ModelCategoryResponse>
    {
        public AdministrateCategoryTableController(DbApp db) : base(db)
        {
        }

        protected override ModelCategoryResponse ConvertToResponse(ModelCategory entity) => entity;

        protected override ModelCategory ConvertToTable(ModelCategoryRequest request) => new ModelCategory(request.Name);

        protected override void UpdateTable(ModelCategory entity, ModelCategoryRequest request)
        {
            entity.ModelCategoryName = request.Name;
        }
    }
}
