--------------------------------------------------------
--  ROLLBACK SCRIPT FOR TABLE BUILDBAGS
--------------------------------------------------------

DROP SEQUENCE BUILDBAGID_SEQ;
DROP TABLE BUILDBAGS;
COMMIT;

------------------------END---------------------------