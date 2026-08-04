insert into resourcekeymaster (RESOURCETYPESID, OPCOID, DCFID, CREATIONUSER) 
select distinct (select RESOURCETYPESID from RESOURCETYPES where NAME = 'LCM') as resourcetypeid, opcoid,  DESIGNCOMPONENTFAMILYID, '1' as CREATIONUSER from
(select opcoid, ds.DESIGNCOMPONENTFAMILYID
from lcmengineering lcm join DESIGNCOMPONENTS ds
on lcm.designcomponentid = ds.designcomponentid
group by opcoid, ds.DESIGNCOMPONENTFAMILYID);

insert into resourcekeymaster (RESOURCETYPESID, OPCOID, DCFID, ELEMENTNAME, CREATIONUSER) 
select distinct (select RESOURCETYPESID from RESOURCETYPES where NAME = 'SWASSET') as resourcetypeid, opcoid,  DESIGNCOMPONENTFAMILYID, ELEMENTNAME, '1' as CREATIONUSER from
(select opcoid, ds.DESIGNCOMPONENTFAMILYID, ELEMENTNAME
from NETWORKELEMENTSASPLANNED asset join DESIGNCOMPONENTS ds 
on asset.designcomponentid = ds.designcomponentid
group by opcoid, ds.DESIGNCOMPONENTFAMILYID,ELEMENTNAME);

insert into resourcekeymaster (RESOURCETYPESID, OPCOID, DCFID, ELEMENTNAME, CREATIONUSER) 
select distinct (select RESOURCETYPESID from RESOURCETYPES where NAME = 'HWASSET') as resourcetypeid, opcoid,  DESIGNCOMPONENTFAMILYID, ELEMENTNAME, '1' as CREATIONUSER from
(select opcoid, ds.DESIGNCOMPONENTFAMILYID, ELEMENTNAME
from NETWORKELEMENTSASPLANNED asset join DESIGNCOMPONENTS ds 
on asset.designcomponentid = ds.designcomponentid
inner join SystemTypes st on ds.systemtypeid = st.systemtypeid
inner join Systemtypesmajorhardwarebuilds mh on mh.systemtypeid = st.systemtypeid
Inner join MajorHardwareBuilds mb on mh.majorhardwareid = mb.majorhardwareid
inner join buildconstructions b on b.buildconstructionid = mb.buildconstructionid
where buildconstruction not in ('BLUEPRINT NFVI','OTHER NFVI','BLUEPRINT NFCI','OTHER NFCI')
group by opcoid, ds.DESIGNCOMPONENTFAMILYID,ELEMENTNAME);

insert into resourcekeymaster (RESOURCETYPESID, OPCOID, DCFID, CREATIONUSER) 
select distinct (select RESOURCETYPESID from RESOURCETYPES where NAME = 'HWASSET') as resourcetypeid, opcoid,  DESIGNCOMPONENTFAMILYID, '1' as CREATIONUSER from
(select opcoid, ds.DESIGNCOMPONENTFAMILYID
from NETWORKELEMENTSASPLANNED asset join DESIGNCOMPONENTS ds
on asset.designcomponentid = ds.designcomponentid
inner join SystemTypes st on ds.systemtypeid = st.systemtypeid
inner join Systemtypesmajorhardwarebuilds mh on mh.systemtypeid = st.systemtypeid
Inner join MajorHardwareBuilds mb on mh.majorhardwareid = mb.majorhardwareid
inner join buildconstructions b on b.buildconstructionid = mb.buildconstructionid
where buildconstruction in ('BLUEPRINT NFVI','OTHER NFVI','BLUEPRINT NFCI','OTHER NFCI')
group by opcoid, ds.DESIGNCOMPONENTFAMILYID);


insert into resourcekeymaster (RESOURCETYPESID, OPCOID, DCFID, ELEMENTNAME, CREATIONUSER) 
select distinct (select RESOURCETYPESID from RESOURCETYPES where NAME = 'IDENTITY') as resourcetypeid, opcoid,  DESIGNCOMPONENTFAMILYID, ELEMENTNAME, '1' as CREATIONUSER from
(select opcoid, t3.DESIGNCOMPONENTFAMILYID, ELEMENTNAME 
from identitiesasis t1
join NETWORKELEMENTSASPLANNED t2 on AssetId = NetworkElementAsPlannedId
join designcomponents t3 ON t2.designcomponentid = t3.designcomponentid
group by opcoid, t3.DESIGNCOMPONENTFAMILYID,ELEMENTNAME);

Commit;
