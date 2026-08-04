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
    SET trg.resourcekey = updatevalue;
Commit;