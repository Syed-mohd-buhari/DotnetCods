---- Create NotDefined Product and OEM for Dummy Bag Creation

INSERT INTO ORIGINALEQUIPMENTMANUFACTURERS(ORIGINALEQUIPMENTMANUFACTURER ,CREATIONUSER,MODIFICATIONUSER) VALUES ('NotDefined',1 ,1) ;

INSERT INTO PRODUCTNAME( DESCRIPTION,CREATIONUSER,MODIFICATIONUSER) values ('NotDefined', 1,1);
Commit;
--INSERT INTO BUILDBAGS(BAGDESCRIPTION ,BAGVERSION ,CREATIONUSER,MODIFICATIONUSER ) values ( 'NotDefined-NotDefined-Empty','1.0',1,1);

---------------------- buildbagid need to create new build id and use that id in below update tables
DECLARE
    dummybuildbagid NUMBER;
BEGIN
    
    INSERT INTO BUILDBAGS (BAGDESCRIPTION, BAGVERSION, CREATIONUSER, MODIFICATIONUSER)
    VALUES ('NotDefined-NotDefined-Empty', '1.0', 1, 1)
    RETURNING BUILDBAGID INTO dummybuildbagid;
 
    UPDATE LCMENGINEERING SET BUILDBAGID = dummybuildbagid;
    UPDATE PLANNEDACTIVITIES SET BUILDBAGID = dummybuildbagid;
    UPDATE NETWORKELEMENTSASPLANNED SET BUILDBAGID = dummybuildbagid;

    COMMIT;
END;
/

ALTER TABLE  LCMENGINEERING  MODIFY  BUILDBAGID  NUMBER(19,0) not null;
ALTER TABLE  NETWORKELEMENTSASPLANNED  MODIFY  BUILDBAGID  NUMBER(19,0) not null;
ALTER TABLE  PLANNEDACTIVITIES  MODIFY  BUILDBAGID  NUMBER(19,0) not null;
 

------ ManageTable Content LCM Dto Delete

DELETE GRIDCUSTOMCOLUMN   WHERE  CLASSNAME in ('LcmEngineeringDtoGrid','DesignComponentFamilyLifeCycleDtoGrid','NetworkElementAsPlannedDtoGrid');
COMMIT;

 
--------------------------------DML SCRIPT FOR AUDITTABLESANDCOLUMNS (Componentsoftwarebuilds) ENTITY  

INSERT INTO AUDITTABLESANDCOLUMNS (ENTITYNAME, ENTITYFIELD, ISLOGENABLED) VALUES ('Lcmengineering', 'Buildbagid', '1');
INSERT INTO AUDITTABLESANDCOLUMNS (ENTITYNAME, ENTITYFIELD, ISLOGENABLED) VALUES ('Networkelementsasplanned', 'Buildbagid', '1');
INSERT INTO AUDITTABLESANDCOLUMNS (ENTITYNAME, ENTITYFIELD, ISLOGENABLED) VALUES ('Plannedactivities', 'Buildbagid', '1');

COMMIT;

----------------------------- ResourceType for Component
DROP SEQUENCE RESOURCETYPES_SEQ;

CREATE SEQUENCE  "RESOURCETYPES_SEQ"  MINVALUE 0 MAXVALUE 9999999999999999 
INCREMENT BY 1 START WITH  5 CACHE 20 NOORDER  NOCYCLE  NOKEEP  NOSCALE  GLOBAL ;


INSERT INTO RESOURCETYPES ( NAME, CREATIONUSER,   MODIFICATIONUSER) VALUES ( 'COMPONENT',1,1);

COMMIT;
-------------------------------- CDFLIFECYCLE Alter
  

ALTER TABLE  DCFLIFECYCLE  ADD  OPCODESCRIPTION  NVARCHAR2(255)   NULL;
ALTER TABLE  DCFLIFECYCLE  ADD  DCFDESCRIPTION  NVARCHAR2(255)   NULL;
ALTER TABLE  DCFLIFECYCLE  ADD  BAGNAME  NVARCHAR2(255)   NULL;
ALTER TABLE  DCFLIFECYCLE  ADD  PLANNEDDETAILS NVARCHAR2(255)   NULL;
ALTER TABLE  DCFLIFECYCLE  ADD  CATEGORYTYPE  NUMBER(10,0)   NULL;  
ALTER TABLE  DCFLIFECYCLE  ADD  DCDESCRIPTION  NVARCHAR2(255)   NULL;
ALTER TABLE DCFLIFECYCLE RENAME COLUMN DETAILS TO CURRENTDETAILS;

-------------- Resource Key 
ALTER TABLE  RESOURCEKEYMASTER  ADD  BUILDBAGID  NUMBER(19,0)    NULL;
ALTER TABLE  RESOURCEKEYMASTER  add COMPONENTID  NUMBER(19,0)   NULL;


---------------------- buildbagid need to create new build id and use that id in below update tables
update  RESOURCEKEYMASTER set buildbagid = ( select max( buildbagid) from BUILDBAGS where bagdescription ='NotDefined-NotDefined-Empty'); 

commit;

 ALTER TABLE  RESOURCEKEYMASTER  Modify   BUILDBAGID  NUMBER(19,0)  not  NULL;

-------------------- Remove Asset Table unused column
ALTER TABLE  NETWORKELEMENTSASPLANNED  DROP COLUMN ISRELEASEDETAILASSETUNKNOWN ;
ALTER TABLE  PLANNEDACTIVITIES  DROP COLUMN ACTIVITYDETAILSTEXT ;


------------- Update script for DCFLifeCyle - categorytype

MERGE INTO dcflifecycle t USING dcflifecycle s ON (t.dcflifecycleid = s.dcflifecycleid)   WHEN MATCHED THEN    UPDATE SET t.categorytype = SUBSTR(s.resourcekey, 1, 1 ) ;  

COMMIT;	 

------------- Update script for DCFLifeCyle - OPCODESCRIPTION  

MERGE INTO dcflifecycle t USING (    SELECT dcflife.dcflifecycleid  , op.opcoid, op.opco as opcoName
FROM dcflifecycle dcflife    JOIN opcos op ON dcflife.opcoid = op.opcoid   ) s 
ON (t.dcflifecycleid = s.dcflifecycleid)WHEN MATCHED THEN  
UPDATE SET t.OPCODESCRIPTION = s.opcoName;

COMMIT;	 

------------- Update script for DCFLifeCyle - DCFDESCRIPTION 
 
MERGE INTO dcflifecycle t USING (    SELECT dclife.dcflifecycleid  ,regexp_replace(case when  sb.Alias is not null then 
 dcf.SYSTEMTYPEIDENTITYNAME||'<b class="text-lowercase"> for </b>'||sb.Alias else 
 dcf.SYSTEMTYPEIDENTITYNAME||'<b class="text-lowercase"> for </b>'|| sb.Description end,'<.+?>' )  as dcfName
FROM    dcflifecycle dclife 
 join opcos op on op.opcoid in dclife.opcoid 
 ---join designcomponents dc on dc.designcomponentid in dclife.dcid  -- Asset and Identity have 0 in DCF Life cycle table
 join designcomponentfamilies dcf on dcf.designcomponentfamilyid in dclife.dcfid
 join subnetworkboundaries sb on sb.id in dcf.SUBNETWORKBOUNDARYID ) s 
ON (t.dcflifecycleid = s.dcflifecycleid)WHEN MATCHED THEN  
UPDATE SET t.DCFDESCRIPTION = s.dcfName;

COMMIT;
------------- Update script for DCFLifeCyle - DCDESCRIPTION 
 MERGE INTO dcflifecycle t USING (select  dclife.dcflifecycleid , regexp_replace(
oem.ORIGINALEQUIPMENTMANUFACTURER||pn.DESCRIPTION||msw.SOFTWAREVERSION||
 case when bc.rule =  3 then '<b class=\"text-lowercase\"> on </b>'||pf.Platform
 else '<b class=\"text-lowercase\"> on </b>'||mhw.Hardwaresolution||pf.Platform||mhw.Hardwaretype 
 end ||'<b class="text-lowercase"> for </b>' || 
 case when  sb.Alias is not null then 
 dcf.SYSTEMTYPEIDENTITYNAME||sb.Alias else 
 dcf.SYSTEMTYPEIDENTITYNAME|| sb.Description end ,'<.+?>' ) as dcName
 from dcflifecycle dclife 
 join opcos op on op.opcoid in dclife.opcoid 
 join designcomponents dc on dc.designcomponentid in dclife.dcid
 join designcomponentfamilies dcf on dcf.designcomponentfamilyid in dclife.dcfid
 join subnetworkboundaries sb on sb.id in dcf.SUBNETWORKBOUNDARYID 
 join systemtypes st on st.SYSTEMTYPEID in dc.SYSTEMTYPEID
 join majorsoftwarebuilds msw on msw.majorsoftwarebuildsid in st.MAJORSOFTWAREBUILDSID 
 join systemtypesmajorhardwarebuilds stmh on stmh.SYSTEMTYPEID in st.SYSTEMTYPEID
 join majorhardwarebuilds mhw on mhw.MAJORHARDWAREID in stmh.MAJORHARDWAREID
 join originalequipmentmanufacturers oem on oem.ORGEQPMANUFACTURERID in mhw.ORGEQPMANUFACTURERID
 join platforms pf on pf.platformid in mhw.PLATFORMID 
 join productname pn on pn.productnameid  in msw.productnameid
 join buildconstructions bc on bc.BUILDCONSTRUCTIONID in mhw.BUILDCONSTRUCTIONID
 where stmh.ISMAIN = 1 ) s 
ON (t.dcflifecycleid = s.dcflifecycleid)WHEN MATCHED THEN  
UPDATE SET t.DCDESCRIPTION = s.dcName;
COMMIT;
-----------------------------------------
---------- Below Script for Disaggregate Report 
update gridcustomcolumn
set deleted=1 ,deletiondate=SYS_EXTRACT_UTC(systimestamp) 
where deleted = 0 and classname in ('ReportHardwareDtoGrid','ReportSoftwareDtoGrid','NetworkLevelTwoSWReportDtoGrid',
'ReportHardwareConfigurationDtoGrid','ReportSubnetWorkHardWareGrid','ReportSubnetWorkSoftWareGrid'   );
commit;

-------------------------------------
 select   op.opcoid,dclife.dcid,op.opco, 
 case when bc.rule =  3 then '<b class=\"text-lowercase\"> on </b>'||pf.Platform
 else '<b class=\"text-lowercase\"> on </b>'||mhw.Hardwaresolution||pf.Platform||mhw.Hardwaretype 
 end ||'<b class="text-lowercase"> for </b>' || 
 case when  sb.Alias is not null then 
 dcf.SYSTEMTYPEIDENTITYNAME||sb.Alias else 
 dcf.SYSTEMTYPEIDENTITYNAME|| sb.Description end  
 from dcflifecycle dclife 
 join opcos op on op.opcoid in dclife.opcoid 
 join designcomponents dc on dc.designcomponentid in dclife.dcid
 join designcomponentfamilies dcf on dcf.designcomponentfamilyid in dclife.dcfid
 join subnetworkboundaries sb on sb.id in dcf.SUBNETWORKBOUNDARYID 
 join systemtypes st on st.SYSTEMTYPEID in dc.SYSTEMTYPEID
 join majorsoftwarebuilds msw on msw.majorsoftwarebuildsid in st.MAJORSOFTWAREBUILDSID 
 join systemtypesmajorhardwarebuilds stmh on stmh.SYSTEMTYPEID in st.SYSTEMTYPEID
 join majorhardwarebuilds mhw on mhw.MAJORHARDWAREID in stmh.MAJORHARDWAREID
 join originalequipmentmanufacturers oem on oem.ORGEQPMANUFACTURERID in mhw.ORGEQPMANUFACTURERID
 join platforms pf on pf.platformid in mhw.PLATFORMID 
 join productname pn on pn.productnameid  in msw.productnameid
 join buildconstructions bc on bc.BUILDCONSTRUCTIONID in mhw.BUILDCONSTRUCTIONID
 where stmh.ISMAIN = 1   ;
   
