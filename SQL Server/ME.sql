USE [master]
GO
/****** Object:  Database [ManufacturingExecution]    Script Date: 03.12.2018 09:56:57 ******/
CREATE DATABASE [ManufacturingExecution]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ManufacturingExecution', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\ManufacturingExecution.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ManufacturingExecution_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\ManufacturingExecution_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [ManufacturingExecution] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ManufacturingExecution].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ManufacturingExecution] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ManufacturingExecution] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ManufacturingExecution] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ManufacturingExecution] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ManufacturingExecution] SET ARITHABORT OFF 
GO
ALTER DATABASE [ManufacturingExecution] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ManufacturingExecution] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ManufacturingExecution] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ManufacturingExecution] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ManufacturingExecution] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ManufacturingExecution] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ManufacturingExecution] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ManufacturingExecution] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ManufacturingExecution] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ManufacturingExecution] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ManufacturingExecution] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ManufacturingExecution] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ManufacturingExecution] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ManufacturingExecution] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ManufacturingExecution] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ManufacturingExecution] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ManufacturingExecution] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ManufacturingExecution] SET RECOVERY FULL 
GO
ALTER DATABASE [ManufacturingExecution] SET  MULTI_USER 
GO
ALTER DATABASE [ManufacturingExecution] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ManufacturingExecution] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ManufacturingExecution] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ManufacturingExecution] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ManufacturingExecution] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'ManufacturingExecution', N'ON'
GO
ALTER DATABASE [ManufacturingExecution] SET QUERY_STORE = OFF
GO
USE [ManufacturingExecution]
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [ManufacturingExecution]
GO
/****** Object:  Table [dbo].[ME_ACTIVITY]    Script Date: 03.12.2018 09:56:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ME_ACTIVITY](
	[ID] [nvarchar](50) NOT NULL,
	[NAME] [nvarchar](50) NULL,
	[PARAMETERS] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ME_BOM]    Script Date: 03.12.2018 09:56:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ME_BOM](
	[ID] [nvarchar](50) NOT NULL,
	[NAME] [nvarchar](50) NOT NULL,
	[DESCRIPTION] [nvarchar](100) NULL,
	[STATUS] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ME_BOM_DETAILS]    Script Date: 03.12.2018 09:56:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ME_BOM_DETAILS](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BOM] [nvarchar](50) NOT NULL,
	[SEQUENCE] [int] NOT NULL,
	[COMPONENT] [nvarchar](50) NOT NULL,
	[OPERATION] [nvarchar](50) NOT NULL,
	[ASSEMBLY_QUANTITY] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ME_BOM_STATUS]    Script Date: 03.12.2018 09:56:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ME_BOM_STATUS](
	[ID] [int] NOT NULL,
	[NAME] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ME_MATERIAL]    Script Date: 03.12.2018 09:56:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ME_MATERIAL](
	[ID] [nvarchar](50) NOT NULL,
	[NAME] [nvarchar](50) NOT NULL,
	[DESCRIPTION] [nvarchar](100) NULL,
	[STATUS] [int] NOT NULL,
	[LOT_SIZE] [int] NOT NULL,
	[TYPE] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ME_MATERIAL_STATUS]    Script Date: 03.12.2018 09:56:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ME_MATERIAL_STATUS](
	[ID] [int] NOT NULL,
	[STATUS] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ME_MATERIAL_TYPE]    Script Date: 03.12.2018 09:56:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ME_MATERIAL_TYPE](
	[ID] [int] NOT NULL,
	[TYPE] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ME_OPERATION]    Script Date: 03.12.2018 09:56:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ME_OPERATION](
	[ID] [nvarchar](50) NOT NULL,
	[NAME] [nvarchar](50) NOT NULL,
	[DESCRIPTION] [nvarchar](100) NULL,
	[STATUS] [int] NOT NULL,
	[RESOURCE] [nvarchar](50) NOT NULL,
	[WORKCENTER] [nvarchar](50) NOT NULL,
	[ACTIVITY] [nvarchar](50) NULL,
	[RESOURCETYPE] [int] NOT NULL,
	[OPERATIONTYPE] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ME_OPERATION_STATUS]    Script Date: 03.12.2018 09:56:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ME_OPERATION_STATUS](
	[ID] [int] NOT NULL,
	[NAME] [nvarchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ME_OPERATION_TYPE]    Script Date: 03.12.2018 09:56:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ME_OPERATION_TYPE](
	[ID] [nvarchar](50) NOT NULL,
	[TYPE] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ME_RESOURCE]    Script Date: 03.12.2018 09:56:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ME_RESOURCE](
	[ID] [nvarchar](50) NOT NULL,
	[NAME] [nvarchar](50) NOT NULL,
	[DESCRIPTION] [nvarchar](100) NULL,
	[STATUS] [int] NOT NULL,
	[TYPE] [int] NOT NULL,
	[OPERATION] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ME_RESOURCE_MATERIAL]    Script Date: 03.12.2018 09:56:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ME_RESOURCE_MATERIAL](
	[RESOURCE] [nvarchar](50) NOT NULL,
	[MATERIAL] [nvarchar](50) NOT NULL,
	[QUANTITY] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ME_RESOURCE_STATUS]    Script Date: 03.12.2018 09:56:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ME_RESOURCE_STATUS](
	[ID] [int] NOT NULL,
	[NAME] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ME_RESOURCE_TYPE]    Script Date: 03.12.2018 09:56:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ME_RESOURCE_TYPE](
	[ID] [int] NOT NULL,
	[TYPE] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ME_ROUTING]    Script Date: 03.12.2018 09:56:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ME_ROUTING](
	[ID] [nvarchar](50) NOT NULL,
	[NAME] [nvarchar](50) NOT NULL,
	[DESCRIPTION] [nvarchar](100) NULL,
	[TYPE] [int] NOT NULL,
	[STATUS] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ME_ROUTING_DETAILS]    Script Date: 03.12.2018 09:56:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ME_ROUTING_DETAILS](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ROUTING] [nvarchar](50) NOT NULL,
	[SEQUENCE] [int] NULL,
	[OPERATION] [nvarchar](50) NULL,
	[CONDITION] [nvarchar](4000) NULL,
	[NEXT_OPERATION] [nvarchar](50) NULL,
	[NC_OPERATION] [nvarchar](50) NULL,
	[SEMANTIC_ANNOTATION] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ME_ROUTING_STATUS]    Script Date: 03.12.2018 09:56:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ME_ROUTING_STATUS](
	[ID] [int] NOT NULL,
	[NAME] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ME_ROUTING_TYPE]    Script Date: 03.12.2018 09:56:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ME_ROUTING_TYPE](
	[ID] [int] NOT NULL,
	[NAME] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ME_SEMANTIC_ANNOTATION]    Script Date: 03.12.2018 09:56:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ME_SEMANTIC_ANNOTATION](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[IRI] [nvarchar](500) NULL,
	[SHORT_NAME] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ME_SETPOINT]    Script Date: 03.12.2018 09:56:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ME_SETPOINT](
	[ID] [nvarchar](50) NOT NULL,
	[NAME] [nvarchar](50) NOT NULL,
	[DESCRIPTION] [nvarchar](100) NOT NULL,
	[DATA_TYPE] [int] NOT NULL,
	[NUMERIC_VALUE] [int] NULL,
	[STRING_VALUE] [nvarchar](50) NULL,
	[STATUS] [int] NOT NULL,
	[START_DATE] [date] NOT NULL,
	[END_DATE] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ME_SETPOINT_DETAILS]    Script Date: 03.12.2018 09:56:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ME_SETPOINT_DETAILS](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SETPOINT] [nvarchar](50) NOT NULL,
	[RESOURCE] [nvarchar](50) NOT NULL,
	[MATERIAL] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ME_SETPOINT_STATUS]    Script Date: 03.12.2018 09:56:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ME_SETPOINT_STATUS](
	[ID] [int] NOT NULL,
	[NAME] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ME_SETPOINT_TYPE]    Script Date: 03.12.2018 09:56:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ME_SETPOINT_TYPE](
	[ID] [int] NOT NULL,
	[NAME] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ME_SHOP_ORDER]    Script Date: 03.12.2018 09:56:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ME_SHOP_ORDER](
	[ID] [nvarchar](50) NOT NULL,
	[NAME] [nvarchar](50) NOT NULL,
	[DESCRIPTION] [nvarchar](100) NULL,
	[STATUS] [int] NOT NULL,
	[TYPE] [int] NOT NULL,
	[PLANNED_MATERIAL] [nvarchar](50) NOT NULL,
	[BOM] [nvarchar](50) NOT NULL,
	[ROUTING] [nvarchar](50) NOT NULL,
	[BUILD_QUANTITY] [int] NOT NULL,
	[CUSTOMER_ORDER] [nvarchar](50) NOT NULL,
	[PLANNED_START] [date] NOT NULL,
	[PLANNED_COMPLETION] [date] NOT NULL,
	[PRIORITY] [int] NOT NULL,
	[RELEASE_QUANTITY] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ME_SHOP_ORDER_STATUS]    Script Date: 03.12.2018 09:56:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ME_SHOP_ORDER_STATUS](
	[ID] [int] NOT NULL,
	[NAME] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ME_SHOP_ORDER_TYPE]    Script Date: 03.12.2018 09:56:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ME_SHOP_ORDER_TYPE](
	[ID] [int] NOT NULL,
	[NAME] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ME_WORKCENTER]    Script Date: 03.12.2018 09:56:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ME_WORKCENTER](
	[ID] [nvarchar](50) NOT NULL,
	[NAME] [nvarchar](50) NOT NULL,
	[DESCRIPTION] [nvarchar](100) NULL,
	[STATUS] [int] NOT NULL,
	[CATEGORY] [int] NOT NULL,
	[TYPE] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ME_WORKCENTER_CATEGORY]    Script Date: 03.12.2018 09:56:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ME_WORKCENTER_CATEGORY](
	[ID] [int] NOT NULL,
	[NAME] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ME_WORKCENTER_RESOURCE]    Script Date: 03.12.2018 09:56:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ME_WORKCENTER_RESOURCE](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[WORKCENTER_ID] [nvarchar](50) NOT NULL,
	[RESOURCE_ID] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ME_WORKCENTER_STATUS]    Script Date: 03.12.2018 09:56:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ME_WORKCENTER_STATUS](
	[ID] [int] NOT NULL,
	[NAME] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ME_WORKCENTER_TYPE]    Script Date: 03.12.2018 09:56:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ME_WORKCENTER_TYPE](
	[ID] [int] NOT NULL,
	[NAME] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
INSERT [dbo].[ME_BOM] ([ID], [NAME], [DESCRIPTION], [STATUS]) VALUES (N'193dd336-55aa-4f0d-acd7-a874bdf8d154', N'Key Finder BOM', N'BOM for Key Finder', 5)
INSERT [dbo].[ME_BOM_STATUS] ([ID], [NAME]) VALUES (1, N'New')
INSERT [dbo].[ME_BOM_STATUS] ([ID], [NAME]) VALUES (2, N'Frozen')
INSERT [dbo].[ME_BOM_STATUS] ([ID], [NAME]) VALUES (3, N'Hold')
INSERT [dbo].[ME_BOM_STATUS] ([ID], [NAME]) VALUES (4, N'Obsolete')
INSERT [dbo].[ME_BOM_STATUS] ([ID], [NAME]) VALUES (5, N'Releasable')
INSERT [dbo].[ME_MATERIAL] ([ID], [NAME], [DESCRIPTION], [STATUS], [LOT_SIZE], [TYPE]) VALUES (N'1641a3ae-1ef5-4032-9011-087e78db3086', N'Circuit Board', N'The circuit board for Key finder module', 1, 1, 1)
INSERT [dbo].[ME_MATERIAL] ([ID], [NAME], [DESCRIPTION], [STATUS], [LOT_SIZE], [TYPE]) VALUES (N'4b3af06f-570a-4343-939f-565774af4e99', N'Upper Shell', N'Upper Shell for Key finder module', 1, 1, 1)
INSERT [dbo].[ME_MATERIAL] ([ID], [NAME], [DESCRIPTION], [STATUS], [LOT_SIZE], [TYPE]) VALUES (N'5c749b0c-1e58-4606-bce9-8b644108c2ba', N'Key Finder', N'This is a assembly where circuit board is sandwiched between upper and lower shells.', 1, 1, 2)
INSERT [dbo].[ME_MATERIAL] ([ID], [NAME], [DESCRIPTION], [STATUS], [LOT_SIZE], [TYPE]) VALUES (N'cccc178e-ebc3-4dd4-ad36-5085ff08ed80', N'Lower Shell', N'Lower Shell for Key Finder module', 1, 1, 1)
INSERT [dbo].[ME_MATERIAL_STATUS] ([ID], [STATUS]) VALUES (1, N'Releasable')
INSERT [dbo].[ME_MATERIAL_STATUS] ([ID], [STATUS]) VALUES (2, N'Frozen')
INSERT [dbo].[ME_MATERIAL_STATUS] ([ID], [STATUS]) VALUES (3, N'Hold')
INSERT [dbo].[ME_MATERIAL_STATUS] ([ID], [STATUS]) VALUES (4, N'New')
INSERT [dbo].[ME_MATERIAL_STATUS] ([ID], [STATUS]) VALUES (5, N'Obsolete')
INSERT [dbo].[ME_MATERIAL_TYPE] ([ID], [TYPE]) VALUES (1, N'Purchased')
INSERT [dbo].[ME_MATERIAL_TYPE] ([ID], [TYPE]) VALUES (2, N'Manufactured')
INSERT [dbo].[ME_OPERATION] ([ID], [NAME], [DESCRIPTION], [STATUS], [RESOURCE], [WORKCENTER], [ACTIVITY], [RESOURCETYPE], [OPERATIONTYPE]) VALUES (N'1b7f6789-e9b1-4045-ab68-40c4f939e753', N'Transportation Operation', N'This operation transports the sub-component or the assembly to respective destination', 1, N'1f816f97-9b70-4cad-aa41-be56e79d8af9', N'4128e430-1f2e-4f6a-9bcf-f5bbbe6e4ec2', NULL, 1, N'8391a7a0-8c11-44a7-9b6d-d0c4e4a08b25')
INSERT [dbo].[ME_OPERATION] ([ID], [NAME], [DESCRIPTION], [STATUS], [RESOURCE], [WORKCENTER], [ACTIVITY], [RESOURCETYPE], [OPERATIONTYPE]) VALUES (N'1b87115e-1b74-4348-9a19-8812b6051096', N'Automated Manufacturing Find Operation', N'This operation finds required service for the production step', 1, N'1f816f97-9b70-4cad-aa41-be56e79d8af9', N'4128e430-1f2e-4f6a-9bcf-f5bbbe6e4ec2', NULL, 1, N'82bde4f7-cc2e-4de0-9700-d9f9d2f6e49a')
INSERT [dbo].[ME_OPERATION] ([ID], [NAME], [DESCRIPTION], [STATUS], [RESOURCE], [WORKCENTER], [ACTIVITY], [RESOURCETYPE], [OPERATIONTYPE]) VALUES (N'4816c2f9-e33e-4221-952d-8daf8b2d97b1', N'Assembly Operation', N'This operation assembles the sub-components to key finder module', 1, N'1f816f97-9b70-4cad-aa41-be56e79d8af9', N'4128e430-1f2e-4f6a-9bcf-f5bbbe6e4ec2', NULL, 1, N'2ebf5c7a-4551-49e4-a1d6-03033d16e1eb')
INSERT [dbo].[ME_OPERATION] ([ID], [NAME], [DESCRIPTION], [STATUS], [RESOURCE], [WORKCENTER], [ACTIVITY], [RESOURCETYPE], [OPERATIONTYPE]) VALUES (N'5c89c472-4d16-49d3-b39d-b7ea922aa203', N'RFID Read Operation', N'This operation reads the RFID chip on the sub-component, and performs validation', 1, N'1f816f97-9b70-4cad-aa41-be56e79d8af9', N'4128e430-1f2e-4f6a-9bcf-f5bbbe6e4ec2', NULL, 1, N'f5b26750-ee35-4a35-baad-8ddba607e2dc')
INSERT [dbo].[ME_OPERATION] ([ID], [NAME], [DESCRIPTION], [STATUS], [RESOURCE], [WORKCENTER], [ACTIVITY], [RESOURCETYPE], [OPERATIONTYPE]) VALUES (N'a0006829-aaa3-4cf9-97ab-a547abc99d4d', N'Place Operation', N'This operation places the components of key finder or whole assembly to respective destination', 1, N'1f816f97-9b70-4cad-aa41-be56e79d8af9', N'4128e430-1f2e-4f6a-9bcf-f5bbbe6e4ec2', NULL, 1, N'0ed860a9-fbc0-4ca2-a9e6-caef17c9d651')
INSERT [dbo].[ME_OPERATION] ([ID], [NAME], [DESCRIPTION], [STATUS], [RESOURCE], [WORKCENTER], [ACTIVITY], [RESOURCETYPE], [OPERATIONTYPE]) VALUES (N'aad188e8-bebe-4dae-8db5-87042d4400de', N'Pick Operation', N'This operation picks the components of key finder or whole assembly to respective destination', 1, N'1f816f97-9b70-4cad-aa41-be56e79d8af9', N'4128e430-1f2e-4f6a-9bcf-f5bbbe6e4ec2', NULL, 1, N'67a79715-459e-45fb-a763-08ab043eaa74')
INSERT [dbo].[ME_OPERATION] ([ID], [NAME], [DESCRIPTION], [STATUS], [RESOURCE], [WORKCENTER], [ACTIVITY], [RESOURCETYPE], [OPERATIONTYPE]) VALUES (N'd3832e61-75b9-4685-80f0-a441bca6fdc9', N'Material Provider Operation', N'This operation provides the sub-components of key finder to the logistics', 1, N'1f816f97-9b70-4cad-aa41-be56e79d8af9', N'4128e430-1f2e-4f6a-9bcf-f5bbbe6e4ec2', NULL, 1, N'7fe880f9-d2fa-4574-a461-60ad068edbd9')
INSERT [dbo].[ME_OPERATION] ([ID], [NAME], [DESCRIPTION], [STATUS], [RESOURCE], [WORKCENTER], [ACTIVITY], [RESOURCETYPE], [OPERATIONTYPE]) VALUES (N'df8d40a6-6e2c-4fee-a570-e3d6ed412b6a', N'2D Matrix and Color Detection Operation', N'This operation captures the image and recognizes the patterns/colors', 1, N'1f816f97-9b70-4cad-aa41-be56e79d8af9', N'4128e430-1f2e-4f6a-9bcf-f5bbbe6e4ec2', NULL, 1, N'9d49d0f0-0024-4144-be33-17dc6690934e')
INSERT [dbo].[ME_OPERATION_STATUS] ([ID], [NAME]) VALUES (1, N'Releasable')
INSERT [dbo].[ME_OPERATION_STATUS] ([ID], [NAME]) VALUES (2, N'Obsolete')
INSERT [dbo].[ME_OPERATION_STATUS] ([ID], [NAME]) VALUES (3, N'Frozen')
INSERT [dbo].[ME_OPERATION_STATUS] ([ID], [NAME]) VALUES (4, N'Hold')
INSERT [dbo].[ME_OPERATION_STATUS] ([ID], [NAME]) VALUES (5, N'New')
INSERT [dbo].[ME_OPERATION_TYPE] ([ID], [TYPE]) VALUES (N'0ed860a9-fbc0-4ca2-a9e6-caef17c9d651', N'Place Service')
INSERT [dbo].[ME_OPERATION_TYPE] ([ID], [TYPE]) VALUES (N'2ebf5c7a-4551-49e4-a1d6-03033d16e1eb', N'Assembly Service')
INSERT [dbo].[ME_OPERATION_TYPE] ([ID], [TYPE]) VALUES (N'67a79715-459e-45fb-a763-08ab043eaa74', N'Pick Service')
INSERT [dbo].[ME_OPERATION_TYPE] ([ID], [TYPE]) VALUES (N'7fe880f9-d2fa-4574-a461-60ad068edbd9', N'Material Provider Service')
INSERT [dbo].[ME_OPERATION_TYPE] ([ID], [TYPE]) VALUES (N'82bde4f7-cc2e-4de0-9700-d9f9d2f6e49a', N'Find Manufacturing Service')
INSERT [dbo].[ME_OPERATION_TYPE] ([ID], [TYPE]) VALUES (N'8391a7a0-8c11-44a7-9b6d-d0c4e4a08b25', N'Transportation Service')
INSERT [dbo].[ME_OPERATION_TYPE] ([ID], [TYPE]) VALUES (N'9d49d0f0-0024-4144-be33-17dc6690934e', N'Camera Service')
INSERT [dbo].[ME_OPERATION_TYPE] ([ID], [TYPE]) VALUES (N'f5b26750-ee35-4a35-baad-8ddba607e2dc', N'RFID Read Service')
INSERT [dbo].[ME_RESOURCE] ([ID], [NAME], [DESCRIPTION], [STATUS], [TYPE], [OPERATION]) VALUES (N'189ded05-6ff4-4ab7-a110-64bc07d3b82a', N'Electric Press', N'Final assembly of Key Finder', 1, 1, NULL)
INSERT [dbo].[ME_RESOURCE] ([ID], [NAME], [DESCRIPTION], [STATUS], [TYPE], [OPERATION]) VALUES (N'1f816f97-9b70-4cad-aa41-be56e79d8af9', N'Model Resource', N'This resource is for conformant purposes', 1, 1, NULL)
INSERT [dbo].[ME_RESOURCE] ([ID], [NAME], [DESCRIPTION], [STATUS], [TYPE], [OPERATION]) VALUES (N'27dc8ac8-a2a9-478f-9dae-dc49d9d82236', N'Key Finder Storage', N'Stores the manufactured Key Finder', 1, 1, NULL)
INSERT [dbo].[ME_RESOURCE] ([ID], [NAME], [DESCRIPTION], [STATUS], [TYPE], [OPERATION]) VALUES (N'36de96cd-1a41-49da-83d5-92d0950502e1', N'PicknPlace Robot', N'Logistics', 1, 1, NULL)
INSERT [dbo].[ME_RESOURCE] ([ID], [NAME], [DESCRIPTION], [STATUS], [TYPE], [OPERATION]) VALUES (N'6fd74c19-5b58-4f95-ac3e-e8a63accd178', N'Conveyer Belt', N'Logistics', 1, 1, NULL)
INSERT [dbo].[ME_RESOURCE] ([ID], [NAME], [DESCRIPTION], [STATUS], [TYPE], [OPERATION]) VALUES (N'a70294f2-7a9b-46a5-bbe5-78f66a07296b', N'Industrial Camera', N'Detects data matrix code', 1, 1, NULL)
INSERT [dbo].[ME_RESOURCE] ([ID], [NAME], [DESCRIPTION], [STATUS], [TYPE], [OPERATION]) VALUES (N'b27f1257-a7e5-4666-b5a0-2a2ee1caf9e7', N'Pneumatic Press', N'Final assembly of Key Finder', 1, 1, NULL)
INSERT [dbo].[ME_RESOURCE] ([ID], [NAME], [DESCRIPTION], [STATUS], [TYPE], [OPERATION]) VALUES (N'b76236f8-fe4c-4f96-bd77-1621660738d9', N'PlugnPlay Pneumatic Press', N'Final assembly of Key Finder', 1, 1, NULL)
INSERT [dbo].[ME_RESOURCE] ([ID], [NAME], [DESCRIPTION], [STATUS], [TYPE], [OPERATION]) VALUES (N'c601a82a-cc39-4143-8a24-8495b9b4acbd', N'RFID Reader', N'Reads the RFID chip for upper shell validation', 1, 1, NULL)
INSERT [dbo].[ME_RESOURCE] ([ID], [NAME], [DESCRIPTION], [STATUS], [TYPE], [OPERATION]) VALUES (N'd227171e-6fb2-4083-8bd1-9ef9875f9538', N'Upper Shell Provider', N'Upper Shell warehouse', 1, 1, NULL)
INSERT [dbo].[ME_RESOURCE] ([ID], [NAME], [DESCRIPTION], [STATUS], [TYPE], [OPERATION]) VALUES (N'e813f135-27eb-4411-b593-efb79a5e530e', N'Lower Shell Provider', N'Lower shell warehouse ', 1, 1, NULL)
INSERT [dbo].[ME_RESOURCE] ([ID], [NAME], [DESCRIPTION], [STATUS], [TYPE], [OPERATION]) VALUES (N'f9ae0ab1-2ab3-4594-8189-ed2a86a77bcb', N'Circuit Board Provider', N'Circuit Board warehouse', 1, 1, NULL)
INSERT [dbo].[ME_RESOURCE_STATUS] ([ID], [NAME]) VALUES (1, N'Enabled')
INSERT [dbo].[ME_RESOURCE_STATUS] ([ID], [NAME]) VALUES (2, N'Disabled')
INSERT [dbo].[ME_RESOURCE_STATUS] ([ID], [NAME]) VALUES (3, N'OnHold')
INSERT [dbo].[ME_RESOURCE_STATUS] ([ID], [NAME]) VALUES (4, N'Scheduled Down')
INSERT [dbo].[ME_RESOURCE_STATUS] ([ID], [NAME]) VALUES (5, N'Unschdeduled Down')
INSERT [dbo].[ME_RESOURCE_TYPE] ([ID], [TYPE]) VALUES (1, N'Assembly')
INSERT [dbo].[ME_RESOURCE_TYPE] ([ID], [TYPE]) VALUES (2, N'Test')
INSERT [dbo].[ME_RESOURCE_TYPE] ([ID], [TYPE]) VALUES (3, N'Pack')
INSERT [dbo].[ME_RESOURCE_TYPE] ([ID], [TYPE]) VALUES (4, N'Non-Resource')
INSERT [dbo].[ME_ROUTING] ([ID], [NAME], [DESCRIPTION], [TYPE], [STATUS]) VALUES (N'c53aebb8-ea62-4d96-bf71-e766d3a64496', N'Key Finder Module Assembly Routing', N'Sequences of operations for Key Finder Assembly', 1, 1)
SET IDENTITY_INSERT [dbo].[ME_ROUTING_DETAILS] ON 

INSERT [dbo].[ME_ROUTING_DETAILS] ([ID], [ROUTING], [SEQUENCE], [OPERATION], [CONDITION], [NEXT_OPERATION], [NC_OPERATION], [SEMANTIC_ANNOTATION]) VALUES (50, N'c53aebb8-ea62-4d96-bf71-e766d3a64496', 1, N'1b87115e-1b74-4348-9a19-8812b6051096', NULL, N'8391a7a0-8c11-44a7-9b6d-d0c4e4a08b25', N'1b7f6789-e9b1-4045-ab68-40c4f939e753', 18)
INSERT [dbo].[ME_ROUTING_DETAILS] ([ID], [ROUTING], [SEQUENCE], [OPERATION], [CONDITION], [NEXT_OPERATION], [NC_OPERATION], [SEMANTIC_ANNOTATION]) VALUES (51, N'c53aebb8-ea62-4d96-bf71-e766d3a64496', 2, N'1b87115e-1b74-4348-9a19-8812b6051096', NULL, N'7fe880f9-d2fa-4574-a461-60ad068edbd9', N'1b7f6789-e9b1-4045-ab68-40c4f939e753', 39)
INSERT [dbo].[ME_ROUTING_DETAILS] ([ID], [ROUTING], [SEQUENCE], [OPERATION], [CONDITION], [NEXT_OPERATION], [NC_OPERATION], [SEMANTIC_ANNOTATION]) VALUES (52, N'c53aebb8-ea62-4d96-bf71-e766d3a64496', 3, N'1b87115e-1b74-4348-9a19-8812b6051096', NULL, N'7fe880f9-d2fa-4574-a461-60ad068edbd9', N'1b7f6789-e9b1-4045-ab68-40c4f939e753', 25)
INSERT [dbo].[ME_ROUTING_DETAILS] ([ID], [ROUTING], [SEQUENCE], [OPERATION], [CONDITION], [NEXT_OPERATION], [NC_OPERATION], [SEMANTIC_ANNOTATION]) VALUES (53, N'c53aebb8-ea62-4d96-bf71-e766d3a64496', 4, N'1b87115e-1b74-4348-9a19-8812b6051096', NULL, N'7fe880f9-d2fa-4574-a461-60ad068edbd9', N'1b7f6789-e9b1-4045-ab68-40c4f939e753', 28)
INSERT [dbo].[ME_ROUTING_DETAILS] ([ID], [ROUTING], [SEQUENCE], [OPERATION], [CONDITION], [NEXT_OPERATION], [NC_OPERATION], [SEMANTIC_ANNOTATION]) VALUES (54, N'c53aebb8-ea62-4d96-bf71-e766d3a64496', 5, N'1b87115e-1b74-4348-9a19-8812b6051096', NULL, N'9d49d0f0-0024-4144-be33-17dc6690934e', N'1b7f6789-e9b1-4045-ab68-40c4f939e753', 36)
INSERT [dbo].[ME_ROUTING_DETAILS] ([ID], [ROUTING], [SEQUENCE], [OPERATION], [CONDITION], [NEXT_OPERATION], [NC_OPERATION], [SEMANTIC_ANNOTATION]) VALUES (55, N'c53aebb8-ea62-4d96-bf71-e766d3a64496', 6, N'1b87115e-1b74-4348-9a19-8812b6051096', NULL, N'f5b26750-ee35-4a35-baad-8ddba607e2dc', N'1b7f6789-e9b1-4045-ab68-40c4f939e753', 14)
INSERT [dbo].[ME_ROUTING_DETAILS] ([ID], [ROUTING], [SEQUENCE], [OPERATION], [CONDITION], [NEXT_OPERATION], [NC_OPERATION], [SEMANTIC_ANNOTATION]) VALUES (56, N'c53aebb8-ea62-4d96-bf71-e766d3a64496', 7, N'1b87115e-1b74-4348-9a19-8812b6051096', N'SPARQL QUERY - SHORTEST DISTANCE', N'2ebf5c7a-4551-49e4-a1d6-03033d16e1eb', N'1b7f6789-e9b1-4045-ab68-40c4f939e753', 38)
INSERT [dbo].[ME_ROUTING_DETAILS] ([ID], [ROUTING], [SEQUENCE], [OPERATION], [CONDITION], [NEXT_OPERATION], [NC_OPERATION], [SEMANTIC_ANNOTATION]) VALUES (57, N'c53aebb8-ea62-4d96-bf71-e766d3a64496', 8, N'1b7f6789-e9b1-4045-ab68-40c4f939e753', NULL, N'8391a7a0-8c11-44a7-9b6d-d0c4e4a08b25', N'1b7f6789-e9b1-4045-ab68-40c4f939e753', 39)
INSERT [dbo].[ME_ROUTING_DETAILS] ([ID], [ROUTING], [SEQUENCE], [OPERATION], [CONDITION], [NEXT_OPERATION], [NC_OPERATION], [SEMANTIC_ANNOTATION]) VALUES (58, N'c53aebb8-ea62-4d96-bf71-e766d3a64496', 10, N'aad188e8-bebe-4dae-8db5-87042d4400de', N'PRE_COND:rescom_resources_AutonomousTransporter(?at) ^ currentPositionX(?at, ?at_XCoOrd) ^ currentPositionY(?at, ?at_YCoOrd) ^ currentPositionZ(?at, ?at_ZCoOrd) ^ Value(?at_XCoOrd, ?at_xValue) ^ Value(?at_YCoOrd, ?at_yValue) ^ Value(?at_ZCoOrd, ?at_zValue) ^   rescom_resources_LowerShellProvider(?lsProvider) ^ hasPositionX(?lsProvider, ?lSProvider_XCoOrd) ^ hasPositionY(?lsProvider, ?lSProvider_YCoOrd) ^ hasPositionZ(?lsProvider, ?lSProvider_ZCoOrd) ^ Value(?lSProvider_XCoOrd, ?ls_xValue) ^ Value(?lSProvider_YCoOrd, ?ls_yValue) ^ Value(?lSProvider_ZCoOrd, ?ls_zValue) ^   swrlb:equal(?at_xValue, ?ls_xValue) ^ swrlb:equal(?at_yValue, ?ls_yValue) ^ swrlb:equal(?at_zValue, ?ls_zValue) -> sqwrl:select(true) ', N'0ed860a9-fbc0-4ca2-a9e6-caef17c9d651', N'1b7f6789-e9b1-4045-ab68-40c4f939e753', 39)
INSERT [dbo].[ME_ROUTING_DETAILS] ([ID], [ROUTING], [SEQUENCE], [OPERATION], [CONDITION], [NEXT_OPERATION], [NC_OPERATION], [SEMANTIC_ANNOTATION]) VALUES (59, N'c53aebb8-ea62-4d96-bf71-e766d3a64496', 11, N'1b7f6789-e9b1-4045-ab68-40c4f939e753', NULL, N'8391a7a0-8c11-44a7-9b6d-d0c4e4a08b25', N'1b7f6789-e9b1-4045-ab68-40c4f939e753', 36)
INSERT [dbo].[ME_ROUTING_DETAILS] ([ID], [ROUTING], [SEQUENCE], [OPERATION], [CONDITION], [NEXT_OPERATION], [NC_OPERATION], [SEMANTIC_ANNOTATION]) VALUES (60, N'c53aebb8-ea62-4d96-bf71-e766d3a64496', 12, N'df8d40a6-6e2c-4fee-a570-e3d6ed412b6a', N'POST_COND:rescom_product_LowerShell(?ls) ^ hasColor(?ls, White) -> isIdentified(?ls, true) ;', N'9d49d0f0-0024-4144-be33-17dc6690934e', N'1b7f6789-e9b1-4045-ab68-40c4f939e753', 36)
INSERT [dbo].[ME_ROUTING_DETAILS] ([ID], [ROUTING], [SEQUENCE], [OPERATION], [CONDITION], [NEXT_OPERATION], [NC_OPERATION], [SEMANTIC_ANNOTATION]) VALUES (61, N'c53aebb8-ea62-4d96-bf71-e766d3a64496', 13, N'1b7f6789-e9b1-4045-ab68-40c4f939e753', N'PRE_COND:rescom_product_LowerShell(?ls) ^ isIdentified(?ls, true) -> sqwrl:select(true) ; ', N'8391a7a0-8c11-44a7-9b6d-d0c4e4a08b25', N'1b7f6789-e9b1-4045-ab68-40c4f939e753', 38)
INSERT [dbo].[ME_ROUTING_DETAILS] ([ID], [ROUTING], [SEQUENCE], [OPERATION], [CONDITION], [NEXT_OPERATION], [NC_OPERATION], [SEMANTIC_ANNOTATION]) VALUES (62, N'c53aebb8-ea62-4d96-bf71-e766d3a64496', 14, N'a0006829-aaa3-4cf9-97ab-a547abc99d4d', N' PRE_COND:rescom_resources_AutonomousTransporter(?at) ^ currentPositionX(?at, ?at_XCoOrd) ^ currentPositionY(?at, ?at_YCoOrd) ^ currentPositionZ(?at, ?at_ZCoOrd) ^  Value(?at_XCoOrd, ?at_xValue) ^ Value(?at_YCoOrd, ?at_yValue) ^ Value(?at_ZCoOrd, ?at_zValue) ^   rescom_capability_abstracts_Assembler(?assmblr) ^ hasPositionX(?assmblr, ?assmblr_XCoOrd) ^ hasPositionY(?assmblr, ?assmblr_YCoOrd) ^ hasPositionZ(?assmblr, ?assmblr_ZCoOrd) ^  Value(?assmblr_XCoOrd, ?as_xValue) ^ Value(?assmblr_YCoOrd, ?as_yValue) ^ Value(?assmblr_ZCoOrd, ?as_zValue) ^   swrlb:equal(?at_xValue, ?as_xValue) ^ swrlb:equal(?at_yValue, ?as_yValue) ^ swrlb:equal(?at_zValue, ?as_zValue)  -> sqwrl:select(true) ; POST_COND:rescom_product_LowerShell(?ls) -> isPlacedInAssembly(?ls, true) ', N'0ed860a9-fbc0-4ca2-a9e6-caef17c9d651', N'1b7f6789-e9b1-4045-ab68-40c4f939e753', 38)
INSERT [dbo].[ME_ROUTING_DETAILS] ([ID], [ROUTING], [SEQUENCE], [OPERATION], [CONDITION], [NEXT_OPERATION], [NC_OPERATION], [SEMANTIC_ANNOTATION]) VALUES (63, N'c53aebb8-ea62-4d96-bf71-e766d3a64496', 15, N'1b7f6789-e9b1-4045-ab68-40c4f939e753', NULL, N'8391a7a0-8c11-44a7-9b6d-d0c4e4a08b25', N'1b7f6789-e9b1-4045-ab68-40c4f939e753', 25)
INSERT [dbo].[ME_ROUTING_DETAILS] ([ID], [ROUTING], [SEQUENCE], [OPERATION], [CONDITION], [NEXT_OPERATION], [NC_OPERATION], [SEMANTIC_ANNOTATION]) VALUES (64, N'c53aebb8-ea62-4d96-bf71-e766d3a64496', 9, N'd3832e61-75b9-4685-80f0-a441bca6fdc9', NULL, N'7fe880f9-d2fa-4574-a461-60ad068edbd9', N'1b7f6789-e9b1-4045-ab68-40c4f939e753', 39)
INSERT [dbo].[ME_ROUTING_DETAILS] ([ID], [ROUTING], [SEQUENCE], [OPERATION], [CONDITION], [NEXT_OPERATION], [NC_OPERATION], [SEMANTIC_ANNOTATION]) VALUES (65, N'c53aebb8-ea62-4d96-bf71-e766d3a64496', 16, N'd3832e61-75b9-4685-80f0-a441bca6fdc9', NULL, N'7fe880f9-d2fa-4574-a461-60ad068edbd9', N'1b7f6789-e9b1-4045-ab68-40c4f939e753', 25)
INSERT [dbo].[ME_ROUTING_DETAILS] ([ID], [ROUTING], [SEQUENCE], [OPERATION], [CONDITION], [NEXT_OPERATION], [NC_OPERATION], [SEMANTIC_ANNOTATION]) VALUES (66, N'c53aebb8-ea62-4d96-bf71-e766d3a64496', 17, N'aad188e8-bebe-4dae-8db5-87042d4400de', N' PRE_COND:rescom_resources_AutonomousTransporter(?at) ^ currentPositionX(?at, ?at_XCoOrd) ^ currentPositionY(?at, ?at_YCoOrd) ^ currentPositionZ(?at, ?at_ZCoOrd) ^ Value(?at_XCoOrd, ?at_xValue) ^ Value(?at_YCoOrd, ?at_yValue) ^ Value(?at_ZCoOrd, ?at_zValue) ^  rescom_resources_CircuitBoardProvider(?cbProvider) ^ hasPositionX(?cbProvider, ?cbProvider_XCoOrd) ^ hasPositionY(?cbProvider, ?cbProvider_YCoOrd) ^ hasPositionZ(?cbProvider, ?cbProvider_ZCoOrd) ^ Value(?cbProvider_XCoOrd, ?cb_xValue) ^ Value(?cbProvider_YCoOrd, ?cb_yValue) ^ Value(?cbProvider_ZCoOrd, ?cb_zValue) ^  swrlb:equal(?at_xValue, ?cb_xValue) ^ swrlb:equal(?at_yValue, ?cb_yValue) ^ swrlb:equal(?at_zValue, ?cb_zValue) -> sqwrl:select(true)', N'67a79715-459e-45fb-a763-08ab043eaa74', N'1b7f6789-e9b1-4045-ab68-40c4f939e753', 25)
INSERT [dbo].[ME_ROUTING_DETAILS] ([ID], [ROUTING], [SEQUENCE], [OPERATION], [CONDITION], [NEXT_OPERATION], [NC_OPERATION], [SEMANTIC_ANNOTATION]) VALUES (67, N'c53aebb8-ea62-4d96-bf71-e766d3a64496', 18, N'1b7f6789-e9b1-4045-ab68-40c4f939e753', NULL, N'8391a7a0-8c11-44a7-9b6d-d0c4e4a08b25', N'1b7f6789-e9b1-4045-ab68-40c4f939e753', 36)
INSERT [dbo].[ME_ROUTING_DETAILS] ([ID], [ROUTING], [SEQUENCE], [OPERATION], [CONDITION], [NEXT_OPERATION], [NC_OPERATION], [SEMANTIC_ANNOTATION]) VALUES (68, N'c53aebb8-ea62-4d96-bf71-e766d3a64496', 19, N'df8d40a6-6e2c-4fee-a570-e3d6ed412b6a', N' POST_COND:rescom_product_CircuitBoard(?circuitBoard) ^ isIdentified(?circuitBoard, true) -> sqwrl:select(true) ', N'9d49d0f0-0024-4144-be33-17dc6690934e', N'1b7f6789-e9b1-4045-ab68-40c4f939e753', 36)
INSERT [dbo].[ME_ROUTING_DETAILS] ([ID], [ROUTING], [SEQUENCE], [OPERATION], [CONDITION], [NEXT_OPERATION], [NC_OPERATION], [SEMANTIC_ANNOTATION]) VALUES (69, N'c53aebb8-ea62-4d96-bf71-e766d3a64496', 20, N'1b7f6789-e9b1-4045-ab68-40c4f939e753', NULL, N'8391a7a0-8c11-44a7-9b6d-d0c4e4a08b25', N'1b7f6789-e9b1-4045-ab68-40c4f939e753', 27)
INSERT [dbo].[ME_ROUTING_DETAILS] ([ID], [ROUTING], [SEQUENCE], [OPERATION], [CONDITION], [NEXT_OPERATION], [NC_OPERATION], [SEMANTIC_ANNOTATION]) VALUES (70, N'c53aebb8-ea62-4d96-bf71-e766d3a64496', 21, N'a0006829-aaa3-4cf9-97ab-a547abc99d4d', N' PRE_COND:rescom_resources_AutonomousTransporter(?at) ^ currentPositionX(?at, ?at_XCoOrd) ^ currentPositionY(?at, ?at_YCoOrd) ^ currentPositionZ(?at, ?at_ZCoOrd) ^ Value(?at_XCoOrd, ?at_xValue) ^ Value(?at_YCoOrd, ?at_yValue) ^ Value(?at_ZCoOrd, ?at_zValue) ^  rescom_capability_abstracts_Assembler(?assmblr) ^ hasPositionX(?assmblr, ?assmblr_XCoOrd) ^ hasPositionY(?assmblr, ?assmblr_YCoOrd) ^ hasPositionZ(?assmblr, ?assmblr_ZCoOrd) ^ Value(?assmblr_XCoOrd, ?as_xValue) ^ Value(?assmblr_YCoOrd, ?as_yValue) ^ Value(?assmblr_ZCoOrd, ?as_zValue) ^ swrlb:equal(?at_xValue, ?as_xValue) ^ swrlb:equal(?at_yValue, ?as_yValue) ^ swrlb:equal(?at_zValue, ?as_zValue) ^rescom_product_LowerShell(?ls) ^ isPlacedInAssembly(?ls,true) -> sqwrl:select(true); POST_COND:rescom_product_CircuitBoard(?cb) -> isPlacedInAssembly(?cb, true) ; ', N'0ed860a9-fbc0-4ca2-a9e6-caef17c9d651', N'1b7f6789-e9b1-4045-ab68-40c4f939e753', 38)
INSERT [dbo].[ME_ROUTING_DETAILS] ([ID], [ROUTING], [SEQUENCE], [OPERATION], [CONDITION], [NEXT_OPERATION], [NC_OPERATION], [SEMANTIC_ANNOTATION]) VALUES (71, N'c53aebb8-ea62-4d96-bf71-e766d3a64496', 22, N'1b7f6789-e9b1-4045-ab68-40c4f939e753', NULL, N'8391a7a0-8c11-44a7-9b6d-d0c4e4a08b25', N'1b7f6789-e9b1-4045-ab68-40c4f939e753', 28)
INSERT [dbo].[ME_ROUTING_DETAILS] ([ID], [ROUTING], [SEQUENCE], [OPERATION], [CONDITION], [NEXT_OPERATION], [NC_OPERATION], [SEMANTIC_ANNOTATION]) VALUES (72, N'c53aebb8-ea62-4d96-bf71-e766d3a64496', 23, N'aad188e8-bebe-4dae-8db5-87042d4400de', N' PRE_COND:rescom_resources_AutonomousTransporter(?at) ^ currentPositionX(?at, ?at_XCoOrd) ^ currentPositionY(?at, ?at_YCoOrd) ^ currentPositionZ(?at, ?at_ZCoOrd)  ^ Value(?at_XCoOrd, ?at_xValue) ^ Value(?at_YCoOrd, ?at_yValue) ^ Value(?at_ZCoOrd, ?at_zValue) ^  rescom_resources_UpperShellProvider(?usProvider) ^ hasPositionX(?usProvider, ?usProvider_XCoOrd) ^ hasPositionY(?usProvider, ?usProvider_YCoOrd) ^ hasPositionZ(?usProvider, ?usProvider_ZCoOrd) ^  Value(?usProvider_XCoOrd, ?us_xValue) ^ Value(?usProvider_YCoOrd, ?us_yValue) ^ Value(?usProvider_ZCoOrd, ?us_zValue)  ^ swrlb:equal(?at_xValue, ?us_xValue) ^ swrlb:equal(?at_yValue, ?us_yValue) ^ swrlb:equal(?at_zValue, ?us_zValue)  -> sqwrl:select(true) ', N'67a79715-459e-45fb-a763-08ab043eaa74', N'1b7f6789-e9b1-4045-ab68-40c4f939e753', 28)
INSERT [dbo].[ME_ROUTING_DETAILS] ([ID], [ROUTING], [SEQUENCE], [OPERATION], [CONDITION], [NEXT_OPERATION], [NC_OPERATION], [SEMANTIC_ANNOTATION]) VALUES (73, N'c53aebb8-ea62-4d96-bf71-e766d3a64496', 24, N'1b7f6789-e9b1-4045-ab68-40c4f939e753', NULL, N'8391a7a0-8c11-44a7-9b6d-d0c4e4a08b25', N'1b7f6789-e9b1-4045-ab68-40c4f939e753', 36)
INSERT [dbo].[ME_ROUTING_DETAILS] ([ID], [ROUTING], [SEQUENCE], [OPERATION], [CONDITION], [NEXT_OPERATION], [NC_OPERATION], [SEMANTIC_ANNOTATION]) VALUES (74, N'c53aebb8-ea62-4d96-bf71-e766d3a64496', 25, N'df8d40a6-6e2c-4fee-a570-e3d6ed412b6a', N'POST_COND:rescom_product_UpperShell(?us) ^ hasColor(?us, White) -> isIdentified(?us, true) ', N'9d49d0f0-0024-4144-be33-17dc6690934e', N'1b7f6789-e9b1-4045-ab68-40c4f939e753', 36)
INSERT [dbo].[ME_ROUTING_DETAILS] ([ID], [ROUTING], [SEQUENCE], [OPERATION], [CONDITION], [NEXT_OPERATION], [NC_OPERATION], [SEMANTIC_ANNOTATION]) VALUES (75, N'c53aebb8-ea62-4d96-bf71-e766d3a64496', 26, N'1b7f6789-e9b1-4045-ab68-40c4f939e753', NULL, N'8391a7a0-8c11-44a7-9b6d-d0c4e4a08b25', N'1b7f6789-e9b1-4045-ab68-40c4f939e753', 14)
INSERT [dbo].[ME_ROUTING_DETAILS] ([ID], [ROUTING], [SEQUENCE], [OPERATION], [CONDITION], [NEXT_OPERATION], [NC_OPERATION], [SEMANTIC_ANNOTATION]) VALUES (76, N'c53aebb8-ea62-4d96-bf71-e766d3a64496', 27, N'5c89c472-4d16-49d3-b39d-b7ea922aa203', N'POST_COND:rescom_product_UpperShell(?ls) ^ state(?us, true) -> isIdentified(?us,true)', N'f5b26750-ee35-4a35-baad-8ddba607e2dc', N'1b7f6789-e9b1-4045-ab68-40c4f939e753', 14)
INSERT [dbo].[ME_ROUTING_DETAILS] ([ID], [ROUTING], [SEQUENCE], [OPERATION], [CONDITION], [NEXT_OPERATION], [NC_OPERATION], [SEMANTIC_ANNOTATION]) VALUES (77, N'c53aebb8-ea62-4d96-bf71-e766d3a64496', 28, N'1b7f6789-e9b1-4045-ab68-40c4f939e753', NULL, N'8391a7a0-8c11-44a7-9b6d-d0c4e4a08b25', N'1b7f6789-e9b1-4045-ab68-40c4f939e753', 38)
INSERT [dbo].[ME_ROUTING_DETAILS] ([ID], [ROUTING], [SEQUENCE], [OPERATION], [CONDITION], [NEXT_OPERATION], [NC_OPERATION], [SEMANTIC_ANNOTATION]) VALUES (78, N'c53aebb8-ea62-4d96-bf71-e766d3a64496', 29, N'a0006829-aaa3-4cf9-97ab-a547abc99d4d', N' PRE_COND:rescom_resources_AutonomousTransporter(?at) ^ currentPositionX(?at, ?at_XCoOrd) ^ currentPositionY(?at, ?at_YCoOrd) ^ currentPositionZ(?at, ?at_ZCoOrd) ^ Value(?at_XCoOrd, ?at_xValue) ^ Value(?at_YCoOrd, ?at_yValue) ^ Value(?at_ZCoOrd, ?at_zValue) ^  rescom_capability_abstracts_Assembler(?assmblr) ^ hasPositionX(?assmblr, ?assmblr_XCoOrd) ^ hasPositionY(?assmblr, ?assmblr_YCoOrd) ^ hasPositionZ(?assmblr, ?assmblr_ZCoOrd) ^ Value(?assmblr_XCoOrd, ?as_xValue) ^ Value(?assmblr_YCoOrd, ?as_yValue) ^ Value(?assmblr_ZCoOrd, ?as_zValue) ^  swrlb:equal(?at_xValue, ?as_xValue) ^ swrlb:equal(?at_yValue, ?as_yValue) ^ swrlb:equal(?at_zValue, ?as_zValue) ^ rescom_product_CircuitBoard(?cb) ^ isPlacedInAssembly(?cb,true)  -> sqwrl:select(true);POST_COND:rescom_product_UpperShell(?us) -> isPlacedInAssembly(?us, true); ', N'0ed860a9-fbc0-4ca2-a9e6-caef17c9d651', N'1b7f6789-e9b1-4045-ab68-40c4f939e753', 38)
INSERT [dbo].[ME_ROUTING_DETAILS] ([ID], [ROUTING], [SEQUENCE], [OPERATION], [CONDITION], [NEXT_OPERATION], [NC_OPERATION], [SEMANTIC_ANNOTATION]) VALUES (79, N'c53aebb8-ea62-4d96-bf71-e766d3a64496', 30, N'4816c2f9-e33e-4221-952d-8daf8b2d97b1', N' PRE_COND:rescom_product_LowerShell(?lowerShell) ^ isPlacedInAssembly(?lowerShell, true)^ rescom_product_CircuitBoard(?circuitBoard) ^ isPlacedInAssembly(?circuitBoard, true)^ rescom_product_UpperShell(?upperShell) ^ isPlacedInAssembly(?upperShell, true) -> sqwrl:select(true)', N'2ebf5c7a-4551-49e4-a1d6-03033d16e1eb', N'1b7f6789-e9b1-4045-ab68-40c4f939e753', 38)
INSERT [dbo].[ME_ROUTING_DETAILS] ([ID], [ROUTING], [SEQUENCE], [OPERATION], [CONDITION], [NEXT_OPERATION], [NC_OPERATION], [SEMANTIC_ANNOTATION]) VALUES (80, N'c53aebb8-ea62-4d96-bf71-e766d3a64496', 33, N'a0006829-aaa3-4cf9-97ab-a547abc99d4d', NULL, N'0ed860a9-fbc0-4ca2-a9e6-caef17c9d651', N'1b7f6789-e9b1-4045-ab68-40c4f939e753', 44)
INSERT [dbo].[ME_ROUTING_DETAILS] ([ID], [ROUTING], [SEQUENCE], [OPERATION], [CONDITION], [NEXT_OPERATION], [NC_OPERATION], [SEMANTIC_ANNOTATION]) VALUES (81, N'c53aebb8-ea62-4d96-bf71-e766d3a64496', 31, N'aad188e8-bebe-4dae-8db5-87042d4400de', NULL, N'67a79715-459e-45fb-a763-08ab043eaa74', N'1b7f6789-e9b1-4045-ab68-40c4f939e753', 38)
INSERT [dbo].[ME_ROUTING_DETAILS] ([ID], [ROUTING], [SEQUENCE], [OPERATION], [CONDITION], [NEXT_OPERATION], [NC_OPERATION], [SEMANTIC_ANNOTATION]) VALUES (82, N'c53aebb8-ea62-4d96-bf71-e766d3a64496', 32, N'1b7f6789-e9b1-4045-ab68-40c4f939e753', NULL, N'8391a7a0-8c11-44a7-9b6d-d0c4e4a08b25', N'1b7f6789-e9b1-4045-ab68-40c4f939e753', 44)
SET IDENTITY_INSERT [dbo].[ME_ROUTING_DETAILS] OFF
INSERT [dbo].[ME_ROUTING_STATUS] ([ID], [NAME]) VALUES (1, N'New')
INSERT [dbo].[ME_ROUTING_STATUS] ([ID], [NAME]) VALUES (2, N'Obsolete')
INSERT [dbo].[ME_ROUTING_STATUS] ([ID], [NAME]) VALUES (3, N'Hold')
INSERT [dbo].[ME_ROUTING_STATUS] ([ID], [NAME]) VALUES (4, N'Frozen')
INSERT [dbo].[ME_ROUTING_STATUS] ([ID], [NAME]) VALUES (5, N'Releasable')
INSERT [dbo].[ME_ROUTING_TYPE] ([ID], [NAME]) VALUES (1, N'Production routing')
INSERT [dbo].[ME_ROUTING_TYPE] ([ID], [NAME]) VALUES (2, N'Configurable')
INSERT [dbo].[ME_ROUTING_TYPE] ([ID], [NAME]) VALUES (3, N'NC Routing')
INSERT [dbo].[ME_ROUTING_TYPE] ([ID], [NAME]) VALUES (4, N'Special routing')
INSERT [dbo].[ME_ROUTING_TYPE] ([ID], [NAME]) VALUES (5, N'SFC Specific routing')
SET IDENTITY_INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ON 

INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (1, N'http://emea.global.corp.sap/rescom#rescom_capability_abstracts_Mover', N'rescom_capability_abstracts_Mover')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (2, N'http://emea.global.corp.sap/rescom#rescom_capability_abstracts_Picker', N'rescom_capability_abstracts_Picker')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (3, N'http://emea.global.corp.sap/rescom#rescom_capability_abstracts_PickerAndPlacer', N'rescom_capability_abstracts_PickerAndPlacer')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (4, N'http://emea.global.corp.sap/rescom#rescom_capability_abstracts_PickerPlacerMover', N'rescom_capability_abstracts_PickerPlacerMover')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (5, N'http://emea.global.corp.sap/rescom#rescom_capability_abstracts_Placer', N'rescom_capability_abstracts_Placer')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (6, N'http://emea.global.corp.sap/rescom#rescom_capability_abstracts_Provider', N'rescom_capability_abstracts_Provider')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (7, N'http://emea.global.corp.sap/rescom#rescom_capability_abstracts_QualityAssurance', N'rescom_capability_abstracts_QualityAssurance')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (8, N'http://emea.global.corp.sap/rescom#rescom_capability_abstracts_Slider', N'rescom_capability_abstracts_Slider')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (9, N'http://emea.global.corp.sap/rescom#rescom_capability_abstracts_Status', N'rescom_capability_abstracts_Status')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (10, N'http://emea.global.corp.sap/rescom#rescom_capability_concrete_DataMatrixDetector', N'rescom_capability_concrete_DataMatrixDetector')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (11, N'http://emea.global.corp.sap/rescom#rescom_capability_concrete_IsAssemblyInUse', N'rescom_capability_concrete_IsAssemblyInUse')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (12, N'http://emea.global.corp.sap/rescom#rescom_capability_concrete_MaterialSupplier', N'rescom_capability_concrete_MaterialSupplier')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (13, N'http://emea.global.corp.sap/rescom#rescom_capability_concrete_Presser', N'rescom_capability_concrete_Presser')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (14, N'http://emea.global.corp.sap/rescom#rescom_capability_concrete_RFIDRead', N'rescom_capability_concrete_RFIDRead')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (15, N'http://emea.global.corp.sap/rescom#rescom_capability_concrete_Transporter', N'rescom_capability_concrete_Transporter')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (16, N'http://emea.global.corp.sap/rescom#rescom_concepts_Color', N'rescom_concepts_Color')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (17, N'http://emea.global.corp.sap/rescom#rescom_concepts_CoOrdinate', N'rescom_concepts_CoOrdinate')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (18, N'http://emea.global.corp.sap/rescom#rescom_concepts_Displacer', N'rescom_concepts_Displacer')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (19, N'http://emea.global.corp.sap/rescom#rescom_concepts_MethodType', N'rescom_concepts_MethodType')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (20, N'http://emea.global.corp.sap/rescom#rescom_concepts_ThreeDimensionalMover', N'rescom_concepts_ThreeDimensionalMover')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (21, N'http://emea.global.corp.sap/rescom#rescom_concepts_XCoOrdinate', N'rescom_concepts_XCoOrdinate')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (22, N'http://emea.global.corp.sap/rescom#rescom_concepts_YCoOrdinate', N'rescom_concepts_YCoOrdinate')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (23, N'http://emea.global.corp.sap/rescom#rescom_product_LowerShell', N'rescom_product_LowerShell')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (24, N'http://emea.global.corp.sap/rescom#rescom_product_UpperShell', N'rescom_product_UpperShell')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (25, N'http://emea.global.corp.sap/rescom#rescom_resources_CircuitBoardProvider', N'rescom_resources_CircuitBoardProvider')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (26, N'http://emea.global.corp.sap/rescom#rescom_resources_ConveyorBelt', N'rescom_resources_ConveyorBelt')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (27, N'http://emea.global.corp.sap/rescom#rescom_resources_PressResource', N'rescom_resources_PressResource')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (28, N'http://emea.global.corp.sap/rescom#rescom_resources_UpperShellProvider', N'rescom_resources_UpperShellProvider')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (29, N'http://emea.global.corp.sap/rescom#rescom_capability_concrete_PrepareAssembly', N'rescom_capability_concrete_PrepareAssembly')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (30, N'http://emea.global.corp.sap/rescom#rescom_concepts_ZCoOrdinate', N'rescom_concepts_ZCoOrdinate')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (31, N'http://emea.global.corp.sap/rescom#rescom_product_CircuitBoard', N'rescom_product_CircuitBoard')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (32, N'http://emea.global.corp.sap/rescom#rescom_product_KeyFinder', N'rescom_product_KeyFinder')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (33, N'http://emea.global.corp.sap/rescom#rescom_product_Material', N'rescom_product_Material')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (34, N'http://emea.global.corp.sap/rescom#rescom_resources_AutonomousTransporter', N'rescom_resources_AutonomousTransporter')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (35, N'http://emea.global.corp.sap/rescom#rescom_resources_CircuitBoardWarehouse', N'rescom_resources_CircuitBoardWarehouse')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (36, N'http://emea.global.corp.sap/rescom#rescom_resources_IndustrialCamera', N'rescom_resources_IndustrialCamera')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (37, N'http://emea.global.corp.sap/rescom#rescom_resources_UpperShellWarehouse', N'rescom_resources_UpperShellWarehouse')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (38, N'http://emea.global.corp.sap/rescom#rescom_capability_abstracts_Assembler', N'rescom_capability_abstracts_Assembler')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (39, N'http://emea.global.corp.sap/rescom#rescom_resources_LowerShellProvider', N'rescom_resources_LowerShellProvider')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (40, N'http://emea.global.corp.sap/rescom#rescom_resources_RFIDReader', N'rescom_resources_RFIDReader')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (41, N'http://emea.global.corp.sap/rescom#rescom_resources_Warehouse', N'rescom_resources_Warehouse')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (43, N'http://emea.global.corp.sap/rescom#rescom_resources_LowerShellWarehouse', N'rescom_resources_LowerShellWarehouse')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (44, N'http://emea.global.corp.sap/rescom#rescom_resources_StoreKeyFinderUnit', N'rescom_resources_StoreKeyFinderUnit')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (45, N'http://emea.global.corp.sap/rescom#supplies', N'supplies')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (46, N'http://emea.global.corp.sap/rescom#Value', N'Value')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (47, N'http://emea.global.corp.sap/rescom#isIdentified', N'isIdentified')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (48, N'http://emea.global.corp.sap/rescom#rFIDDataReadPosition', N'rFIDDataReadPosition')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (49, N'http://emea.global.corp.sap/rescom#rFIDDataReadLength', N'rFIDDataReadLength')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (50, N'http://emea.global.corp.sap/rescom#hasColor', N'hasColor')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (51, N'http://emea.global.corp.sap/rescom#isPlacedInAssembly', N'isPlacedInAssembly')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (52, N'http://emea.global.corp.sap/rescom#provides', N'provides')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (53, N'http://emea.global.corp.sap/rescom#hasPositionX', N'hasPositionX')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (54, N'http://emea.global.corp.sap/rescom#hasPositionY', N'hasPositionY')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (55, N'http://emea.global.corp.sap/rescom#hasPositionZ', N'hasPositionZ')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (56, N'http://emea.global.corp.sap/rescom#providesMaterial', N'providesMaterial')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (57, N'http://emea.global.corp.sap/rescom#motionVelocity', N'motionVelocity')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (58, N'http://emea.global.corp.sap/rescom#motionDirection', N'motionDirection')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (59, N'http://emea.global.corp.sap/rescom#isStopped', N'isStopped')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (60, N'http://emea.global.corp.sap/rescom#isInUse', N'isInUse')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (61, N'http://emea.global.corp.sap/rescom#canBePrepared', N'canBePrepared')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (62, N'http://emea.global.corp.sap/rescom#canBeAssembled', N'canBeAssembled')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (63, N'http://emea.global.corp.sap/rescom#codematrix', N'codematrix')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (64, N'http://emea.global.corp.sap/rescom#isPlacedInStorage', N'isPlacedInStorage')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (65, N'http://emea.global.corp.sap/rescom#hasPicker', N'hasPicker')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (66, N'http://emea.global.corp.sap/rescom#hasPlacer', N'hasPlacer')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (67, N'http://emea.global.corp.sap/rescom#hasMover', N'hasMover')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (68, N'http://emea.global.corp.sap/rescom#currentPositionX', N'currentPositionX')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (69, N'http://emea.global.corp.sap/rescom#currentPositionY', N'currentPositionY')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (70, N'http://emea.global.corp.sap/rescom#currentPositionZ', N'currentPositionZ')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (71, N'http://emea.global.corp.sap/rescom#x_Direction_Acceleration', N'x_Direction_Acceleration')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (72, N'http://emea.global.corp.sap/rescom#y_Direction_Acceleration', N'y_Direction_Acceleration')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (73, N'http://emea.global.corp.sap/rescom#z_Direction_Acceleration', N'z_Direction_Acceleration')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (74, N'http://emea.global.corp.sap/rescom#x_Direction_Decceleration', N'x_Direction_Decceleration')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (75, N'http://emea.global.corp.sap/rescom#y_Direction_Decceleration', N'y_Direction_Decceleration')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (76, N'http://emea.global.corp.sap/rescom#z_Direction_Decceleration', N'z_Direction_Decceleration')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (77, N'http://emea.global.corp.sap/rescom#x_Direction_Velocity', N'x_Direction_Velocity')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (78, N'http://emea.global.corp.sap/rescom#y_Direction_Velocity', N'y_Direction_Velocity')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (79, N'http://emea.global.corp.sap/rescom#z_Direction_Velocity', N'z_Direction_Velocity')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (80, N'http://emea.global.corp.sap/rescom#x_MotionDirection', N'x_MotionDirection')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (81, N'http://emea.global.corp.sap/rescom#y_MotionDirection', N'y_MotionDirection')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (82, N'http://emea.global.corp.sap/rescom#z_MotionDirection', N'z_MotionDirection')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (83, N'http://emea.global.corp.sap/rescom#isInMove', N'isInMove')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (84, N'http://emea.global.corp.sap/rescom#rFIDDataWritePosition', N'rFIDDataWritePosition')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (85, N'http://emea.global.corp.sap/rescom#rFIDWriteData', N'rFIDWriteData')
INSERT [dbo].[ME_SEMANTIC_ANNOTATION] ([ID], [IRI], [SHORT_NAME]) VALUES (86, N'http://emea.global.corp.sap/rescom#storesMaterial', N'storesMaterial')
SET IDENTITY_INSERT [dbo].[ME_SEMANTIC_ANNOTATION] OFF
INSERT [dbo].[ME_SETPOINT] ([ID], [NAME], [DESCRIPTION], [DATA_TYPE], [NUMERIC_VALUE], [STRING_VALUE], [STATUS], [START_DATE], [END_DATE]) VALUES (N'cb43db7c-f48b-4fa6-8359-222ef36cef28', N'Set point 1', N'Set point 1 description', 1, 45, NULL, 1, CAST(N'2017-04-24' AS Date), CAST(N'2017-04-26' AS Date))
INSERT [dbo].[ME_SETPOINT_STATUS] ([ID], [NAME]) VALUES (1, N'NEW')
INSERT [dbo].[ME_SETPOINT_STATUS] ([ID], [NAME]) VALUES (2, N'Frozen')
INSERT [dbo].[ME_SETPOINT_STATUS] ([ID], [NAME]) VALUES (3, N'Hold')
INSERT [dbo].[ME_SETPOINT_STATUS] ([ID], [NAME]) VALUES (4, N'Obsolete')
INSERT [dbo].[ME_SETPOINT_STATUS] ([ID], [NAME]) VALUES (5, N'Releasable')
INSERT [dbo].[ME_SETPOINT_TYPE] ([ID], [NAME]) VALUES (1, N'Numeric')
INSERT [dbo].[ME_SETPOINT_TYPE] ([ID], [NAME]) VALUES (2, N'String')
INSERT [dbo].[ME_SHOP_ORDER] ([ID], [NAME], [DESCRIPTION], [STATUS], [TYPE], [PLANNED_MATERIAL], [BOM], [ROUTING], [BUILD_QUANTITY], [CUSTOMER_ORDER], [PLANNED_START], [PLANNED_COMPLETION], [PRIORITY], [RELEASE_QUANTITY]) VALUES (N'357bb82a-9025-4719-afcb-fe9c1d6611e0', N'Key Finder Module Shop Order', N'This shop order produces the key finder module', 6, 1, N'5c749b0c-1e58-4606-bce9-8b644108c2ba', N'193dd336-55aa-4f0d-acd7-a874bdf8d154', N'c53aebb8-ea62-4d96-bf71-e766d3a64496', 1, N'ABCD', CAST(N'2018-11-08' AS Date), CAST(N'2018-11-10' AS Date), 1, 1)
INSERT [dbo].[ME_SHOP_ORDER_STATUS] ([ID], [NAME]) VALUES (1, N'Releasable')
INSERT [dbo].[ME_SHOP_ORDER_STATUS] ([ID], [NAME]) VALUES (2, N'Closed')
INSERT [dbo].[ME_SHOP_ORDER_STATUS] ([ID], [NAME]) VALUES (3, N'Done')
INSERT [dbo].[ME_SHOP_ORDER_STATUS] ([ID], [NAME]) VALUES (4, N'Hold')
INSERT [dbo].[ME_SHOP_ORDER_STATUS] ([ID], [NAME]) VALUES (5, N'Released')
INSERT [dbo].[ME_SHOP_ORDER_STATUS] ([ID], [NAME]) VALUES (6, N'Production')
INSERT [dbo].[ME_SHOP_ORDER_TYPE] ([ID], [NAME]) VALUES (1, N'Production')
INSERT [dbo].[ME_SHOP_ORDER_TYPE] ([ID], [NAME]) VALUES (2, N'Engineering')
INSERT [dbo].[ME_SHOP_ORDER_TYPE] ([ID], [NAME]) VALUES (3, N'Inspection')
INSERT [dbo].[ME_SHOP_ORDER_TYPE] ([ID], [NAME]) VALUES (4, N'Rework')
INSERT [dbo].[ME_SHOP_ORDER_TYPE] ([ID], [NAME]) VALUES (5, N'Repetitive')
INSERT [dbo].[ME_WORKCENTER] ([ID], [NAME], [DESCRIPTION], [STATUS], [CATEGORY], [TYPE]) VALUES (N'4128e430-1f2e-4f6a-9bcf-f5bbbe6e4ec2', N'Key Finder Assembly Work Station', N'RES-COM Project Assembly Work Station', 1, 1, 1)
INSERT [dbo].[ME_WORKCENTER_CATEGORY] ([ID], [NAME]) VALUES (1, N'None')
INSERT [dbo].[ME_WORKCENTER_CATEGORY] ([ID], [NAME]) VALUES (2, N'Cell')
INSERT [dbo].[ME_WORKCENTER_CATEGORY] ([ID], [NAME]) VALUES (3, N'Cell Group')
INSERT [dbo].[ME_WORKCENTER_CATEGORY] ([ID], [NAME]) VALUES (4, N'Line')
INSERT [dbo].[ME_WORKCENTER_CATEGORY] ([ID], [NAME]) VALUES (5, N'Line Group')
INSERT [dbo].[ME_WORKCENTER_CATEGORY] ([ID], [NAME]) VALUES (6, N'Building')
SET IDENTITY_INSERT [dbo].[ME_WORKCENTER_RESOURCE] ON 

INSERT [dbo].[ME_WORKCENTER_RESOURCE] ([ID], [WORKCENTER_ID], [RESOURCE_ID]) VALUES (36, N'4128e430-1f2e-4f6a-9bcf-f5bbbe6e4ec2', N'1f816f97-9b70-4cad-aa41-be56e79d8af9')
SET IDENTITY_INSERT [dbo].[ME_WORKCENTER_RESOURCE] OFF
INSERT [dbo].[ME_WORKCENTER_STATUS] ([ID], [NAME]) VALUES (1, N'Enabled')
INSERT [dbo].[ME_WORKCENTER_STATUS] ([ID], [NAME]) VALUES (2, N'Disabled')
INSERT [dbo].[ME_WORKCENTER_TYPE] ([ID], [NAME]) VALUES (1, N'Assembly')
INSERT [dbo].[ME_WORKCENTER_TYPE] ([ID], [NAME]) VALUES (2, N'Fabrication')
INSERT [dbo].[ME_WORKCENTER_TYPE] ([ID], [NAME]) VALUES (3, N'Dispatch')
INSERT [dbo].[ME_WORKCENTER_TYPE] ([ID], [NAME]) VALUES (4, N'Shipping')
INSERT [dbo].[ME_WORKCENTER_TYPE] ([ID], [NAME]) VALUES (5, N'Prod Control')
ALTER TABLE [dbo].[ME_BOM]  WITH CHECK ADD  CONSTRAINT [fk_bom_status] FOREIGN KEY([STATUS])
REFERENCES [dbo].[ME_BOM_STATUS] ([ID])
GO
ALTER TABLE [dbo].[ME_BOM] CHECK CONSTRAINT [fk_bom_status]
GO
ALTER TABLE [dbo].[ME_BOM_DETAILS]  WITH CHECK ADD  CONSTRAINT [fk_bom_id] FOREIGN KEY([BOM])
REFERENCES [dbo].[ME_BOM] ([ID])
GO
ALTER TABLE [dbo].[ME_BOM_DETAILS] CHECK CONSTRAINT [fk_bom_id]
GO
ALTER TABLE [dbo].[ME_BOM_DETAILS]  WITH CHECK ADD  CONSTRAINT [fk_bom_material] FOREIGN KEY([COMPONENT])
REFERENCES [dbo].[ME_MATERIAL] ([ID])
GO
ALTER TABLE [dbo].[ME_BOM_DETAILS] CHECK CONSTRAINT [fk_bom_material]
GO
ALTER TABLE [dbo].[ME_BOM_DETAILS]  WITH CHECK ADD  CONSTRAINT [fk_bom_operation] FOREIGN KEY([OPERATION])
REFERENCES [dbo].[ME_OPERATION] ([ID])
GO
ALTER TABLE [dbo].[ME_BOM_DETAILS] CHECK CONSTRAINT [fk_bom_operation]
GO
ALTER TABLE [dbo].[ME_MATERIAL]  WITH CHECK ADD  CONSTRAINT [fk_material_status] FOREIGN KEY([STATUS])
REFERENCES [dbo].[ME_MATERIAL_STATUS] ([ID])
GO
ALTER TABLE [dbo].[ME_MATERIAL] CHECK CONSTRAINT [fk_material_status]
GO
ALTER TABLE [dbo].[ME_MATERIAL]  WITH CHECK ADD  CONSTRAINT [fk_material_type] FOREIGN KEY([TYPE])
REFERENCES [dbo].[ME_MATERIAL_TYPE] ([ID])
GO
ALTER TABLE [dbo].[ME_MATERIAL] CHECK CONSTRAINT [fk_material_type]
GO
ALTER TABLE [dbo].[ME_OPERATION]  WITH CHECK ADD  CONSTRAINT [fk_operation_activity] FOREIGN KEY([ACTIVITY])
REFERENCES [dbo].[ME_ACTIVITY] ([ID])
GO
ALTER TABLE [dbo].[ME_OPERATION] CHECK CONSTRAINT [fk_operation_activity]
GO
ALTER TABLE [dbo].[ME_OPERATION]  WITH CHECK ADD  CONSTRAINT [fk_OPERATION_OPERATIONTYPE] FOREIGN KEY([OPERATIONTYPE])
REFERENCES [dbo].[ME_OPERATION_TYPE] ([ID])
GO
ALTER TABLE [dbo].[ME_OPERATION] CHECK CONSTRAINT [fk_OPERATION_OPERATIONTYPE]
GO
ALTER TABLE [dbo].[ME_OPERATION]  WITH CHECK ADD  CONSTRAINT [fk_operation_resource] FOREIGN KEY([RESOURCE])
REFERENCES [dbo].[ME_RESOURCE] ([ID])
GO
ALTER TABLE [dbo].[ME_OPERATION] CHECK CONSTRAINT [fk_operation_resource]
GO
ALTER TABLE [dbo].[ME_OPERATION]  WITH CHECK ADD  CONSTRAINT [fk_operation_resourcetype] FOREIGN KEY([RESOURCETYPE])
REFERENCES [dbo].[ME_RESOURCE_TYPE] ([ID])
GO
ALTER TABLE [dbo].[ME_OPERATION] CHECK CONSTRAINT [fk_operation_resourcetype]
GO
ALTER TABLE [dbo].[ME_OPERATION]  WITH CHECK ADD  CONSTRAINT [fk_operation_status] FOREIGN KEY([STATUS])
REFERENCES [dbo].[ME_OPERATION_STATUS] ([ID])
GO
ALTER TABLE [dbo].[ME_OPERATION] CHECK CONSTRAINT [fk_operation_status]
GO
ALTER TABLE [dbo].[ME_OPERATION]  WITH CHECK ADD  CONSTRAINT [fk_operation_workcenter] FOREIGN KEY([WORKCENTER])
REFERENCES [dbo].[ME_WORKCENTER] ([ID])
GO
ALTER TABLE [dbo].[ME_OPERATION] CHECK CONSTRAINT [fk_operation_workcenter]
GO
ALTER TABLE [dbo].[ME_RESOURCE]  WITH CHECK ADD  CONSTRAINT [fk_RESOURCE_Status] FOREIGN KEY([STATUS])
REFERENCES [dbo].[ME_RESOURCE_STATUS] ([ID])
GO
ALTER TABLE [dbo].[ME_RESOURCE] CHECK CONSTRAINT [fk_RESOURCE_Status]
GO
ALTER TABLE [dbo].[ME_RESOURCE]  WITH CHECK ADD  CONSTRAINT [fk_RESOURCE_type] FOREIGN KEY([TYPE])
REFERENCES [dbo].[ME_RESOURCE_TYPE] ([ID])
GO
ALTER TABLE [dbo].[ME_RESOURCE] CHECK CONSTRAINT [fk_RESOURCE_type]
GO
ALTER TABLE [dbo].[ME_RESOURCE_MATERIAL]  WITH CHECK ADD  CONSTRAINT [fk_RESOURCE_ID] FOREIGN KEY([RESOURCE])
REFERENCES [dbo].[ME_RESOURCE] ([ID])
GO
ALTER TABLE [dbo].[ME_RESOURCE_MATERIAL] CHECK CONSTRAINT [fk_RESOURCE_ID]
GO
ALTER TABLE [dbo].[ME_RESOURCE_MATERIAL]  WITH CHECK ADD  CONSTRAINT [fk_RESOURCE_MATERIAL] FOREIGN KEY([MATERIAL])
REFERENCES [dbo].[ME_MATERIAL] ([ID])
GO
ALTER TABLE [dbo].[ME_RESOURCE_MATERIAL] CHECK CONSTRAINT [fk_RESOURCE_MATERIAL]
GO
ALTER TABLE [dbo].[ME_ROUTING]  WITH CHECK ADD  CONSTRAINT [fk_ROUTING_status] FOREIGN KEY([STATUS])
REFERENCES [dbo].[ME_ROUTING_STATUS] ([ID])
GO
ALTER TABLE [dbo].[ME_ROUTING] CHECK CONSTRAINT [fk_ROUTING_status]
GO
ALTER TABLE [dbo].[ME_ROUTING]  WITH CHECK ADD  CONSTRAINT [fk_ROUTING_type] FOREIGN KEY([TYPE])
REFERENCES [dbo].[ME_ROUTING_TYPE] ([ID])
GO
ALTER TABLE [dbo].[ME_ROUTING] CHECK CONSTRAINT [fk_ROUTING_type]
GO
ALTER TABLE [dbo].[ME_ROUTING_DETAILS]  WITH CHECK ADD  CONSTRAINT [fk_ROUTING_failoperation] FOREIGN KEY([NC_OPERATION])
REFERENCES [dbo].[ME_OPERATION] ([ID])
GO
ALTER TABLE [dbo].[ME_ROUTING_DETAILS] CHECK CONSTRAINT [fk_ROUTING_failoperation]
GO
ALTER TABLE [dbo].[ME_ROUTING_DETAILS]  WITH CHECK ADD  CONSTRAINT [fk_ROUTING_ID] FOREIGN KEY([ROUTING])
REFERENCES [dbo].[ME_ROUTING] ([ID])
GO
ALTER TABLE [dbo].[ME_ROUTING_DETAILS] CHECK CONSTRAINT [fk_ROUTING_ID]
GO
ALTER TABLE [dbo].[ME_ROUTING_DETAILS]  WITH CHECK ADD  CONSTRAINT [fk_ROUTING_operation] FOREIGN KEY([OPERATION])
REFERENCES [dbo].[ME_OPERATION] ([ID])
GO
ALTER TABLE [dbo].[ME_ROUTING_DETAILS] CHECK CONSTRAINT [fk_ROUTING_operation]
GO
ALTER TABLE [dbo].[ME_ROUTING_DETAILS]  WITH CHECK ADD  CONSTRAINT [fk_ROUTING_operationType] FOREIGN KEY([NEXT_OPERATION])
REFERENCES [dbo].[ME_OPERATION_TYPE] ([ID])
GO
ALTER TABLE [dbo].[ME_ROUTING_DETAILS] CHECK CONSTRAINT [fk_ROUTING_operationType]
GO
ALTER TABLE [dbo].[ME_ROUTING_DETAILS]  WITH CHECK ADD  CONSTRAINT [fk_ROUTING_SEMANTIC_ANNOTATION] FOREIGN KEY([SEMANTIC_ANNOTATION])
REFERENCES [dbo].[ME_SEMANTIC_ANNOTATION] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ME_ROUTING_DETAILS] CHECK CONSTRAINT [fk_ROUTING_SEMANTIC_ANNOTATION]
GO
ALTER TABLE [dbo].[ME_SETPOINT]  WITH CHECK ADD  CONSTRAINT [fk_SETPOINT_STATUS] FOREIGN KEY([STATUS])
REFERENCES [dbo].[ME_SETPOINT_STATUS] ([ID])
GO
ALTER TABLE [dbo].[ME_SETPOINT] CHECK CONSTRAINT [fk_SETPOINT_STATUS]
GO
ALTER TABLE [dbo].[ME_SETPOINT]  WITH CHECK ADD  CONSTRAINT [fk_SETPOINT_TYPE] FOREIGN KEY([DATA_TYPE])
REFERENCES [dbo].[ME_SETPOINT_TYPE] ([ID])
GO
ALTER TABLE [dbo].[ME_SETPOINT] CHECK CONSTRAINT [fk_SETPOINT_TYPE]
GO
ALTER TABLE [dbo].[ME_SETPOINT_DETAILS]  WITH CHECK ADD  CONSTRAINT [fk_SETPOINTDETAILS_MATERIAL] FOREIGN KEY([MATERIAL])
REFERENCES [dbo].[ME_MATERIAL] ([ID])
GO
ALTER TABLE [dbo].[ME_SETPOINT_DETAILS] CHECK CONSTRAINT [fk_SETPOINTDETAILS_MATERIAL]
GO
ALTER TABLE [dbo].[ME_SETPOINT_DETAILS]  WITH CHECK ADD  CONSTRAINT [fk_SETPOINTDETAILS_RESOURCE] FOREIGN KEY([RESOURCE])
REFERENCES [dbo].[ME_RESOURCE] ([ID])
GO
ALTER TABLE [dbo].[ME_SETPOINT_DETAILS] CHECK CONSTRAINT [fk_SETPOINTDETAILS_RESOURCE]
GO
ALTER TABLE [dbo].[ME_SETPOINT_DETAILS]  WITH CHECK ADD  CONSTRAINT [fk_SETPOINTDETAILS_SETPOINT] FOREIGN KEY([SETPOINT])
REFERENCES [dbo].[ME_SETPOINT] ([ID])
GO
ALTER TABLE [dbo].[ME_SETPOINT_DETAILS] CHECK CONSTRAINT [fk_SETPOINTDETAILS_SETPOINT]
GO
ALTER TABLE [dbo].[ME_SHOP_ORDER]  WITH CHECK ADD  CONSTRAINT [fk_SHOPORDER_BOM] FOREIGN KEY([BOM])
REFERENCES [dbo].[ME_BOM] ([ID])
GO
ALTER TABLE [dbo].[ME_SHOP_ORDER] CHECK CONSTRAINT [fk_SHOPORDER_BOM]
GO
ALTER TABLE [dbo].[ME_SHOP_ORDER]  WITH CHECK ADD  CONSTRAINT [fk_SHOPORDER_ORDER_TYPE] FOREIGN KEY([TYPE])
REFERENCES [dbo].[ME_SHOP_ORDER_TYPE] ([ID])
GO
ALTER TABLE [dbo].[ME_SHOP_ORDER] CHECK CONSTRAINT [fk_SHOPORDER_ORDER_TYPE]
GO
ALTER TABLE [dbo].[ME_SHOP_ORDER]  WITH CHECK ADD  CONSTRAINT [fk_SHOPORDER_PLANNED_MATERIAL] FOREIGN KEY([PLANNED_MATERIAL])
REFERENCES [dbo].[ME_MATERIAL] ([ID])
GO
ALTER TABLE [dbo].[ME_SHOP_ORDER] CHECK CONSTRAINT [fk_SHOPORDER_PLANNED_MATERIAL]
GO
ALTER TABLE [dbo].[ME_SHOP_ORDER]  WITH CHECK ADD  CONSTRAINT [fk_SHOPORDER_ROUTING] FOREIGN KEY([ROUTING])
REFERENCES [dbo].[ME_ROUTING] ([ID])
GO
ALTER TABLE [dbo].[ME_SHOP_ORDER] CHECK CONSTRAINT [fk_SHOPORDER_ROUTING]
GO
ALTER TABLE [dbo].[ME_SHOP_ORDER]  WITH CHECK ADD  CONSTRAINT [fk_SHOPORDER_STATUS] FOREIGN KEY([STATUS])
REFERENCES [dbo].[ME_SHOP_ORDER_STATUS] ([ID])
GO
ALTER TABLE [dbo].[ME_SHOP_ORDER] CHECK CONSTRAINT [fk_SHOPORDER_STATUS]
GO
ALTER TABLE [dbo].[ME_WORKCENTER]  WITH CHECK ADD  CONSTRAINT [fk_WORKCENTER_category] FOREIGN KEY([CATEGORY])
REFERENCES [dbo].[ME_WORKCENTER_CATEGORY] ([ID])
GO
ALTER TABLE [dbo].[ME_WORKCENTER] CHECK CONSTRAINT [fk_WORKCENTER_category]
GO
ALTER TABLE [dbo].[ME_WORKCENTER]  WITH CHECK ADD  CONSTRAINT [fk_WORKCENTER_status] FOREIGN KEY([STATUS])
REFERENCES [dbo].[ME_WORKCENTER_STATUS] ([ID])
GO
ALTER TABLE [dbo].[ME_WORKCENTER] CHECK CONSTRAINT [fk_WORKCENTER_status]
GO
ALTER TABLE [dbo].[ME_WORKCENTER]  WITH CHECK ADD  CONSTRAINT [fk_WORKCENTER_type] FOREIGN KEY([TYPE])
REFERENCES [dbo].[ME_WORKCENTER_TYPE] ([ID])
GO
ALTER TABLE [dbo].[ME_WORKCENTER] CHECK CONSTRAINT [fk_WORKCENTER_type]
GO
ALTER TABLE [dbo].[ME_WORKCENTER_RESOURCE]  WITH CHECK ADD  CONSTRAINT [fk_WORKCENTER_RESOURCE1] FOREIGN KEY([WORKCENTER_ID])
REFERENCES [dbo].[ME_WORKCENTER] ([ID])
GO
ALTER TABLE [dbo].[ME_WORKCENTER_RESOURCE] CHECK CONSTRAINT [fk_WORKCENTER_RESOURCE1]
GO
ALTER TABLE [dbo].[ME_WORKCENTER_RESOURCE]  WITH CHECK ADD  CONSTRAINT [fk_WORKCENTER_RESOURCE2] FOREIGN KEY([RESOURCE_ID])
REFERENCES [dbo].[ME_RESOURCE] ([ID])
GO
ALTER TABLE [dbo].[ME_WORKCENTER_RESOURCE] CHECK CONSTRAINT [fk_WORKCENTER_RESOURCE2]
GO
USE [master]
GO
ALTER DATABASE [ManufacturingExecution] SET  READ_WRITE 
GO
