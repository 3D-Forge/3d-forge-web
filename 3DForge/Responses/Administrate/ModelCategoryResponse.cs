using Backend3DForge.Models;

namespace Backend3DForge.Responses.Administrate
{
    public class ModelCategoryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ModelCategoryResponse() { }

        public ModelCategoryResponse(ModelCategory model)
        {
            Id = model.Id;
            Name = model.ModelCategoryName;
        }


        public static implicit operator ModelCategoryResponse(Models.ModelCategory v)
        {
            return new ModelCategoryResponse
            {
                Id = v.Id,
                Name = v.ModelCategoryName
            };
        }
    }
}
