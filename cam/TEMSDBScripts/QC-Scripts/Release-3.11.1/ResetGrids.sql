update gridcustomcolumn
set deleted=1 ,deletiondate=SYS_EXTRACT_UTC(systimestamp) 
where deleted = 0 and classname='DesignAspectDtoGrid';
commit;

update gridcustomcolumn
set deleted=1 ,deletiondate=SYS_EXTRACT_UTC(systimestamp) 
where deleted = 0 and classname='LcmEngineeringDtoGrid';
commit;

update gridcustomcolumn
set deleted=1 ,deletiondate=SYS_EXTRACT_UTC(systimestamp) 
where deleted = 0 and classname='NetworkElementAsPlannedDtoGrid';
commit;


update gridcustomcolumn
set deleted=1 ,deletiondate=SYS_EXTRACT_UTC(systimestamp) 
where deleted = 0 and classname='ArchivedLcmengineeringDtoGrid	';
commit;