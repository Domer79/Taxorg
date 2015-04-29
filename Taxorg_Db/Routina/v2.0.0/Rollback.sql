USE [Taxorg]
GO
ALTER TABLE [sec].[UserGroups] DROP CONSTRAINT [FK_UserGroups_Users]
GO
ALTER TABLE [sec].[UserGroups] DROP CONSTRAINT [FK_UserGroups_Groups]
GO
ALTER TABLE [sec].[MemberRole] DROP CONSTRAINT [FK_MemberRole_Role]
GO
ALTER TABLE [sec].[MemberRole] DROP CONSTRAINT [FK_MemberRole_Member]
GO
ALTER TABLE [sec].[_User] DROP CONSTRAINT [FK_User_Sid]
GO
ALTER TABLE [sec].[_User] DROP CONSTRAINT [FK_User_Member]
GO
ALTER TABLE [sec].[_User] DROP CONSTRAINT [FK_User_Email]
GO
ALTER TABLE [sec].[_Group] DROP CONSTRAINT [FK_Group_Member]
GO
ALTER TABLE [sec].[_Grant] DROP CONSTRAINT [FK_Grant_SecObject]
GO
ALTER TABLE [sec].[_Grant] DROP CONSTRAINT [FK_Grant_Role]
GO
ALTER TABLE [sec].[_Grant] DROP CONSTRAINT [FK_Grant_AccessType]
GO
/****** Object:  View [sec].[RoleOfMember]    Script Date: 29.04.2015 10:02:18 ******/
DROP VIEW [sec].[RoleOfMember]
GO
/****** Object:  View [sec].[Members]    Script Date: 29.04.2015 10:02:18 ******/
DROP VIEW [sec].[Members]
GO
/****** Object:  View [sec].[Grants]    Script Date: 29.04.2015 10:02:18 ******/
DROP VIEW [sec].[Grants]
GO
/****** Object:  View [sec].[UserGroupsDetail]    Script Date: 29.04.2015 10:02:18 ******/
DROP VIEW [sec].[UserGroupsDetail]
GO
/****** Object:  View [sec].[Users]    Script Date: 29.04.2015 10:02:18 ******/
DROP VIEW [sec].[Users]
GO
/****** Object:  View [sec].[Groups]    Script Date: 29.04.2015 10:02:18 ******/
DROP VIEW [sec].[Groups]
GO
/****** Object:  Table [sec].[UserGroups]    Script Date: 29.04.2015 10:02:18 ******/
DROP TABLE [sec].[UserGroups]
GO
/****** Object:  Table [sec].[sid]    Script Date: 29.04.2015 10:02:18 ******/
DROP TABLE [sec].[sid]
GO
/****** Object:  Table [sec].[Settings]    Script Date: 29.04.2015 10:02:18 ******/
DROP TABLE [sec].[Settings]
GO
/****** Object:  Table [sec].[SecObject]    Script Date: 29.04.2015 10:02:18 ******/
DROP TABLE [sec].[SecObject]
GO
/****** Object:  Table [sec].[MemberRole]    Script Date: 29.04.2015 10:02:18 ******/
DROP TABLE [sec].[MemberRole]
GO
/****** Object:  Table [sec].[Member]    Script Date: 29.04.2015 10:02:18 ******/
DROP TABLE [sec].[Member]
GO
/****** Object:  Table [sec].[email]    Script Date: 29.04.2015 10:02:18 ******/
DROP TABLE [sec].[email]
GO
/****** Object:  Table [sec].[AccessType]    Script Date: 29.04.2015 10:02:18 ******/
DROP TABLE [sec].[AccessType]
GO
/****** Object:  Table [sec].[_User]    Script Date: 29.04.2015 10:02:18 ******/
DROP TABLE [sec].[_User]
GO
/****** Object:  Table [sec].[_Role]    Script Date: 29.04.2015 10:02:18 ******/
DROP TABLE [sec].[_Role]
GO
/****** Object:  Table [sec].[_Group]    Script Date: 29.04.2015 10:02:18 ******/
DROP TABLE [sec].[_Group]
GO
/****** Object:  Table [sec].[_Grant]    Script Date: 29.04.2015 10:02:18 ******/
DROP TABLE [sec].[_Grant]
GO
/****** Object:  UserDefinedFunction [sec].[IsAllowByName]    Script Date: 29.04.2015 10:02:18 ******/
DROP FUNCTION [sec].[IsAllowByName]
GO
/****** Object:  UserDefinedFunction [sec].[IsAllowById]    Script Date: 29.04.2015 10:02:18 ******/
DROP FUNCTION [sec].[IsAllowById]
GO
/****** Object:  UserDefinedFunction [sec].[GetSettings]    Script Date: 29.04.2015 10:02:18 ******/
DROP FUNCTION [sec].[GetSettings]
GO
/****** Object:  UserDefinedFunction [sec].[GetIdentificationMode]    Script Date: 29.04.2015 10:02:18 ******/
DROP FUNCTION [sec].[GetIdentificationMode]
GO
/****** Object:  StoredProcedure [sec].[UpdateUserGroup]    Script Date: 29.04.2015 10:02:18 ******/
DROP PROCEDURE [sec].[UpdateUserGroup]
GO
/****** Object:  StoredProcedure [sec].[UpdateUser]    Script Date: 29.04.2015 10:02:19 ******/
DROP PROCEDURE [sec].[UpdateUser]
GO
/****** Object:  StoredProcedure [sec].[UpdateMemberRole]    Script Date: 29.04.2015 10:02:19 ******/
DROP PROCEDURE [sec].[UpdateMemberRole]
GO
/****** Object:  StoredProcedure [sec].[UpdateGroup]    Script Date: 29.04.2015 10:02:19 ******/
DROP PROCEDURE [sec].[UpdateGroup]
GO
/****** Object:  StoredProcedure [sec].[UpdateGrant]    Script Date: 29.04.2015 10:02:19 ******/
DROP PROCEDURE [sec].[UpdateGrant]
GO
/****** Object:  StoredProcedure [sec].[SetIdentificationMode]    Script Date: 29.04.2015 10:02:19 ******/
DROP PROCEDURE [sec].[SetIdentificationMode]
GO
/****** Object:  StoredProcedure [sec].[DeleteUserFromGroup]    Script Date: 29.04.2015 10:02:19 ******/
DROP PROCEDURE [sec].[DeleteUserFromGroup]
GO
/****** Object:  StoredProcedure [sec].[DeleteUser]    Script Date: 29.04.2015 10:02:19 ******/
DROP PROCEDURE [sec].[DeleteUser]
GO
/****** Object:  StoredProcedure [sec].[DeleteMemberRole]    Script Date: 29.04.2015 10:02:19 ******/
DROP PROCEDURE [sec].[DeleteMemberRole]
GO
/****** Object:  StoredProcedure [sec].[DeleteGroup]    Script Date: 29.04.2015 10:02:19 ******/
DROP PROCEDURE [sec].[DeleteGroup]
GO
/****** Object:  StoredProcedure [sec].[DeleteGrant]    Script Date: 29.04.2015 10:02:19 ******/
DROP PROCEDURE [sec].[DeleteGrant]
GO
/****** Object:  StoredProcedure [sec].[AddUserToGroup]    Script Date: 29.04.2015 10:02:19 ******/
DROP PROCEDURE [sec].[AddUserToGroup]
GO
/****** Object:  StoredProcedure [sec].[AddUser]    Script Date: 29.04.2015 10:02:19 ******/
DROP PROCEDURE [sec].[AddUser]
GO
/****** Object:  StoredProcedure [sec].[AddMemberRole]    Script Date: 29.04.2015 10:02:19 ******/
DROP PROCEDURE [sec].[AddMemberRole]
GO
/****** Object:  StoredProcedure [sec].[AddGroup]    Script Date: 29.04.2015 10:02:19 ******/
DROP PROCEDURE [sec].[AddGroup]
GO
/****** Object:  StoredProcedure [sec].[AddGrant]    Script Date: 29.04.2015 10:02:19 ******/
DROP PROCEDURE [sec].[AddGrant]
GO
/****** Object:  Schema [sec]    Script Date: 29.04.2015 10:02:19 ******/
DROP SCHEMA [sec]
GO
