ALTER TABLE dynamicreports ADD ExportFilePath NVARCHAR2(2000);
ALTER TABLE dynamicreports ADD ExportFileFormat NVARCHAR2(100);
ALTER TABLE dynamicreports ADD SCHEDULEDDate NUMBER(10,0);
ALTER TABLE dynamicreports ADD ExportType NUMBER(5,0);
Commit;