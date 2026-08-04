--------------------------------------------------------
-- Create Backup Table For plannedactivities
--------------------------------------------------------
CREATE TABLE plannedactivities08122025_BKP AS 
SELECT pa.plannedactivityid, pa.designcomponentid ,pa.designcomponentfamilyid,
pa.LCMENGINEERINGID,pa.NETWORKELEMENTASPLANNEDID,pa.DESIGNASPECTID
FROM plannedactivities pa;
commit;

--select * from plannedactivities_BKP;

----------------------------------------------------------------------------------------------------------------
-- Create Temp Data -for respective pa id -insert dc or dcf id from lcm or da or asset table ( lcmid or asset id or da id)  in plannedactivities table 
----------------------------------------------------------------------------------------------------------------

CREATE TABLE TEMPPlannedActivity AS
select pa.plannedactivityid as PAID,pa.LCMENGINEERINGID as LCMID,lcme.designcomponentid as LCMDCID,pa.NETWORKELEMENTASPLANNEDID AS ASSETID,
ass.designcomponentid as ASSETDCID,pa.DESIGNASPECTID as DAID,da.designcomponentfamilyid as DADCFID
from plannedactivities pa
left join lcmengineering lcme on  pa.LCMENGINEERINGID =lcme.LCMENGINEERINGID 
left join NETWORKELEMENTSASPLANNED ass ON pa.NETWORKELEMENTASPLANNEDID =ass.NETWORKELEMENTASPLANNEDID
left join designaspects da on pa.designaspectid =da.id
where pa.designcomponentid is null and pa.designcomponentfamilyid is null ;
commit;

--select * from TEMPPlannedActivity;

----------------------------------------------------------------------------------------------------------------
-- Updating dc or dcf id in plannedactivities with respective pa id from Temp Data table
----------------------------------------------------------------------------------------------------------------

MERGE INTO plannedactivities pa
using TEMPPlannedActivity temppa
ON (temppa.paid=pa.plannedactivityid)
WHEN MATCHED THEN UPDATE SET pa.designcomponentid = 
case 
	when temppa.lcmdcid is not null  and pa.designcomponentid is null then temppa.lcmdcid 
	when temppa.assetdcid is not null and pa.designcomponentid is null then temppa.assetdcid
	else pa.designcomponentid
end,
pa.designcomponentfamilyid = 
case
	when temppa.dadcfid is not null  and pa.designcomponentfamilyid is null then temppa.dadcfid
	else pa.designcomponentfamilyid
end;
commit;                                        

drop table TEMPPlannedActivity;
commit;

----------------------------------------------------------------------------------------------------------------
-- SELECT pa.plannedactivityid, pa.designcomponentid ,pa.designcomponentfamilyid,
-- pa.LCMENGINEERINGID,pa.NETWORKELEMENTASPLANNEDID,pa.DESIGNASPECTID,tpa.LCMDCID,tpa.ASSETDCID,tpa.DADCFID
-- FROM plannedactivities pa  inner join TEMPPlannedActivity tpa on tpa.paid =pa.plannedactivityid

-- select * from plannedactivities pa where pa.designcomponentid is null and pa.designcomponentfamilyid is null ; 
----------------------------------------------------------------------------------------------------------------





