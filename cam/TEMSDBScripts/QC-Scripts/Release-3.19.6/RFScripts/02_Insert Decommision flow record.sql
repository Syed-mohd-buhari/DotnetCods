
--------------------------------------------------------
--  INSERT QUERY FOR PLANNEDACTIVITYTYPES and ALTER PLANNEDACTIVITYTYPESID_SEQ id
--------------------------------------------------------
 
 --INSERT INTO PLANNEDACTIVITYTYPES ("PLANNEDACTIVITYTYPEDESCRIPTION",    "HWOEM",    "HWSOLUTION",    "HWPLATFORM",    "SWOEM",    "SWPRODUCTNAME",    "SWVERSION",   "SUBNETWORKSERVICE","LINKEDDCRULE",	"CREATIONUSER",  "MODIFICATIONUSER" ) VALUES ('Decommission Service Node', 0, 0, 0, 0, 0, 0, 0, 0, 13, 13);
 
 
 DECLARE
    maxplannedactid NUMBER(9, 0);
BEGIN
    SELECT
        MAX(plannedactivitytypesid) + 1
    INTO maxplannedactid
    FROM
        plannedactivitytypes;

    EXECUTE IMMEDIATE 'ALTER SEQUENCE PLANNEDACTIVITYTYPESID_SEQ RESTART MINVALUE 0 MAXVALUE 9999999999999999 INCREMENT BY 1  
 START WITH '|| maxplannedactid                      || '';
    COMMIT;
    INSERT ALL INTO plannedactivitytypes (
        "PLANNEDACTIVITYTYPEDESCRIPTION", "HWOEM", "HWSOLUTION", "HWPLATFORM", "SWOEM", "SWPRODUCTNAME", "SWVERSION", "SUBNETWORKSERVICE", "LINKEDDCRULE", "CREATIONUSER", "MODIFICATIONUSER"
    ) VALUES (
        plannedactivitytypedescription,hwoem, hwsolution, hwplatform, swoem, swproductname, swversion, subnetworkservice, linkeddcrule, creationuser, modificationuser
    ) WITH names AS (
          SELECT
              'Decommission Service Nodes' AS plannedactivitytypedescription, hwoem, hwsolution, hwplatform, swoem, swproductname, swversion, subnetworkservice, linkeddcrule, creationuser, modificationuser
			  FROM
              plannedactivitytypes
          WHERE
              plannedactivitytypesid = 0
      )
      SELECT
          *
      FROM
          names;

    COMMIT;
END;


--------------------------------------------------------
--  UPDATE QUERY FOR SETTINGSUPDATEPLANNEDACTIVITY for DECOMMISSION FLOW  SETTINGS
--------------------------------------------------------
  
declare 
ruleid number(9,0);
plannedActResId number(9,0);
begin
  
  select plannedactivitytypesid into ruleid from plannedactivitytypes 
  where plannedactivitytypedescription = 'Decommission Service Nodes';
 
 update plannedactivityresources set rulelinkeddc = ruleid  WHERE plannedactivityresource = 'Decommission Service Nodes';
 commit;   
end ;

 