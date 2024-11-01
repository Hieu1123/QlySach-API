using Microsoft.EntityFrameworkCore;
using QlySach_API.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QlySach_API.Model.Entity
{
    public class SachDanhMuc
    {
        public int sachId { get; set; }
        public int danhMucId { get; set; }

        [ForeignKey("sachId")]
        [JsonIgnore]
        public virtual Sach? Sach { get; set; }
        [ForeignKey("danhMucId")]
        [JsonIgnore]
        public virtual DanhMuc? DanhMuc { get; set; }
    }
    public class SachDanhMucModel
    {
        private readonly AppDbContext appDbcontext;
        public SachDanhMucModel(AppDbContext appDbcontext)
        {
            this.appDbcontext = appDbcontext;
        }
        public async Task<IEnumerable<SachDanhMuc>> getAllSachDanhMuc()
        {
            return await appDbcontext.SachDanhMucs
                .Include(sd => sd.Sach)
                .Include(sd => sd.DanhMuc)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<SachDanhMuc> addSachDanhMuc(SachDanhMuc newSachDanhMuc)
        {
            if (newSachDanhMuc == null)
            {
                throw new ArgumentException(nameof(newSachDanhMuc));
            }

            var existingRelation = await appDbcontext.SachDanhMucs.FirstOrDefaultAsync(sdm => sdm.sachId == newSachDanhMuc.sachId && sdm.danhMucId == newSachDanhMuc.danhMucId);


            if (existingRelation != null)
            {
                throw new ArgumentException(nameof(existingRelation));
            }

            appDbcontext.SachDanhMucs.Add(newSachDanhMuc);
            await appDbcontext.SaveChangesAsync();
            return newSachDanhMuc;
        }

        public async Task<SachDanhMuc> updateSachDanhMuc(int sachId, List<int> danhMucIds)
        {
            var existingSachDanhMuc = await appDbcontext.SachDanhMucs
                .Where(sdm => sdm.sachId == sachId)
                .ToListAsync();
            appDbcontext.SachDanhMucs.RemoveRange(existingSachDanhMuc);

            var newSachDanhMucs = danhMucIds.Select(danhMucId => new SachDanhMuc
            {
                sachId = sachId,
                danhMucId = danhMucId
            });

            await appDbcontext.SachDanhMucs.AddRangeAsync(newSachDanhMucs);
            await appDbcontext.SaveChangesAsync();

            return newSachDanhMucs.FirstOrDefault();
        }
        public async Task<bool> deleteSachDanhMuc(int sachId, int danhMucId)
        {
            var existingSachDanhMuc = await appDbcontext.SachDanhMucs
                .FirstOrDefaultAsync(sd => sd.sachId == sachId && sd.danhMucId == danhMucId);

            if (existingSachDanhMuc == null)
            {
                throw new KeyNotFoundException();
            }

            appDbcontext.SachDanhMucs.Remove(existingSachDanhMuc);
            await appDbcontext.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<SachDanhMuc>> getSachDanhMucBySachId(int SachId)
        {
            var sachDanhMucList = await appDbcontext.SachDanhMucs
                    .Where(sdm => sdm.sachId == SachId)
                    .ToListAsync();
            if (sachDanhMucList == null || !sachDanhMucList.Any())
            {
                throw new KeyNotFoundException();
            }
            return sachDanhMucList;
        }
    }
}
