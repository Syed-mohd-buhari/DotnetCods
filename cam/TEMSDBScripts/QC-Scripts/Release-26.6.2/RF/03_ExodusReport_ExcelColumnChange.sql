--------  NOTE EXODUS TEMPLATE CHANGE BELOW---------------
--1.RFO column----------
--2.RFA column ---------

--------  ALTER SCRIPT EXODUS EXCEL---------------
update exceltemplateconfiguration set columnorder=26 where processname='EXODUS' and propertyname='startRfo';
update exceltemplateconfiguration set columnorder=27 where processname='EXODUS' and propertyname='RfaDate';
commit;
-------------------------------------------------------
delete from gridcustomcolumn where classname='ExodusAssetLevelReportDtoGrid';
commit;
