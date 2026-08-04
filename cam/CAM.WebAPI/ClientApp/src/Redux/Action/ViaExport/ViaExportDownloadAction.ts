import {
  ApiCallWithErrorHandling,
  BASE_PATH,
} from "../../../Business/Common/CommonBusiness";
import { ExportApi } from "../../../Business/Report/ExportBusiness";
import { FileResult, ReturnFile } from "../../../Model/Common";
import setLoader from "../LoaderAction";
import { ReportViaQueryAllDto } from "../../../Model/ViaExport/ExportViaExport";

export async function DownloadViaReport(query: ReportViaQueryAllDto) {
  setLoader("ADD", "DownloadViaReport");
  let api = new ExportApi();

  let res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
    api.exportExportViaReport(query)
  );
  let fileNameBase = res?.FileName.split(";")[1] ?? "";
  var index = fileNameBase?.indexOf('"') + 1;
  var lastIndex = fileNameBase?.indexOf('"', index);
  let fileName = fileNameBase?.substring(index, lastIndex);

  let result = res?.File.then((x) => {
    return { file: x, fileName: fileName } as FileResult;
  });
  // console.log("JSONONE DOWNLOAD", JSON.stringify(query))
  setLoader("REMOVE", "DownloadViaReport");
  return result;
}

export async function ChangeDbMode(mode?: string) {
  setLoader("ADD", "ChangeDbMode");
  let api = new ExportApi();

  let res = await ApiCallWithErrorHandling<Promise<any>>(() =>
    api.changeDbMode(mode)
  );
  console.log(res);
  setLoader("REMOVE", "ChangeDbMode");
  return res;
}

export async function DownloadCSV(query: ReportViaQueryAllDto, type: string) {
  setLoader("ADD", "DownloadViaReport");
  let api = new ExportApi();

  let res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
    api.exportExportViaCSV(query, type)
  );
  let fileNameBase = res?.FileName.split(";")[1] ?? "";
  var index = fileNameBase?.indexOf('"') + 1;
  var lastIndex = fileNameBase?.indexOf('"', index);
  let fileName = fileNameBase?.substring(index, lastIndex);
  let result = res?.File.then((x) => {
    return { file: x, fileName: fileName } as FileResult;
  });
  setLoader("REMOVE", "DownloadViaReport");
  return result;
}
