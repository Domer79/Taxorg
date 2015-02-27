/*
Шаблон скрипта после развертывания							
--------------------------------------------------------------------------------------
 В данном файле содержатся инструкции SQL, которые будут добавлены в скрипт построения.		
 Используйте синтаксис SQLCMD для включения файла в скрипт после развертывания.			
 Пример:      :r .\myfile.sql								
 Используйте синтаксис SQLCMD для создания ссылки на переменную в скрипте после развертывания.		
 Пример:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

if not exists(select 1 from Settings where name = 'taxperiodyear')
	insert into Settings(name, value, description) values('taxperiodyear', 'current', 'Установка года для выбора периода, если значение не установлено или отсутствует, то значение устанавливается в current, что означает текущий год')

if not exists(select 1 from Settings where name = 'taxperiodmonth')
	insert into Settings(name, value, description) values('taxperiodmonth', 'current', 'Установка месяца для выбора периода, если значение не установлено или отсутствует, то значение устанавливается в current, что означает текущий месяц')

if not exists(select 1 from Settings where name = 'taxprevperiod')
	insert into Settings(name, value, description) values('taxprevperiod', '1', 'Значение для вычисления предыдущего периода')

exec GrantToPublic 0