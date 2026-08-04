		MERGE INTO RESOURCEKEYMASTER l
USING (
    select distinct dc.designcomponentfamilyid,rkm.RESOURCEKEYMASTERID from RESOURCEKEYMASTER rkm
	left join lcmengineering lcm on lcm.opcoid = rkm.opcoid and lcm.designcomponentfamilyid = rkm.dcfid
left join designcomponents dc on dc.designcomponentid = lcm.designcomponentid
where RESOURCEKEYMASTERID in (
select rk.RESOURCEKEYMASTERID
from designcomponentfamilies dcf
Left join designcomponents dc on dc.designcomponentfamilyid = dcf.designcomponentfamilyid 
left join RESOURCEKEYMASTER rk on rk.dcfid = dcf.designcomponentfamilyid
where dc.designcomponentid is  null)
) src
ON (l.RESOURCEKEYMASTERID = src.RESOURCEKEYMASTERID)
WHEN MATCHED THEN
    UPDATE SET l.dcfid = src.designcomponentfamilyid;
	-- MERGE INTO dcflifecycle l
-- USING (
    -- select distinct dc1.designcomponentfamilyid,dcfl.dcflifecycleid from dcflifecycle dcfl
-- left join designcomponents dc1 on dc1.designcomponentid = dcfl.dcid
-- where dcflifecycleid in (
-- select dcfl.dcflifecycleid
-- from designcomponentfamilies dcf
-- Left join designcomponents dc on dc.designcomponentfamilyid = dcf.designcomponentfamilyid 
-- left join dcflifecycle dcfl on dcfl.dcfid = dcf.designcomponentfamilyid
-- where dc.designcomponentid is  null)
-- ) src
-- ON (l.dcflifecycleid = src.dcflifecycleid)
-- WHEN MATCHED THEN
    -- UPDATE SET l.dcfid = src.designcomponentfamilyid;

			MERGE INTO dcflifecycle l
USING (
    select distinct dc_l.designcomponentfamilyid,dcfl.dcflifecycleid from dcflifecycle dcfl
join networkelementsasplanned asset on asset.HWRESOURCEKEY = dcfl.RESOURCEKEY or asset.SWRESOURCEKEY = dcfl.RESOURCEKEY
join designcomponents dc_l on dc_l.designcomponentid = asset.designcomponentid
where dcflifecycleid in (
select dcfl.dcflifecycleid
from designcomponentfamilies dcf
Left join designcomponents dc on dc.designcomponentfamilyid = dcf.designcomponentfamilyid 
left join dcflifecycle dcfl on dcfl.dcfid = dcf.designcomponentfamilyid
where dc.designcomponentid is  null)
) src
ON (l.dcflifecycleid = src.dcflifecycleid)
WHEN MATCHED THEN
    UPDATE SET l.dcfid = src.designcomponentfamilyid;

		MERGE INTO dcflifecycle l
USING (
    select distinct dc_l.designcomponentfamilyid,dcfl.dcflifecycleid from dcflifecycle dcfl
left join designcomponents dc on dc.designcomponentid = dcfl.dcid
left join lcmengineering lcm on lcm.designcomponentfamilyid = dcfl.dcfid
left join designcomponents dc_l on dc_l.designcomponentid = lcm.designcomponentid
where dcflifecycleid in (
select dcfl.dcflifecycleid
from designcomponentfamilies dcf
Left join designcomponents dc on dc.designcomponentfamilyid = dcf.designcomponentfamilyid 
left join dcflifecycle dcfl on dcfl.dcfid = dcf.designcomponentfamilyid
where dc.designcomponentid is  null)
) src
ON (l.dcflifecycleid = src.dcflifecycleid)
WHEN MATCHED THEN
    UPDATE SET l.dcfid = src.designcomponentfamilyid;
 
 
 
		MERGE INTO designaspects l
USING (
    select distinct dc.designcomponentfamilyid,da.id,lcm.lcmengineeringid from designaspects da
join lcmengineering lcm on lcm.opcoid = da.opcoid and lcm.designcomponentfamilyid = da.designcomponentfamilyid
join designcomponents dc on dc.designcomponentid = lcm.designcomponentid
where id in (
select da.id
from designcomponentfamilies dcf
Left join designcomponents dc on dc.designcomponentfamilyid = dcf.designcomponentfamilyid 
left join designaspects da on da.designcomponentfamilyid = dcf.designcomponentfamilyid
where dc.designcomponentid is  null)
) src
ON (l.id = src.id)
WHEN MATCHED THEN
    UPDATE SET l.designcomponentfamilyid = src.designcomponentfamilyid;
 
MERGE INTO lcmengineering l
USING (
    select distinct dc.designcomponentfamilyid,lcm.lcmengineeringid from lcmengineering lcm
join designcomponents dc on dc.designcomponentid = lcm.designcomponentid
where lcmengineeringid in (
select lcm.lcmengineeringid
from designcomponentfamilies dcf
Left join designcomponents dc on dc.designcomponentfamilyid = dcf.designcomponentfamilyid 
left join lcmengineering lcm on lcm.DESIGNCOMPONENTFAMILYID = dcf.DESIGNCOMPONENTFAMILYID
where dc.designcomponentid is  null)
) src
ON (l.lcmengineeringid = src.lcmengineeringid)
WHEN MATCHED THEN
    UPDATE SET l.designcomponentfamilyid = src.designcomponentfamilyid;

 
		MERGE INTO plannedactivities l
USING (
    select distinct dc.designcomponentfamilyid,pa.plannedactivityid from plannedactivities pa
join designcomponents dc on dc.designcomponentid = pa.designcomponentid
where plannedactivityid in (
select pa.plannedactivityid
from designcomponentfamilies dcf
Left join designcomponents dc on dc.designcomponentfamilyid = dcf.designcomponentfamilyid 
left join plannedactivities pa on pa.designcomponentfamilyid = dcf.designcomponentfamilyid
where dc.designcomponentid is  null)
) src
ON (l.plannedactivityid = src.plannedactivityid)
WHEN MATCHED THEN
    UPDATE SET l.designcomponentfamilyid = src.designcomponentfamilyid;
 

		MERGE INTO DAPLANNEDACTIVITYDCF l
USING (
    select distinct pa.designcomponentfamilyid,rkm.DAPLANNEDACTIVITYDCFID from DAPLANNEDACTIVITYDCF rkm
left join PLANNEDACTIVITies pa on pa.PLANNEDACTIVITYID = rkm.PLANNEDACTIVITYID
where DAPLANNEDACTIVITYDCFID in (
select rk.DAPLANNEDACTIVITYDCFID 
from designcomponentfamilies dcf
Left join designcomponents dc on dc.designcomponentfamilyid = dcf.designcomponentfamilyid 
left join DAPLANNEDACTIVITYDCF rk on rk.designcomponentfamilyid = dcf.designcomponentfamilyid
where dc.designcomponentid is  null)
) src
ON (l.DAPLANNEDACTIVITYDCFID = src.DAPLANNEDACTIVITYDCFID)
WHEN MATCHED THEN
    UPDATE SET l.designcomponentfamilyid = src.designcomponentfamilyid;

Create table dcf_backup as select * from designcomponentfamilies;
-- delete from designcomponentfamilies where designcomponentfamilyid in (select dcf.designcomponentfamilyid
-- from designcomponentfamilies dcf
-- Left join designcomponents dc on dc.designcomponentfamilyid = dcf.designcomponentfamilyid 
-- where dc.designcomponentid is  null);

Create table DA_backup as select * from designaspects;
-- delete from designaspects where designcomponentfamilyid in (select dcf.designcomponentfamilyid
-- from designcomponentfamilies dcf
-- Left join designcomponents dc on dc.designcomponentfamilyid = dcf.designcomponentfamilyid 
-- Left join designaspects da on da.designcomponentfamilyid = dcf.designcomponentfamilyid
-- left join lcmengineering lcm on lcm.opcoid = da.opcoid and lcm.designcomponentfamilyid= da.designcomponentfamilyid
-- left join plannedactivities pa on pa.designaspectid = da.id
-- where dc.designcomponentid is  null and pa.plannedactivityid is null and lcm.lcmengineeringid is null);
 
 
MERGE INTO designaspects l
USING (
    select distinct dc.designcomponentfamilyid,da.id,lcm.lcmengineeringid from designaspects da
left join lcmengineering lcm on lcm.opcoid = da.opcoid and lcm.designcomponentfamilyid = da.designcomponentfamilyid
left join designcomponents dc on dc.designcomponentid = lcm.designcomponentid
where id in (
select da.id
from designcomponentfamilies dcf
Left join designcomponents dc on dc.designcomponentfamilyid = dcf.designcomponentfamilyid 
left join designaspects da on da.designcomponentfamilyid = dcf.designcomponentfamilyid
where dc.designcomponentid is  null)
) src
ON (l.id = src.id)
WHEN MATCHED THEN
    UPDATE SET l.deleted = '1';
 
		MERGE INTO designcomponentfamilies l
USING (
    select distinct dcf.designcomponentfamilyid,dcf.implementation,dcf.deleted from designcomponentfamilies dcf
where  designcomponentfamilyid in (
select dcf.designcomponentfamilyid 
from designcomponentfamilies dcf
Left join designcomponents dc on dc.designcomponentfamilyid = dcf.designcomponentfamilyid 
where dc.designcomponentid is  null)
) src
ON (l.designcomponentfamilyid = src.designcomponentfamilyid)
WHEN MATCHED THEN
    UPDATE SET l.implementation = '0',l.deleted = '1';