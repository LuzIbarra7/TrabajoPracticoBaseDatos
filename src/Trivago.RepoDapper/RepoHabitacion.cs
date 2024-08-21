using System.Data;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.RepoDapper;

public class RepoHabitacion : RepoDapper, IRepoHabitacion 
{
    public RepoHabitacion(IDbConnection conexion) : base(conexion)
    {
    }

    public void Alta(Habitacion elemento)
    {
        throw new NotImplementedException();
    }

    public Habitacion? Detalle(uint id)
    {
        throw new NotImplementedException();
    }

    public List<Habitacion> Listar()
    {
        throw new NotImplementedException();
    }
}

