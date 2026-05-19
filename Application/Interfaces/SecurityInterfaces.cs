using PracticeWeb.ErrorHandling;
namespace PracticeWeb.Interface;
public interface IHasher
{
    public string HashPassword(string password);
    public bool VerifyPassword(string password, string hashPassword);
    public string CreateToken(int UserId);
    public string CreateHashids(int GroupId);
    public Result<int> DecodeHashids(string hashh);
}