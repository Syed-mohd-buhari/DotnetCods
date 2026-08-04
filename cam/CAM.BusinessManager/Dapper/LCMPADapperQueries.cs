using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace CAM.BusinessManager.Dapper
{
    public class LCMPADapperQueries : BaseManager
    {
        IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerManager _loggerManager;

        public LCMPADapperQueries(IEnumerable<IRepositoryWrapper> wrappers, ILoggerManager loggerManager,
           IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _loggerManager = loggerManager;
        }

        public  string  GetLcmPaQueryForAbstraction()
        {
            try
            {
                string lcmPaQuery = @"SELECT
    /*---------------------------------- LCM ENGINEERING ----------------------------------*/
    le.LCMENGINEERINGID,
    
    le.ARCHIVED                        AS LCMARCHIVED,
    le.DELETED                         AS LCMDELETED,

    /*----------------------------------OPCO ----------------------------------*/
    op.OPCOID,
    op.OPCO,

    /*----------------------------------   LCM DESIGN COMPONENT    ----------------------------------*/
    ldc.DESIGNCOMPONENTID              AS LCMDCID,
    lst.SYSTEMTYPEID                   AS LCMSYSTEMTYPEID,

    /*----------------------------------        LCM DESIGN COMPONENT    ----------------------------------*/
    loem.ORIGINALEQUIPMENTMANUFACTURER     AS LCMOEM,
    lpn.DESCRIPTION                  AS LCMPRODUCTNAME,

    /*----------------------------------        PLANNED ACTIVITY    ----------------------------------*/
    pa.PLANNEDACTIVITYID,
    pa.DELETED                         AS PADELETED,
    pa.LCMENGINEERINGID                AS PALCMID,
    pa.PLANNEDCOMPLETION,
    pa.STARTDATE,
    pa.PLANNEDACTIVITYRESOURCEID,
    pa.DELIVERYSTATUSID                AS PADELIVERYSTATUSID,
    pa.ARCHIVED                        AS PAARCHIVED,
    pa.OPCOID                          AS PAOPCOID,
    pa.DESIGNCOMPONENTFAMILYID         AS PADCFID,
    pa.DESIGNCOMPONENTID               AS PADCID,
    ds.DELIVERYSTATUS                  AS PADELIVERYSTATUS,
    par.PLANNEDACTIVITYRESOURCE,
    pst.SYSTEMTYPEID                   AS PASYSTEMTYPEID,

    /*----------------------------------        PA SOFTWARE DETAILS    ----------------------------------*/
    pmsw.PRODUCTNAMEID                 AS PAPRODUCTID,
    pmsw.ORGEQPMANUFACTURERID          AS PAOEMID,
    pmsw.ENDOFMAINTENANCE              AS PAEOM,
    pmsw.ENDOFSUPPORT                  AS PAEOS,
    pmsw.SOFTWAREVERSION               AS PAVERSION,
    poem.ORIGINALEQUIPMENTMANUFACTURER     AS PAOEM,
    ppn.DESCRIPTION                  AS PAPRODUCTNAME,
    /*----------------------------------        LCM DC NAME    ----------------------------------*/
    'LCMDCNAME'

    AS LCMDCNAME,

    /*----------------------------------        PA DC NAME    ----------------------------------*/
    'PADCNAME'

    AS PADCNAME

FROM LCMENGINEERING le

/*----------------------------------    OPCO----------------------------------*/
LEFT JOIN OPCOS op
       ON op.OPCOID = le.OPCOID

/*----------------------------------    LCM DESIGN COMPONENT----------------------------------*/
LEFT JOIN DESIGNCOMPONENTS ldc
       ON ldc.DESIGNCOMPONENTID = le.DESIGNCOMPONENTID

LEFT JOIN SYSTEMTYPES lst
       ON lst.SYSTEMTYPEID = ldc.SYSTEMTYPEID

LEFT JOIN MAJORSOFTWAREBUILDS lmsw
       ON lmsw.MAJORSOFTWAREBUILDSID = lst.MAJORSOFTWAREBUILDSID

LEFT JOIN PRODUCTNAME lpn
       ON lpn.PRODUCTNAMEID = lmsw.PRODUCTNAMEID

LEFT JOIN ORIGINALEQUIPMENTMANUFACTURERS loem
       ON loem.ORGEQPMANUFACTURERID = lmsw.ORGEQPMANUFACTURERID

LEFT JOIN SYSTEMTYPESMAJORHARDWAREBUILDS lstmh
       ON lstmh.SYSTEMTYPEID = lst.SYSTEMTYPEID
      AND lstmh.ISMAIN = 1
      AND lstmh.DELETED = 0

LEFT JOIN MAJORHARDWAREBUILDS lmhw
       ON lmhw.MAJORHARDWAREID = lstmh.MAJORHARDWAREID

LEFT JOIN BUILDCONSTRUCTIONS lbc
       ON lbc.BUILDCONSTRUCTIONID = lmhw.BUILDCONSTRUCTIONID

LEFT JOIN PLATFORMS lpf
       ON lpf.PLATFORMID = lmhw.PLATFORMID

LEFT JOIN DESIGNCOMPONENTFAMILIES ldcf
       ON ldcf.DESIGNCOMPONENTFAMILYID = ldc.DESIGNCOMPONENTFAMILYID

LEFT JOIN SUBNETWORKBOUNDARIES lsnb
       ON lsnb.ID = ldcf.SUBNETWORKBOUNDARYID

/*----------------------------------    PLANNED ACTIVITIES----------------------------------*/
LEFT JOIN PLANNEDACTIVITIES pa
       ON pa.LCMENGINEERINGID = le.LCMENGINEERINGID

LEFT JOIN DELIVERYSTATUSES ds
       ON ds.DELIVERYSTATUSID = pa.DELIVERYSTATUSID

LEFT JOIN PLANNEDACTIVITYRESOURCES par
       ON par.PLANNEDACTIVITYRESOURCEID = pa.PLANNEDACTIVITYRESOURCEID

LEFT JOIN PLANNEDACTIVITYTYPES pat
       ON pat.PLANNEDACTIVITYTYPESID = par.RULELINKEDDC

/*----------------------------------    PA DESIGN COMPONENT----------------------------------*/
LEFT JOIN DESIGNCOMPONENTS pdc
       ON pdc.DESIGNCOMPONENTID = pa.DESIGNCOMPONENTID

LEFT JOIN SYSTEMTYPES pst
       ON pst.SYSTEMTYPEID = pdc.SYSTEMTYPEID

LEFT JOIN MAJORSOFTWAREBUILDS pmsw
       ON pmsw.MAJORSOFTWAREBUILDSID = pst.MAJORSOFTWAREBUILDSID

LEFT JOIN PRODUCTNAME ppn
       ON ppn.PRODUCTNAMEID = pmsw.PRODUCTNAMEID

LEFT JOIN ORIGINALEQUIPMENTMANUFACTURERS poem
       ON poem.ORGEQPMANUFACTURERID = pmsw.ORGEQPMANUFACTURERID

LEFT JOIN SYSTEMTYPESMAJORHARDWAREBUILDS pstmh
       ON pstmh.SYSTEMTYPEID = pst.SYSTEMTYPEID
      AND pstmh.ISMAIN = 1
      AND pstmh.DELETED = 0

LEFT JOIN MAJORHARDWAREBUILDS pmhw
       ON pmhw.MAJORHARDWAREID = pstmh.MAJORHARDWAREID

LEFT JOIN BUILDCONSTRUCTIONS pbc
       ON pbc.BUILDCONSTRUCTIONID = pmhw.BUILDCONSTRUCTIONID

LEFT JOIN PLATFORMS ppf
       ON ppf.PLATFORMID = pmhw.PLATFORMID

LEFT JOIN DESIGNCOMPONENTFAMILIES pdcf
       ON pdcf.DESIGNCOMPONENTFAMILYID = pdc.DESIGNCOMPONENTFAMILYID

LEFT JOIN SUBNETWORKBOUNDARIES psnb
       ON psnb.ID = pdcf.SUBNETWORKBOUNDARYID
/*WHERE
    (
        :userId = 0

        OR EXISTS (
            SELECT 1
            FROM LCMENGINEERINGSUBDOMAINSPOC sd
            WHERE sd.LCMENGINEERINGID = le.LCMENGINEERINGID
              AND sd.SUBDOMAINSPOCID = :userId
        )

        OR EXISTS (
            SELECT 1
            FROM LCMENGINEERINGEDUSPOC edu
            WHERE edu.LCMENGINEERINGID = le.LCMENGINEERINGID
              AND edu.EDUSPOCID = :userId
        )
    )
ORDER BY pa.PLANNEDCOMPLETION */
";

                return lcmPaQuery;

            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex.StackTrace);
                throw;
            }

        }

        public string GetLcmPaQueryForAbstractionName()
        {
            try
            {
                string lcmPaQuery = @"SELECT
    /*----------------------------------
        LCM ENGINEERING
    ----------------------------------*/
    le.LCMENGINEERINGID,
    
    COUNT(*) OVER (
        PARTITION BY le.LCMENGINEERINGID
    ) AS LCM_COUNT,

    le.ARCHIVED                        AS LCMARCHIVED,
    le.DELETED                         AS LCMDELETED,

    /*----------------------------------
        OPCO
    ----------------------------------*/
    op.OPCOID,
    op.OPCO,

    /*----------------------------------
        LCM DESIGN COMPONENT
    ----------------------------------*/
    ldc.DESIGNCOMPONENTID              AS LCMDCID,
    lst.SYSTEMTYPEID                   AS LCMSYSTEMTYPEID,

    /*----------------------------------
        LCM DESIGN COMPONENT
    ----------------------------------*/
    loem.ORIGINALEQUIPMENTMANUFACTURER     AS LCMOEM,
    lpn.DESCRIPTION                  AS LCMPRODUCTNAME,

    /*----------------------------------
        PLANNED ACTIVITY
    ----------------------------------*/
    pa.PLANNEDACTIVITYID,
    pa.DELETED                         AS PADELETED,
    pa.LCMENGINEERINGID                AS PALCMID,
    pa.PLANNEDCOMPLETION,
    pa.STARTDATE,
    pa.PLANNEDACTIVITYRESOURCEID,
    pa.DELIVERYSTATUSID                AS PADELIVERYSTATUSID,
    pa.ARCHIVED                        AS PAARCHIVED,
    pa.OPCOID                          AS PAOPCOID,
    pa.DESIGNCOMPONENTFAMILYID         AS PADCFID,
    pa.DESIGNCOMPONENTID               AS PADCID,
    ds.DELIVERYSTATUS                  AS PADELIVERYSTATUS,
    par.PLANNEDACTIVITYRESOURCE,
    pst.SYSTEMTYPEID                   AS PASYSTEMTYPEID,

    /*----------------------------------
        PA SOFTWARE DETAILS
    ----------------------------------*/
    pmsw.PRODUCTNAMEID                 AS PAPRODUCTID,
    pmsw.ORGEQPMANUFACTURERID          AS PAOEMID,
    pmsw.ENDOFMAINTENANCE              AS PAEOM,
    pmsw.ENDOFSUPPORT                  AS PAEOS,
    pmsw.SOFTWAREVERSION               AS PAVERSION,
    poem.ORIGINALEQUIPMENTMANUFACTURER     AS PAOEM,
    ppn.DESCRIPTION                  AS PAPRODUCTNAME,
    /*----------------------------------
        LCM DC NAME
    ----------------------------------*/
    NVL(CAST(loem.ORIGINALEQUIPMENTMANUFACTURER AS VARCHAR2(500)), '')
    || ' ' ||
    NVL(CAST(lpn.DESCRIPTION AS VARCHAR2(500)), '')
    || ' ' ||
    NVL(CAST(lmsw.SOFTWAREVERSION AS VARCHAR2(500)), '')

    ||

    CASE
        WHEN lmhw.MAJORHARDWAREID IS NOT NULL THEN
            CASE
                WHEN lbc.RULE IN (:ProprietaryHW, :CotsHW) THEN
                    ' on '
                    || NVL(CAST(lmhw.HARDWARESOLUTION AS VARCHAR2(500)), '')
                    || ' '
                    || NVL(CAST(lpf.PLATFORM AS VARCHAR2(500)), '')
                    || ' '
                    || NVL(CAST(lmhw.HARDWARETYPE AS VARCHAR2(500)), '')
                ELSE
                    ' on '
                    || NVL(CAST(lpf.PLATFORM AS VARCHAR2(500)), '')
            END
        ELSE ''
    END

    ||

    CASE
        WHEN lsnb.ALIAS IS NOT NULL THEN
            ' for '
            || CAST(lsnb.ALIAS AS VARCHAR2(500))

        WHEN lsnb.DESCRIPTION IS NOT NULL THEN
            ' for '
            || CAST(lsnb.DESCRIPTION AS VARCHAR2(500))

        ELSE ''
    END

    AS LCMDCNAME,

    /*----------------------------------
        PA DC NAME
    ----------------------------------*/
    NVL(CAST(poem.ORIGINALEQUIPMENTMANUFACTURER AS VARCHAR2(500)), '')
    || ' ' ||
    NVL(CAST(ppn.DESCRIPTION AS VARCHAR2(500)), '')
    || ' ' ||
    NVL(CAST(pmsw.SOFTWAREVERSION AS VARCHAR2(500)), '')

    ||

    CASE
        WHEN pmhw.MAJORHARDWAREID IS NOT NULL THEN
            CASE
                WHEN pbc.RULE IN (:ProprietaryHW, :CotsHW) THEN
                    ' on '
                    || NVL(CAST(pmhw.HARDWARESOLUTION AS VARCHAR2(500)), '')
                    || ' '
                    || NVL(CAST(ppf.PLATFORM AS VARCHAR2(500)), '')
                    || ' '
                    || NVL(CAST(pmhw.HARDWARETYPE AS VARCHAR2(500)), '')
                ELSE
                    ' on '
                    || NVL(CAST(ppf.PLATFORM AS VARCHAR2(500)), '')
            END
        ELSE ''
    END

    ||

    CASE
        WHEN psnb.ALIAS IS NOT NULL THEN
            ' for '
            || CAST(psnb.ALIAS AS VARCHAR2(500))

        WHEN psnb.DESCRIPTION IS NOT NULL THEN
            ' for '
            || CAST(psnb.DESCRIPTION AS VARCHAR2(500))

        ELSE ''
    END

    AS PADCNAME

FROM LCMENGINEERING le

/*----------------------------------
    OPCO
----------------------------------*/
LEFT JOIN OPCOS op
       ON op.OPCOID = le.OPCOID

/*----------------------------------
    LCM DESIGN COMPONENT
----------------------------------*/
LEFT JOIN DESIGNCOMPONENTS ldc
       ON ldc.DESIGNCOMPONENTID = le.DESIGNCOMPONENTID

LEFT JOIN SYSTEMTYPES lst
       ON lst.SYSTEMTYPEID = ldc.SYSTEMTYPEID

LEFT JOIN MAJORSOFTWAREBUILDS lmsw
       ON lmsw.MAJORSOFTWAREBUILDSID = lst.MAJORSOFTWAREBUILDSID

LEFT JOIN PRODUCTNAME lpn
       ON lpn.PRODUCTNAMEID = lmsw.PRODUCTNAMEID

LEFT JOIN ORIGINALEQUIPMENTMANUFACTURERS loem
       ON loem.ORGEQPMANUFACTURERID = lmsw.ORGEQPMANUFACTURERID

LEFT JOIN SYSTEMTYPESMAJORHARDWAREBUILDS lstmh
       ON lstmh.SYSTEMTYPEID = lst.SYSTEMTYPEID
      AND lstmh.ISMAIN = 1
      AND lstmh.DELETED = 0

LEFT JOIN MAJORHARDWAREBUILDS lmhw
       ON lmhw.MAJORHARDWAREID = lstmh.MAJORHARDWAREID

LEFT JOIN BUILDCONSTRUCTIONS lbc
       ON lbc.BUILDCONSTRUCTIONID = lmhw.BUILDCONSTRUCTIONID

LEFT JOIN PLATFORMS lpf
       ON lpf.PLATFORMID = lmhw.PLATFORMID

LEFT JOIN DESIGNCOMPONENTFAMILIES ldcf
       ON ldcf.DESIGNCOMPONENTFAMILYID = ldc.DESIGNCOMPONENTFAMILYID

LEFT JOIN SUBNETWORKBOUNDARIES lsnb
       ON lsnb.ID = ldcf.SUBNETWORKBOUNDARYID

/*----------------------------------
    PLANNED ACTIVITIES
----------------------------------*/
LEFT JOIN PLANNEDACTIVITIES pa
       ON pa.LCMENGINEERINGID = le.LCMENGINEERINGID

LEFT JOIN DELIVERYSTATUSES ds
       ON ds.DELIVERYSTATUSID = pa.DELIVERYSTATUSID

LEFT JOIN PLANNEDACTIVITYRESOURCES par
       ON par.PLANNEDACTIVITYRESOURCEID = pa.PLANNEDACTIVITYRESOURCEID

LEFT JOIN PLANNEDACTIVITYTYPES pat
       ON pat.PLANNEDACTIVITYTYPESID = par.RULELINKEDDC

/*----------------------------------
    PA DESIGN COMPONENT
----------------------------------*/
LEFT JOIN DESIGNCOMPONENTS pdc
       ON pdc.DESIGNCOMPONENTID = pa.DESIGNCOMPONENTID

LEFT JOIN SYSTEMTYPES pst
       ON pst.SYSTEMTYPEID = pdc.SYSTEMTYPEID

LEFT JOIN MAJORSOFTWAREBUILDS pmsw
       ON pmsw.MAJORSOFTWAREBUILDSID = pst.MAJORSOFTWAREBUILDSID

LEFT JOIN PRODUCTNAME ppn
       ON ppn.PRODUCTNAMEID = pmsw.PRODUCTNAMEID

LEFT JOIN ORIGINALEQUIPMENTMANUFACTURERS poem
       ON poem.ORGEQPMANUFACTURERID = pmsw.ORGEQPMANUFACTURERID

LEFT JOIN SYSTEMTYPESMAJORHARDWAREBUILDS pstmh
       ON pstmh.SYSTEMTYPEID = pst.SYSTEMTYPEID
      AND pstmh.ISMAIN = 1
      AND pstmh.DELETED = 0

LEFT JOIN MAJORHARDWAREBUILDS pmhw
       ON pmhw.MAJORHARDWAREID = pstmh.MAJORHARDWAREID

LEFT JOIN BUILDCONSTRUCTIONS pbc
       ON pbc.BUILDCONSTRUCTIONID = pmhw.BUILDCONSTRUCTIONID

LEFT JOIN PLATFORMS ppf
       ON ppf.PLATFORMID = pmhw.PLATFORMID

LEFT JOIN DESIGNCOMPONENTFAMILIES pdcf
       ON pdcf.DESIGNCOMPONENTFAMILYID = pdc.DESIGNCOMPONENTFAMILYID

LEFT JOIN SUBNETWORKBOUNDARIES psnb
       ON psnb.ID = pdcf.SUBNETWORKBOUNDARYID
/*WHERE
    (
        :userId = 0

        OR EXISTS (
            SELECT 1
            FROM LCMENGINEERINGSUBDOMAINSPOC sd
            WHERE sd.LCMENGINEERINGID = le.LCMENGINEERINGID
              AND sd.SUBDOMAINSPOCID = :userId
        )

        OR EXISTS (
            SELECT 1
            FROM LCMENGINEERINGEDUSPOC edu
            WHERE edu.LCMENGINEERINGID = le.LCMENGINEERINGID
              AND edu.EDUSPOCID = :userId
        )
    )
ORDER BY pa.PLANNEDCOMPLETION */
";

                return lcmPaQuery;

            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex.StackTrace);
                throw;
            }

        }

        public string GetLcmQueryFoEomAndEos()
        {
            return @"
    SELECT DISTINCT
           le.LCMENGINEERINGID,
            le.deleted,le.archived as LcmArchived,
           op.OPCO,
           lpn.DESCRIPTION AS PRODUCT,
           loem.ORIGINALEQUIPMENTMANUFACTURER AS OriginalEquipmentManufacturer,
           lmsw.SOFTWAREVERSION AS SoftwareVersion,
           lmsw.ENDOFMAINTENANCE AS EndOfMaintenance,
           lmsw.ENDOFSUPPORT AS EndOfSupport,
           lmsw.MAJORSOFTWAREBUILDSID AS MajorSoftwareBuildId,
           le.PRODUCTIMPORTANCEID,
            LISTAGG(msdc.designcontactid, ',') WITHIN GROUP (ORDER BY msdc.designcontactid) AS DESIGNCONTACTIDS,
             CASE
                    WHEN le.PRODUCTIMPORTANCEID =
                         (
                             SELECT pi.PRODUCTIMPORTANCEID
                             FROM productimportances pi
                             WHERE LOWER(REPLACE(pi.PRODUCTIMPORTANCE, ' ', '')) = :ProdImportanceStrategic
                             FETCH FIRST 1 ROW ONLY
                         )
                    THEN 1
                    ELSE 0   END AS ProductImportanceId
    FROM LCMENGINEERING le

    LEFT JOIN OPCOS op
           ON op.OPCOID = le.OPCOID

    LEFT JOIN DESIGNCOMPONENTS ldc
           ON ldc.DESIGNCOMPONENTID = le.DESIGNCOMPONENTID

    LEFT JOIN SYSTEMTYPES lst
           ON lst.SYSTEMTYPEID = ldc.SYSTEMTYPEID

    LEFT JOIN MAJORSOFTWAREBUILDS lmsw
           ON lmsw.MAJORSOFTWAREBUILDSID = lst.MAJORSOFTWAREBUILDSID

    LEFT JOIN PRODUCTNAME lpn
           ON lpn.PRODUCTNAMEID = lmsw.PRODUCTNAMEID

    LEFT JOIN ORIGINALEQUIPMENTMANUFACTURERS loem
           ON loem.ORGEQPMANUFACTURERID = lmsw.ORGEQPMANUFACTURERID

    LEFT JOIN MAJORSWBUILDSDESIGNCONTACTS msdc
           ON msdc.MAJORSOFTWAREBUILDSID = lmsw.MAJORSOFTWAREBUILDSID 
     
   /* WHERE le.ARCHIVED = 0
      AND (
            :designContactId = 0
            OR msdc.DESIGNCONTACTID = :designContactId
          )
      AND (
            (
                lmsw.ENDOFMAINTENANCE > :messagingDate
                AND lmsw.ENDOFMAINTENANCE < :today
            )
            OR
            (
                lmsw.ENDOFSUPPORT > :messagingDate
                AND lmsw.ENDOFSUPPORT < :today
            )
          )

    ORDER BY lmsw.ENDOFMAINTENANCE*/";
        }
    }
}
