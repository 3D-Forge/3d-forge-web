using Backend3DForge.Models;
using Backend3DForge.Requests;
using Backend3DForge.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend3DForge.Controllers
{
    [Route("api/catalog/{modelId}")]
    [ApiController]
    public class CatalogModelFeedbackController : BaseController
    {
        public CatalogModelFeedbackController(DbApp db) : base(db)
        {
        }

        [HttpGet("feedback")]
        public async Task<IActionResult> GetFeedback(int modelId, [FromQuery] PageRequest page)
        {
            if (!await DB.CatalogModels.AnyAsync(p => p.Id == modelId))
            {
                return NotFound(new BaseResponse.ErrorResponse($"Model '{modelId}' not found"));
            }

            IQueryable<CatalogModelFeedback> feedbacks = DB.CatalogModelFeedbacks.Where(p => p.CatalogModelId == modelId)
                .Include(x => x.Order)
                .Include(p => p.User);

            int totalItemsCount = await feedbacks.CountAsync();
            int totalPagesCount = (int)Math.Ceiling((double)totalItemsCount / page.PageSize);
            feedbacks = feedbacks.Skip((page.Page - 1) * page.PageSize)
            .Take(page.PageSize);

            var result = await feedbacks.Select(p => new CatalogModelFeedbackResponse.View(p)).ToListAsync();

            var response = new PageResponse<CatalogModelFeedbackResponse.View>(
                result,
                page.Page,
                page.PageSize,
                totalItemsCount
                );
            return Ok(response);
        }

        [HttpPost("feedback")]
        public async Task<IActionResult> PostFeedback(int modelId, [FromBody] CatalogModelFeedbackRequest feedback)
        {
            if (!await DB.CatalogModels.AnyAsync(p => p.Id == modelId))
            {
                return NotFound(new BaseResponse.ErrorResponse($"Model '{modelId}' not found"));
            }

            var orderedModel = await DB.OrderedModels
                .Include(p => p.Order).ThenInclude(p => p.User)
                .Include(p => p.CatalogModel)
                .SingleOrDefaultAsync(p => p.Id == feedback.OrderedModelId);

            if (orderedModel is null)
            {
                return NotFound(new BaseResponse.ErrorResponse($"Ordered model '{feedback.OrderedModelId}' not found"));
            }

            if (orderedModel.Order?.UserId != AuthorizedUserId)
            {
                return NotFound(new BaseResponse.ErrorResponse($"Ordered model '{feedback.OrderedModelId}' not found"));
            }

            var model = (await DB.CatalogModelFeedbacks.AddAsync(new CatalogModelFeedback
            {
                CatalogModelId = modelId,
                UserId = AuthorizedUserId,
                OrderId = feedback.OrderedModelId,
                Text = feedback.Text,
                Cons = feedback.Cons,
                Pros = feedback.Pros,
                Rate = feedback.Rate,
                CreatedAt = DateTime.UtcNow
            })).Entity;

            await DB.SaveChangesAsync();

            if(orderedModel.CatalogModel is not null)
            {
                orderedModel.CatalogModel.Rating = (float)await DB.CatalogModelFeedbacks.Where(p => p.CatalogModelId == modelId).AverageAsync(p => p.Rate);
                await DB.SaveChangesAsync();
            }

            var result = await DB.CatalogModelFeedbacks
                .Include(x => x.Order)
                .Include(o => o.User)
                .SingleAsync(p => p.Id == model.Id);

            return Ok(new CatalogModelFeedbackResponse(result));
        }

        [Authorize]
        [HttpDelete("feedback/{feedbackId}")]
        public async Task<IActionResult> DeleteFeedbackAsync(int modelId, int feedbackId)
        {
            if (!await DB.CatalogModels.AnyAsync(p => p.Id == modelId))
            {
                return NotFound(new BaseResponse.ErrorResponse($"Model '{modelId}' not found"));
            }

            var feedback = await DB.CatalogModelFeedbacks.Where(p => p.Id == feedbackId)
                .Include(x => x.Order)
                .Include(p => p.User)
                .Include(p => p.CatalogModel)
                .SingleOrDefaultAsync();

            if (feedback is null)
            {
                return NotFound(new BaseResponse.ErrorResponse($"Feedback '{feedbackId}' not found"));
            }

            if (feedback.UserId != AuthorizedUserId)
            {
                return Forbid();
            }

            DB.CatalogModelFeedbacks.Remove(feedback);
            await DB.SaveChangesAsync();

            feedback.CatalogModel.Rating = (float)await DB.CatalogModelFeedbacks.Where(p => p.CatalogModelId == modelId).AverageAsync(p => p.Rate);
            await DB.SaveChangesAsync();
           
            return Ok(new CatalogModelFeedbackResponse(feedback));
        }
    }
}
