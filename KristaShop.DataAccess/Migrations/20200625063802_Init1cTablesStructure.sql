-- MySQL dump 10.13  Distrib 8.0.25, for Linux (x86_64)
--
-- Host: localhost    Database: krista_shop
-- ------------------------------------------------------
-- Server version	8.0.25

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
-- Table structure for table `1c_barcodes`
--

DROP TABLE IF EXISTS `1c_barcodes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `1c_barcodes` (
  `id` int NOT NULL AUTO_INCREMENT,
  `model` int DEFAULT NULL,
  `color` int DEFAULT NULL,
  `razmer` varchar(10) DEFAULT NULL,
  `nomenklatura` int DEFAULT NULL,
  `barcode` varchar(16) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7879 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `1c_catalog`
--

DROP TABLE IF EXISTS `1c_catalog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `1c_catalog` (
  `id` int NOT NULL AUTO_INCREMENT,
  `model` int DEFAULT NULL,
  `artikul` varchar(50) DEFAULT NULL,
  `color` int DEFAULT NULL,
  `razmer` varchar(10) DEFAULT NULL,
  `nomenklatura` int DEFAULT NULL,
  `kolichestvo` int DEFAULT NULL,
  `datav` date DEFAULT NULL,
  `razdel` int DEFAULT NULL,
  `price` float DEFAULT '0',
  `price_rub` float DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=10327 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `1c_city`
--

DROP TABLE IF EXISTS `1c_city`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `1c_city` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(255) DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1582 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `1c_cityclient`
--

DROP TABLE IF EXISTS `1c_cityclient`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `1c_cityclient` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(255) DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1009 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `1c_clients`
--

DROP TABLE IF EXISTS `1c_clients`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `1c_clients` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(255) DEFAULT '',
  `fullname` varchar(255) DEFAULT '',
  `date` varchar(255) DEFAULT '',
  `typprice` int DEFAULT NULL,
  `skidka` float DEFAULT NULL,
  `login` varchar(255) DEFAULT '',
  `password` varchar(255) DEFAULT '',
  `password_md5` varchar(255) DEFAULT '',
  `datepass` varchar(255) DEFAULT '',
  `phone` varchar(255) DEFAULT '',
  `address` varchar(255) DEFAULT '',
  `addresstc` varchar(255) DEFAULT '',
  `city` int DEFAULT NULL,
  `email` varchar(255) DEFAULT '',
  `file_1` varchar(255) DEFAULT '',
  `file_2` varchar(255) DEFAULT '',
  `file_3` varchar(255) DEFAULT '',
  `file_4` varchar(255) DEFAULT '',
  `file_5` varchar(255) DEFAULT '',
  `rating` int DEFAULT NULL,
  `avansdolg` float DEFAULT '0',
  `avansdolgrub` float DEFAULT '0',
  `limit` int DEFAULT NULL,
  `access1` tinyint(1) DEFAULT '0',
  `access2` tinyint(1) DEFAULT '0',
  `access3` tinyint(1) DEFAULT '0',
  `access4` tinyint(1) DEFAULT '0',
  `access5` tinyint(1) DEFAULT '0',
  `access6` tinyint(1) DEFAULT '0',
  `imanager` tinyint(1) DEFAULT '0',
  `manager` int DEFAULT NULL,
  `upd` tinyint(1) DEFAULT '0',
  `cdate` varchar(16) DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1634 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `1c_collection`
--

DROP TABLE IF EXISTS `1c_collection`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `1c_collection` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(255) DEFAULT '',
  `dates` date DEFAULT NULL,
  `datev` date DEFAULT NULL,
  `procent` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `1c_colors`
--

DROP TABLE IF EXISTS `1c_colors`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `1c_colors` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(255) DEFAULT '',
  `group` int DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1766 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `1c_colors_group`
--

DROP TABLE IF EXISTS `1c_colors_group`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `1c_colors_group` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(255) DEFAULT '',
  `rgb` varchar(7) DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=49 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `1c_doc_raschety_klientov`
--

DROP TABLE IF EXISTS `1c_doc_raschety_klientov`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `1c_doc_raschety_klientov` (
  `id` int NOT NULL AUTO_INCREMENT,
  `klient` int DEFAULT NULL,
  `nach_ost` float DEFAULT '0',
  `kon_ost` float DEFAULT '0',
  `dolg_plus` float DEFAULT '0',
  `dolg_minus` float DEFAULT '0',
  `data_doc` date DEFAULT NULL,
  `nomer_doc` varchar(8) DEFAULT NULL,
  `name_doc` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `client_id` (`klient`)
) ENGINE=InnoDB AUTO_INCREMENT=3918 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `1c_doc_raschety_klientov_sostav`
--

DROP TABLE IF EXISTS `1c_doc_raschety_klientov_sostav`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `1c_doc_raschety_klientov_sostav` (
  `id` int NOT NULL AUTO_INCREMENT,
  `klient` int DEFAULT NULL,
  `id_doc` int DEFAULT NULL,
  `model` int DEFAULT NULL,
  `color` int DEFAULT NULL,
  `price` float DEFAULT '0',
  `kolvo` int DEFAULT NULL,
  `summa` float DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `client_id` (`klient`)
) ENGINE=InnoDB AUTO_INCREMENT=4855 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `1c_klient_proizvodstvo`
--

DROP TABLE IF EXISTS `1c_klient_proizvodstvo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `1c_klient_proizvodstvo` (
  `id` mediumint NOT NULL AUTO_INCREMENT,
  `klient` mediumint DEFAULT NULL,
  `model` mediumint DEFAULT NULL,
  `color` mediumint DEFAULT NULL,
  `kroitsya` mediumint DEFAULT NULL,
  `gotovkroy` mediumint DEFAULT NULL,
  `zapusk` mediumint DEFAULT NULL,
  `vposhive` mediumint DEFAULT NULL,
  `skladgp` mediumint DEFAULT NULL,
  `cena` float DEFAULT '0',
  `cena_rub` float DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=814 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `1c_manager`
--

DROP TABLE IF EXISTS `1c_manager`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `1c_manager` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(255) DEFAULT '',
  `otdel` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `1c_material`
--

DROP TABLE IF EXISTS `1c_material`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `1c_material` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(255) DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=10880 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `1c_materialcolors`
--

DROP TABLE IF EXISTS `1c_materialcolors`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `1c_materialcolors` (
  `id` int NOT NULL AUTO_INCREMENT,
  `color` int DEFAULT NULL,
  `material` int DEFAULT NULL,
  `photo` varchar(255) DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `1c_modelcolorphoto`
--

DROP TABLE IF EXISTS `1c_modelcolorphoto`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `1c_modelcolorphoto` (
  `id` int NOT NULL AUTO_INCREMENT,
  `model` int DEFAULT NULL,
  `color` int DEFAULT NULL,
  `photo` varchar(255) DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `1c_modelpicture`
--

DROP TABLE IF EXISTS `1c_modelpicture`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `1c_modelpicture` (
  `id` int NOT NULL AUTO_INCREMENT,
  `model` int DEFAULT NULL,
  `main` tinyint(1) DEFAULT NULL,
  `notshow` tinyint(1) DEFAULT NULL,
  `photo` varchar(255) DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=978 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `1c_modelrazdely`
--

DROP TABLE IF EXISTS `1c_modelrazdely`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `1c_modelrazdely` (
  `id` int NOT NULL AUTO_INCREMENT,
  `model` int DEFAULT NULL,
  `razdel` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1902 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `1c_models`
--

DROP TABLE IF EXISTS `1c_models`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `1c_models` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(255) DEFAULT '',
  `articul` varchar(255) DEFAULT '',
  `comment` varchar(255) DEFAULT '',
  `status` int DEFAULT NULL,
  `razmerov` tinyint DEFAULT '0',
  `weight` float DEFAULT NULL,
  `line` varchar(255) DEFAULT '',
  `material` int DEFAULT NULL,
  `skidka` tinyint DEFAULT '0',
  `photo` varchar(255) DEFAULT '',
  `price` float DEFAULT '0',
  `collection` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1010 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `1c_modelvideo`
--

DROP TABLE IF EXISTS `1c_modelvideo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `1c_modelvideo` (
  `id` int NOT NULL AUTO_INCREMENT,
  `model` int DEFAULT NULL,
  `video` varchar(255) DEFAULT '',
  `preview` varchar(255) DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `1c_otdely`
--

DROP TABLE IF EXISTS `1c_otdely`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `1c_otdely` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(255) DEFAULT '',
  `rukovoditel` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `1c_price`
--

DROP TABLE IF EXISTS `1c_price`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `1c_price` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(255) DEFAULT '',
  `model` int DEFAULT NULL,
  `typeprice` int DEFAULT NULL,
  `price` float DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `1c_prodagi_klientov`
--

DROP TABLE IF EXISTS `1c_prodagi_klientov`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `1c_prodagi_klientov` (
  `id` int NOT NULL AUTO_INCREMENT,
  `klient` int DEFAULT NULL,
  `model` int DEFAULT NULL,
  `color` int DEFAULT NULL,
  `razmer` varchar(32) DEFAULT NULL,
  `kolichestvo` int DEFAULT NULL,
  `cena` float DEFAULT '0',
  `cena_rub` float DEFAULT '0',
  `datav` date DEFAULT NULL,
  `schet` int DEFAULT '0',
  `collection` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4887 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `1c_raschety_klientov`
--

DROP TABLE IF EXISTS `1c_raschety_klientov`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `1c_raschety_klientov` (
  `id` int NOT NULL AUTO_INCREMENT,
  `klient` int DEFAULT NULL,
  `nach_ost` float DEFAULT '0',
  `kon_ost` float DEFAULT '0',
  `dolg_plus` float DEFAULT '0',
  `dolg_minus` float DEFAULT '0',
  `nach_data` date DEFAULT NULL,
  `kon_data` date DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `client_id` (`klient`)
) ENGINE=InnoDB AUTO_INCREMENT=415 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `1c_razdelycataloga`
--

DROP TABLE IF EXISTS `1c_razdelycataloga`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `1c_razdelycataloga` (
  `id` int NOT NULL AUTO_INCREMENT,
  `razdel` varchar(255) DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `1c_rezervy_klientov`
--

DROP TABLE IF EXISTS `1c_rezervy_klientov`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `1c_rezervy_klientov` (
  `id` int NOT NULL AUTO_INCREMENT,
  `klient` int DEFAULT NULL,
  `model` int DEFAULT NULL,
  `color` int DEFAULT NULL,
  `razmer` varchar(32) DEFAULT NULL,
  `kolichestvo` int DEFAULT NULL,
  `cena` float DEFAULT '0',
  `cena_rub` float DEFAULT '0',
  `ves` float DEFAULT '0',
  `datav` date DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `client_id` (`klient`),
  KEY `model_id` (`model`),
  KEY `color_id` (`color`)
) ENGINE=InnoDB AUTO_INCREMENT=461 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `1c_scheta_klientov`
--

DROP TABLE IF EXISTS `1c_scheta_klientov`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `1c_scheta_klientov` (
  `id` int NOT NULL AUTO_INCREMENT,
  `id_dok` int DEFAULT NULL,
  `klient` int DEFAULT NULL,
  `model` int DEFAULT NULL,
  `color` int DEFAULT NULL,
  `razmer` varchar(32) DEFAULT NULL,
  `osnovanie` varchar(255) DEFAULT NULL,
  `flstr` tinyint(1) DEFAULT '0',
  `kolichestvo` int DEFAULT NULL,
  `cena` float DEFAULT '0',
  `collection` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5867 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `1c_scheta_klientov_dok`
--

DROP TABLE IF EXISTS `1c_scheta_klientov_dok`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `1c_scheta_klientov_dok` (
  `id` int NOT NULL AUTO_INCREMENT,
  `klient` int DEFAULT NULL,
  `pklient` varchar(256) DEFAULT NULL,
  `datadok` date DEFAULT NULL,
  `nomerdok` varchar(8) DEFAULT NULL,
  `ispavans` int DEFAULT NULL,
  `koplate` int DEFAULT NULL,
  `val` varchar(3) DEFAULT NULL,
  `kurs_rub` float DEFAULT NULL,
  `oplachen` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1862 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `1c_sklad`
--

DROP TABLE IF EXISTS `1c_sklad`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `1c_sklad` (
  `id` int NOT NULL AUTO_INCREMENT,
  `sklad` int DEFAULT NULL,
  `model` int DEFAULT NULL,
  `color` int DEFAULT NULL,
  `nomenklatura` int DEFAULT NULL,
  `line` tinyint(1) DEFAULT '0',
  `kolichestvo` int DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=402 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `1c_sklady`
--

DROP TABLE IF EXISTS `1c_sklady`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `1c_sklady` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(255) DEFAULT '',
  `obsch` tinyint(1) DEFAULT '0',
  `mesto` tinyint(1) DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `1c_sklady_zapret`
--

DROP TABLE IF EXISTS `1c_sklady_zapret`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `1c_sklady_zapret` (
  `id` int NOT NULL AUTO_INCREMENT,
  `kontragent` int DEFAULT '0',
  `sklad` int DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `1c_typprice`
--

DROP TABLE IF EXISTS `1c_typprice`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `1c_typprice` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(255) DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `1c_zakaz_history`
--

DROP TABLE IF EXISTS `1c_zakaz_history`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `1c_zakaz_history` (
  `cod` int NOT NULL AUTO_INCREMENT,
  `klient` int DEFAULT NULL,
  `datez` date DEFAULT NULL,
  `model` mediumint DEFAULT NULL,
  `color` mediumint DEFAULT NULL,
  `kolichestvo` mediumint DEFAULT NULL,
  PRIMARY KEY (`cod`)
) ENGINE=InnoDB AUTO_INCREMENT=22407 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `1c_zayavka_klientov`
--

DROP TABLE IF EXISTS `1c_zayavka_klientov`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `1c_zayavka_klientov` (
  `id` int NOT NULL AUTO_INCREMENT,
  `klient` int DEFAULT NULL,
  `model` int DEFAULT NULL,
  `color` int DEFAULT NULL,
  `kolichestvo` int DEFAULT NULL,
  `cena` float DEFAULT '0',
  `cena_rub` float DEFAULT '0',
  `datav` date DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=206 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-07-29 16:31:08
