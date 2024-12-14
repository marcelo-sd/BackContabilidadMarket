using ContabilidaMarket.clasesUtiles;

namespace ContabilidaMarket.interfaces01
{
    public interface IAltasMain
    {
        Task<string> RegistrarVentas(ModeloVenta newVenta);

    }
}
