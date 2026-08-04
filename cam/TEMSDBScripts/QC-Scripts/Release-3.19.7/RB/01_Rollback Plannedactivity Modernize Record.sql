
--------------------------------------------------------
--  Delete "Decommission Service Node" Record QUERY FOR PLANNEDACTIVITYTYPES and ALTER PLANNEDACTIVITYTYPESID_SEQ id
--------------------------------------------------------
 
Delete from  PLANNEDACTIVITYTYPES  where "PLANNEDACTIVITYTYPEDESCRIPTION" = 'Modernize Solution Successor Network' ;
COMMIT;


DECLARE
maxplannedactid NUMBER(9, 0);
BEGIN
SELECT MAX(plannedactivitytypesid) + 1 INTO maxplannedactid FROM plannedactivitytypes;
EXECUTE IMMEDIATE 'ALTER SEQUENCE PLANNEDACTIVITYTYPESID_SEQ RESTART MINVALUE 0 MAXVALUE 9999999999999999 INCREMENT BY 1 START WITH '|| maxplannedactid|| '';
COMMIT;
END;

--------------------------------------------------------
--  UPDATE QUERY FOR SETTINGSUPDATEPLANNEDACTIVITY for DECOMMISSION FLOW  SETTINGS
--------------------------------------------------------
 
declare 
ruleid number(9,0);
plannedActResId number(9,0);
begin
select plannedactivitytypesid into ruleid from plannedactivitytypes where  REGEXP_REPLACE(lower(PLANNEDACTIVITYTYPEDESCRIPTION), ' ', '') = 'hardwareupgrade';
update plannedactivityresources set rulelinkeddc = ruleid WHERE lower(REGEXP_REPLACE(plannedactivityresource, ' ', '')) = 'modernizesolution(successornetworkusesincumbentvendor)';
commit;   
end ;
  