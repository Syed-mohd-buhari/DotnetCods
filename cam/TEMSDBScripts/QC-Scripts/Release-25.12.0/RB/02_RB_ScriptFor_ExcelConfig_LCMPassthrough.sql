----------------RB Script for Excel config of LCM Passthrough-------------------
delete from exceltemplateconfiguration where PROCESSNAME = 'PassThroughLcmSoftware';
delete from exceltemplateconfiguration where PROCESSNAME ='PassThroughLcmHardware';
commit;