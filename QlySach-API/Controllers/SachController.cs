using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QlySach_API.Data;
using QlySach_API.Model.Entity;

namespace QlySach_API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class SachController : ControllerBase
    {
        private readonly SachModel sachModel;
        private readonly AppDbContext appDbContext;
        public SachController(SachModel sachModel, AppDbContext appDbContext)
        {
            this.sachModel = sachModel;
            this.appDbContext = appDbContext;
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<ActionResult<Sach>> AddSach([FromBody] Sach sach, [FromQuery] List<int> danhMucIds)
        {
            if (sach == null)
            {
                return BadRequest();
            }
            var add = await sachModel.addSach(sach, danhMucIds);
            return CreatedAtAction(nameof(getSachById), new { id = add.Id }, add);
        }

        [Authorize(Policy = "UserOnly , AdminOnly")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sach>>> getAllSach()
        {
            var get = await sachModel.getAllSachAndDanhMuc();
            return Ok(get);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{Id}")]
        public async Task<IActionResult> updateSach(int Id, [FromBody] Sach updatedSach, [FromQuery] List<int> danhMucIds)
        {
            if (updatedSach == null)
            {
                return BadRequest();
            }
            var update = await sachModel.updateSach(Id, updatedSach, danhMucIds);
            if (update == null)
            {
                return NotFound();
            }

            return Ok(update);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> deleteSach(int Id)
        {
            var del = await sachModel.deleteSach(Id);
            if (del == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("{Id}")]
        public async Task<ActionResult<IEnumerable<Sach>>> getSachById(int Id)
        {
            var getId = await sachModel.getSachById(Id);

            if (getId == null)
            {
                return NotFound();
            }

            return Ok(getId);
        }
        [Authorize(Policy = "UserOnly , AdminOnly")]
        [HttpGet("/Page/Sach")]
        public async Task<IActionResult> pageSachDanhMuc(int page = 1, int pageSize = 5)
        {
            var totalItems = await appDbContext.ListSach.CountAsync();

            var allSachDanhMuc = await appDbContext.ListSach
                .Include(s => s.sachDanhMucs)
                .ThenInclude(sdm => sdm.DanhMuc)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                Items = allSachDanhMuc.Select(s => new
                {
                    s.Id,
                    s.tenSach,
                    s.soLuong,
                    s.giaTien,
                    sachDanhMucs = s.sachDanhMucs.Select(sdm => new
                    {
                        sdm.sachId,
                        sdm.danhMucId
                    })
                })
            };
            return Ok(result);
        }

    }
}
