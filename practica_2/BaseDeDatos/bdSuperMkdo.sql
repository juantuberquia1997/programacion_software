Create Database bdSuperMkdo
go
USE bdSuperMkdo
go

Create Table tblUsuario
(   codigo int Primary Key NOT NULL,
    nroDoc int NOT NULL,
    idTipDoc int NOT NULL,
    nombre varchar(50) NOT NULL,
    apellido varchar(50) NOT NULL,
    genero int NOT NULL,
    activo bit default 1 NOT NULL,
    observac varchar(500) NULL
)
go

--============ tblUsuario ============
INSERT INTO tblUsuario VALUES
(30001, 1020001000, 7, 'Claudia',    'Aristizábal Rúa',    20, 1, 'Ppto Particip'),
(30002, 1020001001, 8, 'Mario A.',   'Martínez Rios',      10, 1, 'Fondo EPM'),
(30003, 1020001002, 1, 'Natalia',    'Castrillón Caguán',  20, 1, null),
(30004, 1020001003, 1, 'Rubén D.',   'Soto Rios',          10, 1, 'Pruebas de todo'),
(30005, 1020001004, 3, 'Scott',      'Rangún Suteu',       10, 1, 'Reserva'),
(30006, 1020001005, 9, 'Jazmín A.',  'Arango Marín',       20, 1, 'Ppto Particip.'),
(30007, 1020001006, 6, 'Cristina M.','Soto Diaz',          20, 1, 'Suspende Matríc')
go

/* SELECT * FROM tblUsuario where codigo = 30006 , que diferencia hay entre este linea y la siguiente */
--buscar usuario, se le pasa un id
Create Procedure USP_Usu_BuscarXCodigo
    @intCodigo int
AS
BEGIN
    SELECT nroDoc, idTipDoc, nombre, apellido, genero, Activo, isnull( observac,'' ) observac
    FROM tblUsuario
    WHERE codigo = @intCodigo
    -- EXEC USP_Usu_BuscarXCodigo 30001;
END
go

--agregar el metodo de grabar
Create Procedure USP_Usu_Grabar
    @intNroDoc  int,
    @intTipDoc  int,
    @strNombre  VARCHAR(50),
    @strApelli  VARCHAR(50),
    @intGenero  int,
    @bitActivo  BIT,
    @strObserv  VARCHAR(500)
As
BEGIN
    IF EXISTS( SELECT * FROM tblUsuario
               WHERE nroDoc = @intNroDoc )
    BEGIN
        SELECT -1 AS Rpta
        Return
    END
    ELSE
    BEGIN
        BEGIN TRANSACTION tx
            Declare @intNewCod int
            Set @intNewCod = ( select max( codigo ) + 1
                                from tblUsuario )

            INSERT INTO tblUsuario VALUES ( @intNewCod,
                @intNroDoc, @intTipDoc, @strNombre, @strApelli,
                @intGenero, @bitActivo, @strObserv );

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
-- EXEC USP_Usu_Grabar 70500600, 1, 'Andrés', 'Duque', 10, 1, null;
-- EXEC USP_Usu_BuscarXCodigo 30008;


--agregar el metodo de modificar
Create Procedure USP_Usu_Modificar
    @intCodigo  int,
    @intNroDoc  int,
    @intTipDoc  int,
    @strNombre  VARCHAR(50),
    @strApelli  VARCHAR(50),
    @intGenero  int,
    @bitActivo  BIT,
    @strObserv  VARCHAR(500)
As
BEGIN
    IF EXISTS( SELECT * FROM tblUsuario
               WHERE nroDoc = @intNroDoc and codigo <> @intCodigo )
    BEGIN
        SELECT -1 AS Rpta
        Return
    END
    ELSE
    BEGIN
        BEGIN TRANSACTION tx
            UPDATE tblUsuario SET
                nroDoc   = @intNroDoc,
                idTipDoc = @intTipDoc,
                nombre   = @strNombre,
                apellido = @strApelli,
                genero   = @intGenero,
                Activo   = @bitActivo,
                Observac = @strObserv
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
-- EXEC USP_Usu_Modificar 30009, 1026157076, 6, 'cambio', 'cambio', 20, 1, 'test';
-- EXEC USP_Usu_BuscarXCodigo 30009;
