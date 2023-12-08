namespace Backend3DForge.Responses.Administrate
{
    public class PrintExtensionResponse
    {
        public string Name { get; set; }

        public PrintExtensionResponse(Models.PrintExtension printExtension)
        {
            Name = printExtension.Id;
        }

        public PrintExtensionResponse()
        {
        }

        public static implicit operator PrintExtensionResponse(Models.PrintExtension printExtension)
        {
            return new PrintExtensionResponse(printExtension);
        }
    }
}
