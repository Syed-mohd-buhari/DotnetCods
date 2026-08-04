-- Create Temp Table  
 CREATE TABLE ONETIMESYSTEMTYPE (SYSTEMTYPEID  NUMBER(19)  ,SYSTEMTYPENAMEOEM  NVARCHAR2(255)  
,MAJORSOFTWAREBUILDSID  NUMBER(19)  ,MAJORHARDWAREID  NUMBER(19)  ,
VODAFONENAME  NUMBER(19), PLATFORMID  NUMBER(19));

-- Insert into Temp Table 
INSERT INTO ONETIMESYSTEMTYPE  (SYSTEMTYPEID ,SYSTEMTYPENAMEOEM ,MAJORSOFTWAREBUILDSID ,MAJORHARDWAREID 
,VODAFONENAME ,PLATFORMID  ) 
SELECT
 ST.SYSTEMTYPEID, ST.SYSTEMTYPENAMEOEM, ST.MAJORSOFTWAREBUILDSID, STMH.MAJORHARDWAREID , 
  ST.VODAFONENAME,PT.PLATFORMID
    FROM SYSTEMTYPES ST 
    JOIN SYSTEMTYPESMAJORHARDWAREBUILDS STMH   ON STMH.SYSTEMTYPEID = ST.SYSTEMTYPEID       AND STMH.ISMAIN = 1
    JOIN MAJORHARDWAREBUILDS MH        ON MH.MAJORHARDWAREID = STMH.MAJORHARDWAREID
    JOIN MAJORSOFTWAREBUILDS MS        ON MS.MAJORSOFTWAREBUILDSID = ST.MAJORSOFTWAREBUILDSID
    JOIN PLATFORMS PT        ON PT.PLATFORMID = MH.PLATFORMID
    WHERE ST.DELETED = 0 AND ISMAIN =1  AND PT.PLATFORM = 'VMWare NFVI'  
	 AND st.systemtypeid NOT IN (
        SELECT stbk.systemtypeid
        FROM ONETIMESYSTEMTYPE stbk
        WHERE stbk.systemtypeid IS NOT NULL
  );   

Commit;

-------------------------- Insert into System Type Table
 INSERT INTO systemtypes (
 creationuser, modificationuser, systemtypenamevodafone, systemtypename3gpp, systemtypenameoem, majorsoftwarebuildsid, constraintscaling, endofmaintenance, assetcategoryid, sparefieldsjson, productimportanceid, assetclassid, assettypeid, constraintlcm, vodafonename
    )
SELECT
 1, 1, st.systemtypenamevodafone, st.systemtypename3gpp, st.systemtypenameoem, st.majorsoftwarebuildsid, st.constraintscaling,
 st.endofmaintenance, st.assetcategoryid, st.sparefieldsjson, st.productimportanceid, st.assetclassid, st.assettypeid, 
 st.constraintlcm, st.vodafonename
 FROM systemtypes st
 join ONETIMESYSTEMTYPE stbk on stbk.systemtypeid in st.systemtypeid ;

Commit;
 -------  Insert into SYSTEMTYPESMAJORHARDWAREBUILDS  

INSERT INTO systemtypesmajorhardwarebuilds ( systemtypeid, majorhardwareid, creationuser, modificationuser, ismain )   
SELECT st.systemtypeid, 
(SELECT MAJORHARDWAREID FROM (select  mh.MAJORHARDWAREID  from majorhardwarebuilds mh join platforms pt on pt.PLATFORMID in
mh.PLATFORMID where   pt.PLATFORM = 'CEE-CNIS' ORDER BY mh.MAJORHARDWAREID )WHERE ROWNUM = 1), 1, 1,1
FROM systemtypes st
WHERE st.creationuser = 1
  AND st.modificationuser = 1
  AND st.systemtypeid NOT IN (
        SELECT stbk.systemtypeid
        FROM ONETIMESYSTEMTYPE stbk
        WHERE stbk.systemtypeid IS NOT NULL
  );    
  
 Commit; 
  
select * from systemtypes order by modificationdate desc;

select * from systemtypesmajorhardwarebuilds order by modificationdate desc;
 
 
--------------------------- Another way  
DECLARE
    v_systemtypeid systemtypes.systemtypeid%TYPE;
BEGIN
    /* 1. Insert   SYSTEMTYPES */
    INSERT INTO systemtypes (
        creationuser, modificationuser, systemtypenamevodafone, systemtypename3gpp, systemtypenameoem, majorsoftwarebuildsid, constraintscaling, endofmaintenance, assetcategoryid, sparefieldsjson, productimportanceid, assetclassid, assettypeid, constraintlcm, vodafonename
    )
    SELECT
        1, 1, systemtypenamevodafone, systemtypename3gpp, systemtypenameoem, majorsoftwarebuildsid, constraintscaling, endofmaintenance, assetcategoryid, sparefieldsjson, productimportanceid, assetclassid, assettypeid, constraintlcm, vodafonename
    FROM systemtypes
    WHERE systemtypeid = 3267;

    /* 2. Get SYSTEMTYPEID */
    SELECT MAX(systemtypeid)
    INTO v_systemtypeid
    FROM systemtypes;

    /* 3. Insert   SYSTEMTYPESMAJORHARDWAREBUILDS */
    INSERT INTO systemtypesmajorhardwarebuilds (
        systemtypeid, majorhardwareid, creationuser, modificationuser, ismain
    )
    values (
        v_systemtypeid,
        (select  mh.MAJORHARDWAREID  from majorhardwarebuilds mh
        join platforms pt on pt.PLATFORMID in mh.PLATFORMID where   pt.PLATFORM = 'CEE-CNIS'),
        1, 1, 1 ) ;
    COMMIT;
END;
/  
