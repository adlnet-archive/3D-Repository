delimiter $$
alter TABLE `associatedkeywords` (
  modify `ContentObjectId` int(10) unsigned NOT NULL AUTO_INCREMENT,
  modify `KeywordId` int(10) unsigned NOT NULL,
  KEY `FK_AssociatedKeywords_1` (`ContentObjectId`),
  KEY `FK_associatedkeywords_2` (`KeywordId`),
  CONSTRAINT `FK_AssociatedKeywords_1` FOREIGN KEY (`ContentObjectId`) REFERENCES `contentobjects` (`ID`),
  CONSTRAINT `FK_associatedkeywords_2` FOREIGN KEY (`KeywordId`) REFERENCES `keywords` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1$$

delimiter $$

alter TABLE `contentobjects` (
  modify `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  modify `Description` varchar(400) NOT NULL DEFAULT ' ',
  modify `Title` varchar(400) NOT NULL DEFAULT ' ',
  modify `ContentFileName` varchar(400) NOT NULL DEFAULT ' ',
  modify `ContentFileId` varchar(400) NOT NULL DEFAULT ' ',
  modify `ScreenShotFileName` varchar(400) NOT NULL DEFAULT ' ',
  modify `ScreenShotFileId` varchar(400) NOT NULL DEFAULT ' ',
  modify `Submitter` varchar(400) NOT NULL DEFAULT ' ',
  modify `SponsorLogoFileName` varchar(400) NOT NULL DEFAULT ' ',
  modify `SponsorLogoFileId` varchar(400) NOT NULL DEFAULT ' ',
  modify `DeveloperLogoFileName` varchar(400) NOT NULL DEFAULT ' ',
  modify `DeveloperLogoFileId` varchar(400) NOT NULL DEFAULT ' ',
  modify `AssetType` varchar(400) NOT NULL DEFAULT ' ',
  modify `DisplayFileName` varchar(400) NOT NULL DEFAULT ' ',
  modify `DisplayFileId` varchar(400) NOT NULL DEFAULT ' ',
  modify `MoreInfoUrl` varchar(400) NOT NULL DEFAULT ' ',
  modify `DeveloperName` varchar(400) NOT NULL DEFAULT ' ',
  modify `SponsorName` varchar(400) NOT NULL DEFAULT ' ',
  modify `ArtistName` varchar(400) NOT NULL DEFAULT ' ',
  modify `CreativeCommonsLicenseUrl` varchar(400) NOT NULL DEFAULT ' ',
  modify `UnitScale` varchar(400) NOT NULL DEFAULT ' ',
  modify `UpAxis` varchar(400) NOT NULL DEFAULT ' ',
  modify `UVCoordinateChannel` varchar(400) NOT NULL DEFAULT ' ',
  modify `IntentionOfTexture` varchar(400) NOT NULL DEFAULT ' ',
  modify `Format` varchar(400) NOT NULL DEFAULT ' ',
  modify `Views` int(10) unsigned zerofill NOT NULL DEFAULT '0000000000',
  modify `Downloads` int(10) unsigned zerofill NOT NULL DEFAULT '0000000000',
  modify `NumPolygons` int(10) unsigned zerofill NOT NULL DEFAULT '0000000000',
  modify `NumTextures` int(10) unsigned zerofill NOT NULL DEFAULT '0000000000',
  modify `UploadedDate` datetime DEFAULT '0000-00-00 00:00:00',
  modify `LastModified` datetime DEFAULT '0000-00-00 00:00:00',
  modify `LastViewed` datetime DEFAULT '0000-00-00 00:00:00',
  modify `PID` varchar(45) DEFAULT NULL,
  modify `Revision` varchar(45) NOT NULL,
  modify `Enabled` tinyint(1) DEFAULT NULL,
  modify `requiressubmit` tinyint(1) DEFAULT NULL,
  modify `OriginalFileName` varchar(400) DEFAULT NULL,
  modify `OriginalFileId` varchar(400) DEFAULT NULL,
  modify `UploadComplete` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `FK_contentobjects_1` (`Submitter`)
) ENGINE=InnoDB AUTO_INCREMENT=134 DEFAULT CHARSET=latin1$$

delimiter $$

alter TABLE `current_uploads` (
  modify `pid` varchar(100) NOT NULL,
  modify `hash` varchar(100) NOT NULL,
  KEY `pid` (`pid`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1$$

delimiter $$

alter TABLE `keywords` (
  modify `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  modify `Keyword` varchar(45) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1$$

delimiter $$

alter TABLE `missingtextures` (
  modify `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  modify `Filename` varchar(45) NOT NULL,
  modify `Type` varchar(45) NOT NULL,
  modify `UVSet` int(10) unsigned NOT NULL,
  modify `PID` varchar(45) NOT NULL,
  modify `Revision` int(10) unsigned NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=latin1$$

delimiter $$

alter TABLE `openid` (
  modify `openId_url` varchar(256) CHARACTER SET utf8 DEFAULT NULL,
  modify `user_id` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1$$

delimiter $$

alter TABLE `personalization` (
  modify `username` varchar(255) DEFAULT NULL,
  modify `path` varchar(255) DEFAULT NULL,
  modify `applicationname` varchar(255) DEFAULT NULL,
  modify `personalizationblob` blob
) ENGINE=InnoDB DEFAULT CHARSET=latin1$$

delimiter $$

alter TABLE `profiles` (
  modify `UniqueID` int(8) NOT NULL AUTO_INCREMENT,
  modify `Username` varchar(255) NOT NULL DEFAULT '',
  modify `ApplicationName` varchar(255) NOT NULL DEFAULT '',
  modify `IsAnonymous` tinyint(1) DEFAULT '0',
  modify `LastActivityDate` datetime DEFAULT NULL,
  modify `LastUpdatedDate` datetime DEFAULT NULL,
  PRIMARY KEY (`UniqueID`),
  UNIQUE KEY `PKProfiles` (`Username`,`ApplicationName`),
  UNIQUE KEY `PKID` (`UniqueID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1$$

delimiter $$

alter TABLE `reviews` (
  modify `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  modify `Rating` int(10) unsigned NOT NULL,
  modify `Text` varchar(45) NOT NULL,
  modify `SubmittedBy` varchar(45) NOT NULL,
  modify `SubmittedDate` datetime NOT NULL,
  modify `ContentObjectId` varchar(400) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `FK_Reviews_1` (`ContentObjectId`),
  KEY `FK_reviews_2` (`SubmittedBy`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1$$

delimiter $$

alter TABLE `roles` (
  modify `Rolename` varchar(255) NOT NULL DEFAULT '',
  modify `ApplicationName` varchar(255) NOT NULL DEFAULT '',
  PRIMARY KEY (`Rolename`,`ApplicationName`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1$$

delimiter $$

alter TABLE `sitemap` (
  modify `ID` int(11) NOT NULL AUTO_INCREMENT,
  modify `ApplicationName` varchar(255) NOT NULL DEFAULT '',
  modify `Title` varchar(255) DEFAULT NULL,
  modify `Description` text,
  modify `Url` text,
  modify `Roles` text,
  modify `Parent` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1$$

delimiter $$

alter TABLE `supportingfiles` (
  modify `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  modify `Filename` varchar(45) NOT NULL,
  modify `Description` varchar(45) NOT NULL,
  modify `PID` varchar(45) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=latin1$$

delimiter $$

alter TABLE `text_log` (
  modify `Log` varchar(255) NOT NULL DEFAULT ''
) ENGINE=InnoDB DEFAULT CHARSET=latin1$$

delimiter $$

alter TABLE `texturereferences` (
  modify `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  modify `Filename` varchar(45) NOT NULL,
  modify `Type` varchar(45) NOT NULL,
  modify `UVSet` int(10) unsigned NOT NULL,
  modify `PID` varchar(45) NOT NULL,
  modify `Revision` int(10) unsigned NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=latin1$$

delimiter $$

alter TABLE `userprofiles` (
  modify `UserID` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  modify `UserGuid` char(36) NOT NULL,
  modify `FirstName` varchar(255) DEFAULT NULL,
  modify `LastName` varchar(255) DEFAULT NULL,
  modify `Email` varchar(255) DEFAULT NULL,
  modify `WebsiteURL` text,
  modify `SponsorName` varchar(255) DEFAULT NULL,
  modify `SponsorLogo` mediumblob,
  modify `DeveloperName` varchar(255) DEFAULT NULL,
  modify `DeveloperLogo` mediumblob,
  modify `ArtistName` varchar(255) DEFAULT NULL,
  modify `Phone` varchar(50) DEFAULT NULL,
  modify `alterdDate` date DEFAULT NULL,
  modify `alterdBy` varchar(255) DEFAULT NULL,
  modify `LastEditedBy` varchar(255) DEFAULT NULL,
  modify `Comments` varchar(255) DEFAULT NULL,
  modify `Description` varchar(255) DEFAULT NULL,
  modify `DeveloperLogoContentType` varchar(255) DEFAULT NULL,
  modify `SponsorLogoContentType` varchar(255) DEFAULT NULL,
  modify `DeveloperLogoFileName` varchar(255) DEFAULT NULL,
  modify `SponsorLogoFileName` varchar(255) DEFAULT NULL,
  modify `LastEditedDate` date DEFAULT NULL,
  modify `UserName` varchar(255) NOT NULL,
  PRIMARY KEY (`UserID`),
  UNIQUE KEY `UserID` (`UserID`),
  KEY `UserProfiles_UserGuid_fkey` (`UserGuid`),
  CONSTRAINT `UserProfiles_UserGuid_fkey` FOREIGN KEY (`UserGuid`) REFERENCES `users` (`PKID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1$$

delimiter $$

alter TABLE `users` (
  modify `PKID` varchar(255) NOT NULL DEFAULT '',
  modify `Username` varchar(255) NOT NULL DEFAULT '',
  modify `ApplicationName` varchar(255) NOT NULL DEFAULT '',
  modify `Email` varchar(128) DEFAULT NULL,
  modify `Comment` varchar(255) DEFAULT NULL,
  modify `Password` varchar(128) NOT NULL DEFAULT '',
  modify `FailedPasswordAttemptWindowStart` datetime DEFAULT NULL,
  modify `PasswordQuestion` varchar(255) DEFAULT NULL,
  modify `IsLockedOut` tinyint(1) DEFAULT '0',
  modify `PasswordAnswer` varchar(255) DEFAULT NULL,
  modify `FailedPasswordAnswerAttemptCount` int(8) DEFAULT '0',
  modify `FailedPasswordAttemptCount` int(8) DEFAULT '0',
  modify `IsApproved` tinyint(1) NOT NULL DEFAULT '0',
  modify `FailedPasswordAnswerAttemptWindowStart` datetime DEFAULT NULL,
  modify `LastActivityDate` datetime DEFAULT NULL,
  modify `IsOnLine` tinyint(1) DEFAULT '0',
  modify `CreationDate` datetime DEFAULT NULL,
  modify `LastPasswordChangedDate` datetime DEFAULT NULL,
  modify `LastLockedOutDate` datetime DEFAULT NULL,
  modify `LastLoginDate` datetime DEFAULT NULL,
  PRIMARY KEY (`PKID`),
  UNIQUE KEY `PKID` (`PKID`),
  KEY `PKID_2` (`PKID`),
  KEY `usr` (`Username`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1$$

delimiter $$

alter TABLE `usersinroles` (
  modify `Username` varchar(255) NOT NULL DEFAULT '',
  modify `Rolename` varchar(255) NOT NULL DEFAULT '',
  modify `ApplicationName` varchar(255) NOT NULL DEFAULT '',
  PRIMARY KEY (`Username`,`Rolename`,`ApplicationName`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1$$

