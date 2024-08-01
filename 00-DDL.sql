-- -----------------------------------------------------
-- Schema Trivago
-- -----------------------------------------------------
DROP SCHEMA IF EXISTS `Trivago` ;

-- -----------------------------------------------------
-- Schema Trivago
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `Trivago`;
USE `Trivago` ;

-- -----------------------------------------------------
-- Table `Trivago`.`Pais`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Trivago`.`Pais` ;

CREATE TABLE IF NOT EXISTS `Trivago`.`Pais` (
  `idPais` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `Nombre` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`idPais`),
  UNIQUE INDEX `nombre_UNIQUE` (`Nombre`) )
;


-- -----------------------------------------------------
-- Table `Trivago`.`Ciudad`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Trivago`.`Ciudad` ;

CREATE TABLE IF NOT EXISTS `Trivago`.`Ciudad` (
  `idCiudad` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `idPais` INT UNSIGNED NOT NULL,
  `nombre` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`idCiudad`),
  UNIQUE INDEX `idCiudad_UNIQUE` (`idCiudad` ASC, `idPais` ASC) ,
  INDEX `fk_Ciudad_Pais1_idx` (`idPais` ASC) ,
  CONSTRAINT `fk_Ciudad_Pais1`
    FOREIGN KEY (`idPais`)
    REFERENCES `Trivago`.`Pais` (`idPais`))
;


-- -----------------------------------------------------
-- Table `Trivago`.`Hotel`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Trivago`.`Hotel` ;

CREATE TABLE IF NOT EXISTS `Trivago`.`Hotel` (
  `idHotel` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `idCuidad` INT UNSIGNED NOT NULL,
  `Nombre` VARCHAR(45) NOT NULL,
  `Direccion` VARCHAR(45) NOT NULL,
  `Telefono` INT UNSIGNED NOT NULL,
  `URL` VARCHAR(90) NOT NULL,
  PRIMARY KEY (`idHotel`),
  UNIQUE INDEX `Hotelcol_UNIQUE` (`Telefono` ASC) ,
  UNIQUE INDEX `Direccion_UNIQUE` (`Direccion` ASC) ,
  INDEX `fk_Hotel_Ciudad1_idx` (`idCuidad` ASC) ,
  UNIQUE INDEX `URL_UNIQUE` (`URL` ASC) ,
  CONSTRAINT `fk_Hotel_Ciudad1`
    FOREIGN KEY (`idCuidad`)
    REFERENCES `Trivago`.`Ciudad` (`idCiudad`)
)
;


-- -----------------------------------------------------
-- Table `Trivago`.`TipoHabitacion`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Trivago`.`TipoHabitacion` ;

CREATE TABLE IF NOT EXISTS `Trivago`.`TipoHabitacion` (
  `idTipo` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `Nombre` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`idTipo`),
  UNIQUE INDEX `Nombre_UNIQUE` (`Nombre` ASC) )
;


-- -----------------------------------------------------
-- Table `Trivago`.`Habitacion`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Trivago`.`Habitacion` ;

CREATE TABLE IF NOT EXISTS `Trivago`.`Habitacion` (
  `idHabitacion` INT UNSIGNED NOT NULL,
  `idHotel` INT UNSIGNED NOT NULL,
  `idTipo` INT UNSIGNED NOT NULL,
  `PrecioPorNoche` DECIMAL UNSIGNED NULL,
  PRIMARY KEY (`idHabitacion`, `idHotel`),
  INDEX `fk_Habitacion_Hotel1_idx` (`idHotel` ASC) ,
  INDEX `fk_Habitacion_TipoHanbitacion1_idx` (`idTipo` ASC) ,
  CONSTRAINT `fk_Habitacion_Hotel1`
    FOREIGN KEY (`idHotel`)
    REFERENCES `Trivago`.`Hotel` (`idHotel`)
,
  CONSTRAINT `fk_Habitacion_TipoHanbitacion1`
    FOREIGN KEY (`idTipo`)
    REFERENCES `Trivago`.`TipoHabitacion` (`idTipo`)
)
;


-- -----------------------------------------------------
-- Table `Trivago`.`MetodoPago`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Trivago`.`MetodoPago` ;

CREATE TABLE IF NOT EXISTS `Trivago`.`MetodoPago` (
  `idMetodoPago` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `TipoMedioPago` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`idMetodoPago`),
  UNIQUE INDEX `TipoMedioPago_UNIQUE` (`TipoMedioPago` ASC) )
;

-- -----------------------------------------------------
-- Table `Trivago`.`Usuario`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Trivago`.`Usuario` ;

CREATE TABLE IF NOT EXISTS `Trivago`.`Usuario` (
  `idUsuario` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `Nombre` VARCHAR(45) NOT NULL,
  `Apellido` VARCHAR(45) NOT NULL,
  `Mail` VARCHAR(60) NOT NULL,
  `Contraseña` CHAR(64) NOT NULL,
  PRIMARY KEY (`idUsuario`),
  UNIQUE INDEX `Mail_UNIQUE` (`Mail` ASC)
)
;


-- -----------------------------------------------------
-- Table `Trivago`.`Reserva`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Trivago`.`Reserva` ;

CREATE TABLE IF NOT EXISTS `Trivago`.`Reserva` (
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
    REFERENCES `Trivago`.`Habitacion` (`idHabitacion`)
,
  CONSTRAINT `fk_Reserva_MetodoPago1`
    FOREIGN KEY (`idMetododePago`)
    REFERENCES `Trivago`.`MetodoPago` (`idMetodoPago`)
,
  CONSTRAINT `fk_Reserva_Usuario1`
    FOREIGN KEY (`idUsuario`)
    REFERENCES `Trivago`.`Usuario` (`idUsuario`)
)
;



-- -----------------------------------------------------
-- Table `Trivago`.`Comentario`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Trivago`.`Comentario` ;

CREATE TABLE IF NOT EXISTS `Trivago`.`Comentario` (
  `idComentario` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `idHabitacion` INT UNSIGNED NOT NULL,
  `Comentario` VARCHAR(100) NOT NULL,
  `Calificacion` TINYINT(10) NOT NULL,
  PRIMARY KEY (`idComentario`),
  INDEX `fk_Comentario_Habitacion_idx` (`idHabitacion` ASC) ,
  CONSTRAINT `fk_Comentario_Habitacion`
    FOREIGN KEY (`idHabitacion`)
    REFERENCES `Trivago`.`Habitacion` (`idHabitacion`)
)
