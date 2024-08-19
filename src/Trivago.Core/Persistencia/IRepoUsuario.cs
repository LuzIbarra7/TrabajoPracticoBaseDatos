using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trivago.Core.Ubicacion;

namespace Trivago.Core.Persistencia
{
    public interface IRepoUsuario : IAlta<Usuario>
    {
        Usuario? UsuarioPorPass(string email, string pass);
    }
}