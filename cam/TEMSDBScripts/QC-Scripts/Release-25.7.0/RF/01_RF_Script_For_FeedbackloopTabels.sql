-- Update Raw Tables -- 
UPDATE NETWORKELEMENT SET OPCO = 'UK',OEM = 'ERICSSON';
UPDATE IDENTITIES SET OPCO = 'UK',OEM = 'ERICSSON';
UPDATE SOFTWARECOMPONENT SET OPCO = 'UK',OEM = 'ERICSSON';
UPDATE HARDWARECONFIGURATION SET OPCO = 'UK',OEM = 'ERICSSON';
UPDATE SOFTWARECONFIGURATION SET OPCO = 'UK',OEM = 'ERICSSON';
-- Update Raw Audti Tables -- 
UPDATE AUDITHISTORY SET OPCO = 'UK',OEM = 'ERICSSON';
UPDATE FEEDBACKLOOPAUDITS SET OPCO = 'UK',OEM = 'ERICSSON';
UPDATE NODEPARSEHISTORY SET OPCO = 'UK',OEM = 'ERICSSON';


-- Clean up Worklog approvel table --

DELETE FROM AUDITHISTORY WHERE COLUMNNAME NOT IN ('softwareproductnumber','softwareproductdate',
'softwareinstalldate',
'dataacquisitiondate',
'nodetype',
'platformtype',
'softwarereleaseinformation',
'spare1ossorenm',
'hardwaretype',
'ipaddress',
'Softwareversion'
);

COMMIT;