using System.Data;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.RepoDapper;

public class RepoHotel : RepoDapper, IRepoHotel
{
    public RepoHotel(IDbConnection conexion) : base(conexion)
    {
    }

    //Altas Hoteles
    private async Task<uint> AltaHotelInternaAsync(Hotel hotel, Func<string, DynamicParameters, Task> ejecutor)
    {
        string storedProcedure = "insert_hotel";

        var parametros = new DynamicParameters();
        parametros.Add("p_idCiudad", hotel.idCiudad);
        parametros.Add("p_Nombre", hotel.Nombre);
        parametros.Add("p_Direccion", hotel.Direccion);
        parametros.Add("p_Telefono", hotel.Telefono);
        parametros.Add("p_URL", hotel.URL);
        parametros.Add("p_idHotel", direction: ParameterDirection.Output);

        await ejecutor(storedProcedure, parametros);

        hotel.idHotel = parametros.Get<uint>("p_idHotel");
        return hotel.idHotel;
    }

    public uint Alta(Hotel hotel)
    {
        return AltaHotelInternaAsync(hotel, (sp, p) =>
        {
            _conexion.Execute(sp, p);
            return Task.CompletedTask;
        }).GetAwaiter().GetResult();
    }
    public async Task<uint> AltaAsync(Hotel hotel)
    {
        return await AltaHotelInternaAsync(hotel, (sp, p) => _conexion.ExecuteAsync(sp, p));
    }

    //Detalle Async
    private async Task<Hotel?> DetalleHotelInternaAsync(uint id, Func<string, object, Task<Hotel?>> ejecutor)
    {
        string sql = "SELECT * FROM Hotel WHERE idHotel = @Id LIMIT 1";
        return await ejecutor(sql, new { Id = id });
    }
    public Hotel? Detalle(uint id)
    {
        return DetalleHotelInternaAsync(id, (sql, param) =>
        {
            var result = _conexion.QuerySingleOrDefault<Hotel>(sql, param);
            return Task.FromResult(result);
        }).GetAwaiter().GetResult();
    }
    public async Task<Hotel?> DetalleAsync(uint id)
    {
        return await DetalleHotelInternaAsync(id, (sql, param) =>
            _conexion.QuerySingleOrDefaultAsync<Hotel>(sql, param));
    }

    //Listar
    private async Task<List<Hotel>> ListarHotelInternaAsync(Func<string, Task<IEnumerable<Hotel>>> ejecutor)
    {
        string sql = "SELECT * FROM Hotel";
        var resultado = await ejecutor(sql);
        return resultado.ToList();
    }

    public List<Hotel> Listar()
    {
        return ListarHotelInternaAsync(sql =>
        {
            var result = _conexion.Query<Hotel>(sql);
            return Task.FromResult(result);
        }).GetAwaiter().GetResult();
    }
    public async Task<List<Hotel>> ListarAsync()
    {
        return await ListarHotelInternaAsync(sql => _conexion.QueryAsync<Hotel>(sql));
    }

    //Informar Hoteles Async
    private async Task<List<Hotel>> InformarHotelInternaAsync(int idHotel, Func<string, object, Task<IEnumerable<Hotel>>> ejecutor)
    {
        string sql = "SELECT * FROM Hotel WHERE idHotel = @Id";
        var resultado = await ejecutor(sql, new { Id = idHotel });
        return resultado.ToList();
    }

    public List<Hotel> InformarHotelesPorIdCiudad(int idHotel)
    {
        return InformarHotelInternaAsync(idHotel, (sql, param) =>
        {
            var result = _conexion.Query<Hotel>(sql, param);
            return Task.FromResult(result);
        }).GetAwaiter().GetResult();
    }
    public async Task<List<Hotel>> InformarHotelesPorIdCiudadAsync(int idHotel)
    {
        return await InformarHotelInternaAsync(idHotel, (sql, param) =>
            _conexion.QueryAsync<Hotel>(sql, param));
    }

}