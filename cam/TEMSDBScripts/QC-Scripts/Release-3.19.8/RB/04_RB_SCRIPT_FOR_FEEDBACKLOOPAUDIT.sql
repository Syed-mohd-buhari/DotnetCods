----------------------------------------------------------
-- ROLLBACK SCRIPT FOR TABLE FEEDBACKLOOPAUDITS
----------------------------------------------------------
DROP SEQUENCE "FEEDBACKLOOPAUDIT_SEQ";

DROP TABLE "FEEDBACKLOOPAUDITS";

COMMIT;