import React, { useState, useEffect, useRef } from "react";
import { createRoot } from "react-dom/client";
import "./chartStyle.css";
import Grid from "@mui/material/Grid";
import { Row, Col, Dropdown, Modal, Form } from "react-bootstrap";
import Radio from "@mui/material/Radio";
import RadioGroup from "@mui/material/RadioGroup";
import FormControlLabel from "@mui/material/FormControlLabel";
import FormControl from "@mui/material/FormControl";
import FormLabel from "@mui/material/FormLabel";
import { GoArrowLeft } from "react-icons/go";
import {
  GetNetworkElementGraphAllOpco,
  GetNetworkElementGraph,
  GetAssetOverviewByMarketReport,
} from "../../Redux/Action/LookUp/NetworkElementGraph/NetworkElementGraphAction";
import { FaPlus, FaMinus } from "react-icons/fa6";
import { jsPDF } from "jspdf";
import html2canvas from "html2canvas";
import setLoader from "../../Redux/Action/LoaderAction";
import FormGroup from "@mui/material/FormGroup";
import Checkbox from "@mui/material/Checkbox";
import PptxGenJS from "pptxgenjs";
import { useAuth } from "../../Hook/useAuth";
import { useTheme } from "../../Context/ThemeContext";
import {
  DropdownInputComponent,
  MultiReactSelect,
  MultiSelectComponent,
} from "../FormField";
import { AssestOverviewByMarketQueryObjectGrid } from "../../Model/NetworkElementAsPlanned";
import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Alert,
  Box,
  Button,
  Chip,
  Divider,
  Drawer,
  IconButton,
  InputAdornment,
  InputLabel,
  ListItem,
  ListItemText,
  Menu,
  MenuItem,
  OutlinedInput,
  Paper,
  Select,
  Stack,
  Typography,
  Card,
  CardHeader,
  CardContent,
  Avatar,
} from "@mui/material";
import { styled } from "@mui/material/styles";
import { IoFilter } from "react-icons/io5";
import { FiChevronDown } from "react-icons/fi";
import { IoMdDownload } from "react-icons/io";
import { BsArrowsCollapse, BsArrowsExpand } from "react-icons/bs";
import { BiSolidFilterAlt } from "react-icons/bi";
import { createTheme, ThemeProvider } from "@mui/material/styles";
import { IoClose } from "react-icons/io5";
import { safeNumber } from "../../Hook/Common";
import { MultiSelect } from "react-multi-select-component";
import { useSearchParams } from "react-router-dom";
import { IoMdArrowUp } from "react-icons/io";
import { MultiSelectCheckmarks, MultiSingleSelect } from "../MUISelect";
import { FcDownload, FcInfo } from "react-icons/fc";
import Tooltip, { TooltipProps, tooltipClasses } from "@mui/material/Tooltip";
import CustomMenuButton from "../CustomMenuButton";
import { BiExpandAlt, BiCollapseAlt } from "react-icons/bi";

const networkElementGraphQuery = {
  // sortBy: "string",
  // isSortAscending: true,
  // page: 0,
  // pageSize: 0,
  // lastModified: {
  //   startDate: "2024-08-14T11:37:15.325Z",
  //   endDate: "2024-08-14T11:37:15.325Z",
  // },
  // principalId: 0,
  // deleted: true,
  // orphan: true,
  lastModifiedBy: [],
  verticalId: [],
  verticalDescrption: [],
  oemVendorId: [],
  oemVendor: [],
  productNameNeInstances: [],
  networkElementsAsPlannedId: [],
  networkElementsPlannedName: [],
};

let paginationQuery: AssestOverviewByMarketQueryObjectGrid = {
  subDomainResponseCeFunctionId: [],
  subDomainResponseCeFunction: [],
  oemVendorId: [],
  oemVendor: [],
  productNameNeInstances: [],
  networkElementsAsPlannedId: [],
  networkElementsPlannedName: [],
  opcoId: [],
  systemTypetId: [],
  opCoDescrption: [],
  verticalId: [],
  verticalDescrption: [],
  networkElementCount: [],
  dcfId: [],
  hwBuild: [],
  supportService: [],
  environmentId: [],
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  lastModified: undefined,
  principalId: undefined,
  deleted: undefined,
  orphan: undefined,
  lastModifiedBy: [],
};

const lightTheme = createTheme({
  palette: {
    mode: "light",
    primary: {
      main: "#1976d2",
    },
    text: {
      primary: "#000000", // Black text
      secondary: "#555555", // Slightly lighter black for secondary text
    },
    background: {
      default: "#ffffff",
    },
  },
});

const darkTheme = createTheme({
  palette: {
    mode: "dark",
    primary: {
      main: "#90caf9",
    },
    text: {
      primary: "#ffffff", // Black text
      secondary: "#555555", // Slightly lighter black for secondary text
    },
    background: {
      default: "#121212",
    },
  },
});

const Item = styled(Paper)(({ theme }) => ({
  ...theme.typography.body2,
  textAlign: "center",
  color: theme.palette.text.secondary,
  height: 60,
  lineHeight: "60px",
}));

const HtmlTooltip = styled(({ className, ...props }: TooltipProps) => (
  <Tooltip {...props} classes={{ popper: className }} placement="right-start" />
))(({ theme }) => ({
  [`& .${tooltipClasses.tooltip}`]: {
    backgroundColor: "#2c2a2ae8",
    color: "rgba(0, 0, 0, 0.87)",
    minWidth: 220,
    fontSize: theme.typography.pxToRem(12),
    border: "1px solid #dadde9",
  },
}));

const Chart2 = () => {
  const [searchParams] = useSearchParams();
  const [networkElementGraphData, setNetworkElementGraphData] =
    useState<any>(null);
  const [networkEleData, setNetworkEleData] = useState<any>(null);
  const [networkElementOpco, setNetworkElementOpco] = useState<any>();
  const [selectedOpCo, setSelectedOpCo] = useState<any[]>([]);
  const [selectedDcf, setSelectedDcf] = useState<any[]>([]);
  const [selectedVertical, setSelectedVertical] = useState<any>(null);
  const [selectedEnvironment, setSelectedEnvironment] = useState<any>(null);
  const [selectedSystemType, setSelectedSystemType] = useState<any>(null);
  const [selectedSupport, setSelectedSupport] = useState<any>(null);
  const [selectedVendor, setSelectedVendor] = useState<any>(null);
  const [selectedBuildCons, setSelectedBuildCons] = useState<any>(null);
  const [isQueryParams, setIsQueryParams] = useState(false);
  const [showCanvas, setShowCanvas] = useState(false);
  const [menuIsOpen, setMenuIsOpen] = useState(false);
  const [isNoData, setIsNoData] = useState(false);
  const [isOpen, setIsOpen] = useState(false);
  const { darkMode, selectDarkMode } = useTheme();
  const theme = darkMode ? darkTheme : lightTheme;
  const ITEM_HEIGHT = 48;
  const ITEM_PADDING_TOP = 8;
  const { isPermesso, pageSize } = useAuth();
  const [formattedGraphData, setFormattedGraphData] = useState<any>();
  const [filterHeading, setFilterHeading] = useState("");
  const [disableField, setDisableField] = useState<any>([]);
  const [excelPopup, setExcelPopUp] = useState<Boolean>(false);
  const [downloadType, setDownloadType] = useState<string>("pdf");
  const [individualView, setIndividualView] = useState<any>("");
  const [selectedIndividualData, setSelectedIndividualData] = useState<any>({});
  const topRef = useRef<HTMLDivElement>(null);
  const [collapseMenuTriggered, setCollapseMenuTriggered] = useState(false);
  const [hideBtns, setHideBtns] = useState<boolean>(false);
  const [selectedTab, setSelectedTab] = useState<any>([]);

  useEffect(() => {
    selectDarkMode(true);

    return () => {
      selectDarkMode(false);
    };
  }, []);
  const getNetworkElementGraphAllOpco = async () => {
    const opco = await GetNetworkElementGraphAllOpco();
    if (opco.ResultDtoCreate) {
      const formattedOpcoEntries = Object.entries(opco.ResultDtoCreate);
      formattedOpcoEntries.sort((a, b) => a[1].localeCompare(b[1]));
      setNetworkElementOpco(formattedOpcoEntries);
      setSelectedValues(formattedOpcoEntries.map(([key]) => key));
    }
  };
  const getNetworkElementGraph = async (
    pageQuery?: any,
    isFromField?: boolean,
    field?: string,
    fieldValue?: any
  ) => {
    const isField = isFromField === undefined ? false : isFromField;
    const graphData = await GetNetworkElementGraph(
      pageQuery
        ? pageQuery
        : {
            ...paginationQuery,
            opcoId: field == "opco" ? fieldValue : selectedOpCo,
            dcfId: field == "dcf" ? fieldValue : selectedDcf,
            hwBuild:
              field == "build"
                ? [fieldValue]
                : selectedBuildCons !== null && selectedBuildCons !== 0
                ? [selectedBuildCons]
                : [],
            environmentId:
              field == "env"
                ? [fieldValue]
                : selectedEnvironment !== null && selectedEnvironment !== 0
                ? [selectedEnvironment]
                : [],
            systemTypetId:
              field == "systemType"
                ? [fieldValue]
                : selectedSystemType !== null && selectedSystemType !== 0
                ? [selectedSystemType]
                : [],
            supportService:
              field == "support"
                ? [fieldValue]
                : selectedSupport !== null && selectedSupport !== 0
                ? [selectedSupport]
                : [],
            oemVendorId:
              field == "vendor"
                ? [fieldValue]
                : selectedVendor !== null && selectedVendor !== 0
                ? [selectedVendor]
                : [],
            verticalId:
              field == "vertical"
                ? [fieldValue]
                : selectedVertical !== null && selectedVertical !== 0
                ? [selectedVertical]
                : [],
          }
    );
    if (
      graphData?.item !== null &&
      graphData?.item !== undefined &&
      graphData?.item?.length > 0
    ) {
      setIsNoData(false);
    } else setIsNoData(true);
    if (isField && graphData?.allResource) {
      if (field === "opco") {
        const { allOpcos, ...res } = graphData?.allResource?.data;
        setNetworkElementGraphData({
          ...networkElementGraphData,
          allResource: {
            data: {
              allOpcos: networkElementGraphData?.allResource?.data.allOpcos,
              ...res,
            },
          },
        });
      } else if (field === "dcf") {
        const { allDcf, ...res } = graphData?.allResource?.data;
        setNetworkElementGraphData({
          ...networkElementGraphData,
          allResource: {
            data: {
              allDcf: networkElementGraphData?.allResource?.data.allDcf,
              ...res,
            },
          },
        });
      } else if (field === "vertical") {
        const { allVertical, ...res } = graphData?.allResource?.data;
        setNetworkElementGraphData({
          ...networkElementGraphData,
          allResource: {
            data: {
              allVertical:
                networkElementGraphData?.allResource?.data.allVertical,
              ...res,
            },
          },
        });
      } else if (field === "support") {
        const { allSupport, ...res } = graphData?.allResource?.data;
        setNetworkElementGraphData({
          ...networkElementGraphData,
          allResource: {
            data: {
              allSupport: networkElementGraphData?.allResource?.data.allSupport,
              ...res,
            },
          },
        });
      } else if (field === "env") {
        const { allEnvironment, ...res } = graphData?.allResource?.data;
        setNetworkElementGraphData({
          ...networkElementGraphData,
          allResource: {
            data: {
              allEnvironment:
                networkElementGraphData?.allResource?.data.allEnvironment,
              ...res,
            },
          },
        });
      } else if (field === "systemType") {
        const { allSystemType, ...res } = graphData?.allResource?.data;
        setNetworkElementGraphData({
          ...networkElementGraphData,
          allResource: {
            data: {
              allSystemType:
                networkElementGraphData?.allResource?.data.allSystemType,
              ...res,
            },
          },
        });
      } else if (field === "build") {
        const { allBuildCons, ...res } = graphData?.allResource?.data;
        setNetworkElementGraphData({
          ...networkElementGraphData,
          allResource: {
            data: {
              allBuildCons:
                networkElementGraphData?.allResource?.data.allBuildCons,
              ...res,
            },
          },
        });
      } else if (field === "vendor") {
        const { allVendor, ...res } = graphData?.allResource?.data;
        setNetworkElementGraphData({
          ...networkElementGraphData,
          allResource: {
            data: {
              allVendor: networkElementGraphData?.allResource?.data.allVendor,
              ...res,
            },
          },
        });
      } else {
        setNetworkElementGraphData({
          ...networkElementGraphData,
          allResource: graphData?.allResource,
        });
      }
    } else {
      setNetworkElementGraphData(graphData);
    }
  };
  function groupByFieldMatch(data) {
    return data.reduce((acc, item) => {
      const primaryKey = String(item["verticalDescrption"]);
      const secondaryKey = String(item["oemVendor"]);
      const tertiaryKey = String(item["productNameNeInstances"]);
      const fourthKey = String(item["opCoDescrption"]);

      if (!acc[primaryKey]) {
        acc[primaryKey] = {};
      }

      if (!acc[primaryKey][secondaryKey]) {
        acc[primaryKey][secondaryKey] = {};
      }

      if (!acc[primaryKey][secondaryKey][tertiaryKey]) {
        acc[primaryKey][secondaryKey][tertiaryKey] = {};
      }

      if (!acc[primaryKey][secondaryKey][tertiaryKey][fourthKey]) {
        acc[primaryKey][secondaryKey][tertiaryKey][fourthKey] = [];
      }

      acc[primaryKey][secondaryKey][tertiaryKey][fourthKey].push(
        item?.networkElementCount
      );

      return acc;
    }, {});
  }
  function transformData(data: any, depth: number): any {
    if (depth > 1 && typeof data === "object" && data !== null) {
      return Object.entries(data).map(([key, value]) => ({
        title: key,
        data:
          typeof value === "object" && value !== null && !Array.isArray(value)
            ? transformData(value, depth - 1)
            : Array.isArray(value)
            ? value.map((v) =>
                typeof v === "object"
                  ? transformData(v, depth - 1)
                  : { title: String(v), data: [] }
              )
            : [value],
      }));
    } else if (depth === 1 && typeof data === "object" && data !== null) {
      // 4th level: treat values as final objects and map them directly
      return Object.entries(data).map(([key, value]) => ({
        title: key,
        data: value,
      }));
    } else {
      return [data];
    }
  }

  useEffect(() => {
    if (networkElementGraphData?.item?.length) {
      const grouped = groupByFieldMatch(networkElementGraphData.item);
      const transformed = transformData(grouped, 4); // Assuming you want 4 levels deep
      setNetworkEleData(transformed);
    }
  }, [networkElementGraphData]);

  // console.log("networkEleData", networkEleData);

  // console.log(transformedResult);

  // useEffect(() => {
  //   if (
  //     isPermesso &&
  //     (selectedOpCo?.length > 0 ||
  //       selectedDcf?.length > 0 ||
  //       selectedBuildCons !== null ||
  //       selectedEnvironment !== null ||
  //       selectedSupport !== null ||
  //       selectedVendor !== null ||
  //       selectedVertical !== null)
  //   ) {
  //     paginationQuery.opcoId = selectedOpCo?.length > 0 ? selectedOpCo : [];
  //     paginationQuery.dcfId = selectedDcf?.length > 0 ? selectedDcf : [];
  //     paginationQuery.hwBuild =
  //       selectedBuildCons !== null ? [selectedBuildCons] : [];
  //     paginationQuery.environmentId =
  //       selectedEnvironment !== null ? [selectedEnvironment] : [];
  //     paginationQuery.supportService =
  //       selectedSupport !== null ? [selectedSupport] : [];
  //     paginationQuery.oemVendorId =
  //       selectedVendor !== null ? [selectedVendor] : [];
  //     paginationQuery.verticalId =
  //       selectedVertical !== null ? [selectedVertical] : [];
  //     getFilterHeading();
  //     getNetworkElementGraph(paginationQuery);
  //   }
  // }, [
  //   selectedOpCo,
  //   selectedDcf,
  //   selectedBuildCons,
  //   selectedEnvironment,
  //   selectedSupport,
  //   selectedVendor,
  //   selectedVertical,
  // ]);

  useEffect(() => {
    if (!networkElementGraphData?.item?.length && isPermesso) {
      setIsQueryParams(
        searchParams && searchParams["size"] !== 0 ? true : false
      );
      if (searchParams && searchParams["size"] !== 0) {
        const paramsObj = Object.fromEntries(searchParams?.entries());
        const transformedObj = Object.keys(paramsObj).reduce((acc, key) => {
          const value = paramsObj[key];
          if (key === "selectedDate") {
            acc[key] = String(value);
          } else {
            acc[key] = value
              ?.split(",")
              .map((val) => (val !== "" ? safeNumber(val) : 0));
          }

          return acc;
        }, {});
        if (transformedObj["opcoId"]) {
          setSelectedOpCo(transformedObj["opcoId"]);
        }
        if (transformedObj["dcfId"]) {
          setSelectedDcf(transformedObj["dcfId"]);
        }
        if (transformedObj["buildConsId"]) {
          setSelectedBuildCons(transformedObj["buildConsId"]);
        }
        if (transformedObj["environmentId"]) {
          setSelectedEnvironment(transformedObj["environmentId"]);
        }
        if (transformedObj["supportServiceId"]) {
          setSelectedSupport(transformedObj["supportServiceId"]);
        }
        if (transformedObj["vendorId"]) {
          setSelectedVendor(transformedObj["vendorId"]);
        }
        if (transformedObj["verticalId"]) {
          setSelectedVertical(transformedObj["verticalId"]);
        }
        if (transformedObj["systemTypeId"]) {
          setSelectedSystemType(transformedObj["systemTypeId"]);
        }
        console.log(transformedObj);
        getNetworkElementGraph({
          ...paginationQuery,
          opcoId: transformedObj["opcoId"] ?? [],
          dcfId: transformedObj["dcfId"] ?? [],
          hwBuild: transformedObj["buildConsId"] ?? [],
          environmentId: transformedObj["environmentId"] ?? [],
          supportService: transformedObj["supportServiceId"] ?? [],
          oemVendorId: transformedObj["vendorId"] ?? [],
          verticalId: transformedObj["verticalId"] ?? [],
          systemTypetId: transformedObj["systemTypeId"] ?? [],
        } as AssestOverviewByMarketQueryObjectGrid);
      } else {
        getNetworkElementGraph();
      }
    }
    // if (!networkElementOpco?.length && isPermesso) {
    //   getNetworkElementGraphAllOpco();
    // }
  }, [isPermesso]);

  const groupAndAggregateData = (data) => {
    const result = {};

    // Iterate over each item in the data array
    data?.forEach((item) => {
      const {
        verticalDescrption,
        oemVendor,
        productNameNeInstances,
        opcoId,
        opCoDescrption,
        networkElementCount,
      } = item;

      // Initialize the verticalDescrption group if it doesn't exist
      if (!result[verticalDescrption]) {
        result[verticalDescrption] = {};
      }

      // Initialize the oemVendor group if it doesn't exist
      if (!result[verticalDescrption][oemVendor]) {
        result[verticalDescrption][oemVendor] = {};
      }

      // Initialize the productNameNeInstances group if it doesn't exist
      if (!result[verticalDescrption][oemVendor][productNameNeInstances]) {
        result[verticalDescrption][oemVendor][productNameNeInstances] = {
          data: [],
          total: 0,
        };
      }

      // Add the data and update the total count
      result[verticalDescrption][oemVendor][productNameNeInstances].data.push({
        opcoId,
        opCoDescrption,
        networkElementCount,
      });

      result[verticalDescrption][oemVendor][productNameNeInstances].total +=
        networkElementCount;
    });

    // Convert the result object into the desired array format
    return Object.keys(result).map((verticalDescrption) => {
      return {
        verticalDescrption,
        oemVendors: Object.keys(result[verticalDescrption]).map((oemVendor) => {
          return {
            oemVendor,
            data: Object.keys(result[verticalDescrption][oemVendor]).map(
              (productNameNeInstances) => {
                const { data, total } =
                  result[verticalDescrption][oemVendor][productNameNeInstances];
                return {
                  productNameNeInstances,
                  data,
                  total,
                };
              }
            ),
          };
        }),
      };
    });
  };

  const filterData = (data, filterCriteria) => {
    return data.filter((item) => {
      return Object.keys(filterCriteria).some((key) => {
        if (key === "opcoId") {
          return filterCriteria[key].includes(item[key]);
        }
        if (key === "opCoDescrption") {
          return filterCriteria[key].includes(item[key]);
        }
        return false;
      });
    });
  };

  useEffect(() => {
    if (networkElementGraphData?.item) {
      // Group and aggregate the data
      const groupedData = groupAndAggregateData(networkElementGraphData?.item);
      setFormattedGraphData(
        groupedData.sort((a, b) => {
          // Compare the 'verticalDescrption' properties of the objects
          if (a.verticalDescrption < b.verticalDescrption) return -1;
          if (a.verticalDescrption > b.verticalDescrption) return 1;
          return 0;
        })
      );

      setInitialData(
        groupedData.sort((a, b) => {
          // Compare the 'verticalDescrption' properties of the objects
          if (a.verticalDescrption < b.verticalDescrption) return -1;
          if (a.verticalDescrption > b.verticalDescrption) return 1;
          return 0;
        })
      );
      isQueryParams && getFilterHeading();
    }
  }, [networkElementGraphData]);

  const collapseMenu = () => {
    if (!collapseMenuTriggered) {
      setDisableField(formattedGraphData?.map((fgd) => fgd.verticalDescrption));
      setCollapseMenuTriggered(true);
    } else {
      setDisableField([]);
      setCollapseMenuTriggered(false);
    }
  };

  const createPDF = async () => {
    const pdf = new jsPDF("portrait", "pt", "a4");
    const canvas = await html2canvas(
      document.querySelector("#pdf") as HTMLElement,
      { scale: 2 }
    );
    const img = canvas.toDataURL("image/png");

    const pdfWidth = pdf.internal.pageSize.getWidth();
    const pdfHeight = pdf.internal.pageSize.getHeight();

    const imgProperties = pdf.getImageProperties(img);
    const imgWidth = imgProperties.width;
    const imgHeight = imgProperties.height;

    // Calculate scaling ratio to fit the width of the page
    const widthRatio = pdfWidth / imgWidth;
    const scaledImgWidth = pdfWidth;
    const scaledImgHeight = imgHeight * widthRatio;

    // Calculate the total height required
    const totalHeight = scaledImgHeight;

    // If the total height is greater than the PDF page height, adjust the height of the PDF
    pdf.internal.pageSize.height = totalHeight;

    // Add the image to the PDF
    pdf.addImage(img, "PNG", 0, 0, scaledImgWidth, scaledImgHeight);

    // Save the PDF
    pdf.save("network_overview.pdf");
    setLoader("REMOVE", "");
    setHideBtns(false);
  };

  const createPPT = async (components) => {
    const pptx = new PptxGenJS();
    for (const component of components) {
      // Create a temporary container for rendering the component
      const container = document.createElement("div");
      container.style.position = "absolute";
      container.style.top = "-9999px";
      container.style.left = "-9999px";
      document.body.appendChild(container);

      // Render the component into the container using createRoot
      const root = createRoot(container);
      root.render(component);

      // Wait for the component to render fully
      await new Promise((resolve) => setTimeout(resolve, 100)); // Small delay to ensure rendering

      // Set target height to match slide height in pixels (720px for 5.63 inches at 96 DPI)
      const targetHeight = 720;
      const scaleFactor = targetHeight / container.clientHeight; // Scale to match slide height
      const targetWidth = 1200; // Calculate proportional width

      // Capture the component as a canvas with the calculated dimensions
      const canvas = await html2canvas(container, { scale: 2 });
      const imgData = canvas.toDataURL("image/png");

      // Clean up
      root.unmount();
      document.body.removeChild(container);
      // // Set slide dimensions for PptxGenJS
      const slideWidth = 10; // 10 inches wide (default width for PptxGenJS)
      const slideHeight = 5.63; // 5.63 inches high (16:9 aspect ratio)

      // // Calculate aspect ratio and dimensions to fit the image within the slide
      const imgProperties = canvas;
      const imgWidth = imgProperties.width;
      const imgHeight = imgProperties.height;

      const widthRatio = slideWidth / imgWidth;
      const scaledImgWidth = slideWidth;
      const scaledImgHeight = imgHeight * widthRatio;
      // Add the image to a new slide, aligning as needed
      const slide = pptx.addSlide();
      slide.addImage({
        data: imgData,
        x: 0, // Center horizontally
        y: 0, // Align to top
        w: scaledImgWidth,
        h: scaledImgHeight,
      });
    }

    // Save the PowerPoint file
    pptx.writeFile({ fileName: "networkoverview.pptx" });
    setLoader("REMOVE", "");
  };

  // Main Category Row Component
  const MainCategoryRow = ({ currentVendor, indexValue }) => {
    return (
      <>
        <div
          style={{
            maxWidth: "1260px",
          }}
        >
          <div
            style={{
              width: "1260px",
              padding: "8px",
            }}
          >
            {indexValue === 0 && (
              <>
                <div
                  style={{
                    display: "flex",
                    justifyContent: "space-between",
                  }}
                >
                  <div className="headerPage row mx-0">
                    <h3
                      className="voda-bold fz-28"
                      style={{
                        textAlign: "left",
                        color: "black",
                        display: "flex",
                      }}
                    >
                      Asset Overview by Market
                    </h3>
                  </div>
                  <div className="d-flex">
                    <section className="legend-container">
                      <div className="legend-style legend-blue"></div>
                      <div className="legend-blue-title pl-2">
                        Network Functions
                      </div>
                    </section>
                    <section className="legend-container">
                      <div className="legend-style legend-white"></div>
                      <div className="legend-white-title pl-2">Vendor</div>
                    </section>
                    <section className="legend-container">
                      <div className="legend-style legend-orange"></div>
                      <div className="legend-orange-title pl-2">
                        NE - instances
                      </div>
                    </section>
                  </div>
                </div>
                {isQueryParams && (
                  <div>
                    <Typography
                      component="span"
                      sx={{ fontWeight: "bolder !important" }}
                    >
                      Applied Filters
                    </Typography>
                    {displayHeading?.map(({ name, list }) => (
                      <div className="d-flex mr-2">
                        <FormLabel
                          component="legend"
                          sx={{
                            width: "auto !important",
                            display: "block",
                            fontWeight: "bold !important",
                            marginBottom: "0px !important",
                            fontSize: "16px !important",
                            marginTop: "12px",
                          }}
                        >
                          {name}
                        </FormLabel>
                        <div>
                          {list?.map((val) => {
                            return (
                              <Chip
                                sx={{ width: "max-content" }}
                                variant="outlined"
                                label={
                                  <Typography
                                    component="span"
                                    sx={{
                                      display: "block",
                                      whiteSpace: "normal",
                                      wordBreak: "break-word",
                                    }}
                                    dangerouslySetInnerHTML={{ __html: val }}
                                  />
                                }
                              />
                            );
                          })}
                        </div>
                      </div>
                    ))}
                  </div>
                )}
                {filterHeading && (
                  <div className="headerPage row mx-0 px-0 my-3">
                    <div
                      className="voda-bold fz-28 ml-0"
                      style={{
                        textAlign: "left",
                        marginLeft: "1rem",
                        color: "black",
                      }}
                    >
                      {"Filters -  "}
                      <span
                        dangerouslySetInnerHTML={{
                          __html: filterHeading,
                        }}
                      />
                    </div>
                  </div>
                )}
              </>
            )}
            <div className="invidual-card-container d-flex mt-3">
              <div className="row">
                <div className="col-12">
                  <div className="main-title">
                    {currentVendor?.["verticalDescrption"]}
                  </div>
                  <div className="main-card-individual">
                    <div className="row">
                      {currentVendor?.oemVendors?.map((vendor) => {
                        const allTotalsZero = vendor.data.every(
                          (item) => item.total === 0
                        );
                        return (
                          <>
                            {!allTotalsZero && (
                              <div
                                className="col m-2"
                                style={{ flexGrow: 0 }}
                                key={vendor.oemVendors}
                              >
                                <h3 className="main-category-title">
                                  {vendor.oemVendor}
                                </h3>
                                <Row
                                  className="main-category-sub"
                                  role="submenu"
                                  style={{
                                    width: "max-content",
                                    maxWidth: "800px",
                                  }}
                                >
                                  {vendor.data.map((product) => (
                                    <Col key={product.id}>
                                      <div
                                        className="main-category-sub-card-single"
                                        style={{
                                          maxWidth: "max-content",
                                          minWidth: "1200px",
                                        }}
                                      >
                                        <div>
                                          <div className="sub-card-title">
                                            {product.productNameNeInstances}
                                          </div>
                                          <div
                                            className={`sub-card-value ${
                                              product.total
                                                ? "sub-card-hasValue"
                                                : ""
                                            }`}
                                          >
                                            {product.total}
                                          </div>
                                        </div>
                                      </div>
                                    </Col>
                                  ))}
                                </Row>
                              </div>
                            )}
                          </>
                        );
                      })}
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </>
    );
  };

  // const createPPT = async () => {

  //   console.log("formattedGraphData", formattedGraphData);

  // // Initialize PowerPoint
  // const pptx = new PptxGenJS();
  // const slide = pptx.addSlide();

  // // Capture the image of the #pdf element (the ID can be changed to your component's container ID)
  // const canvas = await html2canvas(
  //   document.querySelector("#pdf") as HTMLElement,
  //   { scale: 2 }
  // );
  // const img = canvas.toDataURL("image/png");

  // // Set slide dimensions for PptxGenJS
  // const slideWidth = 10; // 10 inches wide (default width for PptxGenJS)
  // const slideHeight = 5.63; // 5.63 inches high (16:9 aspect ratio)

  // // Calculate aspect ratio and dimensions to fit the image within the slide
  // const imgProperties = canvas;
  // const imgWidth = imgProperties.width;
  // const imgHeight = imgProperties.height;

  // const widthRatio = slideWidth / imgWidth;
  // const scaledImgWidth = slideWidth;
  // const scaledImgHeight = imgHeight * widthRatio;

  // // Add the captured image to the PowerPoint slide
  // slide.addImage({
  //   data: img,
  //   x: 0.5, // starting X position (you can adjust this for padding)
  //   y: 0.5, // starting Y position (you can adjust this for padding)
  //   w: scaledImgWidth,
  //   h: scaledImgHeight,
  // });

  // // Save the PowerPoint
  // pptx.writeFile({ fileName: "network_overview.pptx" });
  // };

  useEffect(() => {
    if (hideBtns) {
      createPDF();
    }
  }, [hideBtns]);

  // filter logic
  const [initialData, setInitialData] = useState<any>();
  const synchronizeData = (all, filtered) => {
    // Create a lookup for the filtered data for easy access
    const filteredLookup = filtered.reduce((acc, subDomain) => {
      acc[subDomain.verticalDescrption] = subDomain.oemVendors.reduce(
        (oemAcc, oemVendor) => {
          oemAcc[oemVendor.oemVendor] = oemVendor.data.reduce(
            (dataAcc, data) => {
              dataAcc[data.productNameNeInstances] = data.data.reduce(
                (dataDetailAcc, detail) => {
                  dataDetailAcc[detail.opCoDescrption] =
                    detail.networkElementCount;
                  return dataDetailAcc;
                },
                {}
              );
              return dataAcc;
            },
            {}
          );
          return oemAcc;
        },
        {}
      );
      return acc;
    }, {});

    // Function to update all with filtered data
    function updateData(allData, filteredData) {
      return allData
        ?.map((subDomain) => {
          const filteredSubDomain =
            filteredData[subDomain.verticalDescrption] || {};
          const updatedOemVendors = subDomain.oemVendors
            .map((oemVendor) => {
              const filteredOemVendor =
                filteredSubDomain[oemVendor.oemVendor] || {};
              const updatedData = oemVendor.data
                .map((data) => {
                  const filteredDataEntry =
                    filteredOemVendor[data.productNameNeInstances] || {};
                  const updatedDetails = data.data.map((detail) => {
                    const filteredCount =
                      filteredDataEntry[detail.opCoDescrption] || 0;
                    return {
                      ...detail,
                      networkElementCount: filteredCount,
                    };
                  });

                  const total = updatedDetails.reduce(
                    (sum, detail) => sum + detail.networkElementCount,
                    0
                  );

                  return {
                    ...data,
                    data: updatedDetails,
                    total: total,
                  };
                })
                .filter((data) => data.total > 0);

              return {
                ...oemVendor,
                data: updatedData,
              };
            })
            .filter((oemVendor) => oemVendor.data.length > 0);

          return {
            ...subDomain,
            oemVendors: updatedOemVendors,
          };
        })
        .filter((subDomain) => subDomain.oemVendors.length > 0); // Filter out subDomains with no valid oemVendors
    }

    return updateData(all, filteredLookup);
  };

  // multiselect checkbox
  const [selectedValues, setSelectedValues] = useState<string[]>([]);

  const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { value, checked } = event.target;

    if (value === "") {
      // "All" checkbox
      if (checked) {
        // If "All" is checked, select all other options
        setSelectedValues(networkElementOpco.map(([key]) => key));
        // All fields
        setFormattedGraphData(
          synchronizeData(
            initialData,
            groupAndAggregateData(networkElementGraphData?.item).sort(
              (a, b) => {
                if (a.verticalDescrption < b.verticalDescrption) return -1;
                if (a.verticalDescrption > b.verticalDescrption) return 1;
                return 0;
              }
            )
          )
        );
        // Individual fields
        setSelectedIndividualData(
          groupAndAggregateData(networkElementGraphData?.item)?.filter(
            (individualData) =>
              individualData.verticalDescrption === individualView
          )
        );
      } else {
        // If "All" is unchecked, unselect all options
        setSelectedValues([]);
        // All fields
        setFormattedGraphData(
          synchronizeData(
            initialData,
            groupAndAggregateData(
              filterData(networkElementGraphData?.item, {
                opcoId: [],
              })
            ).sort((a, b) => {
              // Compare the 'verticalDescrption' properties of the objects
              if (a.verticalDescrption < b.verticalDescrption) return -1;
              if (a.verticalDescrption > b.verticalDescrption) return 1;
              return 0;
            })
          )
        );
        // Individual fields
        setSelectedIndividualData(
          synchronizeData(
            initialData.filter(
              (individualData) =>
                individualData.verticalDescrption === individualView
            ),
            groupAndAggregateData(
              filterData(networkElementGraphData?.item, {
                opcoId: [],
              })
            )?.filter(
              (individualData) =>
                individualData.verticalDescrption === individualView
            )
          )
        );
      }
    } else {
      // If an individual option is checked or unchecked
      if (checked) {
        // Add the selected option to the list
        setSelectedValues((prevValues) => [...prevValues, value]);
        // All fields
        console.log("INITIAL DATA", initialData);
        setFormattedGraphData(
          synchronizeData(
            initialData,
            groupAndAggregateData(
              filterData(networkElementGraphData?.item, {
                opcoId: [...selectedValues, value].map(Number),
              })
            ).sort((a, b) => {
              // Compare the 'verticalDescrption' properties of the objects
              if (a.verticalDescrption < b.verticalDescrption) return -1;
              if (a.verticalDescrption > b.verticalDescrption) return 1;
              return 0;
            })
          )
        );
        // Individual fields
        setSelectedIndividualData(
          synchronizeData(
            initialData.filter(
              (individualData) =>
                individualData.verticalDescrption === individualView
            ),
            groupAndAggregateData(
              filterData(networkElementGraphData?.item, {
                opcoId: [...selectedValues, value].map(Number),
              })
            )?.filter(
              (individualData) =>
                individualData.verticalDescrption === individualView
            )
          )
        );
      } else {
        // Remove the deselected option from the list
        setSelectedValues((prevValues) =>
          prevValues.filter((item) => item !== value)
        );
        // All fields
        setFormattedGraphData(
          synchronizeData(
            initialData,
            groupAndAggregateData(
              filterData(networkElementGraphData?.item, {
                opcoId: [...selectedValues]
                  .filter((item) => item !== value)
                  .map(Number),
              })
            ).sort((a, b) => {
              // Compare the 'verticalDescrption' properties of the objects
              if (a.verticalDescrption < b.verticalDescrption) return -1;
              if (a.verticalDescrption > b.verticalDescrption) return 1;
              return 0;
            })
          )
        );
        // Individual fields
        setSelectedIndividualData(
          synchronizeData(
            initialData.filter(
              (individualData) =>
                individualData.verticalDescrption === individualView
            ),
            groupAndAggregateData(
              filterData(networkElementGraphData?.item, {
                opcoId: [...selectedValues]
                  .filter((item) => item !== value)
                  .map(Number),
              })
            )?.filter(
              (individualData) =>
                individualData.verticalDescrption === individualView
            )
          )
        );

        // Uncheck "All" if any option is unchecked
        if (selectedValues.includes("")) {
          setSelectedValues((prevValues) =>
            prevValues.filter((item) => item !== "")
          );
        }
      }
    }
  };

  const resetFilters = () => {
    paginationQuery = {
      subDomainResponseCeFunctionId: [],
      subDomainResponseCeFunction: [],
      oemVendorId: [],
      oemVendor: [],
      productNameNeInstances: [],
      networkElementsAsPlannedId: [],
      networkElementsPlannedName: [],
      opcoId: [],
      opCoDescrption: [],
      verticalId: [],
      systemTypetId: [],
      verticalDescrption: [],
      networkElementCount: [],
      dcfId: [],
      hwBuild: [],
      supportService: [],
      environmentId: [],
      sortBy: "",
      isSortAscending: false,
      page: 1,
      pageSize: pageSize,
      lastModified: undefined,
      principalId: undefined,
      deleted: undefined,
      orphan: undefined,
      lastModifiedBy: [],
    };
    setSelectedOpCo([]);
    setSelectedDcf([]);
    setSelectedBuildCons(null);
    setSelectedSystemType(null);
    setSelectedEnvironment(null);
    setSelectedSupport(null);
    setSelectedVendor(null);
    setSelectedVertical(null);
    setFilterHeading("");
    setTempSelectedOpCo([]);
    setTempSelectedDcf([]);
    getNetworkElementGraph({
      subDomainResponseCeFunctionId: [],
      subDomainResponseCeFunction: [],
      oemVendorId: [],
      oemVendor: [],
      productNameNeInstances: [],
      networkElementsAsPlannedId: [],
      networkElementsPlannedName: [],
      opcoId: [],
      opCoDescrption: [],
      verticalId: [],
      verticalDescrption: [],
      networkElementCount: [],
      systemTypetId: [],
      dcfId: [],
      hwBuild: [],
      supportService: [],
      environmentId: [],
      sortBy: "",
      isSortAscending: false,
      page: 1,
      pageSize: 10,
      lastModified: undefined,
      principalId: undefined,
      deleted: undefined,
      orphan: undefined,
      lastModifiedBy: [],
    });
  };

  const handleIndividualView = (individual) => {
    if (topRef.current) {
      topRef.current.scrollIntoView({
        behavior: "smooth",
        block: "start",
      });
    }
    // if (selectedValues) {
    setSelectedIndividualData(
      synchronizeData(
        initialData?.filter(
          (individualData) => individualData.verticalDescrption === individual
        ),
        groupAndAggregateData(networkElementGraphData?.item)?.filter(
          (individualData) => individualData.verticalDescrption === individual
        )
      )
    );
    // } else {
    //   setSelectedIndividualData(
    //     synchronizeData(
    //       initialData?.filter(
    //         (individualData) => individualData.verticalDescrption === individual
    //       ),
    //       groupAndAggregateData(networkElementGraphData?.item)?.filter(
    //         (individualData) => individualData.verticalDescrption === individual
    //       )
    //     )
    //   );
    // }
  };

  const handleDownloadPPT = () => {
    // Create PowerPoint slides from vendor data
    // console.log("formattedGraphData", formattedGraphData);
    // const components = formattedGraphData.map((vendor, index) => {
    //   vendor?.oemVendors?.map((res) => {
    //     console.log("res", res);
    //     const allTotalsZero = res.data.every((item) => item.total === 0);
    //     console.log("allTotalsZero", allTotalsZero);
    //     return (
    //       <>
    //         {!allTotalsZero && (
    //           <MainCategoryRow
    //             key={vendor.oemVendors}
    //             currentVendor={vendor}
    //             indexValue={index}
    //           />
    //         )}
    //       </>
    //     );
    //   });
    // });
    const components = formattedGraphData?.map((vendor, index) => (
      <MainCategoryRow
        key={vendor.oemVendors}
        currentVendor={vendor}
        indexValue={index}
      />
    ));
    createPPT(components);
  };
  const [displayHeading, setDisplayHeading] = useState<any>([]);
  const getFilterHeading = () => {
    let combineArray: any = [];
    // Find matching Opcos from data?.allOpcos
    const matchedOpcos = selectedOpCo
      .map((id) =>
        networkElementGraphData?.allResource?.data?.allOpcos?.find(
          (opco) => opco.key === id
        )
      )
      .filter((opco) => opco); // Filter out undefined results

    // Append Opcos to the combined string
    if (matchedOpcos.length > 0) {
      combineArray.push({
        name: "Opco",
        list: matchedOpcos.map((opco, index) => opco.value) ?? null,
      });
    }
    // Find matching Opcos from data?.allDcf
    const matchedDcf = selectedDcf
      .map((id) =>
        networkElementGraphData?.allResource?.data?.allDcf?.find(
          (dcf) => dcf.id === id
        )
      )
      .filter((dcf) => dcf); // Filter out undefined results

    // Append Opcos to the combined string
    if (matchedDcf.length > 0) {
      combineArray.push({
        name: "DCF",
        list: matchedDcf.map((dcf, index) => dcf.description) ?? null,
      });
    }
    const matchedVertical =
      selectedVertical
        ?.map((id) =>
          networkElementGraphData?.allResource?.data?.allVertical?.find(
            (vertical) => vertical.key === id
          )
        )
        .filter((vertical) => vertical) ?? []; // Filter out undefined results

    if (matchedVertical.length > 0) {
      combineArray.push({
        name: "Vertical",
        list: matchedVertical.map((vertical, index) => vertical.value) ?? null,
      });
    }
    const matchedEnv =
      selectedEnvironment
        ?.map((id) =>
          networkElementGraphData?.allResource?.data?.allEnvironment?.find(
            (env) => env.key === id
          )
        )
        .filter((env) => env) ?? []; // Filter out undefined results
    if (matchedEnv.length > 0) {
      combineArray.push({
        name: "Environment",
        list: matchedEnv.map((env, index) => env.value) ?? null,
      });
    }
    // console.log("selectedStstem", selectedSystemType);
    const matchedSystemType =
      selectedSystemType
        ?.map((id) =>
          networkElementGraphData?.allResource?.data?.allSystemType?.find(
            (sys) => sys.key === id
          )
        )
        .filter((sys) => sys) ?? []; // Filter out undefined results
    console.log("matchedSystemType", matchedSystemType);
    if (matchedSystemType.length > 0) {
      combineArray.push({
        name: "System Type",
        list: matchedSystemType.map((sys, index) => sys.value) ?? null,
      });
    }
    const matchedVendor =
      selectedVendor
        ?.map((id) =>
          networkElementGraphData?.allResource?.data?.allVendor?.find(
            (vendor) => vendor.key === id
          )
        )
        .filter((vendor) => vendor) ?? []; // Filter out undefined results

    if (matchedVendor.length > 0) {
      combineArray.push({
        name: "Vendor",
        list: matchedVendor.map((vendor, index) => vendor.value) ?? null,
      });
    }
    const matchedSupport =
      selectedSupport
        ?.map((id) =>
          networkElementGraphData?.allResource?.data?.allSupport?.find(
            (support) => support.key === id
          )
        )
        .filter((support) => support) ?? []; // Filter out undefined results

    if (matchedSupport.length > 0) {
      combineArray.push({
        name: "Supported Services",
        list: matchedSupport.map((support, index) => support.value) ?? null,
      });
    }
    const matchedBuildCons =
      selectedBuildCons
        ?.map((id) =>
          networkElementGraphData?.allResource?.data?.allBuildCons?.find(
            (buildCons) => buildCons.key === id
          )
        )
        .filter((buildCons) => buildCons) ?? []; // Filter out undefined results

    if (matchedBuildCons.length > 0) {
      combineArray.push({
        name: "Build Construction",
        list: matchedSupport.map((buildCons, index) => buildCons.value) ?? null,
      });
    }
    console.log("combineArray", combineArray);
    setDisplayHeading(combineArray);
  };

  const [tempSelectedOpCo, setTempSelectedOpCo] = useState<any[]>(selectedOpCo);
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const opcoOptions =
    networkElementGraphData?.allResource?.data?.allOpcos || [];

  // Open & Close Handlers
  const handleOpen = (event: React.SyntheticEvent) => {
    setTempSelectedOpCo(selectedOpCo); // Preserve previous selection
    setAnchorEl(event.currentTarget as HTMLElement);
  };

  const handleClose = () => setAnchorEl(null);

  const handleApply = () => {
    setSelectedOpCo(tempSelectedOpCo); // Apply selected values
    handleClose();
  };

  const handleSelectAll = () => {
    if (tempSelectedOpCo.length === opcoOptions.length) {
      setTempSelectedOpCo([]);
    } else {
      setTempSelectedOpCo([...opcoOptions]);
    }
  };

  const [tempSelectedDcf, setTempSelectedDcf] = useState<any[]>(selectedDcf);
  const [anchorE2, setAnchorE2] = useState<null | HTMLElement>(null);
  const dcfOptions =
    networkElementGraphData?.allResource?.data?.allDcf?.map((val) => {
      return { key: val.id, value: val.description };
    }) || [];

  // Open & Close Handlers
  const handleDcfOpen = (event: React.SyntheticEvent) => {
    setTempSelectedDcf(selectedDcf); // Preserve previous selection
    setAnchorE2(event.currentTarget as HTMLElement);
  };

  const handleCloseE2 = () => setAnchorE2(null);

  const dcfHandleApply = () => {
    setSelectedDcf(tempSelectedDcf); // Apply selected values
    handleCloseE2();
  };

  const handleDcfSelectAll = () => {
    if (tempSelectedDcf.length === dcfOptions.length) {
      setTempSelectedDcf([]);
    } else {
      setTempSelectedDcf([...dcfOptions]);
    }
  };

  const InvocheDownload = async () => {
    let result = await GetAssetOverviewByMarketReport({
      ...paginationQuery,
      opcoId: selectedOpCo,
      dcfId: selectedDcf,
      hwBuild: selectedBuildCons,
      environmentId: selectedEnvironment,
      supportService: selectedSupport,
      systemTypetId: selectedSystemType,
      oemVendorId: selectedVendor,
      verticalId: selectedVertical,
    });

    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };

  const hamdleRedirect = () => {
    const filterObj: Record<string, any> = {};

    if (selectedOpCo?.length > 0) filterObj.opcoId = selectedOpCo;
    if (selectedDcf?.length > 0) filterObj.dcfId = selectedDcf;

    const conditionalFields = [
      { key: "buildConsId", value: selectedBuildCons },
      { key: "environmentId", value: selectedEnvironment },
      { key: "supportServiceId", value: selectedSupport },
      { key: "vendorId", value: selectedVendor },
      { key: "verticalId", value: selectedVertical },
      { key: "systemTypeId", value: selectedSystemType },
    ];

    conditionalFields.forEach(({ key, value }) => {
      if (value !== 0 && value !== null) {
        filterObj[key] = [value];
      }
    });
    setSelectedOpCo([]);
    setSelectedDcf([]);
    setSelectedBuildCons(null);
    setSelectedSystemType(null);
    setSelectedEnvironment(null);
    setSelectedSupport(null);
    setSelectedVendor(null);
    setSelectedVertical(null);
    setFilterHeading("");
    setTempSelectedOpCo([]);
    setTempSelectedDcf([]);
    getNetworkElementGraph({
      subDomainResponseCeFunctionId: [],
      subDomainResponseCeFunction: [],
      oemVendorId: [],
      oemVendor: [],
      productNameNeInstances: [],
      networkElementsAsPlannedId: [],
      networkElementsPlannedName: [],
      opcoId: [],
      opCoDescrption: [],
      verticalId: [],
      verticalDescrption: [],
      networkElementCount: [],
      dcfId: [],
      hwBuild: [],
      supportService: [],
      systemTypetId: [],
      environmentId: [],
      sortBy: "",
      isSortAscending: false,
      page: 1,
      pageSize: 10,
      lastModified: undefined,
      principalId: undefined,
      deleted: undefined,
      orphan: undefined,
      lastModifiedBy: [],
    });
    const queryString = new URLSearchParams(filterObj).toString();
    window.open(`/assetoverviewbymarket/?${queryString}`, "_blank");
  };

  const checkIsDisable = () => {
    const requiredArrays = [selectedOpCo, selectedDcf];
    const requiredValues = [
      selectedBuildCons,
      selectedEnvironment,
      selectedSupport,
      selectedVendor,
      selectedVertical,
      selectedSystemType,
    ];

    const isArrayValid = requiredArrays.some(
      (arr) => Array.isArray(arr) && arr.length > 0
    );
    const isValueValid = requiredValues.some(
      (val) => val !== 0 && val !== null
    );

    return !(isArrayValid || isValueValid);
  };

  const RenderCardContent = ({
    node,
    level = 1,
    firstLevelTitle = "", // Add a new prop to track the first-level title
  }: {
    node: any;
    level?: number;
    firstLevelTitle?: string; // New prop to pass the first-level title
  }) => {
    const isLeaf =
      !Array.isArray(node?.data) ||
      typeof node?.data[0] !== "object" ||
      !node?.data[0]?.title;

    // If at the first level, store the title
    if (level === 1 && node?.title) {
      firstLevelTitle = node?.title;
    }

    // Only render from level 2 onwards, but stop at level 4
    if (level < 2) {
      return node?.data?.map((child: any, index: number) => (
        <RenderCardContent
          key={index}
          node={child}
          level={level + 1}
          firstLevelTitle={firstLevelTitle}
        />
      ));
    }

    if (level >= 3 && !isQueryParams) {
      const total = node?.data?.reduce((sum, item) => sum + item.data[0], 0);
      return (
        <Chip
          key={`${firstLevelTitle}-${node.title}-${node.data[0]}`}
          sx={{
            position: "relative",
            display: "flex",
            flexWrap: "wrap",
            color: "black",
            gap: "6px",
            fontWeight: "bolder",
            background: "linear-gradient(to bottom, #ffd4d4, #ff8f8fed)",
            border: "1px solid #e22727",
            paddingRight: 0,
            paddingBottom: 0,
            borderRadius: "4px",
          }}
          color="primary"
          variant="outlined"
          avatar={
            <Avatar
              sx={{
                backgroundColor: "#ffffff40 !important",
                color: "#000000de !important",
                fontSize: "15px !important",
              }}
            >
              {total}
            </Avatar>
          }
          label={node.title}
        ></Chip>
      );
    }
    // Stop rendering children at level 4 and display the title
    if (level >= 4 && isQueryParams) {
      return (
        <Chip
          key={`${firstLevelTitle}-${node.title}-${node.data[0]}`}
          sx={{
            position: "relative",
            display: "flex",
            flexWrap: "wrap",
            color: "black",
            gap: "6px",
            fontWeight: "bolder",
            background:
              "linear-gradient(to bottom, #f9909000, rgba(0, 0, 0, 0.12))",
            border: "1px solid rgb(249 144 144)",
            paddingRight: 0,
            paddingBottom: 0,
            borderRadius: "4px",
          }}
          color="primary"
          variant="outlined"
          avatar={
            <Avatar
              sx={{
                backgroundColor: "#ffffff26 !important",
                color: "#000000de !important",
                fontSize: "15px !important",
              }}
            >
              {node.data[0]}
            </Avatar>
          }
          label={node.title}
        ></Chip>
      );
    }

    return (
      <CardContent
        sx={{
          padding: level === 3 ? "14px 10px 0px 10px" : "16px",
          background: level === 3 ? "none" : "#defcff",
          color: "black",
        }}
      >
        <Box
          sx={{
            border: "2px solid black",
            borderColor: level === 3 ? "#e22727" : "black",
            backgroundColor: level === 3 ? "#d9f7f9" : "#ffffff",
            background:
              level === 3 ? "linear-gradient(to bottom, #ffd4d4, #ff8f8f)" : "",
            position: "relative",
            padding:
              level === 2 && !isQueryParams
                ? "24px 14px 14px 14px"
                : level === 2
                ? "16px 0px"
                : "16px",
            display: "flex",
            flexWrap: "wrap",
            gap: 2,
            borderRadius: 2,
            paddingTop: "24px",
          }}
        >
          <Box
            sx={{
              position: "absolute",
              top: -12,
              left: -2,
              backgroundColor: level === 3 ? "#e22727" : "black",
              color: "white",
              px: 1.5,
              py: 0.5,
              fontWeight: "bold",
              borderTopLeftRadius: 4,
              borderTopRightRadius: 4,
              fontSize: "12px",
              width: "max-content",
            }}
          >
            {node?.title}
          </Box>

          {isLeaf ? (
            <Card
              sx={{
                minWidth: 100,
                backgroundColor: "#007c9957",
                border: "2px solid #a94442",
                borderRadius: "12px",
                padding: "8px 16px",
                boxShadow: "none",
                textAlign: "center",
              }}
            >
              <CardContent sx={{ padding: "0 !important" }}>
                <Typography
                  variant="body2"
                  sx={{
                    fontWeight: "bold",
                    fontSize: "14px",
                    color: "#333",
                  }}
                >
                  {String(node?.data?.[0])}
                </Typography>
              </CardContent>
            </Card>
          ) : (
            node?.data?.map((child: any, index: number) => (
              <RenderCardContent
                key={index}
                node={child}
                level={level + 1}
                firstLevelTitle={firstLevelTitle}
              />
            ))
          )}
        </Box>
      </CardContent>
    );
  };

  const MainCard = ({ res }: { res: any }) => (
    <Card
      key={res?.title ?? ""}
      elevation={4}
      sx={{
        background: "linear-gradient(to bottom, #00c2e4, #0097b1)",
        color: "white",
        borderStyle: "double",
        // width: "fit-content",
        // mb: 2,
        height:
          hideBtns === true ||
          (collapseMenuTriggered === true && selectedTab?.length > 0) ||
          (collapseMenuTriggered === true && selectedTab?.length === 0)
            ? "fit-content"
            : "45rem",
        display: "flex",
        flexDirection: "column",
      }}
    >
      <CardHeader
        sx={{
          padding: "8px 16px",
          borderBottomStyle:
            !collapseMenuTriggered || selectedTab?.length > 0 ? "double" : "",
          textAlign: "left",
          backgroundColor: "#007c9991",
          position: "sticky",
          top: 0,
          zIndex: 1,
          ".MuiCardHeader-content": {
            overflow: "hidden",
          },
        }}
        title={
          <>
            {/* {selectedTab?.length > 0 && (
              <IconButton aria-label="Back" onClick={() => setSelectedTab([])}>
                <GoArrowLeft color="white" />
              </IconButton>
            )} */}
            <Typography
              variant="button"
              sx={{
                fontSize: "16px",
                color: "white",
                textAlign: "left",
                fontWeight: "bolder",
                wordBreak: "break-word",
                textTransform: "none",
              }}
            >
              {res?.title ?? ""}
            </Typography>
          </>
        }
        action={
          <IconButton
            aria-label="Expand"
            sx={{ alignSelf: "center" }}
            onClick={() => {
              selectedTab?.length === 0
                ? setSelectedTab(
                    (prev) =>
                      prev.includes(res?.title)
                        ? prev.filter((t) => t !== res?.title) // remove if already present
                        : [...prev, res?.title] // add if not present
                  )
                : setSelectedTab([]);
            }}
          >
            {selectedTab?.length === 0 ? (
              <BiExpandAlt color="white" />
            ) : (
              <BiCollapseAlt color="white" />
            )}
          </IconButton>
        }
      />
      {(!collapseMenuTriggered || selectedTab?.length > 0) && (
        <Box
          sx={{
            overflowY: "auto",
            flexGrow: 1,
            display: hideBtns ? "flex" : "",
            gap: hideBtns ? 2 : 0,
            flexWrap: "wrap",
            backgroundColor: "#defcff",
          }}
        >
          <RenderCardContent node={res} firstLevelTitle={res?.title} />
        </Box>
      )}
    </Card>
  );

  return (
    <>
      <ThemeProvider theme={theme}>
        <Drawer
          anchor={"right"}
          open={showCanvas}
          onClose={() => setShowCanvas(!showCanvas)}
        >
          <Box
            sx={{
              display: "flex",
              alignItems: "center",
              width: "400px",
              padding: 2,
              bgcolor: "background.paper",
              color: "text.secondary",
              "& svg": {
                m: 1,
              },
            }}
          >
            <BiSolidFilterAlt />
            <Typography variant="h6" gutterBottom sx={{ mb: 0 }}>
              Filter
            </Typography>
          </Box>
          <Divider />
          <Box sx={{ p: 2, mb: 4 }}>
            <Stack
              direction="column"
              sx={{ width: "100%", textAlign: "left" }}
              spacing={2}
            >
              <Box
                sx={{
                  width: "23rem !important",
                  rowGap: "1rem !important",
                  display: "grid",
                }}
              >
                <MultiSelectCheckmarks
                  options={
                    networkElementGraphData?.allResource?.data?.allOpcos ?? []
                  }
                  selectedValues={selectedOpCo}
                  onChange={(e) => {
                    getNetworkElementGraph(null, true, "opco", e);
                    setSelectedOpCo(e);
                  }}
                  label="Select Opco"
                  isComponent={true}
                  darkTheme={darkMode}
                />
                <MultiSelectCheckmarks
                  options={
                    networkElementGraphData?.allResource?.data?.allDcf?.map(
                      (val) => {
                        return { key: val.id, value: val.description };
                      }
                    ) ?? []
                  }
                  selectedValues={selectedDcf}
                  onChange={(e) => {
                    getNetworkElementGraph(null, true, "dcf", e);
                    setSelectedDcf(e);
                  }}
                  label="Select DCF"
                  darkTheme={darkMode}
                />
                <MultiSingleSelect
                  options={
                    networkElementGraphData?.allResource?.data?.allVertical ??
                    []
                  }
                  selectedValues={selectedVertical}
                  onChange={(e) => {
                    getNetworkElementGraph(null, true, "vertical", e);
                    setSelectedVertical(e);
                  }}
                  label="Select Vertical"
                  darkTheme={darkMode}
                />
                <MultiSingleSelect
                  options={
                    networkElementGraphData?.allResource?.data
                      ?.allEnvironment ?? []
                  }
                  selectedValues={selectedEnvironment}
                  onChange={(e) => {
                    getNetworkElementGraph(null, true, "env", e);
                    setSelectedEnvironment(e);
                  }}
                  label="Select Environment"
                  darkTheme={darkMode}
                />
                <MultiSingleSelect
                  options={
                    networkElementGraphData?.allResource?.data?.allSystemType ??
                    []
                  }
                  selectedValues={selectedSystemType}
                  onChange={(e) => {
                    getNetworkElementGraph(null, true, "systemType", e);
                    setSelectedSystemType(e);
                  }}
                  label="Select System Type"
                  darkTheme={darkMode}
                />
                <MultiSingleSelect
                  options={
                    networkElementGraphData?.allResource?.data?.allBuildCons ??
                    []
                  }
                  selectedValues={selectedBuildCons}
                  onChange={(e) => {
                    getNetworkElementGraph(null, true, "build", e);
                    setSelectedBuildCons(e);
                  }}
                  label="Select Build Construction"
                  darkTheme={darkMode}
                />
                <MultiSingleSelect
                  options={
                    networkElementGraphData?.allResource?.data?.allSupport ?? []
                  }
                  selectedValues={selectedSupport}
                  onChange={(e) => {
                    getNetworkElementGraph(null, true, "support", e);
                    setSelectedSupport(e);
                  }}
                  label="Select Supported Services"
                  darkTheme={darkMode}
                />
                <MultiSingleSelect
                  options={
                    networkElementGraphData?.allResource?.data?.allVendor ?? []
                  }
                  selectedValues={selectedVendor}
                  onChange={(e) => {
                    getNetworkElementGraph(null, true, "vendor", e);
                    setSelectedVendor(e);
                  }}
                  label="Select Vendor"
                  darkTheme={darkMode}
                />
              </Box>
              <Box sx={{ py: 2, mb: 5 }}>
                <Stack
                  direction="row"
                  spacing={2}
                  sx={{ justifyContent: "space-between" }}
                >
                  <Button
                    variant="outlined"
                    color="inherit"
                    onClick={() => {
                      resetFilters();
                      setShowCanvas(false);
                    }}
                  >
                    Close
                  </Button>
                  <div className="d-flex">
                    <Button
                      variant="contained"
                      color="inherit"
                      onClick={() => {
                        resetFilters();
                      }}
                      sx={{ marginRight: 2 }}
                    >
                      Reset
                    </Button>
                    <Button
                      variant="contained"
                      color="error"
                      onClick={() => {
                        setShowCanvas(false);
                        hamdleRedirect();
                      }}
                      disabled={checkIsDisable()}
                    >
                      Apply
                    </Button>
                  </div>
                </Stack>
              </Box>
            </Stack>
          </Box>
        </Drawer>
        <Modal
          show={excelPopup === true}
          backdrop="static"
          keyboard={false}
          size="lg"
          centered
        >
          <Modal.Header className="d-flex justify-content-center">
            <div className="col-12 px-0">
              <div className="col-12">Export Reports</div>
            </div>
          </Modal.Header>
          <Modal.Body>
            <Form style={{ padding: "0px 10px " }}>
              {["radio"].map((type) => (
                <div key={`inline-${type}`} className="mb-3">
                  <Form.Check
                    onClick={() => setDownloadType("pdf")}
                    inline
                    checked={downloadType === "pdf" ? true : false}
                    label="PDF"
                    name="group1"
                    type={"radio"}
                    id={`inline-${type}-1`}
                  />
                  <Form.Check
                    onClick={() => setDownloadType("ppt")}
                    inline
                    checked={downloadType === "ppt" ? true : false}
                    label="PPT"
                    name="group1"
                    type={"radio"}
                    id={`inline-${type}-1`}
                  />
                  <Form.Check
                    onClick={() => setDownloadType("Excel")}
                    inline
                    checked={downloadType === "Excel" ? true : false}
                    label="Excel"
                    name="group1"
                    type={"radio"}
                    id={`inline-${type}-1`}
                  />
                </div>
              ))}
            </Form>
          </Modal.Body>
          <Modal.Footer className="headerPage row mx-0">
            <button
              className="download-to-excel"
              onClick={() => setExcelPopUp(false)}
            >
              Cancel
            </button>
            <button
              className="btn btn-danger mrl-10"
              onClick={() => {
                if (downloadType === "ppt") {
                  handleDownloadPPT();
                  setLoader("ADD", "");
                } else if (downloadType === "Excel") {
                  InvocheDownload();
                } else {
                  setHideBtns(true);
                  setLoader("ADD", "");
                }
                setExcelPopUp(false);
              }}
            >
              Download
            </button>
          </Modal.Footer>
        </Modal>
      </ThemeProvider>
      <div className="pageContainer" id="pdf">
        <div ref={topRef} />
        {hideBtns ? (
          <>
            <div
              style={{
                display: "flex",
                justifyContent: "space-between",
              }}
            >
              <div className="headerPage row mx-0">
                <h3
                  className="voda-bold fz-28"
                  style={{
                    textAlign: "left",
                    color: `${darkMode ? "white" : "black"}`,
                    display: "flex",
                  }}
                >
                  Asset Overview by Market
                </h3>
              </div>
              <div className="d-flex">
                <section className="legend-container">
                  <div className="legend-style legend-blue"></div>
                  <div className="legend-blue-title pl-2">
                    Network Functions
                  </div>
                </section>
                <section className="legend-container">
                  <div className="legend-style legend-white"></div>
                  <div
                    className="legend-white-title pl-2"
                    style={{ color: "white" }}
                  >
                    Vendor
                  </div>
                </section>
                <section className="legend-container">
                  <div className="legend-style legend-orange"></div>
                  <div className="legend-orange-title pl-2">NE - instances</div>
                </section>
              </div>
            </div>
            {isQueryParams && (
              <Accordion defaultExpanded>
                <AccordionSummary
                  expandIcon={<IoMdArrowUp size={25} />}
                  aria-controls="panel1-content"
                  id="panel1-header"
                  sx={{ minHeight: "40px !important" }}
                >
                  <Typography
                    component="span"
                    sx={{ fontWeight: "bolder !important" }}
                  >
                    Applied Filters
                  </Typography>
                </AccordionSummary>
                <AccordionDetails>
                  {displayHeading?.map(({ name, list }) => (
                    <div className="row">
                      <div className="col-2">
                        <FormLabel
                          component="legend"
                          sx={{
                            width: "auto !important",
                            display: "block",
                            fontWeight: "bold !important",
                            marginBottom: "0px !important",
                            fontSize: "16px !important",
                            marginTop: "12px",
                          }}
                        >
                          {name}
                        </FormLabel>
                      </div>
                      <div className="col-12">
                        {list?.map((val) => {
                          return (
                            <Chip
                              sx={{ width: "max-content" }}
                              variant="outlined"
                              label={
                                <Typography
                                  component="span"
                                  sx={{
                                    display: "block",
                                    whiteSpace: "normal",
                                    wordBreak: "break-word",
                                  }}
                                  dangerouslySetInnerHTML={{ __html: val }}
                                />
                              }
                            />
                          );
                        })}
                      </div>
                    </div>
                  ))}
                </AccordionDetails>
              </Accordion>
            )}
            {filterHeading && (
              <div className="headerPage row mx-0 px-0 my-3">
                <div
                  className="voda-bold fz-28 ml-0"
                  style={{
                    textAlign: "left",
                    marginLeft: "1rem",
                    color: `${darkMode ? "white" : "black"}`,
                  }}
                >
                  {"Filters -  "}
                  <span
                    dangerouslySetInnerHTML={{
                      __html: filterHeading,
                    }}
                  />
                </div>
              </div>
            )}
          </>
        ) : (
          <Grid
            size={12}
            sx={{
              height: "8vh",
              textAlign: "center",
              alignContent: "center",
              alignItems: "center",
              justifyContent: "space-between",
              display: "flex",
            }}
          >
            <Typography
              variant="h4"
              sx={{
                marginBottom: 0,
                display: "flex",
                alignItems: "center",
                fontSize: "1.8rem !important",
                color: (theme) => theme.palette.text.primary, // Using primary color
              }}
            >
              {/* <HtmlTooltip title={<>{"Asset Overview by Market"}</>}>
                <Button
                  startIcon={<FcInfo />}
                  sx={{
                    width: "max-content",
                    fontWeight: "bold",
                    fontSize: "24px !important",
                    color: "white",
                    textTransform: "unset",
                  }}
                >
                  Asset Overview by Market
                </Button>
              </HtmlTooltip> */}

              <Button
                sx={{
                  width: "max-content",
                  fontWeight: "bold",
                  fontSize: "24px !important",
                  color: "white",
                  textTransform: "unset",
                }}
              >
                Asset Overview by Market
              </Button>
            </Typography>
            <div
              className="d-flex pr-0"
              style={{
                display: `${hideBtns ? "none" : "block"}`,
                alignContent: "center",
              }}
            >
              {formattedGraphData?.length &&
                isQueryParams &&
                selectedTab?.length === 0 && (
                  <Button
                    variant="contained"
                    size="medium"
                    startIcon={
                      collapseMenuTriggered ? (
                        <BsArrowsExpand />
                      ) : (
                        <BsArrowsCollapse />
                      )
                    }
                    sx={{
                      backgroundColor: "#6c757db0",
                      color: "white",
                      fontWeight: "bold",
                      "&:hover": {
                        backgroundColor: "#6c757d70",
                      },
                    }}
                    onClick={(e) => {
                      e.preventDefault();
                      collapseMenu();
                    }}
                  >
                    {collapseMenuTriggered ? "Expand All" : "Collapse All"}
                  </Button>
                )}

              {!isNoData && (
                <Button
                  variant="contained"
                  size="medium"
                  color="error"
                  endIcon={<IoMdDownload style={{ color: "white" }} />}
                  sx={{
                    width: "fit-content",
                    marginLeft: 2,
                    marginRight: 2,
                    minWidth: "10rem",
                    // backgroundColor: "#e0f7fa", // light blue
                    // color: "#006064", // contrast text color
                    fontWeight: "bold",
                    boxShadow: "none", // no shadow
                    "&:hover": {
                      backgroundColor: "#b20000", // slightly darker cyan for contrast
                    },
                  }}
                  onClick={(e) => {
                    e.preventDefault();
                    setExcelPopUp(true);
                  }}
                >
                  Download
                </Button>
              )}
              <CustomMenuButton
                buttonLabel="More Options"
                endIcon={<FiChevronDown />}
                darkTheme={darkMode}
                options={[
                  {
                    title: "Legend",
                  },
                  {
                    label: "Network Functions",
                    iconOrContent: (
                      <div className="legendaElement legend-blue"></div>
                    ),
                  },
                  {
                    label: "Vendor",
                    iconOrContent: (
                      <div className="legendaElement legend-white"></div>
                    ),
                  },
                  {
                    label: "NE - instances",
                    iconOrContent: (
                      <div className="legendaElement legend-orange"></div>
                    ),
                  },
                ]}
              />
            </div>
          </Grid>
        )}
        {isQueryParams && !hideBtns && (
          <Accordion>
            <AccordionSummary
              expandIcon={<IoMdArrowUp size={25} />}
              aria-controls="panel1-content"
              id="panel1-header"
              sx={{ minHeight: "40px !important" }}
            >
              <Typography
                component="span"
                sx={{ fontWeight: "bolder !important" }}
              >
                Applied Filters
              </Typography>
            </AccordionSummary>
            <AccordionDetails>
              {displayHeading?.map(({ name, list }) => (
                <>
                  <Box
                    sx={{
                      display: "grid",
                      px: 0,
                      mx: 0,
                      alignItems: "flex-start",
                      gap: 2,
                      gridTemplateColumns: { xs: "1fr", md: "auto 1fr" },
                    }}
                  >
                    <Box
                      sx={{
                        display: "flex",
                        justifyContent: "flex-start",
                        alignItems: "center",
                        justifySelf: "start",
                        width: "max-content",
                        gap: 2,
                        mt: { xs: 2, md: 0 },
                      }}
                    >
                      {name}
                    </Box>
                    <Box sx={{ display: "flex", flexWrap: "wrap", gap: 2 }}>
                      {list?.map((val) => (
                        <Chip
                          sx={{ width: "max-content" }}
                          variant="outlined"
                          label={
                            <Typography
                              component="span"
                              sx={{
                                display: "block",
                                whiteSpace: "normal",
                                wordBreak: "break-word",
                              }}
                              dangerouslySetInnerHTML={{ __html: val }}
                            />
                          }
                        />
                      ))}
                    </Box>
                  </Box>
                  {/* <div className="d-flex mr-2">
                  <FormLabel
                    component="legend"
                    sx={{
                      width: "auto !important",
                      display: "block",
                      fontWeight: "bold !important",
                      marginBottom: "0px !important",
                      fontSize: "16px !important",
                      marginTop: "12px",
                    }}
                  >
                    {name}
                  </FormLabel>
                  <Box
                    key={name}
                    sx={{
                      display: "flex !important",
                      alignItems: "center !important",
                      overflowX: "auto",
                    }}
                    component="ul"
                  >
                    {list?.map((val) => {
                      return (
                        <ListItem key={`${val}`} sx={{ width: "auto" }}>
                          <Chip
                            sx={{ width: "max-content" }}
                            variant="outlined"
                            label={
                              <Typography
                                component="span"
                                sx={{
                                  display: "block",
                                  whiteSpace: "normal",
                                  wordBreak: "break-word",
                                }}
                                dangerouslySetInnerHTML={{ __html: val }}
                              />
                            }
                          />
                        </ListItem>
                      );
                    })}
                  </Box>
                </div> */}
                </>
              ))}
            </AccordionDetails>
          </Accordion>
        )}
        {!hideBtns && filterHeading && (
          <div className="headerPage row mx-0 px-0 my-3">
            <div
              className="voda-bold fz-28 ml-0"
              style={{
                textAlign: "left",
                marginLeft: "1rem",
                color: `${darkMode ? "white" : "black"}`,
              }}
            >
              {"Filters -  "}
              <span
                dangerouslySetInnerHTML={{
                  __html: filterHeading,
                }}
              />
            </div>
          </div>
        )}

        <Box sx={{ flexGrow: 1 }}>
          <ThemeProvider theme={darkTheme}>
            {!hideBtns && !isQueryParams && (
              <Box
                sx={{
                  px: 0,
                  mx: 0,
                  display: "grid",
                  gridTemplateColumns: { xs: "1fr", md: "auto 1fr" },
                  alignItems: "flex-start",
                  gap: 2,
                }}
              >
                <Box sx={{ display: "flex", flexWrap: "wrap", gap: 2 }}>
                  <Box>
                    <MultiSelectCheckmarks
                      options={
                        networkElementGraphData?.allResource?.data?.allOpcos ??
                        []
                      }
                      size="small"
                      selectedValues={selectedOpCo}
                      onChange={(e) => {
                        getNetworkElementGraph(null, true, "opco", e);
                        setSelectedOpCo(e);
                      }}
                      label="Select Opco"
                      darkTheme={darkMode}
                      widthSize={200}
                    />
                  </Box>

                  <Box>
                    <MultiSingleSelect
                      options={
                        networkElementGraphData?.allResource?.data
                          ?.allSupport ?? []
                      }
                      size="small"
                      selectedValues={selectedSupport}
                      onChange={(e) => {
                        getNetworkElementGraph(null, true, "support", e);
                        setSelectedSupport(e);
                      }}
                      label="Select Supported Services"
                      darkTheme={darkMode}
                      widthSize={250}
                    />
                  </Box>

                  <Box>
                    <MultiSelectCheckmarks
                      options={
                        networkElementGraphData?.allResource?.data?.allDcf?.map(
                          (val) => ({
                            key: val.id,
                            value: val.description,
                          })
                        ) ?? []
                      }
                      size="small"
                      selectedValues={selectedDcf}
                      onChange={(e) => {
                        getNetworkElementGraph(null, true, "dcf", e);
                        setSelectedDcf(e);
                      }}
                      label="Select DCF"
                      darkTheme={darkMode}
                      widthSize={300}
                    />
                  </Box>

                  {(selectedOpCo?.length > 0 ||
                    selectedDcf?.length > 0 ||
                    (selectedSupport !== null && selectedSupport !== 0)) && (
                    <>
                      <Button
                        variant="contained"
                        size="medium"
                        color="error"
                        sx={{ fontWeight: "bold" }}
                        onClick={(e) => {
                          e.preventDefault();
                          hamdleRedirect();
                        }}
                      >
                        Apply
                      </Button>
                      <Button
                        variant="outlined"
                        size="medium"
                        sx={{
                          fontWeight: "bold",
                          color: "white",
                          borderColor: "white",
                        }}
                        onClick={(e) => {
                          e.preventDefault();
                          resetFilters();
                        }}
                      >
                        Reset
                      </Button>
                    </>
                  )}
                </Box>
                {formattedGraphData?.length && !isQueryParams && (
                  <Box
                    sx={{
                      display: "flex",
                      justifyContent: "flex-end",
                      alignItems: "center",
                      justifySelf: "end",
                      width: "max-content",
                      mt: { xs: 2, md: 0 },
                      gap: 2,
                    }}
                  >
                    <Button
                      size="medium"
                      endIcon={<IoFilter />}
                      sx={{
                        fontWeight: "bold",
                        color: "white",
                        "&:hover": {
                          background: "white",
                          color: "black",
                        },
                      }}
                      onClick={(e) => {
                        e.preventDefault();
                        setShowCanvas(true);
                      }}
                    >
                      More Filters
                    </Button>

                    {selectedTab?.length === 0 && (
                      <Button
                        variant="contained"
                        size="medium"
                        startIcon={
                          collapseMenuTriggered ? (
                            <BsArrowsExpand />
                          ) : (
                            <BsArrowsCollapse />
                          )
                        }
                        sx={{
                          backgroundColor: "#6c757db0",
                          minWidth: "10rem",
                          color: "white",
                          fontWeight: "bold",
                          "&:hover": {
                            backgroundColor: "#6c757d70",
                          },
                        }}
                        onClick={(e) => {
                          e.preventDefault();
                          collapseMenu();
                        }}
                      >
                        {collapseMenuTriggered ? "Expand All" : "Collapse All"}
                      </Button>
                    )}
                  </Box>
                )}
              </Box>
            )}
            <Box
              sx={{
                // p: "20px 10px",
                // borderRadius: 2,
                // bgcolor: "background.default",
                // background: "#e9ecef",
                marginTop: "2rem",
                // borderRadius: "7px",
                // borderStyle: "double",
                display: "grid",
                height: "fit-content",
                gridTemplateColumns: {
                  xs:
                    selectedTab?.length > 0
                      ? "1fr"
                      : collapseMenuTriggered
                      ? "repeat(auto-fit, minmax(300px, 1fr))"
                      : "1fr 1fr",
                },
                gap: "40px",
              }}
            >
              {networkEleData && selectedTab?.length > 0
                ? networkEleData
                    .filter((res) => res.title === selectedTab[0])
                    .map((res: any) => <MainCard res={res} />)
                : networkEleData?.map((res: any) => <MainCard res={res} />)}
            </Box>
            {isNoData && (
              <Alert
                severity="error"
                sx={{
                  background: "rgb(255 0 0 / 71%)",
                  fontWeight: "bolder",
                  fontSize: "16px",
                }}
              >
                No Data Found
              </Alert>
            )}
          </ThemeProvider>
        </Box>
        {/* {individualView ? (
          <div className="invidual-card-container d-flex mt-3">
            <div className="row">
              <div className="col-12">
                <div className="main-title">
                  {" "}
                  <GoArrowLeft
                    onClick={() => {
                      setIndividualView("");
                    }}
                    size={25}
                    // color={`${darkMode ? "white" : "black"}`}
                  />
                  {individualView}
                </div>
                <div className="main-card-individual" style={{ zIndex: 0 }}>
                  <div className="main-category-card">
                    <div className="main-category-container">
                      {selectedIndividualData &&
                      selectedIndividualData?.length &&
                      selectedIndividualData[0].oemVendors &&
                      selectedIndividualData[0].oemVendors.length > 0 ? (
                        <>
                          {(() => {
                            const elements: JSX.Element[] = [];
                            let i = 0;

                            while (
                              i < selectedIndividualData[0].oemVendors.length
                            ) {
                              if (
                                i <
                                selectedIndividualData[0].oemVendors.length - 1
                              ) {
                                const currentVendor =
                                  selectedIndividualData[0].oemVendors[i];
                                const nextVendor =
                                  selectedIndividualData[0].oemVendors[i + 1];
                                const currentVendorEligible =
                                  currentVendor.data.length === 2;
                                const nextVendorEligible =
                                  nextVendor.data.length === 2;

                                if (
                                  currentVendorEligible &&
                                  nextVendorEligible
                                ) {
                                  // Render the current and next vendor with 50% width each
                                  elements.push(
                                    <div
                                      className="row col"
                                      key={`combined-${i}`}
                                    >
                                      {[currentVendor, nextVendor].map(
                                        (vendor) => (
                                          <div
                                            className="main-category-item mb-4"
                                            style={{ zIndex: 0 }}
                                            key={vendor.oemVendor}
                                          >
                                            <h3 className="main-category-title">
                                              {vendor.oemVendor}
                                            </h3>
                                            <Row
                                              className="row-cols-auto main-category-sub"
                                              role="submenu"
                                            >
                                              {vendor.data.map((product) => (
                                                <Col
                                                  key={product.id}
                                                  className="noGrow"
                                                >
                                                  <div className="main-category-sub-card-single">
                                                    <div>
                                                      <div className="sub-card-title">
                                                        {
                                                          product.productNameNeInstances
                                                        }
                                                      </div>
                                                      <div
                                                        className={`sub-card-value ${
                                                          product.total
                                                            ? "sub-card-hasValue"
                                                            : ""
                                                        }`}
                                                      >
                                                        {product.total}
                                                      </div>
                                                    </div>
                                                  </div>
                                                </Col>
                                              ))}
                                            </Row>
                                          </div>
                                        )
                                      )}
                                    </div>
                                  );
                                  i += 2; // Skip the next 2 vendors
                                  continue;
                                }
                              }

                              const vendorsToCheck =
                                selectedIndividualData[0].oemVendors.slice(
                                  i,
                                  i + 4
                                );
                              const allFourEligible =
                                vendorsToCheck.length === 4 &&
                                vendorsToCheck.every(
                                  (vendor) => vendor.data.length <= 1
                                );

                              if (allFourEligible) {
                                // Render the first 4 vendors in one row with 25% width each
                                elements.push(
                                  <div
                                    className="row col"
                                    key={`combined-${i}`}
                                  >
                                    {vendorsToCheck.map((vendor) => (
                                      <div
                                        className="main-category-item mb-4"
                                        style={{ width: "25%", zIndex: 0 }}
                                        key={vendor.oemVendor}
                                      >
                                        <h3 className="main-category-title">
                                          {vendor.oemVendor}
                                        </h3>
                                        <Row
                                          className="row-cols-auto main-category-sub"
                                          role="submenu"
                                        >
                                          {vendor.data.map((product) => (
                                            <Col
                                              key={product.id}
                                              className="noGrow"
                                            >
                                              <div className="main-category-sub-card-single">
                                                <div>
                                                  <div className="sub-card-title">
                                                    {
                                                      product.productNameNeInstances
                                                    }
                                                  </div>
                                                  <div
                                                    className={`sub-card-value ${
                                                      product.total
                                                        ? "sub-card-hasValue"
                                                        : ""
                                                    }`}
                                                  >
                                                    {product.total}
                                                  </div>
                                                </div>
                                              </div>
                                            </Col>
                                          ))}
                                        </Row>
                                      </div>
                                    ))}
                                  </div>
                                );
                                i += 4; // Skip the next 4 vendors
                              } else {
                                const vendorsToCheckThree =
                                  selectedIndividualData[0].oemVendors.slice(
                                    i,
                                    i + 3
                                  );
                                const allThreeEligible =
                                  vendorsToCheckThree.length === 3 &&
                                  vendorsToCheckThree.every(
                                    (vendor) => vendor.data.length <= 1
                                  );

                                if (allThreeEligible) {
                                  // Render the next 3 vendors in one row with 33% width each
                                  elements.push(
                                    <div
                                      className="row col"
                                      key={`combined-${i}`}
                                    >
                                      {vendorsToCheckThree.map((vendor) => (
                                        <div
                                          className="main-category-item mb-4"
                                          style={{ width: "33%", zIndex: 0 }}
                                          key={vendor.oemVendor}
                                        >
                                          <h3 className="main-category-title">
                                            {vendor.oemVendor}
                                          </h3>
                                          <Row
                                            className="row-cols-auto main-category-sub"
                                            role="submenu"
                                          >
                                            {vendor.data.map((product) => (
                                              <Col
                                                key={product.id}
                                                className="noGrow"
                                              >
                                                <div className="main-category-sub-card-single">
                                                  <div>
                                                    <div className="sub-card-title">
                                                      {
                                                        product.productNameNeInstances
                                                      }
                                                    </div>
                                                    <div
                                                      className={`sub-card-value ${
                                                        product.total
                                                          ? "sub-card-hasValue"
                                                          : ""
                                                      }`}
                                                    >
                                                      {product.total}
                                                    </div>
                                                  </div>
                                                </div>
                                              </Col>
                                            ))}
                                          </Row>
                                        </div>
                                      ))}
                                    </div>
                                  );
                                  i += 3; // Skip the next 3 vendors
                                } else {
                                  const vendorsToCheckTwo =
                                    selectedIndividualData[0].oemVendors.slice(
                                      i,
                                      i + 2
                                    );
                                  const allTwoEligible =
                                    vendorsToCheckTwo.length === 2 &&
                                    vendorsToCheckTwo.every(
                                      (vendor) => vendor.data.length <= 1
                                    );

                                  if (allTwoEligible) {
                                    // Render the first 2 vendors in one row with 50% width each
                                    elements.push(
                                      <div
                                        className="row col"
                                        key={`combined-${i}`}
                                      >
                                        {vendorsToCheckTwo.map((vendor) => (
                                          <div
                                            className="main-category-item mb-4"
                                            style={{ zIndex: 0 }}
                                            key={vendor.oemVendor}
                                          >
                                            <h3 className="main-category-title">
                                              {vendor.oemVendor}
                                            </h3>
                                            <Row
                                              className="row-cols-auto main-category-sub"
                                              role="submenu"
                                            >
                                              {vendor.data.map((product) => (
                                                <Col
                                                  key={product.id}
                                                  className="noGrow"
                                                >
                                                  <div className="main-category-sub-card-individual-2">
                                                    <div>
                                                      <div className="sub-card-title">
                                                        {
                                                          product.productNameNeInstances
                                                        }
                                                      </div>
                                                      <div
                                                        className={`sub-card-value ${
                                                          product.total
                                                            ? "sub-card-hasValue"
                                                            : ""
                                                        }`}
                                                      >
                                                        {product.total}
                                                      </div>
                                                    </div>
                                                  </div>
                                                </Col>
                                              ))}
                                            </Row>
                                          </div>
                                        ))}
                                      </div>
                                    );
                                    i += 2; // Skip the next 2 vendors
                                  } else {
                                    // Render the current vendor alone with 100% width
                                    const currentVendor =
                                      selectedIndividualData[0].oemVendors[i];

                                    if (currentVendor?.data?.length === 3) {
                                      elements.push(
                                        <div
                                          className="row col"
                                          key={currentVendor.oemVendor}
                                        >
                                          <div
                                            className="main-category-item mb-4"
                                            style={{ width: "100%", zIndex: 0 }}
                                          >
                                            <h3 className="main-category-title">
                                              {currentVendor.oemVendor}
                                            </h3>
                                            <Row
                                              className="row-cols-auto main-category-sub"
                                              role="submenu"
                                            >
                                              {currentVendor.data.map(
                                                (product) => (
                                                  <Col
                                                    key={product.id}
                                                    className="noGrow"
                                                  >
                                                    <div className="main-category-sub-card-individual-3">
                                                      <div>
                                                        <div className="sub-card-title">
                                                          {
                                                            product.productNameNeInstances
                                                          }
                                                        </div>
                                                        <div
                                                          className={`sub-card-value ${
                                                            product.total
                                                              ? "sub-card-hasValue"
                                                              : ""
                                                          }`}
                                                        >
                                                          {product.total}
                                                        </div>
                                                      </div>
                                                    </div>
                                                  </Col>
                                                )
                                              )}
                                            </Row>
                                          </div>
                                        </div>
                                      );
                                    } else {
                                      elements.push(
                                        <div
                                          className="row col"
                                          key={currentVendor.oemVendor}
                                        >
                                          <div
                                            className="main-category-item mb-4"
                                            style={{ width: "100%", zIndex: 0 }}
                                          >
                                            <h3 className="main-category-title">
                                              {currentVendor.oemVendor}
                                            </h3>
                                            <Row
                                              className="row-cols-auto main-category-sub"
                                              role="submenu"
                                            >
                                              {currentVendor.data.map(
                                                (product) => (
                                                  <Col
                                                    key={product.id}
                                                    className="noGrow"
                                                  >
                                                    <div className="main-category-sub-card-individual">
                                                      <div>
                                                        <div className="sub-card-title">
                                                          {
                                                            product.productNameNeInstances
                                                          }
                                                        </div>
                                                        <div
                                                          className={`sub-card-value ${
                                                            product.total
                                                              ? "sub-card-hasValue"
                                                              : ""
                                                          }`}
                                                        >
                                                          {product.total}
                                                        </div>
                                                      </div>
                                                    </div>
                                                  </Col>
                                                )
                                              )}
                                            </Row>
                                          </div>
                                        </div>
                                      );
                                    }
                                    i++; // Move to the next vendor
                                  }
                                }
                              }
                            }

                            return elements;
                          })()}
                        </>
                      ) : (
                        ""
                      )}
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <div className="filter-container">
                  <FormControl
                    className="filter-card"
                    style={{ background: "white" }}
                  >
                    <FormLabel id="demo-checkbox-group-label">Market</FormLabel>
                    <FormGroup aria-labelledby="demo-checkbox-group-label">
                      <FormControlLabel
                        control={
                          <Checkbox
                            value=""
                            checked={
                              selectedValues.length ===
                              networkElementOpco.length
                            }
                            onChange={handleChange}
                          />
                        }
                        label="All"
                      />
                      {networkElementOpco &&
                        networkElementOpco.map(([key, value]) => (
                          <FormControlLabel
                            key={key}
                            control={
                              <Checkbox
                                value={key}
                                checked={selectedValues.includes(key)}
                                onChange={handleChange}
                              />
                            }
                            label={value}
                          />
                        ))}
                    </FormGroup>
                  </FormControl>
                </div>
          </div>
        ) : (
          <>
            {formattedGraphData?.length ? (
              <div className="row" style={{ marginBottom: "5rem !important" }}>
                {formattedGraphData?.map((data) => {
                  return (
                    <div className="col-md-6 card-col" style={{ zIndex: 0 }}>
                      <div
                        className="main-title"
                        onClick={(e) => {
                          // individual view
                          setIndividualView(data.verticalDescrption);
                          handleIndividualView(data.verticalDescrption);
                        }}
                      >
                        {data.verticalDescrption}
                        <>
                          {disableField?.includes(data.verticalDescrption) ? (
                            <FaPlus
                              onClick={(e) => {
                                e.stopPropagation();
                                // shrink view
                                setDisableField((prev) => {
                                  if (
                                    prev &&
                                    prev.includes(data.verticalDescrption)
                                  ) {
                                    return prev.filter(
                                      (prevData) =>
                                        prevData !== data.verticalDescrption
                                    );
                                  } else {
                                    return [...prev, data.verticalDescrption];
                                  }
                                });
                              }}
                              size={20}
                              color={"black"}
                            />
                          ) : (
                            <FaMinus
                              onClick={(e) => {
                                e.stopPropagation();
                                // shrink view
                                setDisableField((prev) => {
                                  if (
                                    prev &&
                                    prev.includes(data.verticalDescrption)
                                  ) {
                                    return prev.filter(
                                      (prevData) =>
                                        prevData !== data.verticalDescrption
                                    );
                                  } else {
                                    return [...prev, data.verticalDescrption];
                                  }
                                });
                              }}
                              size={20}
                              color={"black"}
                            />
                          )}
                        </>
                      </div>
                      {disableField?.includes(data.verticalDescrption) ? (
                        <></>
                      ) : (
                        <div className="main-card">
                          <div className="main-category-card">
                            <div className="main-category-container">
                              {data.oemVendors &&
                                data.oemVendors.length > 0 && (
                                  <>
                                    {(() => {
                                      const elements: JSX.Element[] = [];
                                      for (
                                        let i = 0;
                                        i < data.oemVendors.length;
                                        i++
                                      ) {
                                        const vendor = data.oemVendors[i];
                                        const nextVendor =
                                          data.oemVendors[i + 1];
                                        const isNextVendorEligible =
                                          nextVendor &&
                                          nextVendor.data.length <= 1;
                                        const isCurrentVendorEligible =
                                          vendor.data.length <= 1;

                                        if (
                                          isCurrentVendorEligible &&
                                          isNextVendorEligible
                                        ) {
                                          elements.push(
                                            <div
                                              className="row col"
                                              key={vendor.oemVendor}
                                            >
                                              <div
                                                className="main-category-item mb-4"
                                                style={{ zIndex: 0 }}
                                              >
                                                <h3 className="main-category-title">
                                                  {vendor.oemVendor}
                                                </h3>
                                                <Row
                                                  className="row-cols-auto main-category-sub"
                                                  role="submenu"
                                                >
                                                  {vendor.data.map(
                                                    (product) => (
                                                      <Col
                                                        key={product.id}
                                                        className="noGrow"
                                                      >
                                                        <div className="main-category-sub-card-single">
                                                          <div>
                                                            <div className="sub-card-title">
                                                              {
                                                                product.productNameNeInstances
                                                              }
                                                            </div>
                                                            <div
                                                              className={`sub-card-value ${
                                                                product.total
                                                                  ? "sub-card-hasValue"
                                                                  : ""
                                                              }`}
                                                            >
                                                              {product.total}
                                                            </div>
                                                          </div>
                                                        </div>
                                                      </Col>
                                                    )
                                                  )}
                                                </Row>
                                              </div>
                                              <div
                                                className="main-category-item mb-4"
                                                style={{ zIndex: 0 }}
                                              >
                                                <h3 className="main-category-title">
                                                  {nextVendor.oemVendor}
                                                </h3>
                                                <Row
                                                  className="row-cols-auto main-category-sub"
                                                  role="submenu"
                                                >
                                                  {nextVendor.data.map(
                                                    (product) => (
                                                      <Col
                                                        key={product.id}
                                                        className="noGrow"
                                                      >
                                                        <div className="main-category-sub-card-single">
                                                          <div>
                                                            <div className="sub-card-title">
                                                              {
                                                                product.productNameNeInstances
                                                              }
                                                            </div>
                                                            <div
                                                              className={`sub-card-value ${
                                                                product.total
                                                                  ? "sub-card-hasValue"
                                                                  : ""
                                                              }`}
                                                            >
                                                              {product.total}
                                                            </div>
                                                          </div>
                                                        </div>
                                                      </Col>
                                                    )
                                                  )}
                                                </Row>
                                              </div>
                                            </div>
                                          );
                                          i++; // Skip the next vendor
                                        } else {
                                          elements.push(
                                            <div
                                              className="row col"
                                              key={vendor.oemVendor}
                                            >
                                              <div
                                                className="main-category-item mb-4"
                                                style={{
                                                  width: "100%",
                                                  zIndex: 0,
                                                }}
                                              >
                                                <h3 className="main-category-title">
                                                  {vendor.oemVendor}
                                                </h3>
                                                <Row
                                                  className="row-cols-auto main-category-sub"
                                                  role="submenu"
                                                >
                                                  {vendor.data.map(
                                                    (product) => (
                                                      <Col
                                                        key={product.id}
                                                        className="noGrow"
                                                      >
                                                        <div className="main-category-sub-card">
                                                          <div>
                                                            <div className="sub-card-title">
                                                              {
                                                                product.productNameNeInstances
                                                              }
                                                            </div>
                                                            <div
                                                              className={`sub-card-value ${
                                                                product.total
                                                                  ? "sub-card-hasValue"
                                                                  : ""
                                                              }`}
                                                            >
                                                              {product.total}
                                                            </div>
                                                          </div>
                                                        </div>
                                                      </Col>
                                                    )
                                                  )}
                                                </Row>
                                              </div>
                                            </div>
                                          );
                                        }
                                      }
                                      return elements;
                                    })()}
                                  </>
                                )}
                            </div>
                          </div>
                        </div>
                      )}
                    </div>
                  );
                })}
              </div>
            ) : (
              <>
                {isNoData ? (
                  <Alert severity="error">No Data Found</Alert>
                ) : (
                  <></>
                )}
              </>
            )}
          </>
        )} */}
      </div>
    </>
  );
};

export default Chart2;
