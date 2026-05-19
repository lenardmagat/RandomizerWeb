
using PracticeWeb.DataBase;
public class DataRepository<T> where T : class
{
    private readonly DbManager db = null!;

    public async Task Save(T entity)
    {
        db.Set<T>().Add(entity);
        db.SaveChanges();
    }

    public List<T> GetAll() => db.Set<T>().ToList();
}