using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Requests
{
    public class SearchModelRequest : PageRequest
    {

        [FromQuery(Name = "q")]
        public string? Query { get; set; }

        [FromQuery(Name = "categories")]
        public int[]? Categories { get; set; }

        [FromQuery(Name = "keywords")]
        public string[]? Keywords { get; set; }
        [FromQuery(Name = "sort_parameter")]
        [RegularExpression("^(name|price|rating)$")]
        public string SortParameter { get; set; } = "name";
        [FromQuery(Name = "sort_direction")]
        [RegularExpression("^(asc|desc)$")]
        public string SortDirection { get; set; } = "asc";

        [FromQuery(Name = "min_price")]
        public float? MinPrice { get; set; }
        [FromQuery(Name = "max_price")]
        public float? MaxPrice { get; set; }

        [FromQuery(Name = "rating")]
        public int[]? Rating { get; set; }

        [FromQuery(Name = "author")]
        public string? Author { get; set; }
    }
}
