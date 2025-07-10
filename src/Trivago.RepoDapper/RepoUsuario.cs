using System.Data;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.RepoDapper;

public class RepoUsuario : RepoDapper, IRepoUsuario
{
    public RepoUsuario(IDbConnection conexion) : base(conexion)
    {
    }
    //altas
    private async Task<uint> AltaUsuarioInternaAsync(Usuario usuario, Func<string, DynamicParameters, Task> ejecutor)
    {
        string storedProcedure = "insert_usuario";

        var parametros = new DynamicParameters();
        parametros.Add("p_Nombre", usuario.Nombre);
        parametros.Add("p_Apellido", usuario.Apellido);
        parametros.Add("p_Mail", usuario.Mail);
        parametros.Add("p_Contraseña", usuario.Contrasena);
        parametros.Add("p_idUsuario", ParameterDirection.Output);

        await ejecutor(storedProcedure, parametros);

        usuario.idUsuario = parametros.Get<uint>("p_idUsuario");
        return usuario.idUsuario;
    }

    public uint Alta(Usuario usuario)
    {
        return AltaUsuarioInternaAsync(usuario, (sp, p) =>
        {
            _conexion.Execute(sp, p);
            return Task.CompletedTask;
        }).GetAwaiter().GetResult();
    }
    public async Task<uint> AltaAsync(Usuario usuario)
    {
        return await AltaUsuarioInternaAsync(usuario, (sp, p) => _conexion.ExecuteAsync(sp, p));
    }

    //detalle

    private async Task<Usuario?> DetalleUsuarioInternaAsync(uint id, Func<string, object, Task<Usuario?>> ejecutor)
    {
        string sql = "SELECT * FROM Usuario WHERE idUsuario = @Id LIMIT 1";
        return await ejecutor(sql, new { Id = id });
    }

    public Usuario? Detalle(uint id)
    {
        return DetalleUsuarioInternaAsync(id, (sql, param) =>
        {
            var result = _conexion.QuerySingleOrDefault<Usuario>(sql, param);
            return Task.FromResult(result);
        }).GetAwaiter().GetResult();
    }
    public async Task<Usuario?> DetalleAsync(uint id)
    {
        return await DetalleUsuarioInternaAsync(id, (sql, param) =>
            _conexion.QuerySingleOrDefaultAsync<Usuario>(sql, param));
    }


    //listar
    private async Task<List<Usuario>> ListarUsuarioInternaAsync(Func<string, Task<IEnumerable<Usuario>>> ejecutor)
    {
        string sql = "SELECT * FROM Usuario";
        var resultado = await ejecutor(sql);
        return resultado.ToList();
    }

    public List<Usuario> Listar()
    {
        return ListarUsuarioInternaAsync(sql =>
        {
            var result = _conexion.Query<Usuario>(sql);
            return Task.FromResult(result);
        }).GetAwaiter().GetResult();
    }
    public async Task<List<Usuario>> ListarAsync()
    {
        return await ListarUsuarioInternaAsync(sql => _conexion.QueryAsync<Usuario>(sql));
    }

//usuario
    private async Task<Usuario?> UsuarioPorPassInternaAsync(string email, string pass, Func<string, object, Task<Usuario?>> ejecutor)
    {
        string sql = "SELECT * FROM Usuario WHERE Mail = @mail AND Contrasena = @Contrasena";
        return await ejecutor(sql, new { mail = email, Contrasena = pass });
    }
    public async Task<Usuario?> UsuarioPorPassAsync(string email, string pass)
    {
        return await UsuarioPorPassInternaAsync(email, pass, (sql, param) =>
            _conexion.QuerySingleOrDefaultAsync<Usuario>(sql, param));
    }

    public Usuario? UsuarioPorPass(string email, string pass)
    {
        return UsuarioPorPassInternaAsync(email, pass, (sql, param) =>
        {
            var result = _conexion.QuerySingleOrDefault<Usuario>(sql, param);
            return Task.FromResult(result);
        }).GetAwaiter().GetResult();
    }

}