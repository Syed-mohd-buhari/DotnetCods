--------------------------------------------------------
--  UPDATE 'Lcmancillarydata'
--------------------------------------------------------

update lcmancillarydata set engupdatetracker = 'To be started' where engupdatetracker is null;
commit;

update lcmancillarydata set handedovertooperation = 1 where handedovertooperation is null; 
commit;
 