using Backend3DForge.Enums;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Requests
{
    public class ListOrdersRequest : PageRequest
    {
        [FromQuery(Name = "quary")]
        public string Query { get; set; }
        [FromQuery (Name = "status")]
        public OrderStatusType? OrderStatus { get; set; }
        [FromQuery(Name = "sort_parameter")]
        [RegularExpression("^(createdAt|status)$")]
        public string SortParameter { get; set; } = "createdAt";
        [FromQuery(Name = "sort_direction")]
        [RegularExpression("^(asc|desc)$")]
        public string SortDirection { get; set; } = "asc";
    }
}
