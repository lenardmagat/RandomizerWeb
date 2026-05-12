using Microsoft.EntityFrameworkCore;
using PracticeWeb.DataBase;
public class DataRepository<T> where T : class
{
    private readonly DbManager _db = null!;

    public async Task Save(T entity)
    {
        _db.Set<T>().Add(entity);
        _db.SaveChanges();
    }

    public List<T> GetAll() => _db.Set<T>().ToList();
}