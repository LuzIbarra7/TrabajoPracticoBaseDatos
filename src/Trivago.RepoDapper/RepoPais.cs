using Microsoft.VisualBasic;

namespace Trivago.RepoDapper;
public class RepoPais : RepoDapper, IRepoPais
{
    public RepoPais(IDbConnection conexion) : base(conexion)
    {
    }

    public uint Alta(Pais pais)
    {
        string storedProcedure = "insert_pais";

        var parametros = new DynamicParameters();
        parametros.Add("p_Nombre", pais.Nombre);
        parametros.Add("p_idPais", direction: ParameterDirection.Output);
               
        _conexion.Execute(storedProcedure, parametros);

        pais.idPais = parametros.Get<uint>("p_idPais");
        return pais.idPais;
    }

    public Pais? Detalle(uint id)
    {
        string sql = "Select * from Pais where idPais = @Id LIMIT 1";
        var resultado = _conexion.QuerySingleOrDefault<Pais>(sql, new { Id = id});
        return resultado;
    }
    public Pais? DetallePorNombre(string nombrePais)
    {
        string sql = "Select * from Pais where Nombre = @Nombre Limit 1";
        var resultado = _conexion.QuerySingleOrDefault<Pais>(sql, new {Nombre = nombrePais});
        return resultado;
    }

    public List<Pais> Listar()
    {
        string sql = "Select * from Pais";
        var resultado = _conexion.Query<Pais>(sql).ToList();
        return resultado;
    }
}
