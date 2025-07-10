using System.Data;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.RepoDapper;

public class RepoTipoHabitacion : RepoDapper, IRepoTipoHabitacion
{
    public RepoTipoHabitacion(IDbConnection conexion) : base(conexion)
    {
    }

    //Altas
    private async Task<uint> AltaTipoHabitacionInternaAsync(TipoHabitacion tipoHabitacion, Func<string, DynamicParameters, Task> ejecutor)
    {
        string storedProcedure = "insert_tipo_habitacion";

        var parametros = new DynamicParameters();
        parametros.Add("p_Nombre", tipoHabitacion.Nombre);
        parametros.Add("p_idTipo", ParameterDirection.Output);

        await ejecutor(storedProcedure, parametros);

        tipoHabitacion.idTipo = parametros.Get<uint>("p_idTipo");
        return tipoHabitacion.idTipo;
    }

    public uint Alta(TipoHabitacion tipoHabitacion)
    {
        return AltaTipoHabitacionInternaAsync(tipoHabitacion, (sp, p) =>
        {
            _conexion.Execute(sp, p);
            return Task.CompletedTask;
        }).GetAwaiter().GetResult();
    }
    public async Task<uint> AltaAsync(TipoHabitacion tipoHabitacion)
    {
        return await AltaTipoHabitacionInternaAsync(tipoHabitacion, (sp, p) => _conexion.ExecuteAsync(sp, p));
    }


    //Detalle
    private async Task<TipoHabitacion?> DetalleTipoHabitacionInternaAsync(uint id, Func<string, object, Task<TipoHabitacion?>> ejecutor)
    {
        string sql = "SELECT * FROM TipoHabitacion WHERE idTipo = @Id LIMIT 1";
        return await ejecutor(sql, new { Id = id });
    }

    public TipoHabitacion? Detalle(uint id)
    {
        return DetalleTipoHabitacionInternaAsync(id, (sql, param) =>
        {
            var result = _conexion.QuerySingleOrDefault<TipoHabitacion>(sql, param);
            return Task.FromResult(result);
        }).GetAwaiter().GetResult();
    }
    public async Task<TipoHabitacion?> DetalleAsync(uint id)
    {
        return await DetalleTipoHabitacionInternaAsync(id, (sql, param) =>
            _conexion.QuerySingleOrDefaultAsync<TipoHabitacion>(sql, param));
    }

    //Listar
    private async Task<List<TipoHabitacion>> ListarTipoHabitacionInternaAsync(Func<string, Task<IEnumerable<TipoHabitacion>>> ejecutor)
    {
        string sql = "SELECT * FROM TipoHabitacion";
        var resultado = await ejecutor(sql);
        return resultado.ToList();
    }

    public List<TipoHabitacion> Listar()
    {
        return ListarTipoHabitacionInternaAsync(sql =>
        {
            var result = _conexion.Query<TipoHabitacion>(sql);
            return Task.FromResult(result);
        }).GetAwaiter().GetResult();
    }
    public async Task<List<TipoHabitacion>> ListarAsync()
    {
        return await ListarTipoHabitacionInternaAsync(sql => _conexion.QueryAsync<TipoHabitacion>(sql));
    }

}