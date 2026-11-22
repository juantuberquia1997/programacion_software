use master;
go


create database lavanderiadb;
go

use lavanderiadb;
go

create table profesion (
    idprofesion integer primary key,
    nombre_profesion varchar(100) not null
);
go

create table tipo_documento (
    idtipo_documento integer primary key,
    nombre_documento varchar(100) not null
);
go

create table departamento (
    iddepartamento integer primary key,
    nombre_departamento varchar(100) not null
);
go

create table ciudad (
    idciudad integer primary key,
    iddepartamento integer not null,
    nombre_ciudad varchar(100) not null,
    foreign key (iddepartamento) references departamento(iddepartamento)
);
go

create table tipo_telefono (
    idtipo_telefono integer primary key,
    tipo_telefono varchar(50) not null
);
go

create table estado_prenda (
    idestado_prenda integer primary key,
    estado_prenda varchar(100) not null
);
go

create table tipo_servicio (
    idtipo_servicio integer primary key,
    nombre_servicio varchar(100) not null,
    valor integer not null,
    activo bit not null
);
go

create table tipo_prenda (
    idtipo_prenda integer primary key,
    nombre_prenda varchar(100) not null,
    activo bit not null
);
go

create table tipo_prenda_servicio (
    idtipo_prenda_servicio integer primary key,
    idtipo_servicio integer not null,
    idtipo_prenda integer not null,
    valor_unitario decimal(10, 2) not null,
    activo bit not null,
    foreign key (idtipo_servicio) references tipo_servicio(idtipo_servicio),
    foreign key (idtipo_prenda) references tipo_prenda(idtipo_prenda)
);
go

create table descuento (
    iddescuento integer primary key,
    idtipo_prenda integer not null,
    cantidad_min integer not null,
    cantidad_max integer not null,
    porcentaje decimal(5, 2) not null,
    activo bit not null,
    foreign key (idtipo_prenda) references tipo_prenda(idtipo_prenda)
);
go

create table empleado (
    idempleado integer primary key,
    empleado_idempleado integer,
    idtipo_documento integer not null,
    nombre_apellido_emp varchar(150) not null,
    documento_empleado varchar(50) not null,
    activo bit not null,
    fecha_ingreso date not null,
    salario decimal(12, 2) not null,
    foreign key (empleado_idempleado) references empleado(idempleado),
    foreign key (idtipo_documento) references tipo_documento(idtipo_documento)
);
go

create table empleado_has_profesion (
    empleado_idempleado integer not null,
    profesion_idprofesion integer not null,
    primary key (empleado_idempleado, profesion_idprofesion),
    foreign key (empleado_idempleado) references empleado(idempleado),
    foreign key (profesion_idprofesion) references profesion(idprofesion)
);
go

create table usuario (
    idusuarios integer primary key,
    empleado_idempleado integer not null,
    usuario varchar(80) not null,
    clave varchar(120) not null,
    activo bit not null,
    foreign key (empleado_idempleado) references empleado(idempleado)
);
go

create table direccion_empleado (
    iddireccion_empleado integer primary key,
    idciudad integer not null,
    empleado_idempleado integer not null,
    direccion_empleado varchar(150) not null,
    foreign key (idciudad) references ciudad(idciudad),
    foreign key (empleado_idempleado) references empleado(idempleado)
);
go

create table telefono_empleado_2 (
    idtelefono_empleado_2 integer primary key,
    idtipo_telefono integer not null,
    empleado_idempleado integer not null,
    numero varchar(30) not null,
    foreign key (idtipo_telefono) references tipo_telefono(idtipo_telefono),
    foreign key (empleado_idempleado) references empleado(idempleado)
);
go

create table cliente (
    idcliente integer primary key,
    empleado_idempleado integer not null,
    idtipo_documento integer not null,
    nombre_apellido_cliente varchar(150) not null,
    numero_documento varchar(50) not null,
    foreign key (empleado_idempleado) references empleado(idempleado),
    foreign key (idtipo_documento) references tipo_documento(idtipo_documento)
);
go

create table direccion_cliente (
    iddireccion integer primary key,
    idciudad integer not null,
    idcliente integer not null,
    direccion_cliente varchar(150) not null,
    foreign key (idciudad) references ciudad(idciudad),
    foreign key (idcliente) references cliente(idcliente)
);
go

create table telefono_cliente (
    idtelefono integer primary key,
    idtipo_telefono integer not null,
    idcliente integer not null,
    numero varchar(30) not null,
    foreign key (idtipo_telefono) references tipo_telefono(idtipo_telefono),
    foreign key (idcliente) references cliente(idcliente)
);
go

create table ingreso (
    idingreso integer primary key,
    idcliente integer not null,
    iddireccion integer not null,
    empleado_idempleado integer not null,
    fecha date not null,
    domicilio bit not null,
    activo bit not null,
    foreign key (idcliente) references cliente(idcliente),
    foreign key (iddireccion) references direccion_cliente(iddireccion),
    foreign key (empleado_idempleado) references empleado(idempleado)
);
go

create table detalle_ingreso (
    iddetalle_ingreso integer primary key,
    iddescuento integer not null,
    idtipo_prenda_servicio integer not null,
    idcliente integer not null,
    idingreso integer not null,
    idestado_prenda integer not null,
    cantidad integer not null,
    valor decimal(10, 2) not null,
    observacion varchar(200),
    foreign key (iddescuento) references descuento(iddescuento),
    foreign key (idtipo_prenda_servicio) references tipo_prenda_servicio(idtipo_prenda_servicio),
    foreign key (idcliente) references cliente(idcliente),
    foreign key (idingreso) references ingreso(idingreso),
    foreign key (idestado_prenda) references estado_prenda(idestado_prenda)
);
go

-- registros iniciales (5 por tabla)

insert into profesion (idprofesion, nombre_profesion) values
(1, 'administrador'),
(2, 'operario de lavanderia'),
(3, 'sastre'),
(4, 'auxiliar contable'),
(5, 'logistica y entregas');
go

insert into tipo_documento (idtipo_documento, nombre_documento) values
(1, 'cedula de ciudadania'),
(2, 'cedula de extranjeria'),
(3, 'tarjeta de identidad'),
(4, 'pasaporte'),
(5, 'nit');
go

insert into departamento (iddepartamento, nombre_departamento) values
(1, 'antioquia'),
(2, 'cundinamarca'),
(3, 'valle del cauca'),
(4, 'atlantico'),
(5, 'santander');
go

insert into ciudad (idciudad, iddepartamento, nombre_ciudad) values
(1, 1, 'medellin'),
(2, 1, 'bello'),
(3, 2, 'bogota'),
(4, 3, 'cali'),
(5, 4, 'barranquilla');
go

insert into tipo_telefono (idtipo_telefono, tipo_telefono) values
(1, 'celular'),
(2, 'fijo'),
(3, 'whatsapp'),
(4, 'corporativo'),
(5, 'emergencia');
go

insert into estado_prenda (idestado_prenda, estado_prenda) values
(1, 'buen estado'),
(2, 'tiene un rasguño'),
(3, 'esta descolorida'),
(4, 'tiene una mancha'),
(5, 'tiene un roto');
go

insert into tipo_servicio (idtipo_servicio, nombre_servicio, valor, activo) values
(1, 'lavado en seco', 18000, 1),
(2, 'lavado normal', 12000, 1),
(3, 'planchado', 8000, 1),
(4, 'tintoreria', 25000, 1),
(5, 'ajuste y costura', 15000, 1);
go

insert into tipo_prenda (idtipo_prenda, nombre_prenda, activo) values
(1, 'traje completo', 1),
(2, 'camisa', 1),
(3, 'pantalon', 1),
(4, 'vestido', 1),
(5, 'abrigo', 1);
go

insert into tipo_prenda_servicio (idtipo_prenda_servicio, idtipo_servicio, idtipo_prenda, valor_unitario, activo) values
(1, 1, 1, 28000.00, 1),
(2, 2, 2, 10000.00, 1),
(3, 3, 3, 7000.00, 1),
(4, 4, 4, 24000.00, 1),
(5, 1, 5, 26000.00, 1);
go

insert into descuento (iddescuento, idtipo_prenda, cantidad_min, cantidad_max, porcentaje, activo) values
(1, 1, 2, 5, 10.00, 1),
(2, 2, 5, 15, 15.00, 1),
(3, 3, 3, 10, 12.00, 1),
(4, 4, 2, 6, 8.00, 1),
(5, 5, 1, 4, 5.00, 1);
go

insert into empleado (idempleado, empleado_idempleado, idtipo_documento, nombre_apellido_emp, documento_empleado, activo, fecha_ingreso, salario) values
(1, null, 1, 'carlos andres restrepo', '1017200101', 1, '2024-01-15', 2800000.00),
(2, 1, 1, 'laura milena gomez', '1017200102', 1, '2024-03-01', 1850000.00),
(3, 1, 1, 'jorge enrique munoz', '1017200103', 1, '2024-05-10', 1900000.00),
(4, 2, 1, 'sofia valentina jaramillo', '1017200104', 1, '2025-01-20', 1750000.00),
(5, 2, 2, 'mateo alejandro ospina', '9001017205', 1, '2025-04-02', 1700000.00);
go

insert into empleado_has_profesion (empleado_idempleado, profesion_idprofesion) values
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 5);
go

insert into usuario (idusuarios, empleado_idempleado, usuario, clave, activo) values
(1, 1, 'crestrepo', 'clave_1234', 1),
(2, 2, 'lgomez', 'clave_2345', 1),
(3, 3, 'jmunoz', 'clave_3456', 1),
(4, 4, 'sjaramillo', 'clave_4567', 1),
(5, 5, 'mospina', 'clave_5678', 1);
go

insert into direccion_empleado (iddireccion_empleado, idciudad, empleado_idempleado, direccion_empleado) values
(1, 1, 1, 'calle 50 # 45-20'),
(2, 1, 2, 'carrera 70 # 32-15'),
(3, 2, 3, 'diagonal 55 # 40-12'),
(4, 1, 4, 'transversal 39 # 71-08'),
(5, 2, 5, 'carrera 64 # 52-30');
go

insert into telefono_empleado_2 (idtelefono_empleado_2, idtipo_telefono, empleado_idempleado, numero) values
(1, 1, 1, '3004112233'),
(2, 1, 2, '3105223344'),
(3, 3, 3, '3206334455'),
(4, 1, 4, '3157445566'),
(5, 2, 5, '6044556677');
go

insert into cliente (idcliente, empleado_idempleado, idtipo_documento, nombre_apellido_cliente, numero_documento) values
(1, 2, 1, 'andrea paola zapata', '1036601001'),
(2, 2, 1, 'daniel felipe cardona', '1036601002'),
(3, 3, 1, 'natalia isabel bedoya', '1036601003'),
(4, 4, 2, 'sebastian david meza', '4006601004'),
(5, 5, 1, 'maria camila lopez', '1036601005');
go

insert into direccion_cliente (iddireccion, idciudad, idcliente, direccion_cliente) values
(1, 1, 1, 'carrera 43a # 14-50'),
(2, 1, 2, 'calle 10 # 38-25'),
(3, 2, 3, 'carrera 50 # 48-19'),
(4, 1, 4, 'circular 4 # 73-10'),
(5, 2, 5, 'calle 46 # 54-09'),
(6, 2, 1, 'calle 80 # 50-12');
go

insert into telefono_cliente (idtelefono, idtipo_telefono, idcliente, numero) values
(1, 1, 1, '3012109876'),
(2, 3, 2, '3113209876'),
(3, 1, 3, '3124309876'),
(4, 2, 4, '6045409876'),
(5, 1, 5, '3185509876');
go

insert into ingreso (idingreso, idcliente, iddireccion, empleado_idempleado, fecha, domicilio, activo) values
(1, 1, 1, 2, '2026-11-10', 1, 1),
(2, 2, 2, 2, '2026-11-11', 0, 1),
(3, 3, 3, 3, '2026-11-12', 1, 1),
(4, 4, 4, 4, '2026-11-13', 0, 1),
(5, 5, 5, 5, '2026-11-14', 1, 1);
go

insert into detalle_ingreso (iddetalle_ingreso, iddescuento, idtipo_prenda_servicio, idcliente, idingreso, idestado_prenda, cantidad, valor, observacion) values
(1, 1, 1, 1, 1, 4, 2, 50400.00, 'traje con mancha leve en solapa'),
(2, 2, 2, 2, 2, 2, 5, 42500.00, 'camisas de algodon sin almidon'),
(3, 3, 3, 3, 3, 3, 3, 18480.00, 'pantalones con doblez marcado'),
(4, 4, 4, 4, 4, 1, 2, 44160.00, 'vestido de gala con pedreria'),
(5, 5, 5, 5, 5, 5, 1, 24700.00, 'abrigo entregado conforme');
go

select * from tipo_prenda