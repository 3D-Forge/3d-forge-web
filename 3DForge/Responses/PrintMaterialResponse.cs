using Backend3DForge.Models;

namespace Backend3DForge.Responses
{
    public class PrintMaterialResponse : BaseResponse
    {
        public PrintMaterialResponse(PrintMaterial material) : base(true, null, null)
        {
            Data = new View(material);
        }

        public PrintMaterialResponse(IEnumerable<PrintMaterial> materials) : base(true, null, null)
        {
            Data = materials.Select(p => new View(p));
        }

        class View
        {
            public string Material { get; set; }
            public string PrintType{ get; set; }
            public float Density { get; set; }
            public float Cost { get; set; }

            public View(PrintMaterial material)
            {
                Material = material.Id;
                PrintType = material.PrintTypeId;
                Density = material.Density;
                Cost = material.Cost;
            }
        }
    }
}
