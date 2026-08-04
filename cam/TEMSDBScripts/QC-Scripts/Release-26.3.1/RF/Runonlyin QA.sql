 delete gridcustomcolumn where classname in ('RoleWisePreferenceName','UserPreferenceDto');
 commit;

---- INSERT SCRIPT FOR ASPNETMODULES
DELETE FROM ASPNETUSERROLEPERMISSIONS;
DELETE FROM ASPNETMODULES;
DROP SEQUENCE ASPNETMODULES_SEQ;
 
CREATE SEQUENCE  "ASPNETMODULES_SEQ"  MINVALUE 1 MAXVALUE 9999999999999999 
INCREMENT BY 1 START WITH  1 CACHE 20 NOORDER  NOCYCLE  NOKEEP  NOSCALE  GLOBAL ;
SET DEFINE OFF;

INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Major HW','/majorhardware','Libraries','1','Product and Design',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Major SW','/majorsoftware','Libraries','1','Product and Design',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Compnent Software ','/componentswbuild','Libraries','0','Product and Design',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('System Type','/systemtype','Libraries','1','Product and Design',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('System Verification Problems','/systemverificationproblems','Libraries','0','Product and Design',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('NFVI Compatibility','/nfvisoftwarecompatibility','Libraries','0','Product and Design',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Design Component','/designcomponent','Libraries','1','Product and Design',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('DCF','/designcomponentfamily','Libraries','1','Product and Design',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Subnettwork Boundary','/subnetwork','Libraries','0','Product and Design',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('LCM Engineering','/lcmengineering','LCM','1','Network Plan',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Design Aspects','/designAspect','LCM','1','Network Plan',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Service Level PA','/servicelevel','LCM','0','Network Plan',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Asset','/asplanned','LCM','0','Network Plan',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Identities','/Identities','LCM','0','Network Plan',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Component SW','/componentswbag','LCM','0','Network Plan',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Network Element','/feedbackloop/networkelement','AssetConfigMaster','0','Network Plan',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Discovered Identities','/feedbackloop/identity','AssetConfigMaster','0','Network Plan',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Hardware Configuration','/feedbackloop/hardwareconfiguration','AssetConfigMaster','0','Network Plan',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Software Configuration','/feedbackloop/softwareconfigurations','AssetConfigMaster','0','Network Plan',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Software Component','/feedbackloop/softwarecomponent','AssetConfigMaster','0','Network Plan',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Network Element As-Is','/asis','AssetConfigMaster','0','Network Plan',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('VBOM Info','/virtualbom','AssetConfigMaster','0','Network Plan',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('CBOM Info','/cbom','AssetConfigMaster','0','Network Plan',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Passthrough Data','/passthroughdata','LCM','0','Network Plan',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('LCM ','/lcmengineering','LCM ','0','Manage Network Plan',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Assets','/asplanned','AssetConfigMaster','0','Manage Network Plan',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Design Aspects','/designAspect','LCM ','0','Manage Network Plan',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('LCM Planned Activities','/plannedActivities/LCM','LCM ','0','Manage Network Plan',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Asset Planned Activities','/plannedActivities/Asset','LCM ','0','Manage Network Plan',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Design Aspect Planned Activities','/plannedActivities/DesignAspect','LCM ','0','Manage Network Plan',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Initialize New Product','-setIsVisibleModalInitializeNewProduct(true)','Libraries','0','Manage Transformation',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Manage Migration','-setIsVisibleModalManage(true)','LCM ','0','Manage Transformation',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Update Planned Activity Status','-setIsVisibleModalStatus(true)','LCM ','0','Manage Transformation',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Reconciliation','/reconciliation','AssetConfigMaster','0','Manage Transformation',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Delivery Tracking','/deliveryTracking','Delivery & Planning','0','Manage Transformation',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Product Lifecycle Constraint','-setIsVisibleModalProductLifecycle(true)','Libraries','0','Manage Transformation',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('LCM Export','/generatelcmdb','LCM ','1','Reports',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Disaggregated Reports','/generatelcmdbR10','LCM ','1','Reports',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Vai Export','/vaiexport','LCM ','0','Reports',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Planned Activity Tracker','/plannedActivityTracker','LCM ','0','Reports',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('TSR Report','/tsrreport','LCM ','0','Reports',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('FNT Report','/fntreport','LCM ','0','Reports',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('LCM @ Glance','/lcmatglance','LCM ','0','Reports',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Asset over view by market','/assetoverviewbymarket','Asset over view by market','0','Reports',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Network Visualizer','/networkvisualizer','Network Visualizer','0','Reports',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Planned Activity Report','/plannedactivityreport','Planned Activity Report','0','Reports',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Compatibility@ Glance','/nfvicreport','Compatibility@ Glance','0','Reports',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Asset Pivot By Location','/assetpivotbylocation','Asset Pivot By Location','0','Reports',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Planned Activity','/plannedActivities/All','All Planned Activities','0','Reports',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('User Defined Reports','/genericreports','User Defined Reports','0','Reports',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('BPT Report','/bptreport','BPT Report','0','Reports',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Exodus Report','/exodusreport','Exodus Report','0','Reports',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('VBOM Report','/virtualbomreport','AssetConfigMaster','0','Reports',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('CBOM Report','/cbomreport','AssetConfigMaster','0','Reports',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES(' KPIs Worklog and Approvals','/targetmonthlyapprovals','','','Reports',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('VoLTE KPI Dashboard','/dashboard','','','Reports',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Generate VoLTE Dashboard','/generatevoltedashboard','Generate VoLTE Dashboard','0','Reports',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Settings Update Planned Activity','/settingsupdateplannedactivity','Settings Update Planned Activity','0','Administration',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Planned Activity Type','/plannedactivitytype','Planned Activity Type','0','Administration',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('SW App - VF Name','/vodafoneName','SW App - VF Name','0','Administration',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('General Settings','/generalsettings','General Settings','0','Administration',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Resource Key Master Details','/resourcekeymaster','Resource Key Master Details','0','Administration',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('DCF Life Cycle','/dcflifecycle','DCF Life Cycle','0','Administration',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Log Management','/userLogLevel','Log Management','0','Administration',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('User Details','/usermanagement','User Details','0','Administration',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Organization Info','/organizationinfo','Organization Info','0','Administration',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Worklog & Approval','/feedbackloop/audit','Worklog & Approval','0','Administration',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Asset Mapping','/assetmapinfo','Asset Mapping','0','Administration',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Asset Category','Asset Category','LibraryMaster','0','',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Asset Type','Asset Type','LibraryMaster','0','',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Build Construction','Build Construction','LibraryMaster','0','',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Component','Component ','LibraryMaster','0','',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Bag','Current Bag','LibraryMaster','0','',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Environment','Environment','LibraryMaster','0','',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Functional Entity','Functional Entity','LibraryMaster','0','',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Location','Location','LibraryMaster','0','',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('OpCo ','OpCo ','LibraryMaster','0','',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Operating System','Operating System','LibraryMaster','0','',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Operational Contact','Operational Contact','LibraryMaster','0','',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Equipment Manufacturer','Original Equipment Manufacturer','LibraryMaster','0','',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Problem Category','Problem Category','LibraryMaster','0','',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Product Importance','Product Importance','LibraryMaster','0','',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Product Name','Product Name - VF Name','LibraryMaster','0','',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Software Family','Software Family','LibraryMaster','0','',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('SubNetwork Boundary','SubNetwork Boundary','LibraryMaster','0','',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Supported Resource','Supported Resource','LibraryMaster','0','',1,1);
INSERT INTO ASPNETMODULES(MODULE,MODULEPATH,CATEGORY,ISDEFAULT,MENU,CREATIONUSER,MODIFICATIONUSER) VALUES('Vodafone Name','Vodafone Name','LibraryMaster','0','',1,1);


Commit; 



---- INSERT SCRIPT FOR ASPNETUSERROLEPERMISSIONS
DELETE FROM ASPNETUSERROLEPERMISSIONS;
DROP SEQUENCE ASPNETUSERROLEPERMISSIONS_SEQ;
 
CREATE SEQUENCE  "ASPNETUSERROLEPERMISSIONS_SEQ"  MINVALUE 1 MAXVALUE 9999999999999999 
INCREMENT BY 1 START WITH  1 CACHE 20 NOORDER  NOCYCLE  NOKEEP  NOSCALE  GLOBAL ;
SET DEFINE OFF;
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='System Type'  WHERE r.name = 'SW Product Owner';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='Vodafone Name'  WHERE r.name = 'Product Collection Owner';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='Vodafone Name'  WHERE r.name = 'SBD: System Security';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='Risk Cluster'  WHERE r.name = 'SBD: System Security';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='Major SW'  WHERE r.name = 'SW Product Owner';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='Equipment Manufacturer'  WHERE r.name = 'SW Product Owner';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='Product Name'  WHERE r.name = 'SW Product Owner';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='Functional Entity'  WHERE r.name = 'SW Product Owner';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='SW Family'  WHERE r.name = 'SW Product Owner';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='Operating System'  WHERE r.name = 'SW Product Owner';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='Major HW'  WHERE r.name = 'HW Product Owner';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='Build Construction'  WHERE r.name = 'TEMS System Owner';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='NFVI Compatibility'  WHERE r.name = 'Evolution Architect';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='System Verification Problems'  WHERE r.name = 'Test Manager';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='Problem Category'  WHERE r.name = 'Test Manager';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='Design Aspects'  WHERE r.name = 'Network Architect';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='Design Aspects'  WHERE r.name = 'Business Continuity';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='Design Aspects'  WHERE r.name = 'SBD: Network Security';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='Design Aspects'  WHERE r.name = 'SBD: Access Control';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='Design Aspects'  WHERE r.name = 'Life Cycle Owner';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='Design Aspects'  WHERE r.name = 'SBD: Sec Management';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='Design Aspects'  WHERE r.name = 'Logical Design';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='Functional Release Cycle Period'  WHERE r.name = 'Life Cycle Manager';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='DCF'  WHERE r.name = 'Network Architect';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='Subnetwork Boundary'  WHERE r.name = 'Network Architect';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='Subnetwork Boundary'  WHERE r.name = 'SBD: Privacy';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='Subnetwork Boundary'  WHERE r.name = 'SBD: Network Security';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='Subnetwork Boundary'  WHERE r.name = 'Life Cycle Owner';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='Subnetwork Boundary'  WHERE r.name = 'SBD: System Security';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='Subnetwork Boundary'  WHERE r.name = 'SBD: Compliance';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='Subnetwork Boundary'  WHERE r.name = 'Business Continuity';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='DC'  WHERE r.name = 'Network Architect';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='Planned Activity'  WHERE r.name = 'Network Evolution Planning';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='LCM'  WHERE r.name = 'Network Architect';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='LCM'  WHERE r.name = 'LifeCycle Owner';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='Identities'  WHERE r.name = 'Opco E2E Network Design';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='Category'  WHERE r.name = 'TEMS System Owner';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='Class'  WHERE r.name = 'TEMS System Owner';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='Type'  WHERE r.name = 'TEMS System Owner';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='Bag'  WHERE r.name = 'LifeCycle Owner';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='Component SW'  WHERE r.name = 'LifeCycle Owner';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='Component'  WHERE r.name = 'LifeCycle Owner';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='Cluster'  WHERE r.name = 'Infra Capacity Engineer';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='Instance'  WHERE r.name = 'Infra Capacity Engineer';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='Capacity'  WHERE r.name = 'Infra Capacity Engineer';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module ='Delivery Tracking'  WHERE r.name = 'DMO Delivery Tracking'; 
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module =' KPIs Worklog and Approvals'  WHERE r.name = 'KPI Administrator';
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.module =' KPIs Worklog and Approvals'  WHERE r.name = 'KPI Editor'; 
---Refactor User 
INSERT INTO ASPNETUSERROLEPERMISSIONS
(roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER)
SELECT r.id, m.aspnetmoduleid,7,1,1 FROM aspnetroles r JOIN aspnetmodules m  ON m.modulepath ='/usermanagement'  WHERE r.name = 'Refactor User'; 
---'TEMS System Owner', 'TEMS Functional Admin','Admin'
INSERT INTO ASPNETUSERROLEPERMISSIONS (roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER) SELECT r.id,  m.aspnetmoduleid, 7,1,1 FROM aspnetroles r CROSS JOIN aspnetmodules m WHERE r.name IN ('TEMS System Owner', 'TEMS Functional Admin','Admin') AND NOT EXISTS (  SELECT 1  FROM ASPNETUSERROLEPERMISSIONS p   WHERE p.roleid = r.id AND p.moduleid = m.aspnetmoduleid );

--- 'User','Readonly'
INSERT INTO ASPNETUSERROLEPERMISSIONS (roleid, moduleid, permissionlevel, CREATIONUSER, MODIFICATIONUSER) SELECT r.id,  m.aspnetmoduleid, 7,1,1 FROM aspnetroles r CROSS JOIN aspnetmodules m WHERE r.name IN ('User','Readonly') and m.menu != 'Administration' AND NOT EXISTS (  SELECT 1  FROM ASPNETUSERROLEPERMISSIONS p   WHERE p.roleid = r.id AND p.moduleid = m.aspnetmoduleid );

Commit;

UPDATE organisation SET isdesigncontact = 1 WHERE contactid IN ( SELECT designcontactid FROM majorhwbuildsdesigncontacts
) OR contactid IN ( SELECT designcontactid FROM majorswbuildsdesigncontacts);

Commit;