update gridcustomcolumn
set deleted=1 ,deletiondate=SYS_EXTRACT_UTC(systimestamp) 
where deleted = 0 and classname='MajorHardwareBuildDtoGrid';
commit;


update gridcustomcolumn
set deleted=1 ,deletiondate=SYS_EXTRACT_UTC(systimestamp) 
where deleted = 0 and classname='MajorSoftwareBuildDtoGrid';
commit;

update gridcustomcolumn
set deleted=1 ,deletiondate=SYS_EXTRACT_UTC(systimestamp) 
where deleted = 0 and classname='SystemTypeDtoGrid';
commit;

update gridcustomcolumn
set deleted=1 ,deletiondate=SYS_EXTRACT_UTC(systimestamp) 
where deleted = 0 and classname='DesignComponentDtoGrid';
commit;


update gridcustomcolumn
set deleted=1 ,deletiondate=SYS_EXTRACT_UTC(systimestamp) 
where deleted = 0 and classname='SystemTypeDtoGrid';
commit;

update gridcustomcolumn
set deleted=1 ,deletiondate=SYS_EXTRACT_UTC(systimestamp) 
where deleted = 0 and classname='DesignComponentFamilyDtoGrid';
commit;

update gridcustomcolumn
set deleted=1 ,deletiondate=SYS_EXTRACT_UTC(systimestamp) 
where deleted = 0 and classname='SubNetworkBoundaryGridDto';
commit;

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
where deleted = 0 and classname='NetworkElementAsIsDtoGrid';
commit;

update gridcustomcolumn
set deleted=1 ,deletiondate=SYS_EXTRACT_UTC(systimestamp) 
where deleted = 0 and classname='PlannedActivityDtoGrid';
commit;


update gridcustomcolumn
set deleted=1 ,deletiondate=SYS_EXTRACT_UTC(systimestamp) 
where deleted = 0 and classname='BundleUpgradeInitiativeDtoGrid';
commit;

update gridcustomcolumn
set deleted=1 ,deletiondate=SYS_EXTRACT_UTC(systimestamp) 
where deleted = 0 and classname='NFVITransitionDtoGrid';
commit;

update gridcustomcolumn
set deleted=1 ,deletiondate=SYS_EXTRACT_UTC(systimestamp) 
where deleted = 0 and classname='VNFTransitionDtoGrid';
commit;

update gridcustomcolumn
set deleted=1 ,deletiondate=SYS_EXTRACT_UTC(systimestamp) 
where deleted = 0 and classname='VodafoneNameDtoGrid';
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
where deleted = 0 and classname='ViaExport';
commit;

update gridcustomcolumn
set deleted=1 ,deletiondate=SYS_EXTRACT_UTC(systimestamp) 
where deleted = 0 and classname='ArchivedLcmengineeringDtoGrid	';
commit;

update gridcustomcolumn
set deleted=1 ,deletiondate=SYS_EXTRACT_UTC(systimestamp) 
where deleted = 0 and classname='SettingsUpdatePlannedActivityDtoGrid	';
commit;