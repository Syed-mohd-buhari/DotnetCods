--------------  ROLLBACK SCRIPT FOR SETTINGSUPDATEPLANNEDACTIVITY ---------------


alter table settingsupdateplannedactivity drop column IsRollback;
commit;


-----------------------------------END-----------------------------------------