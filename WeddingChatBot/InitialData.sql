IF NOT EXISTS (SELECT TOP 1 1 FROM ChatPositions)
BEGIN
	INSERT INTO ChatPositions ([Name], [ParentId], [IdMessageText]) VALUES 
							 ('main', NULL, 1),
							 ('infoevent', 1, 2),
							 ('infouser', 1, 3),
							 ('infonewlyweds', 2, 4),
							 ('infoeventplan', 2, 5),
							 ('infolocations', 2, 6),
							 ('infocolor', 2, 7),
							 ('setchoice', 3, 8),
							 ('setcompanion', 3,9),
							 ('setalcohol', 3, 10),
							 ('setfood', 3, 11)
END

IF NOT EXISTS (SELECT TOP 1 1 FROM TextInMessages)
BEGIN
	INSERT INTO TextInMessages ([Text] , [ImageUrl], [VideoUrl], [StickerId], [FirstLatitude], [FirstLongitude], [SecondLatitude], [SecondLongitude], [IdButtons])
	VALUES ('Выбирай пункты меню и узнай всё о мероприятии!', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1),
		   ('В этом меню ты можешь узнать все подробности о нашем событии!', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 2),
		   ('В этом меню ты можешь рассказать о себе!', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 3),
		   ('Мы Марина и Кирилл!', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 7),
		   ('Сначала ЗАГС потом фотки, потом банкет!', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 7),
		   ('Все места мероприятия находятся здесь!', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 7),
		   ('Основной цвет свадьбы изумрудный!', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 7),
		   ('Вы придете к нам?', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 4),
		   ('Укажите имя Вашей второй половинки!', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 7),
		   ('Укажите предпочтения в алкоголе', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 5),
		   ('Укажите предпочтения в еде', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 6)
END

IF NOT EXISTS (SELECT TOP 1 1 FROM Buttons)
BEGIN
	INSERT INTO Buttons ([Text], [IdMessage], [ButtonType])
	VALUES ('Вопросы о свадьбе', 1, 0),								
		   ('Записать информацию о себе', 1, 0),
		   ('Какой план мероприятия?', 2, 0),
		   ('В каком цвете свадьба?', 2, 0),
		   ('Кто женится то вообще?', 2, 0),
		   ('Дай знать где ты находишься (узнать локации мероприятия)', 2, 0),
		   ('Подтвердить присутствие', 3, 0),
		   ('Указать вторую половинку', 3, 0),
		   ('Выбрать желаемый алкоголь', 3, 0),
		   ('Указать предпочтения в закуске', 3, 0),
		   ('Да!', 4, 0),
		   ('Нет!', 4, 0),
		   ('Подумаю!', 4, 0),
		   ('Водка', 5, 0),
		   ('Вино', 5, 0),
		   ('Самогон', 5, 0),
		   ('Шампанское', 5, 0),
		   ('Ананасовый сироп оп оп оп', 5, 0),
		   ('Не ем грибы', 6, 0),
		   ('Я не местный', 6, 0),
		   ('Борщ с капусткой, но не красный', 6, 0),
		   ('Сосисочки', 6, 0),
		   ('Вернуться назад', 7, 1),
		   ('Вернуться в главное меню', 7, 2)
END
