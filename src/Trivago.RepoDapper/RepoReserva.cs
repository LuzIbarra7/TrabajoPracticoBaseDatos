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
        var IdInsertado = _conexion.QuerySingle<uint>(storedProcedure, reserva);
        return IdInsertado;
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