using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trivago.Core.Ubicacion;

namespace Dtos
{


    public record struct Paisdto(uint IdPais, string Nombre);


    public record struct UsuarioDto(uint idUsuario, string Nombre, string Apellido, string Mail);
    public record struct CrearTipoHabitacionDto(string Nombre);
    public record struct TipoHabitacionDto(uint Id, string Nombre);
    public record struct CiudadesDto(uint idCiudad, string Nombre, uint idPais);
    public record struct HabitacionDto(uint idHabitacion, decimal PrecioPorNoche);
    public record struct HotelDto(uint idHotel, uint idCiudad, string Nombre, string Direccion, string Telefono, string URL);
    public record struct MetodoPagoDto(uint idMetodoPago, string TipoMedioPago);
    public record struct ReservaDto(uint idReserva,uint idHabitacion, uint idUsuario,MetodoPago MetodoPago, DateTime Entrada, DateTime Salida, decimal Precio, uint Telefono);

    public class PaisDto
    {
        public uint IdPais { get; set; }
        public string Nombre { get; set; }
        public List<CiudadDto> Ciudades { get; set; }
    }

public class CiudadDto
{
    public uint IdCiudad { get; set; }
    public uint IdPais { get; set; }
    public string Nombre { get; set; }
}
    }
    
