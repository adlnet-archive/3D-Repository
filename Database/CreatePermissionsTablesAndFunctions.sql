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
-- Create schema test2
--

CREATE DATABASE IF NOT EXISTS test2;
USE test2;

--
-- Definition of table `associatedkeywords`
--

DROP TABLE IF EXISTS `associatedkeywords`;
CREATE TABLE `associatedkeywords` (
  `ContentObjectId` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `KeywordId` int(10) unsigned NOT NULL,
  KEY `FK_AssociatedKeywords_1` (`ContentObjectId`),
  KEY `FK_associatedkeywords_2` (`KeywordId`),
  CONSTRAINT `FK_AssociatedKeywords_1` FOREIGN KEY (`ContentObjectId`) REFERENCES `contentobjects` (`ID`) ON DELETE CASCADE,
  CONSTRAINT `FK_associatedkeywords_2` FOREIGN KEY (`KeywordId`) REFERENCES `keywords` (`ID`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=142 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `associatedkeywords`
--

/*!40000 ALTER TABLE `associatedkeywords` DISABLE KEYS */;
INSERT INTO `associatedkeywords` (`ContentObjectId`,`KeywordId`) VALUES 
 (138,4),
 (139,5),
 (140,6),
 (141,7);
/*!40000 ALTER TABLE `associatedkeywords` ENABLE KEYS */;


--
-- Definition of table `contentobjects`
--

DROP TABLE IF EXISTS `contentobjects`;
CREATE TABLE `contentobjects` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Description` varchar(400) NOT NULL DEFAULT ' ',
  `Title` varchar(400) NOT NULL DEFAULT ' ',
  `ContentFileName` varchar(400) NOT NULL DEFAULT ' ',
  `ContentFileId` varchar(400) NOT NULL DEFAULT ' ',
  `ScreenShotFileName` varchar(400) NOT NULL DEFAULT ' ',
  `ScreenShotFileId` varchar(400) NOT NULL DEFAULT ' ',
  `Submitter` varchar(400) NOT NULL DEFAULT ' ',
  `SponsorLogoFileName` varchar(400) NOT NULL DEFAULT ' ',
  `SponsorLogoFileId` varchar(400) NOT NULL DEFAULT ' ',
  `DeveloperLogoFileName` varchar(400) NOT NULL DEFAULT ' ',
  `DeveloperLogoFileId` varchar(400) NOT NULL DEFAULT ' ',
  `AssetType` varchar(400) NOT NULL DEFAULT ' ',
  `DisplayFileName` varchar(400) NOT NULL DEFAULT ' ',
  `DisplayFileId` varchar(400) NOT NULL DEFAULT ' ',
  `MoreInfoUrl` varchar(400) NOT NULL DEFAULT ' ',
  `DeveloperName` varchar(400) NOT NULL DEFAULT ' ',
  `SponsorName` varchar(400) NOT NULL DEFAULT ' ',
  `ArtistName` varchar(400) NOT NULL DEFAULT ' ',
  `CreativeCommonsLicenseUrl` varchar(400) NOT NULL DEFAULT ' ',
  `UnitScale` varchar(400) NOT NULL DEFAULT ' ',
  `UpAxis` varchar(400) NOT NULL DEFAULT ' ',
  `UVCoordinateChannel` varchar(400) NOT NULL DEFAULT ' ',
  `IntentionOfTexture` varchar(400) NOT NULL DEFAULT ' ',
  `Format` varchar(400) NOT NULL DEFAULT ' ',
  `Views` int(10) unsigned zerofill NOT NULL DEFAULT '0000000000',
  `Downloads` int(10) unsigned zerofill NOT NULL DEFAULT '0000000000',
  `NumPolygons` int(10) unsigned zerofill NOT NULL DEFAULT '0000000000',
  `NumTextures` int(10) unsigned zerofill NOT NULL DEFAULT '0000000000',
  `UploadedDate` datetime DEFAULT '0000-00-00 00:00:00',
  `LastModified` datetime DEFAULT '0000-00-00 00:00:00',
  `LastViewed` datetime DEFAULT '0000-00-00 00:00:00',
  `PID` varchar(45) DEFAULT NULL,
  `Revision` varchar(45) NOT NULL,
  `Enabled` tinyint(1) DEFAULT NULL,
  `requiressubmit` tinyint(1) DEFAULT NULL,
  `OriginalFileName` varchar(400) DEFAULT NULL,
  `OriginalFileId` varchar(400) DEFAULT NULL,
  `UploadComplete` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `FK_contentobjects_1` (`Submitter`)
) ENGINE=InnoDB AUTO_INCREMENT=149 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `contentobjects`
--

/*!40000 ALTER TABLE `contentobjects` DISABLE KEYS */;
INSERT INTO `contentobjects` (`ID`,`Description`,`Title`,`ContentFileName`,`ContentFileId`,`ScreenShotFileName`,`ScreenShotFileId`,`Submitter`,`SponsorLogoFileName`,`SponsorLogoFileId`,`DeveloperLogoFileName`,`DeveloperLogoFileId`,`AssetType`,`DisplayFileName`,`DisplayFileId`,`MoreInfoUrl`,`DeveloperName`,`SponsorName`,`ArtistName`,`CreativeCommonsLicenseUrl`,`UnitScale`,`UpAxis`,`UVCoordinateChannel`,`IntentionOfTexture`,`Format`,`Views`,`Downloads`,`NumPolygons`,`NumTextures`,`UploadedDate`,`LastModified`,`LastViewed`,`PID`,`Revision`,`Enabled`,`requiressubmit`,`OriginalFileName`,`OriginalFileId`,`UploadComplete`) VALUES 
 (138,'','Bench','bench.zip','content192','screenshot.png','content189','psadmin@problemsolutions.net','screenshot.png','','screenshot.png','','Model','bench.o3d','content192','','','','','http://creativecommons.org/licenses/by-sa/3.0/legalcode','1','Y','','','.dae',0000000001,0000000000,0000000284,0000000001,'0000-00-00 00:00:00','2011-05-20 11:34:06','2011-05-20 11:34:06','adl:65','0',1,0,'original_bench.zip','content190',0),
 (139,'','Box','box.zip','content196','screenshot.png','content193','psadmin@problemsolutions.net','','','','','Model','box.o3d','content196','','','','','http://creativecommons.org/licenses/by-sa/3.0/legalcode','1','Y','','','.dae',0000000004,0000000000,0000000012,0000000001,'0000-00-00 00:00:00','2011-05-20 11:34:26','2011-05-23 13:25:52','adl:66','0',1,0,'original_box.zip','content194',0),
 (140,'','Console','console.zip','content200','screenshot.png','content197','psadmin@problemsolutions.net','','','','','Model','console.o3d','content200','','','','','http://creativecommons.org/licenses/by-sa/3.0/legalcode','0.0254','Z','','','.fbx',0000000001,0000000000,0000001640,0000000001,'0000-00-00 00:00:00','2011-05-20 11:34:52','2011-05-20 11:34:52','adl:67','0',1,0,'original_console.zip','content198',0),
 (141,'','Duffle Bag','duffle_bag.zip','content204','screenshot.png','content201','psadmin@problemsolutions.net','','','','','Model','duffle_bag.o3d','content204','','','','','http://creativecommons.org/licenses/by-sa/3.0/legalcode','1','Y','','','.dae',0000000005,0000000000,0000000694,0000000002,'0000-00-00 00:00:00','2011-05-20 11:35:19','2011-05-23 13:23:12','adl:68','0',1,0,'original_duffle_bag.zip','content202',0),
 (142,'','logotest','logotest.zip','','',' ','psadmin@problemsolutions.net','','','','','Model','logotest.o3d','','','','','','','1','Y','','','.dae',0000000000,0000000000,0000000284,0000000001,'0000-00-00 00:00:00','0000-00-00 00:00:00','0000-00-00 00:00:00','adl:69','0',0,0,'','',0),
 (143,'','asdf','asdf.zip','content209','screenshot.png','content206','psadmin@problemsolutions.net','','','developer_logo.JPG','content205','Model','asdf.o3d','content209','','','','','http://creativecommons.org/licenses/by-sa/3.0/legalcode','0.01','Z','','','.DAE',0000000003,0000000000,0000000666,0000000001,'0000-00-00 00:00:00','2011-05-24 14:25:42','2011-05-24 14:31:54','adl:70','0',1,0,'original_asdf.zip','content207',0),
 (144,'','a','a.zip','content214','screenshot.png','content211','psadmin@problemsolutions.net','','','developer_logo.JPG','content210','Model','a.o3d','content214','','asdf','','asdf','http://creativecommons.org/licenses/by-sa/3.0/legalcode','0.01','Z','','','.DAE',0000000001,0000000000,0000000666,0000000001,'0000-00-00 00:00:00','2011-05-24 14:34:28','2011-05-24 14:34:28','adl:71','0',1,0,'original_a.zip','content212',0),
 (145,'','a','a.zip','content217','','','psadmin@problemsolutions.net','','','','','Model','a.o3d','content217','','','','','http://creativecommons.org/licenses/by-sa/3.0/legalcode','1','Y','','','.dae',0000000001,0000000000,0000000284,0000000001,'0000-00-00 00:00:00','2011-05-24 15:47:14','2011-05-24 15:47:15','adl:72','0',1,0,'original_a.zip','content215',0),
 (146,'','1','1.zip','content220','','','psadmin@problemsolutions.net','','','','','Model','1.o3d','content220','','','','','http://creativecommons.org/licenses/by-sa/3.0/legalcode','1','Y','','','.dae',0000000001,0000000000,0000000284,0000000001,'0000-00-00 00:00:00','2011-05-24 15:50:16','2011-05-24 15:50:16','adl:73','0',1,0,'original_1.zip','content218',0),
 (147,'','1','1.zip','content223','','','psadmin@problemsolutions.net','','','','','Model','1.o3d','content223','','','','','http://creativecommons.org/licenses/by-sa/3.0/legalcode','0.01','Z','','','.DAE',0000000001,0000000000,0000000666,0000000001,'0000-00-00 00:00:00','2011-05-24 15:59:37','2011-05-24 15:59:38','adl:74','0',1,0,'original_1.zip','content221',0),
 (148,'','1','1.zip','content226','','','psadmin@problemsolutions.net','','','','','Model','1.o3d','content226','','','','','http://creativecommons.org/licenses/by-sa/3.0/legalcode','1','Y','','','.dae',0000000001,0000000000,0000000012,0000000001,'0000-00-00 00:00:00','2011-05-24 16:03:32','2011-05-24 16:03:40','adl:75','0',1,0,'original_1.zip','content224',0);
/*!40000 ALTER TABLE `contentobjects` ENABLE KEYS */;


--
-- Definition of table `current_uploads`
--

DROP TABLE IF EXISTS `current_uploads`;
CREATE TABLE `current_uploads` (
  `pid` varchar(100) NOT NULL,
  `hash` varchar(100) NOT NULL,
  KEY `pid` (`pid`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Dumping data for table `current_uploads`
--

/*!40000 ALTER TABLE `current_uploads` DISABLE KEYS */;
/*!40000 ALTER TABLE `current_uploads` ENABLE KEYS */;


--
-- Definition of table `keywords`
--

DROP TABLE IF EXISTS `keywords`;
CREATE TABLE `keywords` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Keyword` varchar(45) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `keywords`
--

/*!40000 ALTER TABLE `keywords` DISABLE KEYS */;
INSERT INTO `keywords` (`ID`,`Keyword`) VALUES 
 (1,'tag'),
 (2,'tag2'),
 (3,'tag3'),
 (4,'Military'),
 (5,'Military'),
 (6,'Military'),
 (7,'Military');
/*!40000 ALTER TABLE `keywords` ENABLE KEYS */;


--
-- Definition of table `missingtextures`
--

DROP TABLE IF EXISTS `missingtextures`;
CREATE TABLE `missingtextures` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Filename` varchar(45) NOT NULL,
  `Type` varchar(45) NOT NULL,
  `UVSet` int(10) unsigned NOT NULL,
  `PID` varchar(45) NOT NULL,
  `Revision` int(10) unsigned NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `missingtextures`
--

/*!40000 ALTER TABLE `missingtextures` DISABLE KEYS */;
/*!40000 ALTER TABLE `missingtextures` ENABLE KEYS */;


--
-- Definition of table `openid`
--

DROP TABLE IF EXISTS `openid`;
CREATE TABLE `openid` (
  `openId_url` varchar(256) CHARACTER SET utf8 DEFAULT NULL,
  `user_id` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `openid`
--

/*!40000 ALTER TABLE `openid` DISABLE KEYS */;
/*!40000 ALTER TABLE `openid` ENABLE KEYS */;


--
-- Definition of table `personalization`
--

DROP TABLE IF EXISTS `personalization`;
CREATE TABLE `personalization` (
  `username` varchar(255) DEFAULT NULL,
  `path` varchar(255) DEFAULT NULL,
  `applicationname` varchar(255) DEFAULT NULL,
  `personalizationblob` blob
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `personalization`
--

/*!40000 ALTER TABLE `personalization` DISABLE KEYS */;
/*!40000 ALTER TABLE `personalization` ENABLE KEYS */;


--
-- Definition of table `pidingroup`
--

DROP TABLE IF EXISTS `pidingroup`;
CREATE TABLE `pidingroup` (
  `PID` varchar(255) NOT NULL,
  `GroupName` varchar(45) NOT NULL,
  `PermissionLevel` int(10) unsigned NOT NULL,
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `pidingroup`
--

/*!40000 ALTER TABLE `pidingroup` DISABLE KEYS */;
INSERT INTO `pidingroup` (`PID`,`GroupName`,`PermissionLevel`,`id`) VALUES 
 ('adl:65','NPS',0,9);
/*!40000 ALTER TABLE `pidingroup` ENABLE KEYS */;


--
-- Definition of table `profiles`
--

DROP TABLE IF EXISTS `profiles`;
CREATE TABLE `profiles` (
  `UniqueID` int(8) NOT NULL AUTO_INCREMENT,
  `Username` varchar(255) NOT NULL DEFAULT '',
  `ApplicationName` varchar(255) NOT NULL DEFAULT '',
  `IsAnonymous` tinyint(1) DEFAULT '0',
  `LastActivityDate` datetime DEFAULT NULL,
  `LastUpdatedDate` datetime DEFAULT NULL,
  PRIMARY KEY (`UniqueID`),
  UNIQUE KEY `PKProfiles` (`Username`,`ApplicationName`),
  UNIQUE KEY `PKID` (`UniqueID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `profiles`
--

/*!40000 ALTER TABLE `profiles` DISABLE KEYS */;
/*!40000 ALTER TABLE `profiles` ENABLE KEYS */;


--
-- Definition of table `reviews`
--

DROP TABLE IF EXISTS `reviews`;
CREATE TABLE `reviews` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Rating` int(10) unsigned NOT NULL,
  `Text` varchar(45) NOT NULL,
  `SubmittedBy` varchar(45) NOT NULL,
  `SubmittedDate` datetime NOT NULL,
  `ContentObjectId` varchar(400) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `FK_Reviews_1` (`ContentObjectId`),
  KEY `FK_reviews_2` (`SubmittedBy`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `reviews`
--

/*!40000 ALTER TABLE `reviews` DISABLE KEYS */;
INSERT INTO `reviews` (`ID`,`Rating`,`Text`,`SubmittedBy`,`SubmittedDate`,`ContentObjectId`) VALUES 
 (1,3,'test review','psadmin@problemsolutions.net','2011-05-18 17:11:19','adl:64'),
 (2,3,'test review 12','psadmin@problemsolutions.net','2011-05-18 17:11:25','adl:64');
/*!40000 ALTER TABLE `reviews` ENABLE KEYS */;


--
-- Definition of table `roles`
--

DROP TABLE IF EXISTS `roles`;
CREATE TABLE `roles` (
  `Rolename` varchar(255) NOT NULL DEFAULT '',
  `ApplicationName` varchar(255) NOT NULL DEFAULT '',
  PRIMARY KEY (`Rolename`,`ApplicationName`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `roles`
--

/*!40000 ALTER TABLE `roles` DISABLE KEYS */;
INSERT INTO `roles` (`Rolename`,`ApplicationName`) VALUES 
 ('Administrators','PS'),
 ('Users','PS');
/*!40000 ALTER TABLE `roles` ENABLE KEYS */;


--
-- Definition of table `sitemap`
--

DROP TABLE IF EXISTS `sitemap`;
CREATE TABLE `sitemap` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `ApplicationName` varchar(255) NOT NULL DEFAULT '',
  `Title` varchar(255) DEFAULT NULL,
  `Description` text,
  `Url` text,
  `Roles` text,
  `Parent` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `sitemap`
--

/*!40000 ALTER TABLE `sitemap` DISABLE KEYS */;
/*!40000 ALTER TABLE `sitemap` ENABLE KEYS */;


--
-- Definition of table `supportingfiles`
--

DROP TABLE IF EXISTS `supportingfiles`;
CREATE TABLE `supportingfiles` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Filename` varchar(45) NOT NULL,
  `Description` varchar(45) NOT NULL,
  `PID` varchar(45) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `supportingfiles`
--

/*!40000 ALTER TABLE `supportingfiles` DISABLE KEYS */;
/*!40000 ALTER TABLE `supportingfiles` ENABLE KEYS */;


--
-- Definition of table `text_log`
--

DROP TABLE IF EXISTS `text_log`;
CREATE TABLE `text_log` (
  `Log` varchar(255) NOT NULL DEFAULT ''
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `text_log`
--

/*!40000 ALTER TABLE `text_log` DISABLE KEYS */;
/*!40000 ALTER TABLE `text_log` ENABLE KEYS */;


--
-- Definition of table `texturereferences`
--

DROP TABLE IF EXISTS `texturereferences`;
CREATE TABLE `texturereferences` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Filename` varchar(45) NOT NULL,
  `Type` varchar(45) NOT NULL,
  `UVSet` int(10) unsigned NOT NULL,
  `PID` varchar(45) NOT NULL,
  `Revision` int(10) unsigned NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `texturereferences`
--

/*!40000 ALTER TABLE `texturereferences` DISABLE KEYS */;
INSERT INTO `texturereferences` (`ID`,`Filename`,`Type`,`UVSet`,`PID`,`Revision`) VALUES 
 (17,'big_box.png','unknown',0,'adl:75',0);
/*!40000 ALTER TABLE `texturereferences` ENABLE KEYS */;


--
-- Definition of table `usergroups`
--

DROP TABLE IF EXISTS `usergroups`;
CREATE TABLE `usergroups` (
  `GroupName` varchar(255) NOT NULL,
  `Owner` varchar(255) NOT NULL,
  `Description` varchar(1000) NOT NULL,
  `PermissionLevel` int(10) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`GroupName`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `usergroups`
--

/*!40000 ALTER TABLE `usergroups` DISABLE KEYS */;
INSERT INTO `usergroups` (`GroupName`,`Owner`,`Description`,`PermissionLevel`) VALUES 
 ('ADL','psadmin@problemsolutions.net','test',2),
 ('DAU','test@tset.com','test',0),
 ('NPS','test@test.com','test',0),
 ('TestGroup1','permtest@test.com','test',1),
 ('TestGroup2','permtest@test.com','test',1);
/*!40000 ALTER TABLE `usergroups` ENABLE KEYS */;


--
-- Definition of table `userprofiles`
--

DROP TABLE IF EXISTS `userprofiles`;
CREATE TABLE `userprofiles` (
  `UserID` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `UserGuid` char(36) NOT NULL,
  `FirstName` varchar(255) DEFAULT NULL,
  `LastName` varchar(255) DEFAULT NULL,
  `Email` varchar(255) DEFAULT NULL,
  `WebsiteURL` text,
  `SponsorName` varchar(255) DEFAULT NULL,
  `SponsorLogo` mediumblob,
  `DeveloperName` varchar(255) DEFAULT NULL,
  `DeveloperLogo` mediumblob,
  `ArtistName` varchar(255) DEFAULT NULL,
  `Phone` varchar(50) DEFAULT NULL,
  `CreatedDate` date DEFAULT NULL,
  `CreatedBy` varchar(255) DEFAULT NULL,
  `LastEditedBy` varchar(255) DEFAULT NULL,
  `Comments` varchar(255) DEFAULT NULL,
  `Description` varchar(255) DEFAULT NULL,
  `DeveloperLogoContentType` varchar(255) DEFAULT NULL,
  `SponsorLogoContentType` varchar(255) DEFAULT NULL,
  `DeveloperLogoFileName` varchar(255) DEFAULT NULL,
  `SponsorLogoFileName` varchar(255) DEFAULT NULL,
  `LastEditedDate` date DEFAULT NULL,
  `UserName` varchar(255) NOT NULL,
  PRIMARY KEY (`UserID`),
  UNIQUE KEY `UserID` (`UserID`),
  KEY `UserProfiles_UserGuid_fkey` (`UserGuid`),
  CONSTRAINT `UserProfiles_UserGuid_fkey` FOREIGN KEY (`UserGuid`) REFERENCES `users` (`PKID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `userprofiles`
--

/*!40000 ALTER TABLE `userprofiles` DISABLE KEYS */;
/*!40000 ALTER TABLE `userprofiles` ENABLE KEYS */;


--
-- Definition of table `users`
--

DROP TABLE IF EXISTS `users`;
CREATE TABLE `users` (
  `PKID` varchar(255) NOT NULL DEFAULT '',
  `Username` varchar(255) NOT NULL DEFAULT '',
  `ApplicationName` varchar(255) NOT NULL DEFAULT '',
  `Email` varchar(128) DEFAULT NULL,
  `Comment` varchar(255) DEFAULT NULL,
  `Password` varchar(128) NOT NULL DEFAULT '',
  `FailedPasswordAttemptWindowStart` datetime DEFAULT NULL,
  `PasswordQuestion` varchar(255) DEFAULT NULL,
  `IsLockedOut` tinyint(1) DEFAULT '0',
  `PasswordAnswer` varchar(255) DEFAULT NULL,
  `FailedPasswordAnswerAttemptCount` int(8) DEFAULT '0',
  `FailedPasswordAttemptCount` int(8) DEFAULT '0',
  `IsApproved` tinyint(1) NOT NULL DEFAULT '0',
  `FailedPasswordAnswerAttemptWindowStart` datetime DEFAULT NULL,
  `LastActivityDate` datetime DEFAULT NULL,
  `IsOnLine` tinyint(1) DEFAULT '0',
  `CreationDate` datetime DEFAULT NULL,
  `LastPasswordChangedDate` datetime DEFAULT NULL,
  `LastLockedOutDate` datetime DEFAULT NULL,
  `LastLoginDate` datetime DEFAULT NULL,
  PRIMARY KEY (`PKID`),
  UNIQUE KEY `PKID` (`PKID`),
  KEY `PKID_2` (`PKID`),
  KEY `usr` (`Username`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `users`
--

/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` (`PKID`,`Username`,`ApplicationName`,`Email`,`Comment`,`Password`,`FailedPasswordAttemptWindowStart`,`PasswordQuestion`,`IsLockedOut`,`PasswordAnswer`,`FailedPasswordAnswerAttemptCount`,`FailedPasswordAttemptCount`,`IsApproved`,`FailedPasswordAnswerAttemptWindowStart`,`LastActivityDate`,`IsOnLine`,`CreationDate`,`LastPasswordChangedDate`,`LastLockedOutDate`,`LastLoginDate`) VALUES 
 ('dda9a618-fb5b-40ae-929d-a68c5713c98c','psadmin@problemsolutions.net','PS','psAdmin@problemsolutions.net','','/nzkN++Zudkox+eKZGSE/FJIoWxUBDVw5nfjGTy8N0M=','2011-05-18 17:04:03','',0,'Dhr6S0mHvFMMkivznIEEdw==',0,0,1,'2011-05-18 17:04:03','2011-05-18 17:04:03',0,'2011-05-18 17:04:03','2011-05-18 17:04:03','2011-05-18 17:04:03','2011-05-24 15:30:10');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;


--
-- Definition of table `usersingroups`
--

DROP TABLE IF EXISTS `usersingroups`;
CREATE TABLE `usersingroups` (
  `UserName` varchar(255) NOT NULL,
  `GroupName` varchar(45) NOT NULL,
  `index` int(10) unsigned NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`index`)
) ENGINE=InnoDB AUTO_INCREMENT=43 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `usersingroups`
--

/*!40000 ALTER TABLE `usersingroups` DISABLE KEYS */;
INSERT INTO `usersingroups` (`UserName`,`GroupName`,`index`) VALUES 
 ('psadmin@problemsolutions.net','NPS',2),
 ('psadmin@problemsolutions.net','ADL',6);
/*!40000 ALTER TABLE `usersingroups` ENABLE KEYS */;


--
-- Definition of table `usersinroles`
--

DROP TABLE IF EXISTS `usersinroles`;
CREATE TABLE `usersinroles` (
  `Username` varchar(255) NOT NULL DEFAULT '',
  `Rolename` varchar(255) NOT NULL DEFAULT '',
  `ApplicationName` varchar(255) NOT NULL DEFAULT '',
  PRIMARY KEY (`Username`,`Rolename`,`ApplicationName`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `usersinroles`
--

/*!40000 ALTER TABLE `usersinroles` DISABLE KEYS */;
INSERT INTO `usersinroles` (`Username`,`Rolename`,`ApplicationName`) VALUES 
 ('psAdmin@problemsolutions.net','Administrators','PS');
/*!40000 ALTER TABLE `usersinroles` ENABLE KEYS */;


--
-- Definition of function `CheckPermission`
--

DROP FUNCTION IF EXISTS `CheckPermission`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` FUNCTION `CheckPermission`(inpid varchar(255), inusername varchar(255)) RETURNS int(11)
if ((select COUNT(*) from `contentobjects` where `pid` = inpid) ) > 0 and ((select COUNT(*) from `users` where `username` = inusername) ) > 0 then
return (Select max(`PermissionLevel`) from `pidingroup` where `PID` = inpid AND `GroupName` in ( select `GroupName` from `usersingroups` where `UserName` = inusername));
else
return 0;
end if $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `AddMissingTexture`
--

DROP PROCEDURE IF EXISTS `AddMissingTexture`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddMissingTexture`(newfilename varchar(45),
newtype varchar(45), newuvset int(10), newcontentobjectid varchar(400), newrevision int(10))
BEGIN
      INSERT INTO `missingtextures`(Filename,
      Type,UVSet,PID,Revision)
      values(newfilename,newtype,newuvset,newcontentobjectid,newrevision);
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `AddSupportingFile`
--

DROP PROCEDURE IF EXISTS `AddSupportingFile`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddSupportingFile`(newfilename varchar(45),
newdescription varchar(400),newcontentobjectid varchar(400))
BEGIN
      INSERT INTO `supportingfiles`(Filename,
      Description,PID)
      values(newfilename,newdescription,newcontentobjectid);
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `AddTextureReference`
--

DROP PROCEDURE IF EXISTS `AddTextureReference`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddTextureReference`(newfilename varchar(45),
newtype varchar(45), newuvset int(10), newcontentobjectid varchar(400), newrevision int(10))
BEGIN
      INSERT INTO `texturereferences`(Filename,
      Type,UVSet,PID,Revision)
      values(newfilename,newtype,newuvset,newcontentobjectid,newrevision);
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `AddToCurrentUploads`
--

DROP PROCEDURE IF EXISTS `AddToCurrentUploads`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddToCurrentUploads`(targetpid varchar(400), targethash varchar (100))
BEGIN
  INSERT INTO `current_uploads` (`pid`, `hash`)
  VALUES (targetpid, targethash);
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `AddUserToGroup`
--

DROP PROCEDURE IF EXISTS `AddUserToGroup`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddUserToGroup`(inusername varchar(255), ingroupname varchar(255))
if( select count(*) from `users` where `email`=inusername > 0)
    and (select count(*) from `usergroups` where `groupname` = ingroupname > 0)
    and (select count(*) from `usersingroups` where (`username` = inusername and `groupname`=ingroupname) = 0) then
        insert into `usersingroups`(username,groupname) values(inusername,ingroupname);
    end if $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `AssociateKeyword`
--

DROP PROCEDURE IF EXISTS `AssociateKeyword`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `AssociateKeyword`(coid int(10), kid int(10))
BEGIN
                 INSERT INTO `associatedkeywords`(`ContentObjectId`,`KeywordId`)
                 VALUES (coid,kid);
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `CheckGroupPermission`
--

DROP PROCEDURE IF EXISTS `CheckGroupPermission`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `CheckGroupPermission`(ingroupname varchar(255), inpid varchar(255))
BEGIN

          Select `PermissionLevel` from `pidingroup` where `Groupname`=groupname and `pid`=inpid;
          END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `CreateUserGroup`
--

DROP PROCEDURE IF EXISTS `CreateUserGroup`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `CreateUserGroup`(ingroupname varchar(255), inowner varchar(255), indescription varchar(1000),inlevel integer)
BEGIN
       insert into `usergroups` values(ingroupname,inowner,indescription,inlevel);
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `DeleteContentObject`
--

DROP PROCEDURE IF EXISTS `DeleteContentObject`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `DeleteContentObject`(inpid varchar(400))
BEGIN
        DELETE
        FROM `contentobjects`
        WHERE PID = inpid;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `DeleteMissingTexture`
--

DROP PROCEDURE IF EXISTS `DeleteMissingTexture`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `DeleteMissingTexture`(inpid varchar(400),infilename varchar(400), inrevision int(10))
BEGIN
        DELETE
        FROM `missingtextures`
        WHERE PID = inpid AND Filename = infilename AND Revision = inrevision;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `DeleteSupportingFile`
--

DROP PROCEDURE IF EXISTS `DeleteSupportingFile`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `DeleteSupportingFile`(inpid varchar(400),infilename varchar(400))
BEGIN
        DELETE
        FROM `supportingfiles`
        WHERE PID = inpid AND Filename = infilename;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `DeleteTextureReference`
--

DROP PROCEDURE IF EXISTS `DeleteTextureReference`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `DeleteTextureReference`(inpid varchar(400),infilename varchar(400), inrevision int(10))
BEGIN
        DELETE
        FROM `texturereferences`
        WHERE PID = inpid AND Filename = infilename AND Revison = inrevision;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `DeleteUserGroup`
--

DROP PROCEDURE IF EXISTS `DeleteUserGroup`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `DeleteUserGroup`(ingroupname varchar(255))
BEGIN
   delete from `usergroups` where `groupname` = ingroupname;
   delete from `usersingroups` where `groupname` = ingroupname;
   delete from `pidingroup` where `groupname`=ingroupname;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `GetAllContentObjects`
--

DROP PROCEDURE IF EXISTS `GetAllContentObjects`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetAllContentObjects`()
BEGIN
  SELECT *
  FROM `contentobjects`;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `GetContentObject`
--

DROP PROCEDURE IF EXISTS `GetContentObject`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetContentObject`(targetpid varchar(400))
BEGIN
  SELECT *
  FROM `contentobjects`
  WHERE pid = targetpid;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `GetGroupMembers`
--

DROP PROCEDURE IF EXISTS `GetGroupMembers`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetGroupMembers`(ingroupname varchar(255))
BEGIN
  select `username` from `usersingroups` where `groupname`=ingroupname;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `GetHighestRated`
--

DROP PROCEDURE IF EXISTS `GetHighestRated`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetHighestRated`(s integer, length integer)
BEGIN
SET @lmt = length;
SET @s = s;
PREPARE STMT FROM "SELECT PID, Title, ScreenShotFileName,ScreenShotFileId, Description
FROM ContentObjects
LEFT JOIN Reviews
ON ContentObjects.PID = Reviews.ContentObjectId
GROUP BY ContentObjects.PID
ORDER BY AVG(Reviews.Rating) DESC
LIMIT ?,?";
EXECUTE STMT USING @s, @lmt;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `GetKeywords`
--

DROP PROCEDURE IF EXISTS `GetKeywords`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetKeywords`(targetPid varchar(400))
BEGIN
SELECT Keyword
FROM ContentObjects INNER JOIN AssociatedKeywords
ON ContentObjects.Id = AssociatedKeywords.ContentObjectId
INNER JOIN Keywords ON AssociatedKeywords.KeywordId = Keywords.Id
WHERE PID = targetPid;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `GetMissingTextures`
--

DROP PROCEDURE IF EXISTS `GetMissingTextures`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetMissingTextures`(inpid varchar(400), inrevision int(10))
BEGIN
        SELECT *
        FROM `missingtextures`
        WHERE PID = inpid AND Revision = inrevision;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `GetModelOwner`
--

DROP PROCEDURE IF EXISTS `GetModelOwner`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetModelOwner`(inpid varchar(255))
BEGIN
     Select `submitter` from `contentobjects` where pid = inpid;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `GetModelsInGroup`
--

DROP PROCEDURE IF EXISTS `GetModelsInGroup`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetModelsInGroup`(ingroupname varchar(255))
BEGIN
     Select `pid` from `pidingroup` where `groupname` = ingroupname and `PermissionLevel` > 0;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `GetMostPopularContentObjects`
--

DROP PROCEDURE IF EXISTS `GetMostPopularContentObjects`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetMostPopularContentObjects`()
BEGIN
    SELECT PID, Title, ScreenShotFileName,ScreenShotFileId
    FROM ContentObjects
    ORDER BY Views;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `GetMostRecentlyUpdated`
--

DROP PROCEDURE IF EXISTS `GetMostRecentlyUpdated`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetMostRecentlyUpdated`(s integer, length integer)
BEGIN
    SET @lmt = length;
    set @s = s;
    PREPARE STMT FROM "SELECT PID, Title, ScreenShotFileName,ScreenShotFileId, Description
    FROM ContentObjects
    ORDER BY LastModified DESC LIMIT ?,?";
    EXECUTE STMT USING @s, @lmt;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `GetMostRecentlyViewed`
--

DROP PROCEDURE IF EXISTS `GetMostRecentlyViewed`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetMostRecentlyViewed`(s integer, length integer)
BEGIN
    SET @s = s;
    set @lmt = length;
    PREPARE STMT FROM "SELECT PID, Title, ScreenShotFileName,ScreenShotFileId, Description
    FROM ContentObjects
    ORDER BY LastViewed DESC
    LIMIT ?,?";
    EXECUTE STMT USING @s, @lmt;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `GetReviews`
--

DROP PROCEDURE IF EXISTS `GetReviews`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetReviews`(pid varchar(400))
BEGIN
        SELECT *
        FROM `reviews`
        WHERE ContentObjectId = pid;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `GetSupportingFiles`
--

DROP PROCEDURE IF EXISTS `GetSupportingFiles`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetSupportingFiles`(inpid varchar(400))
BEGIN
        SELECT *
        FROM `supportingfiles`
        WHERE pid = inpid;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `GetTextureReferences`
--

DROP PROCEDURE IF EXISTS `GetTextureReferences`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetTextureReferences`(inpid varchar(400), inrevision int(10))
BEGIN
        SELECT *
        FROM `texturereferences`
        WHERE PID = inpid AND Revision = inrevision;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `GetUserGroup`
--

DROP PROCEDURE IF EXISTS `GetUserGroup`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetUserGroup`(ingroupname varchar(255))
BEGIN
        Select * from `UserGroups` where `groupname`=ingroupname;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `GetUserMembership`
--

DROP PROCEDURE IF EXISTS `GetUserMembership`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetUserMembership`(inusername varchar(255))
BEGIN
        select `groupname` from `usersingroups` where `username` = inusername;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `IncrementDownloads`
--

DROP PROCEDURE IF EXISTS `IncrementDownloads`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `IncrementDownloads`(targetpid varchar(400))
BEGIN
        UPDATE ContentObjects SET Downloads = Downloads+1
        WHERE PID =targetpid;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `IncrementViews`
--

DROP PROCEDURE IF EXISTS `IncrementViews`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `IncrementViews`(targetpid varchar(400))
BEGIN
        UPDATE ContentObjects SET Views = Views+1, LastViewed=NOW()
        WHERE PID =targetpid;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `InsertContentObject`
--

DROP PROCEDURE IF EXISTS `InsertContentObject`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `InsertContentObject`(newpid nvarchar(400),
newtitle nvarchar(400),
newcontentfilename nvarchar(400),
newcontentfileid nvarchar(400),
newsubmitter nvarchar(400),
newcreativecommonslicenseurl nvarchar(400),
newdescription nvarchar(400),
newscreenshotfilename nvarchar(400),
newscreenshotfileid nvarchar(400),
newsponsorlogofilename nvarchar(400),
newsponsorlogofileid nvarchar(400),
newdeveloperlogofilename nvarchar(400),
newdeveloperlogofileid nvarchar(400),
newassettype nvarchar(400),
newdisplayfilename nvarchar(400),
newdisplayfileid nvarchar(400),
newmoreinfourl nvarchar(400),
newdevelopername nvarchar(400),
newsponsorname nvarchar(400),
newartistname nvarchar(400),
newunitscale nvarchar(400),
newupaxis nvarchar(400),
newuvcoordinatechannel nvarchar(400),
newintentionoftexture nvarchar(400),
newformat nvarchar(400),
newnumpolys int(10),
newNumTextures int(10),
newRevisionNumber int(10),
newRequireResubmit TINYINT(1),
newenabled tinyint(1),
newready tinyint(1),
newOriginalFileName nvarchar(400),
newOriginalFileId nvarchar(400))
BEGIN
INSERT INTO `ContentObjects` (pid,
title,
contentfilename,
contentfileid,
submitter,
creativecommonslicenseurl,
description,
screenshotfilename,
screenshotfileid,
sponsorlogofilename,
sponsorlogofileid,
developerlogofilename,
developerlogofileid,
assettype,
displayfilename,
displayfileid,
moreinfourl,
developername,
sponsorname,
artistname,
unitscale,
upaxis,
uvcoordinatechannel,
intentionoftexture,
format, numpolygons,numtextures,revision, requiressubmit, enabled, uploadcomplete,OriginalFileName,OriginalFileId)
values (newpid,
newtitle,
newcontentfilename,
newcontentfileid,
newsubmitter,
newcreativecommonslicenseurl,
newdescription,
newscreenshotfilename,
screenshotfileid,
newsponsorlogofilename,
newsponsorlogofileid,
newdeveloperlogofilename,
newdeveloperlogofileid,
newassettype,
newdisplayfilename,
newdisplayfileid,
newmoreinfourl,
newdevelopername,
newsponsorname,
newartistname,
newunitscale,
newupaxis,
newuvcoordinatechannel,
newintentionoftexture,
newformat,
newnumpolys,newNumTextures,newRevisionNumber,
newRequireResubmit,
newenabled,
newready,newOriginalFileName,newOriginalFileId);
SELECT LAST_INSERT_ID();
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `InsertKeyword`
--

DROP PROCEDURE IF EXISTS `InsertKeyword`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `InsertKeyword`(newKeyword varchar(45))
BEGIN
        INSERT INTO keywords(keyword) VALUES(newKeyword);
        SELECT LAST_INSERT_ID();
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `InsertReview`
--

DROP PROCEDURE IF EXISTS `InsertReview`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `InsertReview`(newrating int(10),
newtext varchar(45),newsubmittedby varchar(45),newcontentobjectid varchar(400))
BEGIN
      INSERT INTO `reviews`(rating,
      text,submittedby,contentobjectid,SubmittedDate)
      values(newrating,newtext,newsubmittedby,newcontentobjectid, NOW());
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `OpenId_DeleteUserOpenIdLink`
--

DROP PROCEDURE IF EXISTS `OpenId_DeleteUserOpenIdLink`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `OpenId_DeleteUserOpenIdLink`(
openId_Url nvarchar(256),
userId varchar(256))
delete from test2.OpenId where (test2.openId_url=openId_Url)
or (test2.user_id=userId) $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `OpenId_GetOpenIdsByUserId`
--

DROP PROCEDURE IF EXISTS `OpenId_GetOpenIdsByUserId`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `OpenId_GetOpenIdsByUserId`(userId varchar(256))
select openId_url from openid where (user_id=userId) $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `OpenId_GetUserIdByOpenld`
--

DROP PROCEDURE IF EXISTS `OpenId_GetUserIdByOpenld`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `OpenId_GetUserIdByOpenld`(openIdurl varchar(256))
select user_id from test2.openid where (openId_url=openIdurl) $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `OpenId_LinkUserWithOpenId`
--

DROP PROCEDURE IF EXISTS `OpenId_LinkUserWithOpenId`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `OpenId_LinkUserWithOpenId`(
openId_Url nvarchar(256),
userId varchar(256))
insert into test2.OpenId (openId_url,user_id) values(openId_Url, userId) $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `OpenId_Membership_GetAllUsers`
--

DROP PROCEDURE IF EXISTS `OpenId_Membership_GetAllUsers`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `OpenId_Membership_GetAllUsers`(
    ApplicationName       nvarchar(256))
SELECT u.UserName,o.openId_url, u.Email, u.PasswordQuestion, u.Comment, u.IsApproved,
            u.CreationDate,
            u.LastLoginDate,
            u.LastActivityDate,
            u.LastPasswordChangedDate,
            u.PKID, u.IsLockedOut,
            u.LastLockedOutDate
    FROM   test2.Users u

	inner join test2.openid o on o.User_id=u.PKID

	WHERE  u.ApplicationName = ApplicationName

    ORDER BY u.UserName $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `RemoveFromCurrentUploads`
--

DROP PROCEDURE IF EXISTS `RemoveFromCurrentUploads`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `RemoveFromCurrentUploads`(targetpid varchar(400))
BEGIN
  DELETE FROM `current_uploads` 
  WHERE pid = targetpid;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `RemoveUserFromGroup`
--

DROP PROCEDURE IF EXISTS `RemoveUserFromGroup`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `RemoveUserFromGroup`(inusername varchar(255), ingroupname varchar(255))
if( select count(*) from `users` where `email`=inusername > 0)
    and (select count(*) from `usergroups` where `groupname` = ingroupname > 0)
    and (select count(*) from `usersingroups` where (`username` = inusername and `groupname`=ingroupname) = 0) then
        delete from `usersingroups` where `username` = inusername and `groupname`=ingroupname;
    end if $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `SetGroupPolicy`
--

DROP PROCEDURE IF EXISTS `SetGroupPolicy`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `SetGroupPolicy`(ingroupname varchar(255), plevel integer)
BEGIN
update `usergroups`  set PermissionLevel=plevel where `GroupName` = ingroupname;
          END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `SetPermission`
--

DROP PROCEDURE IF EXISTS `SetPermission`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `SetPermission`(inpid varchar(255), inusername varchar(255), plevel integer)
if (select count(`PermissionLevel`) from `pidingroup`   where `PID` = inpid AND `GroupName` = inusername) >0

then

        update `pidingroup`  set PermissionLevel=plevel where `PID` = inpid AND `GroupName` = inusername;

elseif (select count(`pid`) from `contentobjects` where `pid` = inpid) > 0 AND (select count(`GroupName`) from `usergroups` where `GroupName` = inusername) > 0
     then
            insert into `pidingroup` (pid,groupname,permissionlevel) values(inpid,inusername,plevel);


end if $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `StrongEye_OpenID_Membership_GetAllUsers`
--

DROP PROCEDURE IF EXISTS `StrongEye_OpenID_Membership_GetAllUsers`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `StrongEye_OpenID_Membership_GetAllUsers`(
    ApplicationName       nvarchar(256))
SELECT u.UserName, m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
            m.CreateDate,
            m.LastLoginDate,
            u.LastActivityDate,
            m.LastPasswordChangedDate,
            u.UserId, m.IsLockedOut,
            m.LastLockoutDate
    FROM   tes2.aspnet_Membership m, test2.aspnet_Users u
    WHERE  u.ApplicationId = ApplicationId AND u.UserId = m.UserId
    ORDER BY u.UserName $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `StrongEye_OpenID_Membership_GetUserByName`
--

DROP PROCEDURE IF EXISTS `StrongEye_OpenID_Membership_GetUserByName`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `StrongEye_OpenID_Membership_GetUserByName`(
    ApplicationName      nvarchar(256),
    UserName             nvarchar(256),
    CurrentTimeUtc       datetime,
    UpdateLastActivity   bit)
SELECT u.Username, m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
                m.CreateDate, m.LastLoginDate, u.LastActivityDate, m.LastPasswordChangedDate,
                u.UserId, m.IsLockedOut,m.LastLockoutDate
        FROM    test2.aspnet_Applications a, test2.aspnet_Users u, test2.aspnet_Membership m
        WHERE    LOWER(ApplicationName) = a.LoweredApplicationName AND
                u.ApplicationId = a.ApplicationId    AND
                LOWER(UserName) = u.LoweredUserName AND u.UserId = m.UserId
        LIMIT 1 $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `StrongEye_OpenID_Membership_GetUserByUserId`
--

DROP PROCEDURE IF EXISTS `StrongEye_OpenID_Membership_GetUserByUserId`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `StrongEye_OpenID_Membership_GetUserByUserId`(UserId varchar(256),CurrentTimeUtc datetime,UpdateLastActivity   bit)
SELECT u.Username, m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
                m.CreateDate, m.LastLoginDate, @CurrentTimeUtc, m.LastPasswordChangedDate,
                u.UserId, m.IsLockedOut,m.LastLockoutDate
    FROM    dbo.aspnet_Users u, dbo.aspnet_Membership m
    WHERE   @UserId = u.UserId AND u.UserId = m.UserId $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `UpdateContentObject`
--

DROP PROCEDURE IF EXISTS `UpdateContentObject`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `UpdateContentObject`(newpid nvarchar(400),
newtitle nvarchar(400),
newcontentfilename nvarchar(400),
newcontentfileid nvarchar(400),
newsubmitter nvarchar(400),
newcreativecommonslicenseurl nvarchar(400),
newdescription nvarchar(400),
newscreenshotfilename nvarchar(400),
newscreenshotfileid nvarchar(400),
newsponsorlogofilename nvarchar(400),
newsponsorlogofileid nvarchar(400),
newdeveloperlogofilename nvarchar(400),
newdeveloperlogofileid nvarchar(400),
newassettype nvarchar(400),
newdisplayfilename nvarchar(400),
newdisplayfileid nvarchar(400),
newmoreinfourl nvarchar(400),
newdevelopername nvarchar(400),
newsponsorname nvarchar(400),
newartistname nvarchar(400),
newunitscale nvarchar(400),
newupaxis nvarchar(400),
newuvcoordinatechannel nvarchar(400),
newintentionoftexture nvarchar(400),
newformat nvarchar(400),
newnumpolys int(10),
newNumTextures int(10),
newRevisionNumber int(10),
newRequireResubmit TINYINT(1),
newenabled tinyint(1),
newready tinyint(1),
newOriginalFileName nvarchar(400),
newOriginalFileId nvarchar(400))
BEGIN
UPDATE `ContentObjects`
SET title = newtitle,
contentfilename = newcontentfilename,
contentfileid = newcontentfileid,
submitter = newsubmitter,
creativecommonslicenseurl = newcreativecommonslicenseurl,
description = newdescription,
screenshotfilename = newscreenshotfilename,
screenshotfileid = newscreenshotfileid,
sponsorlogofilename = newsponsorlogofilename,
sponsorlogofileid = newsponsorlogofileid,
developerlogofilename = newdeveloperlogofilename,
developerlogofileid = newdeveloperlogofileid,
assettype = newassettype,
displayfilename = newdisplayfilename,
displayfileid = newdisplayfileid,
moreinfourl = newmoreinfourl,
developername = newdevelopername,
sponsorname = newsponsorname,
artistname = newartistname,
unitscale = newunitscale,
upaxis = newupaxis,
uvcoordinatechannel = newuvcoordinatechannel,
intentionoftexture = newintentionoftexture,
revision = newRevisionNumber,
LastModified = NOW(),
format = newformat,
numpolygons = newnumpolys,
numtextures = newNumTextures,
enabled = newenabled,
uploadcomplete = newready,
requiressubmit = newRequireResubmit,
OriginalFileName = newOriginalFileName,
OriginalFileId = newOriginalFileId
WHERE pid=newpid AND revision = newRevisionNumber;
SELECT ID
FROM ContentObjects
WHERE pid = newpid;
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
