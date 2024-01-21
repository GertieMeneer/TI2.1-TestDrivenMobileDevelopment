namespace TDMD.Interfaces
{
    public interface IViewModel 
    {
        Task<bool> GetUserIdAsync();
        Task LoadLamps();
    }
}
