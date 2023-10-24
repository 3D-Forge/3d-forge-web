using Backend3DForge.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace Backend3DForge.Responses
{
    public class UserResponse : BaseResponse
    {
        public UserResponse(bool success, string? message, Models.User? data) : base(success, message, data)
        {
        }

        public class User
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }
            [JsonPropertyName("login")]
            public string Login { get; set; }
            [JsonPropertyName("passwordHash")]
            public string PasswordHash { get; set; }
            [JsonPropertyName("email")]
            public string Email { get; set; }
            [JsonPropertyName("birthday")]
            public DateTime Birthday { get; set; }
            [JsonPropertyName("sex")]
            public string? Sex { get; set; }
            [JsonPropertyName("phoneNumber")]
            public string? PhoneNumber { get; set; }
            [JsonPropertyName("firstName")]
            public string? Firstname { get; set; }
            [JsonPropertyName("midName")]
            public string? Midname { get; set; }
            [JsonPropertyName("lastName")]
            public string? Lastname { get; set; }
            [JsonPropertyName("region")]
            public string? Region { get; set; }
            [JsonPropertyName("cityRegion")]
            public string? CityRegion { get; set; }
            [JsonPropertyName("city")]
            public string? City { get; set; }
            [JsonPropertyName("street")]
            public string? Street { get; set; }
            [JsonPropertyName("house")]
            public string? House { get; set; }
            [JsonPropertyName("apartment")]
            public string? Apartment { get; set; }
            [JsonPropertyName("departmentNumber")]
            public string? DepartmentNumber { get; set; }
            [JsonPropertyName("deliveryType")]
            public string? DeliveryType { get; set; }
            [JsonPropertyName("blocked")]
            public bool Blocked { get; set; }
            [JsonPropertyName("canAdministrateForum")]
            public bool CanAdministrateForum { get; set; }
            [JsonPropertyName("canRetrieveDelivery")]
            public bool CanRetrieveDelivery { get; set; }
            [JsonPropertyName("canModerateCatalog")]
            public bool CanModerateCatalog { get; set; }
            [JsonPropertyName("canAdministrateSystem")]
            public bool CanAdministrateSystem { get; set; }
            [JsonPropertyName("registrationDate")]
            public DateTime RegistrationDate { get; set; }

            public User(Models.User user)
            {
                Id = user.Id;
                Login = user.Login;
                PasswordHash = user.PasswordHash;
                Email = user.Email;
                Birthday = user.Birthday;
                Sex = user.Sex;
                PhoneNumber = user.PhoneNumber;
                Firstname = user.Firstname;
                Midname = user.Midname;
                Lastname = user.Lastname;
                Region = user.Region;
                CityRegion = user.CityRegion;
                City = user.City;
                Street = user.Street;
                House = user.House;
                Apartment = user.Apartment;
                DepartmentNumber = user.DepartmentNumber;
                DeliveryType = user.DeliveryType;
                Blocked = user.Blocked;
                CanAdministrateForum = user.CanAdministrateForum;
                CanRetrieveDelivery = user.CanRetrieveDelivery;
                CanModerateCatalog = user.CanModerateCatalog;
                CanAdministrateSystem = user.CanAdministrateSystem;
                RegistrationDate = user.RegistrationDate;
            }
        }
    }
}
