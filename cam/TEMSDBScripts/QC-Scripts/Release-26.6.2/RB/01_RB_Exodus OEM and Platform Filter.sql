--------  SCRIPT FOR APPSETTINGS---------------
delete from APPSETTINGSCONFIGURATION where APPSETTINGID in (3,4);
delete from APPSETTINGS where APPSETTINGID in (3,4);
Commit;