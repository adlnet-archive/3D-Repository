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
  `RESTAPI` varchar(255) NOT NULL,
  `SOAPAPI` varchar(255) NOT NULL,
  `OrganizationName` varchar(45) NOT NULL,
  `OrganizationURL` varchar(45) NOT NULL,
  `OrganizationPOC` varchar(45) NOT NULL,
  `OrganizationPOCEmail` varchar(45) NOT NULL,
  `OrganizationPOCPassword` varchar(45) NOT NULL,
  `ActivationState` int(10) unsigned NOT NULL,
  `AllowFederatedSearch` tinyint(1) NOT NULL,
  `AllowFederatedDownload` tinyint(1) NOT NULL,
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `federaterecords`
--

/*!40000 ALTER TABLE `federaterecords` DISABLE KEYS */;
INSERT INTO `federaterecords` (`prefix`,`RESTAPI`,`SOAPAPI`,`OrganizationName`,`OrganizationURL`,`OrganizationPOC`,`OrganizationPOCEmail`,`OrganizationPOCPassword`,`ActivationState`,`AllowFederatedSearch`,`AllowFederatedDownload`,`ID`) VALUES 
 ('adl','http://10.100.10.90:8880/api/_3DRAPI.svc','','Stage Server!!!','http://www.somecompany.com','Admin','admin@somecompany.com','password',5,1,1,7),
 ('adl','http://10.100.10.90:8880/api/_3DRAPI.svc','','STAGE SERVER!!!','http://www.somecompany.com','Admin','admin@somecompany.com','password',5,1,1,8),
 ('localhost','http://localhost/api/_3DRAPI.svc','','localhost','http://www.somecompany.com','Admin','admin@somecompany.com','password',0,1,1,9),
 ('adl','http://3dr.adlnet.gov/api/rest','','RELEASESERVER!!!','http://www.somecompany.com','Admin','admin@somecompany.com','password',0,1,1,10),
 ('adlSTAGE','http://10.100.10.90:8880/api/_3DRAPI.svc','','STAGE SERVER!!!','http://www.somecompany.com','Admin','admin@somecompany.com','password',5,1,1,11),
 ('adlSTAGE','http://10.100.10.90:8880/api/_3DRAPI.svc','','Some Company','http://www.somecompany.com','Admin','admin@somecompany.com','password',0,1,1,12);
/*!40000 ALTER TABLE `federaterecords` ENABLE KEYS */;


--
-- Definition of procedure `CreateFederateRecord`
--

DROP PROCEDURE IF EXISTS `CreateFederateRecord`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `CreateFederateRecord`(newPrefix varchar(255),
newRESTAPI varchar(255),
newSOAPAPI varchar(255),
newOrganizationName   varchar(255),
newOrganizationURL         varchar(255),
newOrganizationPOC         varchar(255),
newOrganizationPOCEmail    varchar(255),
newOrganizationPOCPassword  varchar(255),
newActivationState           int       ,
newAllowFederatedSearch      bool       ,
newAllowFederatedDownload    bool       
)
BEGIN

      INSERT INTO `3drfederateregistry`.`federaterecords`(prefix,RESTAPI,SOAPAPI,
      OrganizationName,
      OrganizationURL,
      OrganizationPOC,
      OrganizationPOCEmail,
      OrganizationPOCPassword,
      ActivationState,
      AllowFederatedSearch  ,
      AllowFederatedDownload

      ) values(newPrefix, newRESTAPI, newSOAPAPI,newOrganizationName,newOrganizationURL,
      newOrganizationPOC,
      newOrganizationPOCEmail,
      newOrganizationPOCPassword,
      newActivationState,
      newAllowFederatedSearch,
      newAllowFederatedDownload
      );
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

        SELECT * FROM federaterecords WHERE prefix = newprefix AND ActivationState != 5;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `UpdateFederateRecord`
--

DROP PROCEDURE IF EXISTS `UpdateFederateRecord`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `UpdateFederateRecord`(
       newprefix varchar(255),
       newRESTAPI  varchar(255),
       newSOAPAPI  varchar(255),
       newOrganizationName VARCHAR(255),
       newOrganizationUrl VARCHAR(255),
       newOrganizationPOC VARCHAR(255),
       newOrganizationPOCEmail VARCHAR(255),
       newOrganizationPOCPassword VARCHAR(255),
       newActivationState integer(10),
       newAllowFederatedSearch tinyint(1),
       newAllowFederatedDownload tinyint(1)

       )
BEGIN

      UPDATE `federaterecords` SET
       OrganizationName=newOrganizationName  ,
       OrganizationUrl=newOrganizationUrl  ,
       OrganizationPOC=newOrganizationPOC ,
       OrganizationPOCEmail=newOrganizationPOCEmail,
       OrganizationPOCPassword=newOrganizationPOCPassword ,
       ActivationState=newActivationState ,
       AllowFederatedSearch=newAllowFederatedSearch,
       AllowFederatedDownload=newAllowFederatedDownload,
       RESTAPI = newRESTAPI,
       SOAPAPI = newSOAPAPI
      WHERE prefix = newprefix AND ActivationState != 5;
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
