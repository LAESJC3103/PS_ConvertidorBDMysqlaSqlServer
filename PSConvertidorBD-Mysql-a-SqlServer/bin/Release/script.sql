/****** Object:  StoredProcedure [dbo].[FiltroReporteAgrupacion]    Script Date: 28/04/2014 06:57:18 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FiltroReporteAgrupacion]
 (
 @gpoAnalisis VARCHAR(30),
 @agrupacion VARCHAR(30)
 ) 
AS
BEGIN
DECLARE @Grupo varchar(20); 
SET @Grupo = (SELECT COD_GPO FROM tblagrupacionart WHERE DES_GPO=@gpoAnalisis);
SELECT COD_AGR AS AGRUPACION FROM tblCatAgrupacionArt WHERE COD_GPO=@Grupo AND DES_AGR=@agrupacion;
END
GO
/****** Object:  StoredProcedure [dbo].[FiltroReporteProveedor]    Script Date: 28/04/2014 06:57:18 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FiltroReporteProveedor]
(
 @proveedor varchar(150)  
) 
AS
BEGIN
 SELECT COD_PROV AS PROVEEDOR FROM tblcatproveedor WHERE NOM_PROV=@proveedor;
END
GO
/****** Object:  StoredProcedure [dbo].[IniAnos]    Script Date: 28/04/2014 06:57:18 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[IniAnos]
AS  
BEGIN
SELECT DISTINCT year(FEC_REG) AS years FROM tblgralventas;
END
GO
/****** Object:  StoredProcedure [dbo].[rptAgrupacion]    Script Date: 28/04/2014 06:57:18 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[rptAgrupacion]
 (
  @FecI VARCHAR(30), 
  @FecF VARCHAR(30),  
  @unidad VARCHAR(10), 
  @agr VARCHAR(30), 
  @sucursales VARCHAR(7999)
 
 
 
 ) 
AS
BEGIN

DECLARE @consulta NVARCHAR(4000);
DECLARE @Todas VARCHAR(10);
DECLARE @vAgr VARCHAR(10);
SET @Todas = 'TODAS';
SET @vAgr = '-1';
SET @consulta = ' 
 DECLARE @TotalUnidades DECIMAL(13,4);
 DECLARE @TotalRegistros INT;
 DECLARE @TotalImportes DECIMAL(13,4);
 DECLARE @PorcLimiteAcumulado DECIMAL(13,4);
 DECLARE @Porc DECIMAL(13,4);
 DECLARE @TotalPorcUnidades DECIMAL(13,4); 
 DECLARE @Todas VARCHAR(10);
 DECLARE @TSUCURSALES VARCHAR(7999)
 
        DECLARE @ProdAgrupacion TABLE( 
        
                COD1_ART VARCHAR(20),
				TICKETS VARCHAR(300),
				UNIDAD_ART VARCHAR(10),
				UNIDADES DECIMAL(13,4),                                                                                                                                                            
				PORC_UNIDADES DECIMAL(13,4), 
				IMPORTE DECIMAL(13,4),
				PORC_IMPORTE DECIMAL(13,4))
     
       IF (''' + @unidad + ''' = '''+ @Todas +''') 
               
       INSERT INTO @ProdAgrupacion
       SELECT rv.COD1_ART, 
        COUNT(DISTINCT rv.REF_DOC) AS TICKETS,
        rv.COD_UND AS UNIDAD_ART,
        SUM(rv.CAN_ART - rv.CAN_DEV) AS UNIDADES,
        0 AS PORC_UNIDADES,
        SUM((((rv.CAN_ART * rv.PCIO_VEN) + rv.IMP1_REG + rv.IMP2_REG)/rv.CAN_ART) * (rv.CAN_ART - rv.CAN_DEV)) AS IMPORTE,
        0 AS PORC_IMPORTES    
        FROM tblgralventas AS gral
        INNER JOIN tblrenventas AS rv ON rv.REF_DOC = gral.REF_DOC 
        WHERE gral.FEC_REG >= CONVERT(DATETIME, convert(char(8), '+ @FecI + ' )) AND gral.FEC_REG < CONVERT(DATETIME,convert(char(8),' + @FecF + ' )) AND  convert(char(8),  COD_SUCU, 112) IN (''' + @sucursales + ''')
        GROUP BY rv.COD1_ART ,rv.COD_UND  ; 
          
           
      ELSE
       
       INSERT INTO @ProdAgrupacion
      SELECT rv.COD1_ART,
               COUNT(DISTINCT rv.REF_DOC) AS TICKETS,
               '''+ @unidad +''' AS UNIDAD_ART,
               SUM(rv.EQV_UND * rv.CAN_ART - rv.CAN_DEV)/(SELECT ucpa2.EQV_UND 
               FROM tblUndCosPreArt AS ucpa2 WHERE ucpa2.COD1_ART = rv.COD1_ART AND COD_UND =''' + @unidad + ''') AS UNIDADES,
               0 AS PORC_UNIDADES,
               SUM((((rv.CAN_ART * rv.PCIO_VEN) + rv.IMP1_REG + rv.IMP2_REG)/rv.CAN_ART) * (rv.CAN_ART - rv.CAN_DEV)) AS IMPORTE,
               0 AS PORC_IMPORTES     
        FROM tblgralventas AS gral
            INNER JOIN tblrenventas AS rv ON rv.REF_DOC = gral.REF_DOC
            INNER JOIN tblUndCosPreArt as un ON rv.COD1_ART = un.COD1_ART AND un.COD_UND = ''' + @unidad + '''
        WHERE gral.FEC_REG >= CONVERT(DATETIME, convert(char(8), '+ @FecI + ' )) AND gral.FEC_REG < CONVERT(DATETIME,convert(char(8),' + @FecF + ' )) AND convert(char(8),  COD_SUCU, 112) IN (''' + @sucursales + ''')
        GROUP BY rv.COD1_ART ,rv.COD_UND; 
        
    IF ('+ @agr +' <> '+ @vAgr +') 
     
    DELETE FROM @ProdAgrupacion
    WHERE COD1_ART NOT IN (SELECT COD1_ART FROM tblGpoArticulos WHERE COD_AGR = ' + @agr +'); 
	 

	IF ('''+ @unidad + ''' <> '''+ @Todas +''') 
	 
    		
    SET @TotalUnidades = (SELECT SUM(UNIDADES) FROM @ProdAgrupacion);
    SET @TotalImportes = (SELECT SUM(IMPORTE) FROM @ProdAgrupacion);
    UPDATE @ProdAgrupacion
    SET PORC_UNIDADES = (100*UNIDADES)/@TotalUnidades, PORC_IMPORTE = (100*IMPORTE)/@TotalImportes;	
      
     
   SELECT catAgr.DES_AGR AS Agrupacion,
         pa.COD1_ART AS Codigo,
         cat.DES1_ART AS Descripcion,
         pa.TICKETS AS Tickets,
         pa.UNIDAD_ART AS Unidad_Art, pa.UNIDADES AS Unidades,
	     ROUND(pa.PORC_UNIDADES,2) AS PorcentajeUnidades, pa.IMPORTE AS Importe,
         ROUND(pa.PORC_IMPORTE,2) AS PorcentajeImporte
	FROM @ProdAgrupacion AS pa
       INNER JOIN tblCatArticulos AS cat ON cat.COD1_ART = pa.COD1_ART
       INNER JOIN tblGpoArticulos AS ga ON ga.COD1_ART = pa.COD1_ART
       INNER JOIN tblCatAgrupacionArt AS catAgr ON ga.COD_AGR = catAgr.COD_AGR 
    ORDER BY catAgr.COD_AGR DESC;
       
     '    
     exec sp_executesql @consulta;   
 
    END         
    
    
------------------------------------------------------------------------------------------------------------------------------------------

GO
/****** Object:  StoredProcedure [dbo].[rptAgrupacionProductosMasVendidos]    Script Date: 28/04/2014 06:57:18 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[rptAgrupacionProductosMasVendidos]
 (
@FecI VARCHAR(30),
@FecF VARCHAR(30),
@agr VARCHAR(60),
@porcLimite VARCHAR(30),
@sucursales varchar(7999)
 
 
 ) 
AS
BEGIN

DECLARE @consulta NVARCHAR(4000);
 
DECLARE @Todas VARCHAR(10);
DECLARE @agrupacion VARCHAR(10);
DECLARE @top varchar(23)
SET @Todas = 'TODAS';
SET @agrupacion = '-1';
 
    DECLARE @Tlimite varchar(30);
    DECLARE @ConteoRegistros INT;
    DECLARE @TotalRegistros INT;
    DECLARE @TotalImportes DECIMAL(13,4);
    DECLARE @PorcLimiteAcumulado VARCHAR(30);                                                                                                                                                            
    DECLARE @Porc DECIMAL(13,4);                                                                                                                                
    DECLARE @TotalPorc DECIMAL(13,4);
    
 
        DECLARE @TempProductosMasVendidos TABLE( 
        
           	COD_AGR VARCHAR(30),
			COD1_ART VARCHAR(20),
			DES1_ART VARCHAR(60),                                                                                                                                                                    
			PRE_UNITARIO DECIMAL(13,4),
			IMPORTE DECIMAL(13,4),
			PORCENTAJE DECIMAL(13,4),
            TOTALAGRUP_IMPORTE DECIMAL(13,4),
			TOTALAGRUP_PORCENTAJE DECIMAL(13,4)
				
				)
     
       IF (@agr = '-1') 
         BEGIN      
       INSERT INTO @TempProductosMasVendidos
 	 
		SELECT   tblgpoarticulos.COD_AGR, tblrenventas.COD1_ART, tblCatArticulos.DES1_ART,
               SUM((tblrenventas.CAN_ART * tblrenventas.PCIO_VEN) + tblrenventas.IMP1_REG+tblrenventas.IMP2_REG)/tblrenventas.CAN_ART,
               SUM((((tblrenventas.CAN_ART * tblrenventas.PCIO_VEN) + tblrenventas.IMP1_REG+tblrenventas.IMP2_REG)/tblrenventas.CAN_ART)*(tblrenventas.CAN_ART - tblrenventas.CAN_DEV)) AS Importe,
			   0, 0 AS TotVentas, 0 AS TotPorc
		FROM tblgralventas 
		INNER JOIN tblrenventas    ON tblgralventas.REF_DOC = tblrenventas.REF_DOC 
		INNER JOIN tblgpoarticulos ON tblrenventas.COD1_ART = tblgpoarticulos.COD1_ART 
		INNER JOIN tblCatArticulos ON tblrenventas.COD1_ART = tblCatArticulos.COD1_ART
		WHERE (tblgralventas.FEC_REG BETWEEN   @FecI  AND   @FecF  ) AND  COD_SUCU in ( @sucursales ) -- AND tblgpoarticulos.COD_AGR = agr (NO ACTIVAR ES = -1!!!)
		GROUP BY tblgpoarticulos.COD_AGR,tblrenventas.COD1_ART ,tblCatArticulos.DES1_ART,tblrenventas.CAN_ART--, TotVentas, TotPorc    
		ORDER BY Importe DESC;
        END   
      ELSE
      BEGIN
      INSERT INTO @TempProductosMasVendidos
		SELECT   tblgpoarticulos.COD_AGR, tblrenventas.COD1_ART, tblCatArticulos.DES1_ART,
               SUM((tblrenventas.CAN_ART * tblrenventas.PCIO_VEN) + tblrenventas.IMP1_REG+tblrenventas.IMP2_REG)/tblrenventas.CAN_ART,
               SUM((((tblrenventas.CAN_ART * tblrenventas.PCIO_VEN) + tblrenventas.IMP1_REG+tblrenventas.IMP2_REG)/tblrenventas.CAN_ART)*(tblrenventas.CAN_ART - tblrenventas.CAN_DEV)) AS Importe,
			   0, 0 AS TotVentas, 0 AS TotPorc
		FROM tblgralventas 
		INNER JOIN tblrenventas    ON tblgralventas.REF_DOC = tblrenventas.REF_DOC 
		INNER JOIN tblgpoarticulos ON tblrenventas.COD1_ART = tblgpoarticulos.COD1_ART 
		INNER JOIN tblCatArticulos ON tblrenventas.COD1_ART = tblCatArticulos.COD1_ART
		WHERE (tblgralventas.FEC_REG BETWEEN     @FecI   AND   @FecF   ) AND COD_SUCU IN (@sucursales) AND tblgpoarticulos.COD_AGR =  @agr   
		GROUP BY tblgpoarticulos.COD_AGR,tblrenventas.COD1_ART ,tblCatArticulos.DES1_ART,tblrenventas.CAN_ART--, TotVentas, TotPorc    
		ORDER BY Importe DESC;
		 END
		
		 
     
	SET @TotalImportes = (SELECT SUM(IMPORTE) FROM @TempProductosMasVendidos);   
    UPDATE @TempProductosMasVendidos
    SET PORCENTAJE = (100*IMPORTE)/@TotalImportes, TOTALAGRUP_IMPORTE = @TotalImportes;
    SET @TotalPorc = (SELECT SUM(PORCENTAJE) FROM @TempProductosMasVendidos);
    UPDATE @TempProductosMasVendidos                                                                                                                                                                              
    SET TOTALAGRUP_PORCENTAJE = @TotalPorc;

    SET @TotalRegistros = 1;
    SET  @ConteoRegistros  = (SELECT COUNT(*) FROM @TempProductosMasVendidos);        
	SET @PorcLimiteAcumulado = 0;
		
	DECLARE cur1 CURSOR FOR SELECT PORCENTAJE FROM @TempProductosMasVendidos ORDER BY PORCENTAJE DESC; 	
	OPEN cur1  
	FETCH NEXT FROM cur1 INTO @Porc 
	WHILE @@FETCH_STATUS = 0
	BEGIN  
	    --SET @Tlimite = 80;
		SET @PorcLimiteAcumulado = @PorcLimiteAcumulado + @Porc;
		IF (@PorcLimiteAcumulado >= @porcLimite ) 
		BEGIN
			BREAK;
		END
		SET  @TotalRegistros  = @TotalRegistros + 1 ;
        
		IF(@TotalRegistros  =  @ConteoRegistros) 
		BEGIN
			BREAK;
		END
         
		FETCH NEXT FROM cur1 INTO @Porc
	END  
	CLOSE cur1  
	DEALLOCATE cur1 
		
 
		
		SELECT TOP (@TotalRegistros) COD_AGR AS Agrupacion, COD1_ART AS Codigo, DES1_ART AS Descripcion, 
        PRE_UNITARIO AS Pre_Unitario, IMPORTE AS Importe, PORCENTAJE AS Porcentaje,
        TOTALAGRUP_IMPORTE AS VentasTotales, TOTALAGRUP_PORCENTAJE AS PorcentajesTotales
		FROM @TempProductosMasVendidos
    --LIMIT TotalRegistros;
 
		 
 
 
    END    
   
GO
/****** Object:  StoredProcedure [dbo].[rptAgrupacionTop]    Script Date: 28/04/2014 06:57:18 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[rptAgrupacionTop]
 (
  @FecI VARCHAR(30), 
  @FecF VARCHAR(30),  
  @unidad VARCHAR(10), 
  @agr VARCHAR(30), 
  @sucursales VARCHAR(7999)
  
 
 
 ) 
AS
BEGIN
 DECLARE @consulta NVARCHAR(4000);
DECLARE @Todas VARCHAR(10);
DECLARE @vAgr VARCHAR(10);
SET @Todas = 'TODAS';
SET @vAgr = '-1';
SET @consulta = ' 
 DECLARE @TotalUnidades DECIMAL(13,4);
 DECLARE @TotalRegistros INT;
 DECLARE @TotalImportes DECIMAL(13,4);
 DECLARE @PorcLimiteAcumulado DECIMAL(13,4);
 DECLARE @Porc DECIMAL(13,4);
 DECLARE @TotalPorcUnidades DECIMAL(13,4); 
 DECLARE @Todas VARCHAR(10);
 
 
   
        DECLARE @ProdAgrupacion TABLE( 
        
                COD1_ART VARCHAR(20),
				TICKETS VARCHAR(300),
				UNIDAD_ART VARCHAR(10),
				UNIDADES DECIMAL(13,4),                                                                                                                                                            
				PORC_UNIDADES DECIMAL(13,4), 
				IMPORTE DECIMAL(13,4),
				PORC_IMPORTE DECIMAL(13,4))
     
        IF (''' + @unidad + ''' = '''+ @Todas +''') 
               
       INSERT INTO @ProdAgrupacion
       SELECT rv.COD1_ART, 
        COUNT(DISTINCT rv.REF_DOC) AS TICKETS,
        rv.COD_UND AS UNIDAD_ART,
        SUM(rv.CAN_ART - rv.CAN_DEV) AS UNIDADES,
        0 AS PORC_UNIDADES,
        SUM((((rv.CAN_ART * rv.PCIO_VEN) + rv.IMP1_REG + rv.IMP2_REG)/rv.CAN_ART) * (rv.CAN_ART - rv.CAN_DEV)) AS IMPORTE,
        0 AS PORC_IMPORTES    
        FROM tblgralventas AS gral
        INNER JOIN tblrenventas AS rv ON rv.REF_DOC = gral.REF_DOC 
        WHERE gral.FEC_REG >= CONVERT(DATETIME, convert(char(8), '+ @FecI + ' )) AND gral.FEC_REG < CONVERT(DATETIME,convert(char(8),' + @FecF + ' )) AND convert(char(8),  COD_SUCU, 112) IN (''' + @sucursales + ''')
        GROUP BY rv.COD1_ART ,rv.COD_UND; 
         
          
           
      ELSE
       
       INSERT INTO @ProdAgrupacion
      SELECT rv.COD1_ART,
               COUNT(DISTINCT rv.REF_DOC) AS TICKETS,
               '''+ @unidad +''' AS UNIDAD_ART,
               SUM(rv.EQV_UND * rv.CAN_ART - rv.CAN_DEV)/(SELECT ucpa2.EQV_UND 
               FROM tblUndCosPreArt AS ucpa2 WHERE ucpa2.COD1_ART = rv.COD1_ART AND COD_UND = ''' + @unidad + ''') AS UNIDADES,
               0 AS PORC_UNIDADES,
               SUM((((rv.CAN_ART * rv.PCIO_VEN) + rv.IMP1_REG + rv.IMP2_REG)/rv.CAN_ART) * (rv.CAN_ART - rv.CAN_DEV)) AS IMPORTE,
               0 AS PORC_IMPORTES     
        FROM tblgralventas AS gral
            INNER JOIN tblrenventas AS rv ON rv.REF_DOC = gral.REF_DOC
            INNER JOIN tblUndCosPreArt as un ON rv.COD1_ART = un.COD1_ART AND un.COD_UND = ''' + @unidad + '''
        WHERE gral.FEC_REG >= CONVERT(DATETIME, convert(char(8), '+ @FecI + ' )) AND gral.FEC_REG < CONVERT(DATETIME,convert(char(8),' + @FecF + ' )) AND convert(char(8),  COD_SUCU, 112) IN (''' + @sucursales + ''')
        GROUP BY rv.COD1_ART ,rv.COD_UND; 
        
    IF ('+ @agr +' <> '+ @vAgr +') 
     
    DELETE FROM @ProdAgrupacion
    WHERE COD1_ART NOT IN (SELECT COD1_ART FROM tblGpoArticulos WHERE COD_AGR = ' + @agr +'); 
	 

	IF ('''+ @unidad + ''' <> '''+ @Todas +''') 
    		
    SET @TotalUnidades = (SELECT SUM(UNIDADES) FROM @ProdAgrupacion);
    SET @TotalImportes = (SELECT SUM(IMPORTE) FROM @ProdAgrupacion);
    UPDATE @ProdAgrupacion
    SET PORC_UNIDADES = (100*UNIDADES)/@TotalUnidades, PORC_IMPORTE = (100*IMPORTE)/@TotalImportes;	
      
     
   SELECT TOP 5 catAgr.DES_AGR AS Agrupacion,
         pa.COD1_ART AS Codigo,
         cat.DES1_ART AS Descripcion,
         pa.TICKETS AS Tickets,
         pa.UNIDAD_ART AS Unidad_Art, pa.UNIDADES AS Unidades,
	     ROUND(pa.PORC_UNIDADES,2) AS PorcentajeUnidades, pa.IMPORTE AS Importe,
         ROUND(pa.PORC_IMPORTE,2) AS PorcentajeImporte
	FROM @ProdAgrupacion AS pa
       INNER JOIN tblCatArticulos AS cat ON cat.COD1_ART = pa.COD1_ART
       INNER JOIN tblGpoArticulos AS ga ON ga.COD1_ART = pa.COD1_ART
       INNER JOIN tblCatAgrupacionArt AS catAgr ON ga.COD_AGR = catAgr.COD_AGR 
    ORDER BY IMPORTE DESC;'
       
     exec sp_executesql @consulta;   
 
    END         
    
    
    
   
    
----------------------------------------------------------------------------------------------------------------------------------------------

GO
/****** Object:  StoredProcedure [dbo].[rptDiariosMes]    Script Date: 28/04/2014 06:57:18 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[rptDiariosMes]
 (
 @Año varchar(30),
 @Mes varchar(30), 
 @sucursales varchar(7999)
  
 ) 
AS
BEGIN
DECLARE @consulta NVARCHAR(4000);
SET @consulta = ' 
DECLARE @TotalNeto INT;
    DECLARE @TotalRegistros INT;
    DECLARE @TotalImportes DECIMAL(13,4);
    DECLARE @PorcLimiteAcumulado DECIMAL(13,4);
    DECLARE @Porc DECIMAL(13,4);
    DECLARE @TotalPorcUnidades DECIMAL(13,4);
 
 
   
        DECLARE @GenDiariasMes TABLE( 
        
            DIA VARCHAR(6),
			TICKETS VARCHAR(40),
			SUB_TOTAL DECIMAL(13,4),      
            IMPUESTO DECIMAL(13,4), 
            TOTAL DECIMAL(13,4),
            DEVOLUCIONES DECIMAL(13,4),
            NETO DECIMAL(13,4),                                                                                                                                                            
			PORCENTAJE DECIMAL(13,4) )
 
               
        INSERT INTO @GenDiariasMes
       
		SELECT day(tblgralventas.FEC_REG),COUNT(tblgralventas.TOT_DOC),SUM(tblgralventas.SUB_DOC),SUM(tblgralventas.IVA_DOC),SUM(tblgralventas.TOT_DOC), 
		ISNULL(sum(tblEncDevolucion.TOT_DEV),0), ISNULL(sum(tblgralventas.TOT_DOC-tblEncDevolucion.TOT_DEV),SUM(tblgralventas.TOT_DOC)), 0 AS PORCENTAJE
		FROM tblgralventas left JOIN tblEncDevolucion ON tblgralventas.REF_DOC = tblEncDevolucion.REF_DOC
		where year(tblgralventas.FEC_REG)='+@Año+' and month(tblgralventas.FEC_REG)='+@Mes+' AND convert(char(8), tblgralventas.COD_SUCU, 112) IN (''' + @sucursales + ''') 
		GROUP BY  day(tblgralventas.FEC_REG)   
		ORDER BY day(tblgralventas.FEC_REG) ASC;   
		        
		
         		
		SET @TotalNeto = (SELECT SUM(NETO) FROM @GenDiariasMes);
		 
        UPDATE @GenDiariasMes
        SET PORCENTAJE = (100*NETO)/@TotalNeto;
	
		SELECT   DIA AS DIA, TICKETS AS TICKETS, SUB_TOTAL AS SUBTOTAL,IMPUESTO AS IMPUESTO,TOTAL AS TOTAL, DEVOLUCIONES AS DEVOLUCIONES,
        NETO AS NETO, ROUND(PORCENTAJE,2) AS  PORCENTAJE
		FROM @GenDiariasMes; '      
	 
   exec sp_executesql @consulta;   
 
    END         
        
    
    
--------------------------------------------------------------------------------------------------------------------------------------------

GO
/****** Object:  StoredProcedure [dbo].[rptGenDiaSemana]    Script Date: 28/04/2014 06:57:18 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[rptGenDiaSemana]
 (
  @fecha varchar(30),  
  @sucursales varchar(7999)
 )
AS
BEGIN
DECLARE @consulta NVARCHAR(4000);
SET @consulta = '  
SELECT day(tblgralventas.FEC_REG) as Dia ,COUNT(*) as Tickets,SUM(tblgralventas.SUB_DOC) as Subtotal,SUM(tblgralventas.IVA_DOC) as Impuesto,SUM(tblgralventas.TOT_DOC) as Total,ISNULL(SUM(tblEncDevolucion.TOT_DEV),0) as Devoluciones    
FROM tblgralventas left JOIN tblEncDevolucion ON tblgralventas.REF_DOC = tblEncDevolucion.REF_DOC 
where tblgralventas.FEC_REG=Convert(DATETIME, LEFT(' + @fecha + ', 8)) AND   convert(char(8), tblgralventas.COD_SUCU, 112)  IN (''' + @sucursales + ''')                          
GROUP BY  day(tblgralventas.FEC_REG); '
 
                                                                                                                                                                                                     
     exec sp_executesql @consulta;   
 
END      
--------------------------------------------------------------------------------------------------------------------------------------------------

GO
/****** Object:  StoredProcedure [dbo].[rptGenMensuales]    Script Date: 28/04/2014 06:57:18 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[rptGenMensuales]
 (
 @FecI varchar(50),
 @FecF varchar(50),
 @sucursales varchar(7999)
  
 ) 
AS
BEGIN
DECLARE @consulta NVARCHAR(4000);
SET @consulta = ' 
DECLARE @TotalNeto INT;
    DECLARE @TotalRegistros INT;
    DECLARE @TotalImportes DECIMAL(13,4);
    DECLARE @PorcLimiteAcumulado DECIMAL(13,4);
    DECLARE @Porc DECIMAL(13,4);
    DECLARE @TotalPorcUnidades DECIMAL(13,4);
 
 
   
        DECLARE @GenMes TABLE( 
        
            AÑO VARCHAR(6),
            MES VARCHAR(6),
			TICKETS VARCHAR(300),
			SUB_TOTAL DECIMAL(13,4),      
            IMPUESTO DECIMAL(13,4), 
            TOTAL DECIMAL(13,4),
            DEVOLUCIONES DECIMAL(13,4),
            NETO DECIMAL(13,4),                                                                                                                                                            
			PORCENTAJE DECIMAL(13,4) 
  )
 INSERT INTO @GenMes 

 SELECT  year(tblgralventas.FEC_REG), month(tblgralventas.FEC_REG), COUNT(tblgralventas.REF_DOC),SUM(tblgralventas.SUB_DOC),SUM(tblgralventas.IVA_DOC),
 SUM(tblgralventas.TOT_DOC), 
 ISNULL(sum(tblEncDevolucion.TOT_DEV),0),ISNULL(SUM(tblgralventas.TOT_DOC)-SUM(tblEncDevolucion.TOT_DEV),SUM(tblgralventas.TOT_DOC)), 0 AS PORCENTAJE
 FROM tblgralventas left JOIN tblEncDevolucion ON tblgralventas.REF_DOC = tblEncDevolucion.REF_DOC
 WHERE   tblgralventas.FEC_REG >= CONVERT(DATETIME, convert(char(8), '+ @FecI + ' )) AND tblgralventas.FEC_REG < CONVERT(DATETIME,convert(char(8),' + @FecF + ' )) AND convert(char(8),  tblgralventas.COD_SUCU, 112) IN (''' + @sucursales + ''')                                                                              
 GROUP BY year(tblgralventas.FEC_REG),month(tblgralventas.FEC_REG)
 ORDER BY  month(tblgralventas.FEC_REG) ASC;                                                                                                                                                                     
                                                                                                                                                                       
         		
		SET @TotalNeto = (SELECT SUM(NETO) FROM @GenMes);
		 
        UPDATE @GenMes
        SET PORCENTAJE = (100*NETO)/@TotalNeto;
	
		SELECT   AÑO AS ANO, MES AS MES,TICKETS AS TICKETS, SUB_TOTAL AS SUBTOTAL,IMPUESTO AS IMPUESTO,TOTAL AS TOTAL, DEVOLUCIONES AS DEVOLUCIONES,
        NETO AS NETO, ROUND(PORCENTAJE,2) AS  PORCENTAJE
		FROM @GenMes;                                                                                                                                                                                           
        
           
   '    
     exec sp_executesql @consulta;   
 
    END         
    
    
   
---------------------------------------------------------------------------------------------------------------------------------------------------

GO
/****** Object:  StoredProcedure [dbo].[rptGenPorHoras]    Script Date: 28/04/2014 06:57:18 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[rptGenPorHoras]
 (
 @FecI varchar(30),
 @FecF varchar(30),
 @sucursales varchar(7999)
  
 ) 
AS
BEGIN
DECLARE @consulta NVARCHAR(4000);
SET @consulta = '

DECLARE @TotalNeto INT;
 DECLARE @TotalRegistros INT;
    DECLARE @TotalImportes DECIMAL(13,4);
    DECLARE @PorcLimiteAcumulado DECIMAL(13,4);
    DECLARE @Porc DECIMAL(13,4);
    DECLARE @TotalPorcUnidades DECIMAL(13,4);
 
 
   
        DECLARE @GenPorHoras  TABLE( 
      	HORA VARCHAR(6),
			TICKETS VARCHAR(300),
			SUB_TOTAL DECIMAL(13,4),      
            IMPUESTO DECIMAL(13,4), 
            TOTAL DECIMAL(13,4),
            DEVOLUCIONES DECIMAL(13,4),
            NETO DECIMAL(13,4),                                                                                                                                                            
			PORCENTAJE DECIMAL(13,4) 
			 			 
  )
 INSERT INTO @GenPorHoras  

  
SELECT DATEPART(hh,tblgralventas.HORA_REG),COUNT(*),SUM(tblgralventas.SUB_DOC),SUM(tblgralventas.IVA_DOC),
SUM(tblgralventas.TOT_DOC),ISNULL(SUM(tblEncDevolucion.TOT_DEV),0) ,ISNULL(Sum(tblgralventas.TOT_DOC)-SUM(tblEncDevolucion.TOT_DEV),SUM(tblgralventas.TOT_DOC)), 0 AS PORCENTAJE 
FROM tblgralventas LEFT JOIN tblEncDevolucion ON tblgralventas.REF_DOC = tblEncDevolucion.REF_DOC -- AND tblgralventas.FEC_REG=tblEncDevolucion.FEC_REG
WHERE  (tblgralventas.FEC_REG)  between Convert(DATETIME, LEFT(' + @FecI + ', 8)) and Convert(DATETIME, LEFT(' + @FecF + ', 8)) AND  convert(char(8), tblgralventas.COD_SUCU, 112)  IN (''' + @sucursales + ''') AND DATEPART(hh,tblgralventas.HORA_REG) BETWEEN 0 AND 23
GROUP BY  DATEPART(hh,tblgralventas.HORA_REG)
ORDER by  DATEPART(hh,tblgralventas.HORA_REG) ;                                                                                                                                                                                                              

         		
		SET @TotalNeto = (SELECT SUM(NETO) FROM @GenPorHoras);
		 
        UPDATE @GenPorHoras
        SET PORCENTAJE = (100*NETO)/@TotalNeto;
	
		SELECT   HORA AS HORA, TICKETS AS TICKETS, SUB_TOTAL AS SUBTOTAL,IMPUESTO AS IMPUESTO,TOTAL AS TOTAL, DEVOLUCIONES AS DEVOLUCIONES,
        NETO AS NETO, ROUND(PORCENTAJE,2) AS  PORCENTAJE
		FROM @GenPorHoras; '                                                                                                                                                                                                                                                                                   

        
           
  exec sp_executesql @consulta;   
 
END    
    
--------------------------------------------------------------------------------------------------------------------------------------------------

GO
/****** Object:  StoredProcedure [dbo].[rptGenPorTurnos]    Script Date: 28/04/2014 06:57:18 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[rptGenPorTurnos]
 (
 @FecI varchar(50),
 @FecF varchar(50),
 @sucursales varchar(7999)
  
 ) 
AS
BEGIN
DECLARE @consulta NVARCHAR(4000);
SET @consulta = '
DECLARE @TotalTotal INT;
    DECLARE @TotalRegistros INT;
    DECLARE @TotalImportes DECIMAL(13,4);
    DECLARE @PorcLimiteAcumulado DECIMAL(13,4);
    DECLARE @Porc DECIMAL(13,4);
    DECLARE @TotalPorcUnidades DECIMAL(13,4);
 
   
        DECLARE @GenPorTurno  TABLE( 
        
            TURNO VARCHAR(6),
			TICKETS VARCHAR(300),
			SUB_TOTAL DECIMAL(13,4),      
            IMPUESTO DECIMAL(13,4), 
            TOTAL DECIMAL(13,4),
            PORCENTAJE DECIMAL(13,4) 
  )
 INSERT INTO @GenPorTurno  

SELECT tblgralventas.CAJA_TUR,COUNT(*),SUM(tblgralventas.SUB_DOC),
SUM(tblgralventas.IVA_DOC),SUM(tblgralventas.TOT_DOC), 0 AS PORCENTAJE  
FROM tblgralventas 
WHERE   tblgralventas.FEC_REG BETWEEN Convert(DATETIME, LEFT(' + @FecI + ', 8)) and Convert(DATETIME, LEFT(' + @FecF + ', 8)) AND  convert(char(8), tblgralventas.COD_SUCU, 112)  IN (''' + @sucursales + ''')  AND tblgralventas.CAJA_TUR BETWEEN 1 AND 3 
GROUP BY tblgralventas.CAJA_TUR                                                                                                            
ORDER BY tblgralventas.CAJA_TUR ASC;                                                                                                                               

        		
		SET @TotalTotal = (SELECT SUM(TOTAL) FROM @GenPorTurno);
		 
        UPDATE @GenPorTurno
        SET PORCENTAJE = (100*TOTAL)/@TotalTotal;
	
		SELECT   TURNO AS TURNO, TICKETS AS TICKETS, SUB_TOTAL AS SUBTOTAL,IMPUESTO AS IMPUESTO,TOTAL AS TOTAL,
        ROUND(PORCENTAJE,2) AS  PORCENTAJE
		FROM @GenPorTurno; '                                                                                                                                                                                                                                                                                    
           
  exec sp_executesql @consulta;   
 
END   
---------------------------------------------------------------------------------------------------------------------------------------------------

GO
/****** Object:  StoredProcedure [dbo].[rptProductosPorProveedor]    Script Date: 28/04/2014 06:57:18 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[rptProductosPorProveedor]
 (
@FecI VARCHAR(30), 
@FecF VARCHAR(30), 
@proveedor VARCHAR(10),
@codUnidad VARCHAR(4),
@porcLimite VARCHAR(4), 
@sucursales varchar(7999)
  
 ) 
AS
BEGIN
DECLARE @Todas VARCHAR(10);
DECLARE @prov VARCHAR(10);
 
DECLARE @consulta NVARCHAR(4000);
 
 
    DECLARE @TotalUnidades DECIMAL(13,4);
    DECLARE @TotalRegistros DECIMAL(13,4);
    DECLARE @TotalImportes DECIMAL(13,4);
    DECLARE @PorcLimiteAcumulado DECIMAL(13,4);
    DECLARE @Porc DECIMAL(13,4);
    DECLARE @TotalPorcUnidades DECIMAL(13,4);
	 
 
   IF (  @proveedor   = '-1') 
   BEGIN
        DECLARE @ProdPorProveedor TABLE( 
            NOM_PROV VARCHAR(100),
			TICKETS VARCHAR(300),
            UNIDAD_ART DECIMAL(13,4),                                                                                                                                                                                               
            UNIDADES DECIMAL(13,4),                                                                                                                                                            
			PORC_UNIDADES DECIMAL(13,4), 
			IMPORTE DECIMAL(13,4),                                                                                                                                                                
			PORC_IMPORTE DECIMAL(13,4)
  )
 INSERT INTO @ProdPorProveedor
 SELECT cp.NOM_PROV,COUNT(DISTINCT(rv.REF_DOC)),
 (SELECT SUM(rv.EQV_UND * rv.CAN_ART) / SUM(ucpa2.EQV_UND) FROM tblUndCosPreArt AS ucpa2 WHERE ucpa2.COD1_ART = rv.COD1_ART AND ucpa2.COD_UND = @codUnidad ) AS EQV_UND2,
   SUM(rv.CAN_ART - rv.CAN_DEV) AS UNIDADES, 0 AS PORC_UNIDADES,
   SUM((((rv.CAN_ART * rv.PCIO_VEN) + rv.IMP1_REG + rv.IMP2_REG)/rv.CAN_ART) * (rv.CAN_ART - rv.CAN_DEV)) AS IMPORTE,
			   0 AS PORC_IMPORTE
		  FROM tblCatProveedor AS cp
            INNER JOIN tblArtiProveedor AS ap ON cp.COD_PROV = ap.COD_PROV -- AND cp.COD_PROV = 3
            INNER JOIN tblRenVentas AS rv ON ap.COD1_ART = rv.COD1_ART
            INNER JOIN tblGralVentas AS gv ON rv.REF_DOC = gv.REF_DOC
            INNER JOIN tblGpoArticulos AS ga ON rv.COD1_ART = ga.COD1_ART 
            INNER JOIN tblCatArticulos AS ca ON rv.COD1_ART = ca.COD1_ART 
            INNER JOIN tblUndCosPreArt AS ucpa ON ca.COD1_ART = ucpa.COD1_ART AND ucpa.COD_UND = @codUnidad
		WHERE gv.FEC_REG BETWEEN @FecI AND @FecF AND  COD_SUCU IN ( @sucursales) -- AND rv.COD1_ART = proveedor
		GROUP BY cp.NOM_PROV, rv.COD1_ART, ca.DES1_ART 
		ORDER BY Importe DESC  ;                                                                                                                            
		                                                                                                                            
	                                                                                                                          
		 
 	
		SET @TotalUnidades = (SELECT SUM(UNIDADES) FROM @ProdPorProveedor);
		SET @TotalImportes = (SELECT SUM(IMPORTE) FROM @ProdPorProveedor);
        UPDATE @ProdPorProveedor
        SET PORC_UNIDADES = (100*UNIDADES)/@TotalUnidades, PORC_IMPORTE = (100*IMPORTE)/@TotalImportes;
	
		SELECT  NOM_PROV AS Proveedor,TICKETS as Tickets,UNIDAD_ART as Unidad_Art, UNIDADES as Unidades,
        PORC_UNIDADES AS PorcentajeUnidades, IMPORTE AS Importe, PORC_IMPORTE AS PorcentajeImporte
		FROM @ProdPorProveedor;
		END
ELSE
BEGIN
   DECLARE @ProdPorProveedorDos TABLE( 
           	NOM_PROV VARCHAR(100),
			COD1_ART VARCHAR(20),
			DES1_ART VARCHAR(60),      
            TICKETS DECIMAL(13,4), 
            UNIDAD_ART DECIMAL(13,4),
            UNIDADES DECIMAL(13,4),                                                                                                                                                            
			PORC_UNIDADES DECIMAL(13,4), 
			IMPORTE DECIMAL(13,4),
			PORC_IMPORTE DECIMAL(13,4)
  )
INSERT INTO @ProdPorProveedorDos
SELECT cp.NOM_PROV, rv.COD1_ART, ca.DES1_ART, COUNT(DISTINCT(rv.REF_DOC)),
 (SELECT  sum(rv.EQV_UND * rv.CAN_ART) / ucpa2.EQV_UND FROM tblUndCosPreArt AS ucpa2 WHERE ucpa2.COD1_ART = rv.COD1_ART AND ucpa2.COD_UND = @codUnidad ) AS EQV_UND2,
               SUM(rv.CAN_ART - rv.CAN_DEV) AS UNIDADES, 0 AS PORC_UNIDADES,
               SUM((((rv.CAN_ART * rv.PCIO_VEN) + rv.IMP1_REG + rv.IMP2_REG)/rv.CAN_ART) * (rv.CAN_ART - rv.CAN_DEV)) AS IMPORTE,
			   0 AS PORC_IMPORTE
		  FROM tblCatProveedor AS cp
            INNER JOIN tblArtiProveedor AS ap ON cp.COD_PROV = ap.COD_PROV -- AND cp.COD_PROV = 3
            INNER JOIN tblRenVentas AS rv ON ap.COD1_ART = rv.COD1_ART
            INNER JOIN tblGralVentas AS gv ON rv.REF_DOC = gv.REF_DOC
            INNER JOIN tblGpoArticulos AS ga ON rv.COD1_ART = ga.COD1_ART 
            INNER JOIN tblCatArticulos AS ca ON rv.COD1_ART = ca.COD1_ART 
            INNER JOIN tblUndCosPreArt AS ucpa ON ca.COD1_ART = ucpa.COD1_ART AND ucpa.COD_UND = @codUnidad
		WHERE (gv.FEC_REG BETWEEN @FecI AND @FecF) AND  COD_SUCU IN (@sucursales)  AND cp.COD_PROV = @proveedor
		GROUP BY cp.NOM_PROV, rv.COD1_ART, ca.DES1_ART  
		ORDER BY Importe DESC;                                                                                                                              
		                                                                                                                           
		 
		    	
		SET @TotalUnidades = (SELECT SUM(UNIDADES) FROM @ProdPorProveedorDos);
		SET @TotalImportes = (SELECT SUM(IMPORTE) FROM @ProdPorProveedorDos);
        UPDATE @ProdPorProveedorDos
        SET PORC_UNIDADES = (100*UNIDADES)/@TotalUnidades, PORC_IMPORTE = (100*IMPORTE)/@TotalImportes;
	
		SELECT  NOM_PROV AS Proveedor, COD1_ART AS Codigo, DES1_ART AS Descripcion,TICKETS as Tickets,UNIDAD_ART as Unidad_Art, UNIDADES as Unidades,
        PORC_UNIDADES AS PorcentajeUnidades, IMPORTE AS Importe, PORC_IMPORTE AS PorcentajeImporte
		FROM @ProdPorProveedorDos;
END
 
 
END
GO
/****** Object:  StoredProcedure [dbo].[rptResumenMensual]    Script Date: 28/04/2014 06:57:18 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[rptResumenMensual]
 (
@año varchar(30),
@sucursales varchar(7999)
  
 ) 
AS
BEGIN
DECLARE @consulta NVARCHAR(4000);
SET @consulta = '
DECLARE @TotalTickets INT;
DECLARE @TotalVentas INT;
    DECLARE @TotalRegistros INT;
    DECLARE @TotalImportes DECIMAL(13,4);
    DECLARE @PorcLimiteAcumulado DECIMAL(13,4);
    DECLARE @Porc DECIMAL(13,4);
    DECLARE @TotalPorcUnidades DECIMAL(13,4);
 
   
        DECLARE @TicketsPromedio TABLE( 
            AÑO VARCHAR(6),
		    MES VARCHAR(6),
			TICKETS DECIMAL(13,4),
            PORC_TICKETS DECIMAL(13,4), 
			TICKETS_PROM DECIMAL(13,4),      
            VENTAS DECIMAL(13,4), 
            PORC_VENTAS DECIMAL(13,4),
            VENTAS_PROM DECIMAL(13,4) 
  )
 INSERT INTO @TicketsPromedio

SELECT  YEAR(tblgralventas.FEC_REG) as Ano , MONTH(tblgralventas.FEC_REG), COUNT(tblgralventas.REF_DOC),0 AS PORC_TICKETS,0 AS TICKETS_PROM,ISNULL(SUM(tblgralventas.TOT_DOC)-SUM(tblEncDevolucion.TOT_DEV),SUM(tblgralventas.TOT_DOC)),
 0 AS PORC_VENTAS,0 AS VENTAS_PROM   
 FROM tblgralventas left JOIN tblEncDevolucion ON tblgralventas.REF_DOC = tblEncDevolucion.REF_DOC
 WHERE  YEAR(tblgralventas.FEC_REG)='+@año+' AND  convert(char(8), tblgralventas.COD_SUCU, 112)  IN (''' + @sucursales + ''')                                                                                                    
 GROUP BY YEAR(tblgralventas.FEC_REG),MONTH(tblgralventas.FEC_REG)  
 ORDER BY MONTH(tblgralventas.FEC_REG) ASC;
                                                                                                   
                                                                                                                              
 	
		SET @TotalTickets = (SELECT SUM(TICKETS) FROM @TicketsPromedio );
		 
        UPDATE @TicketsPromedio
        SET PORC_TICKETS = (100*TICKETS)/@TotalTickets;

        SET @TotalVentas = (SELECT SUM(VENTAS) FROM @TicketsPromedio );
		 
        UPDATE @TicketsPromedio
        SET PORC_VENTAS = (100*VENTAS)/@TotalVentas;
 		SELECT   AÑO AS ANO, MES AS MES, TICKETS AS TICKETS,ROUND(PORC_TICKETS,2) AS TPORCENTAJE,TICKETS_PROM AS TPROMEDIO,VENTAS AS VENTAS,ROUND(PORC_VENTAS,2) as VPORCENTAJE, VENTAS_PROM AS VPROMEDIO
		FROM @TicketsPromedio;'                                                                                                                                                                                                                                                                                     

                                                                                                                                                                                                      
     exec sp_executesql @consulta;   
 
END
GO
/****** Object:  StoredProcedure [dbo].[rptTicketDiaSemana]    Script Date: 28/04/2014 06:57:18 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[rptTicketDiaSemana]
(
@years varchar(10),
@sucursales varchar(7999)
) 
AS
BEGIN
DECLARE @consulta NVARCHAR(4000);
SET @consulta = '

DECLARE @Devoluciones decimal(13,4);
SET @Devoluciones = ISNULL((select SUM(TOT_DEV) FROM tblEncDevolucion WHERE FEC_REG= Convert(DATETIME, LEFT(' + @years + ', 8))),0);
SELECT COUNT(REF_DOC) AS Tickets,SUM(tblgralventas.TOT_DOC)-@Devoluciones AS Ventas FROM tblgralventas WHERE tblgralventas.FEC_REG= Convert(DATETIME, LEFT(' + @years + ', 8))  AND convert(char(8), tblgralventas.COD_SUCU, 112)  IN (''' + @sucursales + ''')  GROUP BY  day(tblgralventas.FEC_REG);'
  
                                                                                                                                                                                                      
     exec sp_executesql @consulta;   
 
END  
GO
/****** Object:  StoredProcedure [dbo].[rptTicketsDiariosMes]    Script Date: 28/04/2014 06:57:18 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[rptTicketsDiariosMes]
 (
   @Año varchar(30),
   @Mes varchar(30), 
   @sucursales varchar(7999)
  
 ) 
AS
BEGIN
DECLARE @consulta NVARCHAR(4000);
SET @consulta = '

DECLARE @TotalTickets INT;
    DECLARE @TotalRegistros INT;
    DECLARE @TotalImportes DECIMAL(13,4);
    DECLARE @PorcLimiteAcumulado DECIMAL(13,4);
    DECLARE @Porc DECIMAL(13,4);
    DECLARE @TotalPorcUnidades DECIMAL(13,4);
 
   
    DECLARE @TicketsDiariasMes  TABLE( 
        
          	DIA VARCHAR(6),
			TICKETS DECIMAL(13,4),
			PORCENTAJE DECIMAL(13,4),      
            VENTAS DECIMAL(13,4), 
            PROM_TICKET DECIMAL(13,4)
  )
 INSERT INTO @TicketsDiariasMes  

 SELECT day(tblgralventas.FEC_REG),COUNT(tblgralventas.TOT_DOC),0 AS PORCENTAJE,ISNULL(SUM(tblgralventas.TOT_DOC)-SUM(tblEncDevolucion.TOT_DEV),SUM(tblgralventas.TOT_DOC)), 
 0 AS PROM_TICKET
FROM tblgralventas left JOIN tblEncDevolucion ON tblgralventas.REF_DOC = tblEncDevolucion.REF_DOC
where year(tblgralventas.FEC_REG)='+@Año+' and month(tblgralventas.FEC_REG)='+@Mes+' AND  convert(char(8), tblgralventas.COD_SUCU, 112) IN (''' + @sucursales + ''')
GROUP BY  day(tblgralventas.FEC_REG)
ORDER BY day(tblgralventas.FEC_REG) ASC;                                                                                                                               

      		
		SET @TotalTickets = (SELECT SUM(TICKETS) FROM @TicketsDiariasMes);
		 
        UPDATE @TicketsDiariasMes
        SET PORCENTAJE = (100*TICKETS)/@TotalTickets;
	
		SELECT   DIA AS DIA, TICKETS AS TICKETS,ROUND(PORCENTAJE,2) AS  PORCENTAJE,VENTAS AS VENTAS,PROM_TICKET AS PROMEDIO 
		FROM @TicketsDiariasMes; '                                                                                                                                                                                          
               
      
    exec sp_executesql @consulta;        
    END     
GO
/****** Object:  StoredProcedure [dbo].[rptTicketsPorHoras]    Script Date: 28/04/2014 06:57:18 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[rptTicketsPorHoras]
 (
 @FecI varchar(30),
 @FecF varchar(30),
 @sucursales varchar(7999)
  
 ) 
AS
BEGIN
DECLARE @consulta NVARCHAR(4000);
SET @consulta = '
DECLARE @TotalTickets INT;
    DECLARE @TotalRegistros INT;
    DECLARE @TotalImportes DECIMAL(13,4);
    DECLARE @PorcLimiteAcumulado DECIMAL(13,4);
    DECLARE @Porc DECIMAL(13,4);
    DECLARE @TotalPorcUnidades DECIMAL(13,4);
 
 
   
        DECLARE @TicketsPorHoras  TABLE( 
      	HORA VARCHAR(6),
			TICKETS DECIMAL(13,4),
			PORC_TICKETS DECIMAL(13,4),      
            VENTAS DECIMAL(13,4),
            PROM_TICKET DECIMAL(13,4)
			 			 
  )
 INSERT INTO @TicketsPorHoras 
 

SELECT DATEPART(hh,tblgralventas.HORA_REG),count(DISTINCT (tblgralventas.REF_DOC)),0 AS PORC_TICKETS,ISNULL(SUM(tblgralventas.TOT_DOC) - SUM(tblEncDevolucion.TOT_DEV),SUM(tblgralventas.TOT_DOC)),0 AS PROM_TICKET   
FROM tblgralventas left JOIN tblEncDevolucion ON tblgralventas.REF_DOC = tblEncDevolucion.REF_DOC 
WHERE tblgralventas.FEC_REG BETWEEN Convert(DATETIME, LEFT(' + @FecI + ', 8)) and Convert(DATETIME, LEFT(' + @FecF + ', 8)) AND convert(char(8), tblgralventas.COD_SUCU, 112) IN (''' + @sucursales + ''') AND DATEPART(hh,tblgralventas.HORA_REG) between 0 and 23
GROUP BY  DATEPART(hh,tblgralventas.HORA_REG)
ORDER BY DATEPART(hh,tblgralventas.HORA_REG) ASC;
                                                                                                                                      
        --SET SQL_SAFE_UPDATES = 0;		
		SET @TotalTickets = (SELECT SUM(TICKETS) FROM @TicketsPorHoras);                                                                                                                                                                 
		 
        UPDATE @TicketsPorHoras
        SET PORC_TICKETS = (100*TICKETS)/@TotalTickets;
	
		SELECT   HORA, TICKETS,PORC_TICKETS,VENTAS,ROUND(VENTAS/TICKETS,2) AS PROMEDIO 
        FROM @TicketsPorHoras;                                                                                                                                                                                                                                                                                     

        --DROP TABLE TicketsPorHoras; 
  '
                                                                                                                                                                                                                                                                                    

 exec sp_executesql @consulta;        
    END  
GO
/****** Object:  StoredProcedure [dbo].[rptTicketsPorTurnos]    Script Date: 28/04/2014 06:57:18 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[rptTicketsPorTurnos]
 (
 @FecI varchar(30),
 @FecF varchar(30),
 @sucursales varchar(7999)
  
 ) 
AS
BEGIN
DECLARE @consulta NVARCHAR(4000);
SET @consulta = '
DECLARE @TotalTickets INT;
    DECLARE @TotalRegistros INT;
    DECLARE @TotalImportes DECIMAL(13,4);
    DECLARE @PorcLimiteAcumulado DECIMAL(13,4);
    DECLARE @Porc DECIMAL(13,4);
    DECLARE @TotalPorcUnidades DECIMAL(13,4);
 
   
        DECLARE @TicketsPorTurno  TABLE( 
      	TURNO VARCHAR(6),
			TICKETS DECIMAL(13,4),
			PORCENTAJE DECIMAL(13,4),      
            VENTAS DECIMAL(13,4), 
            PROM DECIMAL(13,4) 
			 			 
  )
 INSERT INTO @TicketsPorTurno 
 

SELECT tblgralventas.CAJA_TUR,count(*), 0 AS PORCENTAJE,
SUM(tblgralventas.TOT_DOC), 0 AS PROM 
FROM tblgralventas 
WHERE   tblgralventas.FEC_REG BETWEEN Convert(DATETIME, LEFT(' + @FecI + ', 8)) and Convert(DATETIME, LEFT(' + @FecF + ', 8))  AND convert(char(8),tblgralventas.COD_SUCU, 112) IN (''' + @sucursales + ''') AND tblgralventas.CAJA_TUR BETWEEN 1 AND 3 
GROUP BY tblgralventas.CAJA_TUR   
ORDER BY tblgralventas.CAJA_TUR ASC;                                                                                                         
;                                                                                                                               

        --SET SQL_SAFE_UPDATES = 0;		
		SET @TotalTickets = (SELECT SUM(TICKETS) FROM @TicketsPorTurno );
		 
        UPDATE @TicketsPorTurno
        SET PORCENTAJE = (100*TICKETS)/@TotalTickets;
	
		SELECT   TURNO AS TURNO, TICKETS AS TICKETS, ROUND(PORCENTAJE,2) AS PORCENTAJE,VENTAS AS VENTAS,ROUND(VENTAS/TICKETS,2) AS PROMEDIO
		FROM @TicketsPorTurno ;                                                                                                                                                                                                                                                                                     

        --DROP TABLE TicketsPorTurno ; 
  
                                                                                                                                                                                                                                                                                    
  '
                                                                                                                                                                                                                                                                                    

 exec sp_executesql @consulta;        
    END   
GO
/****** Object:  StoredProcedure [dbo].[rptTicketsPromedio]    Script Date: 28/04/2014 06:57:18 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[rptTicketsPromedio]
 (
 @año varchar(30), 
 @sucursales varchar(7999)
  
 ) 
AS
BEGIN
DECLARE @consulta NVARCHAR(4000);
SET @consulta = '
DECLARE @TotalTickets INT;
    DECLARE @TotalRegistros INT;
    DECLARE @TotalImportes DECIMAL(13,4);
    DECLARE @PorcLimiteAcumulado DECIMAL(13,4);
    DECLARE @Porc DECIMAL(13,4);
    DECLARE @TotalPorcUnidades DECIMAL(13,4);
 
   
        DECLARE @TicketsPromedio  TABLE( 
      	AÑO VARCHAR(6),
		    MES VARCHAR(6),
			TICKETS DECIMAL(13,4),
            PORC_TICKETS DECIMAL(13,4), 
			TICKETS_PROM DECIMAL(13,4),      
            VENTAS DECIMAL(13,4), 
            PORC_VENTAS DECIMAL(13,4),
            VENTAS_PROM DECIMAL(13,4) 
			 			 
  )
 INSERT INTO @TicketsPromedio 
 SELECT  YEAR(tblgralventas.FEC_REG) as Ano , month(tblgralventas.FEC_REG), COUNT(tblgralventas.REF_DOC),0 AS PORC_TICKETS,0 AS TICKETS_PROM,ISNULL(SUM(tblgralventas.TOT_DOC)-SUM(tblEncDevolucion.TOT_DEV),SUM(tblgralventas.TOT_DOC)),
 0 AS PORC_VENTAS,0 AS VENTAS_PROM   
 FROM tblgralventas left JOIN tblEncDevolucion ON tblgralventas.REF_DOC = tblEncDevolucion.REF_DOC
 WHERE  YEAR(tblgralventas.FEC_REG)='+@año+' AND convert(char(8),tblgralventas.COD_SUCU, 112) IN (''' + @sucursales + ''')                                                                                                     
 GROUP BY YEAR(tblgralventas.FEC_REG),MONTH(tblgralventas.FEC_REG) 
 ORDER BY month(tblgralventas.FEC_REG)ASC; 
                                                                                                   
                                                                                                                              

        --SET SQL_SAFE_UPDATES = 0;		
		SET @TotalTickets = (SELECT SUM(TICKETS) FROM @TicketsPromedio );
		 
        UPDATE @TicketsPromedio
        SET PORC_TICKETS = (100*TICKETS)/@TotalTickets;

 		SELECT   AÑO AS ANO, MES AS MES, TICKETS AS TICKETS,ROUND(TICKETS_PROM,2) AS TPROMEDIO,ROUND(PORC_TICKETS,2) AS TPORCENTAJE,VENTAS AS VENTAS, ROUND(VENTAS_PROM,2) AS VPROMEDIO
		FROM @TicketsPromedio;                                                                                                                                                                                                                                                                                     

        --DROP TABLE TicketsPromedio; 

 
                                                                                                                                                                                                                                                                                   
  '
                                                                                                                                                                                                                                                                                    

 exec sp_executesql @consulta;        
    END    
GO
/****** Object:  StoredProcedure [dbo].[sp_FiltroReporteAgrupacion]    Script Date: 28/04/2014 06:57:18 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[sp_FiltroReporteAgrupacion]
(
     
@gpoAnalisis varchar(30) OUT,   
@agrupacion varchar(30)OUT 
) 
AS
DECLARE @Grupo varchar(20); 
BEGIN
SET @Grupo = (select COD_GPO from tblagrupacionart where DES_GPO= @gpoAnalisis);
SELECT COD_AGR AS AGRUPACION from tblCatAgrupacionArt where COD_GPO=@Grupo AND DES_AGR=@agrupacion;
END
GO
/****** Object:  StoredProcedure [dbo].[Unidades]    Script Date: 28/04/2014 06:57:18 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[Unidades]
 (
 @unidad VARCHAR(30)
 ) 
AS
BEGIN
SELECT COD_UND as UNIDAD from tblCatUnidades where DES_UND=@unidad;
END
GO
/****** Object:  StoredProcedure [dbo].[VentaPorAño]    Script Date: 28/04/2014 06:57:18 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[VentaPorAño]
AS
BEGIN
DECLARE @consulta NVARCHAR(4000); 
SET @consulta = 'SELECT YEAR(FEC_REG) as Año ,SUM(TOT_PAG) as Importe from tblgralventas  GROUP BY  YEAR(FEC_REG);';
exec sp_executesql @consulta;   
END
GO
