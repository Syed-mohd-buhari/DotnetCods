-------------RF script for Dynamic reports and LCMANCILLARYDATA-------------

alter table dynamicreports add OpcoId NVARCHAR2(2000);


ALTER TABLE LCMANCILLARYDATA MODIFY (COMMENTONPROJECTSTATUS NVARCHAR2(2000));
COMMIT;