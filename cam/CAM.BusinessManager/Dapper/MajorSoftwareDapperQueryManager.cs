using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace CAM.BusinessManager.Dapper
{
    public class MajorSoftwareDapperQueryManager : BaseManager
    {
        IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerManager _loggerManager;

        public MajorSoftwareDapperQueryManager(IEnumerable<IRepositoryWrapper> wrappers, ILoggerManager loggerManager,
           IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _loggerManager = loggerManager;
        }

        public string GetSoftwareRecords()
        {
            try
            {
                string mswQuery = @"SELECT

                            /*---------------------------------- Major Software Build ----------------------------------*/
                            msb.MAJORSOFTWAREBUILDSID AS MajorSoftwareBuildId,
                            msb.SOFTWAREVERSION       AS SoftwareVersion,
                            CASE
                                         WHEN msb.ENDOFMAINTENANCE is not null THEN  TO_CHAR(TO_DATE(msb.ENDOFSUPPORT, 'DD/MM/YYYY'), 'DD/MM/YYYY')
                                        else  CASE
                                        WHEN msb.EOMStatus  = 0  THEN  'Not Announced'
                                      else 'Not Specified' end
                                      END   AS EndOfMaintenance,
    
                           CASE
                              WHEN msb.ENDOFSUPPORT is null THEN 'Not Announced'
                                     ELSE TO_CHAR(TO_DATE(msb.ENDOFSUPPORT, 'DD/MM/YYYY'), 'DD/MM/YYYY')
                                END   AS EndOfsupport,
                            msb.EOMSTATUS             AS EomStatus,
                            msb.CREATIONDATE,
                            /*---------------------------------- Network Status  ----------------------------------*/
                             CASE
                                 WHEN COUNT(lcm.LCMENGINEERINGID) > 0 THEN 'Live'
                                 ELSE 'Inactive'
                                END as NetworkStatus,
                          /*----------------------------------  LifeCycle Status ----------------------------------*/
                            CASE
                                WHEN msb.ENDOFSUPPORT IS NULL THEN
                                    CASE
                                        WHEN msb.EOMSTATUS = 0 THEN 'On Support'
                                        ELSE 'Expired'
                                    END
                                WHEN TRUNC(msb.ENDOFSUPPORT) <= TRUNC(SYSDATE) THEN
                                    'On Support'
                                ELSE
                                    'Expired'
                            END AS LifeCycleStatus,

                            /*---------------------------------- Hardware details ----------------------------------*/
                              LISTAGG(
                                    DISTINCT mhdwr.MAJORHARDWAREID || '-' ||  NVL(hwoem.ORIGINALEQUIPMENTMANUFACTURER, '') ||
         NVL(mhdwr.HARDWARESOLUTION, '') ||
         NVL(hwPlf.PLATFORM, '') ||
         NVL(mhdwr.HARDWARETYPE, ''),
                                    ', ' ) WITHIN GROUP (  ORDER BY  NVL(hwoem.ORIGINALEQUIPMENTMANUFACTURER, '') ||
         NVL(mhdwr.HARDWARESOLUTION, '') ||
         NVL(hwPlf.PLATFORM, '') ||
         NVL(mhdwr.HARDWARETYPE, '') ) AS LinkedHardware,

                            /*---------------------------------- Design Contact ----------------------------------*/
                            LISTAGG(DISTINCT dcUser.EMAIL, ', ')
                                WITHIN GROUP (ORDER BY dcUser.EMAIL) AS DesignContactEmail,

                            /*---------------------------------- System Type ----------------------------------*/
                            LISTAGG(DISTINCT st.SYSTEMTYPEID, ', ')
                                WITHIN GROUP (ORDER BY st.SYSTEMTYPEID) AS SystemTypeId,

                            /*---------------------------------- Design Component ----------------------------------*/
                            LISTAGG(DISTINCT dc.DESIGNCOMPONENTID, ', ')
                                WITHIN GROUP (ORDER BY dc.DESIGNCOMPONENTID) AS DesignComponentId,

                            /*---------------------------------- LCM ----------------------------------*/
                            LISTAGG(DISTINCT lcm.LCMENGINEERINGID, ', ')
                                WITHIN GROUP (ORDER BY lcm.LCMENGINEERINGID) AS LcmEngineeringId ,

                        '' AS LcmOpcoMapping 

                        FROM MAJORSOFTWAREBUILDS msb

                        /*---------------------------------- Design Contact ----------------------------------*/
                        LEFT JOIN MAJORSWBUILDSDESIGNCONTACTS msbDc
                            ON msbDc.MAJORSOFTWAREBUILDSID = msb.MAJORSOFTWAREBUILDSID and msbDc.deleted = 0

                        LEFT JOIN ASPNETUSERS dcUser
                            ON dcUser.ID = msbDc.DESIGNCONTACTID
                           AND dcUser.ISDESIGNCONTACT = 1

                        /*---------------------------------- System Type ----------------------------------*/
                        LEFT JOIN SYSTEMTYPES st
                            ON st.MAJORSOFTWAREBUILDSID = msb.MAJORSOFTWAREBUILDSID  and st.deleted = 0

                        /*---------------------------------- Major Hardware ----------------------------------*/
                        LEFT JOIN SYSTEMTYPESMAJORHARDWAREBUILDS sthr
                            ON sthr.SYSTEMTYPEID = st.SYSTEMTYPEID
                           AND sthr.ISMAIN = 1  and sthr.deleted = 0

                        LEFT JOIN MAJORHARDWAREBUILDS mhdwr
                            ON sthr.MAJORHARDWAREID = mhdwr.MAJORHARDWAREID  and mhdwr.deleted = 0

                        LEFT JOIN PLATFORMS hwPlf
                            ON hwPlf.PLATFORMID = mhdwr.PLATFORMID

                        LEFT JOIN ORIGINALEQUIPMENTMANUFACTURERS hwoem
                            ON hwoem.ORGEQPMANUFACTURERID = mhdwr.ORGEQPMANUFACTURERID and hwoem.deleted = 0

                        /*---------------------------------- LCM ----------------------------------*/
                        LEFT JOIN DESIGNCOMPONENTS dc
                            ON dc.SYSTEMTYPEID = st.SYSTEMTYPEID and dc.deleted = 0

                        LEFT JOIN LCMENGINEERING lcm
                            ON lcm.DESIGNCOMPONENTID = dc.DESIGNCOMPONENTID and lcm.deleted = 0 and lcm.archived = 0

                        LEFT JOIN OPCOS lcmOpco
                            ON lcmOpco.OPCOID = lcm.OPCOID and lcmOpco.deleted = 0   
                         ";

                return mswQuery;

            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex.StackTrace);
                throw;
            }

        }

        public string GetLcmIdBasedOnOpcoVertical()
        {
            try
            {
                string mswQuery = @"SELECT

                            /*---------------------------------- Major Software Build ----------------------------------*/
                            msb.MAJORSOFTWAREBUILDSID AS MajorSoftwareBuildId,
                            msb.SOFTWAREVERSION       AS SoftwareVersion, 
                            /*---------------------------------- Network Status  ----------------------------------*/
                             CASE
                                 WHEN COUNT(lcm.LCMENGINEERINGID) > 0 THEN 'Live'
                                 ELSE 'Inactive'
                                END as NetworkStatus,  

                            /*---------------------------------- LCM ----------------------------------*/
                            LISTAGG(DISTINCT lcm.LCMENGINEERINGID, ', ')
                                WITHIN GROUP (ORDER BY lcm.LCMENGINEERINGID) AS LcmEngineeringId, 
                            LISTAGG(
                           DISTINCT lcm.LCMENGINEERINGID || '-' || lcmOpco.OPCO||'/'||lcmOpco.Opcoid|| '#' ||vr.VERTICALRESPONSIBLEid,
                            ', ' ) WITHIN GROUP (  ORDER BY lcmOpco.OPCO ) AS LcmOpcoMapping  

                          FROM MAJORSOFTWAREBUILDS msb

                        /*---------------------------------- Design Contact ----------------------------------*/
                        LEFT JOIN MAJORSWBUILDSDESIGNCONTACTS msbDc
                            ON msbDc.MAJORSOFTWAREBUILDSID = msb.MAJORSOFTWAREBUILDSID and msbDc.deleted = 0

                        LEFT JOIN ASPNETUSERS dcUser
                            ON dcUser.ID = msbDc.DESIGNCONTACTID
                           AND dcUser.ISDESIGNCONTACT = 1

                        /*---------------------------------- System Type ----------------------------------*/
                        LEFT JOIN SYSTEMTYPES st
                            ON st.MAJORSOFTWAREBUILDSID = msb.MAJORSOFTWAREBUILDSID  and st.deleted = 0

                        /*---------------------------------- Major Hardware ----------------------------------*/
                        LEFT JOIN SYSTEMTYPESMAJORHARDWAREBUILDS sthr
                            ON sthr.SYSTEMTYPEID = st.SYSTEMTYPEID
                           AND sthr.ISMAIN = 1  and sthr.deleted = 0

                        LEFT JOIN MAJORHARDWAREBUILDS mhdwr
                            ON sthr.MAJORHARDWAREID = mhdwr.MAJORHARDWAREID  and mhdwr.deleted = 0

                        LEFT JOIN PLATFORMS hwPlf
                            ON hwPlf.PLATFORMID = mhdwr.PLATFORMID

                        LEFT JOIN ORIGINALEQUIPMENTMANUFACTURERS hwoem
                            ON hwoem.ORGEQPMANUFACTURERID = mhdwr.ORGEQPMANUFACTURERID and hwoem.deleted = 0

                        /*---------------------------------- LCM ----------------------------------*/
                        LEFT JOIN DESIGNCOMPONENTS dc
                            ON dc.SYSTEMTYPEID = st.SYSTEMTYPEID and dc.deleted = 0

                        LEFT JOIN LCMENGINEERING lcm
                            ON lcm.DESIGNCOMPONENTID = dc.DESIGNCOMPONENTID and lcm.deleted = 0 and lcm.archived = 0

                        LEFT JOIN OPCOS lcmOpco
                            ON lcmOpco.OPCOID = lcm.OPCOID and lcmOpco.deleted = 0


                       LEFT JOIN LCMENGINEERINGSUBDOMAINSPOC subSpoc
                        ON subSpoc.LCMENGINEERINGID = lcm.LCMENGINEERINGID and subSpoc.deleted = 0

                       LEFT JOIN ASPNETUSERS spocUser
                         ON spocUser.ID = subSpoc.SUBDOMAINSPOCID and spocUser.deleted = 0

                        LEFT JOIN ASPNETUSEROPCOS userOpcoMap
                           ON userOpcoMap.USERID = spocUser.ID
                      AND userOpcoMap.ISRESTRICTEDOPCO = 0 and userOpcoMap.deleted = 0 and userOpcoMap.ISINUSEDOPCOS = 1

                      LEFT JOIN OPCOS userOpco
                         ON userOpco.OPCOID = userOpcoMap.OPCOID and userOpco.deleted = 0

                       LEFT JOIN ASPNETUSERVERTICALS userVerticalMap
                           ON userVerticalMap.USERID = spocUser.ID and userVerticalMap.deleted = 0 and userVerticalMap.ISVERTICAL = 1
                            and   userVerticalMap.ISVERTICALRESPONCIBLE = 0 

                      LEFT JOIN ORGANISATION org
                         ON org.ORGANISATIONID = userVerticalMap.ORGANISATIONID  and org.deleted = 0

                        LEFT JOIN VERTICALRESPONSIBLES vr
                          ON vr.VERTICALRESPONSIBLEID = org.VERTICALID and vr.deleted = 0 
                      ";

                return mswQuery;

            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex.StackTrace);
                throw;
            }

        }
    }
}