using System.Data;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.RepoDapper;

public class RepoHotel : RepoDapper, IRepoHotel
{
    public RepoHotel(IDbConnection conexion) : base(conexion)
    {
    }

    public List<Hotel> InformarHoteles(int idHotel)
    {
        string sql = "Select * from Hotel where idHotel = @Id";
        var resultado = _conexion.Query<Hotel>(sql, new { Id = idHotel}).ToList();
        return resultado;
    }
    public uint Alta(Hotel hotel)
    {
        string storedProcedure = "insert_hotel";
        var IdInsertado = _conexion.QuerySingle<uint>(storedProcedure, hotel);
        return IdInsertado;
    }

    public Hotel? Detalle(uint id)
    {
        string sql = "Select * from Hotel where idHotel = @Id LIMIT 1";
        var resultado = _conexion.QuerySingleOrDefault<Hotel>(sql, new { Id = id});
        return resultado;
    }

    public List<Hotel> Listar()
    {
        string sql = "Select * from Hotel";
        var resultado = _conexion.Query<Hotel>(sql).ToList();
        return resultado;
    }
}