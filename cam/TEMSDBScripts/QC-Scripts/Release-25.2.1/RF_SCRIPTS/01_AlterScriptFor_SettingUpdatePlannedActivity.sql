--------------  ALTER SCRIPT FOR SETTINGSUPDATEPLANNEDACTIVITY ---------------


alter table settingsupdateplannedactivity add IsRollback number(1,0) default 0;
commit;



-----------------------------------END-----------------------------------------

