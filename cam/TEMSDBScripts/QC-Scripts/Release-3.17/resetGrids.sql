update gridcustomcolumn
set deleted=1 ,deletiondate=SYS_EXTRACT_UTC(systimestamp) 
where deleted = 0 and classname='IdentityAsIsDtoGrid';
commit;

update gridcustomcolumn
set deleted=1 ,deletiondate=SYS_EXTRACT_UTC(systimestamp) 
where deleted = 0 and classname='DeliveryTrackingDtoGrid';
commit;

update gridcustomcolumn
set deleted=1 ,deletiondate=SYS_EXTRACT_UTC(systimestamp) 
where deleted = 0 and classname='ReportSoftwareDtoGrid';
commit;

update gridcustomcolumn
set deleted=1 ,deletiondate=SYS_EXTRACT_UTC(systimestamp) 
where deleted = 0 and classname='ReportHardwareDtoGrid';
commit;

update gridcustomcolumn
set deleted=1 ,deletiondate=SYS_EXTRACT_UTC(systimestamp) 
where deleted = 0 and classname='PlannedActivityDtoGrid';
commit;