using Backend3DForge.Models;

namespace Backend3DForge.Responses
{
    public class PrintMaterialColorResponse : BaseResponse
    {
        public PrintMaterialColorResponse(PrintMaterialColor color) : base(true, null, null)
        {
            Data = new View(color);
        }

        public PrintMaterialColorResponse(IEnumerable<PrintMaterialColor> colors) : base(true, null, null)
        {
            Data = colors.Select(c => new View(c));
        }

        class View
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public byte Red { get; set; }
            public byte Green { get; set; }
            public byte Blue { get; set; }
            public float Cost { get; set; }

            public View(PrintMaterialColor color)
            {
                Id = color.Id;
                Name = color.Name;
                Red = color.Red;
                Green = color.Green;
                Blue = color.Blue;
                Cost = color.Cost;
            }
        }
    }
}
