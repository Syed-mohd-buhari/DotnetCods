--------------------------------------------------
-----------ALTER SCRIPT FOR LCMEXPORTSETTING
--------------------------------------------------

ALTER TABLE LCMEXPORTSETTINGS DROP COLUMN ISCURRENT;
COMMIT;

ALTER TABLE LCMEXPORTSETTINGS ADD ISDEFAULT NUMBER(1,0) DEFAULT 0;

DELETE GRIDCUSTOMCOLUMN WHERE CLASSNAME IN ('LcmExportSettingDtoGrid');
COMMIT;

-----------------------END-------------------------