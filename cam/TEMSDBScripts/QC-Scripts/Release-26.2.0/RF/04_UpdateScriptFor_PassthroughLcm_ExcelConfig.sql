
update exceltemplateconfiguration set PROPERTYNAME = 'hwResourceKeyPassThrough' where PROCESSNAME = 'PassThroughLcmHardware' and PROPERTYNAME = 'reportId';
update exceltemplateconfiguration set PROPERTYNAME = 'swResourceKeyPassThrough' where PROCESSNAME = 'PassThroughLcmSoftware' and PROPERTYNAME = 'reportId';
Commit;