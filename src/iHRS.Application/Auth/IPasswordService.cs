namespace iHRS.Application.Auth
{
    public interface IPasswordService
    {
        bool IsValid(string hash, string password);
        string Hash(string password);
    }
}