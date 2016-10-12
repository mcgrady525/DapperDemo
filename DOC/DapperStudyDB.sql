/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2008                    */
/* Created on:     2016/10/12 11:54:36                          */
/*==============================================================*/


if exists (select 1
            from  sysobjects
           where  id = object_id('t_sys_rights_role')
            and   type = 'U')
   drop table t_sys_rights_role
go

if exists (select 1
            from  sysobjects
           where  id = object_id('t_sys_rights_user')
            and   type = 'U')
   drop table t_sys_rights_user
go

if exists (select 1
            from  sysobjects
           where  id = object_id('t_sys_rights_user_role')
            and   type = 'U')
   drop table t_sys_rights_user_role
go

/*==============================================================*/
/* Table: t_sys_rights_role                                     */
/*==============================================================*/
create table t_sys_rights_role (
   id                   int                  identity,
   role_name            nvarchar(128)        null,
   constraint PK_T_SYS_RIGHTS_ROLE primary key (id)
)
go

if exists (select 1 from  sys.extended_properties
           where major_id = object_id('t_sys_rights_role') and minor_id = 0)
begin 
   declare @CurrentUser sysname 
select @CurrentUser = user_name() 
execute sp_dropextendedproperty 'MS_Description',  
   'user', @CurrentUser, 'table', 't_sys_rights_role' 
 
end 


select @CurrentUser = user_name() 
execute sp_addextendedproperty 'MS_Description',  
   '角色表
   ', 
   'user', @CurrentUser, 'table', 't_sys_rights_role'
go

/*==============================================================*/
/* Table: t_sys_rights_user                                     */
/*==============================================================*/
create table t_sys_rights_user (
   id                   int                  identity,
   user_id              nvarchar(32)         null,
   user_name            nvarchar(32)         null,
   email                nvarchar(32)         null,
   address              nvarchar(128)        null,
   enable_flag          bit                  null,
   constraint PK_T_SYS_RIGHTS_USER primary key (id)
)
go

if exists (select 1 from  sys.extended_properties
           where major_id = object_id('t_sys_rights_user') and minor_id = 0)
begin 
   declare @CurrentUser sysname 
select @CurrentUser = user_name() 
execute sp_dropextendedproperty 'MS_Description',  
   'user', @CurrentUser, 'table', 't_sys_rights_user' 
 
end 


select @CurrentUser = user_name() 
execute sp_addextendedproperty 'MS_Description',  
   '用户表', 
   'user', @CurrentUser, 'table', 't_sys_rights_user'
go

/*==============================================================*/
/* Table: t_sys_rights_user_role                                */
/*==============================================================*/
create table t_sys_rights_user_role (
   id                   int                  identity,
   user_id              int                  null,
   role_id              int                  null,
   constraint PK_T_SYS_RIGHTS_USER_ROLE primary key (id)
)
go

if exists (select 1 from  sys.extended_properties
           where major_id = object_id('t_sys_rights_user_role') and minor_id = 0)
begin 
   declare @CurrentUser sysname 
select @CurrentUser = user_name() 
execute sp_dropextendedproperty 'MS_Description',  
   'user', @CurrentUser, 'table', 't_sys_rights_user_role' 
 
end 


select @CurrentUser = user_name() 
execute sp_addextendedproperty 'MS_Description',  
   '用户-角色表', 
   'user', @CurrentUser, 'table', 't_sys_rights_user_role'
go

