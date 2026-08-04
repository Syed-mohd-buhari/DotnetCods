		
--INSERT INTO ORG
INSERT INTO ORGANISATION (MAINORGANISATIONID,PRACTICEID,SUBDOMAINRESPONSIBLEID,CONTACTID,ISSUBDOMAINSPOC,ISEDUSPOC)
SELECT distinct 1, 1,st.SUBDOMAINRESPONSIBLEID,u.ID as contact_id,sds.issubdomain  ,sds.isedu 
from SYSTEMTYPES st 
join subdomainresponsibles sr on sr.SUBDOMAINRESPONSIBLEID = st.SUBDOMAINRESPONSIBLEID 
join systemtypessubdomainspoc ssp on st.SYSTEMTYPEID = ssp.SYSTEMTYPEID 
join subdomainspocs sds on ssp.SUBDOMAINSPOCID = sds.SUBDOMAINSPOCID 
join ASPNETUSERS u on upper(regexp_replace(sds.SUBDOMAINSPOC,' ','')) = upper(regexp_replace(u.email,' ',''));
COMMIT;

---VALIDATION FOR ORG TEMP TABLE 
SELECT CASE WHEN COUNT (DISTINCT SDS.SUBDOMAINSPOCID) = 0 THEN 'ALL SUBDOMAIN SPOC MIGRATED' ELSE
        COUNT (DISTINCT SDS.SUBDOMAINSPOCID)||' '||'NUMBER OF SUBDOMAIN USER IS NOT MIGRATED' END RES FROM 
systemtypessubdomainspoc ssp
LEFT join subdomainspocs sds on ssp.SUBDOMAINSPOCID = sds.SUBDOMAINSPOCID 
LEFT join ASPNETUSERS u on upper(regexp_replace(sds.SUBDOMAINSPOC,' ','')) = upper(regexp_replace(u.email,' ',''))
LEFT join ORGANISATION d on d.contactid = u.id
WHERE D.CONTACTID IS NULL;

