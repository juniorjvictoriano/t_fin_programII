-- Crear la base de datos
CREATE DATABASE GestionPersonas;
GO

USE GestionPersonas;
GO

CREATE TABLE personas (
    id_persona INT IDENTITY(1,1) PRIMARY KEY,
    nombre NVARCHAR(50) NOT NULL,
    apellido NVARCHAR(50) NOT NULL,
    fecha_nacimiento DATE NOT NULL,
    genero CHAR(1),
    nacionalidad NVARCHAR(50)
);
GO

CREATE TABLE direcciones (
    id_direccion INT IDENTITY(1,1) PRIMARY KEY,
    id_persona INT FOREIGN KEY REFERENCES personas(id_persona) ON DELETE CASCADE,
    calle NVARCHAR(100) NOT NULL,
    ciudad NVARCHAR(50) NOT NULL,
    estado NVARCHAR(50) NOT NULL,
    codigo_postal NVARCHAR(10),
    pais NVARCHAR(50) NOT NULL
);
GO

CREATE TABLE telefonos (
    id_telefono INT IDENTITY(1,1) PRIMARY KEY,
    id_persona INT FOREIGN KEY REFERENCES personas(id_persona) ON DELETE CASCADE,
    numero NVARCHAR(15) NOT NULL,
    tipo NVARCHAR(10)
);
GO

CREATE TABLE correos (
    id_correo INT IDENTITY(1,1) PRIMARY KEY,
    id_persona INT FOREIGN KEY REFERENCES personas(id_persona) ON DELETE CASCADE,
    correo NVARCHAR(100) NOT NULL UNIQUE,
    tipo NVARCHAR(10)
);
GO

CREATE TABLE ocupaciones (
    id_ocupacion INT IDENTITY(1,1) PRIMARY KEY,
    id_persona INT FOREIGN KEY REFERENCES personas(id_persona) ON DELETE CASCADE,
    empresa NVARCHAR(100) NOT NULL,
    cargo NVARCHAR(50) NOT NULL,
    fecha_inicio DATE NOT NULL,
    fecha_fin DATE NULL
);
GO

INSERT INTO personas (nombre, apellido, fecha_nacimiento, genero, nacionalidad) VALUES
('Carlos', 'Gómez', '1985-06-12', 'M', 'Mexicana'),
('Ana', 'Pérez', '1992-09-24', 'F', 'Argentina'),
('Luis', 'Martínez', '1978-03-15', 'M', 'Colombiana'),
('María', 'Lopez', '1995-07-08', 'F', 'Chilena');
GO

INSERT INTO direcciones (id_persona, calle, ciudad, estado, codigo_postal, pais) VALUES
(1, 'Avenida Reforma 123', 'Ciudad de México', 'CDMX', '01000', 'México'),
(2, 'Calle Florida 567', 'Buenos Aires', 'Buenos Aires', 'C1005', 'Argentina'),
(3, 'Carrera 45 #10-23', 'Medellín', 'Antioquia', '050001', 'Colombia'),
(4, 'Avenida Las Condes 789', 'Santiago', 'RM', '8320000', 'Chile');
GO

INSERT INTO telefonos (id_persona, numero, tipo) VALUES
(1, '555-1234', 'Móvil'),
(1, '555-5678', 'Trabajo'),
(2, '11-2345-6789', 'Móvil'),
(3, '4-567-8901', 'Casa'),
(4, '9-8765-4321', 'Móvil');
GO

INSERT INTO correos (id_persona, correo, tipo) VALUES
(1, 'carlos.gomez@example.com', 'Personal'),
(2, 'ana.perez@example.com', 'Trabajo'),
(3, 'luis.martinez@example.com', 'Personal'),
(4, 'maria.lopez@example.com', 'Trabajo');
GO


INSERT INTO ocupaciones (id_persona, empresa, cargo, fecha_inicio, fecha_fin) VALUES
(1, 'TechCorp', 'Ingeniero de Software', '2015-08-01', NULL),
(2, 'Marketing SA', 'Analista de Marketing', '2020-03-15', NULL),
(3, 'Banco Nacional', 'Gerente', '2010-06-20', '2023-12-31'),
(4, 'Diseño Creativo Ltda', 'Diseñadora Gráfica', '2019-07-10', NULL);
GO

SELECT * FROM personas;
SELECT * FROM direcciones;
SELECT * FROM telefonos;
SELECT * FROM correos;
SELECT * FROM ocupaciones;
