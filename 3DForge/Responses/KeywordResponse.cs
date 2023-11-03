using Backend3DForge.Models;

namespace Backend3DForge.Responses
{
    public class KeywordResponse : BaseResponse
    {
        public KeywordResponse(Keyword model) : base(true, null, null)
        {
            Data = new View(model);
        }

        public KeywordResponse(ICollection<Keyword> models) : base(true, null, null)
        {
            Data = models.Select(p => new View(p));
        }

        public class View
        {
            public string Name { get; set; }

            public View(Keyword model)
            {
                Name = model.Name;
            }
        }
    }
}
