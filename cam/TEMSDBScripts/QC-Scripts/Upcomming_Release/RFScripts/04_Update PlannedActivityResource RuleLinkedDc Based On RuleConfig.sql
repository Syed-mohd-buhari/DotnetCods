----------------------------------------- 
-- UPDATE THE PLANNEDACTIVITYRESOURCES TABLE RULELINKEDDC ID BASED ON PLANNEDACTIVITYTYPES TABLE RULE
----------------------------------------- 

update plannedactivityresources set rulelinkeddc = 10
where  forlcm = 1 and trim(lower(plannedactivityresource)) = lower('Modernize System (incl. virtualization)');

update plannedactivityresources set rulelinkeddc = 9
where  forlcm = 1 and trim(lower(plannedactivityresource)) = lower('Replace Solution (Change Equipment Manufacturer)');

update plannedactivityresources set rulelinkeddc = 1
where  forlcm = 1 and trim(lower(plannedactivityresource)) = lower('Software Upgrade');

update plannedactivityresources set rulelinkeddc = 5
where  forlcm = 1 and trim(lower(plannedactivityresource)) = lower('Hardware Upgrade');

update plannedactivityresources set rulelinkeddc = 12
where  forlcm = 1 and trim(lower(plannedactivityresource)) = lower('New System (HW'||'&'||'SW) Solution');

update plannedactivityresources set rulelinkeddc = 8
where  forlcm = 1 and trim(lower(plannedactivityresource)) = lower('New NFxI Solution');

update plannedactivityresources set rulelinkeddc = 1
where  forlcm = 1 and trim(lower(plannedactivityresource)) = lower('SW Upgrdae');

update plannedactivityresources set rulelinkeddc = 6
where  forlcm = 1 and trim(lower(plannedactivityresource)) = lower('Refactor');

Commit;