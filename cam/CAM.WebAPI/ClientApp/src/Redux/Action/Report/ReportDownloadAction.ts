import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { ExportApi } from "../../../Business/Report/ExportBusiness";
import { FileResult, ReturnFile } from "../../../Model/Common";
import { ReportQueryAllDto } from "../../../Model/Report/Export";
import { ReportNetworkLevel2QueryGrid } from "../../../Model/Report/LcmExportReport";
import { ReportHardwareQueryObjectGrid } from "../../../Model/Report/ReportHardwareModel";
import { ReportNetworkLevel2Grid } from "../../../Model/Report/ReportLcmExportModel";
import { ReportSoftwareQueryObjectGrid } from "../../../Model/Report/ReportSoftwareModel";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function DownloadReport(query: ReportQueryAllDto) {
  setLoader("ADD", "DownloadReport");
  let api = new ExportApi();
  try {
    let res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.exportExportReport(query)
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    // console.log("JSONONE DOWNLOAD", JSON.stringify(query))
    setLoader("REMOVE", "DownloadReport");
    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "DownloadReport");
}
export async function exportSubnetworkAllReport(query: ReportQueryAllDto) {
  setLoader("ADD", "exportSubnetworkAllReport");
  let api = new ExportApi();
  try {
    let res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.exportSubnetworkAllReport(query)
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    // console.log("JSONONE DOWNLOAD", JSON.stringify(query))
    setLoader("REMOVE", "exportSubnetworkAllReport");
    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "exportSubnetworkAllReport");
}
export async function exportLcmAllReport(query: ReportQueryAllDto) {
  setLoader("ADD", "exportLcmAllReport");
  let api = new ExportApi();
  try {
    let res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.exportLcmAllReport(query)
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    // console.log("JSONONE DOWNLOAD", JSON.stringify(query))
    setLoader("REMOVE", "exportLcmAllReport");
    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "exportLcmAllReport");
}
export async function NetworkLevel2DownloadReport(
  query: ReportNetworkLevel2QueryGrid
) {
  setLoader("ADD", "NetworkLevel2DownloadReport");
  let api = new ExportApi();
  try {
    let res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.networkLevel2ExportReport(query)
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    // console.log("JSONONE DOWNLOAD", JSON.stringify(query))
    setLoader("REMOVE", "NetworkLevel2DownloadReport");
    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "NetworkLevel2DownloadReport");
}
export async function HardwareConfigDownloadReport(
  query: ReportHardwareQueryObjectGrid
) {
  setLoader("ADD", "HardwareConfigDownloadReport");
  let api = new ExportApi();
  try {
    let res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.hardwareConfigExportReport(query)
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    // console.log("JSONONE DOWNLOAD", JSON.stringify(query))
    setLoader("REMOVE", "HardwareConfigDownloadReport");
    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "HardwareConfigDownloadReport");
}
export async function SubNetworkDownloadSWReport(
  query: ReportSoftwareQueryObjectGrid
) {
  setLoader("ADD", "SubNetworkDownloadSWReport");
  let api = new ExportApi();
  try {
    let res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.swSubNetworkExportReport(query)
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    // console.log("JSONONE DOWNLOAD", JSON.stringify(query))
    setLoader("REMOVE", "SubNetworkDownloadSWReport");
    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "SubNetworkDownloadSWReport");
}
export async function SubNetworkDownloadHWReport(
  query: ReportHardwareQueryObjectGrid
) {
  setLoader("ADD", "SubNetworkDownloadHWReport");
  let api = new ExportApi();
  try {
    let res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.hwSubNetworkExportReport(query)
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    // console.log("JSONONE DOWNLOAD", JSON.stringify(query))
    setLoader("REMOVE", "SubNetworkDownloadHWReport");
    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "SubNetworkDownloadHWReport");
}
