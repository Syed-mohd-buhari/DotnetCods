------------------------ Aspnetuser , ASPNETUSERROLES from SUBDOMAINSPOC  --------------------

DECLARE
subDomainEmail varchar(300); 
emailName varchar(300);
checkEmail Number;
CURSOR subdomainUserCursor IS      
select distinct ltrim(rtrim(subdomainspoc)) as email from subdomainspocs ;
Begin 
For subDomainUserRec in subdomainUserCursor
Loop 
select count(*) into checkEmail from aspnetusers where email = ltrim(rtrim(subDomainUserRec.email));
subDomainEmail := subDomainUserRec.email;
emailName := SUBSTR(subDomainUserRec.email, 1, INSTR(subDomainUserRec.email,'@') - 1);
IF checkEmail = 0 Then
insert into aspnetusers (username, normalizedusername,email,normalizedemail,emailconfirmed,phonenumberconfirmed,twofactorenabled,lockoutenabled,accessfailedcount,passwordhash,securitystamp,active,deleted) values (emailName,emailName,subDomainEmail,subDomainEmail,1,0,0,1,0,'AQAAAAEAACcQAAAAEByQ9on2IEJlaXfSviZLOIthoyRE9Xq7l8Qd1pB9vSKZaxJp6rZ65hmKzEzSHRY8UA==','867b421c-e23a-4c0d-9be8-b26da8f65feb',1,0);
End If;
End Loop;
COMMIT;
End;
/

---------------------------- Insert ASPNETUSERROLES -------------------------
 
DROP TABLE "ROLEMIGRATION_TMP";
CREATE TABLE "ROLEMIGRATION_TMP" 
(
"USERID" NUMBER(10,0), 	"ROLEID" NUMBER(10,0),	"VERTICALRESPONSIBLEID" NUMBER(10,0), 	"OPCOID" NUMBER(5,0)
);
DECLARE 
userid NUMBER(10,0);
roleid NUMBER(10,0);
opcoid NUMBER(10,0);
domainid NUMBER(10,0);
CURSOR userCursor IS 
select distinct  id  from aspnetusers  where  id not in (select distinct  userid  from aspnetuserroles);
CURSOR DomainCursor IS
select distinct OPCOID,VerticalresponsibleID from OpCos, verticalresponsibles;    
Begin
FOR UserRecords IN userCursor
LOOP
userid := UserRecords.id;
roleid := 4;
FOR DomainRecords IN DomainCursor
LOOP
opcoid := DomainRecords.OPCOID;
domainid := DomainRecords.VerticalresponsibleID;
INSERT INTO "ROLEMIGRATION_TMP" (USERID,ROLEID,VERTICALRESPONSIBLEID,OPCOID) Values (userid,roleid,domainid,opcoid);
END LOOP;
END LOOP;
COMMIT; 
INSERT INTO ASPNETUSERROLES ("USERID" ,"ROLEID" ,"VERTICALRESPONSIBLEID", "OPCOID",DELETED)SELECT DISTINCT USERID,ROLEID,VERTICALRESPONSIBLEID,OPCOID,0 from "ROLEMIGRATION_TMP";
COMMIT;
END; 
/
-----------------------------Drop ROLEMIGRATION_TMP-------------------------------------
DROP TABLE "ROLEMIGRATION_TMP";
COMMIT;

------------------ ORGANISATION table -----------------
 
  
INSERT INTO ORGANISATION (MAINORGANISATIONID,PRACTICEID,SUBDOMAINRESPONSIBLEID,CONTACTID,ISSUBDOMAINSPOC,ISEDUSPOC) SELECT distinct 1, 1,st.SUBDOMAINRESPONSIBLEID,u.ID as contact_id,sds.issubdomain  ,sds.isedu from systemtypes st join verticalresponsibles v on st.VERTICALRESPONSIBLEID = v.VERTICALRESPONSIBLEID join subdomainresponsibles sr on sr.SUBDOMAINRESPONSIBLEID = st.SUBDOMAINRESPONSIBLEID join systemtypessubdomainspoc ssp on st.SYSTEMTYPEID = ssp.SYSTEMTYPEID join subdomainspocs sds on ssp.SUBDOMAINSPOCID = sds.SUBDOMAINSPOCID join aspnetusers u on sds.SUBDOMAINSPOC = u.username ;

COMMIT;
