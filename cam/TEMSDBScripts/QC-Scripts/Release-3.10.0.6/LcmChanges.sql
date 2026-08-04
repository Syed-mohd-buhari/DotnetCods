ALTER TABLE LCMENGINEERING
	ADD "LCMSPREADSHEETHWID" NVARCHAR2(1000);

	ALTER TABLE LCMENGINEERING
	ADD "LCMSPREADSHEETSWID" NVARCHAR2(1000);

 

 update gridcustomcolumn
set deleted=1 ,deletiondate=SYS_EXTRACT_UTC(systimestamp) 
where deleted = 0 and classname='ReportSoftwareDtoGrid';
commit;

update gridcustomcolumn
set deleted=1 ,deletiondate=SYS_EXTRACT_UTC(systimestamp) 
where deleted = 0 and classname='ReportHardwareDtoGrid';
commit;