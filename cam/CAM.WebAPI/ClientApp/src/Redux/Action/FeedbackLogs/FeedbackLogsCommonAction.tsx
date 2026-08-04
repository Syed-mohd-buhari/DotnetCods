import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import {
  GET_FEEDBACK_LOGS,
  FeedbackLogsApi,
} from "../../../Business/FeedbackLogsBusiness";
import { ReturnFile, FileResult } from "../../../Model/Common";
import { ResultDto } from "../../../Model/CommonModels";
import {
  GET_GRID_FEEDBACK_LOGS,
  FeedbackLogsGrid,
  FeedbackLogsQueryObjectGrid,
  QueryResultDtoOfFeedbackLogsDtoGrid,
  GET_FILTER_FEEDBACK_LOGS,
} from "../../../Model/FeedbackLogs";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetFeedbackLogsGrid(
  queryFilter?: FeedbackLogsQueryObjectGrid
) {
  setLoader("ADD", "GetFeedbackLogsGrid");
  let api = new FeedbackLogsApi();

  let result: QueryResultDtoOfFeedbackLogsDtoGrid | null | undefined;
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfFeedbackLogsDtoGrid>
    >(() => api.getFeedbackLogsGrid(queryFilter ?? {}));

    let rtn = {
      FeedbackLogsGridResult: result,
      filter: null,
    } as unknown as FeedbackLogsGrid;

    rootStore.dispatch({ type: GET_GRID_FEEDBACK_LOGS, payload: rtn });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_FEEDBACK_LOGS,
      payload: {
        FeedbackLogsGridResult: null,
        filter: null,
      } as unknown as FeedbackLogsGrid,
    });
  }
  setLoader("REMOVE", "GetFeedbackLogsGrid");
}

export async function GetFilterColumFeedbackLogs(
  columName: string,
  columValue: string,
  queryFilter?: FeedbackLogsQueryObjectGrid
) {
  let result: FilterValueDto[] | undefined;
  let api = new FeedbackLogsApi();
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.FeedbackLogsGetFilterResult(queryFilter ?? {}, columName, columValue)
  );
  let rtn = {
    filter: result,
    FeedbackLogsGridResult: null,
  } as unknown as FeedbackLogsGrid;
  rootStore.dispatch({ type: GET_FILTER_FEEDBACK_LOGS, payload: rtn });
}

export async function DownloadFeedbackLogs(data: any) {
  setLoader("ADD", "DownloadFeedbackLogs");
  let api = new FeedbackLogsApi();
  let res: ReturnFile | undefined;
  try {
    res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.DownloadFeedbackLogs(data)
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    setLoader("REMOVE", "DownloadFeedbackLogs");

    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "DownloadFeedbackLogs");
}
