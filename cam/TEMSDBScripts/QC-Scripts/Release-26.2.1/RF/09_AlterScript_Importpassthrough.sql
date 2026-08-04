update exceltemplateconfiguration set PROPERTYNAME = 'reportId' 
where PROCESSNAME = 'PassThroughLcmHardware' and PROPERTYNAME = 'hwResourceKeyPassThrough';
update exceltemplateconfiguration set PROPERTYNAME = 'reportId' 
where PROCESSNAME = 'PassThroughLcmSoftware' and PROPERTYNAME = 'swResourceKeyPassThrough';
Commit;

ALTER TABLE hwpassthroughlcm 
MODIFY NOTES NVARCHAR2(2000);
commit;