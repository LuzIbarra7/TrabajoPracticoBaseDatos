using System.Data;

namespace Trivago.RepoDapper;
public class RepoDapper
{
    protected readonly IDbConnection _conexion;

    public RepoDapper(IDbConnection conexion) => _conexion = conexion;
}