DROP TABLE IF EXISTS `test`.`reviews`;
DROP TABLE IF EXISTS `test`.`associatedkeywords`;
DROP TABLE IF EXISTS `test`.`contentobjects`;
DROP TABLE IF EXISTS `test`.`keywords`;
CREATE TABLE  `test`.`contentobjects` (
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
  PRIMARY KEY (`ID`),
  KEY `FK_contentobjects_1` (`Submitter`)
) ENGINE=InnoDB AUTO_INCREMENT=33 DEFAULT CHARSET=latin1;

CREATE TABLE  `test`.`keywords` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Keyword` varchar(45) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE  `test`.`associatedkeywords` (
  `ContentObjectId` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `KeywordId` int(10) unsigned NOT NULL,
  KEY `FK_AssociatedKeywords_1` (`ContentObjectId`),
  KEY `FK_associatedkeywords_2` (`KeywordId`),
  CONSTRAINT `FK_AssociatedKeywords_1` FOREIGN KEY (`ContentObjectId`) REFERENCES `contentobjects` (`ID`),
  CONSTRAINT `FK_associatedkeywords_2` FOREIGN KEY (`KeywordId`) REFERENCES `keywords` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE  `test`.`reviews` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Rating` int(10) unsigned NOT NULL,
  `Text` varchar(45) NOT NULL,
  `SubmittedBy` varchar(45) NOT NULL,
  `SubmittedDate` datetime NOT NULL,
  `ContentObjectId` varchar(400) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `FK_Reviews_1` (`ContentObjectId`),
  KEY `FK_reviews_2` (`SubmittedBy`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=latin1;
DELIMITER $$

DROP PROCEDURE IF EXISTS `test`.`GetAllContentObjects`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `test`.`GetAllContentObjects`()
BEGIN
  SELECT *
  FROM `contentobjects`;
END $$

DELIMITER ;
DELIMITER $$

DROP PROCEDURE IF EXISTS `test`.`GetContentObject`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `test`.`GetContentObject`(targetpid varchar(400))
BEGIN
  SELECT *
  FROM `contentobjects`
  WHERE pid = targetpid;
END $$

DELIMITER ;
DELIMITER $$

DROP PROCEDURE IF EXISTS `test`.`GetHighestRated`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `test`.`GetHighestRated`(s integer, length integer)
BEGIN
SET @lmt = length;
SET @s = s;
PREPARE STMT FROM "SELECT PID, Title, ScreenShotFileName,ScreenShotFileId
FROM ContentObjects
LEFT JOIN Reviews
ON ContentObjects.PID = Reviews.ContentObjectId
GROUP BY ContentObjects.PID
ORDER BY AVG(Reviews.Rating) DESC
LIMIT ?,?";
EXECUTE STMT USING @s, @lmt;
END $$

DELIMITER ;

DELIMITER $$

DROP PROCEDURE IF EXISTS `test`.`GetMostPopularContentObjects`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `test`.`GetMostPopularContentObjects`()
BEGIN
    SELECT PID, Title, ScreenShotFileName,ScreenShotFileId
    FROM ContentObjects
    ORDER BY Views;
END $$

DELIMITER ;

DELIMITER $$

DROP PROCEDURE IF EXISTS `test`.`GetMostRecentlyUpdated`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `test`.`GetMostRecentlyUpdated`(s integer, length integer)
BEGIN
    SET @lmt = length;
    set @s = s;
    PREPARE STMT FROM "SELECT PID, Title, ScreenShotFileName,ScreenShotFileId
    FROM ContentObjects
    ORDER BY LastModified DESC LIMIT ?,?";
    EXECUTE STMT USING @s, @lmt;
END $$

DELIMITER ;

DELIMITER $$

DROP PROCEDURE IF EXISTS `test`.`GetMostRecentlyViewed`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `test`.`GetMostRecentlyViewed`(s integer, length integer)
BEGIN
    SET @s = s;
    set @lmt = length;
    PREPARE STMT FROM "SELECT PID, Title, ScreenShotFileName,ScreenShotFileId
    FROM ContentObjects
    ORDER BY LastViewed DESC
    LIMIT ?,?";
    EXECUTE STMT USING @s, @lmt;
END $$

DELIMITER ;

DELIMITER $$

DROP PROCEDURE IF EXISTS `test`.`GetReviews`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `test`.`GetReviews`(pid varchar(400))
BEGIN
        SELECT *
        FROM `test`.`reviews`
        WHERE ContentObjectId = pid;
END $$

DELIMITER ;
DELIMITER $$

DROP PROCEDURE IF EXISTS `test`.`IncrementViews`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `test`.`IncrementViews`(targetpid varchar(400))
BEGIN
        UPDATE ContentObjects SET Views = Views+1, LastViewed=NOW()
        WHERE PID =targetpid;
END $$

DELIMITER ;

DELIMITER $$

DELIMITER $$

DROP PROCEDURE IF EXISTS `test`.`InsertContentObject`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `test`.`InsertContentObject`(newpid nvarchar(400),
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
newNumTextures int(10))
BEGIN
INSERT INTO `test`.`ContentObjects` (pid,
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
format, numpolygons,numtextures)
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
newnumpolys,newNumTextures);
SELECT LAST_INSERT_ID();
END $$

DELIMITER ;

DELIMITER $$

DROP PROCEDURE IF EXISTS `test`.`InsertReview`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `test`.`InsertReview`(newrating int(10),
newtext varchar(45),newsubmittedby varchar(45),newcontentobjectid varchar(400))
BEGIN
      INSERT INTO `test`.`reviews`(rating,
      text,submittedby,contentobjectid,SubmittedDate)
      values(newrating,newtext,newsubmittedby,newcontentobjectid, NOW());
END $$

DELIMITER ;

DELIMITER $$

DELIMITER $$

DROP PROCEDURE IF EXISTS `test`.`UpdateContentObject`$$
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
newnumpolys int(10), newNumTextures int(10))
BEGIN
UPDATE `test`.`ContentObjects`
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
LastModified = NOW(),
format = newformat,
numpolygons = newnumpolys,
numtextures = newNumTextures
WHERE pid=newpid;
SELECT ID
FROM ContentObjects
WHERE pid = newpid;
END $$

DELIMITER ;
DELIMITER $$

DROP PROCEDURE IF EXISTS `test`.`IncrementDownloads`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `test`.`IncrementDownloads`(targetpid varchar(400))
BEGIN
        UPDATE ContentObjects SET Downloads = Downloads+1
        WHERE PID =targetpid;
END $$

DELIMITER ;
DELIMITER $$

DROP PROCEDURE IF EXISTS `test`.`GetKeywords`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `test`.`GetKeywords`(targetPid varchar(400))
BEGIN
SELECT Keyword
FROM ContentObjects INNER JOIN AssociatedKeywords
ON ContentObjects.Id = AssociatedKeywords.ContentObjectId
INNER JOIN Keywords ON AssociatedKeywords.KeywordId = Keywords.Id
WHERE PID = targetPid;
END $$

DELIMITER ;
DROP TABLE IF EXISTS `test`.`associatedkeywords`;
CREATE TABLE  `test`.`associatedkeywords` (
  `ContentObjectId` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `KeywordId` int(10) unsigned NOT NULL,
  KEY `FK_AssociatedKeywords_1` (`ContentObjectId`),
  KEY `FK_associatedkeywords_2` (`KeywordId`),
  CONSTRAINT `FK_AssociatedKeywords_1` FOREIGN KEY (`ContentObjectId`) REFERENCES `contentobjects` (`ID`),
  CONSTRAINT `FK_associatedkeywords_2` FOREIGN KEY (`KeywordId`) REFERENCES `keywords` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=51 DEFAULT CHARSET=latin1;
DELIMITER $$

DROP PROCEDURE IF EXISTS `test`.`InsertKeyword`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `test`.`InsertKeyword`(newKeyword varchar(45))
BEGIN
        INSERT INTO keywords(keyword) VALUES(newKeyword);
        SELECT LAST_INSERT_ID();
END $$

DELIMITER ;