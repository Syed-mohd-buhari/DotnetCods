
INSERT INTO "ASPNETMODULES" (MODULE, MODULEPATH, CREATIONUSER, MODIFICATIONUSER, MENU) VALUES (N'Exodus - Asset Timeline', N'/exportreportthree', '1', '1', N'Graphical Reports');
COMMIT;

update ASPNETMODULES set MODULEPATH = N'/exodusassettimeline' where  MODULE =N'Exodus - Asset Timeline';
Commit; 