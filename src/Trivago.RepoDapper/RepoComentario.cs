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
    public Comentario? Detalle(uint id)
    {
        string sql = "SELECT * FROM Comentario WHERE idComentario = @Id LIMIT 1";
        return _conexion.QuerySingleOrDefault<Comentario>(sql, new { Id = id });
    }

    public async Task<Comentario?> DetalleAsync(uint id)
    {
        string sql = "SELECT * FROM Comentario WHERE idComentario = @Id LIMIT 1";
        return await _conexion.QuerySingleOrDefaultAsync<Comentario>(sql, new { Id = id });
    }


    //Listar Async
    public List<Comentario> Listar()
    {
        string sql = "SELECT * FROM Comentario";
        return _conexion.Query<Comentario>(sql).ToList();
    }

    public async Task<List<Comentario>> ListarAsync()
    {
        string sql = "SELECT * FROM Comentario";
        var resultado = await _conexion.QueryAsync<Comentario>(sql);
        return resultado.ToList();
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