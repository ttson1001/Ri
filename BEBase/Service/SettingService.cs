using BEBase.Database;
using BEBase.Entity;
using BEBase.Repository;
using BEBase.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace BEBase.Services.Implementations
{
    public class SettingService : ISettingService
    {
        private readonly IRepo<Setting> _repo;

        public SettingService(IRepo<Setting> repo)
        {
            _repo = repo;
        }

        public async Task<string?> GetValueAsync(string key)
        {
            var setting = await _repo.Get().FirstOrDefaultAsync(s => s.Key == key);
            return setting?.Value;
        }

        public async Task SetValueAsync(string key, string value)
        {
            var setting = await _repo.Get().FirstOrDefaultAsync(s => s.Key == key);
            if (setting != null)
            {
                setting.Value = value;
            }
            else
            {
               await _repo.AddAsync(new Setting { Key = key, Value = value });
            }

            await _repo.SaveChangesAsync();
        }
    }
}
