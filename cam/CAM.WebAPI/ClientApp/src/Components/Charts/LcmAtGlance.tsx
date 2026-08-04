import React, { useState, useEffect, useRef, useCallback } from "react";
import "./chartStyle.css";
import Paper from "@mui/material/Paper";
import Stack from "@mui/material/Stack";
import Box from "@mui/material/Box";
import Grid from "@mui/material/Grid";
import Skeleton from "@mui/material/Skeleton";
import { styled } from "@mui/material/styles";
import { useLocation, useNavigate, useSearchParams } from "react-router-dom";
import { useAuth } from "../../Hook/useAuth";
import { Theme } from "@mui/material/styles";
import { createTheme, ThemeProvider } from "@mui/material/styles";
import {
  GetLCMAtGlanceGraph,
  GetLCMAtGlanceGraphApiResource,
  GetLCMAtGlancePAGraph,
  GetOpcoWisePercentage,
  GetOverAllPercentageBasedOnFilters,
  GetProductWisePercentage,
  GetSubnetworkWisePercentage,
} from "../../Redux/Action/LookUp/LCMAtGlanceGraph/LCMAtGlanceGraphAction";
import { useTheme } from "../../Context/ThemeContext";
import Typography from "@mui/material/Typography";
import Button from "@mui/material/Button";
import { BiSolidFilterAlt } from "react-icons/bi";
import { IoFilter } from "react-icons/io5";
import { FcInfo } from "react-icons/fc";
import FormControl from "@mui/material/FormControl";
import InputLabel from "@mui/material/InputLabel";
import Select, { SelectChangeEvent } from "@mui/material/Select";
import OutlinedInput from "@mui/material/OutlinedInput";
import MenuItem from "@mui/material/MenuItem";
import Tabs, { tabsClasses } from "@mui/material/Tabs";
import Tab from "@mui/material/Tab";
import Chip from "@mui/material/Chip";
import Divider from "@mui/material/Divider";
import Drawer from "@mui/material/Drawer";
import CustomSlides from "../Slider/CustomSlides";
import CustomEStackedPieChart from "../ECharts/CustomEStackedPieChart";
import CustomEStackedBarChart from "../ECharts/CustomEStackedBarChart";
import CustomEBarChart from "../ECharts/CustomEBarChart";
import Gauge from "./Gauge";
import GraphicLoading from "../ECharts/GraphicLoading";
import InputAdornment from "@mui/material/InputAdornment";
import IconButton from "@mui/material/IconButton";
import { IoClose } from "react-icons/io5";
import DatePicker from "react-datepicker";
import NoDataAnimation from "../ECharts/NoDataAnimation";
import GaugeCharts from "../ECharts/GaugeChart";
import { useSelector } from "react-redux";
import { RootState, rootStore } from "../../Redux/Store/rootStore";
import { LookUpGraphFilter } from "../../Model/LookUp/LookUpGenericModel";
import { yellow } from "@mui/material/colors";
import { safeNumber } from "../../Hook/Common";
import { DropdownInputComponent } from "../FormField";
import { MultiSelectCheckmarks, MultiSingleSelect } from "../MUISelect";
import Tooltip, { TooltipProps, tooltipClasses } from "@mui/material/Tooltip";
import { FormControlLabel, Switch } from "@mui/material";

const Item = styled(Paper)(({ theme }) => ({
  backgroundColor: "#fff",
  ...theme.typography.body2,
  padding: theme.spacing(1),
  textAlign: "center",
  color: theme.palette.text.secondary,
  ...theme.applyStyles("dark", {
    backgroundColor: "#1A2027",
  }),
}));

const HtmlTooltip = styled(({ className, ...props }: TooltipProps) => (
  <Tooltip {...props} classes={{ popper: className }} placement="right-start" />
))(({ theme }) => ({
  [`& .${tooltipClasses.tooltip}`]: {
    backgroundColor: "#f5f5f9",
    color: "rgba(0, 0, 0, 0.87)",
    minWidth: 220,
    fontSize: theme.typography.pxToRem(12),
    border: "1px solid #dadde9",
  },
}));

const FlexboxGapStack = () => {
  const [searchParams] = useSearchParams();
  const { isPermesso } = useAuth();
  const filterRes = (state: RootState) => state.graphFilterObject.FilterData;

  let getFilterData = useSelector(filterRes);
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
  const { darkMode, selectDarkMode } = useTheme();
  const theme = darkMode ? darkTheme : lightTheme;
  const navigate = useNavigate();
  const location = useLocation();
  const [filterHeading, setFilterHeading] = useState("");
  const [filterHeadingObj, setFilterHeadingObj] = useState<any>([]);
  const { reportId, isEdit } = location.state || {}; // Destructure state
  const [opcoLoader, setOpcoLoader] = useState<boolean>(false);
  const [paLoader, setPALoader] = useState<boolean>(false);
  const [productLoader, setProductLoader] = useState<boolean>(false);
  const [subnetworkLoader, setSubnetworkLoader] = useState<boolean>(false);
  const [gaugeLoader, setGaugeLoader] = useState<boolean>(false);
  const [filterLoader, setFilterLoader] = useState<boolean>(false);
  const [opcoGraphData, setOpcoGraphData] = useState<any>();
  const [paGraphDataDisplay, setPAGraphDataDisplay] = useState<any>();
  const [softwarePAGraphData, setSoftwarePAGraphData] = useState<any>();
  const [replacePAGraphData, setReplacePAGraphData] = useState<any>();
  const [modernizePAGraphData, setModernizePAGraphData] = useState<any>();
  const [productGraphData, setProductGraphData] = useState<any>();
  const [subnetworkGraphData, setSubnetworkGraphData] = useState<any>();
  const [verticalGraphData, setVerticalGraphData] = useState<any>();
  const [gaugeGraphData, setGaugeGraphData] = useState<any>();
  const [selectedOpco, setSelectedOpco] = useState<string[]>([]);
  const [selectedProduct, setSelectedProduct] = useState<any>(null);
  const [selectedProductImportance, setSelectedProductImportance] =
    useState<any>(null);
  const [selectedSubNetwork, setSelectedSubNetwork] = useState<any>(null);
  const [selectedPAStatus, setSelectedPAStatus] = useState("");
  const [selectedVertical, setSelectedVertical] = useState<string[]>([]);
  const [defaultVertical, setDefaultVertical] = useState<string[]>([]);
  const [LCMAtGlanceResource, setLCMAtGlanceResource] = useState<any>();
  const [LCMAtGlanceOpCo, setLCMAtGlanceOpCo] = useState<any>();
  const [LCMAtGlanceProduct, setLCMAtGlanceProduct] = useState<any>();
  const [LCMAtGlanceProductImportance, setLCMAtGlanceProductImportance] =
    useState<any>();
  const [LCMAtGlanceSubNetwork, setLCMAtGlanceSubNetwork] = useState<any>();
  const [LCMAtGlanceVertical, setLCMAtGlanceVertical] = useState<any>();
  const [showCanvas, setShowCanvas] = useState(false);
  const [selectedDate, setSelectedDate] = useState<Date | null | undefined>(
    undefined
  );
  const [filterFields, setFilterFields] = useState(null);
  const [isPageLoad, setIsPageLoad] = useState(true);
  const [isEOM, setIsEOM] = useState(false);
  const [plannedActivityResource, setPlannedActivityResource] = useState<
    { key: number; value: string; id: number }[] | null
  >(null);
  const [paDropDown, setPaDropDown] = useState<{
    key: number;
    value: string;
    id: number;
  } | null>(null);
  const [paPayload, setPaPayload] = useState<any>({});
  const isListenerAdded = useRef(false); // Track if the listener has already been added
  const [selectFilterOption, setSelectFilterOption] = useState("");
  const ITEM_HEIGHT = 48;
  const ITEM_PADDING_TOP = 8;
  const MenuProps = {
    PaperProps: {
      style: {
        maxHeight: ITEM_HEIGHT * 4.5 + ITEM_PADDING_TOP,
        width: 250,
      },
    },
  };

  const appliedFilters = ["All", "OpCo", "Product", "Subnetwork", "Vertical"];

  function getStyles(name: string, personName: string[], theme: Theme) {
    return {
      fontWeight: personName.includes(name)
        ? theme.typography.fontWeightMedium
        : theme.typography.fontWeightRegular,
    };
  }

  useEffect(() => {
    // Update Dark Mode on mount and cleanup on unmount
    selectDarkMode(true);

    if (searchParams.get("isEOM") === "Yes") {
      setIsEOM(true);
    }
    return () => {
      selectDarkMode(false);
    };
  }, []);

  useEffect(() => {
    // console.log("useEffect triggered");
    if (!isPermesso) return;
    else {
      const fetchData = async (filterObjData) => {
        const apiFunctions = [
          {
            api: getLCMAtGlanceGraphApiResource,
            setLoader: setFilterLoader,
            filterObj: {},
            name: "getLCMAtGlanceGraphApiResource",
          },
          {
            api: getGaugeGraphData,
            setLoader: setGaugeLoader,
            filterObj: filterObjData,
            name: "getGaugeGraphData",
          },
          {
            api: getOpcoGraphData,
            setLoader: setOpcoLoader,
            filterObj: filterObjData,
            name: "getOpcoGraphData",
          },
          // {
          //   api: searchParams.get("opcoId") ? getPAGraphData : null,
          //   setLoader: setPALoader,
          //   filterObj: filterObjData,
          //   name: "getPAGraphData",
          // },
          {
            api: getProductGraphData,
            setLoader: setProductLoader,
            filterObj: filterObjData,
            name: "getProductGraphData",
          },
          {
            api: getSubnetworkGraphData,
            setLoader: setSubnetworkLoader,
            filterObj: filterObjData,
            name: "getSubnetworkGraphData",
          },
        ];

        try {
          await Promise.all(
            apiFunctions.map(async ({ api, setLoader, name, filterObj }) => {
              if (api) {
                setLoader(true);
                try {
                  await api(filterObj || {});
                } catch (error) {
                  console.error(`Error in ${name}:`, error);
                } finally {
                  setLoader(false);
                }
              }
            })
          );
          // console.log("All API calls completed.");
        } catch (error) {
          console.error("Error in one or more API calls:", error);
        }
      };
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

        // console.log("transformedObj", transformedObj);
        if (transformedObj["productId"]) {
          setSelectedProduct(transformedObj["productId"]);
        }
        if (transformedObj["productimportanceId"]) {
          setSelectedProductImportance(transformedObj["productimportanceId"]);
        }
        if (transformedObj["supportedServicesId"]) {
          setSelectedSubNetwork(transformedObj["supportedServicesId"]);
        }
        if (transformedObj["verticalResponsibleId"]) {
          setSelectedVertical(transformedObj["verticalResponsibleId"]);
        }
        if (transformedObj["opcoId"]) {
          setSelectedOpco(transformedObj["opcoId"]);
        }
        if (transformedObj["selectedDate"]) {
          setSelectedDate(transformedObj["selectedDate"]);
        }
        if (transformedObj["isEOM"] === "Yes") {
          setIsEOM(true);
        }
        fetchData(transformedObj);
        setPaPayload(transformedObj);
      } else {
        fetchData({});
        setPaPayload({});
      }
    }
  }, [isPermesso, searchParams, isEOM]); // Dependency array with memoized callback

  const updateFilterHeading = (data) => {
    const paramsObj = Object.fromEntries(searchParams.entries());

    const transformedObj = Object.keys(paramsObj).reduce((acc, key) => {
      const value = paramsObj[key];

      if (key === "selectedDate") {
        acc[key] = String(value);
      } else {
        acc[key] = [safeNumber(value)];
      }

      return acc;
    }, {});
    let combinedString = "";

    const transformedObjKeys = Object.keys(transformedObj);
    const lastIndex = transformedObjKeys.length - 1;

    let hasAddedValue = false;

    const sourceMap = {
      opcoId: {
        title: "Opco",
        data: data?.allOpcos?.data,
        getLabel: (item) => item?.value,
      },
      verticalResponsibleId: {
        title: "Vertical",
        data: data?.allVerticalResponse,
        getLabel: (item) => item?.value,
      },
      productId: {
        title: "Product",
        data: data?.allProducts,
        getLabel: (item) => item?.value,
      },
      productimportanceId: {
        title: "Product Importance",
        data: data?.allProductImportances,
        getLabel: (item) => item?.value,
      },
      supportedServicesId: {
        title: "Subnetwork",
        data: data?.allSubNetworkBoundary,
        getLabel: (item) => item?.value,
      },
    };

    const result = Object.entries(paramsObj).map(([key, value]) => {
      const config = sourceMap[key];
      if (!config) return { title: key, value: [] };

      const ids = value.split(",").map((val) => safeNumber(val));

      const mappedValues = ids
        .map((id) => config.data?.find((item) => item.key === id))
        .filter(Boolean)
        .map(config.getLabel);
      return {
        title: config.title,
        value: mappedValues,
      };
    });
    // console.log(result);
    setFilterHeadingObj(
      result?.filter(
        (item) =>
          item.title !== "selectedDate" &&
          item.title !== "colorCode" &&
          item.title !== "isEOM"
      )
    );
    transformedObjKeys.forEach((key, index) => {
      if (key === "opcoId" && transformedObj["opcoId"]) {
        const opcoId = searchParams.get("opcoId");
        const opcoIdArray = opcoId ? opcoId.split(",").map(safeNumber) : [];

        // Find matching Opcos from data?.allOpcos
        const matchedOpcos = opcoIdArray
          ?.map((id) => data?.allOpcos?.find((opco) => opco.key === id))
          .filter((opco) => opco); // Filter out undefined results

        // Append Opcos to the combined string
        if (matchedOpcos.length > 0) {
          if (hasAddedValue) combinedString += " , ";
          combinedString +=
            "Opco: " +
            matchedOpcos
              .map((opco, index) => {
                const isLast = index === matchedOpcos.length - 1;
                return opco.value + (isLast ? "" : ", ");
              })
              .join("");
          hasAddedValue = true;
        }
      } else if (
        key === "verticalResponsibleId" &&
        transformedObj["verticalResponsibleId"]
      ) {
        const verticalResponsibleId = searchParams.get("verticalResponsibleId");
        const verticalIdArray = verticalResponsibleId
          ? verticalResponsibleId.split(",").map(safeNumber)
          : [];

        const matchedVertical = verticalIdArray
          ?.map((id) =>
            data?.allVerticalResponse?.find((vertical) => vertical.key === id)
          )
          .filter((vertical) => vertical);

        if (matchedVertical.length > 0) {
          if (hasAddedValue) combinedString += " , ";
          combinedString +=
            "Vertical: " +
            matchedVertical
              .map((vertical, index) => {
                const isLast = index === matchedVertical.length - 1;
                return vertical.value + (isLast ? "" : ", ");
              })
              .join("");
          hasAddedValue = true;
        }
      } else if (key === "productId" && transformedObj["productId"]) {
        const product = data?.allProducts?.find(
          (product) => product.key === safeNumber(transformedObj["productId"])
        );
        if (product) {
          if (hasAddedValue) combinedString += " , ";
          combinedString += "Product: " + product.value;
          hasAddedValue = true;
        }
      } else if (
        key === "productimportanceId" &&
        transformedObj["productimportanceId"]
      ) {
        const product = data?.allProductImportances?.find(
          (product) =>
            product.key === safeNumber(transformedObj["productimportanceId"])
        );
        if (product) {
          if (hasAddedValue) combinedString += " , ";
          combinedString += "Product Importance: " + product.value;
          hasAddedValue = true;
        }
      } else if (
        key === "supportedServicesId" &&
        transformedObj["supportedServicesId"]
      ) {
        const service = data?.allSubNetworkBoundary?.find(
          (service) =>
            service.key === safeNumber(transformedObj["supportedServicesId"])
        );
        if (service) {
          if (hasAddedValue) combinedString += " , ";
          combinedString += "SubNetwork: " + service.value;
          hasAddedValue = true;
        }
      }
    });

    if (!hasAddedValue) {
      combinedString = "";
    }

    setFilterHeading(combinedString);
  };

  // useEffect(() => {
  //   if (searchParams && searchParams["size"] !== 0){
  //     setIsPageLoad(false);
  //   }
  // },[searchParams])

  const getLCMAtGlanceGraphApiResource = async () => {
    const LCMAtGlanceGraphApiResource = await GetLCMAtGlanceGraphApiResource(
      isPageLoad
    );
    setPlannedActivityResource(
      LCMAtGlanceGraphApiResource?.ResultDtoCreate?.data
        ?.allPlannedActivityResources ?? []
    );
    setPaDropDown(
      LCMAtGlanceGraphApiResource?.ResultDtoCreate?.data?.allPlannedActivityResources?.filter(
        (res: any) => res.id == 1
      )[0] ?? null
    );
    if (searchParams && searchParams["size"] !== 0) {
      updateFilterHeading(LCMAtGlanceGraphApiResource?.ResultDtoCreate?.data);
    }
    if (LCMAtGlanceGraphApiResource.ResultDtoCreate) {
      setSelectedVertical(
        LCMAtGlanceGraphApiResource?.ResultDtoCreate?.data?.filterVerticalRespone?.data?.map(
          (res) => res.key
        ) ?? []
      );
      setDefaultVertical(
        LCMAtGlanceGraphApiResource?.ResultDtoCreate?.data?.filterVerticalRespone?.data?.map(
          (res) => res.key
        ) ?? []
      );
      const formattedtLCMAtGlanceApiResource = Object.entries(
        LCMAtGlanceGraphApiResource.ResultDtoCreate
      );
      setLCMAtGlanceResource(formattedtLCMAtGlanceApiResource);
    }
    if (LCMAtGlanceGraphApiResource?.ResultDtoCreate?.data?.allOpcos?.data) {
      const formattedtLCMAtGlanceApiOpCo = Object.entries(
        LCMAtGlanceGraphApiResource?.ResultDtoCreate?.data?.allOpcos?.data
      );
      setLCMAtGlanceOpCo(formattedtLCMAtGlanceApiOpCo);
    }
    if (LCMAtGlanceGraphApiResource?.ResultDtoCreate?.data?.allProducts) {
      const formattedtLCMAtGlanceApiProduct = Object.entries(
        LCMAtGlanceGraphApiResource?.ResultDtoCreate?.data?.allProducts
      );

      setLCMAtGlanceProduct(formattedtLCMAtGlanceApiProduct);
    }
    if (
      LCMAtGlanceGraphApiResource?.ResultDtoCreate?.data?.allProductImportances
    ) {
      const formattedtLCMAtGlanceApiProduct = Object.entries(
        LCMAtGlanceGraphApiResource?.ResultDtoCreate?.data
          ?.allProductImportances
      );

      setLCMAtGlanceProductImportance(formattedtLCMAtGlanceApiProduct);
    }
    if (
      LCMAtGlanceGraphApiResource?.ResultDtoCreate?.data?.allSubNetworkBoundary
    ) {
      const formattedtLCMAtGlanceApiSubNetwork = Object.entries(
        LCMAtGlanceGraphApiResource?.ResultDtoCreate?.data
          ?.allSubNetworkBoundary
      );
      setLCMAtGlanceSubNetwork(formattedtLCMAtGlanceApiSubNetwork);
    }
    if (
      LCMAtGlanceGraphApiResource?.ResultDtoCreate?.data?.allVerticalResponse
    ) {
      const formattedtLCMAtGlanceApiVertical = Object.entries(
        LCMAtGlanceGraphApiResource?.ResultDtoCreate?.data?.allVerticalResponse
      );
      setLCMAtGlanceVertical(formattedtLCMAtGlanceApiVertical);
    }
  };
  // console.log("plannedActivityResource", plannedActivityResource);
  const getOpcoGraphData = async (filters) => {
    setOpcoLoader(true);
    const graphData: any = await GetOpcoWisePercentage(
      isPageLoad,
      filters,
      searchParams.get("isEOM") === "Yes" ? false : !isEOM
    );
    if (graphData?.data) {
      setVerticalGraphData(
        graphData?.data?.verticalFilterValueBasedOnFilter ?? []
      );
      setOpcoGraphData(
        graphData?.data?.opcoWisePercentage?.map((data) => [
          data?.opCoDescrption,
          [
            safeNumber(data?.greenCompatibilityPercentage),
            safeNumber(data?.greenNodeCount),
            safeNumber(data?.amberCompatibilityPercentage),
            safeNumber(data?.amberNodeCount),
            safeNumber(data?.redCompatibilityPercentage),
            safeNumber(data?.redNodeCount),
          ],
        ]) ?? []
      );
      // console.log("OpcoGraphData", graphData?.data?.opcoWisePercentage);
    }
    setOpcoLoader(false);
  };

  const transformedData =
    opcoGraphData?.map(([name, values]) => ({
      category: name,
      green: values[0],
      green_node_count: values[1],
      amber: values[2],
      amber_node_count: values[3],
      red: values[4],
      red_node_count: values[5],
    })) ?? [];

  const EosTransformedData =
    (gaugeGraphData !== undefined &&
      gaugeGraphData?.colorCompatability?.length > 0 &&
      gaugeGraphData?.colorCompatability?.map((compaData: any) => ({
        category: compaData.year,
        green: compaData.greenNodePercentage,
        red: compaData.redNodePercentage,
      }))) ??
    [];

  useEffect(() => {
    if (paDropDown !== null && isPermesso && searchParams.get("opcoId")) {
      callPAGraphApi();
    }
  }, [paDropDown]);

  const callPAGraphApi = async () => {
    setPALoader(true);
    try {
      const results = await GetLCMAtGlancePAGraph(
        {
          ...paPayload,
          PlannedActivityResourceRuleId:
            paDropDown !== null ? [paDropDown.id] : [],
        },
        searchParams.get("isEOM") === "Yes" ? false : !isEOM
      );
      setPAGraphDataDisplay(results?.data);
    } catch (error) {
      console.error("Error fetching PA graph data:", error);
    } finally {
      setPALoader(false);
    }
  };

  const getPAGraphData = async (filters) => {
    setPALoader(true);

    const activities = [
      { key: 1, value: "Software Solution", setter: setSoftwarePAGraphData },
      { key: 14, value: "Modernise", setter: setModernizePAGraphData },
      { key: 9, value: "Replace Solution", setter: setReplacePAGraphData },
    ];

    try {
      const results = await Promise.all(
        activities.map((activity) =>
          GetLCMAtGlancePAGraph(
            {
              ...filters,
              PlannedActivityResourceRuleId: [activity.key],
            },
            searchParams.get("isEOM") === "Yes" ? false : !isEOM
          )
        )
      );

      results.forEach((graphData, index) => {
        if (graphData?.data) {
          activities[index].setter(graphData.data);
          // console.log(`${activities[index].value}GraphData`, graphData.data);
        }
      });
    } catch (error) {
      console.error("Error fetching PA graph data:", error);
    } finally {
      setPALoader(false);
    }
  };

  const getProductGraphData = async (filters) => {
    setProductLoader(true);
    const graphData: any = await GetProductWisePercentage(
      isPageLoad,
      filters,
      searchParams.get("isEOM") === "Yes" ? false : !isEOM
    );
    if (graphData?.data) {
      setVerticalGraphData(
        graphData?.data?.verticalFilterValueBasedOnFilter ?? []
      );
      setProductGraphData(graphData?.data?.productWisePercentage ?? []);
      // console.log("ProductGraphData", graphData?.data?.productWisePercentage);
    }
    setProductLoader(false);
  };

  const getSubnetworkGraphData = async (filters) => {
    setSubnetworkLoader(true);
    const graphData: any = await GetSubnetworkWisePercentage(
      isPageLoad,
      filters,
      searchParams.get("isEOM") === "Yes" ? false : !isEOM
    );
    if (graphData?.data) {
      setVerticalGraphData(
        graphData?.data?.verticalFilterValueBasedOnFilter ?? []
      );
      setSubnetworkGraphData(graphData?.data?.subnetworkWisePercentage ?? []);
      // console.log(
      //   "SubnetworkGraphData",
      //   graphData?.data?.subnetworkWisePercentage
      // );
    }
    setSubnetworkLoader(false);
  };

  const getGaugeGraphData = async (filters) => {
    console.log("searchParams.ge", searchParams.get("isEOM"));
    setGaugeLoader(true);
    const graphData: any = await GetOverAllPercentageBasedOnFilters(
      isPageLoad,
      filters,
      searchParams.get("isEOM") === "Yes" ? false : !isEOM
    );
    if (graphData?.data) {
      setGaugeGraphData(graphData?.data?.overAllOpcoPercentage ?? []);
    }
    setGaugeLoader(false);
  };

  const handleSelectFilter = (event: SelectChangeEvent) => {
    setSelectFilterOption(event.target.value as string);
  };

  const OpCoGraph = React.memo(() => {
    const handleBarClick = (data) => {
      // console.log("Bar clicked:", data);
      const getId = LCMAtGlanceOpCo?.filter(
        (res) => res[1].value === data?.name
      )?.[0]?.[1];
      if (getId?.key) {
        const colorMap = {
          "#FFFF00": "amber", // Yellow
          "#00c300": "green", // Green
        };

        const colorCode = colorMap[data?.color] || "red"; // Default to "red" if no match
        let searchValues: any = {};
        if (searchParams && searchParams["size"] !== 0) {
          const paramsObj = Object.fromEntries(searchParams?.entries());
          searchValues = Object.keys(paramsObj).reduce((acc, key) => {
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
        }
        const url = `/lcmatglance/?opcoId=${getId.key}&colorCode=${colorCode}${
          searchValues && searchValues?.selectedDate
            ? `&selectedDate=${searchValues?.selectedDate}`
            : ""
        }&isEOM=${isEOM ? "Yes" : "No"}`;
        window.open(url, "_blank");
      }
    };
    const handleLegendClick = (data: { name: string; isSelected: boolean }) => {
      console.log("Legend clicked:", data);
    };
    return (
      <div
        style={{
          justifyContent: "center",
          display: "block",
          height: "100%",
          width: "100%",
        }}
      >
        <>
          <Paper
            style={{
              padding: "7px",
              backgroundColor: `${darkMode ? "white" : "#e9ecef"}`,
              color: `${darkMode ? "black" : ""}`,
              fontWeight: "bold !important",
            }}
          >
            <Typography sx={{ fontWeight: "bold" }}>OpCo Compliance</Typography>
          </Paper>
          {opcoLoader ? (
            <GraphicLoading
              height={"76.6vh"}
              isDarkMode={theme}
              isLoading={true}
            />
          ) : (
            <CustomEBarChart
              width="100vh"
              height="80vh"
              isDarkMode={darkMode}
              onBarClick={handleBarClick}
              onLegendClick={handleLegendClick}
              namesList={transformedData?.map((data) => data?.category) ?? []}
              seriesData={[
                { name: "Green", color: "#00c300" },
                { name: "Amber", color: "#FFFF00" },
                { name: "Red", color: "#e60000" },
              ].map(({ name, color }) => ({
                name: name,
                type: "bar",
                data: transformedData?.map((data) => ({
                  value: data[`${name.toLowerCase()}`],
                  nodeCount: data[`${name.toLowerCase()}_node_count`],
                })),
                label: {
                  show: true, // Enable label on top of the bar
                  position: "top", // Position the label at the top of the bar
                  formatter: "{c}%", // Display the value of the bar
                  fontSize: 12, // Optional: Adjust font size
                  color: darkMode ? "white" : "black",
                },
                itemStyle: {
                  color: color,
                },
              }))}
              tooltip={{
                trigger: "item", // Trigger tooltip for all series in the same category
                axisPointer: {
                  type: "shadow", // Display shadow to highlight the category
                },
                formatter: (params) => {
                  // Determine if we are hovering over a category (multiple bars) or a single item (one bar)
                  const isHoveringOnCategory = params.length > 1;
                  const triggerType = isHoveringOnCategory ? "axis" : "item";
                  // console.log(params);
                  if (params?.length > 1) {
                    // Combine data from all bars for the hovered category

                    const header = `<div style="font-family: Arial, sans-serif;font-size: 12px;line-height: 1.5;text-align: left; min-width: 7rem;">
                        <div style="margin-bottom: 10px;padding-bottom: 6px;display: flex;justify-content: space-between;align-items: center; border-bottom-style: ridge;">
                          <span style="font-weight: bold;color: #666;">${params[0].name}</span>
                        </div>`;

                    // Iterate over each bar's data
                    const body = params
                      .map((param) => {
                        const { seriesName, data } = param;
                        return `
                            <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 5px;">
                              <span style="font-weight: bold; color: ${param.color};"></span>
                              <span>Nodes: ${data.nodeCount}</span>
                              <span style="background: #444;color: #fff;padding: 2px 5px;border-radius: 3px;margin-left: 8px;">
                                ${data.value}%
                              </span>
                            </div>`;
                      })
                      .join("");

                    const footer = `</div>`;

                    // Return the combined tooltip content
                    return `${header + body + footer}`;
                  } else {
                    const { name, value, seriesName, data } = params; // Use params[0] to access the hovered item
                    // <span style="background: #444;color: #fff;padding: 2px 5px;border-radius: 3px;">${data.value}%</span>
                    return `<div style="font-family: Arial, sans-serif;font-size: 12px;line-height: 1.5;text-align: left; min-width: 7rem;">
                          <div style="margin-bottom: 10px;padding-bottom: 6px;display: flex;justify-content: space-between;align-items: center; border-bottom-style: ridge;">
                            <span style="font-weight: bold;color: #666;">${name}</span>
                          </div>
                          <div style="display: flex;align-items: center;width: fit-content">
                            <span style="font-weight: bold; padding-right: 2px;">Percentage : </span>
                            <span style="font-weight: bolder; ">${data.value}%</span>
                          </div>
                          <div style="display: flex;align-items: center;width: fit-content">
                            <span style="font-weight: bold; padding-right: 2px;">Nodes : </span>
                            <span style="font-weight: bolder; ">${data.nodeCount}</span>
                          </div>
                        </div>`;
                  }
                },
              }}
              legend={{
                data: !isEOM ? ["Green", "Red"] : ["Green", "Amber", "Red"],
                itemGap: 15,
                top: "2%",
                formatter: (name) => {
                  const customNames = {
                    Green: `${!isEOM ? "EOS" : "EOM"} > ${
                      selectedDate && searchParams.get("selectedDate")
                        ? `${new Date(selectedDate).getDate()}/${
                            new Date(selectedDate).getMonth() + 1
                          }/${new Date(selectedDate).getFullYear()}`
                        : "Today"
                    }`,
                    ...(!isEOM && {
                      Amber: `EOS > ${
                        selectedDate && searchParams.get("selectedDate")
                          ? `${new Date(selectedDate).getDate()}/${
                              new Date(selectedDate).getMonth() + 1
                            }/${new Date(selectedDate).getFullYear()}`
                          : "Today"
                      } > EOM`,
                    }),
                    Red: `${
                      selectedDate && searchParams.get("selectedDate")
                        ? `${new Date(selectedDate).getDate()}/${
                            new Date(selectedDate).getMonth() + 1
                          }/${new Date(selectedDate).getFullYear()}`
                        : "Today"
                    } > EOS`,
                  };
                  return customNames[name] || name; // Return the custom name if available, otherwise the original name
                },
              }}
              grid={{
                top: "14%",
                left: "5%",
                right: "6%",
                bottom: "0%",
                height: "65%",
                containLabel: true,
              }}
              yAXis={[
                {
                  type: "value",
                  min: 0,
                  max: 100,
                },
              ]}
              xAXis={[
                {
                  type: "category",
                  data: transformedData?.map((data) => data.category) ?? [],
                },
              ]}
              dataZoom={[
                {
                  show: true,
                  start: 0,
                  end: 50,
                  top: "82%",
                  showDetail: false,
                },
                {
                  show: false,
                  type: "inside",
                  start: 0,
                  end: 4,
                },
                {
                  show: false,
                  yAxisIndex: 0,
                  filterMode: "empty",
                  height: "65%",
                  width: 30,
                  showDataShadow: false,
                  showDetail: false,
                  left: "95%",
                },
              ]}
            />
          )}
        </>
      </div>
    );
  });

  const PAGraphs = ({ data, title }) => {
    const isPA = paDropDown?.id === 15 ? true : false;
    const updatedData = data?.paGraphDataDisplay ?? null;
    const graphRecord = !isPA
      ? data?.paGraphDataDisplay?.paRecords
      : [
          ...data?.paGraphDataDisplay?.lcmWithOutPaEntity,
          ...data?.paGraphDataDisplay?.paRecords,
        ];
    const colorCode = searchParams.get("colorCode");
    const legendSelected = {
      Green: !colorCode || colorCode === "green", // If no colorCode or if it's "green"
      ...(!isEOM && { Amber: !colorCode || colorCode === "amber" }), // If no colorCode or if it's "amber"
      Red: !colorCode || colorCode === "red", // If no colorCode or if it's "red"
    };
    const seriesData = graphRecord
      ? graphRecord?.map((record: any) => ({
          name: record.paCurrentDCName,
          subName: record.paPlannedDCName ?? undefined,
          nodesCount: record.nodesCount ?? 0,
          plannedCompletion: record.plannedCompletion ?? "",
          paPlannedDCNodesCount: record.paPlannedDCNodesCount ?? 0,
          value: isPA ? record.nodesCount : record.paDelivertyStatusDesc,
        }))
      : [];
    const categories = updatedData
      ? !isPA && updatedData.paRelatedDropDown?.deliveryStatus?.[0]
        ? updatedData.paRelatedDropDown.deliveryStatus[0].map(
            (res) => res.value
          ) || []
        : Array.from({ length: 50 }, (_, i) => i + 1)
      : [];

    // Function to generate random hex colors
    const generateRandomColor = () => {
      const letters = "0123456789ABCDEF";
      let color = "#";
      for (let i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
      }
      return color;
    };
    // Generate a list of unique colors for the series updatedData
    const generateUniqueColors = (count: number) => {
      const colors = new Set<string>();
      while (colors.size < count) {
        colors.add(generateRandomColor());
      }
      return Array.from(colors);
    };

    // Generate the unique colors for the bars
    const colors = generateUniqueColors(graphRecord?.length + 10);

    const [XAxisWidth, setXAxisWidth] = useState<any>(700);

    const [selectedCategory, setSelectedCategory] = useState<string[]>(
      categories || []
    ); // Default to show all categories
    const [highlightedCategory, setHighlightedCategory] = useState<
      string | null
    >(null); // Track highlighted label index
    const [highlightedCategoryIndex, setHighlightedCategoryIndex] =
      useState<any>(null); // Track highlighted label index

    // Now, calculate the count of occurrences for each category (1, 2, 3, etc.)
    const categoryCounts = Array.from(
      { length: selectedCategory?.length || 0 },
      () => 0
    );
    const seriData = [
      { name: "Green", color: "#00c300" },
      ...(isEOM ? [{ name: "Amber", color: "#FFFF00" }] : []),
      { name: "Red", color: "#e60000" },
    ]?.map(({ name, color }) => ({
      name: name,
      type: "bar",
      data: graphRecord?.map((record, index) => {
        const value =
          record.compatibilityColorCode === name?.toLowerCase() &&
          selectedCategory &&
          selectedCategory?.includes(
            isPA ? record.nodesCount : record.paDelivertyStatusDesc
          )
            ? selectedCategory?.indexOf(
                isPA ? record.nodesCount : record.paDelivertyStatusDesc
              ) + 1
            : 0;
        // const trueCount = legendSelected ? Object.values(legendSelected).filter(Boolean).length : 10;
        // const positionHorizonal = trueCount === 3 ? -10 : -7;
        const getMonthYear = (dateStr: string) => {
          const [, month, year] = dateStr?.split("/");
          return `${month}/${year}`;
        };
        const formatToMonthYear = (dateStr: string) => {
          const [day, month, year] = dateStr?.split("/");

          // Create a Date object using yyyy-mm-dd (ISO format)
          const date = new Date(`${year}-${month}-${day}`);

          // Use Intl.DateTimeFormat for month name
          const formatted = new Intl.DateTimeFormat("en-GB", {
            month: "short",
            year: "numeric",
          }).format(date);

          return formatted;
        };
        return {
          value,
          itemStyle: { color: colors[index] },
          label: {
            show: value > 0,
            position: [XAxisWidth, 2],
            formatter: function (params) {
              return record?.plannedCompletion
                ? formatToMonthYear(record?.plannedCompletion)
                : "";
            },
            color: "white",
            fontWeight: "bolder",
            align: "center",
          },
        };
      }),
      tooltip: { show: true },
      cursor: "default",
      itemStyle: {
        color: color,
        borderRadius: [0, 5, 0, 0],
      },
    }));
    // Map the updatedData to corresponding indices based on the dynamic categories
    const categoryIndices = seriesData?.map((item) =>
      selectedCategory?.includes(item.value)
        ? selectedCategory.indexOf(item.value) + 1
        : 0
    );
    categoryIndices.forEach((index) => {
      categoryCounts[index - 1] += 1; // Increment the count for the corresponding category index (adjusted for 0-based index)
    });
    const handleXAxisLabel = (labelIndex, labelValue) => {
      let newLabelValue = isPA ? safeNumber(labelValue) : labelValue;
      if (highlightedCategory == newLabelValue) {
        // If the clicked label is already highlighted, reset to show all categories
        setSelectedCategory(categories || []);
        setHighlightedCategory(null); // Remove highlighting
        setHighlightedCategoryIndex(null); // Remove highlighting
      } else {
        // If a new label is clicked, show only that category
        setSelectedCategory([newLabelValue]);
        setHighlightedCategory(newLabelValue); // Highlight the clicked label
        setHighlightedCategoryIndex(labelIndex); // Remove highlighting
      }
    };
    return (
      <div
        style={{
          justifyContent: "center",
          display: "block",
          height: "100%",
          width: "100%",
        }}
      >
        <>
          <Paper
            style={{
              padding: "7px",
              backgroundColor: `${darkMode ? "white" : "#e9ecef"}`,
              color: `${darkMode ? "black" : ""}`,
              fontWeight: "bold !important",
            }}
          >
            <Typography sx={{ fontWeight: "bold" }}>
              {"Planned Activity Status - " + paDropDown?.value}
            </Typography>
          </Paper>
          {paLoader ? (
            <GraphicLoading
              height={"76.6vh"}
              isDarkMode={theme}
              isLoading={true}
            />
          ) : graphRecord?.length === 0 ? (
            <NoDataAnimation
              height={"76.6vh"}
              isDarkMode={theme}
              message={"No Data Found"}
            />
          ) : (
            <CustomEBarChart
              width="100vh"
              height="80vh"
              isDarkMode={darkMode}
              seriesData={seriData}
              setXAxisWidth={setXAxisWidth}
              tooltip={{
                trigger: "axis",
                position: "right",
                confine: true,
                formatter: (params: any) => {
                  const yAxisLabel = params[0].name;
                  const value = params[0].value;

                  // Find the matching subName from seriesData
                  const matchingData = seriesData.find(
                    (item) => item.name === yAxisLabel
                  );
                  const subName = matchingData
                    ? matchingData.subName
                    : undefined;
                  const date = matchingData
                    ? matchingData.plannedCompletion
                    : "";
                  const nodeCount = matchingData ? matchingData.nodesCount : 0;
                  const paNodeCount = matchingData
                    ? matchingData.paPlannedDCNodesCount
                    : 0;
                  // Replace <b class="text-lowercase"> with custom text for emphasis
                  const plainLabel = yAxisLabel
                    .replace(
                      /<b class="text-lowercase">/gi,
                      "<span style='font-weight:bold;'>"
                    )
                    .replace(/<\/b>/gi, "</span>");

                  return `<div style="font-family: Arial, sans-serif;font-size: 16px;line-height: 1.5;text-align: left; width: 30rem;">
                      <div style="display: block;align-items: center;">
                        <span style="font-weight: bold; padding-right: 2px;">Current DC : </span>
                        <span style="max-width: 250px; white-space: normal; word-wrap: break-word;">${yAxisLabel} - </span>
                        <span style="padding-right: 2px; padding-left: 2px;">(Node:${nodeCount}) </span>
                      </div>
                      ${
                        subName !== "undefined" &&
                        subName !== undefined &&
                        subName !== ""
                          ? `<div style="display: block;align-items: center;">
                        <span style="font-weight: bold; padding-right: 2px;">Planned DC : </span>
                        <span style="max-width: 250px; white-space: normal; word-wrap: break-word;">${subName} - </span>
                        <span style="padding-right: 2px; padding-left: 2px;">(Node:${paNodeCount}) </span>
                      </div>`
                          : ""
                      }
                      ${
                        date !== "undefined" &&
                        date !== undefined &&
                        date !== ""
                          ? `<div style="display: block;align-items: center;">
                        <span style="font-weight: bold; padding-right: 2px;">Planned Completion Date : </span>
                        <span style="max-width: 250px; white-space: normal; word-wrap: break-word;">${date}</span>
                      </div>`
                          : ""
                      }
                    </div>`;
                },
                axisPointer: {
                  type: "shadow",
                },
              }}
              legend={{
                data: !isEOM ? ["Green", "Red"] : ["Green", "Amber", "Red"],
                itemGap: 15,
                top: "2%",
                formatter: (name) => {
                  const customNames = {
                    Green: `${!isEOM ? "EOS" : "EOM"}  > ${
                      selectedDate && searchParams.get("selectedDate")
                        ? `${new Date(selectedDate).getDate()}/${
                            new Date(selectedDate).getMonth() + 1
                          }/${new Date(selectedDate).getFullYear()}`
                        : "Today"
                    }`,
                    ...(isEOM && {
                      Amber: `EOS > ${
                        selectedDate && searchParams.get("selectedDate")
                          ? `${new Date(selectedDate).getDate()}/${
                              new Date(selectedDate).getMonth() + 1
                            }/${new Date(selectedDate).getFullYear()}`
                          : "Today"
                      } > EOM`,
                    }),
                    Red: `${
                      selectedDate && searchParams.get("selectedDate")
                        ? `${new Date(selectedDate).getDate()}/${
                            new Date(selectedDate).getMonth() + 1
                          }/${new Date(selectedDate).getFullYear()}`
                        : "Today"
                    } > EOS`,
                  };
                  return customNames[name] || name; // Return the custom name if available, otherwise the original name
                },
                selected: legendSelected,
              }}
              grid={{
                top: "14%",
                left: "5%",
                right: "12%",
                bottom: "0%",
                height: "65%",
                containLabel: true,
              }}
              yAXis={[
                {
                  type: "category",
                  data: seriesData
                    .sort((a, b) => (a.name < b.name ? -1 : 1)) // Sort in descending order
                    .map((item) => item.name),
                  tooltip: { show: true },
                  silent: false, // Enable mouse events on xAxis
                  triggerEvent: true, // Enable event triggering
                  axisLabel: {
                    formatter: (label: string) => label.split("<b ")[0], // Trim long labels if necessary,
                    triggerEvent: true,
                    rich: {
                      normal: {
                        cursor: "default", // Force normal cursor instead of pointer
                      },
                    },
                  },
                },
              ]}
              xAXis={[
                {
                  type: "value",
                  name: isPA ? "Node" : "",
                  nameTextStyle: {
                    fontWeight: "bold",
                    color: "#fff",
                    marginRight: "4px",
                  },
                  data: selectedCategory,
                  min: 0,
                  interval: 1,
                  max: selectedCategory?.length,
                  silent: false, // Enable mouse events on xAxis
                  triggerEvent: true, // Enable event triggering
                  axisLabel: {
                    formatter: function (value) {
                      // Map numeric x-axis values to the corresponding dynamic category names
                      if (value >= 1 && value <= selectedCategory.length) {
                        return selectedCategory[value - 1]; // Adjusted for 1-based index
                      }
                      return ""; // Default case for other values
                    },
                    color: "#fff",
                    fontWeight: "bold",
                  },
                },
              ]}
              onAxisLabelClick={(index, label) =>
                handleXAxisLabel(index, label)
              }
              dataZoom={[
                {
                  show: true,
                  top: "82%",
                  start: 0,
                  end: isPA ? (selectedCategory?.length === 1 ? 100 : 10) : 100,
                  showDetail: false,
                },
                {
                  show: false,
                  type: "inside",
                },
                {
                  show: true,
                  yAxisIndex: 0,
                  filterMode: "empty",
                  height: "65%",
                  width: 30,
                  start: 0,
                  end: 100,
                  showDataShadow: false,
                  showDetail: false,
                  left: "92%",
                },
              ]}
            />
          )}
        </>
      </div>
    );
  };

  const ProductGraph = () => {
    return (
      <div
        style={{
          justifyContent: "center",
          display: "block",
          height: "100%",
          width: "100%",
        }}
      >
        <>
          <Paper
            style={{
              padding: "7px",
              backgroundColor: `${darkMode ? "white" : "#e9ecef"}`,
              color: `${darkMode ? "black" : ""}`,
              fontWeight: "bold !important",
            }}
          >
            <Typography sx={{ fontWeight: "bold" }}>
              {"Product Compliance"}
            </Typography>
          </Paper>
          {productLoader ? (
            <GraphicLoading
              height={"76.6vh"}
              isDarkMode={theme}
              isLoading={true}
            />
          ) : productGraphData?.length === 0 ? (
            <NoDataAnimation
              height={"76.6vh"}
              isDarkMode={theme}
              message={"No Data Found"}
            />
          ) : (
            <CustomEStackedBarChart
              width="100vh"
              height="80vh"
              colorMap={{
                green: "#00c300",
                ...(isEOM && { amber: "#FFFF00" }),
                red: "#e60000",
              }}
              rawData={
                !isEOM
                  ? [
                      productGraphData?.map((product: any) =>
                        safeNumber(product.greenCompatibilityPercentage)
                      ),
                      productGraphData?.map((product: any) =>
                        safeNumber(product.redCompatibilityPercentage)
                      ),
                    ]
                  : [
                      productGraphData?.map((product: any) =>
                        safeNumber(product.greenCompatibilityPercentage)
                      ),
                      productGraphData?.map((product: any) =>
                        safeNumber(product.amberCompatibilityPercentage)
                      ),
                      productGraphData?.map((product: any) =>
                        safeNumber(product.redCompatibilityPercentage)
                      ),
                    ]
              }
              isDarkMode={darkMode}
              seriesData={{
                data: productGraphData ?? [],
                name: !isEOM ? ["Green", "Red"] : ["Green", "Amber", "Red"],
                color: !isEOM
                  ? ["#00c300", "#e60000"]
                  : ["#00c300", "#FFFF00", "#e60000"],
                cursor: "default",
              }}
              tooltip={{
                trigger: "axis",
                show: false,
                axisPointer: {
                  type: "shadow",
                  label: {
                    show: false,
                  },
                },
              }}
              legend={{
                data: !isEOM ? ["Green", "Red"] : ["Green", "Amber", "Red"],
                itemGap: 15,
                top: "2%",
                formatter: (name) => {
                  const customNames = {
                    Green: `${!isEOM ? "EOS" : "EOM"} > ${
                      selectedDate && searchParams.get("selectedDate")
                        ? `${new Date(selectedDate).getDate()}/${
                            new Date(selectedDate).getMonth() + 1
                          }/${new Date(selectedDate).getFullYear()}`
                        : "Today"
                    }`,
                    ...(isEOM && {
                      Amber: `EOS > ${
                        selectedDate && searchParams.get("selectedDate")
                          ? `${new Date(selectedDate).getDate()}/${
                              new Date(selectedDate).getMonth() + 1
                            }/${new Date(selectedDate).getFullYear()}`
                          : "Today"
                      } > EOM`,
                    }),
                    Red: `${
                      selectedDate && searchParams.get("selectedDate")
                        ? `${new Date(selectedDate).getDate()}/${
                            new Date(selectedDate).getMonth() + 1
                          }/${new Date(selectedDate).getFullYear()}`
                        : "Today"
                    } > EOS`,
                  };
                  return customNames[name] || name; // Return the custom name if available, otherwise the original name
                },
              }}
              grid={{
                top: "14%",
                left: "5%",
                right: "6%",
                bottom: "0%",
                height: "65%",
                containLabel: true,
              }}
              yAXis={[
                {
                  type: "value",
                  min: 0,
                  max: 100,
                },
              ]}
              xAXis={[
                {
                  type: "category",
                  data: productGraphData?.map((data) => data.productName),
                  axisLabel: {
                    triggerEvent: false,
                    rich: {
                      normal: {
                        cursor: "default", // Force normal cursor instead of pointer
                      },
                    },
                  },
                },
              ]}
              dataZoom={[
                { show: true, start: 0, end: 6, top: "82%" },
                {
                  show: false,
                  type: "inside",
                  start: 0,
                  end: 4,
                },
                {
                  show: false,
                  yAxisIndex: 0,
                  filterMode: "empty",
                  height: "65%",
                  width: 30,
                  showDataShadow: false,
                  left: "95%",
                },
              ]}
            />
          )}
        </>
      </div>
    );
  };

  const SubnetworkGraph = () => {
    return (
      <div
        style={{
          justifyContent: "center",
          display: "block",
          height: "100%",
          width: "100%",
        }}
      >
        <>
          <Paper
            style={{
              padding: "7px",
              backgroundColor: `${darkMode ? "white" : "#e9ecef"}`,
              color: `${darkMode ? "black" : ""}`,
              fontWeight: "bold !important",
            }}
          >
            <Typography sx={{ fontWeight: "bold" }}>
              {"Subnetwork Compliance"}
            </Typography>
          </Paper>
          {subnetworkLoader ? (
            <GraphicLoading
              height={"76.6vh"}
              isDarkMode={theme}
              isLoading={true}
            />
          ) : productGraphData?.length === 0 ? (
            <NoDataAnimation
              height={"76.6vh"}
              isDarkMode={theme}
              message={"No Data Found"}
            />
          ) : (
            <CustomEStackedPieChart
              width="100vh"
              height="80vh"
              isDarkMode={darkMode}
              seriesData={
                subnetworkGraphData?.map((data) => ({
                  name: data?.["supportedServicesDescription"],
                  children: [
                    {
                      name: data?.["greenCompatibilityPercentage"],
                      value: 1,
                      itemStyle: { color: "#00c300" },
                    },
                    {
                      ...(isEOM && {
                        name: data?.["amberCompatibilityPercentage"],
                        value: 1,
                        itemStyle: { color: "#FFFF00" },
                      }),
                    },
                    {
                      name: data?.["redCompatibilityPercentage"],
                      value: 1,
                      itemStyle: { color: "#e60000" },
                    },
                  ].filter((val) => val?.name != "0%"),
                })) ?? []
              }
            />
          )}
        </>
      </div>
    );
  };

  const handleOpcoFilter = async (value) => {
    const filterObj = {};
    if (selectedProduct) {
      filterObj["productId"] = [safeNumber(selectedProduct)];
    }
    if (selectedProductImportance) {
      filterObj["productimportanceId"] = [
        safeNumber(selectedProductImportance),
      ];
    }
    if (selectedSubNetwork) {
      filterObj["supportedServicesId"] = [safeNumber(selectedSubNetwork)];
    }
    if (value?.length !== 0) {
      // Initialize opcoId as an array
      filterObj["opcoId"] = value;

      // value.forEach((val) => {
      //   const getId = LCMAtGlanceOpCo?.filter(
      //     (res) => res[1].value === val
      //   )?.[0]?.[1];

      //   if (getId?.key) {
      //     // Push each found key into the opcoId array
      //     filterObj["opcoId"].push(safeNumber(getId.key));
      //   }
      // });
    }
    if (selectedVertical) {
      // Initialize verticalResponsibleId as an array
      filterObj["verticalResponsibleId"] = selectedVertical;

      // selectedVertical.forEach((val) => {
      //   const getId = LCMAtGlanceVertical?.filter(
      //     (res) => res[1].value === val
      //   )?.[0]?.[1];

      //   if (getId?.key) {
      //     // Push each found key into the verticalResponsibleId array
      //     filterObj["verticalResponsibleId"].push(safeNumber(getId.key));
      //   }
      // });
    }
    const graphData = await GetLCMAtGlanceGraph(filterObj);
    const formattedGraphData = graphData?.ResultDtoCreate?.data;

    // const filterProduct = formattedGraphData?.productWisePercentage?.reduce(
    //   (acc, curr) => {
    //     if (
    //       curr.greenCompatibilityPercentage &&
    //       curr.greenCompatibilityPercentage !== "0"
    //     ) {
    //       acc[curr.productId] = {
    //         key: curr.productId,
    //         value: curr.productName,
    //       };
    //     }
    //     return acc;
    //   },
    //   {}
    // );

    // if (
    //   !selectedProduct &&
    //   filterProduct &&
    //   Object.keys(filterProduct).length > 0
    // ) {
    //   setLCMAtGlanceProduct(Object.entries(filterProduct));
    // }

    const filterProductImportance =
      formattedGraphData?.productImportanceFilter?.reduce((acc, curr) => {
        if (curr.text && curr.value) {
          acc[curr.value] = {
            key: curr.value,
            value: curr.text,
          };
        }
        return acc;
      }, {});

    if (
      !selectedProductImportance &&
      filterProductImportance &&
      Object.keys(filterProductImportance).length > 0
    ) {
      setLCMAtGlanceProductImportance(Object.entries(filterProductImportance));
    }

    // const filterSubNetwork =
    //   formattedGraphData?.subnetworkWisePercentage?.reduce((acc, curr) => {
    //     if (
    //       curr.greenCompatibilityPercentage &&
    //       curr.greenCompatibilityPercentage !== "0"
    //     ) {
    //       acc[curr.supportedServicesId] = {
    //         key: curr.supportedServicesId,
    //         value: curr.supportedServicesDescription,
    //       };
    //     }
    //     return acc;
    //   }, {});

    // if (
    //   !selectedSubNetwork &&
    //   filterSubNetwork &&
    //   Object.keys(filterSubNetwork).length > 0
    // ) {
    //   setLCMAtGlanceSubNetwork(Object.entries(filterSubNetwork));
    // }
    // const filterVertical =
    //   formattedGraphData.verticalFilterValueBasedOnFilter?.reduce(
    //     (acc, curr) => {
    //       if (curr.text && curr.value) {
    //         acc[curr.value] = {
    //           key: curr.value,
    //           value: curr.text,
    //         };
    //       }
    //       return acc;
    //     },
    //     {}
    //   );
    // if (
    //   !selectedVertical === false &&
    //   filterVertical &&
    //   Object.keys(filterVertical).length > 0
    // ) {
    //   setLCMAtGlanceVertical(Object.entries(filterVertical));
    // }
  };

  const handleProductFilter = async (value) => {
    const filterObj = {};
    filterObj["productId"] = [safeNumber(value)];
    if (selectedSubNetwork) {
      filterObj["supportedServicesId"] = [safeNumber(selectedSubNetwork)];
    }
    if (selectedProductImportance) {
      filterObj["productimportanceId"] = [
        safeNumber(selectedProductImportance),
      ];
    }
    if (selectedOpco) {
      // Initialize opcoId as an array
      filterObj["opcoId"] = selectedOpco;

      // selectedOpco.forEach((val) => {
      //   const getId = LCMAtGlanceOpCo?.filter(
      //     (res) => res[1].value === val
      //   )?.[0]?.[1];

      //   if (getId?.key) {
      //     // Push each found key into the opcoId array
      //     filterObj["opcoId"].push(safeNumber(getId.key));
      //   }
      // });
    }
    if (selectedVertical) {
      // Initialize verticalResponsibleId as an array
      filterObj["verticalResponsibleId"] = selectedVertical;

      // selectedVertical.forEach((val) => {
      //   const getId = LCMAtGlanceVertical?.filter(
      //     (res) => res[1].value === val
      //   )?.[0]?.[1];

      //   if (getId?.key) {
      //     // Push each found key into the verticalResponsibleId array
      //     filterObj["verticalResponsibleId"].push(safeNumber(getId.key));
      //   }
      // });
    }
    const graphData = await GetLCMAtGlanceGraph(filterObj);
    const formattedGraphData = graphData?.ResultDtoCreate?.data;

    // const filterOpco = formattedGraphData?.opcoWisePercentage?.reduce(
    //   (acc, curr) => {
    //     if (
    //       curr.greenCompatibilityPercentage &&
    //       curr.greenCompatibilityPercentage !== "0"
    //     ) {
    //       acc[curr.opcoId] = {
    //         key: curr.opcoId,
    //         value: curr.opCoDescrption,
    //       };
    //     }
    //     return acc;
    //   },
    //   {}
    // );

    // if (!selectedOpco && filterOpco && Object.keys(filterOpco).length > 0) {
    //   setLCMAtGlanceOpCo(Object.entries(filterOpco));
    // }

    const filterProductImportance =
      formattedGraphData?.productImportanceFilter?.reduce((acc, curr) => {
        if (curr.text && curr.value) {
          acc[curr.value] = {
            key: curr.value,
            value: curr.text,
          };
        }
        return acc;
      }, {});

    if (
      !selectedProductImportance &&
      filterProductImportance &&
      Object.keys(filterProductImportance).length > 0
    ) {
      setLCMAtGlanceProductImportance(Object.entries(filterProductImportance));
    }

    // const filterSubNetwork =
    //   formattedGraphData?.subnetworkWisePercentage?.reduce((acc, curr) => {
    //     if (
    //       curr.greenCompatibilityPercentage &&
    //       curr.greenCompatibilityPercentage !== "0"
    //     ) {
    //       acc[curr.supportedServicesId] = {
    //         key: curr.supportedServicesId,
    //         value: curr.supportedServicesDescription,
    //       };
    //     }
    //     return acc;
    //   }, {});

    // if (
    //   !selectedSubNetwork &&
    //   filterSubNetwork &&
    //   Object.keys(filterSubNetwork).length > 0
    // ) {
    //   setLCMAtGlanceSubNetwork(Object.entries(filterSubNetwork));
    // }

    const filterVertical =
      formattedGraphData.verticalFilterValueBasedOnFilter?.reduce(
        (acc, curr) => {
          if (curr.text && curr.value) {
            acc[curr.value] = {
              key: curr.value,
              value: curr.text,
            };
          }
          return acc;
        },
        {}
      );
    const checkKeys = Object.entries(filterVertical)?.map(
      (res: any) => res[1].key
    );
    const result = checkKeys.filter((val: any) =>
      selectedVertical.map(String).includes(val)
    );

    setSelectedVertical(result ? result : selectedVertical);
    if (
      !selectedVertical === false &&
      filterVertical &&
      Object.keys(filterVertical).length > 0
    ) {
      setLCMAtGlanceVertical(Object.entries(filterVertical));
    }
  };

  const handleProductImportanceFilter = async (value) => {
    const filterObj = {};
    filterObj["productimportanceId"] = [safeNumber(value)];
    if (selectedSubNetwork) {
      filterObj["supportedServicesId"] = [safeNumber(selectedSubNetwork)];
    }
    if (selectedOpco) {
      // Initialize opcoId as an array
      filterObj["opcoId"] = selectedOpco;

      // selectedOpco.forEach((val) => {
      //   const getId = LCMAtGlanceOpCo?.filter(
      //     (res) => res[1].value === val
      //   )?.[0]?.[1];

      //   if (getId?.key) {
      //     // Push each found key into the opcoId array
      //     filterObj["opcoId"].push(safeNumber(getId.key));
      //   }
      // });
    }
    if (selectedVertical) {
      // Initialize verticalResponsibleId as an array
      filterObj["verticalResponsibleId"] = selectedVertical;

      // selectedVertical.forEach((val) => {
      //   const getId = LCMAtGlanceVertical?.filter(
      //     (res) => res[1].value === val
      //   )?.[0]?.[1];

      //   if (getId?.key) {
      //     // Push each found key into the verticalResponsibleId array
      //     filterObj["verticalResponsibleId"].push(safeNumber(getId.key));
      //   }
      // });
    }
    const graphData = await GetLCMAtGlanceGraph(filterObj);
    const formattedGraphData = graphData?.ResultDtoCreate?.data;

    // const filterOpco = formattedGraphData?.opcoWisePercentage?.reduce(
    //   (acc, curr) => {
    //     if (
    //       curr.greenCompatibilityPercentage &&
    //       curr.greenCompatibilityPercentage !== "0"
    //     ) {
    //       acc[curr.opcoId] = {
    //         key: curr.opcoId,
    //         value: curr.opCoDescrption,
    //       };
    //     }
    //     return acc;
    //   },
    //   {}
    // );

    // if (!selectedOpco && filterOpco && Object.keys(filterOpco).length > 0) {
    //   setLCMAtGlanceOpCo(Object.entries(filterOpco));
    // }

    // const filterSubNetwork =
    //   formattedGraphData?.subnetworkWisePercentage?.reduce((acc, curr) => {
    //     if (
    //       curr.greenCompatibilityPercentage &&
    //       curr.greenCompatibilityPercentage !== "0"
    //     ) {
    //       acc[curr.supportedServicesId] = {
    //         key: curr.supportedServicesId,
    //         value: curr.supportedServicesDescription,
    //       };
    //     }
    //     return acc;
    //   }, {});

    // if (
    //   !selectedSubNetwork &&
    //   filterSubNetwork &&
    //   Object.keys(filterSubNetwork).length > 0
    // ) {
    //   setLCMAtGlanceSubNetwork(Object.entries(filterSubNetwork));
    // }

    const filterVertical =
      formattedGraphData.verticalFilterValueBasedOnFilter?.reduce(
        (acc, curr) => {
          if (curr.text && curr.value) {
            acc[curr.value] = {
              key: curr.value,
              value: curr.text,
            };
          }
          return acc;
        },
        {}
      );
    const checkKeys = Object.entries(filterVertical)?.map(
      (res: any) => res[1].key
    );
    const result = checkKeys.filter((val: any) =>
      selectedVertical.map(String).includes(val)
    );

    setSelectedVertical(result ? result : selectedVertical);
    if (
      !selectedVertical === false &&
      filterVertical &&
      Object.keys(filterVertical).length > 0
    ) {
      setLCMAtGlanceVertical(Object.entries(filterVertical));
    }
  };

  const handleSubNetworkFilter = async (value) => {
    const filterObj = {};
    filterObj["supportedServicesId"] = [safeNumber(value)];
    if (selectedProduct) {
      filterObj["productId"] = [safeNumber(selectedProduct)];
    }
    if (selectedProductImportance) {
      filterObj["productimportanceId"] = [
        safeNumber(selectedProductImportance),
      ];
    }
    if (selectedOpco) {
      // Initialize opcoId as an array
      filterObj["opcoId"] = selectedOpco;

      // selectedOpco.forEach((val) => {
      //   const getId = LCMAtGlanceOpCo?.filter(
      //     (res) => res[1].value === val
      //   )?.[0]?.[1];

      //   if (getId?.key) {
      //     // Push each found key into the opcoId array
      //     filterObj["opcoId"].push(safeNumber(getId.key));
      //   }
      // });
    }
    if (selectedVertical) {
      // Initialize verticalResponsibleId as an array
      filterObj["verticalResponsibleId"] = selectedVertical;

      // selectedVertical.forEach((val) => {
      //   const getId = LCMAtGlanceVertical?.filter(
      //     (res) => res[1].value === val
      //   )?.[0]?.[1];

      //   if (getId?.key) {
      //     // Push each found key into the verticalResponsibleId array
      //     filterObj["verticalResponsibleId"].push(safeNumber(getId.key));
      //   }
      // });
    }
    const graphData = await GetLCMAtGlanceGraph(filterObj);
    const formattedGraphData = graphData?.ResultDtoCreate?.data;

    // const filterOpco = formattedGraphData?.opcoWisePercentage?.reduce(
    //   (acc, curr) => {
    //     if (
    //       curr.greenCompatibilityPercentage &&
    //       curr.greenCompatibilityPercentage !== "0"
    //     ) {
    //       acc[curr.opcoId] = {
    //         key: curr.opcoId,
    //         value: curr.opCoDescrption,
    //       };
    //     }
    //     return acc;
    //   },
    //   {}
    // );

    // if (!selectedOpco && filterOpco && Object.keys(filterOpco).length > 0) {
    //   setLCMAtGlanceOpCo(Object.entries(filterOpco));
    // }

    // const filterProduct = formattedGraphData?.productWisePercentage?.reduce(
    //   (acc, curr) => {
    //     if (
    //       curr.greenCompatibilityPercentage &&
    //       curr.greenCompatibilityPercentage !== "0"
    //     ) {
    //       acc[curr.productId] = {
    //         key: curr.productId,
    //         value: curr.productName,
    //       };
    //     }
    //     return acc;
    //   },
    //   {}
    // );

    // if (
    //   !selectedProduct &&
    //   filterProduct &&
    //   Object.keys(filterProduct).length > 0
    // ) {
    //   setLCMAtGlanceProduct(Object.entries(filterProduct));
    // }

    const filterProductImportance =
      formattedGraphData?.productImportanceFilter?.reduce((acc, curr) => {
        if (curr.text && curr.value) {
          acc[curr.value] = {
            key: curr.value,
            value: curr.text,
          };
        }
        return acc;
      }, {});

    if (
      !selectedProductImportance &&
      filterProductImportance &&
      Object.keys(filterProductImportance).length > 0
    ) {
      setLCMAtGlanceProductImportance(Object.entries(filterProductImportance));
    }

    const filterVertical =
      formattedGraphData.verticalFilterValueBasedOnFilter?.reduce(
        (acc, curr) => {
          if (curr.text && curr.value) {
            acc[curr.value] = {
              key: curr.value,
              value: curr.text,
            };
          }
          return acc;
        },
        {}
      );

    const checkKeys = Object.entries(filterVertical)?.map(
      (res: any) => res[1].key
    );
    const result = checkKeys.filter((val: any) =>
      selectedVertical.map(String).includes(val)
    );

    setSelectedVertical(result ? result : selectedVertical);
    if (
      !selectedVertical === false &&
      filterVertical &&
      Object.keys(filterVertical).length > 0
    ) {
      setLCMAtGlanceVertical(Object.entries(filterVertical));
    }
  };

  const handleVerticalFilter = async (value) => {
    const filterObj = {};
    if (selectedProduct) {
      filterObj["productId"] = [safeNumber(selectedProduct)];
    }
    if (selectedProductImportance) {
      filterObj["productimportanceId"] = [
        safeNumber(selectedProductImportance),
      ];
    }
    if (selectedSubNetwork) {
      filterObj["supportedServicesId"] = [safeNumber(selectedSubNetwork)];
    }
    if (selectedOpco) {
      // Initialize opcoId as an array
      filterObj["opcoId"] = selectedOpco;

      // selectedOpco.forEach((val) => {
      //   const getId = LCMAtGlanceOpCo?.filter(
      //     (res) => res[1].value === val
      //   )?.[0]?.[1];

      //   if (getId?.key) {
      //     // Push each found key into the opcoId array
      //     filterObj["opcoId"].push(safeNumber(getId.key));
      //   }
      // });
    }
    if (value?.length !== 0) {
      // Initialize verticalResponsibleId as an array
      filterObj["verticalResponsibleId"] = value;

      // value.forEach((val) => {
      //   const getId = LCMAtGlanceVertical?.filter(
      //     (res) => res[1].value === val
      //   )?.[0]?.[1];

      //   if (getId?.key) {
      //     // Push each found key into the verticalResponsibleId array
      //     filterObj["verticalResponsibleId"].push(safeNumber(getId.key));
      //   }
      // });
    }
    const graphData = await GetLCMAtGlanceGraph(filterObj);
    const formattedGraphData = graphData?.ResultDtoCreate?.data;

    const filterOpco = formattedGraphData?.opcoWisePercentage?.reduce(
      (acc, curr) => {
        if (
          curr.greenCompatibilityPercentage &&
          curr.greenCompatibilityPercentage !== "0"
        ) {
          acc[curr.opcoId] = {
            key: curr.opcoId,
            value: curr.opCoDescrption,
          };
        }
        return acc;
      },
      {}
    );

    if (!selectedOpco && filterOpco && Object.keys(filterOpco).length > 0) {
      setLCMAtGlanceOpCo(Object.entries(filterOpco));
    }

    const filterProduct = formattedGraphData?.productWisePercentage?.reduce(
      (acc, curr) => {
        if (
          curr.greenCompatibilityPercentage &&
          curr.greenCompatibilityPercentage !== "0"
        ) {
          acc[curr.productId] = {
            key: curr.productId,
            value: curr.productName,
          };
        }
        return acc;
      },
      {}
    );

    if (
      !selectedProduct &&
      filterProduct &&
      Object.keys(filterProduct).length > 0
    ) {
      setLCMAtGlanceProduct(Object.entries(filterProduct));
    }

    const filterProductImportance =
      formattedGraphData?.productImportanceFilter?.reduce((acc, curr) => {
        if (curr.text && curr.value) {
          acc[curr.value] = {
            key: curr.value,
            value: curr.text,
          };
        }
        return acc;
      }, {});

    if (
      !selectedProductImportance &&
      filterProductImportance &&
      Object.keys(filterProductImportance).length > 0
    ) {
      setLCMAtGlanceProductImportance(Object.entries(filterProductImportance));
    }

    const filterSubNetwork =
      formattedGraphData?.subnetworkWisePercentage?.reduce((acc, curr) => {
        if (
          curr.greenCompatibilityPercentage &&
          curr.greenCompatibilityPercentage !== "0"
        ) {
          acc[curr.supportedServicesId] = {
            key: curr.supportedServicesId,
            value: curr.supportedServicesDescription,
          };
        }
        return acc;
      }, {});

    if (
      !selectedSubNetwork &&
      filterSubNetwork &&
      Object.keys(filterSubNetwork).length > 0
    ) {
      setLCMAtGlanceSubNetwork(Object.entries(filterSubNetwork));
    }
  };

  const handleChange = (value, type: string) => {
    type === "opco" && setSelectedOpco(value);
    type === "vertical" && setSelectedVertical(value);
  };

  const handleReset = () => {
    setSelectedOpco([]);
    setSelectedProduct(null);
    setSelectedProductImportance(null);
    setSelectedSubNetwork(null);
    setSelectedVertical(defaultVertical);
    setSelectedDate(undefined);
    getLCMAtGlanceGraphApiResource();
    setShowCanvas(false);
  };

  const handleFilter = (val?: any) => {
    setSelectedOpco([]);
    setSelectedProduct(null);
    setSelectedProductImportance(null);
    setSelectedSubNetwork(null);
    setSelectedVertical(defaultVertical);
    setSelectedDate(undefined);
    // setLCMAtGlanceGraphData([]);
    // setFormattedGraphData([]);
    const filterObj = {};
    if (selectedOpco?.length > 0) {
      // Initialize opcoId as an array
      filterObj["opcoId"] = selectedOpco;

      // selectedOpco.forEach((val) => {
      //   const getId = LCMAtGlanceOpCo?.filter(
      //     (res) => res[1].value === val
      //   )?.[0]?.[1];

      //   if (getId?.key) {
      //     // Push each found key into the opcoId array
      //     filterObj["opcoId"].push(safeNumber(getId.key));
      //   }
      // });
    }
    if (selectedVertical?.length > 0) {
      // Initialize verticalResponsibleId as an array
      filterObj["verticalResponsibleId"] =
        val?.length > 0 ? val : selectedVertical;

      // selectedVertical.forEach((val) => {
      //   const getId = LCMAtGlanceVertical?.filter(
      //     (res) => res[1].value === val
      //   )?.[0]?.[1];

      //   if (getId?.key) {
      //     // Push each found key into the verticalResponsibleId array
      //     filterObj["verticalResponsibleId"].push(safeNumber(getId.key));
      //   }
      // });
    }
    if (selectedProduct) {
      filterObj["productId"] = [selectedProduct];
    }
    if (selectedSubNetwork) {
      filterObj["supportedServicesId"] = [selectedSubNetwork];
    }
    if (selectedProductImportance) {
      filterObj["productimportanceId"] = [selectedProductImportance];
    }
    if (selectedDate) {
      const selectedDateString = selectedDate.toISOString();
      filterObj["selectedDate"] = selectedDateString;
    }
    const queryString = new URLSearchParams(filterObj).toString();
    // console.log("queryString", queryString);
    const url = `/lcmatglance/?${queryString}&isEOM=${isEOM ? "Yes" : "No"}`;

    const paramsObj = Object.fromEntries(searchParams.entries());
    if (paramsObj) {
      const transformedObj = Object.keys(paramsObj).reduce((acc, key) => {
        const value = paramsObj[key];

        if (key === "selectedDate") {
          acc[key] = String(value);
        } else {
          acc[key] = [safeNumber(value)];
        }
        return acc;
      }, {});
      if (transformedObj["productId"]) {
        setSelectedProduct(transformedObj["productId"]);
      }
      if (transformedObj["productimportanceId"]) {
        setSelectedProductImportance(transformedObj["productimportanceId"]);
      }
      if (transformedObj["supportedServicesId"]) {
        setSelectedSubNetwork(transformedObj["supportedServicesId"]);
      }
      if (transformedObj["verticalResponsibleId"]) {
        setSelectedVertical(transformedObj["verticalResponsibleId"]);
      }
      if (transformedObj["opcoId"]) {
        setSelectedOpco(transformedObj["opcoId"]);
      }
      if (transformedObj["isEOM"] === "Yes") {
        setIsEOM(true);
      }
    }
    handleReset();
    setShowCanvas(false);
    window.open(url, "_blank");
  };
  const checkIsDisable = () => {
    const requiredArrays = [selectedOpco, selectedVertical];
    const requiredValues = [
      selectedProduct,
      selectedSubNetwork,
      selectedProductImportance,
      selectedDate,
    ];
    const isArrayValid = requiredArrays.some(
      (arr) => Array.isArray(arr) && arr.length > 0
    );
    const isValueValid = requiredValues.some(
      (val) => val !== 0 && val !== null && val !== undefined
    );
    return !(isArrayValid || isValueValid);
  };

  // const [data, setData] = useState(null);
  // const handleOpenInNewTab = () => {
  //   const data = { userId: 123, name: "John Doe" }; // Example data to pass

  //   localStorage.setItem("filterData", JSON.stringify(data));
  //   rootStore.dispatch({
  //     type: "GET_GRAPH_FILTER",
  //     payload: { FilterData: data },
  //   });
  //   // Open the new tab
  //   const newTab = window.open("/lcmatglance/filters", "_blank");
  //   // Send the data using postMessage (if needed)
  //   if (newTab) {
  //     newTab.onload = () => {
  //       newTab.postMessage(data, window.location.origin);
  //     };
  //   }
  // };

  // useEffect(() => {
  //   const handleMessage = (event) => {
  //     // Ensure the message is from the same origin
  //     if (event.origin !== window.location.origin) return;
  //     rootStore.dispatch({
  //       type: "GET_GRAPH_FILTER",
  //       payload: { FilterData: data },
  //     });
  //     // Set data from the message
  //     setData(event.data);

  //     // Optionally, you could also store the received data in localStorage
  //     localStorage.setItem("filterData", JSON.stringify(event.data));
  //   };

  //   // Add event listener for postMessage
  //   window.addEventListener("message", handleMessage);

  //   // Check if there's already data in localStorage
  //   const storedData = localStorage.getItem("filterData");
  //   if (storedData) {
  //     setData(JSON.parse(storedData)); // Retrieve and set the data
  //   }

  //   // Cleanup listener when the component is unmounted
  //   return () => {
  //     window.removeEventListener("message", handleMessage);
  //   };
  // }, []);

  const getGaugeDatas = (type) => {
    const gaugeData: any = [
      [
        type === "lcm"
          ? safeNumber(gaugeGraphData?.greenCompatibilityPercentage)
          : safeNumber(gaugeGraphData?.greenNodePercentage),
        "#00c300",
      ],
      [
        type === "lcm"
          ? safeNumber(gaugeGraphData?.amberCompatibilityPercentage)
          : safeNumber(gaugeGraphData?.amberNodePercentage),
        "#FFFF00",
      ],
      [
        type === "lcm"
          ? safeNumber(gaugeGraphData?.redCompatibilityPercentage)
          : safeNumber(gaugeGraphData?.redNodePercentage),
        "#e60000",
      ],
    ];
    // Normalize colorData to sum up to 1 (100%)
    const total = gaugeData?.reduce((sum, [value]) => sum + value, 0) ?? 100;
    // Generate the cumulative color data with normalization
    let cumulative = 0;
    const normalizedColorData = gaugeData?.map(([value, color]) => {
      cumulative += value / total; // Accumulate the value
      return [cumulative, color]; // Return the cumulative value and color
    });
    return normalizedColorData;
  };

  const getColorCodeData = (type) => {
    const getValue = (color: "green" | "amber" | "red") => {
      if (!isEOM || type === "lcm") {
        return safeNumber(gaugeGraphData?.[`${color}CompatibilityPercentage`]);
      }

      return safeNumber(gaugeGraphData?.[`${color}NodePercentage`]);
    };

    return [
      [getValue("green"), "#00c300"],
      [getValue("amber"), "#FFFF00"],
      [getValue("red"), "#e60000"],
    ];
  };

  let paDropDownOptions = [
    { key: "softwareUpgrade", value: "Software Upgrade" },
    { key: "modernizeSolution", value: "Migrate and Decommission" },
    { key: "replaceSolution", value: "Replace Solution" },
  ];
  const getValue = (color: "green" | "amber" | "red", type) => {
    if (!isEOM || type === "lcm") {
      return safeNumber(gaugeGraphData?.[`${color}CompatibilityPercentage`]);
    }
    return safeNumber(gaugeGraphData?.[`${color}NodePercentage`]);
  };

  return (
    <ThemeProvider theme={theme}>
      <Drawer anchor={"right"} open={showCanvas} onClose={() => handleReset()}>
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
            sx={{
              width: "100%",
              textAlign: "left",
            }}
            spacing={2}
          >
            <Box sx={{ width: "100%" }}>
              <MultiSelectCheckmarks
                options={
                  LCMAtGlanceOpCo?.map((val) => {
                    return {
                      key: val[1]?.key,
                      value: val[1]?.value,
                    };
                  }) ?? []
                }
                selectedValues={selectedOpco}
                onChange={(e) => {
                  setSelectedOpco(e);
                  handleOpcoFilter(e);
                }}
                label="Opco"
                isComponent={true}
                size="small"
                widthSize={360}
                darkTheme={darkMode}
              />
            </Box>
            <Box sx={{ width: "100%" }}>
              <MultiSelectCheckmarks
                options={
                  LCMAtGlanceVertical?.map((val) => {
                    return {
                      key: val[1]?.key,
                      value: val[1]?.value,
                    };
                  }) ?? []
                }
                selectedValues={selectedVertical}
                onChange={(e) => {
                  setSelectedVertical(e);
                  handleVerticalFilter(e);
                }}
                label="Vertical"
                isComponent={true}
                size="small"
                widthSize={360}
                darkTheme={darkMode}
              />
            </Box>
            <Box sx={{ width: "100%" }}>
              <MultiSingleSelect
                options={
                  LCMAtGlanceProduct?.map((val) => {
                    return {
                      key: val[1]?.key,
                      value: val[1]?.value,
                    };
                  }) ?? []
                }
                selectedValues={selectedProduct}
                onChange={(e) => {
                  setSelectedProduct(e);
                  handleProductFilter(e);
                }}
                label="Product"
                size="small"
                widthSize={360}
                darkTheme={darkMode}
              />
            </Box>
            <Box sx={{ width: "100%" }}>
              <MultiSingleSelect
                options={
                  LCMAtGlanceSubNetwork?.map((val) => {
                    return {
                      key: val[1]?.key,
                      value: val[1]?.value,
                    };
                  }) ?? []
                }
                selectedValues={selectedSubNetwork}
                onChange={(e) => {
                  setSelectedSubNetwork(e);
                  handleSubNetworkFilter(e);
                }}
                label="Support Service"
                size="small"
                widthSize={360}
                darkTheme={darkMode}
              />
            </Box>
            <Box sx={{ width: "100%" }}>
              <MultiSingleSelect
                options={
                  LCMAtGlanceProductImportance?.map((val) => {
                    return {
                      key: val[1]?.key,
                      value: val[1]?.value,
                    };
                  }) ?? []
                }
                selectedValues={selectedProductImportance}
                onChange={(e) => {
                  setSelectedProductImportance(e);
                  handleProductImportanceFilter(e);
                }}
                label="Product Importance"
                size="small"
                widthSize={360}
                darkTheme={darkMode}
              />
            </Box>
            <Box sx={{ width: "100%" }}>
              <FormControl sx={{ width: 360, marginBottom: 1 }} size={"small"}>
                <DatePicker
                  placeholderText={"Select Date"}
                  selected={selectedDate}
                  onChange={(newDate, e) => {
                    e.preventDefault();
                    setSelectedDate(newDate);
                  }}
                  className="inputForm w-100"
                  minDate={new Date()}
                  maxDate={new Date(2999, 0, 1)}
                  dateFormat="dd/MM/yyyy"
                />
              </FormControl>
            </Box>
            <Box
              sx={{
                marginTop: 4,
                display: "flex",
                justifyContent: "space-between",
              }}
            >
              <Button
                variant="contained"
                color="info"
                onClick={() => handleReset()}
              >
                Close
              </Button>
              <Button
                variant="contained"
                color="success"
                onClick={() => handleFilter()}
                disabled={checkIsDisable()}
              >
                Apply
              </Button>
            </Box>
          </Stack>
        </Box>
      </Drawer>
      <Box
        sx={{ width: "100%", height: "92.2vh", padding: "16px 16px 16px 66px" }}
      >
        <Grid container spacing={2}>
          <Grid size={12}>
            <Grid
              container
              spacing={2}
              sx={{
                justifyContent: "flex-center",
              }}
            >
              <Grid
                size={12}
                sx={{
                  height: "8vh",
                  textAlign: "center",
                  alignContent: "center",
                  justifyContent: "space-between",
                  display: "flex",
                }}
              >
                <Typography
                  variant="h4"
                  sx={{
                    marginBottom: 0,
                    width: "100%",
                    display: "flex",
                    alignItems: "center",
                    fontSize: "1.8rem !important",
                    color: (theme) => theme.palette.text.primary, // Using primary color
                  }}
                >
                  <HtmlTooltip
                    title={
                      <React.Fragment>
                        {
                          "For dates specified in the future, the KPI considers any planned activity completion date as well as vendor EoM/EoS dates"
                        }
                      </React.Fragment>
                    }
                  >
                    <Button
                      startIcon={<FcInfo />}
                      sx={{
                        width: "max-content",
                        fontWeight: "bold",
                        fontSize: "24px !important",
                        color: "white",
                      }}
                    >
                      LCM @Glance
                    </Button>
                  </HtmlTooltip>
                  <>
                    {filterHeadingObj?.length > 0 ? (
                      <div
                        className="d-flex row col-auto"
                        style={{
                          width: "auto",
                          rowGap: "5px",
                        }}
                      >
                        {filterHeadingObj.map(({ title, value }) => (
                          <Chip
                            sx={{ minWidth: "4rem", marginLeft: "1rem" }}
                            variant="outlined"
                            label={
                              <Typography
                                component="span"
                                sx={{
                                  display: "block",
                                  whiteSpace: "normal",
                                  wordBreak: "break-word",
                                }}
                                dangerouslySetInnerHTML={{
                                  __html: `${title} : ${value.join(",")}`,
                                }}
                              />
                            }
                          />
                        ))}
                      </div>
                    ) : searchParams["size"] === 0 &&
                      defaultVertical?.length > 0 &&
                      LCMAtGlanceVertical?.length > 0 ? (
                      <>
                        <Chip
                          sx={{ minWidth: "4rem", marginLeft: "1rem" }}
                          variant="outlined"
                          label={
                            <Typography
                              component="span"
                              sx={{
                                display: "block",
                                whiteSpace: "normal",
                                wordBreak: "break-word",
                              }}
                              dangerouslySetInnerHTML={{
                                __html: `Vertical : ${LCMAtGlanceVertical?.map(
                                  (val) => {
                                    return {
                                      key: val[1]?.key,
                                      value: val[1]?.value,
                                    };
                                  }
                                )
                                  .filter((res) =>
                                    defaultVertical.includes(res.key)
                                  )
                                  .map((res) => res.value)
                                  .join(",")}`,
                              }}
                            />
                          }
                        />
                      </>
                    ) : (
                      ""
                    )}
                  </>
                </Typography>
                {searchParams["size"] === 0 && (
                  <div className="d-flex">
                    <MultiSelectCheckmarks
                      options={
                        LCMAtGlanceVertical?.map((val) => {
                          return {
                            key: val[1]?.key,
                            value: val[1]?.value,
                          };
                        }) ?? []
                      }
                      selectedValues={selectedVertical}
                      onChange={(e) => {
                        setSelectedVertical(e);
                      }}
                      label="Vertical"
                      isComponent={true}
                      size="small"
                      widthSize={250}
                      darkTheme={darkMode}
                    />
                    <button
                      className={`mx-3 download-to-excel btn-danger ${
                        selectedVertical?.length === 0 ? "disabledCursor" : ""
                      }`}
                      style={{
                        minWidth: "6rem",
                        alignSelf: "center",
                      }}
                      disabled={selectedVertical?.length === 0 ? true : false}
                      onClick={() => handleFilter()}
                    >
                      Apply
                    </button>
                    <Tooltip
                      title={!isEOM ? "EOS & EOM Report" : "EOS Only"}
                      arrow
                    >
                      <button
                        onClick={() => setIsEOM((prev) => !prev)}
                        style={{
                          display: "flex",
                          alignItems: "center",
                          gap: "8px",
                          padding: "6px 12px",
                          borderRadius: "4px",
                          border: "1px solid #ccc",
                          background: "#90caf9",
                          cursor: "pointer",
                          alignSelf: "center",
                          width: "max-content",
                        }}
                      >
                        {"EOM"}
                        <div
                          style={{
                            width: "32px",
                            height: "16px",
                            borderRadius: "10px",
                            background: isEOM ? "#4caf50" : "#ccc",
                            position: "relative",
                            transition: "0.2s",
                          }}
                        >
                          <div
                            style={{
                              width: "14px",
                              height: "14px",
                              borderRadius: "50%",
                              background: "#fff",
                              position: "absolute",
                              top: "1px",
                              left: isEOM ? "16px" : "2px",
                              transition: "0.2s",
                            }}
                          />
                        </div>
                      </button>
                    </Tooltip>
                    <FormControl
                      sx={{
                        m: 1,
                        minWidth: 120,
                        marginLeft: 2,
                        marginRight: 0,
                        alignSelf: "center",
                        display: "flex",
                      }}
                      size="small"
                    >
                      <Button
                        variant="contained"
                        endIcon={<IoFilter />}
                        sx={{
                          width: "max-content",
                          // backgroundColor: "#ff0000",
                          fontWeight: "bold",
                          // color: "black",
                        }}
                        onClick={(e) => {
                          e.preventDefault();
                          setShowCanvas(!showCanvas);
                        }}
                      >
                        More Filters
                      </Button>
                    </FormControl>
                  </div>
                )}
              </Grid>
            </Grid>
          </Grid>
          <Grid size={{ xs: 3 }}>
            <Grid
              container
              sx={{
                justifyContent: "space-between",
                alignItems: "flex-start",
                height: "80vh",
                direction: "column",
              }}
            >
              <>
                {["lcm", "asset"].map((res) => (
                  <Item
                    elevation={8}
                    key={res}
                    sx={{
                      width: "100%",
                      height: res === "asset" && !isEOM ? "38.2vh" : "30.2vh",
                      padding: 0,
                    }}
                  >
                    <Paper
                      style={{
                        padding: "7px",
                        backgroundColor: `${darkMode ? "white" : "#e9ecef"}`,
                        color: `${darkMode ? "black" : ""}`,
                        fontWeight: "bold",
                        borderRadius: "unset",
                      }}
                    >
                      <Typography sx={{ fontWeight: "bold" }}>
                        {`${
                          res === "lcm" ? "LCM Compliance" : "Asset Compliance"
                        }`}
                      </Typography>
                    </Paper>
                    {gaugeLoader ? (
                      <GraphicLoading
                        height={res === "asset" && !isEOM ? "34.2vh" : "26.3vh"}
                        isDarkMode={theme}
                        isLoading={true}
                      />
                    ) : (res === "lcm" &&
                        getColorCodeData(res).reduce(
                          (acc, item) => acc && item[0] === 0,
                          true
                        )) ||
                      (res === "asset" &&
                        !isEOM &&
                        gaugeGraphData?.colorCompatability?.length === 0) ? (
                      <NoDataAnimation
                        height={res === "asset" && !isEOM ? "34.2vh" : "26.3vh"}
                        fontSize={30}
                        isDarkMode={theme}
                        message={"No Data Found"}
                      />
                    ) : (
                      <>
                        {isEOM ? (
                          <Gauge
                            colorData={getColorCodeData(res)}
                            green={getValue("green", res)}
                            red={getValue("red", res)}
                            {...(isEOM && { yellow: getValue("amber", res) })}
                            selectedOpco={""}
                          />
                        ) : (
                          <>
                            {res === "lcm" && (
                              <Gauge
                                colorData={getColorCodeData(res)}
                                green={getValue("green", res)}
                                red={getValue("red", res)}
                                selectedOpco={""}
                              />
                            )}
                            {EosTransformedData && res === "asset" && (
                              <CustomEBarChart
                                height="34.2vh"
                                isDarkMode={darkMode}
                                namesList={
                                  EosTransformedData?.map(
                                    (data) => data?.category
                                  ) ?? []
                                }
                                seriesData={[
                                  { name: "Green", color: "#00c300" },
                                  { name: "Red", color: "#e60000" },
                                ].map(({ name, color }) => ({
                                  name: name,
                                  type: "bar",
                                  data:
                                    EosTransformedData?.map((data) => ({
                                      value: data[`${name.toLowerCase()}`],
                                    })) ?? [],
                                  label: {
                                    show: true, // Enable label on top of the bar
                                    position: "top", // Position the label at the top of the bar
                                    formatter: "{c}%", // Display the value of the bar
                                    fontSize: 12, // Optional: Adjust font size
                                    color: darkMode ? "white" : "black",
                                  },
                                  itemStyle: {
                                    color: color,
                                  },
                                }))}
                                tooltip={{
                                  trigger: "item", // Trigger tooltip for all series in the same category
                                  axisPointer: {
                                    type: "shadow", // Display shadow to highlight the category
                                  },
                                  formatter: (params) => {
                                    const { name, value, seriesName, data } =
                                      params; // Use params[0] to access the hovered item
                                    return `<div style="font-family: Arial, sans-serif;font-size: 12px;line-height: 1.5;text-align: left; min-width: 7rem;">
                          <div style="margin-bottom: 10px;padding-bottom: 6px;display: flex;justify-content: space-between;align-items: center; border-bottom-style: ridge;">
                            <span style="font-weight: bold;color: #666;">${name}</span>
                          </div>
                          <div style="display: flex;align-items: center;width: fit-content">
                            <span style="font-weight: bold; padding-right: 2px;">Percentage : </span>
                            <span style="font-weight: bolder; ">${data.value}%</span>
                          </div>
                        </div>`;
                                  },
                                }}
                                legend={{
                                  data: ["Green", "Red"],
                                  itemGap: 15,
                                  top: 12,
                                  formatter: (name) => {
                                    const customNames = {
                                      Green: `Compliant`,
                                      Red: `Non Compliant`,
                                    };
                                    return customNames[name] || name; // Return the custom name if available, otherwise the original name
                                  },
                                }}
                                toolbox={{
                                  show: false,
                                }}
                                grid={{
                                  top: "20%",
                                  left: "8%",
                                  right: "5%",
                                  bottom: "0%",
                                  height: "60%",
                                  containLabel: true,
                                }}
                                yAXis={[
                                  {
                                    type: "value",
                                    min: 0,
                                    max: 100,
                                    top: 1,
                                    name: "Percentage ->",
                                    nameLocation: "middle",
                                    nameGap: 48,
                                    axisLabel: {
                                      formatter: "{value}%",
                                      fontSize: 12,
                                    },
                                  },
                                ]}
                                xAXis={[
                                  {
                                    type: "category",
                                    data:
                                      EosTransformedData?.map(
                                        (data) => data.category
                                      ) ?? [],
                                    name: "Year ->",
                                    nameLocation: "middle",
                                    nameGap: 28,
                                    axisLabel: { fontSize: 12 },
                                    axisTick: { alignWithLabel: true },
                                  },
                                ]}
                                dataZoom={[
                                  {
                                    type: "slider",
                                    start: 0,
                                    end: 100,
                                    bottom: 15,
                                    height: 18,
                                    right: 44,
                                  },
                                  {
                                    show: false,
                                    type: "inside",
                                    start: 0,
                                    end: 4,
                                  },
                                  {
                                    show: false,
                                    yAxisIndex: 0,
                                    filterMode: "empty",
                                    height: "25%",
                                    width: 30,
                                    showDataShadow: false,
                                    showDetail: false,
                                    left: "95%",
                                  },
                                ]}
                              />
                            )}
                          </>
                        )}
                      </>
                    )}
                  </Item>
                ))}
              </>
              <Item
                elevation={8}
                sx={{ width: "100%", height: !isEOM ? "10vh" : "16vh" }}
              >
                <div className="p-3 ">
                  <div className="d-flex alignItemCenter pb-2">
                    <div className="legend-style legend-green mr-3"></div>
                    <div
                      style={{
                        color: `${darkMode ? "white" : "black"}`,
                      }}
                    >
                      {`${
                        !isEOM
                          ? "Compliant"
                          : `EOM > ${
                              selectedDate && searchParams.get("selectedDate")
                                ? `${new Date(selectedDate).getDate()}/${
                                    new Date(selectedDate).getMonth() + 1
                                  }/${new Date(selectedDate).getFullYear()}`
                                : "Today"
                            }`
                      }`}
                    </div>
                  </div>
                  {isEOM && (
                    <div className="d-flex alignItemCenter pb-2">
                      <div className="legend-style legend-amber mr-3"></div>
                      <div
                        style={{
                          color: `${darkMode ? "white" : "black"}`,
                        }}
                      >
                        {` EOS > ${
                          selectedDate && searchParams.get("selectedDate")
                            ? `${new Date(selectedDate).getDate()}/${
                                new Date(selectedDate).getMonth() + 1
                              }/${new Date(selectedDate).getFullYear()}`
                            : "Today"
                        } > EOM`}
                      </div>
                    </div>
                  )}
                  <div className="d-flex alignItemCenter pb-2">
                    <div className="legend-style legend-red mr-3"></div>
                    <div
                      style={{
                        color: `${darkMode ? "white" : "black"}`,
                      }}
                    >
                      {`${
                        !isEOM
                          ? "Non Compliant"
                          : `${
                              selectedDate && searchParams.get("selectedDate")
                                ? `${new Date(selectedDate).getDate()}/${
                                    new Date(selectedDate).getMonth() + 1
                                  }/${new Date(selectedDate).getFullYear()}`
                                : "Today"
                            } > EOS`
                      }`}
                    </div>
                  </div>
                </div>
              </Item>
            </Grid>
          </Grid>
          <Grid size={{ xs: 9 }}>
            <CustomSlides
              loadDropDownComponent={
                searchParams.get("opcoId") && !paLoader
                  ? [
                      <DropdownInputComponent
                        label={""}
                        labelCSS="mb-0"
                        inputCSS="labelForm voda-bold mb-2"
                        isSearchable={false}
                        isClearable={false}
                        value={paDropDown}
                        options={plannedActivityResource ?? []}
                        onChange={(e: any) => setPaDropDown(e)}
                      />,
                    ]
                  : []
              }
              slides={
                !searchParams.get("opcoId")
                  ? [
                      <OpCoGraph key={"OpCoGraph"} />,
                      <ProductGraph key={"ProductGraph"} />,
                      <SubnetworkGraph key={"SubnetworkGraph"} />,
                    ]
                  : [
                      <PAGraphs
                        key={"PAGraphs"}
                        title={"PAGraphs"}
                        data={{
                          paGraphDataDisplay: paGraphDataDisplay,
                          // softwarePAGraphData: softwarePAGraphData,
                          // replacePAGraphData: replacePAGraphData,
                          // modernizePAGraphData: modernizePAGraphData,
                        }}
                      />,
                      <ProductGraph key={"ProductGraph"} />,
                      <SubnetworkGraph key={"SubnetworkGraph"} />,
                    ]
              }
              autoplayDelay={3000}
            />
          </Grid>
        </Grid>
      </Box>
    </ThemeProvider>
  );
};

export default FlexboxGapStack;
