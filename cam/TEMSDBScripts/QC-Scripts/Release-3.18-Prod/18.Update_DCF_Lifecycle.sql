INSERT INTO dcflifecycle ( opcoid, dcfid, dcid, "EventID", "EventName", resourcekey)
select distinct lcm.OPCOID, dc.designcomponentfamilyID,dc.designcomponentid , 0 as EventID, 'Initial Baseline' as EventName, lcm.resourcekey
from lcmengineering lcm join designcomponents dc  on  dc.designcomponentid = lcm.designcomponentid
join NETWORKELEMENTSASPLANNED asset on lcm.lcmengineeringid = asset.lcmengineeringid
Where dc.designcomponentfamilyID is not null and asset.deploymentstatusid in (select deploymentstatusid from deploymentstatuses where DeploymentStatus = 'IN-SERVICE')
order by opcoid, designcomponentfamilyID;

INSERT INTO dcflifecycle ( opcoid, dcfid,details,  "EventID", "EventName", resourcekey)
Select distinct rk.opcoid, rk.dcfid,np.elementname, 0, 'Start Of Life Cycle', np.SWresourcekey 
from ResourceKeymaster rk join NETWORKELEMENTSASPLANNED np on rk.resourcekey = np.SWresourcekey

INSERT INTO dcflifecycle ( opcoid, dcfid,details,  "EventID", "EventName", resourcekey)
Select distinct rk.opcoid, rk.dcfid,np.elementname, 0, 'Start Of Life Cycle', np.HWresourcekey 
from ResourceKeymaster rk join NETWORKELEMENTSASPLANNED np on rk.resourcekey = np.HWresourcekey

INSERT INTO dcflifecycle ( opcoid, dcfid,details,  "EventID", "EventName", resourcekey)
Select distinct rk.opcoid, rk.dcfid,np.elementname, 0, 'Start Of Life Cycle', i.resourcekey 
from ResourceKeymaster rk join identitiesasis i on rk.resourcekey = i.resourcekey
join NETWORKELEMENTSASPLANNED np on i.assetid = np.networkelementasplannedid

commit;

MERGE INTO dcflifecycle dcf using 
(
        Select DesignComponentId, Regexp_replace(OriginalEquipmentManufacturer || ' ' || p.description || ' ' || msb.SoftwareVersion || '<b class=\"text-lowercase\"> on</b> ' ||
        mb.hardwaresolution ||' ' || pf.platform ||' '|| mb.hardwaretype || '<b class=\"text-lowercase\" > for</b> ' ||
        snb.alias, '<.+?>') as DCName
        from designcomponents dc
        join designcomponentfamilies dcf on dc.designcomponentfamilyid = dcf.designcomponentfamilyid
        join productname p on dcf.productnameid = p.productnameid
        join systemtypes st on dc.systemtypeid = st.systemtypeid
        join Systemtypesmajorhardwarebuilds mh on mh.systemtypeid = st.systemtypeid
        join majorsoftwarebuilds msb on msb.majorsoftwarebuildsid = st.majorsoftwarebuildsid
        join MajorHardwareBuilds mb on mh.majorhardwareid = mb.majorhardwareid
        join platforms pf on mb.platformId = pf.platformId
        join buildconstructions b on b.buildconstructionid = mb.buildconstructionid
        join OriginalEquipmentManufacturers oem on oem.orgeqpmanufacturerid = msb.orgeqpmanufacturerid
        join subnetworkboundaries snb on snb.id = dcf.subnetworkboundaryid
) src
on (dcf.DCID = src.DesignComponentId)
WHEN MATCHED THEN UPDATE
     SET dcf.details = DCName;
Commit;

MERGE INTO dcflifecycle dcf using 
(
        Select DesignComponentId, Regexp_replace(OriginalEquipmentManufacturer || ' ' || p.description || ' ' || msb.SoftwareVersion || '<b class=\"text-lowercase\"> on</b> ' ||
        pf.platform || '<b class=\"text-lowercase\" > for</b> ' || snb.alias, '<.+?>') as DCName
        from designcomponents dc
        join designcomponentfamilies dcf on dc.designcomponentfamilyid = dcf.designcomponentfamilyid
        join productname p on dcf.productnameid = p.productnameid
        join systemtypes st on dc.systemtypeid = st.systemtypeid
        join Systemtypesmajorhardwarebuilds mh on mh.systemtypeid = st.systemtypeid
        join majorsoftwarebuilds msb on msb.majorsoftwarebuildsid = st.majorsoftwarebuildsid
        join MajorHardwareBuilds mb on mh.majorhardwareid = mb.majorhardwareid
        join platforms pf on mb.platformId = pf.platformId
        join buildconstructions b on b.buildconstructionid = mb.buildconstructionid
        join OriginalEquipmentManufacturers oem on oem.orgeqpmanufacturerid = msb.orgeqpmanufacturerid
        join subnetworkboundaries snb on snb.id = dcf.subnetworkboundaryid
        where buildconstruction in ('BLUEPRINT NFVI','OTHER NFVI','BLUEPRINT NFCI','OTHER NFCI')
) src
on (dcf.DCID = src.DesignComponentId)
WHEN MATCHED THEN UPDATE
     SET dcf.details = DCName;
Commit;
