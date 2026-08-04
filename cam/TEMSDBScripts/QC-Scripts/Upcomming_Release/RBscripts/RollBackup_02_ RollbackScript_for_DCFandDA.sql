--------------------------------------------------------
-- DDL ROLLBACK SCRIPT for Table DESIGNCOMPONENTFAMILIES STEP 1 
--------------------------------------------------------
ALTER TABLE DESIGNCOMPONENTFAMILIES ADD CRITICALITYRATING NUMBER(10,0);

ALTER TABLE DESIGNCOMPONENTFAMILIES ADD COUNTRYSPECIFICCRITICALITY NUMBER(1,0) DEFAULT 0;

-----------------------------------------------------------------------------------------
-- ROLLBACK ONE TIME SCRIPT FOR DESIGNCOMPONENTFAMILIES CRITICALITYRATING COLUMN STEP 2
-----------------------------------------------------------------------------------------

MERGE INTO DESIGNCOMPONENTFAMILIES dcf
USING (
    SELECT
    DESIGNCOMPONENTFAMILYID,
    CRITICALITYRATING
    FROM DESIGNASPECTS
) d
ON (dcf.DESIGNCOMPONENTFAMILYID = d.DESIGNCOMPONENTFAMILYID) 

WHEN MATCHED THEN
    UPDATE SET
        dcf.CRITICALITYRATING = d.CRITICALITYRATING;

--------------------------------------------------------
-- ROLLBACK ALTER SCRIPT for Table DESIGNASPECTS STEP 3
--------------------------------------------------------

ALTER TABLE DESIGNASPECTS DROP COLUMN CRITICALITYRATING;

COMMIT;