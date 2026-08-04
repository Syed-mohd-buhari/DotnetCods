MERGE INTO LCMENGINEERING trg using 
(
    select t1.rowid as rid , t3.resourcekey as updatevalue
    from LCMENGINEERING t1
    join designcomponents t2 ON t1.designcomponentid = t2.designcomponentid
    join resourcekeymaster t3 ON t3.DCFID = t2.DESIGNCOMPONENTFAMILYID AND t3.OpcoId = t1.opcoId
    and t3.RESOURCETYPESID = (select RESOURCETYPESID from RESOURCETYPES where NAME = 'LCM')
) src
on (trg.rowid = src.rid)
WHEN MATCHED THEN UPDATE
     SET trg.resourcekey = updatevalue || '_00';
Commit;

MERGE INTO NETWORKELEMENTSASPLANNED trg using 
(
    select t1.rowid as rid , t3.resourcekey as updatevalue
    from NETWORKELEMENTSASPLANNED t1
    join designcomponents t2 ON t1.designcomponentid = t2.designcomponentid
    join resourcekeymaster t3 ON t3.DCFID = t2.DESIGNCOMPONENTFAMILYID AND t3.OpcoId = t1.opcoId and t3.elementname = t1.elementname
    and t3.RESOURCETYPESID = (select RESOURCETYPESID from RESOURCETYPES where NAME = 'SWASSET')
) src
on (trg.rowid = src.rid)
WHEN MATCHED THEN UPDATE
    SET trg.SWRESOURCEKEY = updatevalue;
Commit;

MERGE INTO NETWORKELEMENTSASPLANNED trg using 
(
    select t1.rowid as rid , t3.resourcekey as updatevalue
    from NETWORKELEMENTSASPLANNED t1
    join designcomponents t2 ON t1.designcomponentid = t2.designcomponentid
    join resourcekeymaster t3 ON t3.DCFID = t2.DESIGNCOMPONENTFAMILYID AND t3.OpcoId = t1.opcoId and t3.elementname = t1.elementname
    and t3.RESOURCETYPESID = (select RESOURCETYPESID from RESOURCETYPES where NAME = 'HWASSET')
    and t2.designcomponentid in (Select ds.designcomponentid from designcomponents ds
    inner join SystemTypes st on ds.systemtypeid = st.systemtypeid
    inner join Systemtypesmajorhardwarebuilds mh on mh.systemtypeid = st.systemtypeid
    Inner join MajorHardwareBuilds mb on mh.majorhardwareid = mb.majorhardwareid
    inner join buildconstructions b on b.buildconstructionid = mb.buildconstructionid
    where buildconstruction not in ('BLUEPRINT NFVI','OTHER NFVI','BLUEPRINT NFCI','OTHER NFCI'))
) src
on (trg.rowid = src.rid)
WHEN MATCHED THEN UPDATE
    SET trg.HWRESOURCEKEY = updatevalue;
Commit;

MERGE INTO NETWORKELEMENTSASPLANNED trg using 
(
    select t1.rowid as rid , t3.resourcekey as updatevalue
    from NETWORKELEMENTSASPLANNED t1
    join designcomponents t2 ON t1.designcomponentid = t2.designcomponentid
    join resourcekeymaster t3 ON t3.DCFID = t2.DESIGNCOMPONENTFAMILYID AND t3.OpcoId = t1.opcoId and t3.elementname is null 
    and t3.RESOURCETYPESID = (select RESOURCETYPESID from RESOURCETYPES where NAME = 'HWASSET')
    and t2.designcomponentid in (Select ds.designcomponentid from designcomponents ds
    inner join SystemTypes st on ds.systemtypeid = st.systemtypeid
    inner join Systemtypesmajorhardwarebuilds mh on mh.systemtypeid = st.systemtypeid
    Inner join MajorHardwareBuilds mb on mh.majorhardwareid = mb.majorhardwareid
    inner join buildconstructions b on b.buildconstructionid = mb.buildconstructionid
    where buildconstruction in ('BLUEPRINT NFVI','OTHER NFVI','BLUEPRINT NFCI','OTHER NFCI'))
) src
on (trg.rowid = src.rid)
WHEN MATCHED THEN UPDATE
    SET trg.HWRESOURCEKEY = updatevalue;
Commit;

MERGE INTO identitiesasis trg using 
(
    select distinct t1.rowid as rid, t4.resourcekey as updatevalue
    from identitiesasis t1
    join NETWORKELEMENTSASPLANNED t2 on AssetId = NetworkElementAsPlannedId and CategoryId not in (Select ID from Categories where Description = 'IP Address')
    join resourcekeymaster t4 ON t2.elementname = t4.elementname AND t4.OpcoId = t2.opcoId
    join designcomponents t5 on t4.dcfid = t5.DESIGNCOMPONENTFAMILYID
    and t4.RESOURCETYPESID = (select RESOURCETYPESID from RESOURCETYPES where NAME = 'IDENTITY')
) src
on (trg.rowid = src.rid)
WHEN MATCHED THEN UPDATE
    SET trg.resourcekey = updatevalue;
Commit;

MERGE INTO identitiesasis trg using 
(
    select distinct t1.rowid as rid, t4.resourcekey as updatevalue
    from identitiesasis t1
    join NETWORKELEMENTSASPLANNED t2 on AssetId = NetworkElementAsPlannedId and CategoryId in (Select ID from Categories where Description = 'IP Address')
    join resourcekeymaster t4 ON t2.elementname = t4.elementname AND t4.OpcoId = t2.opcoId
    join designcomponents t5 on t4.dcfid = t5.DESIGNCOMPONENTFAMILYID
    and t4.RESOURCETYPESID = (select RESOURCETYPESID from RESOURCETYPES where NAME = 'IDENTITY')
) src
on (trg.rowid = src.rid)
WHEN MATCHED THEN UPDATE
    SET trg.resourcekey = updatevalue;
Commit;
