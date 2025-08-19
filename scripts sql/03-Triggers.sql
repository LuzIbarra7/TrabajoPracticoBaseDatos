delimiter $$

use `5to_Trivago` $$

SELECT 'Creando Triggers' AS 'Estado' $$

drop trigger if exists befInsReserva$$

create trigger befInsReserva before insert on Reserva
for each row
begin
declare disponible bool;
select DisponibilidadFecha(New.idHabitacion, New.Entrada, New.Salida) into disponible;
if(disponible = false)
then
signal sqlstate '45000'
set message_text = "La fecha solicitada no disponible";
end if;
end$$

#DROP TRIGGER IF EXISTS befInsUsuario$$
#CREATE TRIGGER befInsUsuario BEFORE INSERT ON Usuario
#FOR EACH ROW
#BEGIN
#    SET NEW.Contrasena = SHA2(NEW.Contrasena, 256);
#END$$
