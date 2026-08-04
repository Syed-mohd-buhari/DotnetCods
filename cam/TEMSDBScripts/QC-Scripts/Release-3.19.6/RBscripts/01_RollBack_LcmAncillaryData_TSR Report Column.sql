--------------------------------------------------------
-- ROLLBACK ALTER SCRIPT for Table lcmancillarydata  
--------------------------------------------------------

ALTER TABLE lcmancillarydata DROP COLUMN RegulatoryFields; 
ALTER TABLE lcmancillarydata DROP COLUMN ExternalFacingFlag;
ALTER TABLE LCMANCILLARYDATA DROP COLUMN LOCATIONINFRASTRUCTURE ; 

COMMIT;
