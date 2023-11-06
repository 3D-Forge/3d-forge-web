using Azure.Core;
using Backend3DForge.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend3DForge.Requests
{
    public class UpdateUserInfoRequest
    {
        [StringLength(32, MinimumLength = 4, ErrorMessage = "Login is too short or long")]
        public string? Login { get; set; }
        public string? Email { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
        public string? Firstname { get; set; }
        public string? Midname { get; set; }
        public string? Lastname { get; set; }
        public string? Region { get; set; }
        public string? CityRegion { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? House { get; set; }
        public string? Apartment { get; set; }
        public string? DepartmentNumber { get; set; }
        public string? DeliveryType { get; set; }

        public bool HasDifferences(User user)
		{
            return user.Login != Login
            && user.Email != Email
            && user.PhoneNumber != PhoneNumber
            && user.Firstname != Firstname
            && user.Midname != Midname
            && user.Lastname != Lastname
            && user.Region != Region
            && user.City != City
            && user.Street != Street
            && user.House != House
            && user.Apartment != Apartment
            && user.DepartmentNumber != DepartmentNumber
            && user.DeliveryType != DeliveryType;
		}
    }
}
