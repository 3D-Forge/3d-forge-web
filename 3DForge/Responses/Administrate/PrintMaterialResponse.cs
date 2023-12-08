namespace Backend3DForge.Responses.Administrate
{
    public class PrintMaterialResponse
    {
        public string MaterialName { get; set; }
        public string TechnologyName { get; set; }
        public float Density { get; set; }
        public float Cost { get; set; }

        public PrintMaterialResponse(Models.PrintMaterial printMaterial)
        {
            MaterialName = printMaterial.Id;
            TechnologyName = printMaterial.PrintTypeId;
            Density = printMaterial.Density;
            Cost = printMaterial.Cost;
        }

        public PrintMaterialResponse()
        {
        }

        public static implicit operator PrintMaterialResponse(Models.PrintMaterial printMaterial)
        {
            return new PrintMaterialResponse(printMaterial);
        }
    }
}
