import {
    ApiCallWithErrorHandling,
    FilterValueDto,
  } from "../../../Business/Common/CommonBusiness";
import { LcmEngAuditApi } from "../../../Business/LcmEngAuditBusiness";
 import { GET_FILTER_LCM_ENG_AUDIT, GET_GRID_LCM_ENG_AUDIT, LcmEngAuditGrid, LcmEngAuditQueryDto, QueryResultDtoOfLcmEngAuditDtoGrid } from "../../../Model/LcmEngAudit";
  import { NotifyType } from "../../Reducer/NotificationReducer";
  import { rootStore } from "../../Store/rootStore";
  import setLoader from "../LoaderAction";
  import { setNotification } from "../NotificationAction";
  
  export async function GetLcmEngAuditGrid(
    queryFilter?: LcmEngAuditQueryDto
  ) {
    let api = new LcmEngAuditApi();
    let result: QueryResultDtoOfLcmEngAuditDtoGrid | null | undefined;
    setLoader("ADD", "GetLcmEngAuditGrid");
    try {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfLcmEngAuditDtoGrid>
      >(() => api.lcmEngineeringGetLcmEngAudit(queryFilter ?? {}));
      let rtn = {
        LcmEngAuditGridResult: result,
        filter: null,
      } as LcmEngAuditGrid;
      rootStore.dispatch({ type: GET_GRID_LCM_ENG_AUDIT, payload: rtn });
    } catch (error) {
      rootStore.dispatch(
        setNotification({
          message: "Fail to fetch",
          notifyType: NotifyType.error,
        })
      );
      rootStore.dispatch({
        type: GET_GRID_LCM_ENG_AUDIT,
        payload: {
          LcmEngAuditGridResult: null,
          filter: null,
        } as LcmEngAuditGrid,
      });
    }
    setLoader("REMOVE", "GetLcmEngAuditGrid");
  }
  
  export async function GetFilterColumLcmEngAudit(
    columName: string,
    columValue: string,
    queryFilter?: LcmEngAuditQueryDto
  ) {
    let result: FilterValueDto[] | undefined;
    let api = new LcmEngAuditApi();
  
    // result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    //   api.lcmEngAuditGetFilterResult(queryFilter ?? {}, columName, columValue)
    // );
    // let rtn = {
    //   filter: result,
    //   LcmEngAuditGridResult: null,
    // } as LcmEngAuditGrid;
    // rootStore.dispatch({ type: GET_FILTER_LCM_ENG_AUDIT, payload: rtn });
  }
  