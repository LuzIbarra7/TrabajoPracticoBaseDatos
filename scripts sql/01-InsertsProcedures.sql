DELIMITER //
Use `5to_Trivago` //
SELECT 'Creando SP' AS 'Estado'//
DROP PROCEDURE IF EXISTS insert_pais//



CREATE PROCEDURE insert_pais(
  IN p_Nombre VARCHAR(45),
  OUT p_idPais int unsigned
)
BEGIN
  INSERT INTO `Pais` (`Nombre`) VALUES (p_Nombre);
  set p_idPais = last_insert_id();
END //


DELIMITER ;

-- Procedure for Ciudad
DROP PROCEDURE IF EXISTS insert_ciudad;

DELIMITER //

CREATE PROCEDURE insert_ciudad(
  IN p_idPais INT UNSIGNED,
  IN p_nombre VARCHAR(45),
  out p_IdCiudad int unsigned
)
BEGIN
  INSERT INTO `Ciudad` (`idPais`, `nombre`) VALUES (p_idPais, p_nombre);
  set p_IdCiudad = last_insert_id();
END //


DELIMITER ;

-- Procedure for Hotel
DROP PROCEDURE IF EXISTS insert_hotel;

DELIMITER //

CREATE PROCEDURE insert_hotel(
  IN p_idCiudad INT UNSIGNED,
  IN p_Nombre VARCHAR(45),
  IN p_Direccion VARCHAR(45),
  IN p_Telefono INT,
  IN p_URL VARCHAR(90),
  out p_idHotel int unsigned
)
BEGIN
  INSERT INTO `Hotel` (`idCiudad`, `Nombre`, `Direccion`, `Telefono`, `URL`) VALUES (p_idCiudad, p_Nombre, p_Direccion, p_Telefono, p_URL);
  set p_idHotel = last_insert_id();
END //


DELIMITER ;

-- Procedure for TipoHabitacion
DROP PROCEDURE IF EXISTS insert_tipo_habitacion;

DELIMITER //

CREATE PROCEDURE insert_tipo_habitacion(
  IN p_Nombre VARCHAR(45),
  out p_idTipo int unsigned
)
BEGIN
  INSERT INTO `TipoHabitacion` (`Nombre`) VALUES (p_Nombre);
  set p_idTipo = last_insert_id();
END //

DELIMITER ;

-- Procedure for Habitacion
DROP PROCEDURE IF EXISTS insert_habitacion;

DELIMITER //

CREATE PROCEDURE insert_habitacion(
  IN p_idHotel INT UNSIGNED,
  IN p_idTipo INT UNSIGNED,
  IN p_PrecioPorNoche DECIMAL(10, 2),
  out p_idHabitacion int unsigned
)
BEGIN
  INSERT INTO `Habitacion` (`idHotel`, `idTipo`, `PrecioPorNoche`) VALUES (p_idHotel, p_idTipo, p_PrecioPorNoche);
  set p_idHabitacion = last_insert_id();
END //

DELIMITER ;

-- Procedure for MetodoPago
DROP PROCEDURE IF EXISTS insert_metodo_pago;

DELIMITER //

CREATE PROCEDURE insert_metodo_pago(
  IN p_TipoMedioPago VARCHAR(45),
  out p_idMetodoPago int unsigned
)
BEGIN
  INSERT INTO `MetodoPago` (`TipoMedioPago`) VALUES (p_TipoMedioPago);
  set p_idMetodoPago = last_insert_id();
END //

DELIMITER ;



-- Procedure for Usuario
DROP PROCEDURE IF EXISTS insert_usuario;

DELIMITER //

CREATE PROCEDURE insert_usuario(
  IN p_Nombre VARCHAR(45),
  IN p_Apellido VARCHAR(45),
  IN p_Mail VARCHAR(60),
  IN p_Contraseña CHAR(64),
  OUT p_idUsuario INT unsigned
)
BEGIN
  INSERT INTO `Usuario` (`Nombre`, `Apellido`, `Mail`, `Contraseña`) 
  VALUES (p_Nombre, p_Apellido, p_Mail, p_Contraseña);
  SET p_idUsuario = LAST_INSERT_ID();
END //

DELIMITER ;

-- Procedure for Reserva
DROP PROCEDURE IF EXISTS insert_reserva;

DELIMITER //

CREATE PROCEDURE insert_reserva(
  IN p_idHabitacion INT UNSIGNED,
  IN p_idMetododePago INT UNSIGNED,
  IN p_idUsuario INT UNSIGNED,
  IN p_Entrada DATETIME,
  IN p_Salida DATETIME,
  IN p_Telefono INT,
  out p_idReserva int unsigned
)
BEGIN
  INSERT INTO `Reserva` (`idHabitacion`, `idMetododePago`, `idUsuario`, `Entrada`, `Salida`, `Precio`, `Telefono`)
  select p_idHabitacion, p_idMetododePago, p_idUsuario, p_Entrada, p_Salida, H.PrecioPorNoche * (datediff(p_Salida, p_Entrada)), p_Telefono
  from Habitacion H
  where idHabitacion = p_idHabitacion;
  set p_idReserva = last_insert_id();
END //

DELIMITER ;
-- Procedure for Comentario
DROP PROCEDURE IF EXISTS insert_comentario;

DELIMITER //

CREATE PROCEDURE insert_comentario(
  IN p_idHabitacion INT UNSIGNED,
  IN p_Comentario VARCHAR(100),
  IN p_Calificacion TINYINT(10),
  out p_idComentario int unsigned
)
BEGIN
  INSERT INTO `Comentario` (`idHabitacion`, `Comentario`, `Calificacion`) VALUES (p_idHabitacion, p_Comentario, p_Calificacion);
  set p_idComentario = last_insert_id();
END //

DELIMITER ;