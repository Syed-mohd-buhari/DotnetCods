ALTER TABLE "BUILDBAGS" ADD "DESIGNCOMPONENTFAMILYID" NUMBER(19,0);
ALTER TABLE "BUILDBAGS" ADD "OPCOID" NUMBER(5,0);

ALTER TABLE "BUILDBAGS" ADD CONSTRAINT "FK_BUILDBAGS_DESIGNCOMPONENTFAMILIES_DESIGNCOMPONENTFAMILYID" FOREIGN KEY ("DESIGNCOMPONENTFAMILYID") REFERENCES "DESIGNCOMPONENTFAMILIES" ("DESIGNCOMPONENTFAMILYID") ENABLE;
ALTER TABLE "BUILDBAGS" ADD CONSTRAINT "FK_BUILDBAGS_OPCOS_OPCOID" FOREIGN KEY ("OPCOID") REFERENCES "OPCOS" ("OPCOID") ENABLE;

--------------------- SEt Bag Opcoid and DCFId

UPDATE buildbags bb
SET bb.opcoid = ( SELECT o.opcoid 
FROM opcos o 
WHERE o.opco = 'ZZZ'
),
bb.DESIGNCOMPONENTFAMILYID = (
SELECT dc.designcomponentfamilyid
FROM lcmengineering lcm
JOIN designcomponents dc ON dc.designcomponentid = lcm.designcomponentid
 WHERE ROWNUM = 1
);


COMMIT;

------------------------ Update BAgName remove productname adn OEM contact value
MERGE INTO buildbags b
USING (
  SELECT rid, new_desc
  FROM (
    SELECT b.ROWID AS rid,
           SUBSTR(b.BAGDESCRIPTION,
                  LENGTH(oe.ORIGINALEQUIPMENTMANUFACTURER || '-' || pn.DESCRIPTION || '-') + 1) AS new_desc,
           ROW_NUMBER() OVER (PARTITION BY b.ROWID ORDER BY oe.ORIGINALEQUIPMENTMANUFACTURER) AS rn
    FROM buildbags b
    JOIN originalequipmentmanufacturers oe ON b.BAGDESCRIPTION LIKE oe.ORIGINALEQUIPMENTMANUFACTURER || '%'
    JOIN productname pn ON b.BAGDESCRIPTION LIKE oe.ORIGINALEQUIPMENTMANUFACTURER || '-' || pn.DESCRIPTION || '%'
  )
  WHERE rn = 1
) src
ON (b.ROWID = src.rid)
WHEN MATCHED THEN
  UPDATE SET b.BAGDESCRIPTION = src.new_desc where src.new_desc is not null;

----------------------------- Change 