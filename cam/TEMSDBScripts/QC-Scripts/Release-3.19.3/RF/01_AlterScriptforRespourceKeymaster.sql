update ResourceKeyMaster set lifecycleId = 1 where lifecycleId is NULL;

ALTER TABLE ResourceKeyMaster
MODIFY lifecycleId  DEFAULT 1 NOT NULL;
