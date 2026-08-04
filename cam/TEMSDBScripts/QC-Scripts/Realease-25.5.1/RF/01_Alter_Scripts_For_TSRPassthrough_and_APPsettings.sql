-------------------------Alter script for TSRPASSTHROUGH----------------------------

ALTER TABLE tsrpassthrough ADD NONTEMSVERTICAL number(19,0) default null; 
ALTER TABLE "TSRPASSTHROUGH" ADD CONSTRAINT "FK_TSRPASSTHROUGH_APPSETTINGSCONFIGURATION_NONTEMSVERTICAL" FOREIGN KEY ("NONTEMSVERTICAL") REFERENCES "APPSETTINGSCONFIGURATION" ("APPCONFIGURATIONSETTINGID") ENABLE;
COMMIT;


-----------------------------END----------------------------------

-------------------------ALTER script for APPSETTINGS----------------------------

DROP SEQUENCE "APPSETTINGS_SEQ";
CREATE SEQUENCE  "APPSETTINGS_SEQ"  MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 2 CACHE 20 NOORDER  NOCYCLE  NOKEEP  NOSCALE  GLOBAL ;
COMMIT;


-----------------------------END----------------------------------



-------------------------INSERT script for APPSETTINGS----------------------------

INSERT INTO APPSETTINGS (APPSETTINGID, DESCRIPTION, CREATIONUSER, MODIFICATIONUSER) VALUES (2, 'NONTEMS Vertical', 1, 1);
COMMIT;


-----------------------------END----------------------------------