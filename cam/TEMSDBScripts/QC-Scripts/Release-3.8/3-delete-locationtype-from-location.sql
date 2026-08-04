

--Befor run this script we must run script to migrate data from location type filed in loication to the new bridge table locationDeploymetType

insert into locationdeploymenttypes (LOCATIONID, DEPLOYMENTTYPEID, creationuser, modificationuser)
select locations.locationid, locations.locationtypeid,177,177 from locations

--------------------------------------------------------------
-- drop column LOCATIONTYPEID from "LOCATIONS"

alter table "CAMSDBUSER"."LOCATIONS" drop column LOCATIONTYPEID;

commit
-----------------------------------------------------------------