using AdminWebsite.Models.Repository;
using AdminWebsite.Data;
namespace AdminWebsite.Models.DataManager;

public class LoginManager : IDataRepository<Login, string>
{
    private readonly McbaContext _context;

    public LoginManager(McbaContext context)
    {
        _context = context;
    }

    public Login Get(string id)
    {
        return _context.Login.Find(id);
    }

    public IEnumerable<Login> GetAll()
    {
        return _context.Login.ToList();
    }
    
    public string Add(Login login)
    {
        _context.Login.Add(login);
        _context.SaveChanges();

        return login.LoginID;
    }

    public string Delete(string id)
    {
        _context.Login.Remove(_context.Login.Find(id));
        _context.SaveChanges();

        return id;
    }

    public string Update(string id, Login login)
    {
        _context.Update(login);
        _context.SaveChanges();
            
        return id;
    }

}
