namespace Trivago.RepoDapper;
public class RepoComentario : RepoDapper, IRepoComentario
{
    public RepoComentario(IDbConnection conexion) : base(conexion)
    {
    }

    public uint Alta(Comentario comentario)
    {
        string storedProcedure = "insert_comentario";

        var parametros = new DynamicParameters();
        parametros.Add("p_idHabitacion", comentario.Habitacion);
        parametros.Add("p_Comentario", comentario.comentario);
        parametros.Add("p_Calificacion", comentario.Calificacion);
        parametros.Add("p_idComentario", direction: ParameterDirection.Output);
               
        _conexion.Execute(storedProcedure, parametros);

        comentario.idComentario = parametros.Get<uint>("p_idComentario");
        return comentario.idComentario;
    }


    public Comentario? Detalle(uint id)
    {
        string sql = "Select * from Comentario where idComentario = @Id LIMIT 1";
        var resultado = _conexion.QuerySingleOrDefault<Comentario>(sql, new { Id = id});
        return resultado;
    }

    public List<Comentario> Listar()
    {
        string sql = "Select * from Comentario";
        var resultado = _conexion.Query<Comentario>(sql).ToList();
        return resultado;
    }

    public List<Comentario> ListarPorHabitacion(uint idHabitacion)
    {
        string sql = "Select * from Comentario where idHabitacion = @IdHabitacion";
        var resultado = _conexion.Query<Comentario>(sql, new { idHabitacion = idHabitacion} ).ToList();

        return resultado;
    }
}