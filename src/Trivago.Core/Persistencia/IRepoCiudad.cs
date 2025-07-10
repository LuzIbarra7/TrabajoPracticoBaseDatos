using Trivago.Core.Ubicacion;

namespace Trivago.Core.Persistencia;

public interface IRepoCiudad :  IAlta<Ciudad, uint>, IListado<Ciudad>, IDetalle<Ciudad, uint>
{
    List<Ciudad> InformarCiudadPorIdPais(uint idPais);
    Task<uint> AltaAsync(Ciudad ciudad);
    Task<Ciudad?> DetalleAsync(uint id);
    Task<List<Ciudad>> ListarAsync();
    Task<List<Ciudad>> InformarCiudadAsync(uint idPais);
}