--------------------------------------------------------
--  ROLL BACK ASSET DEPLOYMENT STATUS "POWERED OFF" RECORDS
--------------------------------------------------------

delete from deploymentstatuses where deploymentstatus ='Powered Off' ;

 Commit ;
 