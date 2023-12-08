using Backend3DForge.Models;

namespace Backend3DForge.Responses.Administrate
{
    public class PrintMaterialColorResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }
        public float Cost { get; set; }
        public string PrintMaterialId { get; set; }
        public string PrintTypeId { get; set; }

        public PrintMaterialColorResponse(PrintMaterialColor color)
        {
            Id = color.Id;
            Name = color.Name;
            Red = color.Red;
            Green = color.Green;
            Blue = color.Blue;
            Cost = color.Cost;
            PrintMaterialId = color.PrintMaterialId;
            PrintTypeId = color.PrintMaterial.PrintTypeId;
        }

        public PrintMaterialColorResponse()
        {
        }

        public static implicit operator PrintMaterialColorResponse(PrintMaterialColor color)
        {
            return new PrintMaterialColorResponse(color);
        }
    }
}
