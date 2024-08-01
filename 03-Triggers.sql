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
declare existe bool;
    select verificacion_mail_registrado(New.Mail) into existe;
    if(existe = true)
    then
signal sqlstate '45000'
        set message_text = "Mail ingresado ya registrado, ir a log in?";
end if;
end$$
