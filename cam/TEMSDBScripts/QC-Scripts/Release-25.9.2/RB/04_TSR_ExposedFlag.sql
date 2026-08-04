
update lcmancillarydata set   Locationinfrastructure='Exposed Edge'  where ISEXPOSEDEDGE=1;
commit;
ALTER TABLE lcmancillarydata DROP COLUMN ISEXPOSEDEDGE;