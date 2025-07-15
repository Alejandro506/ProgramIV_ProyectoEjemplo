-- OPCIONAL: Crear base de datos
CREATE DATABASE ProyectoLibros;
GO

-- Seleccionar base de datos
USE ProyectoLibros;
GO

-- Crear tabla Libros
CREATE TABLE Libros (
    LibroId INT PRIMARY KEY,
    Titulo NVARCHAR(200) NOT NULL,
    Autor NVARCHAR(150),
    Genero NVARCHAR(100),
    AnioPublicacion INT NOT NULL
);
GO

-- Insertar datos
INSERT INTO Libros (LibroId, Titulo, Autor, Genero, AnioPublicacion) VALUES
(1, 'El Señor de los Anillos', 'J.R.R. Tolkien', 'Fantasía', 1954),
(2, 'Matar a un Ruiseñor', 'Harper Lee', 'Ficción', 1960),
(3, '1984', 'George Orwell', 'Distopía', 1949),
(4, 'Orgullo y Prejuicio', 'Jane Austen', 'Romance', 1813),
(5, 'El Hobbit', 'J.R.R. Tolkien', 'Fantasía', 1937),
(6, 'Harry Potter y la Piedra Filosofal', 'J.K. Rowling', 'Fantasía', 1997),
(7, 'Cien Años de Soledad', 'Gabriel García Márquez', 'Realismo Mágico', 1967),
(8, 'El Principito', 'Antoine de Saint-Exupéry', 'Fábula', 1943),
(9, 'Don Quijote de la Mancha', 'Miguel de Cervantes', 'Novela', 1605),
(10, 'Crimen y Castigo', 'Fiódor Dostoyevski', 'Ficción Psicológica', 1866);
GO



