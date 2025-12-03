use `5to_Trivago` ;
SELECT 'Creando SF' AS 'Estado';
delimiter $$

drop function if exists verificacion_usuario$$
create function verificacion_usuario(
mail varchar(60),
    contra char(64)
)returns bool reads sql data
begin
declare correcto bool;
if(
exists(
select *
        from Usuario U
        where U.Mail = mail and U.Contrasena = sha2(contra, 256)
)
)
then
set correcto = true;
else
set correcto = false;
end if;
return correcto;
end$$

drop function if exists verificacion_mail_registrado$$
create function verificacion_mail_registrado(unMail varchar(60))
returns bool reads sql data
begin
declare existe bool;
    set existe = false;
    if(
exists(
select *
            from Usuario
            where Mail = unMail
)
)
    then
set existe = true;
end if;
    return true;
end$$

drop function if exists DisponibilidadFecha$$
create function DisponibilidadFecha(unIdHabitacion int unsigned, unaEntrada datetime, unaSalida datetime )
returns bool reads sql data
begin
declare disponible bool;
    set disponible = true;
    if(
exists(
select *
            from Reserva
            where ((Entrada >= unaEntrada and Entrada <= unaSalida)
            or (Entrada <= unaEntrada and Salida >= unaEntrada))
            and idHabitacion = unIdHabitacion
            and unaEntrada < now()
        )
    )
then
set disponible = false;
end if;
    return disponible;
end$$

drop function if exists HabitacionesDisponiblesTipo$$
create function HabitacionesDisponiblesTipo(unIdTipo int unsigned, unidHotel int unsigned, unaEntrada datetime, unaSalida datetime)
returns tinyint unsigned reads sql data
begin
declare disponibles tinyint unsigned ;
select count(*) into disponibles
from Habitacion
where idHotel = unIdHotel and idTipo = unIdTipo;
return disponibles;
end$$
