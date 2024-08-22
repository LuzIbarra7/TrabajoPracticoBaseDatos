namespace Trivago.RepoDapper;
public class RepoCiudad : RepoDapper, IRepoCiudad
{
    public RepoCiudad(IDbConnection conexion) : base(conexion)
    {
    }

    public uint Alta(Ciudad ciudad)
    {
        string storedProcedure = "insert_ciudad";
        var IdInsertado = _conexion.QuerySingle<uint>(storedProcedure, ciudad);
        return IdInsertado;
    }
    public Ciudad? Detalle(uint id)
    {
        string sql = "Select * from Ciudad where idCiudad = @Id LIMIT 1";
        var resultado = _conexion.QuerySingleOrDefault<Ciudad>(sql, new { Id = id});
        return resultado;
    }

    public List<Ciudad> Listar()
    {
        string sql = "Select * from Ciudad";
        var resultado = _conexion.Query<Ciudad>(sql).ToList();
        return resultado;
    }
    public List<Ciudad> InformarCiudad(uint idPais)
    {
        string sql = "Select * from Ciudad where idPais = @IdPais";
        var resultado = _conexion.Query<Ciudad>(sql, new { IdPais = idPais} ).ToList();
        return resultado;
    }

}