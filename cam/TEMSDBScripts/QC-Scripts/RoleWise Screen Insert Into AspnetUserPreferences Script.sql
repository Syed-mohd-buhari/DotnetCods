--Admin role
-- select * from aspnetroles where name in ('Admin', 'User', 'TEMS Functional Admin', 'TEMS System Owner');
--select ID, NAME from aspnetroles where name in ('Admin', 'User', 'TEMS Functional Admin', 'TEMS System Owner');
 
-- Test Query
 
SELECT DISTINCT
ur.id,  m.aspnetmoduleid, 7,    0,    0,    1,    1
FROM aspnetusers ur
JOIN aspnetuserroles ur1  ON ur1.userid = ur.id
JOIN aspnetroles r1  ON ur1.userid = r1.id
JOIN aspnetmodules m  ON m.modulepath = '/exodusatglance'
WHERE r1.name in ('Admin', 'User', 'TEMS Functional Admin', 'TEMS System Owner')
AND NOT EXISTS
(
 SELECT 1  FROM aspnetuserpreferences rp
  WHERE rp.userid = ur.id  AND rp.aspnetmoduleid = m.aspnetmoduleid
);

  
----------------------------
INSERT INTO aspnetuserpreferences
(    userid, aspnetmoduleid,"PERMISSION","ORDER", isuserpreference, CREATIONUSER, MODIFICATIONUSER)
SSELECT DISTINCT
ur.id,  m.aspnetmoduleid, 7,    0,    0,    1,    1
FROM aspnetusers ur
JOIN aspnetuserroles ur1  ON ur1.userid = ur.id
JOIN aspnetroles r1  ON ur1.userid = r1.id
JOIN aspnetmodules m  ON m.modulepath = '/exodusatglance'
WHERE r1.name in ('Admin', 'User', 'TEMS Functional Admin', 'TEMS System Owner')
AND NOT EXISTS
(
 SELECT 1  FROM aspnetuserpreferences rp
  WHERE rp.userid = ur.id  AND rp.aspnetmoduleid = m.aspnetmoduleid
);

 
commit;

-------------------Admin role Below query is for paritcular Role id 
INSERT INTO aspnetuserpreferences  ( userid, aspnetmoduleid, "PERMISSION","ORDER", isuserpreference, CREATIONUSER, MODIFICATIONUSER)
SELECT   ur.id,  m.aspnetmoduleid, 7 as permission1,0 as order1,0 as isuserpreference, 1 as creationuser, 1 as modificationuser
FROM aspnetusers ur
JOIN aspnetuserroles r1  ON r1.userid = ur.id
JOIN aspnetmodules m ON m.modulepath = '/exodusatglance'
WHERE r1.roleid = 1 -- and ur.id = 1323
AND NOT EXISTS
(
SELECT 1 FROM aspnetuserpreferences rp
WHERE  rp.userid = ur.id AND
   rp.aspnetmoduleid = m.aspnetmoduleid
) ;

commit;