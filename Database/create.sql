CREATE DATABASE `test2`;
CREATE TABLE  `test2`.`users` (
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

CREATE TABLE  `test2`.`personalization` (
  `username` varchar(255) DEFAULT NULL,
  `path` varchar(255) DEFAULT NULL,
  `applicationname` varchar(255) DEFAULT NULL,
  `personalizationblob` blob
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE  `test2`.`profiles` (
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

CREATE TABLE  `test2`.`roles` (
  `Rolename` varchar(255) NOT NULL DEFAULT '',
  `ApplicationName` varchar(255) NOT NULL DEFAULT '',
  PRIMARY KEY (`Rolename`,`ApplicationName`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE  `test2`.`sitemap` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `ApplicationName` varchar(255) NOT NULL DEFAULT '',
  `Title` varchar(255) DEFAULT NULL,
  `Description` text,
  `Url` text,
  `Roles` text,
  `Parent` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE  `test2`.`usersinroles` (
  `Username` varchar(255) NOT NULL DEFAULT '',
  `Rolename` varchar(255) NOT NULL DEFAULT '',
  `ApplicationName` varchar(255) NOT NULL DEFAULT '',
  PRIMARY KEY (`Username`,`Rolename`,`ApplicationName`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE  `test2`.`userprofiles` (
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
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;