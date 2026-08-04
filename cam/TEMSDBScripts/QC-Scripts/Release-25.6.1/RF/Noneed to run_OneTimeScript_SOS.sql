------------- Create TEMPORARY  COMPONENTSOFTWAREBUILDS Table for Furture Refrence
create table TEMP_COMPONENTSOFTWAREBUILDS
(
COMPONENTSOFTWAREBUILDID  NUMBER(19) NOT NULL,
ORGEQPMANUFACTURERID   NUMBER(5) NOT NULL ,
PRODUCTNAMEID   NUMBER(20)   )

INSERT INTO TEMP_COMPONENTSOFTWAREBUILDS (COMPONENTSOFTWAREBUILDID, ORGEQPMANUFACTURERID, PRODUCTNAMEID)
SELECT COMPONENTSOFTWAREBUILDID, ORGEQPMANUFACTURERID, PRODUCTNAMEID FROM componentsoftwarebuilds;

commit;

-------------------------------------------- Insert COMPONENTMANUFACTURER from COMPONENTSOFTWAREBUILDS table

INSERT INTO COMPONENTMANUFACTURERS (COMPONENTMANUFACTURER, COMPONENTNAME)
SELECT  oem.ORIGINALEQUIPMENTMANUFACTURER as MANUFACTURERNAME,
pn.DESCRIPTION as PRODUCTNAME
FROM COMPONENTSOFTWAREBUILDS csb
JOIN ORIGINALEQUIPMENTMANUFACTURERS oem
ON csb.ORGEQPMANUFACTURERID = oem.ORGEQPMANUFACTURERID
JOIN PRODUCTNAME pn
ON csb.PRODUCTNAMEID = pn.PRODUCTNAMEID;

commit;

-------------------------------------------- Update COMPONENTMANUFACTURERID  value for COMPONENTMANUFACTURER from COMPONENTSOFTWAREBUILDS  PRODUCTNAME and ORIGINALEQUIPMENTMANUFACTURER table
MERGE INTO COMPONENTSOFTWAREBUILDS csb
USING (
    SELECT *
    FROM (
        SELECT 
            csb.ROWID AS csb_rowid,
            cm.COMPONENTMANUFACTURERID,
            ROW_NUMBER() OVER (PARTITION BY csb.ROWID ORDER BY cm.COMPONENTMANUFACTURERID) AS rn
        FROM COMPONENTSOFTWAREBUILDS csb
        JOIN ORIGINALEQUIPMENTMANUFACTURERS oem
            ON csb.ORGEQPMANUFACTURERID = oem.ORGEQPMANUFACTURERID
        JOIN PRODUCTNAME pn
            ON csb.PRODUCTNAMEID = pn.PRODUCTNAMEID
        JOIN COMPONENTMANUFACTURERS cm
            ON cm.COMPONENTMANUFACTURER = oem.ORIGINALEQUIPMENTMANUFACTURER
           AND cm.COMPONENTNAME = pn.DESCRIPTION
    )
    WHERE rn = 1
) matched_data
ON (csb.ROWID = matched_data.csb_rowid)
WHEN MATCHED THEN
    UPDATE SET csb.COMPONENTMANUFACTURERID = matched_data.COMPONENTMANUFACTURERID;

commit;
 
 -------------------------------------------- generate Dummy bag based on LCM opco and DC 

--INSERt INTO BUILDBAGS (
  --  DESIGNCOMPONENTFAMILYID, 
 --   OPCOID, 
 --   BAGDESCRIPTION, 
 --   BAGVERSION,
 --   CREATIONUSER,
 --   MODIFICATIONUSER
--)
SELECT  dc.DESIGNCOMPONENTFAMILYID,
    MAX(lcm.OPCOID),
    'NotDefined-NotDefined-Empty',
    '1.0' ,1 ,1
FROM LCMENGINEERING lcm
JOIN DESIGNCOMPONENTS dc 
ON dc.DESIGNCOMPONENTID = lcm.DESIGNCOMPONENTID
--where dc.DESIGNCOMPONENTFAMILYID = 2042
GROUP BY dc.DESIGNCOMPONENTFAMILYID;

--commit;

--------------------------- Update Bagid based on Opco and DC in LCM table
--MERGE INTO LCMENGINEERING lcmtarget
--USING (
    SELECT *
    FROM (
        SELECT 
            lcm.ROWID AS lcm_rowid,
            bag.BUILDBAGID,
            ROW_NUMBER() OVER (
                PARTITION BY lcm.ROWID
                ORDER BY bag.BUILDBAGID  
            ) AS rn
        FROM LCMENGINEERING lcm
        JOIN DESIGNCOMPONENTS dc
            ON dc.DESIGNCOMPONENTID = lcm.DESIGNCOMPONENTID
        JOIN BUILDBAGS bag
            ON dc.DESIGNCOMPONENTFAMILYID = bag.DESIGNCOMPONENTFAMILYID
        --WHERE dc.DESIGNCOMPONENTFAMILYID = 2042
    )
  --  WHERE rn = 1
--) matched_data
--ON (lcmtarget.ROWID = matched_data.lcm_rowid)
--WHEN MATCHED THEN
    --UPDATE SET lcmtarget.BUILDBAGID = matched_data.BUILDBAGID;
     

--commit;
update buildbags set bagdescription = 'Empty' where buildbagid = (select buildbagid from buildbags where bagdescription = 'NotDefined-NotDefined-Empty' and bagversion = '1.0');

update lcmengineering set buildbagid = (select buildbagid from buildbags where bagdescription = 'Empty' and bagversion = '1.0');

update plannedactivities set buildbagid = (select buildbagid from buildbags where bagdescription = 'Empty' and bagversion = '1.0');

update networkelementsasplanned set buildbagid = (select buildbagid from buildbags where bagdescription = 'Empty' and bagversion = '1.0');

update dcflifecycle set bagname = 'Empty' where bagname = 'NotDefined-NotDefined-Empty';

--select * from dcflifecycle where bagname = 'NotDefined-NotDefined-Empty';