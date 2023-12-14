using Backend3DForge.Models;

namespace Backend3DForge.Responses
{
    public class OrderResponse : BaseResponse
    {
        public OrderResponse(Order order) : base(true, null, null)
        {
            Data = new View(order);
        }

        public OrderResponse(IEnumerable<Order> orders) : base(true, null, null)
        {
            Data = orders.Select(p => new View(p));
        }

        public class View
        {
            public int Id { get; set; }
            public DateTime CreatedAt { get; set; }
            public string? UserLogin { get; set; }
            public string Firstname { get; set; }
            public string Midname { get; set; }
            public string? Lastname { get; set; }
            public string? Country { get; set; }
            public string? Region { get; set; }
            public string? CityRegion { get; set; }
            public string? City { get; set; }
            public string? Street { get; set; }
            public string? House { get; set; }
            public string? Apartment { get; set; }
            public int? DepartmentNumber { get; set; }
			public int? PostMachineNumber { get; set; }
			public string? DeliveryType { get; set; }
            public string? BillOfLading { get; set; }

            public OrderStatusResponse.View? CurrentStatus { get; set; }
            public double TotalPrice => Models.Sum(p => p.TotalPrice);

            public IEnumerable<OrderedModelResponse.View> Models { get; set; }
            public IEnumerable<OrderStatusResponse.View> OrderStatus { get; set; }

            public View(Order order)
            {
                Id = order.Id;
                CreatedAt = order.CreatedAt;
                UserLogin = order.User?.Login;
                Firstname = order.Firstname;
                Midname = order.Midname;
                Lastname = order.Lastname;
                Country = order.Country;
                Region = order.Region;
                CityRegion = order.CityRegion;
                Street = order.Street;
                House = order.House;
                Apartment = order.Apartment;
                DepartmentNumber = order.DepartmentNumber;
				PostMachineNumber = order.PostMachineNumber;
				DeliveryType = order.DeliveryType;
                BillOfLading = order.BillOfLading;

                Models = order.OrderedModels.Select(p => new OrderedModelResponse.View(p));
                OrderStatus = order.OrderStatusOrders.Select(p => new OrderStatusResponse.View(p));

                CurrentStatus = OrderStatus.OrderByDescending(p => p.CreatedAt).FirstOrDefault();
            }
        }
    }
}
