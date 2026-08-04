---------------------------
-- Drop unused TSR report columns in Ancillary Data
---------------------------

ALTER TABLE lcmancillarydata DROP COLUMN  Kpistatusservice ;
ALTER TABLE lcmancillarydata DROP COLUMN  custom;
ALTER TABLE lcmancillarydata DROP COLUMN custom1;
ALTER TABLE lcmancillarydata DROP COLUMN custom2;
ALTER TABLE lcmancillarydata DROP COLUMN idnew;
ALTER TABLE lcmancillarydata DROP COLUMN productimportancehistory2;
ALTER TABLE lcmancillarydata DROP COLUMN cloudVersion;
ALTER TABLE lcmancillarydata DROP COLUMN certifiedswrealesefornfvibundle;
ALTER TABLE lcmancillarydata DROP COLUMN LCMSTATUS; 
ALTER TABLE LCMANCILLARYDATA  DROP COLUMN SECURITYRISKPOTENTIAL;
commit ;