------------------ Below script is for adding new Columns for Weekly Reporting
ALTER TABLE DynamicReports ADD SCHEDULEDDAYINWEEK NVARCHAR2(50);
ALTER TABLE DynamicReports ADD SCHEDULEDTYPE NUMBER(5);

COMMIT;