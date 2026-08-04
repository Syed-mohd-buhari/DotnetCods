
ALTER TABLE MAJORHARDWAREBUILDS
Add "GENERAAVAILABLEDATE" Date NULL  ;


delete from gridcustomcolumn where classname like 'MajorHardwareBuildDtoGrid';

commit ;