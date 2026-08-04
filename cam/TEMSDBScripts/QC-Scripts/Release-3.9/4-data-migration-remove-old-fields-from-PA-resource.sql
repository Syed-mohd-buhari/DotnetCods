update plannedactivityresources 
set "FORADDASSET"=fornetworkelement,
FORCREATEADDASSET =forcreatenetworkelement,
FOREDITADDASSET = foreditnetworkelement,
RULEADDASSET = rulenetworkelement,
ADDASSETHARDWARE = networkelementhardware,
ADDASSETSOFTWARE = networkelementsoftware,
ONBAREMETALADDASSET= onbaremetalnetworkelement,
ONVIRTUALIZEDADDASSET =onvirtualizednetworkelement,
ACTIVITYDETAILSADDASSET = activitydetailsnetworkelement,
BENEFITTEXTADDASSET= benefittextnetworkelement,
DRIVERTEXTADDASSET= drivertextnetworkelement,
ACTDETAILSFORVRTADDASSET = actdetailsforvrtnetworkelement,
ADDASSETLABELSOFTWARE = networkelementlabelsoftware,
ADDASSETLABELHARDWARE= networkelementlabelhardware,
RULEACTDETAILSADDASSET = ruleactdetailsntkelement,
PLANDESIGNCOMPREQADDASSET = plandesigncompreqnwelement,
"FOREDITASSET"=fornetworkelement,
FORCREATEEDITASSET =forcreatenetworkelement,
FOREDITEDITASSET = foreditnetworkelement,
RULEEDITASSET = rulenetworkelement,
EDITASSETHARDWARE = networkelementhardware,
EDITASSETSOFTWARE = networkelementsoftware,
ONBAREMETALEDITASSET= onbaremetalnetworkelement,
ONVIRTUALIZEDEDITASSET =onvirtualizednetworkelement,
ACTIVITYDETAILSEDITASSET = activitydetailsnetworkelement,
BENEFITTEXTEDITASSET= benefittextnetworkelement,
DRIVERTEXTEDITASSET= drivertextnetworkelement,
ACTDETAILSFORVRTEDITASSET = actdetailsforvrtnetworkelement,
EDITASSETLABELSOFTWARE = networkelementlabelsoftware,
EDITASSETLABELHARDWARE= networkelementlabelhardware,
RULEACTDETAILSEDITASSET = ruleactdetailsntkelement,
PLANDESIGNCOMPREQEDITASSET = plandesigncompreqnwelement;

commit;


--migrate data of beneift, driver, planningrisk
-- Remove old fields 