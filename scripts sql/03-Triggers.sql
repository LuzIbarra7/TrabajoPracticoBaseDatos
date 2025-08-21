delimiter $$

use `5to_Trivago` $$

SELECT 'Creando Triggers' AS 'Estado' $$

drop trigger if exists befInsReserva$$

create trigger befInsReserva before insert on Reserva
for each row
begin
    declare disponible bool;
    SET disponible = DisponibilidadFecha(New.idHabitacion, New.Entrada, New.Salida);
    if(disponible = false)
    then
        signal sqlstate '45000'
        set message_text = "La fecha solicitada no disponible";
    end if;
end$$

