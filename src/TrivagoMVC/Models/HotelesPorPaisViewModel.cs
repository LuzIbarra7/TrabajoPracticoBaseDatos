namespace TrivagoMVC.ViewModels
{
    public class HotelesPorPaisViewModel
    {
        public string NombrePais { get; set; }
        public List<HotelConCiudadViewModel> Hoteles { get; set; } = new List<HotelConCiudadViewModel>();
    }
}
