USE [DapperDemoDB]
GO
/****** Object:  Table [dbo].[t_sys_rights_user_role]    Script Date: 10/17/2016 15:17:12 ******/
SET IDENTITY_INSERT [dbo].[t_sys_rights_user_role] ON
INSERT [dbo].[t_sys_rights_user_role] ([id], [user_id], [role_id]) VALUES (1, 1, 1)
INSERT [dbo].[t_sys_rights_user_role] ([id], [user_id], [role_id]) VALUES (2, 1, 2)
INSERT [dbo].[t_sys_rights_user_role] ([id], [user_id], [role_id]) VALUES (3, 1, 3)
INSERT [dbo].[t_sys_rights_user_role] ([id], [user_id], [role_id]) VALUES (4, 2, 1)
INSERT [dbo].[t_sys_rights_user_role] ([id], [user_id], [role_id]) VALUES (5, 2, 2)
INSERT [dbo].[t_sys_rights_user_role] ([id], [user_id], [role_id]) VALUES (6, 2, 3)
INSERT [dbo].[t_sys_rights_user_role] ([id], [user_id], [role_id]) VALUES (7, 3, 2)
INSERT [dbo].[t_sys_rights_user_role] ([id], [user_id], [role_id]) VALUES (8, 4, 3)
SET IDENTITY_INSERT [dbo].[t_sys_rights_user_role] OFF
/****** Object:  Table [dbo].[t_sys_rights_user]    Script Date: 10/17/2016 15:17:12 ******/
SET IDENTITY_INSERT [dbo].[t_sys_rights_user] ON
INSERT [dbo].[t_sys_rights_user] ([id], [user_id], [user_name], [email], [address], [enable_flag]) VALUES (1, N'admin', N'系统管理员', N'admin@qq.com', N'广东深圳', 1)
INSERT [dbo].[t_sys_rights_user] ([id], [user_id], [user_name], [email], [address], [enable_flag]) VALUES (2, N'mcgrady', N'麦迪', N'mcgrady@qq.com', N'美国休斯顿', 1)
INSERT [dbo].[t_sys_rights_user] ([id], [user_id], [user_name], [email], [address], [enable_flag]) VALUES (3, N'kobe', N'科比', N'kobe@qq.com', N'美国洛杉矶', 1)
INSERT [dbo].[t_sys_rights_user] ([id], [user_id], [user_name], [email], [address], [enable_flag]) VALUES (4, N'james', N'詹姆斯', N'james@qq.com', N'美国克利夫兰', 1)
INSERT [dbo].[t_sys_rights_user] ([id], [user_id], [user_name], [email], [address], [enable_flag]) VALUES (5, N'wade', N'韦德', N'wade@qq.com', N'美国芝加哥', 1)
INSERT [dbo].[t_sys_rights_user] ([id], [user_id], [user_name], [email], [address], [enable_flag]) VALUES (6, N'userId0', N'userName0', N'email0', N'address0', 1)
INSERT [dbo].[t_sys_rights_user] ([id], [user_id], [user_name], [email], [address], [enable_flag]) VALUES (7, N'userId1', N'userName1', N'email1', N'address1', 1)
INSERT [dbo].[t_sys_rights_user] ([id], [user_id], [user_name], [email], [address], [enable_flag]) VALUES (10, N'userId4', N'userName4', N'email4', N'address4', 1)
SET IDENTITY_INSERT [dbo].[t_sys_rights_user] OFF
/****** Object:  Table [dbo].[t_sys_rights_role]    Script Date: 10/17/2016 15:17:12 ******/
SET IDENTITY_INSERT [dbo].[t_sys_rights_role] ON
INSERT [dbo].[t_sys_rights_role] ([id], [role_name]) VALUES (1, N'admin')
INSERT [dbo].[t_sys_rights_role] ([id], [role_name]) VALUES (2, N'guest')
INSERT [dbo].[t_sys_rights_role] ([id], [role_name]) VALUES (3, N'test')
SET IDENTITY_INSERT [dbo].[t_sys_rights_role] OFF
