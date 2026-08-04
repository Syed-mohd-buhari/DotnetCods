ALTER TABLE LCMENGINEERING RENAME COLUMN  ISRELEASEDETAILSKNOWN TO ISRELEASEDETAILUNKNOWN ;
ALTER TABLE NETWORKELEMENTSASPLANNED RENAME COLUMN  ISRELEASEDETAILSKNOWN TO ISRELEASEDETAILASSETUNKNOWN ;
ALTER TABLE PLANNEDACTIVITIES RENAME COLUMN PELIVERYPROJECTID TO DELIVERYPROJECTID ;
COMMIT;

update audittablesandcolumns set entityfield ='Deliveryprojectid' where entityfield = 'Peliveryprojectid' and entityname ='Plannedactivities'; 
update audittablesandcolumns set entityfield ='Isreleasedetailunknown' where entityfield = 'Isreleasedetailsknown' and entityname ='Lcmengineering'; 
update audittablesandcolumns set entityfield ='Isreleasedetailassetunknown' where entityfield = 'Isreleasedetailsknown' and entityname ='Networkelementsasplanned'; 
 
COMMIT;


 