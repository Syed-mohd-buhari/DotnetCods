insert into resourcekeymaster (RESOURCETYPESID, OPCOID, DCFID, CREATIONUSER) 
select (select RESOURCETYPESID from RESOURCETYPES where NAME = 'LCM') as resourcetypeid, opcoid,  DESIGNCOMPONENTFAMILYID, '1' as CREATIONUSER from
(select opcoid, ds.DESIGNCOMPONENTFAMILYID
from lcmengineering lcm join DESIGNCOMPONENTS ds
on lcm.designcomponentid = ds.designcomponentid
group by opcoid, ds.DESIGNCOMPONENTFAMILYID);
commit;