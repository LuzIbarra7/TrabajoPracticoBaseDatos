using System.Data;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.RepoDapper;

public class RepoReserva : RepoDapper, IRepoReserva
{
    public RepoReserva(IDbConnection conexion) : base(conexion)
    {
    }

    public uint Alta(Reserva reserva)
    {
        string storedProcedure = "insert_reserva";

        var parametros = new DynamicParameters();
        parametros.Add("p_idHabitacion", reserva.habitacion.idHabitacion);
        parametros.Add("p_idMetododePago", reserva.metodoPago.idMetodoPago);
        parametros.Add("p_idUsuario", reserva.idUsuario);
        parametros.Add("p_Entrada", reserva.Entrada);
        parametros.Add("p_Salida", reserva.Salida);
        parametros.Add("p_Telefono", reserva.Telefono);
        parametros.Add("p_idReserva", direction: ParameterDirection.Output);
               
        _conexion.Execute(storedProcedure, parametros);

        reserva.idReserva = parametros.Get<uint>("p_idReserva");
        return reserva.idReserva;
    }

    public Reserva? Detalle(uint id)
    {
        string sql = "Select * from Reserva where idReserva = @Id LIMIT 1";
        var resultado = _conexion.QuerySingleOrDefault<Reserva>(sql, new { Id = id});
        return resultado;
    }

    public List<Reserva> Listar()
    {
        string sql = "Select * from Reserva";
        var resultado = _conexion.Query<Reserva>(sql).ToList();
        return resultado;
    }
}