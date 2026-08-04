

ALTER TABLE lcmancillarydata ADD ISEXPOSEDEDGE NUMBER(1) DEFAULT 0;
update lcmancillarydata set  ISEXPOSEDEDGE=1 where Locationinfrastructure='Exposed Edge';
commit;

--update lcmancillarydata set Locationinfrastructure=null where Locationinfrastructure='Exposed Edge'
