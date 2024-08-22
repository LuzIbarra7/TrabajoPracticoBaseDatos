using System.Data;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.RepoDapper;

public class RepoPais : RepoDapper, IRepoPais
{
    public RepoPais(IDbConnection conexion) : base(conexion)
    {
    }

    public uint Alta(Pais pais)
    {
        string storedProcedure = "insert_pais";
        var IdInsertado = _conexion.QuerySingle<uint>(storedProcedure, pais);
        return IdInsertado;
    }

    public Pais? Detalle(uint id)
    {
        string sql = "Select * from Pais where idPais = @Id LIMIT 1";
        var resultado = _conexion.QuerySingleOrDefault<Pais>(sql, new { Id = id});
        return resultado;
    }

    public List<Pais> Listar()
    {
        string sql = "Select * from Pais";
        var resultado = _conexion.Query<Pais>(sql).ToList();
        return resultado;
    }
}
