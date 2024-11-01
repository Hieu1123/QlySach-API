using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using QlySach_API.Data;

namespace QlySach_API.Model.Entity
{
    public class DanhMuc
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string tenDanhMuc { get; set; }
        public int? parentDanhMucId { get; set; }
        [JsonIgnore]
        public virtual DanhMuc? parentDanhMuc { get; set; }
        public virtual ICollection<DanhMuc>? listDanhMucCon { get; set; }
        public virtual ICollection<SachDanhMuc> SachDanhMucs { get; set; } = new List<SachDanhMuc>();
    }
    public class DanhMucModel
    {
        private readonly AppDbContext appDbContext;

        public DanhMucModel(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<DanhMuc> addDanhMuc(DanhMuc danhMuc)
        {
            if (danhMuc == null)
            {
                throw new ArgumentNullException(nameof(danhMuc));
            }
            if (danhMuc.parentDanhMucId.HasValue)
            {
                var parentExists = await appDbContext.DanhMuc.AnyAsync(dm => dm.Id == danhMuc.parentDanhMucId);
                if (!parentExists)
                {
                    throw new ArgumentException();
                }
            }
            appDbContext.DanhMuc.Add(danhMuc);
            await appDbContext.SaveChangesAsync();
            return danhMuc;
        }

        public async Task<IEnumerable<DanhMuc>> getAllDanhMuc()
        {
            return await appDbContext.DanhMuc.Include(dm => dm.listDanhMucCon).ToArrayAsync();
        }

        public async Task<DanhMuc> updateDanhMuc(int Id, DanhMuc danhMuc)
        {
            if (Id != danhMuc.Id)
            {
                throw new ArgumentException();
            }

            var danhMucExists = await appDbContext.DanhMuc
                .Include(dm => dm.listDanhMucCon)
                .FirstOrDefaultAsync(dm => dm.Id == Id);

            if (danhMucExists == null)
            {
                return null;
            }
            danhMucExists.tenDanhMuc = danhMuc.tenDanhMuc;

            if (danhMuc.parentDanhMucId != danhMucExists.parentDanhMucId)
            {
                if (danhMuc.parentDanhMucId.HasValue)
                {
                    var parentExists = await appDbContext.DanhMuc.AnyAsync(dm => dm.Id == danhMuc.parentDanhMucId);
                    if (!parentExists)
                    {
                        throw new ArgumentException("Danh mục cha không tồn tại");
                    }
                }
                danhMucExists.parentDanhMucId = danhMuc.parentDanhMucId;

                if (danhMuc.parentDanhMucId != null)
                {
                    danhMucExists.listDanhMucCon = null;
                }
            }
            if (danhMucExists.parentDanhMucId == null && danhMuc.listDanhMucCon != null)
            {
                var danhMucConExists = danhMucExists.listDanhMucCon.ToList();
                foreach (var subExists in danhMucConExists)
                {
                    if (!danhMuc.listDanhMucCon.Any(sub => sub.Id == subExists.Id))
                    {
                        appDbContext.DanhMuc.Remove(subExists);
                    }
                }
                foreach (var newSub in danhMuc.listDanhMucCon)
                {
                    var existingSub = danhMucExists.listDanhMucCon.FirstOrDefault(sub => sub.Id == newSub.Id);
                    if (existingSub != null)
                    {
                        existingSub.tenDanhMuc = newSub.tenDanhMuc;
                        existingSub.parentDanhMucId = danhMuc.Id;
                    }
                    else
                    {
                        danhMucExists.listDanhMucCon.Add(new DanhMuc
                        {
                            tenDanhMuc = newSub.tenDanhMuc,
                            parentDanhMucId = danhMuc.Id,
                        });
                    }
                }
            }

            appDbContext.Entry(danhMucExists).State = EntityState.Modified;

            try
            {
                await appDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!existsDanhMuc(Id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return danhMucExists;
        }


        private bool existsDanhMuc(int Id)
        {
            return appDbContext.DanhMuc.All(dm => dm.Id == Id);
        }
        public async Task<DanhMuc> deleteDanhMuc(int Id)
        {
            var danhmuc = await appDbContext.DanhMuc.FirstOrDefaultAsync(dm => dm.Id == Id);
            if (danhmuc == null)
            {
                return null;
            }

            appDbContext.DanhMuc.Remove(danhmuc);
            await appDbContext.SaveChangesAsync();
            return danhmuc;
        }
        public async Task<DanhMuc> getDanhMucById(int Id)
        {
            return await appDbContext.DanhMuc.Include(dm => dm.listDanhMucCon).FirstOrDefaultAsync(dm => dm.Id == Id);
        }
    }
}
