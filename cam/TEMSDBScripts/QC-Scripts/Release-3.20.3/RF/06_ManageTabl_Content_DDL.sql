update gridcustomcolumn set deleted=1 ,deletiondate=SYS_EXTRACT_UTC(systimestamp) where deleted = 0 and classname='DeliveryTrackingDtoGrid';
commit;

update dynamicreports set opcoid  = null where opcoid is not null;
commit;