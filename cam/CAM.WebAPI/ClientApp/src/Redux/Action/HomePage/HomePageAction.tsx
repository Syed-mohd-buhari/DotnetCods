import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { HomePageApi } from "../../../Business/HomePageBusiness";
import { achievementDto } from "../../../NewLandingScreens/layouts/MainLayout/MainContent";
import setLoader from "../LoaderAction";

export async function GetAchivementAndSignPostRecord(body: achievementDto) {
  let api = new HomePageApi();
  let response = await ApiCallWithErrorHandling<Promise<any>>(() =>
    api.achivementAndSignPostRecordGet(body)
  );

  return response;
}

export async function GetOpenLinkRecordCount(id: number, roleId: number) {
  let api = new HomePageApi();
  let response = await ApiCallWithErrorHandling<Promise<any>>(() =>
    api.OpenLinkRecordCountGet(id, roleId)
  );

  return response;
}

export async function GetExpiredEomEosActions(id: number, roleId: number) {
  let api = new HomePageApi();
  let response = await ApiCallWithErrorHandling<Promise<any>>(() =>
    api.ExpiredEomEosActionsGet(id, roleId)
  );

  return response;
}

export async function GetUpcomingLinkRecordCount(id: number, roleId: number) {
  let api = new HomePageApi();
  let response = await ApiCallWithErrorHandling<Promise<any>>(() =>
    api.UpcomingLinkRecordCountGet(id, roleId)
  );

  return response;
}

export async function GetUpcomingEomEosActions(id: number, roleId: number) {
  let api = new HomePageApi();
  let response = await ApiCallWithErrorHandling<Promise<any>>(() =>
    api.UpcomingEomEosActionsGet(id, roleId)
  );

  return response;
}

export async function GetComplainceGraphData(id: number, roleId: number) {
  let api = new HomePageApi();
  let response = await ApiCallWithErrorHandling<Promise<any>>(() =>
    api.ComplainceGraphDataGet(id, roleId)
  );

  return response;
}
