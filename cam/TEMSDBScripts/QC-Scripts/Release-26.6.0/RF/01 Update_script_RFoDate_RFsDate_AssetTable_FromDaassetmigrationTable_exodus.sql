/*Create backup*/
CREATE TABLE networkelementsasplanned_bk_16062026 AS 
SELECT NETWORKELEMENTASPLANNEDID,ASSETRFODATE,ASSETRFSDATE,ASSETLIVESTATUSDATE FROM networkelementsasplanned;

/*Create temp table */
drop table Temp_Daassetmigration;

CREATE TABLE Temp_Daassetmigration AS
SELECT 
    networkelementasplannedid,
    MAX(rfodate) AS rfodate ,
    MAX(rfsdate) AS rfsdate,
    MAX(BOMSUBMITTEDDATE) AS BOMSUBMITTEDDATE,
    MAX(HWPORAISEDDATE) AS HWPORAISEDDATE,
    MAX(HWPOARRIVEDDATE) AS HWPOARRIVEDDATE,
    MAX(RFADATE) AS RFADATE
FROM DAASSETMIGRATION
WHERE networkelementasplannedid IS NOT NULL
  AND NEWELEMENTNAME is NULL
  AND TARGETDESIGNCOMPONENETID IS NULL
  AND (deploymentstatusid NOT IN (1) or deploymentstatusid is null)
  AND (rfodate IS NOT NULL OR rfsdate IS NOT NULL OR BOMSUBMITTEDDATE IS NOT NULL OR HWPORAISEDDATE IS NOT NULL
  OR HWPOARRIVEDDATE IS NOT NULL OR RFADATE IS NOT NULL)
GROUP BY networkelementasplannedid;
COMMIT;

--SELECT * FROM Temp_Daassetmigration;
/*Update script */
MERGE INTO networkelementsasplanned D
USING Temp_Daassetmigration T
ON (T.networkelementasplannedid = D.networkelementasplannedid)
WHEN MATCHED THEN
UPDATE SET
    D.ASSETRFODATE = NVL(T.rfodate, D.ASSETRFODATE),
    D.ASSETRFSDATE = NVL(T.rfsdate, D.ASSETRFSDATE),
    D.BOMSUBMITTEDDATE = NVL(T.BOMSUBMITTEDDATE, D.BOMSUBMITTEDDATE),
    D.HWPORAISEDDATE = NVL(T.HWPORAISEDDATE, D.HWPORAISEDDATE),
    D.HWPOARRIVEDDATE = NVL(T.HWPOARRIVEDDATE, D.HWPOARRIVEDDATE),
    D.RFADATE = NVL(T.RFADATE, D.RFADATE) 
WHERE 
    T.rfodate IS NOT NULL
    OR T.rfsdate IS NOT NULL;
COMMIT;


  /*drop temp table */  
    --drop table Temp_Daassetmigration;
    --COMMIT;

