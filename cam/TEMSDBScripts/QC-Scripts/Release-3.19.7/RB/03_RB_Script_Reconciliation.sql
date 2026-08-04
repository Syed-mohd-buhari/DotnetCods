----------------------------------------------------------
-- ROLLBACK SCRIPT FOR TABLE RECONCILIATIONS
----------------------------------------------------------
DROP SEQUENCE "RECONCILIATION_SEQ";

DROP TABLE "RECONCILIATIONS";

COMMIT;