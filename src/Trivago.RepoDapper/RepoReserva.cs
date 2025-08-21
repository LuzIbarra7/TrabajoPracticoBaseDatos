using System.Data;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.RepoDapper;

public class RepoReserva : RepoDapper, IRepoReserva
{
    public RepoReserva(IDbConnection conexion) : base(conexion)
    {
    }

    //Altas
    public async Task<uint> AltaAsync(Reserva reserva)
    {
        string storedProcedure = "insert_reserva";
        DynamicParameters parametros = CargarParamsAlta(reserva);

        await _conexion.ExecuteAsync(storedProcedure, parametros);

        reserva.idReserva = parametros.Get<uint>("p_idReserva");
        return reserva.idReserva;
    }

    private static DynamicParameters CargarParamsAlta(Reserva reserva)
    {
        var parametros = new DynamicParameters();
        parametros.Add("p_idHabitacion", reserva.idHabitacion);
        parametros.Add("p_idMetododePago", reserva.metodoPago.idMetodoPago);
        parametros.Add("p_idUsuario", reserva.idUsuario);
        parametros.Add("p_Entrada", reserva.Entrada);
        parametros.Add("p_Salida", reserva.Salida);
        parametros.Add("p_Telefono", reserva.Telefono);
        parametros.Add("p_idReserva", direction: ParameterDirection.Output);
        return parametros;
    }

    public uint Alta(Reserva reserva)
    {
        var parametrosAlta = CargarParamsAlta(reserva);

        _conexion.Execute("insert_reserva", parametrosAlta);

        reserva.idReserva = parametrosAlta.Get<uint>("p_idReserva");
        return reserva.idReserva;

    }

    //Detalles
    private async Task<Reserva?> DetalleReservaInternaAsync(uint id, Func<string, object, Task<Reserva?>> ejecutor)
    {
        string sql = "SELECT * FROM Reserva WHERE idReserva = @Id LIMIT 1";
        return await ejecutor(sql, new { Id = id });
    }

    public Reserva? Detalle(uint id)
    {
        return DetalleReservaInternaAsync(id, (sql, param) =>
        {
            var result = _conexion.QuerySingleOrDefault<Reserva>(sql, param);
            return Task.FromResult(result);
        }).GetAwaiter().GetResult();
    }
    public async Task<Reserva?> DetalleAsync(uint id)
    {
        return await DetalleReservaInternaAsync(id, (sql, param) =>
            _conexion.QuerySingleOrDefaultAsync<Reserva>(sql, param));
    }

    //Listar
    private async Task<List<Reserva>> ListarReservaInternaAsync(Func<string, Task<IEnumerable<Reserva>>> ejecutor)
    {
        string sql = "SELECT * FROM Reserva";
        var resultado = await ejecutor(sql);
        return resultado.ToList();
    }

    public List<Reserva> Listar()
    {
        return ListarReservaInternaAsync(sql =>
        {
            var result = _conexion.Query<Reserva>(sql);
            return Task.FromResult(result);
        }).GetAwaiter().GetResult();
    }
    public async Task<List<Reserva>> ListarAsync()
    {
        return await ListarReservaInternaAsync(sql => _conexion.QueryAsync<Reserva>(sql));
    }

    //Informar
    private async Task<List<Reserva>> ListarReservasPorCampoAsync(string campo, object valor, Func<string, object, Task<IEnumerable<Reserva>>> ejecutor)
    {
        string sql = $"SELECT * FROM Reserva WHERE {campo} = @{campo}";
        var resultado = await ejecutor(sql, new Dictionary<string, object> { [campo] = valor });
        return resultado.ToList();
    }
    public async Task<List<Reserva>> InformarReservasPorIdHabitacionAsync(uint idHabitacion)
    {
        return await ListarReservasPorCampoAsync("idHabitacion", idHabitacion,
    (sql, param) => _conexion.QueryAsync<Reserva>(sql, param));
    }

    public List<Reserva> InformarReservasPorIdHabitacion(uint idHabitacion)
    {
        return ListarReservasPorCampoAsync("idHabitacion", idHabitacion, (sql, param) =>
        {
            var result = _conexion.Query<Reserva>(sql, param);
            return Task.FromResult(result);
        }).GetAwaiter().GetResult();
    }

    public async Task<List<Reserva>> InformarReservasPorIdMetodoPagoAsync(uint idMetodoPago)
    {
        return await ListarReservasPorCampoAsync("idMetodoPago", idMetodoPago,
    (sql, param) => _conexion.QueryAsync<Reserva>(sql, param));

    }

    public List<Reserva> InformarReservasPorIdMetodoPago(uint idMetodoPago)
    {
        return ListarReservasPorCampoAsync("idMetodoPago", idMetodoPago, (sql, param) =>
        {
            var result = _conexion.Query<Reserva>(sql, param);
            return Task.FromResult(result);
        }).GetAwaiter().GetResult();
    }

}