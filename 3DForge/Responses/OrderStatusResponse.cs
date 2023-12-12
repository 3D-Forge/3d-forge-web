namespace Backend3DForge.Responses
{
    public class OrderStatusResponse : BaseResponse
    {
        public OrderStatusResponse(Models.OrderStatusOrder model) : base(true, null, null)
        {
            Data = new View(model);
        }
        public class View
        {
            public int Id { get; set; }
            public string Status { get; set; }
            public int StatusCode { get; set; }
            public int OrderId { get; set; }
            public DateTime CreatedAt { get; set; }
            public View(Models.OrderStatusOrder model)
            {
                Id = model.Id;
                Status = Enum.GetName(model.OrderStatus) ?? "unknown";
                StatusCode = (int)model.OrderStatus;
                OrderId = model.OrderId;
                CreatedAt = model.CreatedAt;
            }
        }
    }
}
