CREATE DATABASE blazon_database
GO
USE [blazon_database]
GO
/****** Object:  Table [dbo].[blazons]    Script Date: 7/28/2016 6:51:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[blazons](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](500) NULL,
	[blazon] [varchar](500) NULL,
	[shape] [int] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[blazons] ON 

INSERT [dbo].[blazons] ([id], [name], [blazon], [shape]) VALUES (3, N'Default', N'per pale per fess sable a chief or and purpure overall a bend argent and vert 11 mullets argent overall a lozenge azure', 3)
INSERT [dbo].[blazons] ([id], [name], [blazon], [shape]) VALUES (10, N'Pokeball', N'per fess gules and argent overall 1 fess sable 1 escutcheon argent', 1)
INSERT [dbo].[blazons] ([id], [name], [blazon], [shape]) VALUES (11, N'Austria', N'gules 1 fess argent', 2)
INSERT [dbo].[blazons] ([id], [name], [blazon], [shape]) VALUES (12, N'Britian', N'azure 1 saltire argent 1 cross gules', 3)
INSERT [dbo].[blazons] ([id], [name], [blazon], [shape]) VALUES (13, N'Vietnam', N'gules 1 mullet or', 4)
INSERT [dbo].[blazons] ([id], [name], [blazon], [shape]) VALUES (14, N'Brittany', N'ermine', 0)
INSERT [dbo].[blazons] ([id], [name], [blazon], [shape]) VALUES (6, N'America', N'quarterly i azure 50 mullet argent ii gules 1 fess argent iii argent 1 fess gules iv argent 1 fess gules', 0)
INSERT [dbo].[blazons] ([id], [name], [blazon], [shape]) VALUES (7, N'Earl of Bumble', N'per fess or 1 fess sable and sable 1 fess or overall 2 lozenge argent', 1)
SET IDENTITY_INSERT [dbo].[blazons] OFF
