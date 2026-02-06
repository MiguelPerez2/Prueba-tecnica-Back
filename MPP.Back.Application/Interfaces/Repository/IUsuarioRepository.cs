using MPP.Back.Domain.Entities;

namespace MPP.Back.Application.Interfaces.Repository
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> GetByCredentialsUserNameAsync(string userName, CancellationToken cancellationToken);
    }
}
