 
delete from aspnetuserroles where deleted = 1 or opcoid is null or verticalresponsibleid is null;
commit;

delete from  aspnetuserroles where rowid in ( select rowid  from ( SELECT aspnetuserroleid, userid, roleid, opcoid,verticalresponsibleid, ROW_NUMBER() OVER ( PARTITION BY userid, roleid ORDER BY userid) AS rn  FROM aspnetuserroles  where roleid = 1  )   where rn > 1 ) ;
commit;

delete  from aspnetuserroles where rowid in ( select rowid  from ( SELECT aspnetuserroleid, userid, roleid, opcoid,verticalresponsibleid, ROW_NUMBER() OVER ( PARTITION BY userid, roleid  ORDER BY userid) AS rn  FROM aspnetuserroles  where roleid != 1  )   where rn > 1 ) ;  
commit; 

delete from  aspnetuserroles where rowid in ( select rowid  from ( SELECT aspnetuserroleid, userid, roleid, ROW_NUMBER() OVER ( PARTITION BY userid ORDER BY userid) AS rn  FROM aspnetuserroles  where roleid != 1  )   where rn > 1 ) ;
commit;
 
--select   aspnetuserroles where deleted = 1 or opcoid is null or verticalresponsibleid is null;

--select   aspnetuserroleid, userid, roleid, opcoid,verticalresponsibleid  from aspnetuserroles where rowid in ( select rowid  from ( SELECT aspnetuserroleid, userid, roleid, opcoid,verticalresponsibleid, ROW_NUMBER() OVER ( PARTITION BY userid, roleid ORDER BY userid) AS rn  FROM aspnetuserroles  where roleid = 1  )   where rn > 1 ) ;


--select aspnetuserroleid, userid, roleid, opcoid,verticalresponsibleid  from aspnetuserroles where rowid in ( select rowid  from ( SELECT aspnetuserroleid, userid, roleid, opcoid,verticalresponsibleid, ROW_NUMBER() OVER ( PARTITION BY userid, roleid  ORDER BY userid) AS rn  FROM aspnetuserroles  where roleid != 1  )   where rn > 1 ) ;

-- select *  from aspnetuserroles where rowid in ( select rowid  from ( SELECT aspnetuserroleid, userid, roleid, opcoid  ,verticalresponsibleid, ROW_NUMBER() OVER  ( PARTITION BY userid  ORDER BY userid) AS rn  FROM aspnetuserroles  where roleid != 1    )   where rn > 1 ) ;