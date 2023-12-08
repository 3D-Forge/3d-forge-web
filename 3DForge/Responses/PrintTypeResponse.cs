using Backend3DForge.Models;

namespace Backend3DForge.Responses
{
    public class PrintTypeResponse : BaseResponse
    {
        public PrintTypeResponse(PrintType print) : base(true, null, null)
        {
            Data = new View(print);
        }

        public PrintTypeResponse(IEnumerable<PrintType> prints) : base(true, null, null)
        {
            Data = prints.Select(p => new View(p));
        }

        class View
        {
            public string Name { get; set; }

            public View(PrintType print)
            {
                Name = print.Id;
            }
        }
    }
}
