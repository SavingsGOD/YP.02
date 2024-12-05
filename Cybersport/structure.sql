CREATE DATABASE  IF NOT EXISTS `cybersport` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `cybersport`;
-- MySQL dump 10.13  Distrib 8.0.38, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: cybersport
-- ------------------------------------------------------
-- Server version	8.0.30

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `GameGenres`
--

DROP TABLE IF EXISTS `GameGenres`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `GameGenres` (
  `GenreID` int NOT NULL AUTO_INCREMENT,
  `GenreName` varchar(50) NOT NULL,
  PRIMARY KEY (`GenreID`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `MatchResults`
--

DROP TABLE IF EXISTS `MatchResults`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `MatchResults` (
  `MatchID` int NOT NULL AUTO_INCREMENT,
  `TournamentID` int NOT NULL,
  `TeamAID` int NOT NULL,
  `TeamBID` int NOT NULL,
  `ScoreTeamA` int NOT NULL,
  `ScoreTeamB` int NOT NULL,
  `MatchDate` datetime NOT NULL,
  `Winner` varchar(45) NOT NULL,
  PRIMARY KEY (`MatchID`),
  KEY `matchresults_ibfk_1` (`TournamentID`),
  KEY `matchresults_ibfk_2` (`TeamAID`),
  KEY `matchresults_ibfk_3` (`TeamBID`),
  CONSTRAINT `matchresults_ibfk_1` FOREIGN KEY (`TournamentID`) REFERENCES `Tournaments` (`TournamentID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `matchresults_ibfk_2` FOREIGN KEY (`TeamAID`) REFERENCES `Teams` (`TeamID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `matchresults_ibfk_3` FOREIGN KEY (`TeamBID`) REFERENCES `Teams` (`TeamID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Participants`
--

DROP TABLE IF EXISTS `Participants`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Participants` (
  `ParticipantID` int NOT NULL AUTO_INCREMENT,
  `UserID` int NOT NULL,
  `TeamID` int NOT NULL,
  PRIMARY KEY (`ParticipantID`),
  KEY `participants_ibfk_1` (`UserID`),
  KEY `participants_ibfk_2` (`TeamID`),
  CONSTRAINT `participants_ibfk_1` FOREIGN KEY (`UserID`) REFERENCES `Users` (`UserID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `participants_ibfk_2` FOREIGN KEY (`TeamID`) REFERENCES `Teams` (`TeamID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=340 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Teams`
--

DROP TABLE IF EXISTS `Teams`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Teams` (
  `TeamID` int NOT NULL AUTO_INCREMENT,
  `TeamName` varchar(100) NOT NULL,
  `CaptainID` int NOT NULL,
  PRIMARY KEY (`TeamID`),
  KEY `teams_ibfk_1` (`CaptainID`),
  CONSTRAINT `teams_ibfk_1` FOREIGN KEY (`CaptainID`) REFERENCES `Users` (`UserID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=102 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `TournamentRegistrations`
--

DROP TABLE IF EXISTS `TournamentRegistrations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `TournamentRegistrations` (
  `RegistrationID` int NOT NULL AUTO_INCREMENT,
  `TournamentID` int NOT NULL,
  `TeamID` int NOT NULL,
  `CaptainID` int NOT NULL,
  PRIMARY KEY (`RegistrationID`),
  KEY `tournamentregistrations_ibfk_1` (`TournamentID`),
  KEY `tournamentregistrations_ibfk_2` (`TeamID`),
  KEY `tournamentregistrations_ibfk_3` (`CaptainID`),
  CONSTRAINT `tournamentregistrations_ibfk_1` FOREIGN KEY (`TournamentID`) REFERENCES `Tournaments` (`TournamentID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `tournamentregistrations_ibfk_2` FOREIGN KEY (`TeamID`) REFERENCES `Teams` (`TeamID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `tournamentregistrations_ibfk_3` FOREIGN KEY (`CaptainID`) REFERENCES `Users` (`UserID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=89 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Tournaments`
--

DROP TABLE IF EXISTS `Tournaments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Tournaments` (
  `TournamentID` int NOT NULL AUTO_INCREMENT,
  `TournamentName` varchar(100) NOT NULL,
  `StartDate` datetime NOT NULL,
  `EndDate` datetime NOT NULL,
  `GameType` varchar(50) NOT NULL,
  `Status` enum('Предстоящий','Текущий','Завершенный') NOT NULL,
  PRIMARY KEY (`TournamentID`)
) ENGINE=InnoDB AUTO_INCREMENT=33 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `TournamentsTeams`
--

DROP TABLE IF EXISTS `TournamentsTeams`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `TournamentsTeams` (
  `idTournaments` int NOT NULL,
  `idTeam` int NOT NULL,
  PRIMARY KEY (`idTournaments`,`idTeam`),
  KEY `Teams_idx` (`idTeam`),
  CONSTRAINT `Teams` FOREIGN KEY (`idTeam`) REFERENCES `Teams` (`TeamID`),
  CONSTRAINT `Tournament` FOREIGN KEY (`idTournaments`) REFERENCES `Tournaments` (`TournamentID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Users`
--

DROP TABLE IF EXISTS `Users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Users` (
  `UserID` int NOT NULL AUTO_INCREMENT,
  `Username` varchar(50) NOT NULL,
  `Password` varchar(100) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `FIO` varchar(100) NOT NULL,
  `PhoneNumber` varchar(20) NOT NULL,
  `Role` enum('Участник','Менеджер','Администратор') NOT NULL,
  PRIMARY KEY (`UserID`)
) ENGINE=InnoDB AUTO_INCREMENT=154 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-12-03 10:00:24
