import moment from "moment";
import "moment/locale/en-gb";
import * as XLSX from "xlsx-js-style";
import ExcelJS from "exceljs";
import LabelsDictionary from "../Constant/LabelsAndDescriptions.json";
import { GetDesignComponentOrphans } from "../Redux/Action/ForeignIndex/ForeignIndexCommonAction";
import { setNotification } from "../Redux/Action/NotificationAction";
import { NotifyType } from "../Redux/Reducer/NotificationReducer";
import { rootStore } from "../Redux/Store/rootStore";
import { GetLcmEngineeringImportStatus } from "../Redux/Action/LcmEngineering/LcmEngineeringDownloadAction";
import setLoader from "../Redux/Action/LoaderAction";
import { GetNetworkElementAsIsImportStatus } from "../Redux/Action/NetworkElementAsIs/NetworkElementAsIsCreateAction";
import { GetDeliveryTrackingImportStatus } from "../Redux/Action/DeliveryTracking/DeliveryTrackingCreateAction";
import { GetIdentityAsIsImportStatus } from "../Redux/Action/IdentityAsIs/IdentityAsIsCreateAction";
import { GetTSRReportImportStatus } from "../Redux/Action/TSRReport/TSRReportImportAction";
import { GetTemsFNTReportImportStatus } from "../Redux/Action/FNTReport/TemsFNTReportImportAction";
import { GetPassThroughReportImportStatus } from "../Redux/Action/Report/PassThroughImportGridAction";
import { GetPassThroughHardwareReportImportStatus } from "../Redux/Action/Report/PassThroughHardwareImportGridAction";
import { GetPassThroughSoftwareReportImportStatus } from "../Redux/Action/Report/PassThroughSoftwareImportGridAction";
import { importNFVISoftwareCompatibleExcel } from "../Redux/Action/NFVISoftwareCompatible/NFVICSoftwareCompatibleImportAction";
import { ImportVBOMInfoExcel } from "../Redux/Action/VBOMInfo/VBOMInfoImportAction";
import { ImportBPTReport } from "../Redux/Action/BPTReport/BPTReportImportAction";
import { ImportCBOMExcel } from "../Redux/Action/CBOM/CBOMImportAction";

const getNavigatorLanguage = () => {
  const lang = navigator.languages?.[0] || navigator.language || "en-gb";

  return lang.toLowerCase();
};

if (getNavigatorLanguage() !== undefined) {
  moment.locale(getNavigatorLanguage());
}

export const toggleState = (newState: number, state: number) => {
  return state === newState ? 0 : newState;
};

export const toggleStateString = (newState: string, state: string) => {
  return state === newState ? "" : newState;
};

export function lowerFirstLetter(string: any) {
  return string.charAt(0).toLowerCase() + string.slice(1);
}
export function upperFirstLetter(string: string) {
  const str2 = string.charAt(0).toUpperCase() + string.slice(1);
  return str2;
}

export function formatTime(time: Date | undefined) {
  if (time === undefined || time === null) return "";
  time = new Date(time);
  try {
    return formatDate(time).toString();
  } catch (error) {
    console.log(error);
  }
}

export function formatDate(date: Date) {
  try {
    var locale = window.navigator.languages[1] || window.navigator.language;
    moment().locale(locale);
    if (date === undefined) return "";
    var utcData = moment.utc(date).toDate();
    return moment(utcData).local().format("MM-DD-YYYY");
  } catch (error) {
    return "";
  }
}

export function convertDateToString(date: Date) {
  return date.getDay() + "" + date.getMonth() + "" + date.getFullYear();
}

export function convertEnumToArray(enumData: any): Array<any> {
  return (Object.keys(enumData) as Array<any>)
    .filter((key) => isNaN(Number(key)))
    .filter(
      (key) =>
        typeof enumData[key] === "number" || typeof enumData[key] === "string"
    )
    .map((key) => ({
      key: enumData[key],
      value: String(key).replace("/_/g", " "),
    }));
}

export function formatDateWithTime(date: any) {
  if (date === undefined) return;
  var locale = window.navigator.languages[1] || window.navigator.language;
  moment().locale("en");
  var utcData = moment.utc(date).toDate();
  let value = moment(utcData).locale("en").format("DD/MM/YYYY LT");
  if (value === "Invalid date") {
    return "";
  } else {
    return value;
  }
}

export const safeNumber = (value: any): number => {
  const num = Number(value);
  return isNaN(num) ? 0 : num;
};

export function getMaxDate(all_dates: any) {
  var max_dt = all_dates[0],
    max_dtObj = new Date(all_dates[0]);
  var dateIndex = 0;
  all_dates.forEach(function (dt, index) {
    if (new Date(dt) < max_dtObj) {
      max_dt = dt;
      max_dtObj = new Date(dt);
      dateIndex = index;
    }
  });
  return { date: max_dt, index: dateIndex };
}

export function formatDateOnlyString(date: Date | undefined | null | string) {
  if (date === undefined || date === null || date === "") return "";
  const d = new Date(date);

  const day = String(d.getDate()).padStart(2, "0");
  const month = String(d.getMonth() + 1).padStart(2, "0");
  const year = d.getFullYear();

  return `${day}/${month}/${year}`;
}

export function formatTimeLocal(time: Date | undefined | null | string) {
  if (time === undefined) return "";
  if (time == null) return "";
  var locale = window.navigator.languages[1] || window.navigator.language;
  moment.locale("en");
  var utcData = moment.utc(time).toDate();
  var dateAfterParse = moment(utcData).locale("en");
  let value = dateAfterParse.format("DD/MM/YYYY");
  if (value === "Invalid date") {
    return "";
  } else {
    return value;
  }
}
export const filterObjectArrayWithObjectArray = (
  arr: any[],
  itemsToRemove: any[],
  propertyToRemove: string,
  propertyToFilter: string
) => {
  if (itemsToRemove === undefined) return arr;
  let keysToRemove = itemsToRemove.map((x) => {
    return x[propertyToRemove] as string | number;
  });
  let result = arr.filter(
    (item) => !keysToRemove.includes(item[propertyToFilter])
  );
  return result;
};

let delayTimeout: NodeJS.Timeout;

export const delayClose = (closeAction: () => any, delayTimeSpan = 1800) => {
  delayTimeout = setTimeout(closeAction, delayTimeSpan);
};

export const cancelDelayClose = () => {
  clearTimeout(delayTimeout);
};

export function AddMonth(date: Date, numberMonth: number): Date | string {
  let locale = window.navigator.languages[1] || window.navigator.language;
  moment.locale(locale);
  const updatedDate = moment(date).add(numberMonth, "months").toDate();
  const endOfMonthOfUpdatedDate = moment(updatedDate).endOf("month").toDate();
  const endOfMonth = moment(date).endOf("month").toDate();

  let stringDate;
  if (date.getDate() === endOfMonth.getDate()) {
    stringDate = `${endOfMonthOfUpdatedDate.getFullYear()}/${
      endOfMonthOfUpdatedDate.getMonth() + 1
    }/${endOfMonthOfUpdatedDate.getDate()}`;
  } else {
    stringDate = `${updatedDate.getFullYear()}/${
      updatedDate.getMonth() + 1
    }/${updatedDate.getDate()}`;
  }

  return stringDate;
}

export function subtractMonths(date: Date, numberMonth: number): string {
  let locale = window.navigator.languages[1] || window.navigator.language;
  moment.locale(locale);

  const updatedDate = moment(date).subtract(numberMonth, "months").toDate();
  const endOfMonthOfUpdatedDate = moment(updatedDate).endOf("month").toDate();
  const endOfMonth = moment(date).endOf("month").toDate();
  let stringDate;
  if (date.getDate() === endOfMonth.getDate()) {
    stringDate = `${endOfMonthOfUpdatedDate.getFullYear()}/${
      endOfMonthOfUpdatedDate.getMonth() + 1
    }/${endOfMonthOfUpdatedDate.getDate()}`;
  } else {
    stringDate = `${updatedDate.getFullYear()}/${
      updatedDate.getMonth() + 1
    }/${updatedDate.getDate()}`;
  }
  return stringDate;
}

// export function RemoveMonth(date: Date, numberMonth: number): Date {
//   let locale = window.navigator.languages[1] || window.navigator.language;
//   moment.locale(locale);

//   return moment.utc(date).startOf(numberMonth).toDate();
// }

export const getCustomStyles = (isDisabled: any) => ({
  control: (styles) => ({
    ...styles,
    backgroundColor: isDisabled ? "hsl(0, 0%, 95%)" : "white",
    cursor: isDisabled ? "not-allowed" : "default",
  }),
  option: (styles, { data, isDisabled, isFocused, isSelected }) => {
    return {
      ...styles,
      color: data.color ?? "black",
    };
  },
});

export const colourStyles = {
  control: (styles) => ({ ...styles, backgroundColor: "white" }),
  option: (styles, { data, isDisabled, isFocused, isSelected }) => {
    return {
      ...styles,
      color: data.color ?? "black",
    };
  },
};

export const optionValue = (data: string) => {
  if (data && data !== "" && data != null) {
    try {
      let teset = "";
      var dataObj = JSON.parse(data);
      for (var key in dataObj) {
        teset += `${key} : ${dataObj[key]}  </br> `;
      }
      return teset;
    } catch (error) {
      return data;
    }
  }
};

export const rtnRuleGrid = (
  id: number | undefined,
  rulesResource: { key: number; value: string }[]
) => {
  if (id != undefined) {
    return rulesResource.find((x) => x.key == id)?.value;
  } else {
    return "None";
  }
};

export const rtnVersionApp = () => {
  return "6.24-3.19.8";
};

export const InitializeForeignIndex = async (openModal: Function) => {
  await GetDesignComponentOrphans().then((x) => {
    if (x && x != undefined) {
      if (!x.warning) {
        openModal(true);
      }
    }
  });
};

export const changeDate = (id: string, date: Date | undefined) => {
  let target = document.getElementById(id);
  target?.setAttribute("type", "date");
};

export const stringIsNullOrEmpty = (text: string | undefined) => {
  return text == null || text == undefined || text === "";
};

export const numberIsNullOrZero = (number: number | undefined) => {
  return number == null || number == undefined || number === 0;
};

export const listIsNullOrEmpty = (list: Array<any> | undefined) => {
  return list === null || list === undefined || list.length === 0;
};

export const changeText = (id: string, date: Date | undefined) => {
  if (date == undefined || date == null) {
    let target = document.getElementById(id);
    target?.setAttribute("type", "text");
  }
};

export const currencyOption = [
  { key: 1, value: "€" },
  { key: 2, value: "$" },
  { key: 3, value: "£" },
];
export const boolOptions = [
  { key: "YES", value: "YES" },
  { key: "NO", value: "NO" },
];
interface ColumnWidth {
  wch: number;
}
const calculateColumnWidths = (ws, numRows, numCols, jsonLength) => {
  const columnWidths: Array<ColumnWidth> = [];
  const range = XLSX.utils.decode_range(ws["!ref"]);

  for (let col = range.s.c; col <= range.e.c; col++) {
    let maxLength = 0;

    const headerCell = ws[XLSX.utils.encode_cell({ r: range.s.r, c: col })];

    if (headerCell && headerCell.v) {
      const headerLength = String(headerCell.v).length;
      if (headerLength > maxLength) {
        maxLength = headerLength;
      }
    }

    for (let row = range.s.r + 1; row <= range.e.r; row++) {
      const cell = ws[XLSX.utils.encode_cell({ r: row, c: col })];
      if (cell && cell.v) {
        const contentLength = String(cell.v).length;
        if (contentLength > maxLength) {
          maxLength = contentLength;
        }
      }
    }

    columnWidths[col] = { wch: maxLength + 2 };
  }
  return columnWidths;
};
export const headerCaseChange = (getData) => {
  const result = getData.map((res) => {
    const upperCaseItem = {};
    for (const key in res) {
      upperCaseItem[LabelsDictionary[key].Full] = res[key];
    }
    return upperCaseItem;
  });
  return result;
};

const campareArrays = (array1, array2) => {
  const missingObjects: any = [];
  const set1 = new Set(array1);

  for (const obj2 of array2) {
    if (!set1.has(obj2)) {
      missingObjects.push(obj2);
    }
  }

  return missingObjects;
};

export const downloadXLSX = (
  url: string,
  pageType: any,
  fileName: string,
  filterCondition?: any
) => {
  fetch(url)
    .then((response) => response.blob())
    .then((blob) => {
      const reader = new FileReader();
      reader.onload = function (event) {
        const fileContent: any = event.target?.result;
        let totalCount = 0;
        try {
          const workbook = XLSX.read(fileContent, { type: "binary" });

          const apiData: any = XLSX.utils.sheet_to_json(
            workbook.Sheets[workbook.SheetNames[0]]
          );
          // console.log(apiData);
          // console.log(
          //   "Excel Data :",
          //   apiData?.length > 0 ? "Data Available" : apiData
          // );
          // console.log(
          //   "Excel Filter Data :",
          //   filterCondition?.length > 0 ? "Data Available" : filterCondition
          // );
          if (apiData) {
            let dataList: any = [];
            if (pageType === "PlannedActivityTracker") {
              let finalObj: any = {};
              let filterList: any = [];
              let bundleList: any = [];
              let vnfmList: any = [];
              for (const service of Object.values(filterCondition)) {
                const serObj = apiData?.filter(
                  (values: any) => values?.["ProductName"] === service
                );
                totalCount += serObj.length;
                if (service === "Bundle" && serObj.length > 0) {
                  bundleList = bundleList.concat(serObj);
                } else if (service === "VNFM" && serObj.length > 0) {
                  vnfmList = vnfmList.concat(serObj);
                } else {
                  if (serObj.length > 0) {
                    finalObj[`${service}`] = serObj;
                    filterList = filterList.concat(serObj);
                  }
                }
              }
              const missingObject = campareArrays(
                [...filterList, ...bundleList, ...vnfmList],
                apiData
              );
              for (const key in finalObj) {
                finalObj[key]?.length > 0 &&
                  dataList.push({
                    listData: [
                      ...finalObj[key]?.map(({ ProductName, ...rest }) => rest),
                      ...bundleList?.map(({ ProductName, ...rest }) => rest),
                      ...vnfmList?.map(({ ProductName, ...rest }) => rest),
                    ],
                    sheetName: key?.replaceAll("/", "-"),
                    options: {
                      filter: false,
                    },
                  });
              }
              bundleList?.length > 0 &&
                dataList.push({
                  listData: bundleList?.map(({ ProductName, ...rest }) => rest),
                  sheetName: "Bundle",
                  options: {
                    filter: false,
                  },
                });
              vnfmList?.length > 0 &&
                dataList.push({
                  listData: vnfmList?.map(({ ProductName, ...rest }) => rest),
                  sheetName: "VNFM",
                  options: {
                    filter: false,
                  },
                });
              totalCount += missingObject.length;
              missingObject.length > 0 &&
                dataList.push({
                  listData: [
                    ...missingObject?.map(({ ProductName, ...rest }) => rest),
                    ...bundleList?.map(({ ProductName, ...rest }) => rest),
                    ,
                    ...vnfmList?.map(({ ProductName, ...rest }) => rest),
                  ],
                  sheetName: "Others",
                  options: {
                    filter: false,
                  },
                });
              // console.log(apiData,dataList)
              // console.log(apiData.length, totalCount, missingObject.length)
            } else if (pageType === "PlannedActivityTrackerAll") {
              apiData?.length > 0 &&
                dataList.push({
                  listData: apiData?.map(({ ProductName, ...rest }) => rest),
                  sheetName: "PAT",
                  options: {
                    filter: false,
                  },
                });
            } else {
              const filterCriteria = { nodeType: ["3581035", "111053385"] };
              const filterRecords = (records, criteria) => {
                for (const key in criteria) {
                  for (const res of criteria[key]) {
                    let filterData = records?.filter((item) =>
                      res.includes(item[key])
                    );
                    filterData?.length > 0 &&
                      dataList.push({
                        listData: filterData,
                        sheetName: res?.replace("/", "_"),
                        options: {
                          filter: false,
                        },
                      });
                  }
                }
              };
              filterRecords(apiData, filterCriteria);
            }
            // console.log(dataList);
            xlsxSheetCreation(dataList, fileName.replace("/", ""));
          }
        } catch (error) {
          console.log("Error parsing JSON", error);
        }
      };
      reader.readAsBinaryString(blob);
    })
    .catch((error) => {
      console.log("Error fetching file");
    });
};
export const xlsxSheetCreation = (
  dataList: Array<any> | undefined,
  fileName: string
) => {
  const workbook = XLSX.utils.book_new();
  dataList?.map((list) => {
    const {
      listData,
      sheetName,
      options: { filter },
    } = list;
    const normalizedData = (apiData: any) => {
      if (Array.isArray(apiData) && apiData.every(Array.isArray)) {
        return apiData.flatMap((subArray) => subArray);
      } else {
        return apiData;
      }
    };
    const newListData = normalizedData(listData);
    // console.log(typeof newListData, newListData);
    const findCommonKeys = (arr) => {
      if (arr.length === 0) return [];

      // Get the keys of the first object
      let common = Object.keys(arr[0]);

      // Loop through the rest of the objects and get the intersection of keys
      arr.forEach((obj) => {
        common = common.filter((key) => key in obj);
      });

      return common;
    };

    // Calculate common keys and set state
    const commonKeysResult = findCommonKeys(newListData);
    const worksheet = XLSX.utils.json_to_sheet(listData, {
      header: commonKeysResult,
    });

    const headerCellStyle = {
      font: { bold: true },
      fill: { fgColor: { rgb: "D3D3D3" } },
    };
    // console.log("Header", commonKeysResult, Object.keys(newListData?.[0]));
    const numRows = newListData?.length;
    const numCols = commonKeysResult.length;

    // Loop through the commonKeysResult to style header cells
    commonKeysResult.forEach((key, index) => {
      const headerCell = XLSX.utils.encode_cell({ r: 0, c: index }); // Row 0 for headers
      worksheet[headerCell].s = headerCellStyle; // Apply the header style
    });
    // Utility function to convert color name to RGB format (e.g., "red" => "FF0000")
    const colorMap: { [key: string]: string } = {
      red: "FF0000",
      blue: "2F75B5",
      green: "00c300",
      yellow: "ffff00",
      gray: "cccccc",
      black: "0F0D0D",
      // Add more colors as needed
    };

    const getColor = (colorName: string) =>
      colorMap[colorName?.trim()] || "FFFFFF"; // Default to white if color not found
    // Rearrange keys to place those with values containing '|' first
    const keysWithoutColorsFirst = commonKeysResult
      .filter((key) => {
        // Check if any value in the column for this key contains '|'
        return !newListData.some(
          (rowData) =>
            typeof rowData[key] === "string" && rowData[key].includes("|")
        );
      })
      .concat(
        commonKeysResult.filter((key) => {
          // Place keys without '|' after those with '|' symbol
          return newListData.some(
            (rowData) =>
              typeof rowData[key] === "string" && rowData[key].includes("|")
          );
        })
      );
    // Apply header cell styling based on commonKeysResult
    keysWithoutColorsFirst.forEach((key, colIndex) => {
      // Apply the rearranged header cell
      const headerCell = XLSX.utils.encode_cell({ r: 0, c: colIndex });
      worksheet[headerCell] = { v: key }; // Set header key in the Excel sheet
      worksheet[headerCell].s = headerCellStyle; // Apply header cell style

      // Now apply value cell styles for each row under this header
      newListData.forEach((rowData, rowIndex) => {
        const cellValue = rowData[key];
        const valueCell = XLSX.utils.encode_cell({
          r: rowIndex + 1, // Add 1 since headers are in row 0
          c: colIndex,
        });

        if (typeof cellValue === "string" && cellValue.includes("|")) {
          const [value, color] = cellValue.split("|"); // Split the value and color
          worksheet[valueCell] = { v: value.trim() }; // Set the cell value without the color

          // console.log(`value: ${value.trim()} | color: ${color.trim()}`); // Log value and color

          const cellColor = getColor(color.trim()); // Get the RGB color
          worksheet[valueCell].s = {
            fill: { fgColor: { rgb: cellColor } }, // Apply background color
          };
        } else {
          worksheet[valueCell] = { v: cellValue }; // Set the cell value if no color
        }
      });
    });

    // for (let row = 0; row < newListData?.length; row++) {}
    // Calculate column widths based on  maximum header and data content length
    worksheet["!cols"] = calculateColumnWidths(
      worksheet,
      numRows,
      numCols,
      newListData?.length
    );

    filter &&
      (worksheet["!autofilter"] = {
        ref: XLSX.utils.encode_range({
          s: { r: 0, c: 0 },
          e: { r: 0, c: Object.keys(newListData[0]).length - 1 },
        }),
      });
    return XLSX.utils.book_append_sheet(workbook, worksheet, sheetName);
  });
  XLSX.writeFile(workbook, fileName);

  // const exportExcel = async () => {
  //   try {
  //     const workbook = new ExcelJS.Workbook();
  //     dataList?.map(async (list) => {
  //       const {
  //         listData,
  //         sheetName,
  //         options: { filter },
  //       } = list;
  //       const headerStyle = {
  //         fill: {
  //           type: "pattern",
  //           pattern: "solid",
  //           fgColor: { argb: "FFFF00" },
  //         },
  //         font: {
  //           bold: true,
  //         },
  //       };
  //       const sheet = workbook.addWorksheet(sheetName);
  //       const header = columName.map((col) => col?.propertyName);
  //       const newHeader = header.map(
  //         (header) => LabelsDictionary[header]?.["Full"] || header
  //       );

  //       const headerRow = sheet.addRow(newHeader);
  //       headerRow.eachCell({ includeEmpty: true }, (cell) => {
  //         Object.assign(cell, headerStyle);
  //       });
  //       listData.forEach((item) => {
  //         const rowData = header.map((header) => item[header] || "");
  //         sheet.addRow(rowData);
  //       });
  //       // Calculate and set column widths based on content length
  //       const maxContentLength = header.map((header, columnIndex) => {
  //         const headerLength = header.length;
  //         const contentLengths = sheet
  //           .getColumn(columnIndex + 1)
  //           .values?.slice(1)
  //           .map((value) => (value ? value.toString().length : 0));
  //         return Math.max(headerLength, ...contentLengths);
  //       });
  //       maxContentLength.forEach((maxContentLength, colmunIndex) => {
  //         sheet.getColumn(colmunIndex + 1).width = maxContentLength + 2;
  //       });
  //       //Apply Borders to all cells
  //       const listIndex = [1, 4, 6, 8];
  //       sheet?.eachRow((row, rowIndex) => {
  //         row.eachCell((cell, colmunIndex) => {
  //           listIndex.map((index) => {
  //             if (colmunIndex === index) {
  //               cell.fill = {
  //                 type: "pattern",
  //                 pattern: "solid",
  //                 fgColor: { argb: "808080" },
  //               };
  //             }
  //           });

  //           cell.border = {
  //             top: { style: "thin", color: { argb: "FF000000" } },
  //             bottom: { style: "thin", color: { argb: "FF000000" } },
  //             left: { style: "thin", color: { argb: "FF000000" } },
  //             right: { style: "thin", color: { argb: "FF000000" } },
  //           };
  //         });
  //       });
  //     });
  //     const blob = await workbook.xlsx.writeBuffer();
  //     const blobOnject = new Blob([blob], {
  //       type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
  //     });
  //     const objectUrl = URL.createObjectURL(blobOnject);
  //     const link = document.createElement("a");
  //     link.href = objectUrl;
  //     link.download = fileName;
  //     link.click();
  //   } catch (error) {
  //     console.error("Error exporting Excel: ", error);
  //   }
  // };

  // exportExcel();
};

export const getExcelImportStaus = async (
  file: File,
  sheetName: string,
  apiPath: string,
  mode?: Array<number>,
  verticalId?: Array<number>
) => {
  // const searchSheetName = sheetName;
  setLoader("ADD", "GetImportStatus");
  try {
    // const arrayBuffer = await file.arrayBuffer();
    // const workbook = XLSX.read(arrayBuffer, { type: "array" });
    // const isSheetPresent = workbook.SheetNames?.some((sheet) =>
    //   sheet.includes(searchSheetName)
    // );
    // if (isSheetPresent) {
    //   console.log(`${searchSheetName} sheet name is present in the Excel File`);
    // const firstSheetName = workbook.SheetNames[0];
    // const worksheet = workbook.Sheets[firstSheetName];
    // const jsonData = XLSX.utils.sheet_to_json(worksheet);
    // console.log(JSON.stringify(jsonData, null, 2));
    if (apiPath === "lcmEnginnering") {
      let result = await GetLcmEngineeringImportStatus(file);
      setLoader("REMOVE", "GetImportStatus");
      return result;
    } else if (apiPath === "networkElementAsIs") {
      let result = await GetNetworkElementAsIsImportStatus(file);
      setLoader("REMOVE", "GetImportStatus");
      return result;
    } else if (apiPath === "deliveryTracking") {
      let result = await GetDeliveryTrackingImportStatus(file);
      setLoader("REMOVE", "GetImportStatus");
      return result;
    } else if (apiPath === "nfvisoftwarecompatibility") {
      let result = await importNFVISoftwareCompatibleExcel(file);
      setLoader("REMOVE", "NFVISwImportStatus");
      return result;
    } else if (apiPath === "IdentityAsIs") {
      let result = await GetIdentityAsIsImportStatus(file);
      setLoader("REMOVE", "GetImportStatus");
      return result;
    } else if (apiPath === "VBOMInfo") {
      let result = await ImportVBOMInfoExcel(file);
      setLoader("REMOVE", "GetImportStatus");
      return result;
    } else if (apiPath === "CBOMInfo") {
      let result = await ImportCBOMExcel(file);
      setLoader("REMOVE", "GetImportStatus");
      return result;
    } else if (apiPath === "BPTReport") {
      let result = await ImportBPTReport(file);
      setLoader("REMOVE", "GetImportStatus");
      return result;
    } else if (
      apiPath === "TSRReport" &&
      mode != undefined &&
      verticalId != undefined
    ) {
      let result = await GetTSRReportImportStatus(file, mode, verticalId);
      setLoader("REMOVE", "GetImportStatus");
      return result;
    } else if (
      apiPath === "FNTReport" &&
      mode != undefined &&
      verticalId != undefined
    ) {
      let result = await GetTemsFNTReportImportStatus(file, mode, verticalId);
      setLoader("REMOVE", "GetImportStatus");
      return result;
    } else if (apiPath === "PassThroughReport") {
      let result = await GetPassThroughReportImportStatus(
        file,
        verticalId ?? []
      );
      setLoader("REMOVE", "GetImportStatus");
      return result;
    } else if (apiPath === "PassThroughHardwareReport") {
      let result = await GetPassThroughHardwareReportImportStatus(
        file,
        verticalId ?? []
      );
      setLoader("REMOVE", "GetImportStatus");
      return result;
    } else if (apiPath === "PassThroughSoftwareReport") {
      let result = await GetPassThroughSoftwareReportImportStatus(
        file,
        verticalId ?? []
      );
      setLoader("REMOVE", "GetImportStatus");
      return result;
    } // } else {
    //   rootStore.dispatch(
    //     setNotification({
    //       message: `Failed to import the Excel File,\n Update the sheet name as "${searchSheetName}" and re-import the file back`,
    //       notifyType: NotifyType.error,
    //     })
    //   );

    //   return false;
    // }
    setLoader("REMOVE", "GetImportStatus");
    return undefined;
  } catch (error) {
    console.log("An error occurred during upload and conversion", error);
    setLoader("REMOVE", "GetImportStatus");

    return false;
  }
};

export const handleImportFile = (
  sheetName: string,
  apiPath: string,
  mode?: Array<number>,
  verticalId?: Array<number>
) => {
  return new Promise<Object>(async (resolve, reject) => {
    const handleFileChange = async (event: Event) => {
      try {
        const seletedFile = (event.target as HTMLInputElement).files?.[0];
        if (seletedFile) {
          const formData = new FormData();
          formData.append("file", seletedFile);
        }
        if (!seletedFile) {
          return resolve({
            warning: false,
            info: "No file selected.",
          });
        } else {
          // const validFormats = apiPath === "BPTReport" ? [".xls"] : [".xlsx"];
          const validFormats =
            apiPath === "BPTReport"
              ? [".xls"]
              : apiPath === "PassThroughReport"
              ? [".xls", ".xlsx"]
              : [".xlsx"];
          const fileFormat = seletedFile.name
            .slice(seletedFile.name.lastIndexOf("."))
            .toLowerCase();

          if (!validFormats.includes(fileFormat)) {
            resolve({
              warning: false,
              info: "Invalid fle format. Only .xlsx files are allowed to import.",
            });
          } else {
            const status = await getExcelImportStaus(
              seletedFile,
              sheetName,
              apiPath,
              mode,
              verticalId
            );
            if (status) resolve(status);
          }
        }
      } catch (error) {
        reject(error);
      }
    };

    const fileInput = document.createElement("input");
    fileInput.type = "file";
    fileInput.accept = apiPath === "BPTReport" ? ".xls" : ".xlsx";
    fileInput.addEventListener("change", handleFileChange);
    await new Promise<void>((resolve) => setTimeout(resolve, 0));
    fileInput.click();
  });
};
