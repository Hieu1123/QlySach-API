using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Models;
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
        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "1");
        }
        private async Task<ActionResult<T>> CheckUserfunc<T>(Functionality functionality, Func<Task<ActionResult<T>>> action)
        {
            int userId = GetCurrentUserId();
            var hasFunctionality = await roleService.UserHasFunctionalityAsync(userId, functionality);

            // Log the result
            if (!hasFunctionality)
            {
                // Log the forbidden access attempt
                Console.WriteLine($"User ID {userId} bi cam truy cap chuc nang: {functionality}");
                return Forbid();
            }

            return await action();
        }


        [HttpPost]
        public async Task<ActionResult<DanhMuc>> addDanhMuc(DanhMuc danhmuc)
        {
            return await CheckUserfunc<DanhMuc>(Functionality.Add, async () =>
            {
                var add = await danhMucModel.addDanhMuc(danhmuc);
                return Ok(add);
            });
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DanhMuc>>> getAllDanhMuc()
        {
            return await CheckUserfunc<IEnumerable<DanhMuc>>(Functionality.View, async () =>
            {
                var get = await danhMucModel.getAllDanhMuc();
                return Ok(get);
            });
        }

        [HttpPut("{Id}")]
        public async Task<ActionResult<DanhMuc>> updateDanhMuc(int Id, DanhMuc danhmuc)
        {
            return await CheckUserfunc<DanhMuc>(Functionality.Edit, async () =>
            {
                if (Id != danhmuc.Id)
                {
                    return BadRequest();
                }
                var update = await danhMucModel.updateDanhMuc(Id, danhmuc);
                if (update == null)
                {
                    return NotFound();
                }
                return Ok(update);
            });
        }
        [HttpDelete("{Id}")]
        public async Task<ActionResult<DanhMuc>> deleteDanhMuc(int Id)
        {
            return await CheckUserfunc<DanhMuc>(Functionality.Delete, async () =>
            {
                var del = await danhMucModel.deleteDanhMuc(Id);
                if (del == null)
                {
                    return NotFound();
                }

                return NoContent();
            });
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<DanhMuc>> getDanhMucById(int Id)
        {
            return await CheckUserfunc<DanhMuc>(Functionality.getById, async () =>
            {
                var getId = await danhMucModel.getDanhMucById(Id);
                if (getId == null)
                {
                    return NotFound();
                }
                return Ok(getId);
            });
        }
        [HttpGet("Page")]
        public async Task<ActionResult<IEnumerable<DanhMuc>>> pageDanhMuc(int page = 1, int pageSize = 5)
        {
            return await CheckUserfunc<IEnumerable<DanhMuc>>(Functionality.ViewPage, async () =>
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
                    TotalIteam = allDanhMuc.Length,
                    TotalPage = (int)Math.Ceiling((double)allDanhMuc.Length / pageSize),
                    Iteam = pageData
                };
                return Ok(result);
            });
        }
    }
}
