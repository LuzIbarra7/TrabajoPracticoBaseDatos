using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.RepoDapper;

public class RepoUsuario : IRepoUsuario
{
    public void Alta(Usuario elemento)
    {
        throw new NotImplementedException();
    }

    public Usuario? Detalle(uint id)
    {
        throw new NotImplementedException();
    }

    public List<Usuario> Listar()
    {
        throw new NotImplementedException();
    }

    public Usuario? UsuarioPorPass(string email, string pass)
    {
        throw new NotImplementedException();
    }
}