
delimiter $$

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
) ENGINE=InnoDB AUTO_INCREMENT=134 DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `current_uploads` (
  `pid` varchar(100) NOT NULL,
  `hash` varchar(100) NOT NULL,
  KEY `pid` (`pid`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `keywords` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Keyword` varchar(45) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1$$

delimiter $$
CREATE TABLE `associatedkeywords` (
  `ContentObjectId` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `KeywordId` int(10) unsigned NOT NULL,
  KEY `FK_AssociatedKeywords_1` (`ContentObjectId`),
  KEY `FK_associatedkeywords_2` (`KeywordId`),
  CONSTRAINT `FK_AssociatedKeywords_1` FOREIGN KEY (`ContentObjectId`) REFERENCES `contentobjects` (`ID`),
  CONSTRAINT `FK_associatedkeywords_2` FOREIGN KEY (`KeywordId`) REFERENCES `keywords` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `missingtextures` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Filename` varchar(45) NOT NULL,
  `Type` varchar(45) NOT NULL,
  `UVSet` int(10) unsigned NOT NULL,
  `PID` varchar(45) NOT NULL,
  `Revision` int(10) unsigned NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `openid` (
  `openId_url` varchar(256) CHARACTER SET utf8 DEFAULT NULL,
  `user_id` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `personalization` (
  `username` varchar(255) DEFAULT NULL,
  `path` varchar(255) DEFAULT NULL,
  `applicationname` varchar(255) DEFAULT NULL,
  `personalizationblob` blob
) ENGINE=InnoDB DEFAULT CHARSET=latin1$$

delimiter $$

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
) ENGINE=InnoDB DEFAULT CHARSET=latin1$$

delimiter $$

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
) ENGINE=InnoDB DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `roles` (
  `Rolename` varchar(255) NOT NULL DEFAULT '',
  `ApplicationName` varchar(255) NOT NULL DEFAULT '',
  PRIMARY KEY (`Rolename`,`ApplicationName`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `sitemap` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `ApplicationName` varchar(255) NOT NULL DEFAULT '',
  `Title` varchar(255) DEFAULT NULL,
  `Description` text,
  `Url` text,
  `Roles` text,
  `Parent` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `supportingfiles` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Filename` varchar(45) NOT NULL,
  `Description` varchar(45) NOT NULL,
  `PID` varchar(45) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `text_log` (
  `Log` varchar(255) NOT NULL DEFAULT ''
) ENGINE=InnoDB DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `texturereferences` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Filename` varchar(45) NOT NULL,
  `Type` varchar(45) NOT NULL,
  `UVSet` int(10) unsigned NOT NULL,
  `PID` varchar(45) NOT NULL,
  `Revision` int(10) unsigned NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=latin1$$


delimiter $$

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
) ENGINE=InnoDB DEFAULT CHARSET=latin1$$

delimiter $$

CREATE TABLE `usersinroles` (
  `Username` varchar(255) NOT NULL DEFAULT '',
  `Rolename` varchar(255) NOT NULL DEFAULT '',
  `ApplicationName` varchar(255) NOT NULL DEFAULT '',
  PRIMARY KEY (`Username`,`Rolename`,`ApplicationName`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1$$
delimiter $$

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
) ENGINE=InnoDB DEFAULT CHARSET=latin1$$