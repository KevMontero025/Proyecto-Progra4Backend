

namespace Proyecto.BW.Interfaces.BW
{
    public interface IAutenticacionBW
    {
        Task<string> Login(string email, string password);
    }
}
