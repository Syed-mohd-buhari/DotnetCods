------------ALTER SCRIPT FOR LCMEXPORTSETTINGS------------------

alter table lcmexportsettings add Lcmhistoricalinfosw Nclob;
alter table lcmexportsettings add Lcmhistoricalinfohw Nclob;
alter table lcmexportsettings add IsHistorical number(1,0) default 0;
alter table lcmexportsettings add IsCurrent number(1,0) default 0;
alter table lcmexportsettings add ReportLevel number(10,0) default 0;
Commit;