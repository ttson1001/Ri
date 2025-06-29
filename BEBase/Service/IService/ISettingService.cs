namespace BEBase.Service.IService
{
    public interface ISettingService
    {
        Task<string?> GetValueAsync(string key);
        Task SetValueAsync(string key, string value);
    }
}
