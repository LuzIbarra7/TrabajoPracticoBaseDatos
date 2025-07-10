namespace Trivago.Core.Persistencia;

public interface IListado<T>
{
    public List<T> Listar();
    Task<List<T>> ListarAsync();
}