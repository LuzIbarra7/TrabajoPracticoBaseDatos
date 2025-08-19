drop DATABASE if EXISTS 5to_Trivago;

-- -----------------------------------------------------
-- Schema 5to_Trivago
-- -----------------------------------------------------
DROP SCHEMA IF EXISTS `5to_Trivago` ;

-- -----------------------------------------------------
-- Schema 5to_Trivago
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `5to_Trivago`;
CREATE DATABASE if not exists 5to_Trivago;
USE `5to_Trivago` ;

-- -----------------------------------------------------
-- Table `5to_Trivago`.`Pais`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `5to_Trivago`.`Pais` ;

CREATE TABLE IF NOT EXISTS `5to_Trivago`.`Pais` (
  `idPais` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `Nombre` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`idPais`),
  UNIQUE INDEX `nombre_UNIQUE` (`Nombre`) )
;


-- -----------------------------------------------------
-- Table `5to_Trivago`.`Ciudad`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `5to_Trivago`.`Ciudad` ;

CREATE TABLE IF NOT EXISTS `5to_Trivago`.`Ciudad` (
  `idCiudad` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `idPais` INT UNSIGNED NOT NULL,
  `nombre` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`idCiudad`),
  UNIQUE INDEX `idCiudad_UNIQUE` (`idCiudad` ASC, `idPais` ASC) ,
  INDEX `fk_Ciudad_Pais1_idx` (`idPais` ASC) ,
  CONSTRAINT `fk_Ciudad_Pais1`
    FOREIGN KEY (`idPais`)
    REFERENCES `5to_Trivago`.`Pais` (`idPais`))
;


-- -----------------------------------------------------
-- Table `5to_Trivago`.`Hotel`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `5to_Trivago`.`Hotel` ;

CREATE TABLE IF NOT EXISTS `5to_Trivago`.`Hotel` (
  `idHotel` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `idCiudad` INT UNSIGNED NOT NULL,
  `Nombre` VARCHAR(45) NOT NULL,
  `Direccion` VARCHAR(45) NOT NULL,
  `Telefono` INT UNSIGNED NOT NULL,
  `URL` VARCHAR(90),
  PRIMARY KEY (`idHotel`),
  UNIQUE INDEX `Hotelcol_UNIQUE` (`Telefono` ASC) ,
  UNIQUE INDEX `Direccion_UNIQUE` (`Direccion` ASC) ,
  INDEX `fk_Hotel_Ciudad1_idx` (`idCiudad` ASC) ,
  UNIQUE INDEX `URL_UNIQUE` (`URL` ASC) ,
  CONSTRAINT `fk_Hotel_Ciudad1`
    FOREIGN KEY (`idCiudad`)
    REFERENCES `5to_Trivago`.`Ciudad` (`idCiudad`)
)
;


-- -----------------------------------------------------
-- Table `5to_Trivago`.`TipoHabitacion`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `5to_Trivago`.`TipoHabitacion` ;

CREATE TABLE IF NOT EXISTS `5to_Trivago`.`TipoHabitacion` (
  `idTipo` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `Nombre` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`idTipo`),
  UNIQUE INDEX `Nombre_UNIQUE` (`Nombre` ASC) )
;


-- -----------------------------------------------------
-- Table `5to_Trivago`.`Habitacion`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `5to_Trivago`.`Habitacion` ;

CREATE TABLE IF NOT EXISTS `5to_Trivago`.`Habitacion` (
  `idHabitacion` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `idHotel` INT UNSIGNED NOT NULL,
  `idTipo` INT UNSIGNED NOT NULL,
  `PrecioPorNoche` DECIMAL(10,2) UNSIGNED NULL,
  PRIMARY KEY (`idHabitacion`, `idHotel`),
  INDEX `fk_Habitacion_Hotel1_idx` (`idHotel` ASC) ,
  INDEX `fk_Habitacion_TipoHanbitacion1_idx` (`idTipo` ASC) ,
  CONSTRAINT `fk_Habitacion_Hotel1`
    FOREIGN KEY (`idHotel`)
    REFERENCES `5to_Trivago`.`Hotel` (`idHotel`)
,
  CONSTRAINT `fk_Habitacion_TipoHanbitacion1`
    FOREIGN KEY (`idTipo`)
    REFERENCES `5to_Trivago`.`TipoHabitacion` (`idTipo`)
)
;


-- -----------------------------------------------------
-- Table `5to_Trivago`.`MetodoPago`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `5to_Trivago`.`MetodoPago` ;

CREATE TABLE IF NOT EXISTS `5to_Trivago`.`MetodoPago` (
  `idMetodoPago` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `TipoMedioPago` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`idMetodoPago`),
  UNIQUE INDEX `TipoMedioPago_UNIQUE` (`TipoMedioPago` ASC) )
;

-- -----------------------------------------------------
-- Table `5to_Trivago`.`Usuario`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `5to_Trivago`.`Usuario` ;

CREATE TABLE IF NOT EXISTS `5to_Trivago`.`Usuario` (
  `idUsuario` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `Nombre` VARCHAR(45) NOT NULL,
  `Apellido` VARCHAR(45) NOT NULL,
  `Mail` VARCHAR(60) NOT NULL,
  `Contrasena` CHAR(64) NOT NULL,
  PRIMARY KEY (`idUsuario`),
  UNIQUE INDEX `Mail_UNIQUE` (`Mail` ASC)
)
;


-- -----------------------------------------------------
-- Table `5to_Trivago`.`Reserva`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `5to_Trivago`.`Reserva` ;

CREATE TABLE IF NOT EXISTS `5to_Trivago`.`Reserva` (
  `idReserva` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `idHabitacion` INT UNSIGNED NOT NULL,
  `idMetododePago` INT UNSIGNED NOT NULL,
  `idUsuario` INT UNSIGNED NOT NULL,
  `Entrada` DATETIME NOT NULL,
  `Salida` DATETIME NOT NULL,
  `Precio` DECIMAL UNSIGNED NOT NULL,
  `Telefono` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`idReserva`),
  INDEX `fk_Reserva_Habitacion1_idx` (`idHabitacion` ASC) ,
  INDEX `fk_Reserva_MetodoPago1_idx` (`idMetododePago` ASC) ,
  INDEX `fk_Reserva_Usuario1_idx` (`idUsuario` ASC),
  CONSTRAINT `fk_Reserva_Habitacion1`
    FOREIGN KEY (`idHabitacion`)
    REFERENCES `5to_Trivago`.`Habitacion` (`idHabitacion`)
,
  CONSTRAINT `fk_Reserva_MetodoPago1`
    FOREIGN KEY (`idMetododePago`)
    REFERENCES `5to_Trivago`.`MetodoPago` (`idMetodoPago`)
,
  CONSTRAINT `fk_Reserva_Usuario1`
    FOREIGN KEY (`idUsuario`)
    REFERENCES `5to_Trivago`.`Usuario` (`idUsuario`)
)
;



-- -----------------------------------------------------
-- Table `5to_Trivago`.`Comentario`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `5to_Trivago`.`Comentario`;

CREATE TABLE IF NOT EXISTS `5to_Trivago`.`Comentario` (
  `idComentario` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `idHabitacion` INT UNSIGNED NOT NULL,
  `Comentario` VARCHAR(100) NOT NULL,
  `Calificacion` TINYINT(10) NOT NULL,
  `Fecha` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`idComentario`),
  INDEX `fk_Comentario_Habitacion_idx` (`idHabitacion` ASC),
  CONSTRAINT `fk_Comentario_Habitacion`
    FOREIGN KEY (`idHabitacion`)
    REFERENCES `5to_Trivago`.`Habitacion` (`idHabitacion`)
);
