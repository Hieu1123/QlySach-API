using Microsoft.EntityFrameworkCore;
using QlySach_API.Data;
using System.ComponentModel.DataAnnotations;

namespace QlySach_API.Model.Entity
{
    public class Sach
    {
        [Key]
        public int Id { get; set; }
        public required string tenSach { get; set; }
        public required int soLuong { get; set; }
        public required int giaTien { get; set; }
        public virtual ICollection<SachDanhMuc> sachDanhMucs { get; set; } = new List<SachDanhMuc>();
    }
    public class SachModel
    {
        private readonly AppDbContext appDbcontext;
        public SachModel(AppDbContext appDbcontext)
        {
            this.appDbcontext = appDbcontext;
        }

        public async Task<Sach> addSach(Sach sach, List<int> danhMucIds)
        {
            if (sach == null)
            {
                throw new ArgumentNullException(nameof(sach));
            }
            appDbcontext.ListSach.Add(sach);
            await appDbcontext.SaveChangesAsync();

            foreach (var danhMucId in danhMucIds)
            {
                var sachDanhMuc = new SachDanhMuc
                {
                    sachId = sach.Id,
                    danhMucId = danhMucId
                };
                appDbcontext.SachDanhMucs.Add(sachDanhMuc);
            }


            await appDbcontext.SaveChangesAsync();
            return await appDbcontext.ListSach
                .Include(s => s.sachDanhMucs)
                .ThenInclude(sdm => sdm.DanhMuc)
                .FirstOrDefaultAsync(s => s.Id == sach.Id);
        }


        public async Task<IEnumerable<Sach>> getAllSachAndDanhMuc()
        {
            return await appDbcontext.ListSach
                .Include(sdm => sdm.sachDanhMucs)
                .ThenInclude(dm => dm.DanhMuc)
                .ToListAsync();
        }

        public async Task<Sach> updateSach(int Id, Sach sach, List<int> newDanhMucIds)
        {
            if (Id != sach.Id)
            {
                throw new ArgumentException();
            }

            var existingSach = await appDbcontext.ListSach
                .Include(sdm => sdm.sachDanhMucs)
                .ThenInclude(dm => dm.DanhMuc)
                .FirstOrDefaultAsync(s => s.Id == Id);

            if (existingSach == null)
            {
                throw new KeyNotFoundException();
            }

            existingSach.tenSach = sach.tenSach;
            existingSach.soLuong = sach.soLuong;
            existingSach.giaTien = sach.giaTien;

            var currentDanhMucIds = existingSach.sachDanhMucs.Select(sdm => sdm.danhMucId).ToList();

            var toAdd = newDanhMucIds.Except(currentDanhMucIds).ToList();
            var toRemove = currentDanhMucIds.Except(newDanhMucIds).ToList();

            foreach (var danhMucId in toAdd)
            {
                var sachDanhMuc = new SachDanhMuc
                {
                    sachId = Id,
                    danhMucId = danhMucId,
                };

                existingSach.sachDanhMucs.Add(sachDanhMuc);
            }

            foreach (var danhMucId in toRemove)
            {
                var sachDanhMucToRemove = existingSach.sachDanhMucs.FirstOrDefault(sdm => sdm.danhMucId == danhMucId);
                if (sachDanhMucToRemove != null)
                {
                    existingSach.sachDanhMucs.Remove(sachDanhMucToRemove);
                }
            }

            await appDbcontext.SaveChangesAsync();

            return existingSach;
        }



        public async Task<Sach> deleteSach(int Id)
        {
            var sach = await appDbcontext.ListSach.FirstOrDefaultAsync(s => s.Id == Id);
            if (sach == null)
            {
                return null;
            }
            var sachDanhMuc = appDbcontext.SachDanhMucs.Where(sdm => sdm.sachId == Id);
            appDbcontext.SachDanhMucs.RemoveRange(sachDanhMuc);

            appDbcontext.ListSach.Remove(sach);
            await appDbcontext.SaveChangesAsync();
            return sach;
        }

        public async Task<Sach> getSachById(int id)
        {
            return await appDbcontext.ListSach
                .Include(sdm => sdm.sachDanhMucs)
                .ThenInclude(dm => dm.DanhMuc)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

    }

}