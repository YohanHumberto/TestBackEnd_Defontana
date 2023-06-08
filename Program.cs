using TestBackEnd_Defontana;
using TestBackEnd_Defontana.Migrations;

public class Program
{

    public static void Main()
    {

        Consultas _consultas = new Consultas();

        {
            var (montoTotal, cantidadDeVentas) = _consultas.GetTotalVentas();

            Console.WriteLine($"####################################################");
            Console.WriteLine(" #1 TOTAL DE VENTAS");
            Console.WriteLine($"Monto total: {montoTotal}");
            Console.WriteLine($"Cantidad de ventas: {cantidadDeVentas}");
            Console.WriteLine($"####################################################");
        }

        {

            //· El día y hora en que se realizó la venta con el monto más alto(y cuál es aquel monto).
            var ventaMaxAlta = _consultas.GetVentaMasAlta();
            Console.WriteLine($"####################################################");
            Console.WriteLine(" #2 VENTA CON EL MONTO MAS ALTO");
            Console.WriteLine($"Dia y hora: {ventaMaxAlta.Fecha}");
            Console.WriteLine($"Monto: {ventaMaxAlta.Total}");
            Console.WriteLine($"####################################################");
        }

        {

            //· Indicar cuál es el producto con mayor monto total de ventas.
            var (productoConMayorMonto, montoTotal) = _consultas.GetProductoConMayorMontoTotalDeVentas();

            Console.WriteLine($"####################################################");
            Console.WriteLine(" #3 PRODUCTO CON MAYOR MONTO TOTAL DE VENTAS");
            Console.WriteLine($"Producto: {productoConMayorMonto.Nombre}");
            Console.WriteLine($"Total: {montoTotal}");
            Console.WriteLine($"####################################################");

        }
        {
            //· Indicar el local con mayor monto de ventas.
            var (localConMayorMonto, montoTotal) = _consultas.GetLocalConMayorMontoTotalDeVentas();

            Console.WriteLine($"####################################################");
            Console.WriteLine(" #4 lOCAL CON MAYOR MONTO DE VENTAS");
            Console.WriteLine($"Local: {localConMayorMonto.Nombre}");
            Console.WriteLine($"Total: {montoTotal}");
            Console.WriteLine($"####################################################");
        }

        {
            //· ¿Cuál es la marca con mayor margen de ganancias ?
            var (marca, margen) = _consultas.GetMarcaConMayorMargenDeGanancias();

            Console.WriteLine($"####################################################");
            Console.WriteLine(" #5 MARCA CON MAYOR MARGEN DE GANACIAS");
            Console.WriteLine($"Marca: {marca.Nombre}");
            Console.WriteLine($"Margen: {margen}");
            Console.WriteLine($"####################################################");
        }

        {
            //· ¿Cómo obtendrías cuál es el producto que más se vende en cada local?
            var listResult = _consultas.GetProductoMasVendidoPorLocal();


            Console.WriteLine($"####################################################");
            Console.WriteLine(" #6 PRODUCTO QUE MAS SE VENDE EN CADA LOCAL");
            listResult.ForEach(x =>
            {
                var (local, (product, cantidad_ventas)) = x;
                Console.WriteLine($"Local: {local.Nombre},  Producto: {product.Nombre},  Cantidad De Ventas: {cantidad_ventas}");
            });
            Console.WriteLine($"####################################################");

        }
    }
}