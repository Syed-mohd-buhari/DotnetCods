EXECUTE update_RESOURCEKEY_RESOURCEKEYMASTER;
select count(RESOURCEKEY) from RESOURCEKEYMASTER where RESOURCEKEY is null;
--If result is greater than zero, run the procedure again;
commit;

