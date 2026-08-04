// NfvicCompatibleGridAction.ts

import {
    ApiCallWithErrorHandling,
    FilterValueDto,
  } from "../../../Business/Common/CommonBusiness";
  import { NFVICCompatibleApi } from "../../../Business/NFVICompatibilityBusiness";
  import {
    NFVICCompatibilityGrid,
    NFVICCompatibilityQueryDto,
    QueryResultDtoOfNFVICCompatibilityDtoGrid,
    VmWareDropdownResponse,
    GET_GRID_NFVIC_COMPATIBILITY,
    GET_FILTER_NFVIC_COMPATIBILITY,
    GET_VMWARE_DROPDOWN,
  } from "../../../Model/NfvicCompatible";
  import { NotifyType } from "../../Reducer/NotificationReducer";
  import { rootStore } from "../../Store/rootStore";
  import setLoader from "../LoaderAction";
  import { setNotification } from "../NotificationAction";
  
  // Action to fetch NFVI Compatibility Grid
  export async function GetNFVICCompatibleGrid(
    vmVarId: number,
    query?: NFVICCompatibilityQueryDto,
    returnValues?: boolean
  ) {
    setLoader("ADD", "GetNFVICCompatibleGrid");
  
    const api = new NFVICCompatibleApi();
  
    try {
      // Default query parameters if not provided
      const finalQuery = query ?? {
        sortBy: "",
        isSortAscending: true,
        page: 1,
        pageSize: 20,
        deleted: false,
        orphan: false,
        lastModified: {},
        lastModifiedBy: [],
        lastModifiedValue: {},
        principalId: 0,
        market: [],
        application: [],
        domain: [],
        designComponent: [],
        currentVNF: [],
        minimumVNF: [],
        plannedVNF: [],
        status: [],
        deleiveryStatus: [],
        eduSpoc: [],
        subDomainSpoc: [],
        plannedUpgrade: {},
      };
  
      // Fetch the grid data
      const result = await ApiCallWithErrorHandling(() =>
        api.nfvicCompatibleGetGrid(vmVarId, finalQuery)
      );
  
      if (!returnValues) {
        rootStore.dispatch({
          type: GET_GRID_NFVIC_COMPATIBILITY,
          payload: {
            NFVICCompatibilityGridResult: result,
            filter: null, // You can modify this if needed
          } as NFVICCompatibilityGrid,
        });
      } else {
        setLoader("REMOVE", "GetNFVICCompatibleGrid");
        return result?.items;
      }
    } catch (error) {
      rootStore.dispatch(
        setNotification({
          message: "Failed to fetch NFVI Compatibility Grid",
          notifyType: NotifyType.error,
        })
      );
    }
  
    setLoader("REMOVE", "GetNFVICCompatibleGrid");
  }
  
  // Action to fetch VMware dropdown data
  export const GetVmWareDropdown = () => {
    return async (dispatch: any) => {
      const api = new NFVICCompatibleApi();
  
      try {
        // Fetch the dropdown data
        const result: VmWareDropdownResponse | undefined = await ApiCallWithErrorHandling(() =>
          api.nfvicCompatibleGetAllVmware()
        );
  
        // Check if the result is valid and return data
        if (result && result.data) {
          dispatch({
            type: GET_VMWARE_DROPDOWN,
            payload: { dropdowns: result.data }, // Dispatch dropdowns to the reducer
          });
          return result.data;
        } else {
          throw new Error("Invalid response structure from VmWare API");
        }
      } catch (error) {
        dispatch(
          setNotification({
            message: "Failed to load VmWare dropdown",
            notifyType: NotifyType.error,
          })
        );
      }
    };
  };
  
  
  