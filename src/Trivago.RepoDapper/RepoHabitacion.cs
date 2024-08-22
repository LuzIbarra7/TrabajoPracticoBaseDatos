using System.Data;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.RepoDapper;

public class RepoHabitacion : RepoDapper, IRepoHabitacion 
{
    public RepoHabitacion(IDbConnection conexion) : base(conexion)
    {
    }

    public uint Alta(Habitacion habitacion)
    {
        string storedProcedure = "insert_habitacion";
        var IdInsertado = _conexion.QuerySingle<uint>(storedProcedure, habitacion);
        return IdInsertado;
    }

    public Habitacion? Detalle(uint id)
    {
        string sql = "Select * from Habitacion where idHabitacion = @Id LIMIT 1";
        var resultado = _conexion.QuerySingleOrDefault<Habitacion>(sql, new { Id = id});
        return resultado;
    }

    public List<Habitacion> Listar()
    {
        string sql = "Select * from Habitacion";
        var resultado = _conexion.Query<Habitacion>(sql).ToList();
        return resultado;
    }
}

