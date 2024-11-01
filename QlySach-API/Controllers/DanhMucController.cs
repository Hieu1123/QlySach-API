using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QlySach_API.Data;
using QlySach_API.Model.Entity;
using QlySach_API.Service;
using System.Security.Claims;

namespace QlySach_API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    
    public class DanhMucController : ControllerBase
    {
        private readonly DanhMucModel danhMucModel;
        private readonly AppDbContext appDbContext;
        private readonly IRoleService roleService;
        public DanhMucController(DanhMucModel danhMucModel, AppDbContext appDbContext, IRoleService roleService)
        {
            this.danhMucModel = danhMucModel;
            this.appDbContext = appDbContext;
            this.roleService = roleService;
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> addDanhMuc(DanhMuc danhmuc)
        {

            var add = await danhMucModel.addDanhMuc(danhmuc);
            return Ok(add);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DanhMuc>>> getAllDanhMuc()
        {
            var get = await danhMucModel.getAllDanhMuc();
            return Ok(get);
        }
        [HttpPut("{Id}")]
        public async Task<IActionResult> updateDanhMuc(int Id, DanhMuc danhmuc)
        {
            if (Id != danhmuc.Id)
            {
                return BadRequest();
            }
            var edit = await danhMucModel.updateDanhMuc(Id, danhmuc);
            if (edit == null)
            {
                return NotFound();
            }
            return Ok(edit);
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> deleteDanhMuc(int Id)
        {
            var del = await danhMucModel.deleteDanhMuc(Id);
            if (del == null)
            {
                return NotFound();
            }

            return NoContent();
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<DanhMuc>> getDanhMucById(int Id)
        {
            var getId = await danhMucModel.getDanhMucById(Id);

            if (getId == null)
            {
                return NotFound();
            }

            return Ok(getId);
        }
        [HttpGet("Page")]
        public async Task<IActionResult> pageDanhMuc(int page = 1, int pageSize = 5)
        {
            var allDanhMuc = await appDbContext.DanhMuc.ToArrayAsync();
            var pageData = allDanhMuc
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            var result = new
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = allDanhMuc.Length,
                TotalPages = (int)Math.Ceiling((double)allDanhMuc.Length / pageSize),
                Items = pageData
            };
            return Ok(result);
        }
    }
}
