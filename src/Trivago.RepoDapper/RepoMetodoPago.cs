using System.Data;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.RepoDapper;

public class RepoMetodoPago : RepoDapper, IRepoMetodoPago
{
    public RepoMetodoPago(IDbConnection conexion) : base(conexion)
    {
    }

    //Altas MetodoPago
    private async Task<uint> AltaMetodoPagoInternaAsync(MetodoPago metodoPago, Func<string, DynamicParameters, Task> ejecutor)
    {
        string storedProcedure = "insert_metodo_pago";

        var parametros = new DynamicParameters();
        parametros.Add("p_TipoMedioPago", metodoPago.TipoMedioPago);
        parametros.Add("p_idMetodoPago",direction: ParameterDirection.Output);

        await ejecutor(storedProcedure, parametros);

        metodoPago.idMetodoPago = parametros.Get<uint>("p_idMetodoPago");
        return metodoPago.idMetodoPago;
    }

    public uint Alta(MetodoPago metodoPago)
    {
        return AltaMetodoPagoInternaAsync(metodoPago, (sp, p) =>
        {
            _conexion.Execute(sp, p);
            return Task.CompletedTask;
        }).GetAwaiter().GetResult();
    }
    public async Task<uint> AltaAsync(MetodoPago metodoPago)
    {
        return await AltaMetodoPagoInternaAsync(metodoPago, (sp, p) => _conexion.ExecuteAsync(sp, p));
    }

    //Detalle Async
    private async Task<MetodoPago?> DetalleMetodoPagoInternaAsync(uint id, Func<string, object, Task<MetodoPago?>> ejecutor)
    {
        string sql = "SELECT * FROM MetodoPago WHERE idMetodoPago = @Id LIMIT 1";
        return await ejecutor(sql, new { Id = id });
    }

    public MetodoPago? Detalle(uint id)
    {
        return DetalleMetodoPagoInternaAsync(id, (sql, param) =>
        {
            var result = _conexion.QuerySingleOrDefault<MetodoPago>(sql, param);
            return Task.FromResult(result);
        }).GetAwaiter().GetResult();
    }
    public async Task<MetodoPago?> DetalleAsync(uint id)
    {
        return await DetalleMetodoPagoInternaAsync(id, (sql, param) =>
            _conexion.QuerySingleOrDefaultAsync<MetodoPago>(sql, param));
    }

    //Listar Async
    private async Task<List<MetodoPago>> ListarMetodoPagoInternaAsync(Func<string, Task<IEnumerable<MetodoPago>>> ejecutor)
    {
        string sql = "SELECT * FROM MetodoPago";
        var resultado = await ejecutor(sql);
        return resultado.ToList();
    }

    public List<MetodoPago> Listar()
    {
        return ListarMetodoPagoInternaAsync(sql =>
        {
            var result = _conexion.Query<MetodoPago>(sql);
            return Task.FromResult(result);
        }).GetAwaiter().GetResult();
    }
    public async Task<List<MetodoPago>> ListarAsync()
    {
        return await ListarMetodoPagoInternaAsync(sql => _conexion.QueryAsync<MetodoPago>(sql));
    }
}