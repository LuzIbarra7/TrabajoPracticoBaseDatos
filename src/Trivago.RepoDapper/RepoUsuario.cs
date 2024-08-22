using System.Data;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.RepoDapper;

public class RepoUsuario : RepoDapper, IRepoUsuario
{
    public RepoUsuario(IDbConnection conexion) : base(conexion)
    {
    }

    public uint Alta(Usuario usuario)
    {
        string storedProcedure = "insert_usuario";
        var IdInsertado = _conexion.QuerySingle<uint>(storedProcedure, usuario);
        return IdInsertado;
    }

    public Usuario? Detalle(uint id)
    {
        string sql = "Select * from Usuario where idUsuario = @Id LIMIT 1";
        var resultado = _conexion.QuerySingleOrDefault<Usuario>(sql, new { Id = id});
        return resultado;
    }

    public List<Usuario> Listar()
    {
        string sql = "Select * from Usuario";
        var resultado = _conexion.Query<Usuario>(sql).ToList();
        return resultado;
    }

    public Usuario? UsuarioPorPass(string email, string pass)
    {
        throw new NotImplementedException();
    }
}