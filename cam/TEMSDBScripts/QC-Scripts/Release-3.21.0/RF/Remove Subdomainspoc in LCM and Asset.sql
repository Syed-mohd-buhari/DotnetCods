--- LCMENGINEERINGSUBDOMAINSPOC -------------

delete from  lcmengineeringsubdomainspoc where lcmengineeringsubdomainspocid in (select lcmengineeringsubdomainspocid from ( select lcmengineeringsubdomainspocid,  lcmengineeringid , 
  row_number() over (partition by lcmengineeringid order by lcmengineeringsubdomainspocid )as rank
from lcmengineeringsubdomainspoc ) a where a.rank != 1   )
;

--- NETWORKELEMENTASPLANNEDSUBDOMAINSPOC -------------

delete from  networkelementasplannedsubdomainspoc where NTKELEMENTASPLNSUBDOMAINSPOCID in 
(select NTKELEMENTASPLNSUBDOMAINSPOCID from ( select NTKELEMENTASPLNSUBDOMAINSPOCID,  NETWORKELEMENTASPLANNEDID , 
  row_number() over (partition by NETWORKELEMENTASPLANNEDID order by NTKELEMENTASPLNSUBDOMAINSPOCID )as rank
from networkelementasplannedsubdomainspoc)a where a.rank != 1   )