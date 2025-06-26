namespace Trivago.RepoDapper;
public class RepoCiudad : RepoDapper, IRepoCiudad
{
    public RepoCiudad(IDbConnection conexion) : base(conexion)
    {
    }

    public uint Alta(Ciudad ciudad)
    {
        string storedProcedure = "insert_ciudad";

        var parametros = new DynamicParameters();
        parametros.Add("p_nombre", ciudad.Nombre);
        parametros.Add("p_idPais", ciudad.idPais);
        parametros.Add("p_idCiudad", direction: ParameterDirection.Output);
               
        _conexion.Execute(storedProcedure, parametros);

        ciudad.idCiudad = parametros.Get<uint>("p_idCiudad");
        return ciudad.idCiudad;
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
    public List<Ciudad> InformarCiudadPorIdPais(uint idPais)
    {
        string sql = "Select * from Ciudad where idPais = @IdPais";
        var resultado = _conexion.Query<Ciudad>(sql, new { IdPais = idPais} ).ToList();
        return resultado;
    }

}