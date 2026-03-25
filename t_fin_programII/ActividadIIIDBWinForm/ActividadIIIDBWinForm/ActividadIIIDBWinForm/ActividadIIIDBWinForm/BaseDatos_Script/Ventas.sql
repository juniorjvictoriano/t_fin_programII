CREATE DATABASE Ventas;
GO

USE Ventas;
GO

CREATE TABLE Categorias (
    CategoriaID INT PRIMARY KEY IDENTITY(1,1),
    NombreCategoria VARCHAR(50) NOT NULL
);

CREATE TABLE Productos (
    ProductoID INT PRIMARY KEY IDENTITY(1,1),
    NombreProducto VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(255) NULL,
    Precio DECIMAL(10,2) NOT NULL,
    Stock INT NOT NULL DEFAULT 0,
    CategoriaID INT NULL,
    FOREIGN KEY (CategoriaID) REFERENCES Categorias(CategoriaID)
);

CREATE TABLE Clientes (
    ClienteID INT PRIMARY KEY IDENTITY(1,1),
    NombreCompleto VARCHAR(150) NOT NULL,
    CorreoElectronico VARCHAR(100) NULL,
    Telefono VARCHAR(15) NULL,
    Direccion VARCHAR(255) NULL
);

CREATE TABLE Facturas (
    FacturaID INT PRIMARY KEY IDENTITY(1,1),
    ClienteID INT NOT NULL,
    FechaFactura DATE NOT NULL,
    Total DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (ClienteID) REFERENCES Clientes(ClienteID)
);

CREATE TABLE DetallesFactura (
    DetalleID INT PRIMARY KEY IDENTITY(1,1),
    FacturaID INT NOT NULL,
    ProductoID INT NOT NULL,
    Cantidad INT NOT NULL,
    Precio DECIMAL(10,2) NOT NULL,
    Impuesto DECIMAL(10,2) NOT NULL,
    Subtotal DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (FacturaID) REFERENCES Facturas(FacturaID),
    FOREIGN KEY (ProductoID) REFERENCES Productos(ProductoID)
);

CREATE TABLE Proveedores (
    ProveedorID INT PRIMARY KEY IDENTITY(1,1),
    NombreProveedor VARCHAR(100) NOT NULL,
    Telefono VARCHAR(15) NULL,
    CorreoElectronico VARCHAR(100) NULL
);

CREATE TABLE Compras (
    CompraID INT PRIMARY KEY IDENTITY(1,1),
    ProveedorID INT NOT NULL,
    FechaCompra DATE NOT NULL,
    Total DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (ProveedorID) REFERENCES Proveedores(ProveedorID)
);

CREATE TABLE DetallesCompra (
    DetalleCompraID INT PRIMARY KEY IDENTITY(1,1),
    CompraID INT NOT NULL,
    ProductoID INT NOT NULL,
    Cantidad INT NOT NULL,
    CostoUnitario DECIMAL(10,2) NOT NULL,
    Impuesto DECIMAL(10,2) NOT NULL,
    Subtotal DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (CompraID) REFERENCES Compras(CompraID),
    FOREIGN KEY (ProductoID) REFERENCES Productos(ProductoID)
);


-- Insertar datos en Categorias
INSERT INTO Categorias (NombreCategoria) VALUES ('Electrónica'), ('Ropa'), ('Alimentos');

-- Insertar datos en Productos
INSERT INTO Productos (NombreProducto, Descripcion, Precio, Stock, CategoriaID) VALUES
('Laptop', 'Laptop de alto rendimiento', 1200.00, 10, 1),
('Camiseta', 'Camiseta de algodón', 15.00, 50, 2),
('Pan', 'Pan integral', 2.50, 100, 3);

-- Insertar datos en Clientes
INSERT INTO Clientes (NombreCompleto, CorreoElectronico, Telefono, Direccion) VALUES
('Juan Pérez', 'juan@example.com', '8091234567', 'Calle Falsa 123'),
('María López', 'maria@example.com', '8299876543', 'Av. Central 45');

-- Insertar datos en Facturas
INSERT INTO Facturas (ClienteID, FechaFactura, Total) VALUES
(1, '2024-03-01', 1217.50),
(2, '2024-03-02', 32.50);

-- Insertar datos en DetallesFactura
INSERT INTO DetallesFactura (FacturaID, ProductoID, Cantidad, Precio, Impuesto, Subtotal) VALUES
(1, 1, 1, 1200.00, 17.50, 1217.50),
(2, 2, 2, 15.00, 2.50, 32.50);

-- Insertar datos en Proveedores
INSERT INTO Proveedores (NombreProveedor, Telefono, CorreoElectronico) VALUES
('Proveedor Tech', '8091112222', 'tech@example.com'),
('Proveedor Moda', '8293334444', 'moda@example.com');

-- Insertar datos en Compras
INSERT INTO Compras (ProveedorID, FechaCompra, Total) VALUES
(1, '2024-02-28', 2000.00),
(2, '2024-02-27', 500.00);

-- Insertar datos en DetallesCompra
INSERT INTO DetallesCompra (CompraID, ProductoID, Cantidad, CostoUnitario, Impuesto, Subtotal) VALUES
(1, 1, 5, 400.00, 100.00, 2000.00),
(2, 2, 10, 45.00, 50.00, 500.00);
