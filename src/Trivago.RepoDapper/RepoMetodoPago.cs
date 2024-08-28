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

        var parametros = new DynamicParameters();
        parametros.Add("p_TipoMedioPago", metodoPago.TipoMedioPago);
        parametros.Add("p_idMetodoPago", ParameterDirection.Output);
               
        _conexion.Execute(storedProcedure, parametros);

        metodoPago.idMetodoPago = parametros.Get<uint>("p_idMetodoPago");
        return metodoPago.idMetodoPago;
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