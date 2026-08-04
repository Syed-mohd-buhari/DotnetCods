---------------------------
-- Add new TSR report columns in Ancillary Data
---------------------------

Alter table lcmancillarydata add   RegulatoryFields  NVARCHAR2(50);
Alter table lcmancillarydata add   ExternalFacingFlag   NUMBER(1,0);
ALTER TABLE LCMANCILLARYDATA ADD LOCATIONINFRASTRUCTURE NVARCHAR2(50);
commit ;