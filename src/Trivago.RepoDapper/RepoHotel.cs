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

        var parametros = new DynamicParameters();
        parametros.Add("p_idCiudad", hotel.idCiudad);
        parametros.Add("p_Nombre", hotel.Nombre);
        parametros.Add("p_Direccion", hotel.Direccion);
        parametros.Add("p_Telefono", hotel.Telefono);
        parametros.Add("p_URL", hotel.URL);
        parametros.Add("p_idHotel",direction: ParameterDirection.Output);
               
        _conexion.Execute(storedProcedure, parametros);

        hotel.idHotel = parametros.Get<uint>("p_idHotel");
        return hotel.idHotel;
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