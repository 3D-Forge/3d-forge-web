using Backend3DForge.Models;

namespace Backend3DForge.Responses.Administrate
{
    public class UserResponse : BaseResponse
    {
        public UserResponse(User user) : base(true, null, new View(user))
        {
        }

        public UserResponse(IEnumerable<User> users) : base(true, null, users.Select(p => new View(p)))
        {
        }

        public class View
        {
            public int Id { get; set; }
            public string Login { get; set; }
            public string Email { get; set; }
            public string? PhoneNumber { get; set; }
            public string? Firstname { get; set; }
            public string? Midname { get; set; }
            public string? Lastname { get; set; }
            public string? Country { get; set; }
            public string? Region { get; set; }
            public string? City { get; set; }
            public string? Street { get; set; }
            public string? House { get; set; }
            public string? Apartment { get; set; }
            public string? DepartmentNumber { get; set; }
            public string? DeliveryType { get; set; }
            public bool OrderStateChangedNotification { get; set; }
            public bool GetForumResponseNotification { get; set; }
            public bool ModelRatedNotification { get; set; }
            public bool Blocked { get; set; }
            public bool CanAdministrateForum { get; set; }
            public bool CanRetrieveDelivery { get; set; }
            public bool CanModerateCatalog { get; set; }
            public bool CanAdministrateSystem { get; set; }
            public bool IsActivated { get; set; }
            public DateTime RegistrationDate { get; set; }

            public View(User user)
            {
                Id = user.Id;
                Login = user.Login;
                Email = user.Email;
                PhoneNumber = user.PhoneNumber;
                Firstname = user.Firstname;
                Midname = user.Midname;
                Lastname = user.Lastname;
                Country = user.Country;
                Region = user.Region;
                City = user.City;
                Street = user.Street;
                House = user.House;
                Apartment = user.Apartment;
                DepartmentNumber = user.DepartmentNumber;
                DeliveryType = user.DeliveryType;
                OrderStateChangedNotification = user.OrderStateChangedNotification;
                GetForumResponseNotification = user.GetForumResponseNotification;
                ModelRatedNotification = user.ModelRatedNotification;
                Blocked = user.Blocked;
                CanAdministrateForum = user.CanAdministrateForum;
                CanRetrieveDelivery = user.CanRetrieveDelivery;
                CanModerateCatalog = user.CanModerateCatalog;
                CanAdministrateSystem = user.CanAdministrateSystem;
                IsActivated = user.IsActivated;
                RegistrationDate = user.RegistrationDate;
            }
        }
    }
}
