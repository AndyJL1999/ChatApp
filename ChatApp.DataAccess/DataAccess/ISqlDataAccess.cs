namespace ChatApp.DataAccess.DataAccess
{
    public interface ISqlDataAccess
    {
        Task<IEnumerable<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionId = "DefaultConnection");
        Task SaveData<T>(string storedProcedure, T parameters, string connectionId = "DefaultConnection");
    }
}