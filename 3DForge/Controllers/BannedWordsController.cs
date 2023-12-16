using Backend3DForge.Attributes;
using Backend3DForge.Models;
using Backend3DForge.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend3DForge.Controllers
{
	[Route("api/abuse")]
	[ApiController]
	public class BannedWordsController : BaseController
	{
		public BannedWordsController(DbApp db) : base(db)
		{
		}

		[CanAdministrateSystem]
		[HttpGet]
		public async Task<IActionResult> GetBannedWords([FromQuery(Name = "q")] string? q = null)
		{
			var words = DB.BannedWords.Where(x => x.BannedWordName.Contains(q ?? ""));

			return Ok(new BaseResponse.SuccessResponse(words));
		}

		[CanAdministrateSystem]
		[HttpPost]
		public async Task<IActionResult> AddBannedWord([FromBody] string word)
		{
			if (await DB.BannedWords.AnyAsync(x => x.BannedWordName == word))
			{
				return BadRequest(new BaseResponse.ErrorResponse("This word is already exists"));
			}

			var bannedWord = new BannedWord()
			{
				BannedWordName = word,
			};

			bannedWord = (await DB.BannedWords.AddAsync(bannedWord)).Entity;
			await DB.SaveChangesAsync();

			return Ok(new BaseResponse.SuccessResponse(bannedWord));
		}

		[CanAdministrateSystem]
		[HttpDelete]
		public async Task<IActionResult> DeleteVannedWord([FromBody] int wordId)
		{
			var bannedWord = await DB.BannedWords.FirstOrDefaultAsync(x => x.Id == wordId);

			if (bannedWord is null)
			{
				return BadRequest(new BaseResponse.ErrorResponse("Selected word does not exists"));
			}

			DB.BannedWords.Remove(bannedWord);
			await DB.SaveChangesAsync();

			return Ok(new BaseResponse.SuccessResponse("Word has been deleted"));
		}
	}
}
