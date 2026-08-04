create or replace PROCEDURE update_RESOURCEKEY_RESOURCEKEYMASTER
IS   
   update_statement varchar2(1000);  
   random_generated varchar2(10);
   keyexist number:=0;
BEGIN
   DBMS_OUTPUT.PUT_LINE('START');
   FOR item IN (select RESOURCEKEYMASTERID from RESOURCEKEYMASTER where RESOURCEKEY is null)
   LOOP
           DBMS_OUTPUT.PUT_LINE(item.RESOURCEKEYMASTERID);
           execute immediate 'select GenerateRandomString(''1234567890ABCDEF'', 7) from dual connect by level = 7' into random_generated;
           DBMS_OUTPUT.PUT_LINE('Random Key Generated : ' || random_generated);
           execute immediate 'select count(RESOURCEKEYMASTERID) from RESOURCEKEYMASTER where RESOURCEKEY = RESOURCETYPESID || ''' || random_generated || '''';
           DBMS_OUTPUT.PUT_LINE('Key Exists : ' || keyexist);
           if keyexist = 0 THEN
               update_statement := 'update RESOURCEKEYMASTER set RESOURCEKEY = RESOURCETYPESID || ''' || random_generated || '''  where RESOURCEKEYMASTERID = ' || item.RESOURCEKEYMASTERID;
               DBMS_OUTPUT.PUT_LINE(update_statement);
               execute immediate update_statement;
               commit;
           else
               DBMS_OUTPUT.PUT_LINE('Skipping ID ' || item.RESOURCEKEYMASTERID || ' since random key (' || random_generated || ') exist');
           end if;
   END LOOP;
   DBMS_OUTPUT.PUT_LINE('END');

END update_RESOURCEKEY_RESOURCEKEYMASTER;

--set serveroutput on;
--EXECUTE update_RESOURCEKEY_RESOURCEKEYMASTER