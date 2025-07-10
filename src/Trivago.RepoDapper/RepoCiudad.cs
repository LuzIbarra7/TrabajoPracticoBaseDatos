namespace Trivago.RepoDapper;
public class RepoCiudad : RepoDapper, IRepoCiudad
{
    public RepoCiudad(IDbConnection conexion) : base(conexion)
    {
    }

    //Altas Ciudad
    private async Task<uint> AltaInternaAsync(Ciudad ciudad, Func<string, DynamicParameters, Task> executor)
    {
        string storedProcedure = "insert_ciudad";

        var parametros = new DynamicParameters();
        parametros.Add("p_nombre", ciudad.Nombre);
        parametros.Add("p_idPais", ciudad.idPais);
        parametros.Add("p_idCiudad", direction: ParameterDirection.Output);

        await executor(storedProcedure, parametros);

        ciudad.idCiudad = parametros.Get<uint>("p_idCiudad");
        return ciudad.idCiudad;
    }

    public uint Alta(Ciudad ciudad)
    {
        return AltaInternaAsync(ciudad, (sp, p) =>
        {
            _conexion.Execute(sp, p);
            return Task.CompletedTask;
        }).Result;
    }
    public async Task<uint> AltaAsync(Ciudad ciudad)
    {
        return await AltaInternaAsync(ciudad, (sp, p) => _conexion.ExecuteAsync(sp, p));
    }


    //Async Detalle
    private async Task<Ciudad?> DetalleInternoAsync(uint id, Func<string, object, Task<Ciudad?>> ejecutor)
    {
        string sql = "SELECT * FROM Ciudad WHERE idCiudad = @Id LIMIT 1";
        return await ejecutor(sql, new { Id = id });
    }

    public Ciudad? Detalle(uint id)
    {
        return DetalleInternoAsync(id, (query, param) =>
        {
            var result = _conexion.QuerySingleOrDefault<Ciudad>(query, param);
            return Task.FromResult(result);
        }).GetAwaiter().GetResult();
    }
    public async Task<Ciudad?> DetalleAsync(uint id)
    {
        return await DetalleInternoAsync(id, (query, param) =>
            _conexion.QuerySingleOrDefaultAsync<Ciudad>(query, param));
    }


    //Listar Async
    private async Task<List<Ciudad>> ListarInternoAsync(Func<string, Task<IEnumerable<Ciudad>>> ejecutor)
    {
        string sql = "SELECT * FROM Ciudad";
        var resultado = await ejecutor(sql);
        return resultado.ToList();
    }

    public List<Ciudad> Listar()
    {
        return ListarInternoAsync(sql =>
        {
            var result = _conexion.Query<Ciudad>(sql);
            return Task.FromResult(result);
        }).GetAwaiter().GetResult();
    }
    public async Task<List<Ciudad>> ListarAsync()
    {
        return await ListarInternoAsync(sql => _conexion.QueryAsync<Ciudad>(sql));
    }

    //Asycn Informar
    private async Task<List<Ciudad>> InformarCiudadInternoAsync(uint idPais, Func<string, object, Task<IEnumerable<Ciudad>>> ejecutor)
    {
        string sql = "SELECT * FROM Ciudad WHERE idPais = @IdPais";
        var resultado = await ejecutor(sql, new { IdPais = idPais });
        return resultado.ToList();
    }

    public List<Ciudad> InformarCiudadPorIdPais(uint idPais)
    {
        return InformarCiudadInternoAsync(idPais, (sql, param) =>
        {
            var result = _conexion.Query<Ciudad>(sql, param);
            return Task.FromResult(result);
        }).GetAwaiter().GetResult();
    }
    public async Task<List<Ciudad>> InformarCiudadAsync(uint idPais)
    {
        return await InformarCiudadInternoAsync(idPais, (sql, param) =>
            _conexion.QueryAsync<Ciudad>(sql, param));
    }

}