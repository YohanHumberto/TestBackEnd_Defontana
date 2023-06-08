DECLARE @fechaLimite datetime = DATEADD(day, -30, GETDATE());

--  El total de ventas de los últimos 30 días (monto total y Q de ventas).
SELECT COUNT(ID_Venta) AS Total_Ventas, SUM(Total)
FROM Venta
WHERE Fecha >= @fechaLimite 

--· El día y hora en que se realizó la venta con el monto más alto (y cuál es aquel monto).
SELECT top 1 [Total], Fecha 
FROM [Prueba].[dbo].[Venta]
WHERE Fecha >= @fechaLimite 
order by Total desc

--· Indicar cuál es el producto con mayor monto total de ventas.
SELECT TOP 1 A.ID_Producto, SUM(TotalLinea) AS MontoTotal, (SELECT NOMBRE FROM Producto WHERE ID_Producto = A.ID_Producto) AS PRODUCTO
FROM VentaDetalle A
INNER JOIN Venta B ON B.ID_Venta = A.ID_Venta
WHERE B.Fecha >= @fechaLimite 
GROUP BY A.ID_Producto
ORDER BY MontoTotal DESC


--· Indicar el local con mayor monto de ventas.
SELECT top 1 ID_Local, SUM(Total) as MontoTotal,(SELECT NOMBRE FROM Local B WHERE B.ID_Local = A.ID_Local ) as NOMBRE
FROM Venta A
WHERE Fecha >= @fechaLimite 
group by ID_Local
order by  MontoTotal desc

--· ¿Cuál es la marca con mayor margen de ganancias?
SELECT TOP (1) p.ID_Marca,
(SELECT NOMBRE FROM Marca M WHERE M.ID_Marca = P.ID_Marca) AS NOMBRE, 
SUM(Precio_Unitario - Costo_Unitario) as margen
  FROM [Prueba].[dbo].[Producto] p
  INNER JOIN  VentaDetalle vd on vd.ID_Producto = p.ID_Producto
  INNER JOIN Venta V ON V.ID_Venta = VD.ID_Venta
  WHERE V.Fecha >= @fechaLimite 
  group by p.ID_Marca
  order by margen desc

--· ¿Cómo obtendrías cuál es el producto que más se vende en cada local?
SELECT 
[t].[Nombre] as Local,
[t0].[Nombre] AS Producto,
[t0].[Cantidad_ventas]
FROM (
    SELECT [l].[ID_Local], [l].[Nombre] 
	FROM [Venta] AS [v]
    INNER JOIN [Local] AS [l] ON [v].[ID_Local] = [l].[ID_Local]
    WHERE [v].[Fecha] >= @fechaLimite
    GROUP BY [l].[ID_Local], [l].[Nombre]
) AS [t]
OUTER APPLY (
    SELECT TOP(1) [p].[ID_Producto],[p].[Nombre], 
	SUM([v1].[Cantidad]) AS [cantidad_ventas], 1 AS [c]
    FROM [Venta] AS [v0]
    INNER JOIN [Local] AS [l0] ON [v0].[ID_Local] = [l0].[ID_Local]
    INNER JOIN [VentaDetalle] AS [v1] ON [v0].[ID_Venta] = [v1].[ID_Venta]
    INNER JOIN [Producto] AS [p] ON [v1].[ID_Producto] = [p].[ID_Producto]
    WHERE [v0].[Fecha] >= @fechaLimite AND [t].[ID_Local] = [l0].[ID_Local]
    GROUP BY [p].[ID_Producto], [p].[Codigo], [p].[Nombre]
    ORDER BY SUM([v1].[Cantidad]) DESC
) AS [t0]
ORDER BY [t0].[Cantidad_ventas] DESC

