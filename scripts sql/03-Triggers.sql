delimiter $$
use `5to_Trivago`$$
SELECT 'Creando Triggers' AS 'Estado'$$

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

drop trigger if exists befInsUsuario$$
create trigger befInsUsuario before insert on Usuario
for each row
begin
set New.Contraseña = sha2(New.Contraseña, 256);
end$$
