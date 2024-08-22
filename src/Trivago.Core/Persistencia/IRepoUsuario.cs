using Trivago.Core.Ubicacion;

namespace Trivago.Core.Persistencia;

public interface IRepoUsuario : IAlta<Usuario, uint>, IListado<Usuario>, IDetalle<Usuario, uint>
{
    Usuario? UsuarioPorPass(string email, string pass);
}