import React from "react";

import { VodafoneNameApi } from "../../../../Business/LookUp/VodafoneNameBusiness";

import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { ResultDto } from "../../../../Model/CommonModels";

export async function GetProductNameByVodafoneName(
  vodafoneId?: number
) {
  setLoader("ADD", "GetProductNameByVodafoneName");
  let api = new VodafoneNameApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.vodafoneNameGetProductNameByVodafoneName(vodafoneId!)
  );
  setLoader("REMOVE", "GetProductNameByVodafoneName");


  return result?.data ;
//   setLoader("ADD", "GetProductNameByVodafoneName");
//   let result: any;
//   let api = new VodafoneNameApi();
//   result= 
//   await ApiCallWithErrorHandling<Promise<any>>(() =>
//   api.vodafoneNameGetProductNameByVodafoneName(vodafoneId!)
// );
// console.log(result,"result")
  
  
  // if (result && !result.warning) {
  //   setLoader("REMOVE", "GetProductNameByVodafoneName");
  //   return result.data ;
  // } else {
  //   rootStore.dispatch(
  //     setNotification({
  //       message: result?.info ?? "",
  //       notifyType: result?.warning ? NotifyType.error : NotifyType.success,
  //     })
  //   );
  // }
}


