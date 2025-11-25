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
    public Ciudad? Detalle(uint id)
    {
        string sql = @"
        SELECT * FROM Ciudad WHERE idCiudad = @Id LIMIT 1;
        SELECT * FROM Hotel WHERE idCiudad = @Id;
    ";

        using (var multi = _conexion.QueryMultiple(sql, new { Id = id }))
        {
            var ciudad = multi.ReadSingleOrDefault<Ciudad>();
            if (ciudad is not null)
            {
                ciudad.Hoteles = multi.Read<Hotel>().ToList();
            }
            return ciudad;
        }
    }

    public async Task<Ciudad?> DetalleAsync(uint id)
    {
        string sql = @"
        SELECT * FROM Ciudad WHERE idCiudad = @Id LIMIT 1;
        SELECT * FROM Hotel WHERE idCiudad = @Id;
    ";

        using (var multi = await _conexion.QueryMultipleAsync(sql, new { Id = id }))
        {
            var ciudad = await multi.ReadSingleOrDefaultAsync<Ciudad>();
            if (ciudad is not null)
            {
                ciudad.Hoteles = (await multi.ReadAsync<Hotel>()).ToList();
            }
            return ciudad;
        }
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


   // Modificar ciudad
    public async Task ModificarAsync(Ciudad ciudad)
    {
        string sql = @"UPDATE Ciudad 
                       SET Nombre = @Nombre, idPais = @idPais 
                       WHERE idCiudad = @idCiudad";
        await _conexion.ExecuteAsync(sql, ciudad);
    }

}