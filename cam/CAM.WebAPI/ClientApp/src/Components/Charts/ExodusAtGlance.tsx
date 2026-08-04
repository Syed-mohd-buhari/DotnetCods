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
  GetExodusAtGlanceGraph,
  GetExodusAtGlanceGraphApiResource,
  GetExodusAtGlancePAGraph,
  GetOpcoWisePercentage,
} from "../../Redux/Action/LookUp/ExodusAtGlanceGraph/ExodusAtGlanceGraphAction";
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
  const [selectedTargetPlatform, setSelectedTargetPlatform] =
    useState<any>(null);
  const [selectedVendor, setSelectedVendor] = useState<any>(null);
  const [selectedEnvironment, setSelectedEnvironment] = useState<any>(null);
  const [selectedVertical, setSelectedVertical] = useState<any>(null);
  const [selectedSubNetwork, setSelectedSubNetwork] = useState<any>(null);
  const [selectedPAStatus, setSelectedPAStatus] = useState("");
  const [selectedPlatform, setSelectedPlatform] = useState<string[]>([]);
  const [defaultPlatform, setDefaultPlatform] = useState<string[]>([]);
  const [LCMAtGlanceResource, setLCMAtGlanceResource] = useState<any>();
  const [isToggleOn, setIsToggleOn] = useState<boolean>(false);
  const [LCMAtGlanceOpCo, setLCMAtGlanceOpCo] = useState<any>();
  const [LCMAtGlanceProduct, setLCMAtGlanceProduct] = useState<any>();
  const [LCMAtGlanceVendor, setLCMAtGlanceVendor] = useState<any>();
  const [LCMAtGlanceEnvironment, setLCMAtGlanceEnvironment] = useState<any>();
  const [LCMAtGlanceVertical, setLCMAtGlanceVertical] = useState<any>();
  const [LCMAtGlanceSubNetwork, setLCMAtGlanceSubNetwork] = useState<any>();
  const [LCMAtGlancePlatform, setLCMAtGlancePlatform] = useState<any>();
  const [LCMAtGlanceTargetPlatform, setLCMAtGlanceTargetPlatform] =
    useState<any>();
  const [showCanvas, setShowCanvas] = useState(false);
  const [selectedDate, setSelectedDate] = useState<Date | null | undefined>(
    undefined
  );
  const [filterFields, setFilterFields] = useState(null);
  const [isPageLoad, setIsPageLoad] = useState(true);
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
  const [isFilterData, setIsFilterData] = useState<boolean>(false);
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
    return () => {
      selectDarkMode(false);
    };
  }, []);

  useEffect(() => {
    const complianceTypeParam = searchParams.get("complianceType");
    if (complianceTypeParam !== null) {
      setIsToggleOn(complianceTypeParam === "PlannedActivityCompliance");
    }
  }, []);

  useEffect(() => {
    if (!isPermesso) return;
    else {
      const fetchData = async (filterObjData) => {
        const apiFunctions = [
          {
            api: getExodusAtGlanceGraphApiResource,
            setLoader: setFilterLoader,
            filterObj: {},
            name: "getExodusAtGlanceGraphApiResource",
          },
          {
            api: getOpcoGraphData,
            setLoader: setOpcoLoader,
            filterObj: filterObjData,
            name: "getOpcoGraphData",
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

        if (transformedObj["productId"]) {
          setSelectedProduct(transformedObj["productId"]);
        }
        if (transformedObj["vendorId"]) {
          setSelectedVendor(transformedObj["vendorId"]);
        }
        if (transformedObj["platformId"]) {
          setSelectedPlatform(transformedObj["platformId"]);
        }
        if (transformedObj["opcoId"]) {
          setSelectedOpco(transformedObj["opcoId"]);
        }
        if (transformedObj["targetPlatformId"]) {
          setSelectedTargetPlatform(transformedObj["targetPlatformId"]);
        }
        fetchData(transformedObj);
        setPaPayload(transformedObj);
      } else {
        fetchData({});
        setPaPayload({});
      }
    }
  }, [isPermesso, searchParams, isToggleOn]); // Dependency array with memoized callback

  useEffect(() => {
    if (isPermesso && searchParams.get("opcoId")) {
      callPAGraphApi();
    }
  }, [isPermesso, isToggleOn]);

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
        data: data?.allOpcos,
        getLabel: (item) => item?.value,
      },
      vendorId: {
        title: "Vendor",
        data: data?.allVendors,
        getLabel: (item) => item?.value,
      },
      productId: {
        title: "Product",
        data: data?.allProducts,
        getLabel: (item) => item?.value,
      },
      platformId: {
        title: "Current Platform",
        data: data?.allPlatformAndBuildConstructions,
        getLabel: (item) => item?.value,
      },
      targetPlatformId: {
        title: "Target Platform",
        data: data?.targetedPlatformAndBuildConstructions,
        getLabel: (item) => item?.value,
      },
      environmentId: {
        title: "Environment",
        data: data?.environmentResource,
        getLabel: (item) => item?.value,
      },
      verticalId: {
        title: "Vertical",
        data: data?.resourceVertical,
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
    setFilterHeadingObj(
      result?.filter(
        (item) =>
          item.title !== "selectedDate" &&
          item.title !== "colorCode" &&
          item.title !== "complianceType"
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
      } else if (key === "vendorId" && transformedObj["vendorId"]) {
        const product = data?.allVendors?.find(
          (product) => product.key === safeNumber(transformedObj["vendorId"])
        );
        if (product) {
          if (hasAddedValue) combinedString += " , ";
          combinedString += "Vendor: " + product.value;
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
      } else if (key === "platformId" && transformedObj["platformId"]) {
        const platformId = searchParams.get("platformId");
        const verticalIdArray = platformId
          ? platformId.split(",").map(safeNumber)
          : [];

        const matchedVertical = verticalIdArray
          ?.map((id) =>
            data?.allPlatformAndBuildConstructions?.find(
              (vertical) => vertical.key === id
            )
          )
          .filter((vertical) => vertical);

        if (matchedVertical.length > 0) {
          if (hasAddedValue) combinedString += " , ";
          combinedString +=
            "Current Platform: " +
            matchedVertical
              .map((vertical, index) => {
                const isLast = index === matchedVertical.length - 1;
                return vertical.value + (isLast ? "" : ", ");
              })
              .join("");
          hasAddedValue = true;
        }
      } else if (
        key === "targetPlatformId" &&
        transformedObj["targetPlatformId"]
      ) {
        const targetPlatformId = searchParams.get("targetPlatformId");
        const targetPlatformIdArray = targetPlatformId
          ? targetPlatformId.split(",").map(safeNumber)
          : [];

        const matchedTargetPlatforms = targetPlatformIdArray
          ?.map((id) =>
            data?.targetedPlatformAndBuildConstructions?.find(
              (targetPlatform) => targetPlatform.key === id
            )
          )
          .filter((targetPlatform) => targetPlatform);

        if (matchedTargetPlatforms.length > 0) {
          if (hasAddedValue) combinedString += " , ";
          combinedString +=
            "Target Platform: " +
            matchedTargetPlatforms
              .map((targetPlatform, index) => {
                const isLast = index === matchedTargetPlatforms.length - 1;
                return targetPlatform.value + (isLast ? "" : ", ");
              })
              .join("");
          hasAddedValue = true;
        }
      }
    });

    if (!hasAddedValue) {
      combinedString = "";
    }

    setFilterHeading(combinedString);
  };

  const buildDropdownPayload = (overrides = {}) => {
    const obj = {};
    if (selectedOpco?.length) obj["opcoId"] = selectedOpco;
    if (selectedProduct) obj["productId"] = [safeNumber(selectedProduct)];
    if (selectedVendor) obj["vendorId"] = [safeNumber(selectedVendor)];
    if (selectedEnvironment)
      obj["environmentId"] = [safeNumber(selectedEnvironment)];
    if (selectedVertical) obj["verticalId"] = [safeNumber(selectedVertical)];
    if (selectedSubNetwork)
      obj["supportedServicesId"] = [safeNumber(selectedSubNetwork)];
    if (selectedPlatform?.length) obj["platformId"] = selectedPlatform;
    if (selectedTargetPlatform)
      obj["targetPlatformId"] = [safeNumber(selectedTargetPlatform)];
    return { ...obj, ...overrides };
  };

  const getExodusAtGlanceGraphApiResource = async (filterObj = {}) => {
    const ExodusAtGlanceGraphApiResource =
      await GetExodusAtGlanceGraphApiResource(
        isPageLoad,
        filterObj,
        isToggleOn
      );
    setPlannedActivityResource(
      ExodusAtGlanceGraphApiResource?.ResultDtoCreate?.data
        ?.allPlannedActivityResources ?? []
    );
    setPaDropDown(
      ExodusAtGlanceGraphApiResource?.ResultDtoCreate?.data?.allPlannedActivityResources?.filter(
        (res: any) => res.id == 1
      )[0] ?? null
    );
    if (searchParams && searchParams["size"] !== 0) {
      updateFilterHeading(
        ExodusAtGlanceGraphApiResource?.ResultDtoCreate?.data
      );
    }
    // if (ExodusAtGlanceGraphApiResource.ResultDtoCreate) {
    //   setSelectedPlatform(
    //     ExodusAtGlanceGraphApiResource?.ResultDtoCreate?.data?.allPlatformAndBuildConstructions?.map(
    //       (res) => res.key
    //     ) ?? []
    //   );
    //   setDefaultPlatform(
    //     ExodusAtGlanceGraphApiResource?.ResultDtoCreate?.data?.allPlatformAndBuildConstructions?.map(
    //       (res) => res.key
    //     ) ?? []
    //   );
    //   const formattedtLCMAtGlanceApiResource = Object.entries(
    //     ExodusAtGlanceGraphApiResource.ResultDtoCreate
    //   );
    //   setLCMAtGlanceResource(formattedtLCMAtGlanceApiResource);
    // }
    if (ExodusAtGlanceGraphApiResource?.ResultDtoCreate?.data?.allOpcos) {
      const formattedtLCMAtGlanceApiOpCo = Object.entries(
        ExodusAtGlanceGraphApiResource?.ResultDtoCreate?.data?.allOpcos
      );
      setLCMAtGlanceOpCo(formattedtLCMAtGlanceApiOpCo);
    }
    if (ExodusAtGlanceGraphApiResource?.ResultDtoCreate?.data?.allProducts) {
      const formattedtLCMAtGlanceApiProduct = Object.entries(
        ExodusAtGlanceGraphApiResource?.ResultDtoCreate?.data?.allProducts
      );

      setLCMAtGlanceProduct(formattedtLCMAtGlanceApiProduct);
    }
    if (ExodusAtGlanceGraphApiResource?.ResultDtoCreate?.data?.allVendors) {
      const formattedtLCMAtGlanceApiVendor = Object.entries(
        ExodusAtGlanceGraphApiResource?.ResultDtoCreate?.data?.allVendors
      );

      setLCMAtGlanceVendor(formattedtLCMAtGlanceApiVendor);
    }
    if (
      ExodusAtGlanceGraphApiResource?.ResultDtoCreate?.data?.environmentResource
    ) {
      const formattedtLCMAtGlanceApiEnvironment = Object.entries(
        ExodusAtGlanceGraphApiResource?.ResultDtoCreate?.data
          ?.environmentResource
      );
      setLCMAtGlanceEnvironment(formattedtLCMAtGlanceApiEnvironment);
    }

    if (
      ExodusAtGlanceGraphApiResource?.ResultDtoCreate?.data?.resourceVertical
    ) {
      const formattedtLCMAtGlanceApiVerticalList = Object.entries(
        ExodusAtGlanceGraphApiResource?.ResultDtoCreate?.data?.resourceVertical
      );
      setLCMAtGlanceVertical(formattedtLCMAtGlanceApiVerticalList);
    }
    if (
      ExodusAtGlanceGraphApiResource?.ResultDtoCreate?.data
        ?.allPlatformAndBuildConstructions
    ) {
      const formattedtLCMAtGlanceApiTargetPlatform = Object.entries(
        ExodusAtGlanceGraphApiResource?.ResultDtoCreate?.data
          ?.allPlatformAndBuildConstructions
      );
      setLCMAtGlancePlatform(formattedtLCMAtGlanceApiTargetPlatform);
    }

    if (
      ExodusAtGlanceGraphApiResource?.ResultDtoCreate?.data
        ?.targetedPlatformAndBuildConstructions
    ) {
      const formattedtLCMAtGlanceApiVertical = Object.entries(
        ExodusAtGlanceGraphApiResource?.ResultDtoCreate?.data
          ?.targetedPlatformAndBuildConstructions
      );
      setLCMAtGlanceTargetPlatform(formattedtLCMAtGlanceApiVertical);
    }
  };
  const getOpcoGraphData = async (filters) => {
    setOpcoLoader(true);
    const graphData: any = await GetOpcoWisePercentage(
      isPageLoad,
      filters,
      undefined,
      isToggleOn
    );
    if (graphData?.data) {
      setVerticalGraphData(
        graphData?.data?.verticalFilterValueBasedOnFilter ?? []
      );
      setOpcoGraphData(
        graphData?.data?.opcowisePercentage?.map((data) => [
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

  const handleSelectFilter = (event: SelectChangeEvent) => {
    setSelectFilterOption(event.target.value as string);
  };

  const OpCoGraph = React.memo(() => {
    const handleBarClick = (data) => {
      const getId = LCMAtGlanceOpCo?.filter(
        (res) => res[1].value === data?.name
      )?.[0]?.[1];
      if (getId?.key) {
        const colorMap = {
          "#FFFF00": "amber",
          "#00c300": "green",
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
        const { opcoId, ...rest } = searchValues;
        const queryString: any = new URLSearchParams(rest).toString();

        const complianceType = isToggleOn
          ? "PlannedActivityCompliance"
          : "AssetCompliance";
        const url = `/exodusatglance/?opcoId=${
          getId.key
        }&colorCode=${colorCode}&complianceType=${complianceType}${
          Object.keys(queryString).length !== 0 ? `&${queryString}` : ""
        }`;
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
            <Typography sx={{ fontWeight: "bold" }}>
              Platform Migration Plan - Target March 2030
            </Typography>
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
                  value: data[`${name?.toLowerCase()}`],
                  nodeCount: data[`${name?.toLowerCase()}_node_count`],
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
                data: ["Green", "Amber", "Red"],
                itemGap: 15,
                top: "2%",
                formatter: (name) => {
                  const customNames = {
                    Green: `Meets Target`,
                    Amber: `Misses Target`,
                    Red: `No Plan`,
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
    const colorCode = searchParams.get("colorCode");
    const [legendSelected, setLegendSelected] = useState<any>({
      Green: !colorCode || colorCode === "green", // If no colorCode or if it's "green"
      Amber: !colorCode || colorCode === "amber", // If no colorCode or if it's "amber"
      Red: !colorCode || colorCode === "red", // If no colorCode or if it's "red"
    });
    const dataList = data?.paGraphDataDisplay
      ? [
          ...data?.paGraphDataDisplay?.daWithOutPaEntity,
          ...data?.paGraphDataDisplay?.platformMigrationRecords,
          ...data?.paGraphDataDisplay?.noPaDaRecords,
        ]
      : [];
    const [graphRecord, setGraphRecord] = useState<any>([]);

    useEffect(() => {
      const selectedList = Object.keys(legendSelected).filter(
        (key) => legendSelected[key]
      );
      setGraphRecord(
        dataList?.filter((res) =>
          selectedList.some(
            (item) =>
              item.toLowerCase() === res.compatibilityColorCode?.toLowerCase()
          )
        )
      );
    }, [legendSelected, searchParams]);

    const seriesData = graphRecord
      ? graphRecord?.map((record: any) => ({
          name: record.paCurrentDCFName,
          subName: record.padcfName ?? undefined,
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
      { name: "Amber", color: "#FFFF00" },
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
              {"Planned Activity Status"}
            </Typography>
          </Paper>
          {paLoader ? (
            <GraphicLoading
              height={"76.6vh"}
              isDarkMode={theme}
              isLoading={true}
            />
          ) : (
            // : graphRecord?.length === 0 ? (
            //   <NoDataAnimation
            //     height={"76.6vh"}
            //     isDarkMode={theme}
            //     message={"No Data Found"}
            //   />
            // )
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
                        <span style="font-weight: bold; padding-right: 2px;">Current DCF : </span>
                        <span style="max-width: 250px; white-space: normal; word-wrap: break-word;">${yAxisLabel} - </span>
                        <span style="padding-right: 2px; padding-left: 2px;">(Node:${nodeCount}) </span>
                      </div>
                      ${
                        subName !== "undefined" &&
                        subName !== undefined &&
                        subName !== ""
                          ? `<div style="display: block;align-items: center;">
                        <span style="font-weight: bold; padding-right: 2px;">Planned DCF : </span>
                        <span style="max-width: 250px; white-space: normal; word-wrap: break-word;">${
                          subName ?? "NA"
                        } - </span>
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
                        <span style="max-width: 250px; white-space: normal; word-wrap: break-word;">${
                          date ?? "NA"
                        }</span>
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
                data: ["Green", "Amber", "Red"],
                itemGap: 15,
                top: "2%",
                formatter: (name) => {
                  const customNames = {
                    Green: `Meets Target`,
                    Amber: `Misses Target`,
                    Red: `No Plan`,
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
              onLegendClick={(data: any) => setLegendSelected(data?.selected)}
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

  const handleOpcoFilter = async (value) => {
    await getExodusAtGlanceGraphApiResource(
      buildDropdownPayload({ opcoId: value })
    );
    const filterObj = {};
    if (selectedProduct) {
      filterObj["productId"] = [safeNumber(selectedProduct)];
    }
    if (selectedVendor) {
      filterObj["vendorId"] = [safeNumber(selectedVendor)];
    }
    if (value?.length !== 0) {
      filterObj["opcoId"] = value;
    }
    if (selectedPlatform) {
      filterObj["platformId"] = selectedPlatform;
    }
    if (selectedTargetPlatform) {
      filterObj["targetPlatformId"] = [safeNumber(selectedTargetPlatform)];
    }
    const graphData = await GetExodusAtGlanceGraph(filterObj, isToggleOn);
    const formattedGraphData = graphData?.ResultDtoCreate?.data;
    setIsFilterData(
      formattedGraphData?.opcowisePercentage?.length > 0 ? true : false
    );
  };

  const handleProductFilter = async (value) => {
    await getExodusAtGlanceGraphApiResource(
      buildDropdownPayload({
        productId: value ? [safeNumber(value)] : undefined,
      })
    );
    const filterObj = {};
    filterObj["productId"] = [safeNumber(value)];
    if (selectedSubNetwork) {
      filterObj["supportedServicesId"] = [safeNumber(selectedSubNetwork)];
    }
    if (selectedVendor) {
      filterObj["vendorId"] = [safeNumber(selectedVendor)];
    }
    if (selectedOpco) {
      filterObj["opcoId"] = selectedOpco;
    }
    if (selectedTargetPlatform) {
      filterObj["targetPlatformId"] = selectedTargetPlatform;
    }
    if (selectedPlatform) {
      filterObj["platformId"] = selectedPlatform;
    }
    const graphData = await GetExodusAtGlanceGraph(filterObj, isToggleOn);
    const formattedGraphData = graphData?.ResultDtoCreate?.data;
    setIsFilterData(
      formattedGraphData?.opcowisePercentage?.length > 0 ? true : false
    );
  };

  const handleVendorFilter = async (value) => {
    await getExodusAtGlanceGraphApiResource(
      buildDropdownPayload({
        vendorId: value ? [safeNumber(value)] : undefined,
      })
    );
    const filterObj = {};
    filterObj["vendorId"] = [safeNumber(value)];
    if (selectedSubNetwork) {
      filterObj["supportedServicesId"] = [safeNumber(selectedSubNetwork)];
    }
    if (selectedOpco) {
      filterObj["opcoId"] = selectedOpco;
    }
    if (selectedPlatform) {
      filterObj["platformId"] = selectedPlatform;
    }
    const graphData = await GetExodusAtGlanceGraph(filterObj, isToggleOn);
    const formattedGraphData = graphData?.ResultDtoCreate?.data;
    setIsFilterData(
      formattedGraphData?.opcowisePercentage?.length > 0 ? true : false
    );
  };

  const handleEnvironmentFilter = async (value) => {
    await getExodusAtGlanceGraphApiResource(
      buildDropdownPayload({
        environmentId: value ? [safeNumber(value)] : undefined,
      })
    );
    const filterObj = {};
    filterObj["environmentId"] = [safeNumber(value)];
    if (selectedSubNetwork) {
      filterObj["supportedServicesId"] = [safeNumber(selectedSubNetwork)];
    }
    if (selectedOpco) {
      filterObj["opcoId"] = selectedOpco;
    }
    if (selectedPlatform) {
      filterObj["platformId"] = selectedPlatform;
    }
    if (selectedVendor) {
      filterObj["vendorId"] = [safeNumber(selectedVendor)];
    }
    if (selectedProduct) {
      filterObj["productId"] = [safeNumber(selectedProduct)];
    }
    if (selectedVertical) {
      filterObj["verticalId"] = [safeNumber(selectedVertical)];
    }
    const graphData = await GetExodusAtGlanceGraph(filterObj, isToggleOn);
    const formattedGraphData = graphData?.ResultDtoCreate?.data;
    setIsFilterData(
      formattedGraphData?.opcowisePercentage?.length > 0 ? true : false
    );
  };

  const handleVerticalFilter = async (value) => {
    await getExodusAtGlanceGraphApiResource(
      buildDropdownPayload({
        verticalId: value ? [safeNumber(value)] : undefined,
      })
    );
    const filterObj = {};
    filterObj["verticalId"] = [safeNumber(value)];
    if (selectedSubNetwork) {
      filterObj["supportedServicesId"] = [safeNumber(selectedSubNetwork)];
    }
    if (selectedOpco) {
      filterObj["opcoId"] = selectedOpco;
    }
    if (selectedPlatform) {
      filterObj["platformId"] = selectedPlatform;
    }
    if (selectedVendor) {
      filterObj["vendorId"] = [safeNumber(selectedVendor)];
    }
    if (selectedProduct) {
      filterObj["productId"] = [safeNumber(selectedProduct)];
    }
    if (selectedEnvironment) {
      filterObj["environmentId"] = [safeNumber(selectedEnvironment)];
    }
    const graphData = await GetExodusAtGlanceGraph(filterObj, isToggleOn);
    const formattedGraphData = graphData?.ResultDtoCreate?.data;
    setIsFilterData(
      formattedGraphData?.opcowisePercentage?.length > 0 ? true : false
    );
  };

  const handleTargetPlatformFilter = async (value) => {
    await getExodusAtGlanceGraphApiResource(
      buildDropdownPayload({
        targetPlatformId: value ? [safeNumber(value)] : undefined,
      })
    );
    const filterObj = {};
    filterObj["targetPlatformId"] = [safeNumber(value)];
    if (selectedSubNetwork) {
      filterObj["supportedServicesId"] = [safeNumber(selectedSubNetwork)];
    }
    if (selectedOpco) {
      filterObj["opcoId"] = selectedOpco;
    }
    if (selectedPlatform) {
      filterObj["platformId"] = selectedPlatform;
    }
    if (selectedVendor) {
      filterObj["vendorId"] = [safeNumber(selectedVendor)];
    }
    if (selectedProduct) {
      filterObj["productId"] = [safeNumber(selectedProduct)];
    }
    const graphData = await GetExodusAtGlanceGraph(filterObj, isToggleOn);
    const formattedGraphData = graphData?.ResultDtoCreate?.data;
    setIsFilterData(
      formattedGraphData?.opcowisePercentage?.length > 0 ? true : false
    );
  };

  const handlePlatformFilter = async (value) => {
    await getExodusAtGlanceGraphApiResource(
      buildDropdownPayload({ platformId: value })
    );
    const filterObj = {};
    if (selectedProduct) {
      filterObj["productId"] = [safeNumber(selectedProduct)];
    }
    if (selectedVendor) {
      filterObj["vendorId"] = [safeNumber(selectedVendor)];
    }
    if (selectedSubNetwork) {
      filterObj["supportedServicesId"] = [safeNumber(selectedSubNetwork)];
    }
    if (selectedOpco) {
      filterObj["opcoId"] = selectedOpco;
    }
    if (selectedTargetPlatform) {
      filterObj["targetPlatformId"] = [safeNumber(selectedTargetPlatform)];
    }
    if (value?.length !== 0) {
      filterObj["platformId"] = value;
    }

    const graphData = await GetExodusAtGlanceGraph(filterObj, isToggleOn);

    const formattedGraphData = graphData?.ResultDtoCreate?.data;
    setIsFilterData(
      formattedGraphData?.opcowisePercentage?.length > 0 ? true : false
    );
  };

  const handleReset = () => {
    setSelectedOpco([]);
    setSelectedProduct(null);
    setSelectedVendor(null);
    setSelectedEnvironment(null);
    setSelectedVertical(null);
    setSelectedSubNetwork(null);
    setSelectedPlatform(defaultPlatform);
    setSelectedDate(undefined);
    setSelectedTargetPlatform(null);
    getExodusAtGlanceGraphApiResource();
    setShowCanvas(false);
  };

  const handleClear = () => {
    setSelectedOpco([]);
    setSelectedProduct(null);
    setSelectedVendor(null);
    setSelectedEnvironment(null);
    setSelectedVertical(null);
    setSelectedSubNetwork(null);
    setSelectedPlatform([]);
    setSelectedTargetPlatform(null);
    setSelectedDate(undefined);
    setIsFilterData(false);
    getExodusAtGlanceGraphApiResource({});
  };

  const callPAGraphApi = async () => {
    setPALoader(true);
    try {
      const paramsObj = Object.fromEntries(searchParams?.entries());
      const transformedObj: any = Object.keys(paramsObj).reduce((acc, key) => {
        const value = paramsObj[key];
        if (key === "selectedDate") {
          acc[key] = String(value);
        } else if (key !== "colorCode") {
          acc[key] = value
            ?.split(",")
            .map((val) => (val !== "" ? safeNumber(val) : 0));
        }

        return acc;
      }, {});

      if (transformedObj["productId"]) {
        setSelectedProduct(transformedObj["productId"]);
      }
      if (transformedObj["vendorId"]) {
        setSelectedVendor(transformedObj["vendorId"]);
      }
      if (transformedObj["platformId"]) {
        setSelectedPlatform(transformedObj["platformId"]);
      }
      if (transformedObj["targetPlatformId"]) {
        setSelectedTargetPlatform(transformedObj["targetPlatformId"]);
      }
      if (transformedObj["opcoId"]) {
        setSelectedOpco(transformedObj["opcoId"]);
      }
      const results = await GetExodusAtGlancePAGraph(
        {
          ...transformedObj,
          PlannedActivityResourceRuleId:
            paDropDown !== null ? [paDropDown.id] : [],
        },
        isToggleOn
      );
      setPAGraphDataDisplay(results?.data);
    } catch (error) {
      console.error("Error fetching PA graph data:", error);
    } finally {
      setPALoader(false);
    }
  };
  const handleFilter = (val?: any) => {
    setSelectedOpco([]);
    setSelectedProduct(null);
    setSelectedVendor(null);
    setSelectedVertical(null);
    setSelectedEnvironment(null);
    setSelectedTargetPlatform(null);
    setSelectedSubNetwork(null);
    setSelectedPlatform(defaultPlatform);
    setSelectedDate(undefined);
    const filterObj = {};
    if (selectedOpco?.length > 0) {
      filterObj["opcoId"] = selectedOpco;
    }
    if (selectedPlatform?.length > 0) {
      filterObj["platformId"] = val?.length > 0 ? val : selectedPlatform;
    }
    if (selectedProduct) {
      filterObj["productId"] = [selectedProduct];
    }
    if (selectedTargetPlatform) {
      filterObj["targetPlatformId"] = [selectedTargetPlatform];
    }
    if (selectedSubNetwork) {
      filterObj["supportedServicesId"] = [selectedSubNetwork];
    }
    if (selectedVendor) {
      filterObj["vendorId"] = [selectedVendor];
    }
    if (selectedEnvironment) {
      filterObj["environmentId"] = [selectedEnvironment];
    }
    if (selectedVertical) {
      filterObj["verticalId"] = [selectedVertical];
    }
    if (selectedDate) {
      const selectedDateString = selectedDate.toISOString();
      filterObj["selectedDate"] = selectedDateString;
    }
    const queryString = new URLSearchParams(filterObj).toString();
    const url = `/exodusatglance/?${queryString}`;

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
      if (transformedObj["vendorId"]) {
        setSelectedVendor(transformedObj["vendorId"]);
      }
      if (transformedObj["environmentId"]) {
        setSelectedEnvironment(transformedObj["environmentId"]);
      }
      if (transformedObj["verticalId"]) {
        setSelectedVertical(transformedObj["verticalId"]);
      }
      if (transformedObj["supportedServicesId"]) {
        setSelectedSubNetwork(transformedObj["supportedServicesId"]);
      }
      if (transformedObj["platformId"]) {
        setSelectedPlatform(transformedObj["platformId"]);
      }
      if (transformedObj["opcoId"]) {
        setSelectedOpco(transformedObj["opcoId"]);
      }
      if (transformedObj["targetPlatformId"]) {
        setSelectedTargetPlatform(transformedObj["targetPlatformId"]);
      }
    }
    handleReset();
    setShowCanvas(false);
    window.open(url, "_blank");
  };

  const checkIsDisable = () => {
    const requiredArrays = [selectedOpco, selectedPlatform];
    const requiredValues = [selectedProduct, selectedVendor];
    const isArrayValid = requiredArrays.some(
      (arr) => Array.isArray(arr) && arr.length > 0
    );
    const isValueValid = requiredValues.some(
      (val) => val !== 0 && val !== null && val !== undefined
    );
    // console.log("isFilterData", isFilterData);
    // return !(isArrayValid || isValueValid);
    return !isFilterData;
  };

  return (
    <ThemeProvider theme={theme}>
      <Drawer anchor={"right"} open={showCanvas} onClose={() => handleReset()}>
        <Box
          sx={{
            display: "flex",
            alignItems: "center",
            width: "400px",
            padding: "7px",
            bgcolor: "background.paper",
            color: "#e0e0e0",
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
              <MultiSingleSelect
                options={
                  LCMAtGlanceVendor?.map((val) => {
                    return {
                      key: val[1]?.key,
                      value: val[1]?.value,
                    };
                  }) ?? []
                }
                selectedValues={selectedVendor}
                onChange={(e) => {
                  setSelectedVendor(e);
                  handleVendorFilter(e);
                }}
                label="Vendor"
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
              <MultiSelectCheckmarks
                options={
                  LCMAtGlancePlatform?.map((val) => {
                    return {
                      key: val[1]?.key,
                      value: val[1]?.value,
                    };
                  }) ?? []
                }
                selectedValues={selectedPlatform}
                onChange={(e) => {
                  setSelectedPlatform(e);
                  handlePlatformFilter(e);
                }}
                label="Current Platform"
                isComponent={true}
                size="small"
                widthSize={360}
                darkTheme={darkMode}
              />
            </Box>

            <Box sx={{ width: "100%" }}>
              <MultiSingleSelect
                options={
                  LCMAtGlanceTargetPlatform?.map((val) => {
                    return {
                      key: val[1]?.key,
                      value: val[1]?.value,
                    };
                  }) ?? []
                }
                selectedValues={selectedTargetPlatform}
                onChange={(e) => {
                  setSelectedTargetPlatform(e);
                  handleTargetPlatformFilter(e);
                }}
                placeholder="Under Construction..."
                label="Target Platform"
                size="small"
                widthSize={360}
                darkTheme={darkMode}
              />
            </Box>
            {!isToggleOn && (
              <Box sx={{ width: "100%" }}>
                <MultiSingleSelect
                  options={
                    LCMAtGlanceEnvironment?.map((val) => ({
                      key: val[1]?.key,
                      value: val[1]?.value,
                    })) ?? []
                  }
                  selectedValues={selectedEnvironment}
                  onChange={(e) => {
                    setSelectedEnvironment(e);
                    handleEnvironmentFilter(e);
                  }}
                  label="Environment"
                  size="small"
                  widthSize={360}
                  darkTheme={darkMode}
                />
              </Box>
            )}
            {!isToggleOn && (
              <Box sx={{ width: "100%" }}>
                <MultiSingleSelect
                  options={
                    LCMAtGlanceVertical?.map((val) => ({
                      key: val[1]?.key,
                      value: val[1]?.value,
                    })) ?? []
                  }
                  selectedValues={selectedVertical}
                  onChange={(e) => {
                    setSelectedVertical(e);
                    handleVerticalFilter(e);
                  }}
                  label="Vertical"
                  size="small"
                  widthSize={360}
                  darkTheme={darkMode}
                />
              </Box>
            )}
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
                variant="outlined"
                color="warning"
                onClick={() => handleClear()}
              >
                Clear
              </Button>
              <Tooltip
                title={`${
                  !isFilterData ? "No Data found for applied filter." : ""
                }`}
                arrow
                placement="bottom"
              >
                <span>
                  <Button
                    variant="contained"
                    color="success"
                    onClick={() => handleFilter()}
                    disabled={checkIsDisable()}
                  >
                    Apply
                  </Button>
                </span>
              </Tooltip>
            </Box>
          </Stack>
        </Box>
      </Drawer>
      <Box
        sx={{
          width: "100%",
          minHeight: "92.2vh",
          padding: "16px 16px 16px 66px",
        }}
      >
        <Grid container spacing={2}>
          <Grid size={12}>
            <Stack
              direction="row"
              sx={{
                flexWrap: "wrap",
                alignItems: "flex-start",
                minHeight: "8vh",
                width: "100%",
                rowGap: "12px",
              }}
            >
              <Stack
                direction="row"
                sx={{
                  width: { xs: "100%", md: "100%", lg: "50%" },
                  rowGap: "8px",
                  columnGap: "8px",
                  flexWrap: "wrap",
                  alignItems: "center",
                  justifyContent: "space-between",
                }}
              >
                <Stack
                  direction="row"
                  sx={{
                    rowGap: "8px",
                    columnGap: "8px",
                    width: "100%",
                    flexWrap: "wrap",
                    alignItems: "center",
                    justifyContent: "space-between",
                  }}
                >
                  <Typography
                    variant="h4"
                    sx={{
                      margin: 0,
                      display: "flex",
                      alignItems: "center",
                      fontSize: { xs: "1.2rem", sm: "1.5rem", md: "1.8rem" },
                      color: (theme) => theme.palette.text.primary,
                    }}
                  >
                    <HtmlTooltip
                      title={
                        <React.Fragment>
                          {
                            "The KPI is evaluated based on the existence of a Platform Migration Plan for assets operating on an older Platform."
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
                        Exodus @Glance (Under Development)
                      </Button>
                    </HtmlTooltip>
                  </Typography>

                  {filterHeadingObj?.length > 0 && (
                    <Stack
                      direction="row"
                      sx={{
                        rowGap: "5px",
                        columnGap: "8px",
                        maxWidth: "100%",
                        flexWrap: "wrap",
                        alignItems: "center",
                        justifyContent: "space-between",
                      }}
                    >
                      {filterHeadingObj.map(({ title, value }) => (
                        <Tooltip
                          key={title}
                          title={
                            value.length > 3 ? (
                              <div
                                style={{
                                  maxWidth: "400px",
                                  maxHeight: "300px",
                                  overflow: "auto",
                                }}
                              >
                                <Typography
                                  variant="subtitle2"
                                  sx={{ fontWeight: "bold", mb: 1 }}
                                >
                                  {title} ({value.length})
                                </Typography>
                                {value.map((item, index) => (
                                  <Chip
                                    key={index}
                                    label={item}
                                    size="small"
                                    sx={{ m: "2px", fontSize: "11px" }}
                                  />
                                ))}
                              </div>
                            ) : (
                              ""
                            )
                          }
                          arrow
                          placement="bottom"
                        >
                          <Chip
                            sx={{
                              minWidth: "4rem",
                              maxWidth: "300px",
                              height: "auto",
                              cursor: "pointer",
                              "& .MuiChip-label": {
                                display: "block",
                                whiteSpace: "normal",
                                padding: "4px 8px",
                              },
                            }}
                            variant="outlined"
                            label={
                              value.length > 3
                                ? `${title} : ${value
                                    .slice(0, 3)
                                    .join(", ")} +${value.length - 3} more`
                                : `${title} : ${value.join(", ")}`
                            }
                            onClick={() => {}}
                          />
                        </Tooltip>
                      ))}
                    </Stack>
                  )}
                </Stack>
              </Stack>
              {searchParams["size"] === 0 && (
                <Stack
                  direction="row"
                  sx={{
                    gap: "15px",
                    width: {
                      xs: "100%",
                      md: "100%",
                      lg: "50%",
                      flexWrap: "wrap",
                      alignItems: "center",
                      justifyContent: { xs: "flex-end" },
                    },
                  }}
                >
                  <MultiSingleSelect
                    options={
                      LCMAtGlanceVendor?.map((val) => {
                        return {
                          key: val[1]?.key,
                          value: val[1]?.value,
                        };
                      }) ?? []
                    }
                    selectedValues={selectedVendor}
                    onChange={(e) => {
                      setSelectedVendor(e);
                      handleVendorFilter(e);
                    }}
                    label="Vendor"
                    size="small"
                    widthSize={250}
                    darkTheme={darkMode}
                  />
                  <MultiSelectCheckmarks
                    options={
                      LCMAtGlancePlatform?.map((val) => {
                        return {
                          key: val[1]?.key,
                          value: val[1]?.value,
                        };
                      }) ?? []
                    }
                    selectedValues={selectedPlatform}
                    onChange={(e) => {
                      setSelectedPlatform(e);
                      handlePlatformFilter(e);
                    }}
                    label="Current Platform"
                    isComponent={true}
                    size="small"
                    widthSize={250}
                    darkTheme={darkMode}
                  />
                  <button
                    className={`download-to-excel btn-danger ${
                      !isFilterData ? "disabledCursor" : ""
                    }`}
                    style={{
                      width: "6rem",
                      alignSelf: "center",
                    }}
                    title={`${
                      !isFilterData ? "No Data found for applied filter." : ""
                    }`}
                    disabled={isFilterData ? false : true}
                    onClick={() => handleFilter()}
                  >
                    Apply
                  </button>
                  <button
                    className="download-to-excel btn-secondary"
                    style={{
                      width: "6rem",
                      alignSelf: "center",
                    }}
                    onClick={() => handleReset()}
                  >
                    Clear
                  </button>
                  <Tooltip
                    title={
                      isToggleOn
                        ? "Click For Asset Compliance"
                        : "Click For PA Compliance"
                    }
                    arrow
                  >
                    <button
                      onClick={() => setIsToggleOn((prev) => !prev)}
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
                      <span style={{ fontWeight: "bold" }}>Compliance</span>
                      <div
                        style={{
                          width: "32px",
                          height: "16px",
                          borderRadius: "10px",
                          background: isToggleOn ? "#4caf50" : "#ccc",
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
                            left: isToggleOn ? "16px" : "2px",
                            transition: "0.2s",
                          }}
                        />
                      </div>
                    </button>
                  </Tooltip>
                  <FormControl
                    sx={{
                      minWidth: 120,
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
                        fontWeight: "bold",
                      }}
                      onClick={(e) => {
                        e.preventDefault();
                        setShowCanvas(!showCanvas);
                      }}
                    >
                      More Filters
                    </Button>
                  </FormControl>
                </Stack>
              )}
            </Stack>
          </Grid>
          <Grid size={12}>
            {/* <CustomSlides
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
              slides={[<OpCoGraph key={"OpCoGraph"} />]}
              autoplayDelay={3000}
            /> */}
            {searchParams.get("colorCode") ? (
              <PAGraphs
                key={"PAGraphs"}
                title={"PAGraphs"}
                data={{
                  paGraphDataDisplay: paGraphDataDisplay,
                }}
              />
            ) : (
              <OpCoGraph key={"OpCoGraph"} />
            )}
          </Grid>
        </Grid>
      </Box>
    </ThemeProvider>
  );
};

export default FlexboxGapStack;
