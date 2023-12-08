namespace Backend3DForge.Responses.Administrate
{
    public class PrintTypeResponse
    {
        public string Name { get; set; }

        public PrintTypeResponse(Models.PrintType printType)
        {
            Name = printType.Id;
        }

        public PrintTypeResponse()
        {
        }

        public static implicit operator PrintTypeResponse(Models.PrintType printType)
        {
            return new PrintTypeResponse(printType);
        }
    }
}
