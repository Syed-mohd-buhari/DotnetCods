--- EXCELTEMPLATECONFIGURATION - for BPT import ---------

Update exceltemplateconfiguration set ISIMPORTFIELD = 1 where propertyName = 'currentTrackingNumber';
Update exceltemplateconfiguration set ISIMPORTFIELD = 0 where propertyName = 'wbs';
Update exceltemplateconfiguration set ISIMPORTFIELD = 0 where propertyName = 'opco';
Update exceltemplateconfiguration set ISIMPORTFIELD = 0 where propertyName = 'domain';
Update exceltemplateconfiguration set ISIMPORTFIELD = 0 where propertyName = 'team';
Update exceltemplateconfiguration set ISIMPORTFIELD = 0 where propertyName = 'budgetOwner';
Update exceltemplateconfiguration set ISIMPORTFIELD = 0 where propertyName = 'program';
Update exceltemplateconfiguration set ISIMPORTFIELD = 0 where propertyName = 'budgetProject';
Update exceltemplateconfiguration set ISIMPORTFIELD = 0 where propertyName = 'activity';
Update exceltemplateconfiguration set ISIMPORTFIELD = 0 where propertyName = 'priority';
Update exceltemplateconfiguration set ISIMPORTFIELD = 0 where propertyName = 'driver';
Update exceltemplateconfiguration set ISIMPORTFIELD = 0 where propertyName = 'benefits';
Update exceltemplateconfiguration set ISIMPORTFIELD = 0 where propertyName = 'risks';
Update exceltemplateconfiguration set ISIMPORTFIELD = 0 where propertyName = 'category';
Update exceltemplateconfiguration set ISIMPORTFIELD = 0 where propertyName = 'nwelement';
Update exceltemplateconfiguration set ISIMPORTFIELD = 0 where propertyName = 'virtualizedNwElement';
Update exceltemplateconfiguration set ISIMPORTFIELD = 0 where propertyName = 'vendor';
Update exceltemplateconfiguration set ISIMPORTFIELD = 0 where propertyName = 'lcmCategories';

UPDATE  EXCELTEMPLATECONFIGURATION  SET ISEXPORTFIELD = 1 WHERE propertyName = 'currentTrackingNumber' ;

Commit;
