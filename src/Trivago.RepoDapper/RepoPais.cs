using Microsoft.VisualBasic;

namespace Trivago.RepoDapper;
public class RepoPais : RepoDapper, IRepoPais
{
    public RepoPais(IDbConnection conexion) : base(conexion)
    {
    }

    //Altas
    private async Task<uint> AltaPaisInternaAsync(Pais pais, Func<string, DynamicParameters, Task> ejecutor)
    {
        string storedProcedure = "insert_pais";

        var parametros = new DynamicParameters();
        parametros.Add("p_Nombre", pais.Nombre);
        parametros.Add("p_idPais", direction: ParameterDirection.Output);

        await ejecutor(storedProcedure, parametros);

        pais.idPais = parametros.Get<uint>("p_idPais");
        return pais.idPais;
    }

    public uint Alta(Pais pais)
    {
        return AltaPaisInternaAsync(pais, (sp, p) =>
        {
            _conexion.Execute(sp, p);
            return Task.CompletedTask;
        }).GetAwaiter().GetResult();
    }
    public async Task<uint> AltaAsync(Pais pais)
    {
        return await AltaPaisInternaAsync(pais, (sp, p) => _conexion.ExecuteAsync(sp, p));
    }

    //Detalle Async
    private async Task<Pais?> DetallePaisInternaAsync(object param, string filtro, Func<string, object, Task<Pais?>> ejecutor)
    {
        string sql = $"SELECT * FROM Pais WHERE {filtro} LIMIT 1";
        return await ejecutor(sql, param);
    }

    public Pais? Detalle(uint id)
    {
        return DetallePaisInternaAsync(new { Id = id }, "idPais = @Id", (sql, param) =>
        {
            var result = _conexion.QuerySingleOrDefault<Pais>(sql, param);
            return Task.FromResult(result);
        }).GetAwaiter().GetResult();
    }
    public async Task<Pais?> DetalleAsync(uint id)
    {
        return await DetallePaisInternaAsync(new { Id = id }, "idPais = @Id", (sql, param) =>
            _conexion.QuerySingleOrDefaultAsync<Pais>(sql, param));
    }

    //Detalle por nombre Async
    public Pais? DetallePorNombre(string nombrePais)
    {
        return DetallePaisInternaAsync(new { Nombre = nombrePais }, "Nombre = @Nombre", (sql, param) =>
        {
            var result = _conexion.QuerySingleOrDefault<Pais>(sql, param);
            return Task.FromResult(result);
        }).GetAwaiter().GetResult();
    }
    public async Task<Pais?> DetallePorNombreAsync(string nombrePais)
    {
        return await DetallePaisInternaAsync(new { Nombre = nombrePais }, "Nombre = @Nombre", (sql, param) =>
            _conexion.QuerySingleOrDefaultAsync<Pais>(sql, param));
    }

    //Listar Async
    private async Task<List<Pais>> ListarPaisInternaAsync(Func<string, Task<IEnumerable<Pais>>> ejecutor)
    {
        string sql = "SELECT * FROM Pais";
        var resultado = await ejecutor(sql);
        return resultado.ToList();
    }

    public List<Pais> Listar()
    {
        return ListarPaisInternaAsync(sql =>
        {
            var result = _conexion.Query<Pais>(sql);
            return Task.FromResult(result);
        }).GetAwaiter().GetResult();
    }
    public async Task<List<Pais>> ListarAsync()
    {
        return await ListarPaisInternaAsync(sql => _conexion.QueryAsync<Pais>(sql));
    }
}
