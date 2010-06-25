
CREATE TABLE `personalization` (
  `username` varchar(255) default NULL,
  `path` varchar(255) default NULL,
  `applicationname` varchar(255) default NULL,
  `personalizationblob` blob
);

CREATE TABLE `profiles` (
  `UniqueID` int(8) NOT NULL auto_increment,
  `Username` varchar(255) NOT NULL default '',
  `ApplicationName` varchar(255) NOT NULL default '',
  `IsAnonymous` tinyint(1) default '0',
  `LastActivityDate` datetime default NULL,
  `LastUpdatedDate` datetime default NULL,
  PRIMARY KEY  (`UniqueID`),
  UNIQUE KEY `PKProfiles` (`Username`,`ApplicationName`),
  UNIQUE KEY `PKID` (`UniqueID`)
);

CREATE TABLE `roles` (
  `Rolename` varchar(255) NOT NULL default '',
  `ApplicationName` varchar(255) NOT NULL default '',
  PRIMARY KEY  (`Rolename`,`ApplicationName`)
);

CREATE TABLE `sitemap` (
  `ID` int(11) NOT NULL auto_increment,
  `ApplicationName` varchar(255) NOT NULL default '',
  `Title` varchar(255) default NULL,
  `Description` text,
  `Url` text,
  `Roles` text,
  `Parent` int(11) default NULL,
  PRIMARY KEY  (`ID`)
);

CREATE TABLE `users` (
  `PKID` varchar(255) NOT NULL default '',
  `Username` varchar(255) NOT NULL default '',
  `ApplicationName` varchar(255) NOT NULL default '',
  `Email` varchar(128) default NULL,
  `Comment` varchar(255) default NULL,
  `Password` varchar(128) NOT NULL default '',
  `FailedPasswordAttemptWindowStart` datetime default NULL,
  `PasswordQuestion` varchar(255) default NULL,
  `IsLockedOut` tinyint(1) default '0',
  `PasswordAnswer` varchar(255) default NULL,
  `FailedPasswordAnswerAttemptCount` int(8) default '0',
  `FailedPasswordAttemptCount` int(8) default '0',
  `IsApproved` tinyint(1) NOT NULL default '0',
  `FailedPasswordAnswerAttemptWindowStart` datetime default NULL,
  `LastActivityDate` datetime default NULL,
  `IsOnLine` tinyint(1) default '0',
  `CreationDate` datetime default NULL,
  `LastPasswordChangedDate` datetime default NULL,
  `LastLockedOutDate` datetime default NULL,
  `LastLoginDate` datetime default NULL,
  PRIMARY KEY  (`PKID`),
  UNIQUE KEY `PKID` (`PKID`),
  KEY `PKID_2` (`PKID`),
  KEY `usr` (`Username`)
);

CREATE TABLE `usersinroles` (
  `Username` varchar(255) NOT NULL default '',
  `Rolename` varchar(255) NOT NULL default '',
  `ApplicationName` varchar(255) NOT NULL default '',
  PRIMARY KEY  (`Username`,`Rolename`,`ApplicationName`)
);
