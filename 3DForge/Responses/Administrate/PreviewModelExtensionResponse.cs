using Backend3DForge.Models;

namespace Backend3DForge.Responses.Administrate
{
    public class PreviewModelExtensionResponse
    {
        public string Name { get; set; }

        public PreviewModelExtensionResponse(ModelExtension modelExtension)
        {
            Name = modelExtension.Id;
        }

        public PreviewModelExtensionResponse()
        {
        }

        public static implicit operator PreviewModelExtensionResponse(ModelExtension modelExtension)
        {
            return new PreviewModelExtensionResponse(modelExtension);
        }
    }
}
