using System.Data;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.RepoDapper;

public class RepoHabitacion : RepoDapper, IRepoHabitacion
{
    public RepoHabitacion(IDbConnection conexion) : base(conexion)
    {
    }

    //Altas Habitacion
    private async Task<uint> AltaHabitacionInternaAsync(Habitacion habitacion, Func<string, DynamicParameters, Task> ejecutor)
    {
        string storedProcedure = "insert_habitacion";

        var parametros = new DynamicParameters();
        parametros.Add("p_idHotel", habitacion.hotel.idHotel);
        parametros.Add("p_idTipo", habitacion.tipoHabitacion.idTipo);
        parametros.Add("p_PrecioPorNoche", habitacion.PrecioPorNoche);
        parametros.Add("p_idHabitacion", direction: ParameterDirection.Output);

        await ejecutor(storedProcedure, parametros);

        habitacion.idHabitacion = parametros.Get<uint>("p_idHabitacion");
        return habitacion.idHabitacion;
    }

    public uint Alta(Habitacion habitacion)
    {
        return AltaHabitacionInternaAsync(habitacion, (sp, p) =>
        {
            _conexion.Execute(sp, p);
            return Task.CompletedTask;
        }).GetAwaiter().GetResult();
    }
    public async Task<uint> AltaAsync(Habitacion habitacion)
    {
        return await AltaHabitacionInternaAsync(habitacion, (sp, p) => _conexion.ExecuteAsync(sp, p));
    }

    //Destalle Async
    private async Task<Habitacion?> DetalleHabitacionInternaAsync(uint id, Func<string, object, Task<Habitacion?>> ejecutor)
    {
        string sql = "SELECT * FROM Habitacion WHERE idHabitacion = @Id LIMIT 1";
        return await ejecutor(sql, new { Id = id });
    }

    public Habitacion? Detalle(uint id)
    {
        return DetalleHabitacionInternaAsync(id, (sql, param) =>
        {
            var result = _conexion.QuerySingleOrDefault<Habitacion>(sql, param);
            return Task.FromResult(result);
        }).GetAwaiter().GetResult();
    }
    public async Task<Habitacion?> DetalleAsync(uint id)
    {
        return await DetalleHabitacionInternaAsync(id, (sql, param) =>
            _conexion.QuerySingleOrDefaultAsync<Habitacion>(sql, param));
    }

    //Listar Async
    private async Task<List<Habitacion>> ListarHabitacionInternaAsync(Func<string, Task<IEnumerable<Habitacion>>> ejecutor)
    {
        string sql = "SELECT * FROM Habitacion";
        var resultado = await ejecutor(sql);
        return resultado.ToList();
    }

    public List<Habitacion> Listar()
    {
        return ListarHabitacionInternaAsync(sql =>
        {
            var result = _conexion.Query<Habitacion>(sql);
            return Task.FromResult(result);
        }).GetAwaiter().GetResult();
    }
    public async Task<List<Habitacion>> ListarAsync()
    {
        return await ListarHabitacionInternaAsync(sql => _conexion.QueryAsync<Habitacion>(sql));
    }


    //Informar
    private async Task<List<Habitacion>> InformarHabitacionInternoAsync(string sql, object parametros, Func<string, object, Task<IEnumerable<Habitacion>>> ejecutor)
    {
        var resultado = await ejecutor(sql, parametros);
        return resultado.ToList();
    }

    public List<Habitacion> InformarHabitacionPorIdHotel(uint idHotel)
    {
        string sql = "SELECT * FROM Habitacion WHERE idHotel = @IdHotel";
        return InformarHabitacionInternoAsync(sql, new { IdHotel = idHotel }, (q, p) =>
        {
            var result = _conexion.Query<Habitacion>(q, p);
            return Task.FromResult(result);
        }).GetAwaiter().GetResult();
    }

    public async Task<List<Habitacion>> InformarHabitacionPorIdHotelAsync(uint idHotel)
    {
        string sql = "SELECT * FROM Habitacion WHERE idHotel = @IdHotel";
        return await InformarHabitacionInternoAsync(sql, new { IdHotel = idHotel }, (q, p) =>
            _conexion.QueryAsync<Habitacion>(q, p));
    }

    public List<Habitacion> InformarHabitacionPorIdTipo(uint idTipo)
    {
        string sql = "SELECT * FROM Habitacion WHERE idTipo = @IdTipo";
        return InformarHabitacionInternoAsync(sql, new { IdTipo = idTipo }, (q, p) =>
        {
            var result = _conexion.Query<Habitacion>(q, p);
            return Task.FromResult(result);
        }).GetAwaiter().GetResult();
    }

    public async Task<List<Habitacion>> InformarHabitacionPorIdTipoAsync(uint idTipo)
    {
        string sql = "SELECT * FROM Habitacion WHERE idTipo = @IdTipo";
        return await InformarHabitacionInternoAsync(sql, new { IdTipo = idTipo }, (q, p) =>
            _conexion.QueryAsync<Habitacion>(q, p));
    }
}

