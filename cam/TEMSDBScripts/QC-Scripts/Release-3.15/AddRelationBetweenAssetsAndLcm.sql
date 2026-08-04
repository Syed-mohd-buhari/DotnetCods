ALTER TABLE NETWORKELEMENTSASPLANNED ADD
(
LCMENGINEERINGID NUMBER(19,0) NULL
);

COMMIT;

ALTER  TABLE NETWORKELEMENTSASPLANNED ADD CONSTRAINT "FK_NETWORKELEMETSASPLANNED_LCMENGINEERING_LCMENGINEERINGID" FOREIGN KEY ("LCMENGINEERINGID")
REFERENCES LCMENGINEERING("LCMENGINEERINGID") ENABLE;
COMMIT;

update networkelementsasplanned asset
set asset.LCMENGINEERINGID = ( select lcm.lcmengineeringid from lcmengineering lcm where lcm.opcoid = asset.opcoid and lcm.designcomponentid = asset.designcomponentid order by lcm.modificationdate desc fetch first 1 row only)
where
EXISTS(select lcm.LCMENGINEERINGID from lcmengineering lcm where lcm.opcoid = asset.opcoid and lcm.designcomponentid = asset.designcomponentid)
and EXISTS(select networkelementasplannedid from networkelementsasplanned join deploymentstatuses status on asset.deploymentstatusid = status.deploymentstatusid and status.deploymentstatus ='IN-SERVICE');

 commit;