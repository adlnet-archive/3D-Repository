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


USE 3dr;



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


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;