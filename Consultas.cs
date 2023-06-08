using Microsoft.EntityFrameworkCore;
using TestBackEnd_Defontana.Migrations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TestBackEnd_Defontana
{
    public class Consultas
    {

        private List<Ventum> ventas = new List<Ventum>();
        private DateTime fechaLimite = DateTime.Now.AddDays(-30);

        public Consultas()
        {
            var dbContext = new PruebaContext();
            ventas = dbContext.Venta
                .Include(x => x.IdLocalNavigation)
                .Include(a => a.VentaDetalles)
                .ThenInclude(a => a.IdProductoNavigation)
                .ThenInclude(a => a.IdMarcaNavigation)
                .Where(a => a.Fecha >= fechaLimite)
                .ToList();

        }

        public (int, int) GetTotalVentas() => (ventas.Sum(a => a.Total), ventas.Count());

        public Ventum GetVentaMasAlta() => ventas.OrderByDescending(x => x.Total).FirstOrDefault();

        public (Producto, int) GetProductoConMayorMontoTotalDeVentas()
        {
            var productoConMayorMonto = ventas
           .SelectMany(a => a.VentaDetalles)
           .GroupBy(vd => vd.IdProductoNavigation)
           .Select(g => new
           {
               Producto = g.Key,
               MontoTotal = g.Sum(vd => vd.TotalLinea)
           })
          .OrderByDescending(p => p.MontoTotal)
          .FirstOrDefault();

            return (productoConMayorMonto.Producto, productoConMayorMonto.MontoTotal);
        }

        public (Local, int) GetLocalConMayorMontoTotalDeVentas()
        {
            var localConMayorMonto = ventas
                .GroupBy(v => v.IdLocalNavigation)
                .Select(g => new
                {
                    Local = g.Key,
                    MontoTotal = g.Sum(v => v.Total)
                })
                .OrderByDescending(p => p.MontoTotal)
                .FirstOrDefault();

            return (localConMayorMonto.Local, localConMayorMonto.MontoTotal);
        }

        public (Marca, int) GetMarcaConMayorMargenDeGanancias()
        {
            var res = ventas
                .SelectMany(a => a.VentaDetalles)
                .GroupBy(a => a.IdProductoNavigation.IdMarcaNavigation)
                .Select(a => new
                {
                    marca = a.Key,
                    margen = a.Sum(b => b.PrecioUnitario - b.IdProductoNavigation.CostoUnitario),
                })
                .OrderByDescending(a => a.margen)
                .FirstOrDefault();

            return (res.marca, res.margen);
        }

        public List<(Local, (Producto, int))> GetProductoMasVendidoPorLocal()
        {
            var res = ventas
            .GroupBy(a => a.IdLocalNavigation)
            .Select(a => new
            {
                Local = a.Key,
                Venta = a.SelectMany(b => b.VentaDetalles)
                .GroupBy(b => b.IdProductoNavigation).Select(a => new
                {
                    producto = a.Key,
                    cantidad_ventas = a.Sum(b => b.Cantidad)
                }).OrderByDescending(b => b.cantidad_ventas).FirstOrDefault()
            }).OrderByDescending(x => x.Venta.cantidad_ventas).ToList();

            var ress = res.Select(a => (a.Local, (a.Venta.producto, a.Venta.cantidad_ventas))).ToList();
            return ress;
        }
    }
}
