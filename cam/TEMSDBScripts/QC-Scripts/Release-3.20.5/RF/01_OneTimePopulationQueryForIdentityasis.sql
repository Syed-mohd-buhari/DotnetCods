MERGE INTO identitiesasis tgt
USING ( SELECT DISTINCT
            iden.ipaddress ip,idasis.ID
        FROM
                 identities iden
            JOIN opcos                          op ON replace(iden.opco, 'VODAFONE_UK', 'UK') = upper(op.opco)
            JOIN originalequipmentmanufacturers oem ON upper(oem.originalequipmentmanufacturer) = upper(iden.oem)
            JOIN networkelementsasplanned       ass ON ass.opcoid = op.opcoid
                                                 AND ass.orgeqpmanufacturerid = oem.orgeqpmanufacturerid
                                                 AND ass.elementname = iden.elementname
            INNER JOIN identitiesasis idasis on idasis.assetid = ass.networkelementasplannedid
            where iden.ipaddress <> idasis.value and idasis.categoryid = '1'
    ) src on (tgt.id = src.id)
    when matched then update set tgt.value = src.ip;
COMMIT;