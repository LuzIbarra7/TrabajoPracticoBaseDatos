using System.Data;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;
using Dapper;
using System.Security.Cryptography;
using System.Text;

namespace Trivago.RepoDapper;

public class RepoUsuario : RepoDapper, IRepoUsuario
{
    public RepoUsuario(IDbConnection conexion) : base(conexion) { }

    private string HashContrasena(string contrasena)
    {
        using var sha = SHA256.Create();
        byte[] bytes = Encoding.UTF8.GetBytes(contrasena);
        byte[] hash = sha.ComputeHash(bytes);
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }

    // --- ALTAS ---
    private async Task<uint> AltaUsuarioInternaAsync(Usuario usuario, Func<string, DynamicParameters, Task> ejecutor)
    {
        string storedProcedure = "insert_usuario";

        var parametros = new DynamicParameters();
        parametros.Add("p_Nombre", usuario.Nombre);
        parametros.Add("p_Apellido", usuario.Apellido);
        parametros.Add("p_Mail", usuario.Mail);
        parametros.Add("p_Contrasena", HashContrasena(usuario.Contrasena));
        parametros.Add("p_idUsuario", dbType: DbType.UInt32, direction: ParameterDirection.Output);

        await ejecutor(storedProcedure, parametros);

        usuario.idUsuario = parametros.Get<uint>("p_idUsuario");
        return usuario.idUsuario;
    }

    public uint Alta(Usuario usuario)
    {
        return AltaUsuarioInternaAsync(usuario, (sp, p) =>
        {
            _conexion.Execute(sp, p, commandType: CommandType.StoredProcedure);
            return Task.CompletedTask;
        }).GetAwaiter().GetResult();
    }

    public async Task<uint> AltaAsync(Usuario usuario)
    {
        return await AltaUsuarioInternaAsync(usuario, (sp, p) =>
            _conexion.ExecuteAsync(sp, p, commandType: CommandType.StoredProcedure));
    }

    // --- DETALLE ---
    private async Task<Usuario?> DetalleUsuarioInternaAsync(uint id, Func<string, object, Task<Usuario?>> ejecutor)
    {
        string sql = "SELECT idUsuario, Nombre, Apellido, Mail FROM Usuario WHERE idUsuario = @Id LIMIT 1";
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

    // --- LISTAR ---
    private async Task<List<Usuario>> ListarUsuarioInternaAsync(Func<string, Task<IEnumerable<Usuario>>> ejecutor)
    {
        string sql = "SELECT idUsuario, Nombre, Apellido, Mail FROM Usuario";
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

    // --- LOGIN ---
    private async Task<Usuario?> UsuarioPorPassInternaAsync(string email, string pass, Func<string, object, Task<Usuario?>> ejecutor)
    {
        string sql = "SELECT idUsuario, Nombre, Apellido, Mail FROM Usuario WHERE Mail = @mail AND Contrasena = @Contrasena";
        return await ejecutor(sql, new { mail = email, Contrasena = HashContrasena(pass) });
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

    // --- ACTUALIZAR ---
    public async Task ActualizarAsync(Usuario usuario)
    {
        // Obtener la contraseña actual si se deja vacía
        string contrasenaFinal = usuario.Contrasena;
        if (string.IsNullOrEmpty(usuario.Contrasena))
        {
            var actual = await DetalleAsync(usuario.idUsuario);
            contrasenaFinal = actual?.Contrasena ?? "";
        }
        else
        {
            contrasenaFinal = HashContrasena(usuario.Contrasena);
        }

        string sql = @"UPDATE Usuario 
                       SET Nombre = @Nombre, Apellido = @Apellido, Mail = @Mail, Contrasena = @Contrasena 
                       WHERE idUsuario = @idUsuario";

        await _conexion.ExecuteAsync(sql, new
        {
            Nombre = usuario.Nombre,
            Apellido = usuario.Apellido,
            Mail = usuario.Mail,
            Contrasena = contrasenaFinal,
            idUsuario = usuario.idUsuario
        });
    }

    // Método para actualizar contraseña solo si viene
    public async Task ActualizarContrasenaAsync(uint idUsuario, string nuevaContrasena)
    {
        string sql = "UPDATE Usuario SET Contrasena = @Contrasena WHERE idUsuario = @idUsuario";
        await _conexion.ExecuteAsync(sql, new { Contrasena = HashContrasena(nuevaContrasena), idUsuario });
    }
}
