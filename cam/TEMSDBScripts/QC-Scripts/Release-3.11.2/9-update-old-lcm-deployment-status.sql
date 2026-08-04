update lcmengineering
set LCMDEPLOYMENTSTATUSID = 5 -- removed
where archived=1


update lcmengineering
set LCMDEPLOYMENTSTATUSID = 4 -- In-Service
where archived=0