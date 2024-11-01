using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QlySach_API.Model.Entity;

namespace QlySach_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SachDanhMucController : ControllerBase
    {
        private readonly SachDanhMucModel sachDanhMucModel;
        public SachDanhMucController(SachDanhMucModel sachDanhMucModel)
        {
            this.sachDanhMucModel = sachDanhMucModel;
        }
        [Authorize(Policy = "UserOnly , AdminOnly")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SachDanhMuc>>> getAllSachDanhMuc()
        {
            var get = await sachDanhMucModel.getAllSachDanhMuc();
            return Ok(get);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<IActionResult> AddSachDanhMuc([FromBody] SachDanhMuc sachDanhMuc)
        {
            if (sachDanhMuc == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var add = await sachDanhMucModel.addSachDanhMuc(sachDanhMuc);
            return CreatedAtAction(nameof(GetSachDanhMucbySachId), new { add.sachId, add.danhMucId }, add);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{sachId}")]
        public async Task<IActionResult> UpdateSachDanhMuc(int sachId, [FromBody] List<int> danhMucIds)
        {
            if (danhMucIds == null || !danhMucIds.Any())
            {
                return BadRequest();
            }

            var update = await sachDanhMucModel.updateSachDanhMuc(sachId, danhMucIds);
            if (update == null)
            {
                return NotFound();
            }


            return Ok(update);
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{sachId}/{danhMucId}")]
        public async Task<IActionResult> deleteSachDanhMuc(int sachId, int danhMucId)
        {
            var delete = await sachDanhMucModel.deleteSachDanhMuc(sachId, danhMucId);
            if (delete)
            {
                return NoContent();

            }
            return NotFound();
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("{sachId}")]
        public async Task<ActionResult<IEnumerable<SachDanhMuc>>> GetSachDanhMucbySachId(int sachId)
        {
            var sachDanhMucList = await sachDanhMucModel.getSachDanhMucBySachId(sachId);

            if (sachDanhMucList == null || !sachDanhMucList.Any())
            {
                return NotFound();
            }

            return Ok(sachDanhMucList);
        }
    }
}

