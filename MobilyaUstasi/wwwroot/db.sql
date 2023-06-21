USE [master]
GO

/****** Object:  Database [webproje]    Script Date: 20.06.2023 15:51:01 ******/
CREATE DATABASE [webproje]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'webproje', FILENAME = N'C:\Users\User1\webproje.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'webproje_log', FILENAME = N'C:\Users\User1\webproje_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [webproje].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [webproje] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [webproje] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [webproje] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [webproje] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [webproje] SET ARITHABORT OFF 
GO

ALTER DATABASE [webproje] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [webproje] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [webproje] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [webproje] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [webproje] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [webproje] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [webproje] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [webproje] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [webproje] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [webproje] SET  DISABLE_BROKER 
GO

ALTER DATABASE [webproje] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [webproje] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [webproje] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [webproje] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [webproje] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [webproje] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [webproje] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [webproje] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [webproje] SET  MULTI_USER 
GO

ALTER DATABASE [webproje] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [webproje] SET DB_CHAINING OFF 
GO

ALTER DATABASE [webproje] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [webproje] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [webproje] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [webproje] SET QUERY_STORE = OFF
GO

USE [webproje]
GO

ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO

ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO

ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO

ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO

ALTER DATABASE [webproje] SET  READ_WRITE 
GO

