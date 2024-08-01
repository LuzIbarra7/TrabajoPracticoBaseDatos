-- Procedure for Pais
delimiter $$

call insert_pais("Argentina", @idArgentina)$$
call insert_pais("Francia", @idFrancia)$$
call insert_pais("Brasil", @idBrasil)$$

call insert_ciudad(@idArgentina, "Buenos Aires", @idBuenosAires)$$
call insert_ciudad(@idArgentina, "Mendoza", @idMendoza)$$
call insert_ciudad(@idArgentina, "Santiago del Estero", @idSantiagodelEstero)$$

call insert_hotel(@idBuenosAires, "Hotel Buenos Aires", "Rivadavia 1", "11111111", "https:.....", @idHotelBuenosAires)$$
call insert_hotel(@idMendoza, "Hotel Mendoza", "Rivadavia 2", "11111112", "https:....", @idHotelMendoza)$$
call insert_hotel(@idSantiagodelEstero, "Hotel BsAs", "Rivadavia 3", "11111113", "https:..", @idHotelSantiagodelEstero)$$

call insert_tipo_habitacion("Suite", @idSuite)$$
call insert_tipo_habitacion("Junior suite", @idJuniorSuite)$$
call insert_tipo_habitacion("Gran suite", @idGranSuite)$$

call insert_habitacion(@idHotelBuenosAires, @idSuite, 10000.00, @idHabitacionHotelBuenosAiresSuite1)$$
call insert_habitacion(@idHotelMendoza, @idJuniorSuite, 20000.00, @idHabitacionHotelMendozaJuniorSuite1)$$
call insert_habitacion(@idHotelSantiagodelEstero, @idGranSuite, 30000.00, @idHabitacionHotelSantiagodelEsteroGranSuite1)$$

call insert_metodo_pago("Mercado Pago", @idMercadoPago)$$
call insert_metodo_pago("Efectivo", @idEfectivo)$$
call insert_metodo_pago("VisaDebito", @idVisaDebito)$$

call insert_usuario("Leonardo", "Cheng", "leonardocheng@gmail.com", "123", @idUsuarioLeonardoCheng)$$
call insert_usuario("Mario", "Rojas", "mariorojas@gmail.com", "321", @idUsuarioMarioRojas)$$
call insert_usuario("Luz", "Ibarra", "luzibarra@gmail.com", "231", @idUsuarioLuzIbarra)$$

call insert_reserva(@idHabitacionHotelBuenosAiresSuite1, @idMercadoPago, @idUsuarioLeonardoCheng, now(), '01-01-2025', 11111111, @idReservaLeonardoChengHabitacionHotelBuenosAiresSuite1)$$
call insert_reserva(@idHabitacionHotelMendozaJuniorSuite1, @idEfectivo, @idMarioRojas, now(), '01-02-2025', 11111112, @idReservaMarioRojasHabitacionHotelBuenosAiresSuite1)$$
call insert_reserva(@idHabitacionHotelSantiagodelEsteroGranSuite1, @idVisaDebito, @idUsuaioLuzIbarra, now(), '01-03-2025', 11111113, @idReservaLuzIbarraHabitacionHotelBuenosAiresSuite1)$$

call insert_comentario(@idHabitacionHotelBuenosAiresSuite1, "no me gusta", 3, @idComentarioHabitacionHotelBuenosAiresSuite1_1)$$
call insert_comentario(@idHabitacionHotelMendozaJuniorSuite1, "me gusta", 8, @idComentarioHabitacionHotelMendozaJuniorSuite1_1)$$
call insert_comentario(@idHabitacionHotelSantiagodelEsteroGranSuite1, "nose", 6, @idComentarioHabitacionHotelSantiagodelEsteroGranSuite1_1)$$
