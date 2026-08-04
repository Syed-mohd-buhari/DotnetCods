------------------------ Aspnetuser , ASPNETUSERROLES from SUBDOMAINSPOC  --------------------
---- Delete from ASPNetUser table for the users listed as SubdomainSPOC (Comprises of SubDomainSPOC and EDU SPOC)
Delete from aspnetuserroles where UserId in (Select Distinct usr.ID from SystemTypesSubDomainSPOC STSDS JOIN subdomainspocs SDS on SDS.SubDomainSPOCID = STSDS.SubDomainSPOCID JOIN SystemTypes st on STSDS.SYSTEMTYPEID = st.SystemTypeId JOIN DesignComponents dc on dc.SystemTypeId = st.SystemTypeId JOIN LCMENGINEERING lcm on lcm.DesignComponentId = dc.DesignComponentId JOIN OPCOS op on lcm.opcoid = op.opcoid  JOIN aspnetusers usr on usr.email = SDS.SubDomainSPOC JOIN VerticalResponsibles v on st.VERTICALRESPONSIBLEID = v.VERTICALRESPONSIBLEID);

COMMIT;

INSERT INTO aspnetuserroles (USERID, ROLEID,OPCOID,VerticalResponsibleID) Select Distinct usr.ID,1, OP.OPCOID,v.VerticalResponsibleID from SystemTypesSubDomainSPOC STSDS JOIN subdomainspocs SDS on SDS.SubDomainSPOCID = STSDS.SubDomainSPOCID JOIN SystemTypes st on STSDS.SYSTEMTYPEID = st.SystemTypeId JOIN DesignComponents dc on dc.SystemTypeId = st.SystemTypeId JOIN LCMENGINEERING lcm on lcm.DesignComponentId = dc.DesignComponentId JOIN OPCOS op on lcm.opcoid = op.opcoid JOIN aspnetusers usr on usr.email = SDS.SubDomainSPOC JOIN VerticalResponsibles v on st.VERTICALRESPONSIBLEID = v.VERTICALRESPONSIBLEID;

COMMIT; 
 
INSERT INTO ORGANISATION (MAINORGANISATIONID,PRACTICEID,SUBDOMAINRESPONSIBLEID,CONTACTID,ISSUBDOMAINSPOC,ISEDUSPOC) SELECT distinct 1, 1,st.SUBDOMAINRESPONSIBLEID,u.ID as contact_id,sds.issubdomain  ,sds.isedu from systemtypes st join verticalresponsibles v on st.VERTICALRESPONSIBLEID = v.VERTICALRESPONSIBLEID join subdomainresponsibles sr on sr.SUBDOMAINRESPONSIBLEID = st.SUBDOMAINRESPONSIBLEID join systemtypessubdomainspoc ssp on st.SYSTEMTYPEID = ssp.SYSTEMTYPEID join subdomainspocs sds on ssp.SUBDOMAINSPOCID = sds.SUBDOMAINSPOCID join aspnetusers u on sds.SUBDOMAINSPOC = u.email ;
 
COMMIT;