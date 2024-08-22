namespace Trivago.RepoDapper;
public class RepoComentario : RepoDapper, IRepoComentario
{
    public RepoComentario(IDbConnection conexion) : base(conexion)
    {
    }

    public uint Alta(Comentario comentario)
    {
        string storedProcedure = "insert_comentario";
        var IdInsertado = _conexion.QuerySingle<uint>(storedProcedure, comentario);
        return IdInsertado;
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
}