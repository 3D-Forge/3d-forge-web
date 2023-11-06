using Backend3DForge.Models;
using NuGet.Packaging;
using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Responses
{
    public class CatalogModelResponse : BaseResponse
    {
        public CatalogModelResponse(CatalogModel model) : base(true, null, null)
        {
            Data = new View(model);
        }

        public CatalogModelResponse(ICollection<CatalogModel> models) : base(true, null, null)
        {
            Data = models.Select(p => new View(p));
        }

        public class View
        {
            [Key]
            public int Id { get; set; }
            [Required]
            public string Name { get; set; }
            [Required]
            public string Description { get; set; }
            [Required]
            public long PrintFileSize { get; set; }
            [Required]
            public long ModelFileSize { get; set; }
            [Required]
            public string Owner { get; set; }
            [Required]
            public DateTime Uploaded { get; set; }
            [Required]
            public float Height { get; set; }
            [Required]
            public float Width { get; set; }
            [Required]
            public float Depth { get; set; }

            public ICollection<ModelCategoryResponse.View> Categoryes { get; set; } = new List<ModelCategoryResponse.View>();
            public ICollection<string> Keywords { get; set; } = new List<string>();

            public View(CatalogModel model)
            {
                this.Id = model.Id;
                this.Name = model.Name;
                this.Description = model.Description;
                this.PrintFileSize = model.PrintFileSize;
                this.ModelFileSize = model.ModelFileSize;
                this.Owner = model.User.Login;
                this.Uploaded = model.Uploaded;
                this.Height = model.Height;
                this.Width = model.Width;
                this.Depth = model.Depth;

                this.Categoryes.AddRange(model.ModelCategoryes.Select(p => new ModelCategoryResponse.View(p)));
                this.Keywords.AddRange(model.Keywords.Select(p => p.Name));
            }
        }
    }
}
