using System.Data;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.RepoDapper;

public class RepoTipoHabitacion : RepoDapper, IRepoTipoHabitacion
{
    public RepoTipoHabitacion(IDbConnection conexion) : base(conexion)
    {
    }

    public uint Alta(TipoHabitacion tipoHabitacion)
    {
        string storedProcedure = "insert_tipo_habitacion";
        var IdInsertado = _conexion.QuerySingle<uint>(storedProcedure, tipoHabitacion);
        return IdInsertado;
    }

    public TipoHabitacion? Detalle(uint id)
    {
        string sql = "Select * from TipoHabitacion where idTipoHabitacion = @Id LIMIT 1";
        var resultado = _conexion.QuerySingleOrDefault<TipoHabitacion>(sql, new { Id = id});
        return resultado;
    }

    public List<TipoHabitacion> Listar()
    {
        string sql = "Select * from TipoHabitacion";
        var resultado = _conexion.Query<TipoHabitacion>(sql).ToList();
        return resultado;
    }
}