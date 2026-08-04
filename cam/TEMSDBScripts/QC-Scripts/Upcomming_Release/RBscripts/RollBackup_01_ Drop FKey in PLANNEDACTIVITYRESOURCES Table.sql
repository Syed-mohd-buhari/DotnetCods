--------------------------------------------------------
--  ALTER REMOVE FOREIGNKEY REFFERENCE IN PLANNEDACTIVITYRESOURCES
--------------------------------------------------------

ALTER TABLE PLANNEDACTIVITYRESOURCES DROP CONSTRAINT FK_PLANNEDACTIVITYRESOURCES_PLANNEDACTIVITYTYPES_RULELINKEDDC;

COMMIT;

--------------------------------------------------------
--  ALTER DROP PLANNEDACTIVITYTYPES
--------------------------------------------------------

Drop table PLANNEDACTIVITYTYPES;

COMMIT ;

Drop SEQUENCE  PLANNEDACTIVITYTYPESID_SEQ;

COMMIT ; 