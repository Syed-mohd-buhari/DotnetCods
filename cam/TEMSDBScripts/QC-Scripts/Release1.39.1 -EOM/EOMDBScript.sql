ALTER TABLE MAJORSOFTWAREBUILDS
Add "EOMSTATUS" NUMBER(5,0) DEFAULT 0 NOT NULL ENABLE ;

ALTER TABLE MAJORHARDWAREBUILDS
Add "EOMSTATUS" NUMBER(5,0) DEFAULT 0 NOT NULL ENABLE ;



UPDATE majorhardwarebuilds
SET eomstatus =2 
WHERE endofmaintenance IS NOT NULL  ;

COMMIT;

UPDATE majorsoftwarebuilds
SET eomstatus =2 
WHERE endofmaintenance IS NOT NULL  ;

COMMIT;

UPDATE majorhardwarebuilds
SET eomstatus =1
WHERE endofmaintenance IS NULL  ;

COMMIT;

UPDATE majorsoftwarebuilds
SET eomstatus =1
WHERE endofmaintenance IS NULL  ;

COMMIT;

