ALTER TABLE LCMENGINEERING rename column  ISRELEASEDETAILUNKNOWN to ISRELEASEDETAILSKNOWN ;
ALTER TABLE NETWORKELEMENTSASPLANNED RENAME COLUMN  ISRELEASEDETAILASSETUNKNOWN TO ISRELEASEDETAILSKNOWN ;
ALTER TABLE PLANNEDACTIVITIES rename column DELIVERYPROJECTID to PELIVERYPROJECTID ;
Commit; 

update audittablesandcolumns set entityfield ='Peliveryprojectid' where entityfield = 'Deliveryprojectid' and entityname ='Plannedactivities';
update audittablesandcolumns set entityfield ='Isreleasedetailsknown' where entityfield = 'Isreleasedetailunknown' and entityname ='Lcmengineering';
update audittablesandcolumns set entityfield ='Isreleasedetailsknown' where entityfield = 'Isreleasedetailassetunknown' and entityname ='Networkelementsasplanned';
 
COMMIT;
 