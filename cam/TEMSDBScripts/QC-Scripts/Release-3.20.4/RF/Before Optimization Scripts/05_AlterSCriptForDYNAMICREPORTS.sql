----------------------------------------------
-- AlTER SCRIPT FOR DynamicReports ------
ALTER TABLE DYNAMICREPORTS MODIFY ISSCHEDULED NUMBER(1,0) DEFAULT 0; 
COMMIT;

update DYNAMICREPORTS set ISSCHEDULED = null ;
commit;