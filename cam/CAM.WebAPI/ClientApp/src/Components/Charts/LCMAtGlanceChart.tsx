import React, { useState, useEffect, useRef } from "react";
import Gauge from "./Gauge";
import { Col, Row } from "react-bootstrap";
import Carousel from "react-bootstrap/Carousel";
import { FaChevronLeft, FaChevronRight } from "react-icons/fa";
import {
  GetLCMAtGlanceGraphApiResource,
  GetLCMAtGlanceGraph,
  GetLCMAtGlanceGraphOverAll,
  GetLCMAtGlancePAGraph,
} from "../../Redux/Action/LookUp/LCMAtGlanceGraph/LCMAtGlanceGraphAction";
import Box from "@mui/material/Box";
import Stack from "@mui/material/Stack";
import InputLabel from "@mui/material/InputLabel";
import MenuItem from "@mui/material/MenuItem";
import FormControl from "@mui/material/FormControl";
import Select, { SelectChangeEvent } from "@mui/material/Select";
import {
  InputAdornment,
  Typography,
  Paper,
  Drawer,
  Divider,
  Card,
  Button,
  OutlinedInput,
  Chip,
  Theme,
} from "@mui/material";
import IconButton from "@mui/material/IconButton";
import { IoClose } from "react-icons/io5";
import { createTheme, ThemeProvider } from "@mui/material/styles";
import { useTheme } from "../../Context/ThemeContext";
import { BarChart } from "@mui/x-charts/BarChart";
import { HighlightItemData, HighlightScope } from "@mui/x-charts/context";
import { PieChart } from "@mui/x-charts/PieChart";
import "./chartStyle.css";
import { TbCircleArrowLeft, TbCircleArrowRight } from "react-icons/tb";
import { BsFillPauseFill, BsFillPlayFill } from "react-icons/bs";
import EPieCharts from "../ECharts/EPieCharts";
import EBarCharts from "../ECharts/EBarCharts";
import CustomEBarChart from "../ECharts/CustomEBarChart";
import { useAuth } from "../../Hook/useAuth";
import CustomEStackedBarChart from "../ECharts/CustomEStackedBarChart";
import { BiSolidFilterAlt } from "react-icons/bi";
import { useSearchParams } from "react-router-dom";
import CustomSwiper from "../Slider/CustomSlides";
import CustomEStackedPieChart from "../ECharts/CustomEStackedPieChart";
import CustomEGaugeChart from "../ECharts/CustomEGaugeChart";
import CustomCarousel from "../Slider/CustomCarousel";
import CustomSlides from "../Slider/CustomSlides";
import DatePicker from "react-datepicker";
import styles from "./DatePicker.module.css";
import { safeNumber } from "../../Hook/Common";

const ITEM_HEIGHT = 48;
const ITEM_PADDING_TOP = 8;
const MenuProps = {
  PaperProps: {
    style: {
      maxHeight: ITEM_HEIGHT * 4.5 + ITEM_PADDING_TOP,
      width: 250,
    },
  },
} as any;

const LCMAtGlanceChart: React.FC = () => {
  const [searchParams] = useSearchParams();
  const [filterHeading, setFilterHeading] = useState("");
  const { isPermesso } = useAuth();
  const lightTheme = createTheme({
    palette: {
      mode: "light",
      primary: {
        main: "#1976d2",
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
      background: {
        default: "#121212",
      },
    },
  });
  const { darkMode, selectDarkMode } = useTheme();
  const theme = darkMode ? darkTheme : lightTheme;
  const carouselRef = useRef(null);
  const [index, setIndex] = useState(0);
  const [paused, setPaused] = useState(false);
  const [showCanvas, setShowCanvas] = useState(false);
  const [selectedDate, setSelectedDate] = useState<Date | null | undefined>(
    undefined
  );

  const [LCMAtGlanceGraphDataOverAll, setLCMAtGlanceGraphDataOverAll] =
    useState<any>();
  const [LCMAtGlanceGraphData, setLCMAtGlanceGraphData] = useState<any>();
  const [LCMAtGlancePAGraphData, setLCMAtGlancePAGraphData] = useState<any>();
  const [LCMAtGlanceResource, setLCMAtGlanceResource] = useState<any>();

  const [opCoWiseGraphData, setOpCoWiseGraphData] = useState<any>();
  const [overAllOpCoWiseGraphData, setOverAllOpCoWiseGraphData] =
    useState<any>();
  const [productWiseGraphData, setProductWiseGraphData] = useState<any>();
  const [subNetworkWiseGraphData, setSubNetworkWiseGraphData] = useState<any>();

  const [formattedGraphData, setFormattedGraphData] = useState<any>();
  const [formattedGraphPAData, setFormattedGraphPAData] = useState<any>();
  const [productWiseFormattedGraphData, setProductWiseFormattedGraphData] =
    useState<any>();
  const [
    subNetworkWiseFormattedGraphData,
    setSubNetworkWiseFormattedGraphData,
  ] = useState<any>();
  const [subNetworkPieChartData, setSubNetworkPieChartData] = useState<any>();

  const [LCMAtGlanceOpCo, setLCMAtGlanceOpCo] = useState<any>();
  const [LCMAtGlanceProduct, setLCMAtGlanceProduct] = useState<any>();
  const [LCMAtGlanceSubNetwork, setLCMAtGlanceSubNetwork] = useState<any>();
  const [LCMAtGlanceVertical, setLCMAtGlanceVertical] = useState<any>();

  const [selectedOpco, setSelectedOpco] = useState<string[]>([]);
  const [selectedProduct, setSelectedProduct] = useState("");
  const [selectedSubNetwork, setSelectedSubNetwork] = useState("");
  const [selectedVertical, setSelectedVertical] = useState<string[]>([]);

  const getLCMAtGlanceGraphApiResource = async () => {
    const LCMAtGlanceGraphApiResource = await GetLCMAtGlanceGraphApiResource();
    if (LCMAtGlanceGraphApiResource.ResultDtoCreate) {
      const formattedtLCMAtGlanceApiResource = Object.entries(
        LCMAtGlanceGraphApiResource.ResultDtoCreate
      );
      setLCMAtGlanceResource(formattedtLCMAtGlanceApiResource);
      if (searchParams) {
        updateFilterHeading(LCMAtGlanceGraphApiResource?.ResultDtoCreate?.data);
      }
    }
    if (LCMAtGlanceGraphApiResource?.ResultDtoCreate?.data?.allOpcos) {
      const formattedtLCMAtGlanceApiOpCo = Object.entries(
        LCMAtGlanceGraphApiResource?.ResultDtoCreate?.data?.allOpcos
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
  const getLCMAtGlanceGraph = async (filters) => {
    const graphData = await GetLCMAtGlanceGraph(filters);
    setLCMAtGlanceGraphData(graphData?.ResultDtoCreate);

    setOpCoWiseGraphData(graphData?.ResultDtoCreate?.data?.opcoWisePercentage);

    setProductWiseGraphData(
      graphData?.ResultDtoCreate?.data?.productWisePercentage
    );
    setSubNetworkWiseGraphData(
      graphData?.ResultDtoCreate?.data?.subnetworkWisePercentage
    );
  };

  const getLCMAtGlancePAGraph = async (filters) => {
    // console.log("filter", filters, searchParams);
    const graphData: any = await GetLCMAtGlancePAGraph(filters, true);
    // console.log("GetLCMAtGlancePAGraph", graphData);
    setLCMAtGlancePAGraphData(graphData?.ResultDtoCreate);
  };

  const getLCMAtGlanceGraphOverAll = async () => {
    const graphDataOverAll = await GetLCMAtGlanceGraphOverAll();
    setLCMAtGlanceGraphDataOverAll(graphDataOverAll?.ResultDtoCreate);

    setOverAllOpCoWiseGraphData(graphDataOverAll?.ResultDtoCreate?.data);
  };

  useEffect(() => {
    selectDarkMode(true);

    return () => {
      selectDarkMode(false);
    };
  }, []);

  useEffect(() => {
    if (isPermesso && (!searchParams || searchParams.toString() === "")) {
      if (!LCMAtGlanceGraphData?.data) {
        getLCMAtGlanceGraph({});
      }
      // if (!LCMAtGlancePAGraphData?.data) {
      //   getLCMAtGlancePAGraph({});
      // }
      if (!LCMAtGlanceResource?.length) {
        getLCMAtGlanceGraphApiResource();
      }
      if (!LCMAtGlanceGraphDataOverAll?.data) {
        getLCMAtGlanceGraphOverAll();
      }
    }
  }, [isPermesso]);

  useEffect(() => {
    const paramsObj = Object.fromEntries(searchParams.entries());
    // console.log("searchParams", searchParams, paramsObj);
    if (isPermesso && searchParams && searchParams["size"] !== 0) {
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
      getLCMAtGlanceGraph(transformedObj);
      if (!LCMAtGlancePAGraphData?.data) {
        getLCMAtGlancePAGraph(transformedObj);
      }
      if (!LCMAtGlanceResource?.length) {
        getLCMAtGlanceGraphApiResource();
      }
      if (!LCMAtGlanceGraphDataOverAll?.data) {
        getLCMAtGlanceGraphOverAll();
      }
    }
  }, [isPermesso, searchParams]);

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

    // console.log("paramsObj", paramsObj);
    // console.log("transformedObjKeys", transformedObjKeys);
    transformedObjKeys.forEach((key, index) => {
      if (key === "opcoId" && transformedObj["opcoId"]) {
        const opco = data?.allOpcos?.find(
          (opco) => opco.key === safeNumber(transformedObj["opcoId"])
        );
        if (opco) {
          if (hasAddedValue) combinedString += " , ";
          combinedString += "Opco: " + opco.value;
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
      } else if (
        key === "verticalResponsibleId" &&
        transformedObj["verticalResponsibleId"]
      ) {
        const vertical = data?.allVerticalResponse?.find(
          (vertical) =>
            vertical.key === safeNumber(transformedObj["verticalResponsibleId"])
        );
        if (vertical) {
          if (hasAddedValue) combinedString += " , ";
          combinedString += "Vertical: " + vertical.value;
          hasAddedValue = true;
        }
      }
    });

    if (!hasAddedValue) {
      combinedString = "";
    }

    setFilterHeading(combinedString);
  };

  // console.log("isPermesso", isPermesso);
  // console.log("LCMAtGlancePAGraphData", LCMAtGlancePAGraphData);
  // console.log("Products", LCMAtGlanceGraphData?.data?.productWisePercentage);
  useEffect(() => {
    if (subNetworkWiseGraphData?.length) {
      const formatSubNetwork = subNetworkWiseGraphData?.map((subnetwork) => ({
        id: subnetwork?.supportedServicesId,
        value: subnetwork?.greenCompatibilityPercentage,
        label: subnetwork?.supportedServicesDescription,
      }));

      const subNetworkPieChartData = subNetworkWiseGraphData?.map(
        (subnetwork) => ({
          value: subnetwork?.greenCompatibilityPercentage,
          name: subnetwork?.supportedServicesDescription,
        })
      );

      setSubNetworkPieChartData(subNetworkPieChartData);

      setSubNetworkWiseFormattedGraphData(formatSubNetwork);
    }
  }, [subNetworkWiseGraphData]);
  useEffect(() => {
    if (productWiseGraphData?.length) {
      const formatProduct = productWiseGraphData?.map((product) => ({
        id: product?.productId,
        value: product?.greenCompatibilityPercentage,
        label: product?.productName,
      }));
      setProductWiseFormattedGraphData(formatProduct);
    }
  }, [productWiseGraphData]);

  useEffect(() => {
    const data = opCoWiseGraphData;
    if (data?.length) {
      setFormattedGraphData(
        data.map((data) => [
          data?.opCoDescrption,
          [
            safeNumber(data?.greenCompatibilityPercentage),
            safeNumber(data?.greenNodeCount),
            safeNumber(data?.amberCompatibilityPercentage),
            safeNumber(data?.amberNodeCount),
            safeNumber(data?.redCompatibilityPercentage),
            safeNumber(data?.redNodeCount),
          ],
        ])
      );
    }
  }, [opCoWiseGraphData]);

  const transformedData = formattedGraphData?.map(([name, values]) => ({
    category: name,
    green: values[0],
    green_node_count: values[1],
    amber: values[2],
    amber_node_count: values[3],
    red: values[4],
    red_node_count: values[5],
  }));

  const handleFilter = () => {
    setSelectedOpco([]);
    setSelectedProduct("");
    setSelectedSubNetwork("");
    setSelectedVertical([]);
    setSelectedDate(undefined);
    // setLCMAtGlanceGraphData([]);
    // setFormattedGraphData([]);
    const filterObj = {};
    if (selectedProduct) {
      filterObj["productId"] = [safeNumber(selectedProduct)];
    }
    if (selectedSubNetwork) {
      filterObj["supportedServicesId"] = [safeNumber(selectedSubNetwork)];
    }
    if (selectedOpco) {
      // Initialize opcoId as an array
      filterObj["opcoId"] = [];

      selectedOpco.forEach((val) => {
        const getId = LCMAtGlanceOpCo?.filter(
          (res) => res.opCoDescrption === val
        )?.[0];

        if (getId?.opcoId) {
          // Push each found opcoId into the opcoId array
          filterObj["opcoId"].push(safeNumber(getId.opcoId));
        }
      });
    }
    if (selectedVertical) {
      // Initialize verticalResponsibleId as an array
      filterObj["verticalResponsibleId"] = [];

      selectedVertical.forEach((val) => {
        const getId = LCMAtGlanceVertical?.filter(
          (res) => res[1].value === val
        )?.[0]?.[1];

        if (getId?.key) {
          // Push each found key into the verticalResponsibleId array
          filterObj["verticalResponsibleId"].push(safeNumber(getId.key));
        }
      });
    }
    if (selectedDate) {
      const selectedDateString = selectedDate.toISOString();

      filterObj["selectedDate"] = selectedDateString;
    }
    if (selectedDate) {
      const selectedDateString = selectedDate.toISOString();

      filterObj["selectedDate"] = selectedDateString;
    }

    const queryString = new URLSearchParams(filterObj).toString();
    // console.log("queryString", queryString);
    const url = `/lcmatglance/?${queryString}`;

    const paramsObj = Object.fromEntries(searchParams.entries());
    if (paramsObj) {
      const transformedObj = Object.keys(paramsObj).reduce((acc, key) => {
        const value = paramsObj[key];

        acc[key] = [safeNumber(value)];
        return acc;
      }, {});
      if (transformedObj["productId"]) {
        setSelectedProduct(transformedObj["productId"]);
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
    }
    setShowCanvas(false);

    window.open(url, "_blank");
    // getLCMAtGlanceGraph(filterObj);
  };

  const handleReset = () => {
    setLCMAtGlanceGraphData([]);
    setFormattedGraphData([]);
    setSelectedOpco([]);
    setSelectedProduct("");
    setSelectedSubNetwork("");
    setSelectedVertical([]);
    setSelectedDate(undefined);
    getLCMAtGlanceGraph({});
    getLCMAtGlancePAGraph({});
    getLCMAtGlanceGraphApiResource();
    setShowCanvas(false);
  };

  const handleOpcoFilter = async (value) => {
    const filterObj = {};
    filterObj["opcoId"] = [safeNumber(value)];

    if (selectedProduct) {
      filterObj["productId"] = [safeNumber(selectedProduct)];
    }
    if (selectedSubNetwork) {
      filterObj["supportedServicesId"] = [safeNumber(selectedSubNetwork)];
    }
    if (selectedVertical) {
      filterObj["verticalResponsibleId"] = [safeNumber(selectedVertical)];
    }

    const graphData = await GetLCMAtGlanceGraph(filterObj);
    const formattedGraphData = graphData?.ResultDtoCreate?.data;

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

    if (!selectedVertical) {
      setLCMAtGlanceVertical(Object.entries(filterVertical));
    }
  };

  const handleProductFilter = async (value) => {
    const filterObj = {};
    filterObj["productId"] = [safeNumber(value)];
    if (selectedOpco) {
      filterObj["opcoId"] = [safeNumber(selectedOpco)];
    }
    if (selectedSubNetwork) {
      filterObj["supportedServicesId"] = [safeNumber(selectedSubNetwork)];
    }
    if (selectedVertical) {
      filterObj["verticalResponsibleId"] = [safeNumber(selectedVertical)];
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

    if (!selectedVertical) {
      setLCMAtGlanceVertical(Object.entries(filterVertical));
    }
  };

  const handleSubNetworkFilter = async (value) => {
    const filterObj = {};
    filterObj["supportedServicesId"] = [safeNumber(value)];
    if (selectedProduct) {
      filterObj["productId"] = [safeNumber(selectedProduct)];
    }
    if (selectedVertical) {
      filterObj["verticalResponsibleId"] = [safeNumber(selectedVertical)];
    }
    if (selectedOpco) {
      filterObj["opcoId"] = [safeNumber(selectedOpco)];
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

    if (!selectedVertical) {
      setLCMAtGlanceVertical(Object.entries(filterVertical));
    }
  };

  const handleVerticalFilter = async (value) => {
    const filterObj = {};
    filterObj["verticalResponsibleId"] = [safeNumber(value)];
    if (selectedProduct) {
      filterObj["productId"] = [safeNumber(selectedProduct)];
    }
    if (selectedSubNetwork) {
      filterObj["supportedServicesId"] = [safeNumber(selectedSubNetwork)];
    }
    if (selectedOpco) {
      filterObj["opcoId"] = [safeNumber(selectedOpco)];
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

  const handleSelect = (selectedIndex) => {
    setIndex(selectedIndex);
  };

  const goToPrev = () => {
    if (carouselRef.current) {
      const prevIndex = index === 0 ? 2 : index - 1; // Adjust number for total slides
      setIndex(prevIndex);
    }
  };

  const goToNext = () => {
    if (carouselRef.current) {
      const nextIndex = index === 2 ? 0 : index + 1; // Adjust number for total slides
      setIndex(nextIndex);
    }
  };

  const togglePausePlay = () => {
    setPaused(!paused);
  };

  // console.log("LCMAtGlancePAGraphData", LCMAtGlancePAGraphData);

  const OpCoGraph = () => {
    const xLabels = LCMAtGlancePAGraphData?.data
      ? [
          ["0", ""], // Add the initial element
          ...LCMAtGlancePAGraphData?.data?.paRelatedDropDown?.deliveryStatus[0]?.map(
            (res: any) => [String(res.key), res.value] // Map each object to a [key, value] array
          ),
          [
            `${
              LCMAtGlancePAGraphData?.data?.paRelatedDropDown?.deliveryStatus[0]
                ?.length + 1
            }`,
            "",
          ], // Add the initial element
        ]
      : [];
    const xLabelData = LCMAtGlancePAGraphData?.data
      ? [
          "",
          ...LCMAtGlancePAGraphData?.data?.paRelatedDropDown?.deliveryStatus[0]?.map(
            (res: any) => res.value
          ),
          "",
        ]
      : [];
    // console.log("xLabels", xLabels);
    const yLabels = LCMAtGlancePAGraphData?.data
      ? LCMAtGlancePAGraphData?.data?.paRecords?.map(
          (record: any) => record.paCurrentDCName // Unique y-axis labels (dcName)
        )
      : [];
    // console.log("xLabels", xLabels);
    // console.log("xLabelData", xLabelData);
    // const getXIndex = (key: string) => xLabels.indexOf(key); // Map string to its index

    // Function to generate random hex colors
    const generateRandomColor = () => {
      const letters = "0123456789ABCDEF";
      let color = "#";
      for (let i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
      }
      return color;
    };
    // Generate a list of unique colors for the series data
    const generateUniqueColors = (count: number) => {
      const colors = new Set<string>();
      while (colors.size < count) {
        colors.add(generateRandomColor());
      }
      return Array.from(colors);
    };

    // Generate the unique colors for the bars
    const colors = generateUniqueColors(
      LCMAtGlancePAGraphData?.data?.paRecords?.length + 10
    );
    return (
      <div
        style={{
          justifyContent: "center",
          display: "block",
          height: "100%",
          width: "100%",
        }}
      >
        {LCMAtGlanceGraphData?.data &&
          LCMAtGlanceGraphDataOverAll?.data &&
          LCMAtGlanceResource &&
          transformedData?.length && (
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
                  {!selectedOpco
                    ? "OpCo Compliance"
                    : "Planned Activity Status"}
                </Typography>
              </Paper>
              {searchParams.get("opcoId") &&
              selectedOpco &&
              LCMAtGlancePAGraphData?.data !== null ? (
                <CustomEBarChart
                  width="100vh"
                  height="572px"
                  isDarkMode={darkMode}
                  seriesData={[
                    {
                      type: "bar",
                      data: LCMAtGlancePAGraphData?.data?.paRecords?.map(
                        (record: any, index: number) => {
                          const idx = xLabels.findIndex(
                            (label) => label[0] == record.paDelivertyStatusId
                          );
                          return {
                            value: idx + 1,
                            itemStyle: { color: colors[index] },
                          };
                        }
                      ),
                    },
                  ]}
                  tooltip={{
                    trigger: "axis",
                    formatter: (params: any) => {
                      const labelWithHtml = params[0].name;
                      const value = params[0].value;

                      /// Replace <b class="text-lowercase"> with custom text for emphasis
                      const plainLabel = labelWithHtml
                        .replace(
                          /<b class="text-lowercase">/gi,
                          "<span style='font-weight:bold;'>"
                        )
                        .replace(/<\/b>/gi, "</span>");

                      return `${plainLabel}`;
                    },
                    axisPointer: {
                      type: "shadow",
                    },
                  }}
                  // legend={{
                  //   data: ["Green", "Amber", "Red"],
                  //   itemGap: 5,
                  //   top: "2%",
                  // }}
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
                      data: yLabels, // Y-axis labels (dcName)
                      axisLabel: {
                        formatter: (label: string) => label.split("<b ")[0], // Trim long labels if necessary
                      },
                    },
                  ]}
                  xAXis={[
                    {
                      type: "value",
                      data: xLabelData,
                      min: 1,
                      max: xLabelData?.length - 1,
                      axisLabel: {
                        formatter: (value: number) => {
                          return xLabelData[value - 1] ?? "";
                        },
                      },
                    },
                  ]}
                  dataZoom={[
                    {
                      show: true,
                      top: "82%",
                      start: 0,
                      end: 100,
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
                      end: 6,
                      showDataShadow: false,
                      showDetail: false,
                      left: "92%",
                    },
                  ]}
                />
              ) : (
                <CustomEBarChart
                  width="100vh"
                  height="572px"
                  isDarkMode={darkMode}
                  namesList={transformedData?.map((data) => data.category)}
                  seriesData={[
                    { name: "Green", color: "#00c300" },
                    { name: "Amber", color: "#FFFF00" },
                    { name: "Red", color: "#e60000" },
                  ].map(({ name, color }) => ({
                    name: name,
                    type: "bar",
                    data: transformedData.map((data) => ({
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
                  // tooltip={{
                  //   trigger: "item",
                  //   axisPointer: {
                  //     type: "shadow",
                  //     label: {
                  //       show: true,
                  //     },
                  //   },
                  //   formatter: (params) => {
                  //     // Customize tooltip content
                  //     // console.log(params);
                  //     const { name, value, seriesName, data } = params;
                  //     return `
                  //     <div style="font-family: Arial, sans-serif;font-size: 12px;line-height: 1.5;text-align: left; min-width: 7rem;">
                  //       <div style="margin-bottom: 10px;padding-bottom: 6px;display: flex;justify-content: space-between;align-items: center; border-bottom-style: ridge;">
                  //         <span style="font-weight: bold;color: #666;">${name}</span>
                  //         <span style="background: #444;color: #fff;padding: 2px 5px;border-radius: 3px;">${data.value}%</span>
                  //       </div>
                  //       <div style="display: flex;align-items: center;">
                  //         <span style="font-weight: bold;">Nodes : </span>
                  //         <span >${data.nodeCount}</span>
                  //       </div>
                  //     </div>
                  //     `;
                  //   },
                  // }}
                  tooltip={{
                    trigger: "item", // Trigger tooltip for all series in the same category
                    axisPointer: {
                      type: "shadow", // Display shadow to highlight the category
                    },
                    formatter: (params) => {
                      // Determine if we are hovering over a category (multiple bars) or a single item (one bar)
                      const isHoveringOnCategory = params.length > 1;
                      const triggerType = isHoveringOnCategory
                        ? "axis"
                        : "item";
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
                        Green: "EOM > Today",
                        Amber: "EOS > Today > EOM",
                        Red: "Today > EOS",
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
                      data: transformedData?.map((data) => data.category),
                    },
                  ]}
                  dataZoom={[
                    {
                      show: true,
                      start: 0,
                      end: 10,
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
          )}
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
        {LCMAtGlanceGraphData?.data &&
          LCMAtGlanceGraphDataOverAll?.data &&
          LCMAtGlanceResource &&
          transformedData?.length && (
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

              <CustomEStackedBarChart
                width="100vh"
                height="572px"
                isDarkMode={darkMode}
                colorMap={{
                  green: "#00c300",
                  amber: "#FFFF00",
                  red: "#e60000",
                }}
                rawData={[
                  LCMAtGlanceGraphData?.data?.productWisePercentage?.map(
                    (product: any) =>
                      safeNumber(product.greenCompatibilityPercentage)
                  ),
                  LCMAtGlanceGraphData?.data?.productWisePercentage?.map(
                    (product: any) =>
                      safeNumber(product.amberCompatibilityPercentage)
                  ),
                  LCMAtGlanceGraphData?.data?.productWisePercentage?.map(
                    (product: any) =>
                      safeNumber(product.redCompatibilityPercentage)
                  ),
                ]}
                seriesData={{
                  data: LCMAtGlanceGraphData?.data?.productWisePercentage,
                  name: ["Green", "Amber", "Red"],
                  color: ["#00c300", "#FFFF00", "#e60000"],
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
                  data: ["Green", "Amber", "Red"],
                  itemGap: 15,
                  top: "2%",
                  formatter: (name) => {
                    const customNames = {
                      Green: "EOM > Today",
                      Amber: "EOS > Today > EOM",
                      Red: "Today > EOS",
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
                    data: LCMAtGlanceGraphData?.data?.productWisePercentage?.map(
                      (data) => data.productName
                    ),
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
            </>
          )}
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
        {LCMAtGlanceGraphData?.data &&
          LCMAtGlanceGraphDataOverAll?.data &&
          LCMAtGlanceResource &&
          subNetworkPieChartData &&
          transformedData?.length && (
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
              <CustomEStackedPieChart
                width="100vh"
                height="572px"
                isDarkMode={darkMode}
                seriesData={subNetworkPieChartData?.map((data) => ({
                  name: data?.name,
                  children: subNetworkWiseGraphData
                    ?.filter(
                      (res) =>
                        res?.["supportedServicesDescription"] === data?.name
                    )
                    ?.flatMap((res) => [
                      {
                        name: res?.["greenCompatibilityPercentage"],
                        value: 1,
                        itemStyle: { color: "#00c300" },
                      },
                      {
                        name: res?.["amberCompatibilityPercentage"],
                        value: 1,
                        itemStyle: { color: "#FFFF00" },
                      },
                      {
                        name: res?.["redCompatibilityPercentage"],
                        value: 1,
                        itemStyle: { color: "#e60000" },
                      },
                    ]),
                }))}
              />
            </>
          )}
      </div>
    );
  };
  function getStyles(
    name: string,
    personName: readonly string[],
    theme: Theme
  ) {
    return {
      fontWeight: personName.includes(name)
        ? theme.typography.fontWeightMedium
        : theme.typography.fontWeightRegular,
    };
  }

  const handleChange = (value, type: string) => {
    // console.log(value);
    type === "opco" && setSelectedOpco(value);
    type === "vertical" && setSelectedVertical(value);
  };
  return (
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
            sx={{
              width: "100%",
              textAlign: "left",
            }}
            spacing={2}
          >
            <Box sx={{ width: "100%" }}>
              <FormControl fullWidth>
                <InputLabel id="demo-multiple-chip-label">Opco</InputLabel>
                <Select
                  labelId="demo-multiple-chip-label"
                  id="demo-multiple-chip"
                  multiple
                  value={selectedOpco}
                  onChange={(e) => {
                    // console.log(
                    //   e.target.value,
                    //   LCMAtGlanceOpCo?.map((val) => val[1])
                    // );
                    handleChange(e.target.value, "opco");
                    handleOpcoFilter(e.target.value);
                  }}
                  endAdornment={
                    selectedOpco?.length !== 0 && (
                      <InputAdornment position="end">
                        <IconButton
                          onClick={() => {
                            setSelectedOpco([]);
                            if (
                              !selectedProduct &&
                              !selectedSubNetwork &&
                              !selectedVertical
                            ) {
                              getLCMAtGlanceGraphApiResource();
                            }
                          }}
                          edge="end"
                        >
                          <IoClose />
                        </IconButton>
                      </InputAdornment>
                    )
                  }
                  IconComponent={
                    selectedOpco?.length !== 0 ? () => null : undefined
                  }
                  input={
                    <OutlinedInput id="select-multiple-chip" label="Chip" />
                  }
                  renderValue={(selected) => (
                    <Box sx={{ display: "flex", flexWrap: "wrap", gap: 0.5 }}>
                      {selected?.map((value) => (
                        <Chip key={value} label={value} />
                      ))}
                    </Box>
                  )}
                  MenuProps={MenuProps}
                >
                  {LCMAtGlanceOpCo?.length &&
                    LCMAtGlanceOpCo?.map((opco) => (
                      <MenuItem key={opco[1].value} value={opco[1].value}>
                        {opco[1].value}
                      </MenuItem>
                    ))}
                </Select>
              </FormControl>
            </Box>
            <Box sx={{ width: "100%" }}>
              <FormControl fullWidth>
                <InputLabel id="demo-multiple-chip-label">Vertical</InputLabel>
                <Select
                  labelId="demo-multiple-chip-label"
                  id="demo-multiple-chip"
                  multiple
                  value={selectedVertical}
                  onChange={(e) => {
                    handleChange(e.target.value, "vertical");
                    handleVerticalFilter(e.target.value);
                  }}
                  endAdornment={
                    selectedVertical?.length !== 0 && (
                      <InputAdornment position="end">
                        <IconButton
                          onClick={() => {
                            setSelectedVertical([]);
                            if (
                              !selectedProduct &&
                              !selectedSubNetwork &&
                              !selectedOpco
                            ) {
                              getLCMAtGlanceGraphApiResource();
                            }
                          }}
                          edge="end"
                        >
                          <IoClose />
                        </IconButton>
                      </InputAdornment>
                    )
                  }
                  IconComponent={
                    selectedVertical?.length !== 0 ? () => null : undefined
                  }
                  input={
                    <OutlinedInput id="select-multiple-chip" label="Chip" />
                  }
                  renderValue={(selected) => (
                    <Box sx={{ display: "flex", flexWrap: "wrap", gap: 0.5 }}>
                      {selected?.map((value) => (
                        <Chip key={value} label={value} />
                      ))}
                    </Box>
                  )}
                  MenuProps={MenuProps}
                >
                  {LCMAtGlanceVertical?.map((val) => val[1]?.value).map(
                    (name) => (
                      <MenuItem
                        key={name}
                        value={name}
                        style={getStyles(name, selectedVertical, theme)}
                      >
                        {name}
                      </MenuItem>
                    )
                  )}
                </Select>
              </FormControl>
            </Box>
            <Box sx={{ width: "100%" }}>
              <FormControl fullWidth>
                <InputLabel id="demo-simple-select-standard-label">
                  Product
                </InputLabel>
                <Select
                  labelId="demo-multiple-name-label"
                  id="demo-multiple-name"
                  // multiple
                  value={selectedProduct}
                  onChange={(e) => {
                    setSelectedProduct(e.target.value);
                    handleProductFilter(e.target.value);
                  }}
                  label="Product"
                  endAdornment={
                    selectedProduct && (
                      <InputAdornment position="end">
                        <IconButton
                          onClick={() => {
                            setSelectedProduct("");
                            if (
                              !selectedOpco &&
                              !selectedSubNetwork &&
                              !selectedVertical
                            ) {
                              getLCMAtGlanceGraphApiResource();
                            }
                          }}
                          edge="end"
                        >
                          <IoClose />
                        </IconButton>
                      </InputAdornment>
                    )
                  }
                  IconComponent={selectedProduct ? () => null : undefined}
                  MenuProps={MenuProps}
                >
                  {LCMAtGlanceProduct?.length &&
                    LCMAtGlanceProduct?.map((product) => (
                      <MenuItem key={product[1].key} value={product[1].key}>
                        {product[1].value}
                      </MenuItem>
                    ))}
                </Select>
              </FormControl>
            </Box>
            <Box sx={{ width: "100%" }}>
              <FormControl fullWidth>
                <InputLabel id="demo-simple-select-standard-label">
                  Support Service
                </InputLabel>
                <Select
                  labelId="demo-multiple-name-label"
                  id="demo-multiple-name"
                  // multiple
                  value={selectedSubNetwork}
                  onChange={(e) => {
                    setSelectedSubNetwork(e.target.value);
                    handleSubNetworkFilter(e.target.value);
                  }}
                  label="SubNetwork"
                  endAdornment={
                    selectedSubNetwork && (
                      <InputAdornment position="end">
                        <IconButton
                          onClick={() => {
                            setSelectedSubNetwork("");
                            if (
                              !selectedProduct &&
                              !selectedOpco &&
                              !selectedVertical
                            ) {
                              getLCMAtGlanceGraphApiResource();
                            }
                          }}
                          edge="end"
                        >
                          <IoClose />
                        </IconButton>
                      </InputAdornment>
                    )
                  }
                  IconComponent={selectedSubNetwork ? () => null : undefined}
                  MenuProps={MenuProps}
                >
                  {LCMAtGlanceSubNetwork?.length &&
                    LCMAtGlanceSubNetwork?.map((subnetwork) => (
                      <MenuItem
                        key={subnetwork[1].key}
                        value={subnetwork[1].key}
                      >
                        {subnetwork[1].value}
                      </MenuItem>
                    ))}
                </Select>
              </FormControl>
            </Box>
            <Box sx={{ width: "100%" }}>
              <InputLabel id="demo-simple-select-standard-label">
                Select Date
              </InputLabel>
              <FormControl fullWidth>
                <DatePicker
                  selected={selectedDate}
                  onChange={(newDate, e) => {
                    e.preventDefault();
                    setSelectedDate(newDate);
                  }}
                  className={`${styles.inputForm}  w-100`}
                  minDate={new Date()}
                  maxDate={new Date(2999, 0, 1)}
                  dateFormat="dd/MM/yyyy"
                  placeholderText="Select Date"
                  popperClassName={styles.reactDatepicker}
                />
              </FormControl>
            </Box>
          </Stack>
        </Box>
        <Box sx={{ width: "100%" }}>
          <InputLabel id="demo-simple-select-standard-label">
            Select Date
          </InputLabel>
          <FormControl fullWidth>
            <DatePicker
              selected={selectedDate}
              onChange={(newDate, e) => {
                e.preventDefault();
                setSelectedDate(newDate);
              }}
              className="inputForm w-100"
              minDate={new Date()}
              maxDate={new Date(2999, 0, 1)}
              dateFormat="dd/MM/yyyy"
              placeholderText="Select Date"
            />
          </FormControl>
        </Box>
        <Box sx={{ p: 2, mb: 5, alignSelf: "flex-end" }}>
          <Stack direction="row" spacing={2}>
            {/* <Button variant="contained" color="success">
              Apply
            </Button> */}
            {selectedOpco ||
            selectedProduct ||
            selectedSubNetwork ||
            selectedVertical ? (
              <Button
                variant="contained"
                color="error"
                onClick={() => handleReset()}
              >
                Reset
              </Button>
            ) : (
              ""
            )}
            {selectedOpco ||
            selectedProduct ||
            selectedSubNetwork ||
            selectedVertical ||
            selectedDate ? (
              <Button
                variant="contained"
                color="success"
                onClick={() => handleFilter()}
              >
                Apply
              </Button>
            ) : (
              ""
            )}
          </Stack>
        </Box>
      </Drawer>
      <div className="pageContainer mb-3">
        <div
          className={`headerPage row mx-0 ${
            searchParams["size"] === 0 && "justify-content-between"
          }`}
        >
          <h3 className="voda-bold fz-28" style={{ textAlign: "left" }}>
            LCM @ Glance
          </h3>
          {LCMAtGlanceGraphData?.data &&
          transformedData?.length &&
          LCMAtGlanceResource &&
          filterHeading ? (
            <div
              className="voda-bold fz-28"
              style={{
                textAlign: "left",
                marginLeft: "1rem",
                color: `${darkMode ? "white" : "black"}`,
              }}
            >
              {" -   " + filterHeading}
            </div>
          ) : (
            ""
          )}
          {/* {selectedOpco || selectedProduct || selectedSubNetwork ? (
            <button
              className="download-to-excel mrl-10 grid-main-btn"
              onClick={handleReset}
            >
              Reset Filter
            </button>
          ) : ( */}
          {searchParams["size"] === 0 && (
            <button
              className="download-to-excel mrl-10 grid-main-btn"
              style={{ width: "max-content" }}
              onClick={() => setShowCanvas(!showCanvas)}
            >
              Apply Filter
            </button>
          )}
          {/* )} */}
        </div>
      </div>
      {LCMAtGlanceGraphData?.data &&
      LCMAtGlanceGraphDataOverAll?.data &&
      LCMAtGlanceResource &&
      transformedData?.length ? (
        <div className="pageContainer ">
          <Row className="mx-0">
            <Col xs={2} className="p-0 mx-0">
              <div
                className="shadow-lg mb-4"
                style={{
                  backgroundColor: `${darkMode ? "#000831" : ""}`,
                }}
              >
                <Paper
                  style={{
                    padding: "7px",
                    backgroundColor: `${darkMode ? "white" : "#e9ecef"}`,
                    color: `${darkMode ? "black" : ""}`,
                    fontWeight: "bold",
                    borderRadius: "unset",
                    marginBottom: "0.3rem ",
                  }}
                >
                  <Typography sx={{ fontWeight: "bold" }}>
                    {"LCM Compliance "}
                  </Typography>
                </Paper>
                {/* <CustomEGaugeChart /> */}
                <div className="p-3">
                  <Gauge
                    red={
                      searchParams.get("opcoId")
                        ? safeNumber(
                            LCMAtGlanceGraphData?.data?.opcoWisePercentage[0]
                              ?.redCompatibilityPercentage
                          )
                        : safeNumber(
                            overAllOpCoWiseGraphData?.redCompatibilityPercentage
                          )
                    }
                    green={
                      searchParams.get("opcoId")
                        ? safeNumber(
                            LCMAtGlanceGraphData?.data?.opcoWisePercentage[0]
                              ?.greenCompatibilityPercentage
                          )
                        : safeNumber(
                            overAllOpCoWiseGraphData?.greenCompatibilityPercentage
                          )
                    }
                    yellow={
                      searchParams.get("opcoId")
                        ? safeNumber(
                            LCMAtGlanceGraphData?.data?.opcoWisePercentage[0]
                              ?.amberCompatibilityPercentage
                          )
                        : safeNumber(
                            overAllOpCoWiseGraphData?.amberCompatibilityPercentage
                          )
                    }
                    selectedOpco={""}
                  />
                </div>
              </div>
              <div
                className="shadow-lg mb-4"
                style={{
                  backgroundColor: `${darkMode ? "#000831" : ""}`,
                }}
              >
                <Paper
                  style={{
                    padding: "7px",
                    backgroundColor: `${darkMode ? "white" : "#e9ecef"}`,
                    color: `${darkMode ? "black" : ""}`,
                    fontWeight: "bold",
                    borderRadius: "unset",
                    marginBottom: "0.3rem ",
                  }}
                >
                  <Typography sx={{ fontWeight: "bold" }}>
                    {"Asset Compliance "}
                  </Typography>
                </Paper>
                <div className="p-3 ">
                  <Gauge
                    red={
                      searchParams.get("opcoId")
                        ? safeNumber(
                            LCMAtGlanceGraphData?.data?.opcoWisePercentage[0]
                              ?.redNodePercentage
                          )
                        : safeNumber(
                            overAllOpCoWiseGraphData?.redNodePercentage
                          )
                    }
                    green={
                      searchParams.get("opcoId")
                        ? safeNumber(
                            LCMAtGlanceGraphData?.data?.opcoWisePercentage[0]
                              ?.greenNodePercentage
                          )
                        : safeNumber(
                            overAllOpCoWiseGraphData?.greenNodePercentage
                          )
                    }
                    yellow={
                      searchParams.get("opcoId")
                        ? safeNumber(
                            LCMAtGlanceGraphData?.data?.opcoWisePercentage[0]
                              ?.amberNodePercentage
                          )
                        : safeNumber(
                            overAllOpCoWiseGraphData?.amberNodePercentage
                          )
                    }
                    selectedOpco={""}
                  />
                </div>
              </div>
              <div
                className="shadow-lg mb-4"
                style={{
                  backgroundColor: `${darkMode ? "#000831" : "white"}`,
                }}
              >
                <div className="p-3 ">
                  <div className="d-flex alignItemCenter pb-2">
                    <div className="legend-style legend-green mr-3"></div>
                    <div
                      style={{
                        color: `${darkMode ? "white" : "black"}`,
                      }}
                    >
                      {` EOM > ${
                        selectedDate && searchParams.get("selectedDate")
                          ? `${new Date(selectedDate).getDate()}/${
                              new Date(selectedDate).getMonth() + 1
                            }/${new Date(selectedDate).getFullYear()}`
                          : "Today"
                      }`}
                    </div>
                  </div>
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
                  <div className="d-flex alignItemCenter pb-2">
                    <div className="legend-style legend-red mr-3"></div>
                    <div
                      style={{
                        color: `${darkMode ? "white" : "black"}`,
                      }}
                    >
                      {` ${
                        selectedDate && searchParams.get("selectedDate")
                          ? `${new Date(selectedDate).getDate()}/${
                              new Date(selectedDate).getMonth() + 1
                            }/${new Date(selectedDate).getFullYear()}`
                          : "Today"
                      } > EOS`}
                    </div>
                  </div>
                </div>
              </div>
            </Col>

            <Col xs={10} className="pr-0">
              <div
                className={`shadow-lg mb-4 ${darkMode && " bg-white "} rounded`}
              >
                {/* <CustomCarousel
                  slides={[OpCoGraph, ProductGraph, SubnetworkGraph]}
                  autoplayDelay={3000}
                /> */}

                <CustomSlides
                  slides={[
                    <OpCoGraph key={"OpCoGraph"} />,
                    <ProductGraph key={"ProductGraph"} />,
                    <SubnetworkGraph key={"SubnetworkGraph"} />,
                  ]}
                  autoplayDelay={3000}
                />
              </div>
            </Col>
          </Row>
        </div>
      ) : (
        <>
          {LCMAtGlanceGraphData?.data?.opcoWisePercentage &&
          Object.keys(LCMAtGlanceGraphData?.data?.opcoWisePercentage).length ===
            0 ? (
            <div
              style={{
                textAlign: "center",
                color: "#FF4D4F",
                fontSize: "16px",
                fontWeight: "bold",
                padding: "20px",
                backgroundColor: "#FBE3E3",
                borderRadius: "5px",
                marginTop: "20px",
              }}
            >
              No Data Found
            </div>
          ) : (
            ""
          )}
        </>
      )}
      <div className="mt-3 mb-3"></div>
    </ThemeProvider>
  );
};

export default LCMAtGlanceChart;
