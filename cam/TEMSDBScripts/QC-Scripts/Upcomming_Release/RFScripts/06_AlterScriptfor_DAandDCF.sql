--------------------------------------------------------
--   DDL for Table DESIGNASPECTS STEP 1 
--------------------------------------------------------

ALTER TABLE DESIGNASPECTS ADD CRITICALITYRATING NUMBER(10,0);

---------------------------------------------------------------------
--  ONE TIME SCRIPT FOR DESIGNASPECTS CRITICALITYRATING COLUMN STEP 2 
---------------------------------------------------------------------

MERGE INTO DESIGNASPECTS d
USING (
    SELECT
    DESIGNCOMPONENTFAMILYID,
    CRITICALITYRATING
    FROM DESIGNCOMPONENTFAMILIES
) dcf
ON (d.DESIGNCOMPONENTFAMILYID = dcf.DESIGNCOMPONENTFAMILYID) 

WHEN MATCHED THEN
    UPDATE SET
        d.CRITICALITYRATING = dcf.CRITICALITYRATING;
--------------------------------------------------------
--  DDL ALTER SCRIPT for Table DESIGNCOMPONENTFAMILIES STEP 3 
--------------------------------------------------------

ALTER TABLE DESIGNCOMPONENTFAMILIES DROP COLUMN CRITICALITYRATING;

ALTER TABLE DESIGNCOMPONENTFAMILIES DROP COLUMN COUNTRYSPECIFICCRITICALITY;

COMMIT;