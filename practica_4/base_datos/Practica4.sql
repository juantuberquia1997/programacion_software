/* ============================================================
   Practica4 - basedeDatos - bdEventos.sql
   Guía 4-1 Programación de Software (ITM)
   ============================================================ */
create DATABASE bdEventos
go
USE bdEventos
go

-- ============== Tablas ==============
create table tblTipoDoc (
    codigo int primary key,
    nombre varchar(50) Not Null,
    activo bit default 1 Not Null
);
go
create table tblGenero (
    codigo int primary key,
    nombre varchar(20) Not Null,
    activo bit default 1 Not Null
);
go
create table tblInstrum (
    codigo int primary key,
    nombre varchar(20) Not Null,
    activo bit default 1 Not Null
);
go
create table tblDpto (
    codigo int primary key,
    nombre varchar(50) Not Null
);
go
create table tblCiudad (
    codigo int primary key,
    nombre varchar(50) Not Null,
    idDpto int Not Null
);
go
create table tblEvento (
    codigo int primary key,
    nombre varchar(50) Not Null,
    idCiudad int Not Null,
    anio int Not Null,
    fecha varchar(30) Not Null,
    lugar varchar(80) Not Null,
    vrInscEvent decimal(9,2) default 0,
    activo bit default 1 Not Null
);
go
create table tblBanda (
    codigo int primary key,
    nombre varchar(50) Not Null,
    anioFund int Not Null,
    represent varchar(80) Not Null,
    telefono varchar(25) Not Null,
    idCiudad int Not Null,
    activo bit default 1 Not Null
);
go
create table tblInscrip (
    codigo int primary key,
    idEvento int Not Null,
    fecha varchar(12) default FORMAT(getdate(), 'd', 'en-gb'),
    idBanda int Not Null,
    represent varchar(80) Not Null,
    telRepresent varchar(25) Not Null,
    valorInscrip decimal(12,2) default 0
);
go
create table tblArtista (
    codigo int primary key,
    nombre varchar(80) Not Null,
    idtipoDoc int Not Null,
    nroDoc int Not Null,
    idGenero int Not Null,
    idCiudad int Not Null,
    activo bit default 1 Not Null
);
go
create table tblArtBand (
    codigo int identity(1,1) primary key,
    idBanda int Not Null,
    idArtista int Not Null,
    idInstrum int Not Null,
    activo bit default 1 Not Null
);
go
create table tblDetInsc (
    codigo int identity(1,1) primary key,
    idNroInsc int Not Null,
    idArtBand int Not Null,
    edad int Not Null
);
go

-- ============== Modelo relacional ==============
ALTER TABLE tblEvento  add Foreign key (idCiudad)  REFERENCES tblCiudad  (codigo)
ALTER TABLE tblCiudad  add Foreign key (idDpto)    REFERENCES tblDpto    (codigo)
ALTER TABLE tblArtista add Foreign key (idtipoDoc) REFERENCES tblTipoDoc (codigo)
ALTER TABLE tblBanda   add Foreign key (idCiudad)  REFERENCES tblCiudad  (codigo)
ALTER TABLE tblArtista add Foreign key (idGenero)  REFERENCES tblGenero  (codigo)
ALTER TABLE tblArtista add Foreign key (idCiudad)  REFERENCES tblCiudad  (codigo)
ALTER TABLE tblArtBand add Foreign key (idBanda)   REFERENCES tblBanda   (codigo)
ALTER TABLE tblArtBand add Foreign key (idArtista) REFERENCES tblArtista (codigo)
ALTER TABLE tblArtBand add Foreign key (idInstrum) REFERENCES tblInstrum (codigo)
ALTER TABLE tblInscrip add Foreign key (idEvento)  REFERENCES tblEvento  (codigo)
ALTER TABLE tblInscrip add Foreign key (idBanda)   REFERENCES tblBanda   (codigo)
ALTER TABLE tblDetInsc add Foreign key (idNroInsc) REFERENCES tblInscrip (codigo)
ALTER TABLE tblDetInsc add Foreign key (idArtBand) REFERENCES tblArtBand (codigo)
go

-- ============== Datos ==============
-- ======== tblTipoDoc ========
insert into tblTipoDoc values
 (1, 'Cédula Ciudadanía', 1), (2, 'Pasaporte', 1), (3, 'Cédula de Extranjería', 1);
go
-- ======== tblGenero ========
insert into tblGenero values (1, 'Masculino', 1), (2, 'Femenino', 1);
go
-- ======== tblInstrum ========
insert into tblInstrum values
 (101, 'Batería', 1), (102, 'Bajo', 1), (103, 'Guitarra', 1),
 (104, 'Sintetizador', 1), (105, 'Otro', 1), (119, 'Vocalista', 1);
go
-- ======== tblDpto ========
insert into tblDpto values
 (110, 'Antioquia'), (120, 'Atlántico'), (130, 'Valle'), (140, 'Cundinamarca');
go
-- ======== tblCiudad ========
insert into tblCiudad values
 (1, 'Medellín', 110), (2, 'Envigado', 110), (3, 'Bello', 110),
 (4, 'Barranquilla', 120), (5, 'Soledad', 120), (6, 'Malambo', 120),
 (7, 'Cali', 130), (8, 'Buenaventura', 130), (9, 'Buga', 130),
 (10, 'Bogotá', 140), (11, 'Usaquén', 140), (12, 'Soacha', 140);
go
-- ======== tblEvento ========
insert into tblEvento values
 (1, 'Rock Medellín 2026', 1, 2026, 'Diciembre 19', 'Estadio Atanasio Girardot', 200000, 1),
 (2, 'Rock Cali 2027',     7, 2027, 'Junio 26',      'Estadio Pascual Guerrero',  230000, 1);
go
-- ======== tblBanda ========
insert into tblBanda values
 (1, 'Overdrive',     2016, 'Hardrock',        '57 (604)2506970', 1,  1),
 (2, 'Ananaisia',     2017, 'Solo Rock',       '57 3003112233',   10, 1),
 (3, 'Spirit Brutal', 2020, 'Miguel Urrutia',  '57 3125102030',   4,  1);
go
-- ======== tblArtista ========
-- (codigo, nombre, idtipoDoc, nroDoc, idGenero, idCiudad, activo)
insert into tblArtista values
 (1,  'Manuela Herrera',       1, 1234, 2, 7,  1),
 (2,  'Manuela Suárez',        1, 5678, 2, 2,  1),
 (3,  'Melina Pinzón',         1, 3876, 2, 7,  1),
 (4,  'Mauricio Cardona',      1, 9890, 1, 8,  1),
 (5,  'Scott Ragun',           3, 1020, 1, 5,  1),
 (6,  'Jaz Ragun',             3, 3121, 1, 12, 1),
 (7,  'Maya Burgos',           1, 4120, 2, 1,  1),
 (8,  'Mateo Betancur',        1, 7890, 1, 4,  1),
 (9,  'Álvaro Rojas',          1, 9020, 1, 3,  1),
 (10, 'Nicolás Bark',          2, 7080, 1, 6,  1),
 (11, 'Dave Luna',             3, 8090, 1, 10, 1),
 (12, 'Daniel A. Aristizábal', 1, 7050, 1, 1,  1);
go
-- ======== tblArtBand ======== (codigo es identity)
-- (idBanda, idArtista, idInstrum, activo)
insert into tblArtBand values
 (1, 1,  119, 1), (1, 2,  103, 1), (1, 3,  102, 1), (1, 4,  101, 1), (1, 5,  104, 1),
 (2, 6,  103, 1), (2, 7,  119, 1), (2, 8,  101, 1), (2, 9,  102, 1),
 (3, 10, 103, 1), (3, 11, 101, 1), (3, 12, 119, 1);
go
-- ======== tblInscrip ========
-- (codigo, idEvento, fecha, idBanda, represent, telRepresent, valorInscrip)
insert into tblInscrip values
 (1, 1, '15/9/2026', 1, 'Alejandro Aristizabal', '3102030405', 1000000);
go
-- ======== tblDetalle_Inscrip ======== (codigo es identity)
-- (idNroInsc, idArtBand, edad)
insert into tblDetInsc values
 (1, 1, 27), (1, 2, 26), (1, 3, 26), (1, 4, 25), (1, 5, 25);
go

-- ======== Verificación ========
select * from tblArtBand
select * from tblInscrip
select * from tblDetInsc where idNroInsc = 1
go