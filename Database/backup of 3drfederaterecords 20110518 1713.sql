-- MySQL Administrator dump 1.4
--
-- ------------------------------------------------------
-- Server version	5.5.10


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


--
-- Create schema 3drfederateregistry
--

CREATE DATABASE IF NOT EXISTS 3drfederateregistry;
USE 3drfederateregistry;

--
-- Definition of table `federaterecords`
--

DROP TABLE IF EXISTS `federaterecords`;
CREATE TABLE `federaterecords` (
  `prefix` varchar(255) NOT NULL,
  `JSONAPI` varchar(255) NOT NULL,
  `XMLAPI` varchar(255) NOT NULL,
  `SOAPAPI` varchar(255) NOT NULL,
  PRIMARY KEY (`prefix`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `federaterecords`
--

/*!40000 ALTER TABLE `federaterecords` DISABLE KEYS */;
INSERT INTO `federaterecords` (`prefix`,`JSONAPI`,`XMLAPI`,`SOAPAPI`) VALUES 
 ('adl','http://localhost/3DRAPI/_3DRAPI_Json.svc','http://localhost/3DRAPI/_3DRAPI_Xml.svc','http://localhost/3DRAPI/_3DRAPI_Soap.svc');
/*!40000 ALTER TABLE `federaterecords` ENABLE KEYS */;


--
-- Definition of procedure `CreateFederateRecord`
--

DROP PROCEDURE IF EXISTS `CreateFederateRecord`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `CreateFederateRecord`(newPrefix varchar(255), newJSONAPI varchar(255), newXMLAPI varchar(255), newSOAPAPI varchar(255) )
BEGIN

      INSERT INTO `3drfederateregistry`.`federaterecords`(prefix,JSONAPI,XMLAPI,SOAPAPI) values(newPrefix, newJSONAPI, newXMLAPI, newSOAPAPI);
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `GetAllFederateRecords`
--

DROP PROCEDURE IF EXISTS `GetAllFederateRecords`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetAllFederateRecords`()
BEGIN
  SELECT * FROM federaterecords;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `GetFederateRecord`
--

DROP PROCEDURE IF EXISTS `GetFederateRecord`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetFederateRecord`(newprefix varchar(255))
BEGIN

        SELECT * FROM federaterecords WHERE prefix = newprefix;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;



/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
