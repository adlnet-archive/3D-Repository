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
-- Create schema 3dr
--

CREATE DATABASE IF NOT EXISTS 3dr;
USE 3dr;

--
-- Definition of table `associatedkeywords`
--

DROP TABLE IF EXISTS `associatedkeywords`;
CREATE TABLE `associatedkeywords` (
  `ContentObjectId` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `KeywordId` int(10) unsigned NOT NULL,
  KEY `FK_AssociatedKeywords_1` (`ContentObjectId`),
  KEY `FK_associatedkeywords_2` (`KeywordId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `associatedkeywords`
--

/*!40000 ALTER TABLE `associatedkeywords` DISABLE KEYS */;
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
  `ThumbnailFileName` varchar(400) NOT NULL DEFAULT '',
  `ThumbnailFileId` varchar(400) NOT NULL DEFAULT '',
  PRIMARY KEY (`ID`),
  KEY `FK_contentobjects_1` (`Submitter`)
) ENGINE=InnoDB AUTO_INCREMENT=259 DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

--
-- Dumping data for table `contentobjects`
--



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
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

--
-- Dumping data for table `keywords`
--

/*!40000 ALTER TABLE `keywords` DISABLE KEYS */;
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
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;

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
) ENGINE=InnoDB AUTO_INCREMENT=47 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `pidingroup`
--




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
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

--
-- Dumping data for table `reviews`
--

/*!40000 ALTER TABLE `reviews` DISABLE KEYS */;
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
  `dsid` varchar(45) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;

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
) ENGINE=InnoDB AUTO_INCREMENT=93 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `texturereferences`
--




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


--
-- Definition of table `userpermission`
--

DROP TABLE IF EXISTS `userpermission`;
CREATE TABLE `userpermission` (
  `id` int(10) unsigned DEFAULT NULL,
  `username` varchar(255) NOT NULL,
  `pid` varchar(255) NOT NULL,
  `permission` tinyint(3) unsigned DEFAULT NULL,
  PRIMARY KEY (`pid`,`username`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `userpermission`
--




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
  KEY `UserProfiles_UserGuid_fkey` (`UserGuid`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

--
-- Dumping data for table `userprofiles`
--



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

--
-- Definition of table `usersingroups`
--

DROP TABLE IF EXISTS `usersingroups`;
CREATE TABLE `usersingroups` (
  `UserName` varchar(255) NOT NULL,
  `GroupName` varchar(45) NOT NULL,
  `index` int(10) unsigned NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`index`)
) ENGINE=InnoDB AUTO_INCREMENT=54 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `usersingroups`
--

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


--
-- Definition of table `yaf_board`
--

DROP TABLE IF EXISTS `yaf_board`;
CREATE TABLE `yaf_board` (
  `BoardID` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(128) NOT NULL,
  `AllowThreaded` tinyint(1) NOT NULL,
  `MembershipAppName` varchar(255) DEFAULT NULL,
  `RolesAppName` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`BoardID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

--
-- Dumping data for table `yaf_board`
--


--
-- Definition of table `yaf_category`
--

DROP TABLE IF EXISTS `yaf_category`;
CREATE TABLE `yaf_category` (
  `CategoryID` int(11) NOT NULL AUTO_INCREMENT,
  `BoardID` int(11) NOT NULL,
  `Name` varchar(128) NOT NULL,
  `CategoryImage` varchar(255) DEFAULT NULL,
  `SortOrder` smallint(5) NOT NULL,
  PRIMARY KEY (`CategoryID`),
  UNIQUE KEY `IX_test2_yaf_Category` (`BoardID`,`Name`),
  KEY `IX_test2_yaf_Category_BoardID` (`BoardID`),
  KEY `IX_test2_yaf_Category_Name` (`Name`),
  CONSTRAINT `FK_test2_yaf_Category_yaf_Board` FOREIGN KEY (`BoardID`) REFERENCES `yaf_board` (`BoardID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

--
-- Dumping data for table `yaf_category`
--

/*!40000 ALTER TABLE `yaf_category` DISABLE KEYS */;
/*!40000 ALTER TABLE `yaf_category` ENABLE KEYS */;


--
-- Definition of table `yaf_forum`
--

DROP TABLE IF EXISTS `yaf_forum`;
CREATE TABLE `yaf_forum` (
  `ForumID` int(11) NOT NULL AUTO_INCREMENT,
  `CategoryID` int(11) NOT NULL,
  `ParentID` int(11) DEFAULT NULL,
  `Name` varchar(128) NOT NULL,
  `Description` varchar(255) NOT NULL,
  `SortOrder` smallint(5) NOT NULL,
  `LastPosted` datetime DEFAULT NULL,
  `LastTopicID` int(11) DEFAULT NULL,
  `LastMessageID` int(11) DEFAULT NULL,
  `LastUserID` int(11) DEFAULT NULL,
  `LastUserName` varchar(128) DEFAULT NULL,
  `NumTopics` int(11) NOT NULL,
  `NumPosts` int(11) NOT NULL,
  `RemoteURL` varchar(128) DEFAULT NULL,
  `Flags` int(11) NOT NULL DEFAULT '0',
  `ThemeURL` varchar(128) DEFAULT NULL,
  PRIMARY KEY (`ForumID`),
  UNIQUE KEY `IX_test2_yaf_Forum` (`ParentID`,`Name`),
  KEY `FK_test2_yaf_Forum_yaf_Message` (`LastMessageID`),
  KEY `FK_test2_yaf_Forum_yaf_Topic` (`LastTopicID`),
  KEY `FK_test2_yaf_Forum_yaf_User` (`LastUserID`),
  KEY `IX_test2_yaf_Forum_CategoryID` (`CategoryID`),
  KEY `IX_test2_yaf_Forum_Flags` (`Flags`),
  KEY `IX_test2_yaf_Forum_ParentID` (`ParentID`),
  CONSTRAINT `FK_test2_yaf_Forum_yaf_Category` FOREIGN KEY (`CategoryID`) REFERENCES `yaf_category` (`CategoryID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_test2_yaf_Forum_yaf_Forum` FOREIGN KEY (`ParentID`) REFERENCES `yaf_forum` (`ForumID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_test2_yaf_Forum_yaf_Message` FOREIGN KEY (`LastMessageID`) REFERENCES `yaf_message` (`MessageID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_test2_yaf_Forum_yaf_Topic` FOREIGN KEY (`LastTopicID`) REFERENCES `yaf_topic` (`TopicID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_test2_yaf_Forum_yaf_User` FOREIGN KEY (`LastUserID`) REFERENCES `yaf_user` (`UserID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

--
-- Dumping data for table `yaf_forum`
--

/*!40000 ALTER TABLE `yaf_forum` DISABLE KEYS */;
/*!40000 ALTER TABLE `yaf_forum` ENABLE KEYS */;


--
-- Definition of table `yaf_message`
--

DROP TABLE IF EXISTS `yaf_message`;
CREATE TABLE `yaf_message` (
  `MessageID` int(11) NOT NULL AUTO_INCREMENT,
  `TopicID` int(11) NOT NULL,
  `ReplyTo` int(11) DEFAULT NULL,
  `Position` int(11) NOT NULL,
  `Indent` int(11) NOT NULL,
  `UserID` int(11) NOT NULL,
  `UserName` varchar(128) DEFAULT NULL,
  `Posted` datetime NOT NULL,
  `Message` longtext NOT NULL,
  `IP` varchar(15) NOT NULL,
  `Edited` datetime DEFAULT NULL,
  `Flags` int(11) NOT NULL DEFAULT '23',
  `EditReason` varchar(128) DEFAULT NULL,
  `IsModeratorChanged` tinyint(1) NOT NULL DEFAULT '0',
  `DeleteReason` varchar(128) DEFAULT NULL,
  `BlogPostID` varchar(128) DEFAULT NULL,
  PRIMARY KEY (`MessageID`),
  KEY `FK_test2_yaf_Message_yaf_Message` (`ReplyTo`),
  KEY `IX_test2_yaf_Message_Flags` (`Flags`),
  KEY `IX_test2_yaf_Message_TopicID` (`TopicID`),
  KEY `IX_test2_yaf_Message_UserID` (`UserID`),
  CONSTRAINT `FK_test2_yaf_Message_yaf_Message` FOREIGN KEY (`ReplyTo`) REFERENCES `yaf_message` (`MessageID`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `FK_test2_yaf_Message_yaf_Topic` FOREIGN KEY (`TopicID`) REFERENCES `yaf_topic` (`TopicID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_test2_yaf_Message_yaf_User` FOREIGN KEY (`UserID`) REFERENCES `yaf_user` (`UserID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

--
-- Dumping data for table `yaf_message`
--

/*!40000 ALTER TABLE `yaf_message` DISABLE KEYS */;
/*!40000 ALTER TABLE `yaf_message` ENABLE KEYS */;


--
-- Definition of table `yaf_poll`
--

DROP TABLE IF EXISTS `yaf_poll`;
CREATE TABLE `yaf_poll` (
  `PollID` int(11) NOT NULL AUTO_INCREMENT,
  `Question` varchar(128) NOT NULL,
  `Closes` datetime DEFAULT NULL,
  PRIMARY KEY (`PollID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

--
-- Dumping data for table `yaf_poll`
--

/*!40000 ALTER TABLE `yaf_poll` DISABLE KEYS */;
/*!40000 ALTER TABLE `yaf_poll` ENABLE KEYS */;


--
-- Definition of table `yaf_rank`
--

DROP TABLE IF EXISTS `yaf_rank`;
CREATE TABLE `yaf_rank` (
  `RankID` int(11) NOT NULL AUTO_INCREMENT,
  `BoardID` int(11) NOT NULL,
  `Name` varchar(128) NOT NULL,
  `MinPosts` int(11) DEFAULT NULL,
  `RankImage` varchar(128) DEFAULT NULL,
  `Flags` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`RankID`),
  UNIQUE KEY `IX_test2_yaf_Rank` (`BoardID`,`Name`),
  CONSTRAINT `FK_test2_yaf_Rank_yaf_Board` FOREIGN KEY (`BoardID`) REFERENCES `yaf_board` (`BoardID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

--
-- Dumping data for table `yaf_rank`
--

/*!40000 ALTER TABLE `yaf_rank` DISABLE KEYS */;
/*!40000 ALTER TABLE `yaf_rank` ENABLE KEYS */;


--
-- Definition of table `yaf_topic`
--

DROP TABLE IF EXISTS `yaf_topic`;
CREATE TABLE `yaf_topic` (
  `TopicID` int(11) NOT NULL AUTO_INCREMENT,
  `ForumID` int(11) NOT NULL,
  `UserID` int(11) NOT NULL,
  `UserName` varchar(128) DEFAULT NULL,
  `Posted` datetime NOT NULL,
  `Topic` varchar(128) NOT NULL,
  `Views` int(11) NOT NULL,
  `Priority` smallint(5) NOT NULL,
  `PollID` int(11) DEFAULT NULL,
  `TopicMovedID` int(11) DEFAULT NULL,
  `LastPosted` datetime DEFAULT NULL,
  `LastMessageID` int(11) DEFAULT NULL,
  `LastUserID` int(11) DEFAULT NULL,
  `LastUserName` varchar(128) DEFAULT NULL,
  `NumPosts` int(11) NOT NULL,
  `Flags` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`TopicID`),
  KEY `FK_test2_yaf_Topic_yaf_Message` (`LastMessageID`),
  KEY `FK_test2_yaf_Topic_yaf_Poll` (`PollID`),
  KEY `FK_test2_yaf_Topic_yaf_Topic` (`TopicMovedID`),
  KEY `FK_test2_yaf_Topic_yaf_User2` (`LastUserID`),
  KEY `IX_test2_yaf_Topic_Flags` (`Flags`),
  KEY `IX_test2_yaf_Topic_ForumID` (`ForumID`),
  KEY `IX_test2_yaf_Topic_UserID` (`UserID`),
  CONSTRAINT `FK_test2_yaf_Topic_yaf_Forum` FOREIGN KEY (`ForumID`) REFERENCES `yaf_forum` (`ForumID`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `FK_test2_yaf_Topic_yaf_Message` FOREIGN KEY (`LastMessageID`) REFERENCES `yaf_message` (`MessageID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_test2_yaf_Topic_yaf_Poll` FOREIGN KEY (`PollID`) REFERENCES `yaf_poll` (`PollID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_test2_yaf_Topic_yaf_Topic` FOREIGN KEY (`TopicMovedID`) REFERENCES `yaf_topic` (`TopicID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_test2_yaf_Topic_yaf_User` FOREIGN KEY (`UserID`) REFERENCES `yaf_user` (`UserID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_test2_yaf_Topic_yaf_User2` FOREIGN KEY (`LastUserID`) REFERENCES `yaf_user` (`UserID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

--
-- Dumping data for table `yaf_topic`
--

/*!40000 ALTER TABLE `yaf_topic` DISABLE KEYS */;
/*!40000 ALTER TABLE `yaf_topic` ENABLE KEYS */;


--
-- Definition of table `yaf_user`
--

DROP TABLE IF EXISTS `yaf_user`;
CREATE TABLE `yaf_user` (
  `UserID` int(11) NOT NULL AUTO_INCREMENT,
  `BoardID` int(11) NOT NULL,
  `ProviderUserKey` varchar(64) DEFAULT NULL,
  `Name` varchar(128) NOT NULL,
  `Password` varchar(32) NOT NULL,
  `Email` varchar(128) DEFAULT NULL,
  `Joined` datetime NOT NULL,
  `LastVisit` datetime NOT NULL,
  `IP` varchar(15) DEFAULT NULL,
  `NumPosts` int(11) NOT NULL,
  `TimeZone` int(11) NOT NULL,
  `Avatar` varchar(255) DEFAULT NULL,
  `Signature` longtext,
  `AvatarImage` longblob,
  `AvatarImageType` varchar(50) DEFAULT NULL,
  `RankID` int(11) NOT NULL,
  `Suspended` datetime DEFAULT NULL,
  `LanguageFile` varchar(128) DEFAULT NULL,
  `ThemeFile` varchar(128) DEFAULT NULL,
  `OverrideDefaultThemes` tinyint(1) NOT NULL DEFAULT '0',
  `PMNotification` tinyint(1) NOT NULL DEFAULT '1',
  `Flags` int(11) NOT NULL DEFAULT '0',
  `Points` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`UserID`),
  UNIQUE KEY `IX_test2_yaf_User` (`BoardID`,`Name`),
  KEY `FK_test2_yaf_User_yaf_Rank` (`RankID`),
  KEY `IX_test2_yaf_User_Flags` (`Flags`),
  KEY `IX_test2_yaf_User_Name` (`Name`),
  KEY `IX_test2_yaf_User_ProviderUserKey` (`ProviderUserKey`),
  CONSTRAINT `FK_test2_yaf_User_yaf_Board` FOREIGN KEY (`BoardID`) REFERENCES `yaf_board` (`BoardID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_test2_yaf_User_yaf_Rank` FOREIGN KEY (`RankID`) REFERENCES `yaf_rank` (`RankID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

--
-- Dumping data for table `yaf_user`
--

/*!40000 ALTER TABLE `yaf_user` DISABLE KEYS */;
/*!40000 ALTER TABLE `yaf_user` ENABLE KEYS */;


--
-- Definition of procedure `AddSupportingFile`
--

DROP PROCEDURE IF EXISTS `AddSupportingFile`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddSupportingFile`(newfilename varchar(45),
newdescription varchar(400),newcontentobjectid varchar(400),newdsid varchar(400))
BEGIN
      INSERT INTO `supportingfiles`(Filename,
      Description,PID,dsid)
      values(newfilename,newdescription,newcontentobjectid,newdsid);
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
-- Definition of procedure `AddUserToGroup`
--

DROP PROCEDURE IF EXISTS `AddUserToGroup`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddUserToGroup`(inusername varchar(255), ingroupname varchar(255))
insert into `usersingroups`(username,groupname) values(inusername,ingroupname) $$
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

          Select `PermissionLevel` from `pidingroup` where `Groupname`=ingroupname and `pid`=inpid;
          END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `CheckPermission`
--

DROP PROCEDURE IF EXISTS `CheckPermission`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `CheckPermission`(inpid varchar(255),inusername varchar(255))
BEGIN
SELECT permission
  FROM userpermission
  where pid = inpid and username = inusername;
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
-- Definition of procedure `GetGroupsByOwner`
--

DROP PROCEDURE IF EXISTS `GetGroupsByOwner`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetGroupsByOwner`(inOwner varchar(255))
BEGIN
     select * from `usergroups` where `Owner` = inOwner;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `GetGroupsByPid`
--

DROP PROCEDURE IF EXISTS `GetGroupsByPid`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetGroupsByPid`(inpid varchar(45))
BEGIN
       select * from `pidingroup` where `PID` =  inpid;
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
PREPARE STMT FROM "SELECT PID, Title, ScreenShotFileName,ScreenShotFileId, Description, Views, ThumbnailFileName, ThumbnailFileId
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
-- Definition of procedure `GetMostPopular`
--

DROP PROCEDURE IF EXISTS `GetMostPopular`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetMostPopular`(s integer, length integer)
BEGIN
SET @lmt = length;
SET @s = s;
PREPARE STMT FROM 
    "SELECT PID, Title, ScreenShotFileName,ScreenShotFileId, Description, Views, ThumbnailFileName, ThumbnailFileId
     FROM ContentObjects
     ORDER BY Views DESC
     LIMIT ?, ?";
EXECUTE STMT USING @s, @lmt;
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
    PREPARE STMT FROM "SELECT PID, Title, ScreenShotFileName,ScreenShotFileId, Description, Views, ThumbnailFileName, ThumbnailFileId
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
    PREPARE STMT FROM "SELECT PID, Title, ScreenShotFileName,ScreenShotFileId, Description, Views, ThumbnailFileName, ThumbnailFileId
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
-- Definition of procedure `GetUser`
--

DROP PROCEDURE IF EXISTS `GetUser`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetUser`( inusername varchar(255))
BEGIN
    Select * from `users` where `UserName` = inusername;
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
-- Definition of procedure `GetUserWithModelPermission`
--

DROP PROCEDURE IF EXISTS `GetUserWithModelPermission`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetUserWithModelPermission`(pid varchar(255))
BEGIN
	SELECT  userpermission.username, userpermission.permission
  FROM userpermission
  where userpermission.pid = pid;
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
newthumbnailfilename nvarchar (400),
newthumbnailfileid nvarchar(400),
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
thumbnailfilename,
thumbnailfileid,
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
newthumbnailfilename,
newthumbnailfileid,
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
delete from 3dr.OpenId where (3dr.openId_url=openId_Url)
or (3dr.user_id=userId) $$
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
select user_id from 3dr.openid where (openId_url=openIdurl) $$
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
insert into 3dr.OpenId (openId_url,user_id) values(openId_Url, userId) $$
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
    FROM   3dr.Users u

	inner join 3dr.openid o on o.User_id=u.PKID

	WHERE  u.ApplicationName = ApplicationName

    ORDER BY u.UserName $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `procedure1`
--

DROP PROCEDURE IF EXISTS `procedure1`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `procedure1`()
BEGIN
  
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `RemoveAllKeywords`
--

DROP PROCEDURE IF EXISTS `RemoveAllKeywords`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `RemoveAllKeywords`(targetpid varchar(400))
BEGIN
	DELETE FROM `associatedkeywords`
	WHERE ContentObjectID in (SELECT ID FROM `contentobjects` WHERE pid= targetpid);
END $$
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
-- Definition of procedure `RemoveGroupPermission`
--

DROP PROCEDURE IF EXISTS `RemoveGroupPermission`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `RemoveGroupPermission`(ingroupName varchar(255), inpid varchar(255))
BEGIN
	DELETE FROM pidingroup
  WHERE pidingroup.GroupName = ingroupName and pidingroup.PID = inpid;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `RemoveKeyword`
--

DROP PROCEDURE IF EXISTS `RemoveKeyword`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `RemoveKeyword`(targetpid varchar(400), targetkeyword varchar(45))
BEGIN
	DELETE FROM `associatedkeywords`
	WHERE ContentObjectID in (SELECT ID FROM `contentobjects` WHERE pid = targetpid)
	AND KeywordID in (SELECT ID FROM `keywords` WHERE keyword = targetkeyword );
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
begin
delete from `usersingroups` where `username` = inusername and `groupname`=ingroupname;
end $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `RemoveUserPermission`
--

DROP PROCEDURE IF EXISTS `RemoveUserPermission`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `RemoveUserPermission`(pid varchar(255), username varchar(255))
BEGIN
	DELETE FROM userpermission
  where userpermission.pid = pid and userpermission.username = username;
END $$
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
-- Definition of procedure `SetUserPermission`
--

DROP PROCEDURE IF EXISTS `SetUserPermission`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `SetUserPermission`(inusername varchar(255), inpid varchar(255), inplevel tinyint(4))
BEGIN
	if (select count(`permission`) from `userpermission`   where `userpermission`.`PID` = inpid AND `userpermission`.`username` = inusername) >0

then

        update `userpermission`  set permission=inplevel where `userpermission`.`PID` = inpid AND `userpermission`.`username` = inusername;

else
            insert into `userpermission` (pid,username,permission) values(inpid,inusername,inplevel);

end if;
END $$
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
    FROM   tes2.aspnet_Membership m, 3dr.aspnet_Users u
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
        FROM    3dr.aspnet_Applications a, 3dr.aspnet_Users u, 3dr.aspnet_Membership m
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
newthumbnailfilename nvarchar(400),
newthumbnailfileid nvarchar(400),
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
thumbnailfilename = newthumbnailfilename,
thumbnailfileid = newthumbnailfileid,
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
-- Create schema apikeys
--

CREATE DATABASE IF NOT EXISTS apikeys;
USE apikeys;

--
-- Definition of table `apikeys`
--

DROP TABLE IF EXISTS `apikeys`;
CREATE TABLE `apikeys` (
  `Email` varchar(255) NOT NULL,
  `KeyText` varchar(45) NOT NULL,
  `UsageText` varchar(1000) NOT NULL,
  `State` int(10) unsigned NOT NULL,
  PRIMARY KEY (`KeyText`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `apikeys`
--

/*!40000 ALTER TABLE `apikeys` DISABLE KEYS */;
/*!40000 ALTER TABLE `apikeys` ENABLE KEYS */;


--
-- Definition of procedure `DeleteKey`
--

DROP PROCEDURE IF EXISTS `DeleteKey`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `DeleteKey`(newkey varchar(40))
BEGIN
   DELETE FROM `APIKEYS` WHERE keytext = newkey;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `GetByKey`
--

DROP PROCEDURE IF EXISTS `GetByKey`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetByKey`(newkey varchar(40))
BEGIN
   SELECT * FROM `APIKEYS` WHERE keytext = newkey;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `GetByUser`
--

DROP PROCEDURE IF EXISTS `GetByUser`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetByUser`(newEmail varchar(40))
BEGIN
   SELECT * FROM `APIKEYS` WHERE email = newEmail;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `InsertKey`
--

DROP PROCEDURE IF EXISTS `InsertKey`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `InsertKey`(
       newemail VARCHAR(255),
       newkey VARCHAR(45),
       newusage VARCHAR(1000),
       newstate INTEGER(10))
BEGIN

      INSERT INTO `apikeys`(Email,
      KeyText,UsageText,State)
      values(newemail,newkey,newusage,newstate);
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `UpdateKey`
--

DROP PROCEDURE IF EXISTS `UpdateKey`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `UpdateKey`(
       newemail VARCHAR(255),
       newkey VARCHAR(45),
       newusage VARCHAR(1000),
       newstate INTEGER(10))
BEGIN

      UPDATE `apikeys` SET
      Email = newemail,
      KeyText=newkey,
      UsageText=newusage,
      State = newstate
      WHERE KeyText = newkey;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

call `apikeys`.InsertKey('cybrarian@adlnet.gov','00-00-00','ADL 3DR Federation API key',0);

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;

