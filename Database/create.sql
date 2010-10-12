DROP DATABASE IF EXISTS `yafnet`;
CREATE DATABASE `yafnet`;
DROP TABLE IF EXISTS `yafnet`.`users`;
CREATE TABLE  `yafnet`.`users` (
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

DROP TABLE IF EXISTS `yafnet`.`openid`;
CREATE TABLE  `yafnet`.`openid` (
  `openId_url` varchar(256) CHARACTER SET utf8 DEFAULT NULL,
  `user_id` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
CREATE TABLE  `yafnet`.`personalization` (
  `username` varchar(255) DEFAULT NULL,
  `path` varchar(255) DEFAULT NULL,
  `applicationname` varchar(255) DEFAULT NULL,
  `personalizationblob` blob
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE  `yafnet`.`profiles` (
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

CREATE TABLE  `yafnet`.`roles` (
  `Rolename` varchar(255) NOT NULL DEFAULT '',
  `ApplicationName` varchar(255) NOT NULL DEFAULT '',
  PRIMARY KEY (`Rolename`,`ApplicationName`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE  `yafnet`.`sitemap` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `ApplicationName` varchar(255) NOT NULL DEFAULT '',
  `Title` varchar(255) DEFAULT NULL,
  `Description` text,
  `Url` text,
  `Roles` text,
  `Parent` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE  `yafnet`.`usersinroles` (
  `Username` varchar(255) NOT NULL DEFAULT '',
  `Rolename` varchar(255) NOT NULL DEFAULT '',
  `ApplicationName` varchar(255) NOT NULL DEFAULT '',
  PRIMARY KEY (`Username`,`Rolename`,`ApplicationName`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE  `yafnet`.`userprofiles` (
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
DELIMITER $$

DROP PROCEDURE IF EXISTS `yafnet`.`OpenId_DeleteUserOpenIdLink`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `yafnet`.`OpenId_DeleteUserOpenIdLink`(
openId_Url nvarchar(256),
userId varchar(256))
delete from yafnet.OpenId where (yafnet.openId_url=openId_Url)
or (yafnet.user_id=userId) $$

DELIMITER ;
DELIMITER $$

DROP PROCEDURE IF EXISTS `yafnet`.`OpenId_GetOpenIdsByUserId`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `yafnet`.`OpenId_GetOpenIdsByUserId`(userId varchar(256))
select openId_url from openid where (user_id=userId) $$

DELIMITER ;
DELIMITER $$

DROP PROCEDURE IF EXISTS `yafnet`.`OpenId_GetUserIdByOpenld`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `yafnet`.`OpenId_GetUserIdByOpenld`(openIdurl varchar(256))
select user_id from yafnet.openid where (openId_url=openIdurl) $$

DELIMITER ;
DELIMITER $$

DROP PROCEDURE IF EXISTS `yafnet`.`OpenId_LinkUserWithOpenId`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `yafnet`.`OpenId_LinkUserWithOpenId`(
openId_Url nvarchar(256),
userId varchar(256))
insert into yafnet.OpenId (openId_url,user_id) values(openId_Url, userId) $$

DELIMITER ;
DELIMITER $$

DROP PROCEDURE IF EXISTS `yafnet`.`OpenId_Membership_GetAllUsers`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `yafnet`.`OpenId_Membership_GetAllUsers`(
    ApplicationName       nvarchar(256))
SELECT u.UserName,o.openId_url, u.Email, u.PasswordQuestion, u.Comment, u.IsApproved,
            u.CreationDate,
            u.LastLoginDate,
            u.LastActivityDate,
            u.LastPasswordChangedDate,
            u.PKID, u.IsLockedOut,
            u.LastLockedOutDate
    FROM   yafnet.Users u

	inner join yafnet.openid o on o.User_id=u.PKID

	WHERE  u.ApplicationName = ApplicationName

    ORDER BY u.UserName $$

DELIMITER ;
DELIMITER $$

DROP PROCEDURE IF EXISTS `yafnet`.`StrongEye_OpenID_Membership_GetAllUsers`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `yafnet`.`StrongEye_OpenID_Membership_GetAllUsers`(
    ApplicationName       nvarchar(256))
SELECT u.UserName, m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
            m.CreateDate,
            m.LastLoginDate,
            u.LastActivityDate,
            m.LastPasswordChangedDate,
            u.UserId, m.IsLockedOut,
            m.LastLockoutDate
    FROM   tes2.aspnet_Membership m, yafnet.aspnet_Users u
    WHERE  u.ApplicationId = ApplicationId AND u.UserId = m.UserId
    ORDER BY u.UserName $$

DELIMITER ;
DELIMITER $$

DROP PROCEDURE IF EXISTS `yafnet`.`StrongEye_OpenID_Membership_GetUserByName`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `yafnet`.`StrongEye_OpenID_Membership_GetUserByName`(
    ApplicationName      nvarchar(256),
    UserName             nvarchar(256),
    CurrentTimeUtc       datetime,
    UpdateLastActivity   bit)
SELECT u.Username, m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
                m.CreateDate, m.LastLoginDate, u.LastActivityDate, m.LastPasswordChangedDate,
                u.UserId, m.IsLockedOut,m.LastLockoutDate
        FROM    yafnet.aspnet_Applications a, yafnet.aspnet_Users u, yafnet.aspnet_Membership m
        WHERE    LOWER(ApplicationName) = a.LoweredApplicationName AND
                u.ApplicationId = a.ApplicationId    AND
                LOWER(UserName) = u.LoweredUserName AND u.UserId = m.UserId
        LIMIT 1 $$

DELIMITER ;
DELIMITER $$

DROP PROCEDURE IF EXISTS `yafnet`.`StrongEye_OpenID_Membership_GetUserByUserId`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `yafnet`.`StrongEye_OpenID_Membership_GetUserByUserId`(UserId varchar(256),CurrentTimeUtc datetime,UpdateLastActivity   bit)
SELECT u.Username, m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
                m.CreateDate, m.LastLoginDate, @CurrentTimeUtc, m.LastPasswordChangedDate,
                u.UserId, m.IsLockedOut,m.LastLockoutDate
    FROM    dbo.aspnet_Users u, dbo.aspnet_Membership m
    WHERE   @UserId = u.UserId AND u.UserId = m.UserId $$

DELIMITER ;