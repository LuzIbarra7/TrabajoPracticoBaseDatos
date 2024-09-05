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

        var parametros = new DynamicParameters();

        parametros.Add("p_idHotel", habitacion.hotel.idHotel);
        parametros.Add("p_idTipo", habitacion.tipoHabitacion.idTipo);
        parametros.Add("p_PrecioPorNoche", habitacion.PrecioPorNoche);
        parametros.Add("p_idHabitacion",direction: ParameterDirection.Output);
               
        _conexion.Execute(storedProcedure, parametros);

        habitacion.idHabitacion = parametros.Get<uint>("p_idHabitacion");
        return habitacion.idHabitacion;
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

