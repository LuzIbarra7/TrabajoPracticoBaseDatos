using System.Data;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.RepoDapper;

public class RepoPais : RepoDapper, IRepoPais
{
    public RepoPais(IDbConnection conexion) : base(conexion)
    {
    }

    public void Alta(Pais elemento)
    {
        throw new NotImplementedException();
    }

    public void Delete(Pais elemento)
    {
        throw new NotImplementedException();
    }

    public Pais? Detalle(uint id)
    {
        throw new NotImplementedException();
    }

    public List<Pais> Listar()
    {
        throw new NotImplementedException();
    }
}
