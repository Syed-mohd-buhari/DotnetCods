
--------------------------------------------------------
--  INSERT QUERY FOR PLANNEDACTIVITYTYPES and ALTER PLANNEDACTIVITYTYPESID_SEQ id
--------------------------------------------------------
DECLARE
maxplannedactid NUMBER(9, 0);
BEGIN
SELECT MAX(plannedactivitytypesid) + 1 INTO maxplannedactid FROM plannedactivitytypes;
EXECUTE IMMEDIATE 'ALTER SEQUENCE PLANNEDACTIVITYTYPESID_SEQ RESTART MINVALUE 0 MAXVALUE 9999999999999999 INCREMENT BY 1 START WITH '|| maxplannedactid ||'';
COMMIT;
END;

INSERT INTO plannedactivitytypes ("PLANNEDACTIVITYTYPEDESCRIPTION", "HWOEM", "HWSOLUTION", "HWPLATFORM", "SWOEM", "SWPRODUCTNAME","SWVERSION","SUBNETWORKSERVICE","LINKEDDCRULE", "CREATIONUSER", "MODIFICATIONUSER")VALUES( 'Modernize Solution Successor Network',1, 0, 1, 1, 1, 1, 0, 1, 1, 1);
Commit; 
 

/*DECLARE
maxplannedactid NUMBER(9, 0);
BEGIN
SELECT MAX(plannedactivitytypesid) + 1 INTO maxplannedactid FROM plannedactivitytypes;
EXECUTE IMMEDIATE 'ALTER SEQUENCE PLANNEDACTIVITYTYPESID_SEQ RESTART MINVALUE 0 MAXVALUE 9999999999999999 INCREMENT BY 1 START WITH '|| maxplannedactid ||'';
COMMIT;
INSERT ALL INTO plannedactivitytypes ("PLANNEDACTIVITYTYPEDESCRIPTION", "HWOEM", "HWSOLUTION", "HWPLATFORM", "SWOEM", "SWPRODUCTNAME", "SWVERSION","SUBNETWORKSERVICE","LINKEDDCRULE", "CREATIONUSER", "MODIFICATIONUSER" )VALUES( plannedactivitytypedescription,hwoem, hwsolution, hwplatform, swoem,swproductname,swversion, subnetworkservice, linkeddcrule, creationuser, modificationuser )WITH names AS ( SELECT 'Modernize Solution Successor Network' AS plannedactivitytypedescription, hwoem, hwsolution, hwplatform, swoem, swproductname, swversion, subnetworkservice, linkeddcrule, creationuser, modificationuser FROM plannedactivitytypes WHERE REGEXP_REPLACE(lower(PLANNEDACTIVITYTYPEDESCRIPTION), ' ', '') = 'hardwareupgrade') SELECT * FROM names;
COMMIT;
END; */


--------------------------------------------------------
--  UPDATE QUERY FOR SETTINGSUPDATEPLANNEDACTIVITY for DECOMMISSION FLOW  SETTINGS
--------------------------------------------------------
  
declare 
ruleid number(9,0);
plannedActResId number(9,0);
begin
select plannedactivitytypesid into ruleid from plannedactivitytypes where lower(REGEXP_REPLACE(plannedactivitytypedescription, ' ', '')) = 'modernizesolutionsuccessornetwork';
update plannedactivityresources set rulelinkeddc = ruleid  WHERE lower(REGEXP_REPLACE(plannedactivityresource, ' ', '')) = 'modernizesolution(successornetworkusesincumbentvendor)';
commit;   
end ;

  