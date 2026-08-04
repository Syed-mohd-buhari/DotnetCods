---------------------------------- Master Table Records 


--- Clustername Tabel Record  ----

INSERT  INTO clustername (CLUSTERDESCRIPTION, CLUSTERTYPE, CREATIONUSER, MODIFICATIONUSER) VALUES ('Sig Cluster 1', '1', '1','1');
INSERT  INTO clustername (CLUSTERDESCRIPTION, CLUSTERTYPE, CREATIONUSER, MODIFICATIONUSER) VALUES ('SCDE Cluster 1', '1', '1','1');
INSERT  INTO clustername (CLUSTERDESCRIPTION, CLUSTERTYPE, CREATIONUSER, MODIFICATIONUSER) VALUES ('Media Cluster 1', '1', '1','1'); 
INSERT  INTO clustername (CLUSTERDESCRIPTION, CLUSTERTYPE, CREATIONUSER, MODIFICATIONUSER) VALUES ('Sig Cluster 2', '1', '1','1');
INSERT  INTO clustername (CLUSTERDESCRIPTION, CLUSTERTYPE, CREATIONUSER, MODIFICATIONUSER) VALUES ('SCDE Cluster 2', '1', '1','1');
INSERT  INTO clustername (CLUSTERDESCRIPTION, CLUSTERTYPE, CREATIONUSER, MODIFICATIONUSER) VALUES ('Media Cluster 2', '1', '1','1'); 
 Commit;
 
--- Location Tabel Short description column update  ----

UPDATE locations l 
SET shortdescription = (
SELECT UPPER(SUBSTR(s.sitecode, 2, 2))
FROM sites s
WHERE s.locationid = l.locationid
AND LENGTH(s.sitecode) = 3
AND s.sitecode LIKE 'X%'
)WHERE l.locationid IN (
SELECT s.locationid
FROM sites s
WHERE LENGTH(s.sitecode) = 3
AND s.sitecode LIKE 'X%'
AND s.locationid IN (
SELECT loc.locationid FROM locations loc WHERE loc.opcoid IN (  SELECT o.opcoid  FROM opcos o
WHERE LOWER(o.opco) = 'uk'   )  ));

 Commit;
------------------- vnfname table records
INSERT INTO vnfname (VNFDESCRIPTION, PRODUCTID, CREATIONUSER, MODIFICATIONUSER)
SELECT 'vA-BGF VoLTE', p.productnameid, 1, 1 FROM productname p WHERE p.description = 'A-BGF' ;

INSERT INTO vnfname (VNFDESCRIPTION, PRODUCTID, CREATIONUSER, MODIFICATIONUSER)
SELECT 'DRA', p.productnameid, 1, 1 FROM productname p WHERE p.description = 'DRA';

INSERT INTO vnfname (VNFDESCRIPTION, PRODUCTID, CREATIONUSER, MODIFICATIONUSER)
SELECT 'vAFG', p.productnameid, 1, 1 FROM productname p WHERE p.description = 'AFG';

INSERT INTO vnfname (VNFDESCRIPTION, PRODUCTID, CREATIONUSER, MODIFICATIONUSER)
SELECT 'vCSCF VoLTE', p.productnameid, 1, 1 FROM productname p WHERE p.description = 'CSCF';

INSERT INTO vnfname (VNFDESCRIPTION, PRODUCTID, CREATIONUSER, MODIFICATIONUSER)
SELECT 'vMRF VoLTE', p.productnameid, 1, 1 FROM productname p WHERE p.description = 'MRF';

INSERT INTO vnfname (VNFDESCRIPTION, PRODUCTID, CREATIONUSER, MODIFICATIONUSER)
SELECT 'vMTAS', p.productnameid, 1, 1 FROM productname p WHERE p.description = 'MTAS';

INSERT INTO vnfname (VNFDESCRIPTION, PRODUCTID, CREATIONUSER, MODIFICATIONUSER)
SELECT 'vMTAS VoLTE', p.productnameid, 1, 1 FROM productname p WHERE p.description = 'MTAS';

INSERT INTO vnfname (VNFDESCRIPTION, PRODUCTID, CREATIONUSER, MODIFICATIONUSER)
SELECT 'vSAPC (VoLTE)', p.productnameid, 1, 1 FROM productname p WHERE p.description = 'SAPC';

INSERT INTO vnfname (VNFDESCRIPTION, PRODUCTID, CREATIONUSER, MODIFICATIONUSER)
SELECT 'vSBG VoLTE', p.productnameid, 1, 1 FROM productname p WHERE p.description = 'SBG';

 Commit;
 
------------------------- VMTYPENAME table insert

INSERT  INTO VMTYPENAME (vmtypedescription ,VNFNAMEID ,CREATIONUSER, MODIFICATIONUSER) 
select 'BGF', p.vnfnameid ,1 ,1 from vnfname  p where p.vnfdescription = 'vA-BGF VoLTE';

INSERT  INTO VMTYPENAME ( vmtypedescription ,VNFNAMEID,CREATIONUSER, MODIFICATIONUSER)  select 'SC', p.vnfnameid ,1 ,1 from vnfname  p where p.vnfdescription = 'DRA';

INSERT  INTO VMTYPENAME ( vmtypedescription ,VNFNAMEID,CREATIONUSER, MODIFICATIONUSER)  select 'PL', p.vnfnameid ,1 ,1 from vnfname  p where p.vnfdescription = 'DRA';
INSERT  INTO VMTYPENAME ( vmtypedescription ,VNFNAMEID,CREATIONUSER, MODIFICATIONUSER)  select 'MN', p.vnfnameid ,1 ,1 from vnfname  p where p.vnfdescription = 'vAFG';
INSERT  INTO VMTYPENAME ( vmtypedescription ,VNFNAMEID,CREATIONUSER, MODIFICATIONUSER)  select 'MON', p.vnfnameid ,1 ,1 from vnfname  p where p.vnfdescription = 'vAFG';
INSERT  INTO VMTYPENAME ( vmtypedescription ,VNFNAMEID,CREATIONUSER, MODIFICATIONUSER)  select 'TS', p.vnfnameid ,1 ,1 from vnfname  p where p.vnfdescription = 'vAFG';
INSERT  INTO VMTYPENAME ( vmtypedescription ,VNFNAMEID,CREATIONUSER, MODIFICATIONUSER)  select 'DDC', p.vnfnameid ,1 ,1 from vnfname  p where p.vnfdescription = 'vAFG';
INSERT  INTO VMTYPENAME ( vmtypedescription ,VNFNAMEID,CREATIONUSER, MODIFICATIONUSER)  select 'SLB', p.vnfnameid ,1 ,1 from vnfname  p where p.vnfdescription = 'vAFG';
INSERT  INTO VMTYPENAME ( vmtypedescription ,VNFNAMEID,CREATIONUSER, MODIFICATIONUSER)  select 'SC', p.vnfnameid ,1 ,1 from vnfname  p where p.vnfdescription = 'vCSCF';
INSERT  INTO VMTYPENAME ( vmtypedescription ,VNFNAMEID,CREATIONUSER, MODIFICATIONUSER)  select 'PL', p.vnfnameid ,1 ,1 from vnfname  p where p.vnfdescription = 'vCSCF';
INSERT  INTO VMTYPENAME ( vmtypedescription ,VNFNAMEID,CREATIONUSER, MODIFICATIONUSER)  select 'SC', p.vnfnameid ,1 ,1 from vnfname  p where p.vnfdescription = 'vCSCF VoLTE';
INSERT  INTO VMTYPENAME ( vmtypedescription ,VNFNAMEID,CREATIONUSER, MODIFICATIONUSER)  select 'PL', p.vnfnameid ,1 ,1 from vnfname  p where p.vnfdescription = 'vCSCF VoLTE';
INSERT  INTO VMTYPENAME ( vmtypedescription ,VNFNAMEID,CREATIONUSER, MODIFICATIONUSER)  select 'MRF', p.vnfnameid ,1 ,1 from vnfname  p where p.vnfdescription = 'vMRF VoLTE';
INSERT  INTO VMTYPENAME ( vmtypedescription ,VNFNAMEID,CREATIONUSER, MODIFICATIONUSER)  select 'SC', p.vnfnameid ,1 ,1 from vnfname  p where p.vnfdescription = 'vMTAS';
INSERT  INTO VMTYPENAME ( vmtypedescription ,VNFNAMEID,CREATIONUSER, MODIFICATIONUSER)  select 'PL', p.vnfnameid ,1 ,1 from vnfname  p where p.vnfdescription = 'vMTAS';
INSERT  INTO VMTYPENAME ( vmtypedescription ,VNFNAMEID,CREATIONUSER, MODIFICATIONUSER)  select 'SC', p.vnfnameid ,1 ,1 from vnfname  p where p.vnfdescription = 'vMTAS VoLTE';
INSERT  INTO VMTYPENAME ( vmtypedescription ,VNFNAMEID,CREATIONUSER, MODIFICATIONUSER)  select 'PL', p.vnfnameid ,1 ,1 from vnfname  p where p.vnfdescription = 'vMTAS VoLTE';
INSERT  INTO VMTYPENAME ( vmtypedescription ,VNFNAMEID,CREATIONUSER, MODIFICATIONUSER)  select 'SC', p.vnfnameid ,1 ,1 from vnfname  p where p.vnfdescription = 'vSAPC (VoLTE)';
INSERT  INTO VMTYPENAME ( vmtypedescription ,VNFNAMEID,CREATIONUSER, MODIFICATIONUSER)  select 'PL', p.vnfnameid ,1 ,1 from vnfname  p where p.vnfdescription = 'vSAPC (VoLTE)';
INSERT  INTO VMTYPENAME ( vmtypedescription ,VNFNAMEID,CREATIONUSER, MODIFICATIONUSER)  select 'SC', p.vnfnameid ,1 ,1 from vnfname  p where p.vnfdescription = 'vSBG VoLTE';
INSERT  INTO VMTYPENAME ( vmtypedescription ,VNFNAMEID,CREATIONUSER, MODIFICATIONUSER)  select 'PL', p.vnfnameid ,1 ,1 from vnfname  p where p.vnfdescription = 'vSBG VoLTE';
INSERT  INTO VMTYPENAME ( vmtypedescription ,VNFNAMEID,CREATIONUSER, MODIFICATIONUSER)  select 'ADC', p.vnfnameid ,1 ,1 from vnfname  p where p.vnfdescription = 'vSBG VoLTE';

 Commit;