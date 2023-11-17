using Backend3DForge.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend3DForge.Responses
{
    public class CatalogModelFeedbackResponse : BaseResponse
    {

        public CatalogModelFeedbackResponse(CatalogModelFeedback feedback) : base(true, null, null)
        {
            Data = new View(feedback);
        }

        public CatalogModelFeedbackResponse(ICollection<CatalogModelFeedback> feedbacks): base(true, null, null)
        {
            Data = feedbacks.Select(p => new View(p));
        }

        public class View
        {
            public int Id { get; set; }
            public int OrderedModelId { get; set; }
            public int OrderId { get; set; }
            public string UserLogin { get; set; }
            public int Rate { get; set; }
            public string Text { get; set; }
            public string Pros { get; set; }
            public string Cons { get; set; }
            public DateTime CreatedAt { get; set; }

            public View(CatalogModelFeedback feedback)
            {
                Id = feedback.Id;
                OrderedModelId = feedback.OrderId;
                OrderId = feedback.Order.OrderId ?? -1;
                UserLogin = feedback.Order.Order?.User.Login ?? "";
                Rate = feedback.Rate;
                Text = feedback.Text;
                Pros = feedback.Pros;
                Cons = feedback.Cons;
                CreatedAt = feedback.CreatedAt;
            }
        }
    }
}
