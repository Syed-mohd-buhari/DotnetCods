import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { OrganizationInfoApi } from "../../../Business/OrganizationInfoBusiness";
import {
  GET_FILTER_ORGANIZATION_INFO,
  GET_GRID_ORGANIZATION_INFO,
  OrganizationInfoGrid,
  OrganizationInfoQueryObjectGrid,
  QueryResultDtoOfOrganizationInfoDtoGrid,
} from "../../../Model/OrganizationInfo";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetOrganizationInfoGrid(
  queryFilter?: OrganizationInfoQueryObjectGrid,
  returnValues?: boolean
) {
  setLoader("ADD", "GetOrganizationInfoGrid");

  let result: QueryResultDtoOfOrganizationInfoDtoGrid | null | undefined;
  let api = new OrganizationInfoApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfOrganizationInfoDtoGrid>
    >(() => api.organizationInfoGetGrid(queryFilter ?? {}));

    if (returnValues != true) {
      rootStore.dispatch({
        type: GET_GRID_ORGANIZATION_INFO,
        payload: {
          OrganizationInfoGridResult: result,
          filter: null,
        } as OrganizationInfoGrid,
      });
    } else {
      setLoader("REMOVE", "GetOrganizationInfoGrid");

      return result?.items;
    }
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_ORGANIZATION_INFO,
      payload: {
        OrganizationInfoGridResult: null,
        filter: null,
      } as OrganizationInfoGrid,
    });
  }
  setLoader("REMOVE", "GetOrganizationInfoGrid");
}

export async function GetAllOpCosAndVerticalResponsibles(id: number) {
  setLoader("ADD", "GetAllOpCosAndVerticalResponsibles");

  let api = new OrganizationInfoApi();
  let response = await ApiCallWithErrorHandling<Promise<any>>(() =>
    api.GetAllOpCosAndVerticalResponsibles(id)
  );
  setLoader("REMOVE", "GetAllOpCosAndVerticalResponsibles");

  return response;
}

export async function GetOpCosAndVerticalAndSubDomainResponsibles(
  id: Array<number>
) {
  setLoader("ADD", "GetOpCosAndVerticalAndSubDomainResponsibles");

  let api = new OrganizationInfoApi();
  let response = await ApiCallWithErrorHandling<Promise<any>>(() =>
    api.GetOpCosAndVerticalAndSubDomainResponsibles(id)
  );
  setLoader("REMOVE", "GetOpCosAndVerticalAndSubDomainResponsibles");

  return response;
}

export async function GetFilterColumOrganizationInfo(
  columName: string,
  columValue: string,
  queryFilter?: OrganizationInfoQueryObjectGrid
) {
  let api = new OrganizationInfoApi();
  let result: FilterValueDto[] | undefined;
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.organizationInfoGetFilterResult(
      queryFilter ?? {},
      columName,
      columValue
    )
  );
  let rtn = {
    filter: result,
    OrganizationInfoGridResult: null,
  } as OrganizationInfoGrid;
  rootStore.dispatch({ type: GET_FILTER_ORGANIZATION_INFO, payload: rtn });

  return rtn;
}
