
      /*
      * Script created by Quest Schema Compare at 5/23/2011 8:59:11 AM.
      * Please back up your database before running this script.
      *
      * Synchronizing objects from test2 to update_test.
      */
USE `update_test`;

ALTER TABLE `associatedkeywords` DROP FOREIGN KEY `FK_AssociatedKeywords_1`;

ALTER TABLE `associatedkeywords` DROP FOREIGN KEY `FK_associatedkeywords_2`;

ALTER TABLE `userprofiles` DROP FOREIGN KEY `UserProfiles_UserGuid_fkey`;

/* Header line. Object: associatedkeywords. Script date: 5/23/2011 8:59:11 AM. */
DROP TABLE IF EXISTS `_temp_associatedkeywords`;

CREATE TABLE `_temp_associatedkeywords` (
	`ContentObjectId` int(10) unsigned NOT NULL auto_increment,
	`KeywordId` int(10) unsigned NOT NULL,
	KEY `FK_AssociatedKeywords_1` ( `ContentObjectId` ),
	KEY `FK_associatedkeywords_2` ( `KeywordId` )
)
ENGINE = InnoDB
CHARACTER SET = latin1
AUTO_INCREMENT = 1
ROW_FORMAT = Compact
;

INSERT INTO `_temp_associatedkeywords`
( `ContentObjectId`, `KeywordId` )
SELECT
`ContentObjectId`, `KeywordId`
FROM `associatedkeywords`;

DROP TABLE `associatedkeywords`;

ALTER TABLE `_temp_associatedkeywords` RENAME `associatedkeywords`;

/* Header line. Object: contentobjects. Script date: 5/23/2011 8:59:11 AM. */
DROP TABLE IF EXISTS `_temp_contentobjects`;

CREATE TABLE `_temp_contentobjects` (
	`ID` int(10) unsigned NOT NULL auto_increment,
	`Description` varchar(400) NOT NULL default ' ',
	`Title` varchar(400) NOT NULL default ' ',
	`ContentFileName` varchar(400) NOT NULL default ' ',
	`ContentFileId` varchar(400) NOT NULL default ' ',
	`ScreenShotFileName` varchar(400) NOT NULL default ' ',
	`ScreenShotFileId` varchar(400) NOT NULL default ' ',
	`Submitter` varchar(400) NOT NULL default ' ',
	`SponsorLogoFileName` varchar(400) NOT NULL default ' ',
	`SponsorLogoFileId` varchar(400) NOT NULL default ' ',
	`DeveloperLogoFileName` varchar(400) NOT NULL default ' ',
	`DeveloperLogoFileId` varchar(400) NOT NULL default ' ',
	`AssetType` varchar(400) NOT NULL default ' ',
	`DisplayFileName` varchar(400) NOT NULL default ' ',
	`DisplayFileId` varchar(400) NOT NULL default ' ',
	`MoreInfoUrl` varchar(400) NOT NULL default ' ',
	`DeveloperName` varchar(400) NOT NULL default ' ',
	`SponsorName` varchar(400) NOT NULL default ' ',
	`ArtistName` varchar(400) NOT NULL default ' ',
	`CreativeCommonsLicenseUrl` varchar(400) NOT NULL default ' ',
	`UnitScale` varchar(400) NOT NULL default ' ',
	`UpAxis` varchar(400) NOT NULL default ' ',
	`UVCoordinateChannel` varchar(400) NOT NULL default ' ',
	`IntentionOfTexture` varchar(400) NOT NULL default ' ',
	`Format` varchar(400) NOT NULL default ' ',
	`Views` int(10) unsigned zerofill NOT NULL default '0000000000',
	`Downloads` int(10) unsigned zerofill NOT NULL default '0000000000',
	`NumPolygons` int(10) unsigned zerofill NOT NULL default '0000000000',
	`NumTextures` int(10) unsigned zerofill NOT NULL default '0000000000',
	`UploadedDate` datetime default '0000-00-00 00:00:00',
	`LastModified` datetime default '0000-00-00 00:00:00',
	`LastViewed` datetime default '0000-00-00 00:00:00',
	`PID` varchar(45) default NULL,
	`Revision` varchar(45) NOT NULL,
	`Enabled` tinyint(1) default NULL,
	`requiressubmit` tinyint(1) default NULL,
	`OriginalFileName` varchar(400) default NULL,
	`OriginalFileId` varchar(400) default NULL,
	`UploadComplete` tinyint(1) default NULL,
	KEY `FK_contentobjects_1` ( `Submitter` ),
	PRIMARY KEY  ( `ID` )
)
ENGINE = InnoDB
CHARACTER SET = latin1
AUTO_INCREMENT = 164
ROW_FORMAT = Compact
;

INSERT INTO `_temp_contentobjects`
( `ArtistName`, `AssetType`, `ContentFileId`, `ContentFileName`, `CreativeCommonsLicenseUrl`, `Description`, `DeveloperLogoFileId`, `DeveloperLogoFileName`, `DeveloperName`, `DisplayFileId`, `DisplayFileName`, `Downloads`, `Enabled`, `Format`, `ID`, `IntentionOfTexture`, `LastModified`, `LastViewed`, `MoreInfoUrl`, `NumPolygons`, `NumTextures`, `OriginalFileId`, `OriginalFileName`, `PID`, `ScreenShotFileId`, `ScreenShotFileName`, `SponsorLogoFileId`, `SponsorLogoFileName`, `SponsorName`, `Submitter`, `Title`, `UnitScale`, `UpAxis`, `UploadComplete`, `UploadedDate`, `UVCoordinateChannel`, `Views` )
SELECT
`ArtistName`, `AssetType`, `ContentFileId`, `ContentFileName`, `CreativeCommonsLicenseUrl`, `Description`, `DeveloperLogoFileId`, `DeveloperLogoFileName`, `DeveloperName`, `DisplayFileId`, `DisplayFileName`, `Downloads`, `Enabled`, `Format`, `ID`, `IntentionOfTexture`, `LastModified`, `LastViewed`, `MoreInfoUrl`, `NumPolygons`, `NumTextures`, `OriginalFileId`, `OriginalFileName`, `PID`, `ScreenShotFileId`, `ScreenShotFileName`, `SponsorLogoFileId`, `SponsorLogoFileName`, `SponsorName`, `Submitter`, `Title`, `UnitScale`, `UpAxis`, `UploadComplete`, `UploadedDate`, `UVCoordinateChannel`, `Views`
FROM `contentobjects`;

DROP TABLE `contentobjects`;

ALTER TABLE `_temp_contentobjects` RENAME `contentobjects`;

/* Header line. Object: keywords. Script date: 5/23/2011 8:59:11 AM. */
DROP TABLE IF EXISTS `_temp_keywords`;

CREATE TABLE `_temp_keywords` (
	`ID` int(10) unsigned NOT NULL auto_increment,
	`Keyword` varchar(45) NOT NULL,
	PRIMARY KEY  ( `ID` )
)
ENGINE = InnoDB
CHARACTER SET = latin1
AUTO_INCREMENT = 1
ROW_FORMAT = Compact
;

INSERT INTO `_temp_keywords`
( `ID`, `Keyword` )
SELECT
`ID`, `Keyword`
FROM `keywords`;

DROP TABLE `keywords`;

ALTER TABLE `_temp_keywords` RENAME `keywords`;

/* Header line. Object: missingtextures. Script date: 5/23/2011 8:59:11 AM. */
CREATE TABLE `missingtextures` (
	`ID` int(10) unsigned NOT NULL auto_increment,
	`Filename` varchar(45) NOT NULL,
	`Type` varchar(45) NOT NULL,
	`UVSet` int(10) unsigned NOT NULL,
	`PID` varchar(45) NOT NULL,
	`Revision` int(10) unsigned NOT NULL,
	PRIMARY KEY  ( `ID` )
)
ENGINE = InnoDB
CHARACTER SET = latin1
AUTO_INCREMENT = 18
ROW_FORMAT = Compact
;

/* Header line. Object: reviews. Script date: 5/23/2011 8:59:11 AM. */
DROP TABLE IF EXISTS `_temp_reviews`;

CREATE TABLE `_temp_reviews` (
	`ID` int(10) unsigned NOT NULL auto_increment,
	`Rating` int(10) unsigned NOT NULL,
	`Text` varchar(45) NOT NULL,
	`SubmittedBy` varchar(45) NOT NULL,
	`SubmittedDate` datetime NOT NULL,
	`ContentObjectId` varchar(400) default NULL,
	KEY `FK_Reviews_1` ( `ContentObjectId` ),
	KEY `FK_reviews_2` ( `SubmittedBy` ),
	PRIMARY KEY  ( `ID` )
)
ENGINE = InnoDB
CHARACTER SET = latin1
AUTO_INCREMENT = 1
ROW_FORMAT = Compact
;

INSERT INTO `_temp_reviews`
( `ContentObjectId`, `ID`, `Rating`, `SubmittedBy`, `SubmittedDate`, `Text` )
SELECT
`ContentObjectId`, `ID`, `Rating`, `SubmittedBy`, `SubmittedDate`, `Text`
FROM `reviews`;

DROP TABLE `reviews`;

ALTER TABLE `_temp_reviews` RENAME `reviews`;

/* Header line. Object: supportingfiles. Script date: 5/23/2011 8:59:11 AM. */
CREATE TABLE `supportingfiles` (
	`ID` int(10) unsigned NOT NULL auto_increment,
	`Filename` varchar(45) NOT NULL,
	`Description` varchar(45) NOT NULL,
	`PID` varchar(45) NOT NULL,
	PRIMARY KEY  ( `ID` )
)
ENGINE = InnoDB
CHARACTER SET = latin1
AUTO_INCREMENT = 16
ROW_FORMAT = Compact
;

/* Header line. Object: text_log. Script date: 5/23/2011 8:59:11 AM. */
CREATE TABLE `text_log` (
	`Log` varchar(255) NOT NULL
)
ENGINE = InnoDB
CHARACTER SET = latin1
ROW_FORMAT = Compact
;

/* Header line. Object: texturereferences. Script date: 5/23/2011 8:59:11 AM. */
CREATE TABLE `texturereferences` (
	`ID` int(10) unsigned NOT NULL auto_increment,
	`Filename` varchar(45) NOT NULL,
	`Type` varchar(45) NOT NULL,
	`UVSet` int(10) unsigned NOT NULL,
	`PID` varchar(45) NOT NULL,
	`Revision` int(10) unsigned NOT NULL,
	PRIMARY KEY  ( `ID` )
)
ENGINE = InnoDB
CHARACTER SET = latin1
AUTO_INCREMENT = 25
ROW_FORMAT = Compact
;

/* Header line. Object: userprofiles. Script date: 5/23/2011 8:59:11 AM. */
DROP TABLE IF EXISTS `_temp_userprofiles`;

CREATE TABLE `_temp_userprofiles` (
	`UserID` bigint(20) unsigned NOT NULL auto_increment,
	`UserGuid` char(36) NOT NULL,
	`FirstName` varchar(255) default NULL,
	`LastName` varchar(255) default NULL,
	`Email` varchar(255) default NULL,
	`WebsiteURL` text default NULL,
	`SponsorName` varchar(255) default NULL,
	`SponsorLogo` mediumblob default NULL,
	`DeveloperName` varchar(255) default NULL,
	`DeveloperLogo` mediumblob default NULL,
	`ArtistName` varchar(255) default NULL,
	`Phone` varchar(50) default NULL,
	`CreatedDate` date default NULL,
	`CreatedBy` varchar(255) default NULL,
	`LastEditedBy` varchar(255) default NULL,
	`Comments` varchar(255) default NULL,
	`Description` varchar(255) default NULL,
	`DeveloperLogoContentType` varchar(255) default NULL,
	`SponsorLogoContentType` varchar(255) default NULL,
	`DeveloperLogoFileName` varchar(255) default NULL,
	`SponsorLogoFileName` varchar(255) default NULL,
	`LastEditedDate` date default NULL,
	`UserName` varchar(255) NOT NULL,
	PRIMARY KEY  ( `UserID` ),
	UNIQUE INDEX `UserID` ( `UserID` ),
	KEY `UserProfiles_UserGuid_fkey` ( `UserGuid` )
)
ENGINE = InnoDB
CHARACTER SET = latin1
AUTO_INCREMENT = 1
ROW_FORMAT = Compact
;

INSERT INTO `_temp_userprofiles`
( `ArtistName`, `Comments`, `CreatedBy`, `CreatedDate`, `Description`, `DeveloperLogo`, `DeveloperLogoContentType`, `DeveloperLogoFileName`, `DeveloperName`, `Email`, `FirstName`, `LastEditedBy`, `LastEditedDate`, `LastName`, `Phone`, `SponsorLogo`, `SponsorLogoContentType`, `SponsorLogoFileName`, `SponsorName`, `UserGuid`, `UserID`, `UserName`, `WebsiteURL` )
SELECT
`ArtistName`, `Comments`, `CreatedBy`, `CreatedDate`, `Description`, `DeveloperLogo`, `DeveloperLogoContentType`, `DeveloperLogoFileName`, `DeveloperName`, `Email`, `FirstName`, `LastEditedBy`, `LastEditedDate`, `LastName`, `Phone`, `SponsorLogo`, `SponsorLogoContentType`, `SponsorLogoFileName`, `SponsorName`, `UserGuid`, `UserID`, `UserName`, `WebsiteURL`
FROM `userprofiles`;

DROP TABLE `userprofiles`;

ALTER TABLE `_temp_userprofiles` RENAME `userprofiles`;

/* Header line. Object: yaf_board. Script date: 5/23/2011 8:59:11 AM. */
CREATE TABLE `yaf_board` (
	`BoardID` int(11) NOT NULL auto_increment,
	`Name` varchar(128) NOT NULL,
	`AllowThreaded` tinyint(1) NOT NULL,
	`MembershipAppName` varchar(255) default NULL,
	`RolesAppName` varchar(255) default NULL,
	PRIMARY KEY  ( `BoardID` )
)
ENGINE = InnoDB
CHARACTER SET = utf8
AUTO_INCREMENT = 1
ROW_FORMAT = Compact
;

/* Header line. Object: yaf_category. Script date: 5/23/2011 8:59:11 AM. */
CREATE TABLE `yaf_category` (
	`CategoryID` int(11) NOT NULL auto_increment,
	`BoardID` int(11) NOT NULL,
	`Name` varchar(128) NOT NULL,
	`CategoryImage` varchar(255) default NULL,
	`SortOrder` smallint(5) NOT NULL,
	UNIQUE INDEX `IX_test2_yaf_Category` ( `BoardID`, `Name` ),
	KEY `IX_test2_yaf_Category_BoardID` ( `BoardID` ),
	KEY `IX_test2_yaf_Category_Name` ( `Name` ),
	PRIMARY KEY  ( `CategoryID` )
)
ENGINE = InnoDB
CHARACTER SET = utf8
AUTO_INCREMENT = 1
ROW_FORMAT = Compact
;

/* Header line. Object: yaf_forum. Script date: 5/23/2011 8:59:11 AM. */
CREATE TABLE `yaf_forum` (
	`ForumID` int(11) NOT NULL auto_increment,
	`CategoryID` int(11) NOT NULL,
	`ParentID` int(11) default NULL,
	`Name` varchar(128) NOT NULL,
	`Description` varchar(255) NOT NULL,
	`SortOrder` smallint(5) NOT NULL,
	`LastPosted` datetime default NULL,
	`LastTopicID` int(11) default NULL,
	`LastMessageID` int(11) default NULL,
	`LastUserID` int(11) default NULL,
	`LastUserName` varchar(128) default NULL,
	`NumTopics` int(11) NOT NULL,
	`NumPosts` int(11) NOT NULL,
	`RemoteURL` varchar(128) default NULL,
	`Flags` int(11) NOT NULL default '0',
	`ThemeURL` varchar(128) default NULL,
	KEY `FK_test2_yaf_Forum_yaf_Message` ( `LastMessageID` ),
	KEY `FK_test2_yaf_Forum_yaf_Topic` ( `LastTopicID` ),
	KEY `FK_test2_yaf_Forum_yaf_User` ( `LastUserID` ),
	UNIQUE INDEX `IX_test2_yaf_Forum` ( `ParentID`, `Name` ),
	KEY `IX_test2_yaf_Forum_CategoryID` ( `CategoryID` ),
	KEY `IX_test2_yaf_Forum_Flags` ( `Flags` ),
	KEY `IX_test2_yaf_Forum_ParentID` ( `ParentID` ),
	PRIMARY KEY  ( `ForumID` )
)
ENGINE = InnoDB
CHARACTER SET = utf8
AUTO_INCREMENT = 1
ROW_FORMAT = Compact
;

/* Header line. Object: yaf_message. Script date: 5/23/2011 8:59:11 AM. */
CREATE TABLE `yaf_message` (
	`MessageID` int(11) NOT NULL auto_increment,
	`TopicID` int(11) NOT NULL,
	`ReplyTo` int(11) default NULL,
	`Position` int(11) NOT NULL,
	`Indent` int(11) NOT NULL,
	`UserID` int(11) NOT NULL,
	`UserName` varchar(128) default NULL,
	`Posted` datetime NOT NULL,
	`Message` longtext NOT NULL,
	`IP` varchar(15) NOT NULL,
	`Edited` datetime default NULL,
	`Flags` int(11) NOT NULL default '23',
	`EditReason` varchar(128) default NULL,
	`IsModeratorChanged` tinyint(1) NOT NULL default '0',
	`DeleteReason` varchar(128) default NULL,
	`BlogPostID` varchar(128) default NULL,
	KEY `FK_test2_yaf_Message_yaf_Message` ( `ReplyTo` ),
	KEY `IX_test2_yaf_Message_Flags` ( `Flags` ),
	KEY `IX_test2_yaf_Message_TopicID` ( `TopicID` ),
	KEY `IX_test2_yaf_Message_UserID` ( `UserID` ),
	PRIMARY KEY  ( `MessageID` )
)
ENGINE = InnoDB
CHARACTER SET = utf8
AUTO_INCREMENT = 1
ROW_FORMAT = Compact
;

/* Header line. Object: yaf_poll. Script date: 5/23/2011 8:59:11 AM. */
CREATE TABLE `yaf_poll` (
	`PollID` int(11) NOT NULL auto_increment,
	`Question` varchar(128) NOT NULL,
	`Closes` datetime default NULL,
	PRIMARY KEY  ( `PollID` )
)
ENGINE = InnoDB
CHARACTER SET = utf8
AUTO_INCREMENT = 1
ROW_FORMAT = Compact
;

/* Header line. Object: yaf_rank. Script date: 5/23/2011 8:59:11 AM. */
CREATE TABLE `yaf_rank` (
	`RankID` int(11) NOT NULL auto_increment,
	`BoardID` int(11) NOT NULL,
	`Name` varchar(128) NOT NULL,
	`MinPosts` int(11) default NULL,
	`RankImage` varchar(128) default NULL,
	`Flags` int(11) NOT NULL default '0',
	UNIQUE INDEX `IX_test2_yaf_Rank` ( `BoardID`, `Name` ),
	PRIMARY KEY  ( `RankID` )
)
ENGINE = InnoDB
CHARACTER SET = utf8
AUTO_INCREMENT = 1
ROW_FORMAT = Compact
;

/* Header line. Object: yaf_topic. Script date: 5/23/2011 8:59:11 AM. */
CREATE TABLE `yaf_topic` (
	`TopicID` int(11) NOT NULL auto_increment,
	`ForumID` int(11) NOT NULL,
	`UserID` int(11) NOT NULL,
	`UserName` varchar(128) default NULL,
	`Posted` datetime NOT NULL,
	`Topic` varchar(128) NOT NULL,
	`Views` int(11) NOT NULL,
	`Priority` smallint(5) NOT NULL,
	`PollID` int(11) default NULL,
	`TopicMovedID` int(11) default NULL,
	`LastPosted` datetime default NULL,
	`LastMessageID` int(11) default NULL,
	`LastUserID` int(11) default NULL,
	`LastUserName` varchar(128) default NULL,
	`NumPosts` int(11) NOT NULL,
	`Flags` int(11) NOT NULL default '0',
	KEY `FK_test2_yaf_Topic_yaf_Message` ( `LastMessageID` ),
	KEY `FK_test2_yaf_Topic_yaf_Poll` ( `PollID` ),
	KEY `FK_test2_yaf_Topic_yaf_Topic` ( `TopicMovedID` ),
	KEY `FK_test2_yaf_Topic_yaf_User2` ( `LastUserID` ),
	KEY `IX_test2_yaf_Topic_Flags` ( `Flags` ),
	KEY `IX_test2_yaf_Topic_ForumID` ( `ForumID` ),
	KEY `IX_test2_yaf_Topic_UserID` ( `UserID` ),
	PRIMARY KEY  ( `TopicID` )
)
ENGINE = InnoDB
CHARACTER SET = utf8
AUTO_INCREMENT = 1
ROW_FORMAT = Compact
;

/* Header line. Object: yaf_user. Script date: 5/23/2011 8:59:11 AM. */
CREATE TABLE `yaf_user` (
	`UserID` int(11) NOT NULL auto_increment,
	`BoardID` int(11) NOT NULL,
	`ProviderUserKey` varchar(64) default NULL,
	`Name` varchar(128) NOT NULL,
	`Password` varchar(32) NOT NULL,
	`Email` varchar(128) default NULL,
	`Joined` datetime NOT NULL,
	`LastVisit` datetime NOT NULL,
	`IP` varchar(15) default NULL,
	`NumPosts` int(11) NOT NULL,
	`TimeZone` int(11) NOT NULL,
	`Avatar` varchar(255) default NULL,
	`Signature` longtext default NULL,
	`AvatarImage` longblob default NULL,
	`AvatarImageType` varchar(50) default NULL,
	`RankID` int(11) NOT NULL,
	`Suspended` datetime default NULL,
	`LanguageFile` varchar(128) default NULL,
	`ThemeFile` varchar(128) default NULL,
	`OverrideDefaultThemes` tinyint(1) NOT NULL default '0',
	`PMNotification` tinyint(1) NOT NULL default '1',
	`Flags` int(11) NOT NULL default '0',
	`Points` int(11) NOT NULL default '0',
	KEY `FK_test2_yaf_User_yaf_Rank` ( `RankID` ),
	UNIQUE INDEX `IX_test2_yaf_User` ( `BoardID`, `Name` ),
	KEY `IX_test2_yaf_User_Flags` ( `Flags` ),
	KEY `IX_test2_yaf_User_Name` ( `Name` ),
	KEY `IX_test2_yaf_User_ProviderUserKey` ( `ProviderUserKey` ),
	PRIMARY KEY  ( `UserID` )
)
ENGINE = InnoDB
CHARACTER SET = utf8
AUTO_INCREMENT = 1
ROW_FORMAT = Compact
;

/* Header line. Object: AddMissingTexture. Script date: 5/23/2011 8:59:11 AM. */
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddMissingTexture`(newfilename varchar(45),
newtype varchar(45), newuvset int(10), newcontentobjectid varchar(400), newrevision int(10))
BEGIN
      INSERT INTO `missingtextures`(Filename,
      Type,UVSet,PID,Revision)
      values(newfilename,newtype,newuvset,newcontentobjectid,newrevision);
END;

/* Header line. Object: AddSupportingFile. Script date: 5/23/2011 8:59:11 AM. */
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddSupportingFile`(newfilename varchar(45),
newdescription varchar(400),newcontentobjectid varchar(400))
BEGIN
      INSERT INTO `supportingfiles`(Filename,
      Description,PID)
      values(newfilename,newdescription,newcontentobjectid);
END;

/* Header line. Object: AddTextureReference. Script date: 5/23/2011 8:59:11 AM. */
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddTextureReference`(newfilename varchar(45),
newtype varchar(45), newuvset int(10), newcontentobjectid varchar(400), newrevision int(10))
BEGIN
      INSERT INTO `texturereferences`(Filename,
      Type,UVSet,PID,Revision)
      values(newfilename,newtype,newuvset,newcontentobjectid,newrevision);
END;

/* Header line. Object: AssociateKeyword. Script date: 5/23/2011 8:59:11 AM. */
DROP PROCEDURE `AssociateKeyword`;

CREATE DEFINER=`root`@`localhost` PROCEDURE `AssociateKeyword`(coid int(10), kid int(10))
BEGIN
                 INSERT INTO `associatedkeywords`(`ContentObjectId`,`KeywordId`)
                 VALUES (coid,kid);
END;

/* Header line. Object: DeleteContentObject. Script date: 5/23/2011 8:59:11 AM. */
DROP PROCEDURE `DeleteContentObject`;

CREATE DEFINER=`root`@`localhost` PROCEDURE `DeleteContentObject`(inpid varchar(400))
BEGIN
        DELETE
        FROM `contentobjects`
        WHERE PID = inpid;
END;

/* Header line. Object: DeleteMissingTexture. Script date: 5/23/2011 8:59:11 AM. */
CREATE DEFINER=`root`@`localhost` PROCEDURE `DeleteMissingTexture`(inpid varchar(400),infilename varchar(400), inrevision int(10))
BEGIN
        DELETE
        FROM `missingtextures`
        WHERE PID = inpid AND Filename = infilename AND Revision = inrevision;
END;

/* Header line. Object: DeleteSupportingFile. Script date: 5/23/2011 8:59:11 AM. */
CREATE DEFINER=`root`@`localhost` PROCEDURE `DeleteSupportingFile`(inpid varchar(400),infilename varchar(400))
BEGIN
        DELETE
        FROM `supportingfiles`
        WHERE PID = inpid AND Filename = infilename;
END;

/* Header line. Object: DeleteTextureReference. Script date: 5/23/2011 8:59:11 AM. */
CREATE DEFINER=`root`@`localhost` PROCEDURE `DeleteTextureReference`(inpid varchar(400),infilename varchar(400), inrevision int(10))
BEGIN
        DELETE
        FROM `texturereferences`
        WHERE PID = inpid AND Filename = infilename AND Revison = inrevision;
END;

/* Header line. Object: GetAllContentObjects. Script date: 5/23/2011 8:59:11 AM. */
DROP PROCEDURE `GetAllContentObjects`;

CREATE DEFINER=`root`@`localhost` PROCEDURE `GetAllContentObjects`()
BEGIN
  SELECT *
  FROM `contentobjects`;
END;

/* Header line. Object: GetHighestRated. Script date: 5/23/2011 8:59:11 AM. */
DROP PROCEDURE `GetHighestRated`;

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
END;

/* Header line. Object: GetMissingTextures. Script date: 5/23/2011 8:59:11 AM. */
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetMissingTextures`(inpid varchar(400), inrevision int(10))
BEGIN
        SELECT *
        FROM `missingtextures`
        WHERE PID = inpid AND Revision = inrevision;
END;

/* Header line. Object: GetMostRecentlyUpdated. Script date: 5/23/2011 8:59:11 AM. */
DROP PROCEDURE `GetMostRecentlyUpdated`;

CREATE DEFINER=`root`@`localhost` PROCEDURE `GetMostRecentlyUpdated`(s integer, length integer)
BEGIN
    SET @lmt = length;
    set @s = s;
    PREPARE STMT FROM "SELECT PID, Title, ScreenShotFileName,ScreenShotFileId, Description
    FROM ContentObjects
    ORDER BY LastModified DESC LIMIT ?,?";
    EXECUTE STMT USING @s, @lmt;
END;

/* Header line. Object: GetMostRecentlyViewed. Script date: 5/23/2011 8:59:11 AM. */
DROP PROCEDURE `GetMostRecentlyViewed`;

CREATE DEFINER=`root`@`localhost` PROCEDURE `GetMostRecentlyViewed`(s integer, length integer)
BEGIN
    SET @s = s;
    set @lmt = length;
    PREPARE STMT FROM "SELECT PID, Title, ScreenShotFileName,ScreenShotFileId, Description
    FROM ContentObjects
    ORDER BY LastViewed DESC
    LIMIT ?,?";
    EXECUTE STMT USING @s, @lmt;
END;

/* Header line. Object: GetReviews. Script date: 5/23/2011 8:59:11 AM. */
DROP PROCEDURE `GetReviews`;

CREATE DEFINER=`root`@`localhost` PROCEDURE `GetReviews`(pid varchar(400))
BEGIN
        SELECT *
        FROM `reviews`
        WHERE ContentObjectId = pid;
END;

/* Header line. Object: GetSupportingFiles. Script date: 5/23/2011 8:59:11 AM. */
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetSupportingFiles`(inpid varchar(400))
BEGIN
        SELECT *
        FROM `supportingfiles`
        WHERE pid = inpid;
END;

/* Header line. Object: GetTextureReferences. Script date: 5/23/2011 8:59:11 AM. */
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetTextureReferences`(inpid varchar(400), inrevision int(10))
BEGIN
        SELECT *
        FROM `texturereferences`
        WHERE PID = inpid AND Revision = inrevision;
END;

/* Header line. Object: InsertContentObject. Script date: 5/23/2011 8:59:11 AM. */
DROP PROCEDURE `InsertContentObject`;

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
END;

/* Header line. Object: InsertReview. Script date: 5/23/2011 8:59:11 AM. */
DROP PROCEDURE `InsertReview`;

CREATE DEFINER=`root`@`localhost` PROCEDURE `InsertReview`(newrating int(10),
newtext varchar(45),newsubmittedby varchar(45),newcontentobjectid varchar(400))
BEGIN
      INSERT INTO `reviews`(rating,
      text,submittedby,contentobjectid,SubmittedDate)
      values(newrating,newtext,newsubmittedby,newcontentobjectid, NOW());
END;

/* Header line. Object: OpenId_DeleteUserOpenIdLink. Script date: 5/23/2011 8:59:11 AM. */
DROP PROCEDURE `OpenId_DeleteUserOpenIdLink`;

CREATE DEFINER=`root`@`localhost` PROCEDURE `OpenId_DeleteUserOpenIdLink`(
openId_Url nvarchar(256),
userId varchar(256))
delete from update_test.OpenId where (update_test.openId_url=openId_Url)
or (update_test.user_id=userId);

/* Header line. Object: OpenId_GetUserIdByOpenld. Script date: 5/23/2011 8:59:11 AM. */
DROP PROCEDURE `OpenId_GetUserIdByOpenld`;

CREATE DEFINER=`root`@`localhost` PROCEDURE `OpenId_GetUserIdByOpenld`(openIdurl varchar(256))
select user_id from update_test.openid where (openId_url=openIdurl);

/* Header line. Object: OpenId_LinkUserWithOpenId. Script date: 5/23/2011 8:59:11 AM. */
DROP PROCEDURE `OpenId_LinkUserWithOpenId`;

CREATE DEFINER=`root`@`localhost` PROCEDURE `OpenId_LinkUserWithOpenId`(
openId_Url nvarchar(256),
userId varchar(256))
insert into update_test.OpenId (openId_url,user_id) values(openId_Url, userId);

/* Header line. Object: OpenId_Membership_GetAllUsers. Script date: 5/23/2011 8:59:11 AM. */
DROP PROCEDURE `OpenId_Membership_GetAllUsers`;

CREATE DEFINER=`root`@`localhost` PROCEDURE `OpenId_Membership_GetAllUsers`(
    ApplicationName       nvarchar(256))
SELECT u.UserName,o.openId_url, u.Email, u.PasswordQuestion, u.Comment, u.IsApproved,
            u.CreationDate,
            u.LastLoginDate,
            u.LastActivityDate,
            u.LastPasswordChangedDate,
            u.PKID, u.IsLockedOut,
            u.LastLockedOutDate
    FROM   update_test.Users u

	inner join update_test.openid o on o.User_id=u.PKID

	WHERE  u.ApplicationName = ApplicationName

    ORDER BY u.UserName;

/* Header line. Object: StrongEye_OpenID_Membership_GetAllUsers. Script date: 5/23/2011 8:59:11 AM. */
DROP PROCEDURE `StrongEye_OpenID_Membership_GetAllUsers`;

CREATE DEFINER=`root`@`localhost` PROCEDURE `StrongEye_OpenID_Membership_GetAllUsers`(
    ApplicationName       nvarchar(256))
SELECT u.UserName, m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
            m.CreateDate,
            m.LastLoginDate,
            u.LastActivityDate,
            m.LastPasswordChangedDate,
            u.UserId, m.IsLockedOut,
            m.LastLockoutDate
    FROM   tes2.aspnet_Membership m, update_test.aspnet_Users u
    WHERE  u.ApplicationId = ApplicationId AND u.UserId = m.UserId
    ORDER BY u.UserName;

/* Header line. Object: StrongEye_OpenID_Membership_GetUserByName. Script date: 5/23/2011 8:59:11 AM. */
DROP PROCEDURE `StrongEye_OpenID_Membership_GetUserByName`;

CREATE DEFINER=`root`@`localhost` PROCEDURE `StrongEye_OpenID_Membership_GetUserByName`(
    ApplicationName      nvarchar(256),
    UserName             nvarchar(256),
    CurrentTimeUtc       datetime,
    UpdateLastActivity   bit)
SELECT u.Username, m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
                m.CreateDate, m.LastLoginDate, u.LastActivityDate, m.LastPasswordChangedDate,
                u.UserId, m.IsLockedOut,m.LastLockoutDate
        FROM    update_test.aspnet_Applications a, update_test.aspnet_Users u, update_test.aspnet_Membership m
        WHERE    LOWER(ApplicationName) = a.LoweredApplicationName AND
                u.ApplicationId = a.ApplicationId    AND
                LOWER(UserName) = u.LoweredUserName AND u.UserId = m.UserId
        LIMIT 1;

/* Header line. Object: StrongEye_OpenID_Membership_GetUserByUserId. Script date: 5/23/2011 8:59:11 AM. */
DROP PROCEDURE `StrongEye_OpenID_Membership_GetUserByUserId`;

CREATE DEFINER=`root`@`localhost` PROCEDURE `StrongEye_OpenID_Membership_GetUserByUserId`(UserId varchar(256),CurrentTimeUtc datetime,UpdateLastActivity   bit)
SELECT u.Username, m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
                m.CreateDate, m.LastLoginDate, @CurrentTimeUtc, m.LastPasswordChangedDate,
                u.UserId, m.IsLockedOut,m.LastLockoutDate
    FROM    dbo.aspnet_Users u, dbo.aspnet_Membership m
    WHERE   @UserId = u.UserId AND u.UserId = m.UserId;

/* Header line. Object: UpdateContentObject. Script date: 5/23/2011 8:59:11 AM. */
DROP PROCEDURE `UpdateContentObject`;

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
END;

-- Update foreign keys of associatedkeywords
ALTER TABLE `associatedkeywords` ADD CONSTRAINT `FK_AssociatedKeywords_1`
	FOREIGN KEY ( `ContentObjectId` ) REFERENCES `contentobjects` ( `ID` );

ALTER TABLE `associatedkeywords` ADD CONSTRAINT `FK_associatedkeywords_2`
	FOREIGN KEY ( `KeywordId` ) REFERENCES `keywords` ( `ID` );

-- Update foreign keys of userprofiles
ALTER TABLE `userprofiles` ADD CONSTRAINT `UserProfiles_UserGuid_fkey`
	FOREIGN KEY ( `UserGuid` ) REFERENCES `users` ( `PKID` );

-- Update foreign keys of yaf_category
ALTER TABLE `yaf_category` ADD CONSTRAINT `FK_test2_yaf_Category_yaf_Board`
	FOREIGN KEY ( `BoardID` ) REFERENCES `yaf_board` ( `BoardID` ) ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Update foreign keys of yaf_forum
ALTER TABLE `yaf_forum` ADD CONSTRAINT `FK_test2_yaf_Forum_yaf_Category`
	FOREIGN KEY ( `CategoryID` ) REFERENCES `yaf_category` ( `CategoryID` ) ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE `yaf_forum` ADD CONSTRAINT `FK_test2_yaf_Forum_yaf_Forum`
	FOREIGN KEY ( `ParentID` ) REFERENCES `yaf_forum` ( `ForumID` ) ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE `yaf_forum` ADD CONSTRAINT `FK_test2_yaf_Forum_yaf_Message`
	FOREIGN KEY ( `LastMessageID` ) REFERENCES `yaf_message` ( `MessageID` ) ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE `yaf_forum` ADD CONSTRAINT `FK_test2_yaf_Forum_yaf_Topic`
	FOREIGN KEY ( `LastTopicID` ) REFERENCES `yaf_topic` ( `TopicID` ) ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE `yaf_forum` ADD CONSTRAINT `FK_test2_yaf_Forum_yaf_User`
	FOREIGN KEY ( `LastUserID` ) REFERENCES `yaf_user` ( `UserID` ) ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Update foreign keys of yaf_message
ALTER TABLE `yaf_message` ADD CONSTRAINT `FK_test2_yaf_Message_yaf_Message`
	FOREIGN KEY ( `ReplyTo` ) REFERENCES `yaf_message` ( `MessageID` ) ON DELETE CASCADE ON UPDATE NO ACTION;

ALTER TABLE `yaf_message` ADD CONSTRAINT `FK_test2_yaf_Message_yaf_Topic`
	FOREIGN KEY ( `TopicID` ) REFERENCES `yaf_topic` ( `TopicID` ) ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE `yaf_message` ADD CONSTRAINT `FK_test2_yaf_Message_yaf_User`
	FOREIGN KEY ( `UserID` ) REFERENCES `yaf_user` ( `UserID` ) ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Update foreign keys of yaf_rank
ALTER TABLE `yaf_rank` ADD CONSTRAINT `FK_test2_yaf_Rank_yaf_Board`
	FOREIGN KEY ( `BoardID` ) REFERENCES `yaf_board` ( `BoardID` ) ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Update foreign keys of yaf_topic
ALTER TABLE `yaf_topic` ADD CONSTRAINT `FK_test2_yaf_Topic_yaf_Forum`
	FOREIGN KEY ( `ForumID` ) REFERENCES `yaf_forum` ( `ForumID` ) ON DELETE CASCADE ON UPDATE NO ACTION;

ALTER TABLE `yaf_topic` ADD CONSTRAINT `FK_test2_yaf_Topic_yaf_Message`
	FOREIGN KEY ( `LastMessageID` ) REFERENCES `yaf_message` ( `MessageID` ) ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE `yaf_topic` ADD CONSTRAINT `FK_test2_yaf_Topic_yaf_Poll`
	FOREIGN KEY ( `PollID` ) REFERENCES `yaf_poll` ( `PollID` ) ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE `yaf_topic` ADD CONSTRAINT `FK_test2_yaf_Topic_yaf_Topic`
	FOREIGN KEY ( `TopicMovedID` ) REFERENCES `yaf_topic` ( `TopicID` ) ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE `yaf_topic` ADD CONSTRAINT `FK_test2_yaf_Topic_yaf_User`
	FOREIGN KEY ( `UserID` ) REFERENCES `yaf_user` ( `UserID` ) ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE `yaf_topic` ADD CONSTRAINT `FK_test2_yaf_Topic_yaf_User2`
	FOREIGN KEY ( `LastUserID` ) REFERENCES `yaf_user` ( `UserID` ) ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Update foreign keys of yaf_user
ALTER TABLE `yaf_user` ADD CONSTRAINT `FK_test2_yaf_User_yaf_Board`
	FOREIGN KEY ( `BoardID` ) REFERENCES `yaf_board` ( `BoardID` ) ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE `yaf_user` ADD CONSTRAINT `FK_test2_yaf_User_yaf_Rank`
	FOREIGN KEY ( `RankID` ) REFERENCES `yaf_rank` ( `RankID` ) ON DELETE NO ACTION ON UPDATE NO ACTION;

