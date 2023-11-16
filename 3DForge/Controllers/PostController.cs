using Backend3DForge.Responses;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Backend3DForge.Models.PostImpl;

namespace Backend3DForge.Controllers
{
	[Route("api/post")]
	[ApiController]
	public class PostController : BaseController
	{

		private PostCountries _postCountries;

		public PostController(DbApp db) : base(db)
		{
			var fileContent = System.IO.File.ReadAllText(Path.Combine("src", "postLocations.json"));
			_postCountries = JsonConvert.DeserializeObject<PostCountries>(fileContent);

			if (_postCountries is null)
			{
				throw new InvalidDataException();
			}
		}

		[HttpGet("counries")]
		public async Task<IActionResult> GetCounties()
		{
			var countries = _postCountries.Countries
				.Select(x => x.Counrty);
			return Ok(new BaseResponse.SuccessResponse("Post Countries", countries));
		}

		[HttpGet("regions")]
		public async Task<IActionResult> GetRegions([FromQuery] string country)
		{
			var regions = _postCountries.Countries
				.Where(x => x.Counrty == country)
				.SelectMany(x => x.Regions
					.Select(x=>x.Region));
			return Ok(new BaseResponse.SuccessResponse("Post Regions", regions));
		}

		[HttpGet("cities")]
		public async Task<IActionResult> GetCities([FromQuery] string country, [FromQuery] string region)
		{
			var cities = _postCountries.Countries
				.Where(x => x.Counrty == country)
				.SelectMany(x => x.Regions
					.Where(x => x.Region == region)
					.SelectMany(x => x.Cities
						.Select(x => x.City)));
			return Ok(new BaseResponse.SuccessResponse("Post Cities", cities));
		}

		[HttpGet("departments")]
		public async Task<IActionResult> GetDepartments([FromQuery] string country, [FromQuery] string region, [FromQuery] string city)
		{
			var departments = _postCountries.Countries
				.Where(x => x.Counrty == country)
				.SelectMany(x => x.Regions
					.Where(x => x.Region == region)
					.SelectMany(x => x.Cities
						.SelectMany(x => x.Departments)));
			return Ok(new BaseResponse.SuccessResponse("Post Departments", departments));
		}

		[HttpGet("postMachines")]
		public async Task<IActionResult> GetPostMachines([FromQuery] string country, [FromQuery] string region, [FromQuery] string city)
		{
			var postMachines = _postCountries.Countries
				.Where(x => x.Counrty == country)
				.SelectMany(x => x.Regions
					.Where(x => x.Region == region)
					.SelectMany(x => x.Cities
						.SelectMany(x => x.PostMachines)));
			return Ok(new BaseResponse.SuccessResponse("Post Post Machines", postMachines));
		}
	}
}
