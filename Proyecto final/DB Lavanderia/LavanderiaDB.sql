-- ========================================================
-- Creación de la Base de Datos para SQL Server
-- ========================================================
USE master;
GO

DROP DATABASE LavanderiaDB;
GO

CREATE DATABASE LavanderiaDB;
GO

USE LavanderiaDB;
GO

-- ========================================================
-- 1. Tablas Independientes
-- ========================================================

-- Tabla departamento
CREATE TABLE departamento (
    iddepartamento INT PRIMARY KEY,
    nombre_departamento VARCHAR(100) NOT NULL
);
GO

-- Tabla tipo_documento
CREATE TABLE tipo_documento (
    idtipo_documento INT PRIMARY KEY,
    nombre_documento VARCHAR(50) NOT NULL
);
GO

-- Tabla tipo_telefono
CREATE TABLE tipo_telefono (
    idtipo_telefono INT PRIMARY KEY,
    tipo_telefono VARCHAR(50) NOT NULL
);
GO

-- Tabla estado_prenda
CREATE TABLE estado_prenda (
    idestado_prenda INT PRIMARY KEY,
    estado_prenda VARCHAR(50) NOT NULL
);
GO

-- Tabla tipo_servicio
CREATE TABLE tipo_servicio (
    idtipo_servicio INT PRIMARY KEY,
    nombre_servicio VARCHAR(100) NOT NULL,
    valor DECIMAL(10,2) NOT NULL,
    activo BIT NOT NULL DEFAULT 1
);
GO

-- Tabla tipo_prenda
CREATE TABLE tipo_prenda (
    idtipo_prenda INT PRIMARY KEY,
    nombre_prenda VARCHAR(100) NOT NULL,
    activo BIT NOT NULL DEFAULT 1
);
GO

-- Tabla profesion
CREATE TABLE profesion (
    idprofesion INT PRIMARY KEY,
    nombre_profesion VARCHAR(100) NOT NULL
);
GO

-- ========================================================
-- 2. Tablas con Dependencias
-- ========================================================

-- Tabla ciudad
CREATE TABLE ciudad (
    idciudad INT PRIMARY KEY,
    departamento_iddepartamento INT NOT NULL,
    nombre_ciudad VARCHAR(100) NOT NULL,
    CONSTRAINT FK_ciudad_departamento FOREIGN KEY (departamento_iddepartamento) REFERENCES departamento(iddepartamento)
);
GO

-- Tabla empleado
CREATE TABLE empleado (
    idempleado INT PRIMARY KEY,
    empleado_iddempleado INT NULL, -- Supervisor / Autoreferencia
    tipo_documento_idtipo_documento INT NOT NULL,
    nombre_empleado VARCHAR(100) NOT NULL,
    documento_empleado VARCHAR(50) NOT NULL,
    activo BIT NOT NULL DEFAULT 1,
    fecha_ingreso DATE NOT NULL,
    salario DECIMAL(10,2) NOT NULL,
    CONSTRAINT FK_empleado_tipo_documento FOREIGN KEY (tipo_documento_idtipo_documento) REFERENCES tipo_documento(idtipo_documento),
    CONSTRAINT FK_empleado_empleado FOREIGN KEY (empleado_iddempleado) REFERENCES empleado(idempleado)
);
GO

-- Tabla tipo_prenda_servicio
CREATE TABLE tipo_prenda_servicio (
    idtipo_prenda_servicio INT PRIMARY KEY,
    tipo_servicio_idtipo_servicio INT NOT NULL,
    tipo_prenda_idtipo_prenda INT NOT NULL,
    valor_unitario DECIMAL(10,2) NOT NULL,
    CONSTRAINT FK_tipo_pre_serv_tipo_servicio FOREIGN KEY (tipo_servicio_idtipo_servicio) REFERENCES tipo_servicio(idtipo_servicio),
    CONSTRAINT FK_tipo_pre_serv_tipo_prenda FOREIGN KEY ( tipo_prenda_idtipo_prenda) REFERENCES tipo_prenda(idtipo_prenda)
);
GO

-- Tabla usuario
CREATE TABLE usuario (
    idusuario INT NOT NULL,
    empleado_iddempleado INT NOT NULL,
    usuario VARCHAR(100) NOT NULL,
    clave VARCHAR(50) NOT NULL,
    activo BIT NOT NULL DEFAULT 1,
    CONSTRAINT PK_usuario PRIMARY KEY (idusuario, empleado_iddempleado),
    CONSTRAINT FK_usuario_empleado FOREIGN KEY (empleado_iddempleado) REFERENCES empleado(idempleado)
);
GO

-- Tabla descuento
CREATE TABLE descuento (
    iddescuento INT PRIMARY KEY,
    tipo_prenda_idtipo_prenda INT NOT NULL,
    cantidad_min INT NOT NULL,
    cantidad_max INT NOT NULL,
    porcentaje DECIMAL(5,2) NOT NULL,
    activo BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_descuento_tipo_prenda FOREIGN KEY (tipo_prenda_idtipo_prenda) REFERENCES tipo_prenda(idtipo_prenda)
);
GO

-- Tabla cliente
CREATE TABLE cliente (
    idcliente INT PRIMARY KEY,
    empleado_idempleado INT NOT NULL,
    tipo_documento_idtipo_documento INT NOT NULL,
    nombre_cliente VARCHAR(100) NOT NULL,
    numero_documento VARCHAR(50) NOT NULL,
    CONSTRAINT FK_cliente_empleado FOREIGN KEY (empleado_idempleado) REFERENCES empleado(idempleado),
    CONSTRAINT FK_cliente_tipo_documento FOREIGN KEY (tipo_documento_idtipo_documento) REFERENCES tipo_documento(idtipo_documento)
);
GO

-- Tabla direccion_empleado
CREATE TABLE direccion_empleado (
    iddireccion_empleado INT PRIMARY KEY,
    ciudad_idciudad INT NOT NULL,
    empleado_idempleado INT NOT NULL,
    direccion_empleado VARCHAR(200) NOT NULL,
    CONSTRAINT FK_direccion_empleado_ciudad FOREIGN KEY (ciudad_idciudad) REFERENCES ciudad(idciudad),
    CONSTRAINT FK_direccion_empleado_empleado FOREIGN KEY (empleado_idempleado) REFERENCES empleado(idempleado)
);
GO

-- Tabla telefono_empleado
CREATE TABLE telefono_empleado (
    idtelefono_empleado INT PRIMARY KEY,
    tipo_telefono_idtipo_telefono INT NOT NULL,
    empleado_idempleado INT NOT NULL,
    numero_telefono VARCHAR(30) NOT NULL,
    CONSTRAINT FK_telefono_empleado_empleado FOREIGN KEY (empleado_idempleado) REFERENCES empleado(idempleado),
    CONSTRAINT FK_telefono_empleado_tipo_telefono FOREIGN KEY (tipo_telefono_idtipo_telefono) REFERENCES tipo_telefono(idtipo_telefono)
);
GO

-- Tabla intermedia empleado_has_profesion
CREATE TABLE empleado_has_profesion (
    empleado_idempleado INT NOT NULL,
    profesion_idprofesion INT NOT NULL,
    CONSTRAINT PK_empleado_profesion PRIMARY KEY (empleado_idempleado, profesion_idprofesion),
    CONSTRAINT FK_emp_prof_empleado FOREIGN KEY (empleado_idempleado) REFERENCES empleado(idempleado),
    CONSTRAINT FK_emp_prof_profesion FOREIGN KEY (profesion_idprofesion) REFERENCES profesion(idprofesion)
);
GO

-- Tabla direccion_cliente
CREATE TABLE direccion_cliente (
    iddireccion_cliente INT PRIMARY KEY,
    ciudad_idciudad INT NOT NULL,
    cliente_idcliente INT NOT NULL,
    direccion_cliente VARCHAR(200) NOT NULL,
    CONSTRAINT FK_direccion_cliente_ciudad FOREIGN KEY (ciudad_idciudad) REFERENCES ciudad(idciudad),
    CONSTRAINT FK_direccion_cliente_cliente FOREIGN KEY (cliente_idcliente) REFERENCES cliente(idcliente)
);
GO

-- Tabla telefono_cliente
CREATE TABLE telefono_cliente (
    idtelefono_cliente INT PRIMARY KEY,
    tipo_telefono_idtipo_telefono INT NOT NULL,
    cliente_idcliente INT NOT NULL,
    numero VARCHAR(30) NOT NULL,
    CONSTRAINT FK_telefono_cliente_tipo_telefono FOREIGN KEY (tipo_telefono_idtipo_telefono) REFERENCES tipo_telefono(idtipo_telefono),
    CONSTRAINT FK_telefono_cliente_cliente FOREIGN KEY (cliente_idcliente) REFERENCES cliente(idcliente)
);
GO

-- Tabla ingreso
CREATE TABLE ingreso (
    idingreso INT PRIMARY KEY,
    cliente_idcliente INT NOT NULL,
    direccion_cliente_iddireccion_cliente INT NULL, -- Esta puede ir null porqué es la dir del domicilio no?
    empleado_idempleado INT NOT NULL,
    fecha DATE NOT NULL,
    domicilio BIT NOT NULL DEFAULT 0,
    activo BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_ingreso_cliente FOREIGN KEY (cliente_idcliente) REFERENCES cliente(idcliente),
    CONSTRAINT FK_ingreso_empleado FOREIGN KEY (empleado_idempleado) REFERENCES empleado(idempleado),
    CONSTRAINT FK_ingreso_direccion_cliente FOREIGN KEY (direccion_cliente_iddireccion_cliente) REFERENCES direccion_cliente(iddireccion_cliente)
);
GO

-- Tabla detalle_ingreso
CREATE TABLE detalle_ingreso (
    iddetalle_ingreso INT IDENTITY(1,1) PRIMARY KEY,
    descuento_iddescuento INT NULL,
    tipo_prenda_servicio_idtipo_prenda_servicio INT NOT NULL,
    ingreso_idingreso INT NOT NULL,
    estado_prenda_idestado_prenda INT NOT NULL,
    cantidad INT NOT NULL,
    valor DECIMAL(10,2) NOT NULL,
    observacion VARCHAR(500) NULL,
    CONSTRAINT FK_detalle_ingreso_ingreso FOREIGN KEY (ingreso_idingreso) REFERENCES ingreso(idingreso),
    CONSTRAINT FK_detalle_ingreso_estado_prenda FOREIGN KEY (estado_prenda_idestado_prenda) REFERENCES estado_prenda(idestado_prenda),
    CONSTRAINT FK_detalle_ingreso_tipo_pre_serv FOREIGN KEY (tipo_prenda_servicio_idtipo_prenda_servicio) REFERENCES tipo_prenda_servicio(idtipo_prenda_servicio),
    CONSTRAINT FK_detalle_ingreso_descuento FOREIGN KEY (descuento_iddescuento) REFERENCES descuento(iddescuento)
);
GO