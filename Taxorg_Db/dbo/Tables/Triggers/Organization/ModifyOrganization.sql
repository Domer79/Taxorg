create trigger OnAddOrganization on Organization
after insert, update, delete
as

if dbo.TableIsModified('Organization') = 0
	raiserror('Нельзя напрямую вносить изменения в эту таблицу', 16, 10)