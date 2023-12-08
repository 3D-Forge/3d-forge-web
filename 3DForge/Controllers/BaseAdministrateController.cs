using Backend3DForge.Interfaces;
using Backend3DForge.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend3DForge.Controllers
{
    public abstract class BaseAdministrateController<Table, TableKey, Request, Response> : ControllerBase
        where Table : class, ITableKey<TableKey>
        where Request : class
        where Response : class
    {

        protected DbApp db;
        public BaseAdministrateController(DbApp db)
        {
            this.db = db;
        }

        protected virtual IQueryable<Table> LoadDate(DbSet<Table> table) => table;
        protected virtual void BeforePost(DbSet<Table> table, ref Table record) { }

        protected abstract Response ConvertToResponse(Table entity);
        protected abstract Table ConvertToTable(Request request);
        protected abstract void UpdateTable(Table entity, Request request);

        private IEnumerable<Response> ConvertToResponse(IEnumerable<Table> entites) => entites.Select(ConvertToResponse);

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(new BaseResponse.SuccessResponse(ConvertToResponse(await LoadDate(db.Set<Table>()).ToListAsync())));
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById([FromRoute] TableKey id)
        {
            var result = await LoadDate(db.Set<Table>()).SingleOrDefaultAsync(x => x.Id.Equals(id));
            if (result == null)
                return NotFound(new BaseResponse.ErrorResponse($"{id} not found"));
            return Ok(new BaseResponse.SuccessResponse(ConvertToResponse(result)));
        }

        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody] Request request)
        {
            var table = ConvertToTable(request);

            try
            {
                BeforePost(db.Set<Table>(), ref table);
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse.ErrorResponse(ex.Message));
            }

            var newEntity = (await db.Set<Table>().AddAsync(table)).Entity;

            await db.SaveChangesAsync();

            return Ok(new BaseResponse.SuccessResponse(ConvertToResponse(newEntity)));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Request request, [FromRoute] TableKey id)
        {
            var record = await LoadDate(db.Set<Table>()).SingleOrDefaultAsync(p => p.Id.Equals(id));

            if (record == null)
                return NotFound(new BaseResponse.ErrorResponse($"{id} not found"));

            UpdateTable(record, request);

            await db.SaveChangesAsync();

            return Ok(new BaseResponse.SuccessResponse(ConvertToResponse(record)));
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete([FromRoute] TableKey id)
        {
            var record = await LoadDate(db.Set<Table>()).SingleOrDefaultAsync(p => p.Id.Equals(id));

            if (record == null)
                return NotFound(new BaseResponse.ErrorResponse($"{id} not found"));

            db.Remove(record);

            await db.SaveChangesAsync();

            return Ok(new BaseResponse.SuccessResponse(ConvertToResponse(record)));
        }
    }
}
