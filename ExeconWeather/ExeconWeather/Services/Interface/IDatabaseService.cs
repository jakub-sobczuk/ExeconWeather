using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExeconWeather.Services.Interface
{
    public interface IDatabaseService<T> where T : class, new()
    {
        Task CreateTable();
        Task<List<T>> Get();
        Task<int> Insert(T entity);
    }
}
