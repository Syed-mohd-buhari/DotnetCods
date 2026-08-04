--- Add PortalRoleid from Aspnetroles table ----------------

ALTER TABLE ASPNETROLES ADD PORTALROLEID NUMBER(10,0);

update aspnetroles set portalroleid = 1 where   name  in ('SW Product Owner') ; 
update aspnetroles set portalroleid = 2 where  name  in ('HW Product Owner') ; 
commit;