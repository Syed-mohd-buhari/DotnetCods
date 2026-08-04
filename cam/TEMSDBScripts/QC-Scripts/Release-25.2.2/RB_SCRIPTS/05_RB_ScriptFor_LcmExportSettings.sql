------------ROLLBACK SCRIPT FOR LCMEXPORTSETTINGS------------------

alter table lcmexportsettings drop column LcmHistoricalInfo;
alter table lcmexportsettings drop column IsHistorical;
alter table lcmexportsettings drop column IsCurrent;
alter table lcmexportsettings drop column ReportLevel;
alter table lcmexportsettings drop column ReportType;
Commit;