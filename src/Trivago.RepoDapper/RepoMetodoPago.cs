using System.Data;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

namespace Trivago.RepoDapper;

public class RepoMetodoPago : RepoDapper, IRepoMetodoPago
{
    public RepoMetodoPago(IDbConnection conexion) : base(conexion)
    {
    }

    public uint Alta(MetodoPago metodoPago)
    {
        string storedProcedure = "insert_metodo_pago";
        var IdInsertado = _conexion.QuerySingle<uint>(storedProcedure, metodoPago);
        return IdInsertado;
    }

    public MetodoPago? Detalle(uint id)
    {
        string sql = "Select * from MetodoPago where idMetodoPago = @Id LIMIT 1";
        var resultado = _conexion.QuerySingleOrDefault<MetodoPago>(sql, new { Id = id});
        return resultado;
    }

    public List<MetodoPago> Listar()
    {
        string sql = "Select * from MetodoPago";
        var resultado = _conexion.Query<MetodoPago>(sql).ToList();
        return resultado;
    }
}