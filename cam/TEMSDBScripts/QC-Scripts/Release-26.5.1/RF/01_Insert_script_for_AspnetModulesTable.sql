INSERT INTO "ASPNETMODULES" (MODULE, MODULEPATH, ISDEFAULT, CREATIONUSER, MODIFICATIONUSER, DELETED, MENU) 
VALUES (N'Exodus @Glance', N'/exodusatglance', '0', '1', '1', '0', N'Graphical Reports');

/*Please don't run in Prod*/
INSERT INTO "ASPNETMODULES" (MODULE, MODULEPATH, ISDEFAULT, CREATIONUSER, MODIFICATIONUSER, DELETED, MENU) 
VALUES (N'Test - PA', N'/testplannedActivities/test', '0', '1', '1', '0', N'Grid Reports');
commit;