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
    public Pais? Detalle(uint id)
    {
        string sql = @"
        SELECT * FROM Pais WHERE idPais = @Id LIMIT 1;
        SELECT * FROM Ciudad WHERE idPais = @Id;
    ";

        using (var multi = _conexion.QueryMultiple(sql, new { Id = id }))
        {
            var pais = multi.ReadSingleOrDefault<Pais>();
            if (pais is not null)
            {
                pais.Ciudades = multi.Read<Ciudad>().ToList();
            }
            return pais;
        }
    }

    public async Task<Pais?> DetalleAsync(uint id)
    {
        string sql = @"
        SELECT * FROM Pais WHERE idPais = @Id LIMIT 1;
        SELECT * FROM Ciudad WHERE idPais = @Id;
    ";

        using (var multi = await _conexion.QueryMultipleAsync(sql, new { Id = id }))
        {
            var pais = await multi.ReadSingleOrDefaultAsync<Pais>();
            if (pais is not null)
            {
                pais.Ciudades = (await multi.ReadAsync<Ciudad>()).ToList();
            }
            return pais;
        }
    }


    //Detalle por nombre Async
    public Pais? DetallePorNombre(string nombrePais)
    {
        string sql = @"
        SELECT * FROM Pais WHERE Nombre = @Nombre LIMIT 1;
        SELECT * FROM Ciudad WHERE idPais = (SELECT idPais FROM Pais WHERE Nombre = @Nombre LIMIT 1);
    ";

        using (var multi = _conexion.QueryMultiple(sql, new { Nombre = nombrePais }))
        {
            var pais = multi.ReadSingleOrDefault<Pais>();
            if (pais is not null)
            {
                pais.Ciudades = multi.Read<Ciudad>().ToList();
            }
            return pais;
        }
    }

    public async Task<Pais?> DetallePorNombreAsync(string nombrePais)
    {
        string sql = "SELECT * FROM Pais WHERE Nombre = @Nombre LIMIT 1";
        return await _conexion.QuerySingleOrDefaultAsync<Pais>(sql, new { Nombre = nombrePais });
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
