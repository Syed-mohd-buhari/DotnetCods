----------------------------------------------------------
-- ROLLBACK SCRIPT FOR TABLE RISKCLUSTERS STEP-01
----------------------------------------------------------
DROP SEQUENCE "RISKCLUSTER_SEQ";

DROP TABLE "RISKCLUSTERS";

COMMIT;