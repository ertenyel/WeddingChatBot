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
	VALUES ('������� ������ ���� � ����� �� � �����������!', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1),
		   ('� ���� ���� �� ������ ������ ��� ����������� � ����� �������!', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 2),
		   ('� ���� ���� �� ������ ���������� � ����!', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 3),
		   ('�� ������ � ������!', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 7),
		   ('������� ���� ����� �����, ����� ������!', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 7),
		   ('��� ����� ����������� ��������� �����!', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 7),
		   ('�������� ���� ������� ����������!', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 7),
		   ('�� ������� � ���?', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 4),
		   ('������� ��� ����� ������ ���������!', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 7),
		   ('������� ������������ � ��������', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 5),
		   ('������� ������������ � ���', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 6)
END

IF NOT EXISTS (SELECT TOP 1 1 FROM Buttons)
BEGIN
	INSERT INTO Buttons ([Text], [IdMessage], [ButtonType])
	VALUES ('������� � �������', 1, 0),								
		   ('�������� ���������� � ����', 1, 0),
		   ('����� ���� �����������?', 2, 0),
		   ('� ����� ����� �������?', 2, 0),
		   ('��� ������� �� ������?', 2, 0),
		   ('��� ����� ��� �� ���������� (������ ������� �����������)', 2, 0),
		   ('����������� �����������', 3, 0),
		   ('������� ������ ���������', 3, 0),
		   ('������� �������� ��������', 3, 0),
		   ('������� ������������ � �������', 3, 0),
		   ('��!', 4, 0),
		   ('���!', 4, 0),
		   ('�������!', 4, 0),
		   ('�����', 5, 0),
		   ('����', 5, 0),
		   ('�������', 5, 0),
		   ('����������', 5, 0),
		   ('���������� ����� �� �� ��', 5, 0),
		   ('�� �� �����', 6, 0),
		   ('� �� �������', 6, 0),
		   ('���� � ���������, �� �� �������', 6, 0),
		   ('���������', 6, 0),
		   ('��������� �����', 7, 1),
		   ('��������� � ������� ����', 7, 2)
END
