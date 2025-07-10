namespace Trivago.RepoDapper;
public class RepoComentario : RepoDapper, IRepoComentario
{
    public RepoComentario(IDbConnection conexion) : base(conexion)
    {
    }

    //Altas Async
    private async Task<uint> AltaComentarioInternoAsync(Comentario comentario, Func<string, DynamicParameters, Task> ejecutor)
    {
        string storedProcedure = "insert_comentario";

        var parametros = new DynamicParameters();
        parametros.Add("p_idHabitacion", comentario.Habitacion);
        parametros.Add("p_Comentario", comentario.comentario);
        parametros.Add("p_Calificacion", comentario.Calificacion);
        parametros.Add("p_idComentario", direction: ParameterDirection.Output);

        await ejecutor(storedProcedure, parametros);

        comentario.idComentario = parametros.Get<uint>("p_idComentario");
        return comentario.idComentario;
    }


    public uint Alta(Comentario comentario)
    {
        return AltaComentarioInternoAsync(comentario, (sp, p) =>
        {
            _conexion.Execute(sp, p);
            return Task.CompletedTask;
        }).GetAwaiter().GetResult();
    }
    public async Task<uint> AltaAsync(Comentario comentario)
    {
        return await AltaComentarioInternoAsync(comentario, (sp, p) => _conexion.ExecuteAsync(sp, p));
    }

    //Detalle Async
    private async Task<Comentario?> DetalleComentarioInternoAsync(uint id, Func<string, object, Task<Comentario?>> ejecutor)
    {
        string sql = "SELECT * FROM Comentario WHERE idComentario = @Id LIMIT 1";
        return await ejecutor(sql, new { Id = id });
    }

    public Comentario? Detalle(uint id)
    {
        return DetalleComentarioInternoAsync(id, (sql, param) =>
        {
            var result = _conexion.QuerySingleOrDefault<Comentario>(sql, param);
            return Task.FromResult(result);
        }).GetAwaiter().GetResult();
    }
    public async Task<Comentario?> DetalleAsync(uint id)
    {
        return await DetalleComentarioInternoAsync(id, (sql, param) =>
            _conexion.QuerySingleOrDefaultAsync<Comentario>(sql, param));
    }

    //Listar Async
    private async Task<List<Comentario>> ListarComentarioInternoAsync(Func<string, Task<IEnumerable<Comentario>>> ejecutor)
    {
        string sql = "SELECT * FROM Comentario";
        var resultado = await ejecutor(sql);
        return resultado.ToList();
    }

    public List<Comentario> Listar()
    {
        return ListarComentarioInternoAsync(sql =>
        {
            var result = _conexion.Query<Comentario>(sql);
            return Task.FromResult(result);
        }).GetAwaiter().GetResult();
    }
    public async Task<List<Comentario>> ListarAsync()
    {
        return await ListarComentarioInternoAsync(sql => _conexion.QueryAsync<Comentario>(sql));
    }

    //Listar por Habitacion Async
    private async Task<List<Comentario>> ListarComentarioPorHabitacionInternoAsync(uint idHabitacion, Func<string, object, Task<IEnumerable<Comentario>>> ejecutor)
    {
        string sql = "SELECT * FROM Comentario WHERE idHabitacion = @IdHabitacion";
        var resultado = await ejecutor(sql, new { IdHabitacion = idHabitacion });
        return resultado.ToList();
    }

    public List<Comentario> ListarPorIdHabitacion(uint idHabitacion)
    {
        return ListarComentarioPorHabitacionInternoAsync(idHabitacion, (sql, param) =>
        {
            var result = _conexion.Query<Comentario>(sql, param);
            return Task.FromResult(result);
        }).GetAwaiter().GetResult();
    }
    public async Task<List<Comentario>> ListarPorIdHabitacionAsync(uint idHabitacion)
    {
        return await ListarComentarioPorHabitacionInternoAsync(idHabitacion, (sql, param) =>
            _conexion.QueryAsync<Comentario>(sql, param));
    }
}