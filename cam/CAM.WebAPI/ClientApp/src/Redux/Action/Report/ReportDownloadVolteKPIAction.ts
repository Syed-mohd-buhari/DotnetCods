import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { VolteKPIApi } from "../../../Business/VolteKPIBusiness";
import { FileResult, ReturnFile } from "../../../Model/Common";
import { ReportQueryAllDto } from "../../../Model/Report/Export";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

// export async function DownloadVolteKPIReport(year: number) {
// 	 ;
// 	let api = new VolteKPIApi();

// 	let res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() => api.volteKPIExportReport(undefined, undefined, undefined, undefined, year));
// 	let fileNameBase = res?.FileName.split(";")[1] ?? "";
// 	var index = fileNameBase?.indexOf('"') + 1;
// 	var lastIndex = fileNameBase?.indexOf('"', index);
// 	let fileName = fileNameBase?.substring(index, lastIndex);
// 	let result = res?.File.then((x) => {
// 		return { file: x, fileName: fileName } as FileResult;
// 	});
// 	// console.log("JSONONE DOWNLOAD", JSON.stringify(query))
// 	 ;
// 	return result;
// }

export async function DownloadVolteKPIReport(opCo: number[], year?: number) {
  setLoader("ADD", "DownloadVolteKPIReport");
  let api = new VolteKPIApi();

  let res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
    api.volteKPIExportReport(undefined, opCo, undefined, undefined, year)
  );
  let fileNameBase = res?.FileName.split(";")[1] ?? "";
  var index = fileNameBase?.indexOf('"') + 1;
  var lastIndex = fileNameBase?.indexOf('"', index);
  let fileName = fileNameBase?.substring(index, lastIndex);
  let result = res?.File.then((x) => {
    return { file: x, fileName: fileName } as FileResult;
  });
  setLoader("REMOVE", "DownloadVolteKPIReport");
  return result;
}
