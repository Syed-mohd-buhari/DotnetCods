---------------- Add New column in DC Table ----------------

alter table designcomponents add VisibleFlag NUMBER(1,0)default 1;
commit;

-- update designcomponents set visibleflag = 0 where subnetworkboundaryid in (select id from subnetworkboundaries where alias ='All Supported Services');
--commit;