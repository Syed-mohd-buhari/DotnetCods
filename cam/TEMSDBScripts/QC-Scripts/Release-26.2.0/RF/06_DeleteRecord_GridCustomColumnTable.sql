delete from gridcustomcolumn where classname='GenericReportGridCreateDto';
delete from gridcustomcolumn where classname='BuildBagDtoGrid';

----------------ALTER SCRIPT FOR BUILDBAGS------------------
ALTER TABLE "BUILDBAGS" ADD "VISIBLEFLAG" NUMBER(1,0) DEFAULT 1;
commit;
ALTER TABLE DYNAMICREPORTS ADD ISTESTNODEREQUIRED NUMBER(1,0) DEFAULT 0;
commit;

update Buildbags set visibleflag=0 where bagdescription='Empty' and bagversion='1.0';
commit;




