DELIMITER $$

DROP PROCEDURE IF EXISTS `test`.`AddMissingTexture`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `test`.`AddMissingTexture`(newfilename varchar(45),
newtype varchar(45), newuvset int(10), newcontentobjectid varchar(400), newrevision int(10))
BEGIN
      INSERT INTO `test`.`missingtextures`(Filename,
      Type,UVSet,PID,Revision)
      values(newfilename,newtype,newuvset,newcontentobjectid,newrevision);
END $$

DELIMITER ;

DELIMITER $$

DROP PROCEDURE IF EXISTS `test`.`AddSupportingFile`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `test`.`AddSupportingFile`(newfilename varchar(45),
newdescription varchar(400),newcontentobjectid varchar(400))
BEGIN
      INSERT INTO `test`.`supportingfiles`(Filename,
      Description,PID)
      values(newfilename,newdescription,newcontentobjectid);
END $$

DELIMITER ;

DELIMITER $$

DROP PROCEDURE IF EXISTS `test`.`AddTextureReference`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `test`.`AddTextureReference`(newfilename varchar(45),
newtype varchar(45), newuvset int(10), newcontentobjectid varchar(400), newrevision int(10))
BEGIN
      INSERT INTO `test`.`texturereferences`(Filename,
      Type,UVSet,PID,Revision)
      values(newfilename,newtype,newuvset,newcontentobjectid,newrevision);
END $$

DELIMITER ;

DELIMITER $$

DROP PROCEDURE IF EXISTS `test`.`AddToCurrentUploads`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `test`.`AddToCurrentUploads`(targetpid varchar(400), targethash varchar (100))
BEGIN
  INSERT INTO `current_uploads` (`pid`, `hash`)
  VALUES (targetpid, targethash);
END $$

DELIMITER ;

DELIMITER $$

DROP PROCEDURE IF EXISTS `test`.`AssociateKeyword`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `test`.`AssociateKeyword`(coid int(10), kid int(10))
BEGIN
                 INSERT INTO `test`.`associatedkeywords`(`ContentObjectId`,`KeywordId`)
                 VALUES (coid,kid);
END $$

DELIMITER ;

DELIMITER $$

DROP PROCEDURE IF EXISTS `test`.`DeleteContentObject`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `test`.`DeleteContentObject`(inpid varchar(400))
BEGIN
        DELETE
        FROM `test`.`contentobjects`
        WHERE PID = inpid;
END $$

DELIMITER ;

DELIMITER $$

DROP PROCEDURE IF EXISTS `test`.`DeleteMissingTexture`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `test`.`DeleteMissingTexture`(inpid varchar(400),infilename varchar(400), inrevision int(10))
BEGIN
        DELETE
        FROM `test`.`missingtextures`
        WHERE PID = inpid AND Filename = infilename AND Revision = inrevision;
END $$

DELIMITER ;

DELIMITER $$

DROP PROCEDURE IF EXISTS `test`.`DeleteSupportingFile`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `test`.`DeleteSupportingFile`(inpid varchar(400),infilename varchar(400))
BEGIN
        DELETE
        FROM `test`.`supportingfiles`
        WHERE PID = inpid AND Filename = infilename;
END $$

DELIMITER ;

DELIMITER $$

DROP PROCEDURE IF EXISTS `test`.`DeleteTextureReference`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `test`.`DeleteTextureReference`(inpid varchar(400),infilename varchar(400), inrevision int(10))
BEGIN
        DELETE
        FROM `test`.`texturereferences`
        WHERE PID = inpid AND Filename = infilename AND Revison = inrevision;
END $$

DELIMITER ;

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
PREPARE STMT FROM "SELECT PID, Title, ScreenShotFileName,ScreenShotFileId, Description
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

DELIMITER $$

DROP PROCEDURE IF EXISTS `test`.`GetMissingTextures`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `test`.`GetMissingTextures`(inpid varchar(400), inrevision int(10))
BEGIN
        SELECT *
        FROM `test`.`missingtextures`
        WHERE PID = inpid AND Revision = inrevision;
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
    PREPARE STMT FROM "SELECT PID, Title, ScreenShotFileName,ScreenShotFileId, Description
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
    PREPARE STMT FROM "SELECT PID, Title, ScreenShotFileName,ScreenShotFileId, Description
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

DROP PROCEDURE IF EXISTS `test`.`GetSupportingFiles`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `test`.`GetSupportingFiles`(inpid varchar(400))
BEGIN
        SELECT *
        FROM `test`.`supportingfiles`
        WHERE pid = inpid;
END $$

DELIMITER ;

DELIMITER $$

DROP PROCEDURE IF EXISTS `test`.`GetTextureReferences`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `test`.`GetTextureReferences`(inpid varchar(400), inrevision int(10))
BEGIN
        SELECT *
        FROM `test`.`texturereferences`
        WHERE PID = inpid AND Revision = inrevision;
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

DROP PROCEDURE IF EXISTS `test`.`IncrementViews`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `test`.`IncrementViews`(targetpid varchar(400))
BEGIN
        UPDATE ContentObjects SET Views = Views+1, LastViewed=NOW()
        WHERE PID =targetpid;
END $$

DELIMITER ;

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
newNumTextures int(10),
newRevisionNumber int(10),
newRequireResubmit TINYINT(1),
newenabled tinyint(1),
newready tinyint(1),
newOriginalFileName nvarchar(400),
newOriginalFileId nvarchar(400))
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

DELIMITER ;

DELIMITER $$

DROP PROCEDURE IF EXISTS `test`.`InsertKeyword`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `test`.`InsertKeyword`(newKeyword varchar(45))
BEGIN
        INSERT INTO keywords(keyword) VALUES(newKeyword);
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

DROP PROCEDURE IF EXISTS `test`.`OpenId_DeleteUserOpenIdLink`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `test`.`OpenId_DeleteUserOpenIdLink`(
openId_Url nvarchar(256),
userId varchar(256))
delete from test2.OpenId where (test2.openId_url=openId_Url)
or (test2.user_id=userId) $$

DELIMITER ;

DELIMITER $$

DROP PROCEDURE IF EXISTS `test`.`OpenId_GetOpenIdsByUserId`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `test`.`OpenId_GetOpenIdsByUserId`(userId varchar(256))
select openId_url from openid where (user_id=userId) $$

DELIMITER ;

DELIMITER $$

DROP PROCEDURE IF EXISTS `test`.`OpenId_GetUserIdByOpenld`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `test`.`OpenId_GetUserIdByOpenld`(openIdurl varchar(256))
select user_id from test2.openid where (openId_url=openIdurl) $$

DELIMITER ;

DELIMITER $$

DROP PROCEDURE IF EXISTS `test`.`OpenId_LinkUserWithOpenId`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `test`.`OpenId_LinkUserWithOpenId`(
openId_Url nvarchar(256),
userId varchar(256))
insert into test2.OpenId (openId_url,user_id) values(openId_Url, userId) $$

DELIMITER ;

DELIMITER $$

DROP PROCEDURE IF EXISTS `test`.`OpenId_Membership_GetAllUsers`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `test`.`OpenId_Membership_GetAllUsers`(
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

DELIMITER ;

DELIMITER $$

DROP PROCEDURE IF EXISTS `test`.`RemoveFromCurrentUploads`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `test`.`RemoveFromCurrentUploads`(targetpid varchar(400))
BEGIN
  DELETE FROM `current_uploads` 
  WHERE pid = targetpid;
END $$

DELIMITER ;

DELIMITER $$

DROP PROCEDURE IF EXISTS `test`.`StrongEye_OpenID_Membership_GetAllUsers`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `test`.`StrongEye_OpenID_Membership_GetAllUsers`(
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

DELIMITER ;

DELIMITER $$

DROP PROCEDURE IF EXISTS `test`.`StrongEye_OpenID_Membership_GetUserByName`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `test`.`StrongEye_OpenID_Membership_GetUserByName`(
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

DELIMITER ;

DELIMITER $$

DROP PROCEDURE IF EXISTS `test`.`StrongEye_OpenID_Membership_GetUserByUserId`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `test`.`StrongEye_OpenID_Membership_GetUserByUserId`(UserId varchar(256),CurrentTimeUtc datetime,UpdateLastActivity   bit)
SELECT u.Username, m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
                m.CreateDate, m.LastLoginDate, @CurrentTimeUtc, m.LastPasswordChangedDate,
                u.UserId, m.IsLockedOut,m.LastLockoutDate
    FROM    dbo.aspnet_Users u, dbo.aspnet_Membership m
    WHERE   @UserId = u.UserId AND u.UserId = m.UserId $$

DELIMITER ;

DELIMITER $$

DROP PROCEDURE IF EXISTS `test`.`UpdateContentObject`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE  `test`.`UpdateContentObject`(newpid nvarchar(400),
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

DELIMITER ;