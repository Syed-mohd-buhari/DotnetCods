Insert into projectplanaudit (projectsplanid,oldvalue,newvalue,processtype,creationdate,creationuser,modificationdate,modificationuser)
select projectsplanid,PLANNINGENDDATE,PLANNINGENDDATE,1,creationdate,creationuser,modificationdate,modificationuser
from projectsplan
WHERE projectsplanid NOT IN (
    select projectsplanid FROM projectplanaudit
);