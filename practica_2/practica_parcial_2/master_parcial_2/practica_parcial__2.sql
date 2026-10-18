CREATE Database admin_producto;
go

USE admin_producto;
go;

Create Table producto
(   codigo int Primary Key NOT NULL,
    nombre varchar(200) NOT NULL,
    documento int  NOT NULL
)
go


INSERT INTO producto VALUES
(200, 'Santiago', 102615707)
go

/*

select * from producto;
alter table producto add valor_unitario int null

UPDATE producto 
SET nombre = 'Gloria'
WHERE codigo = 3;

ALTER TABLE producto ADD documento int NOT NULL DEFAULT 1026157076;
ALTER TABLE producto DROP CONSTRAINT DF__producto__apelli__573DED66;
GO
ALTER TABLE producto DROP COLUMN apellido;
GO

DELETE FROM producto 
WHERE codigo IN (201);
GO

Para alterar un procedure usar ALTER y ejecuta la seccion

*/


--Procedure buscar
create Procedure USP_Usu_BuscarXCodigo
    @intCodigo int
AS
BEGIN
    SELECT nombre, documento
    FROM producto
    WHERE codigo = @intCodigo
    -- EXEC USP_Usu_BuscarXCodigo 200;
END
go

--Procedure grabar
create Procedure USP_Usu_Grabar
    @strNombre  varchar(200),
    @intDocumento int

As
BEGIN
    IF EXISTS( SELECT * FROM producto
               WHERE documento = @intDocumento )
    BEGIN
        SELECT -1 AS Rpta
        Return
    END
    ELSE
    BEGIN
        BEGIN TRANSACTION tx
            Declare @intNewCod int
            Set @intNewCod = ( select max( codigo ) + 1
                                from producto )

            INSERT INTO producto VALUES ( @intNewCod, @strNombre, @intDocumento);

            IF ( @@ERROR > 0 )
            BEGIN
                ROLLBACK TRANSACTION tx
                SELECT 0 AS Rpta
                Return
            END
        COMMIT TRANSACTION tx
        SELECT @intNewCod AS Rpta
        Return
    END
END
go
-- EXEC USP_Usu_Grabar 3, 'Eulogio', 1026708;
-- EXEC USP_Usu_BuscarXCodigo 200;


--Procedure para modificar
create Procedure USP_Usu_Modificar
    @intCodigo  int,
    @strNombre  VARCHAR(200),
    @intDocumento int
As
BEGIN
    IF EXISTS( SELECT * FROM producto
               WHERE codigo = @intCodigo and codigo <> @intCodigo )
    BEGIN
        SELECT -1 AS Rpta
        Return
    END
    ELSE
    BEGIN
        BEGIN TRANSACTION tx
            UPDATE producto SET
                codigo   = @intCodigo,
                nombre   = @strNombre,
                documento = @intDocumento
            WHERE codigo = @intCodigo

            IF ( @@ERROR > 0 )
            BEGIN
                ROLLBACK TRANSACTION tx
                SELECT 0 AS Rpta
                Return
            END
        COMMIT TRANSACTION tx
        SELECT @intCodigo AS Rpta
        Return
    END
END
go
-- EXEC USP_Usu_Modificar 1, 'David';
-- EXEC USP_Usu_BuscarXCodigo 1;



/*SALAK406-35*/

