import React, { useState, useEffect } from "react";
import Gauge from "./Gauge";
import { Col, Row } from "react-bootstrap";
import {
  GetLCMAtGlanceGraphApiResource,
  GetLCMAtGlanceGraph,
  GetLCMAtGlanceGraphOverAll,
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
} from "@mui/material";
import IconButton from "@mui/material/IconButton";
import { IoClose } from "react-icons/io5";
import { createTheme, ThemeProvider } from "@mui/material/styles";
import { useTheme } from "../../Context/ThemeContext";
import { BarChart } from "@mui/x-charts/BarChart";
import { HighlightItemData, HighlightScope } from "@mui/x-charts/context";
import { PieChart } from "@mui/x-charts/PieChart";
import "./chartStyle.css";
import ProductCompliance from "./ProductCompliance";
import { BiSolidFilterAlt } from "react-icons/bi";
import { useSearchParams, useNavigate } from "react-router-dom";
import { useAuth } from "../../Hook/useAuth";
import { safeNumber } from "../../Hook/Common";

const Chart5: React.FC = () => {
  const [searchParams] = useSearchParams();
  const [filterHeading, setFilterHeading] = useState("");
  const { isPermesso } = useAuth();
  const navigate = useNavigate();

  const [LCMAtGlanceGraphDataOverAll, setLCMAtGlanceGraphDataOverAll] =
    useState<any>();
  const [LCMAtGlanceGraphData, setLCMAtGlanceGraphData] = useState<any>();
  const [LCMAtGlanceResource, setLCMAtGlanceResource] = useState<any>();

  const [opCoWiseGraphData, setOpCoWiseGraphData] = useState<any>();
  const [overAllOpCoWiseGraphData, setOverAllOpCoWiseGraphData] =
    useState<any>();
  const [productWiseGraphData, setProductWiseGraphData] = useState<any>();
  const [subNetworkWiseGraphData, setSubNetworkWiseGraphData] = useState<any>();

  const [formattedGraphData, setFormattedGraphData] = useState<any>();
  const [productWiseFormattedGraphData, setProductWiseFormattedGraphData] =
    useState<any>();
  const [
    subNetworkWiseFormattedGraphData,
    setSubNetworkWiseFormattedGraphData,
  ] = useState<any>();

  const [LCMAtGlanceOpCo, setLCMAtGlanceOpCo] = useState<any>();
  const [LCMAtGlanceProduct, setLCMAtGlanceProduct] = useState<any>();
  const [LCMAtGlanceSubNetwork, setLCMAtGlanceSubNetwork] = useState<any>();
  const [LCMAtGlanceVertical, setLCMAtGlanceVertical] = useState<any>();

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

  const getLCMAtGlanceGraphOverAll = async () => {
    const graphDataOverAll = await GetLCMAtGlanceGraphOverAll();
    setLCMAtGlanceGraphDataOverAll(graphDataOverAll?.ResultDtoCreate);

    setOverAllOpCoWiseGraphData(graphDataOverAll?.ResultDtoCreate?.data);
  };

  useEffect(() => {
    if (isPermesso && (!searchParams || searchParams.toString() === "")) {
      if (!LCMAtGlanceGraphData?.data) {
        getLCMAtGlanceGraph({});
      }
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
    if (isPermesso && searchParams) {
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
      getLCMAtGlanceGraph(transformedObj);
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

      acc[key] = [safeNumber(value)];
      return acc;
    }, {});
    let combinedString = "";
    const transformedObjKeys = Object.keys(transformedObj);
    const lastIndex = transformedObjKeys.length - 1;

    let hasAddedValue = false;

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

  const generateReadableColor = (): string => {
    const hue = Math.floor(Math.random() * 12) * 30;
    const saturation = Math.floor(Math.random() * 50) + 50;

    let lightness = Math.floor(Math.random() * 40) + 30;

    const luminance =
      0.2126 * (lightness / 100) +
      0.7152 * (saturation / 100) +
      0.0722 * (hue / 360);

    if (luminance < 0.5) {
      lightness = Math.min(lightness + 20, 75);
    } else {
      lightness = Math.max(lightness - 20, 30);
    }

    const color = `hsl(${hue}, ${saturation}%, ${lightness}%)`;
    return color;
  };

  useEffect(() => {
    if (subNetworkWiseGraphData?.length) {
      const formatSubNetwork = subNetworkWiseGraphData?.map((subnetwork) => ({
        id: subnetwork?.supportedServicesId,
        value: subnetwork?.greenCompatibilityPercentage,
        label: subnetwork?.supportedServicesDescription,
        color: generateReadableColor(),
      }));
      setSubNetworkWiseFormattedGraphData(formatSubNetwork);
    }
  }, [subNetworkWiseGraphData]);
  useEffect(() => {
    if (productWiseGraphData?.length) {
      const formatProduct = productWiseGraphData?.map((product) => ({
        id: product?.productId,
        value: product?.greenCompatibilityPercentage,
        label: product?.productName,
        product: safeNumber(product?.greenCompatibilityPercentage),
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
            safeNumber(data?.redCompatibilityPercentage),
            safeNumber(data?.amberCompatibilityPercentage),
            safeNumber(data?.greenCompatibilityPercentage),
            safeNumber(data?.redNodeCount),
            safeNumber(data?.amberNodeCount),
            safeNumber(data?.greenNodeCount),
          ],
        ])
      );
    }
  }, [opCoWiseGraphData]);

  const transformedData = formattedGraphData?.map(([name, values]) => ({
    category: name,
    planned: values[0],
    planned_node_count: values[3],
    actual: values[1],
    actual_node_count: values[4],
    target: values[2],
    target_node_count: values[5],
  }));

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

  useEffect(() => {
    selectDarkMode(true);

    return () => {
      selectDarkMode(false);
    };
  }, []);

  const theme = darkMode ? darkTheme : lightTheme;
  const [showCanvas, setShowCanvas] = useState(false);

  const [selectedOpco, setSelectedOpco] = useState("");
  const [selectedProduct, setSelectedProduct] = useState("");
  const [selectedSubNetwork, setSelectedSubNetwork] = useState("");
  const [selectedVertical, setSelectedVertical] = useState("");

  const handleReset = () => {
    // navigate(window.location.pathname, { replace: true });
    // setLCMAtGlanceGraphData([]);
    // setFormattedGraphData([]);
    setSelectedOpco("");
    setSelectedProduct("");
    setSelectedSubNetwork("");
    setSelectedVertical("");
    // getLCMAtGlanceGraph({});
    // getLCMAtGlanceGraphApiResource();
    setShowCanvas(false);
  };

  useEffect(() => {
    if (selectedOpco) {
      const filterProduct =
        LCMAtGlanceGraphData?.data?.productWisePercentage?.reduce(
          (acc, curr) => {
            if (
              curr.greenCompatibilityPercentage &&
              curr.greenCompatibilityPercentage !== "0"
            ) {
              acc[curr.productId] = curr.productName;
            }
            return acc;
          },
          {}
        );
      if (
        filterProduct &&
        Object.keys(filterProduct).length > 0 &&
        !selectedProduct
      ) {
        setLCMAtGlanceProduct(Object.entries(filterProduct));
      }
      const filterSubNetwork =
        LCMAtGlanceGraphData?.data?.subnetworkWisePercentage?.reduce(
          (acc, curr) => {
            if (
              curr.greenCompatibilityPercentage &&
              curr.greenCompatibilityPercentage !== "0"
            ) {
              acc[curr.supportedServicesId] = curr.supportedServicesDescription;
            }
            return acc;
          },
          {}
        );
      if (
        filterSubNetwork &&
        Object.keys(filterSubNetwork).length > 0 &&
        !selectedSubNetwork
      ) {
        setLCMAtGlanceSubNetwork(Object.entries(filterSubNetwork));
      }
    }
  }, [LCMAtGlanceGraphData?.data?.productWisePercentage]);

  const handleFilter = () => {
    setSelectedOpco("");
    setSelectedProduct("");
    setSelectedSubNetwork("");
    setSelectedVertical("");
    // setLCMAtGlanceGraphData([]);
    // setFormattedGraphData([]);
    const filterObj = {};
    if (selectedProduct) {
      filterObj["productId"] = [safeNumber(selectedProduct)];
    }
    if (selectedSubNetwork) {
      filterObj["supportedServicesId"] = [safeNumber(selectedSubNetwork)];
    }
    if (selectedVertical) {
      filterObj["verticalResponsibleId"] = [safeNumber(selectedVertical)];
    }
    if (selectedOpco) {
      filterObj["opcoId"] = [safeNumber(selectedOpco)];
    }
    const queryString = new URLSearchParams(filterObj).toString();
    const url = `/lcmatglance/v1?${queryString}`;

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
  return (
    <ThemeProvider theme={theme}>
      <div className="container mb-3">
        <div className="headerPage row mx-0 justify-content-between ">
          <h3 className="voda-bold fz-28" style={{ textAlign: "left" }}>
            LCM @ Glance
          </h3>
          {LCMAtGlanceGraphData?.data &&
          transformedData?.length &&
          LCMAtGlanceResource &&
          filterHeading ? (
            <h3 className="voda-bold fz-28" style={{ textAlign: "left" }}>
              {filterHeading}
            </h3>
          ) : (
            ""
          )}
          {!searchParams.get("opcoId") &&
          !searchParams.get("verticalResponsibleId") &&
          !searchParams.get("supportedServicesId") &&
          !searchParams.get("productId") ? (
            <button
              className="download-to-excel  mrl-5 grid-main-btn"
              onClick={() => setShowCanvas(!showCanvas)}
            >
              Apply Filter
            </button>
          ) : (
            <div></div>
          )}
        </div>

        {LCMAtGlanceGraphData?.data &&
        LCMAtGlanceGraphDataOverAll?.data &&
        LCMAtGlanceResource &&
        transformedData?.length ? (
          <>
            {overAllOpCoWiseGraphData ? (
              <Row className="row-cols-3 mt-5 justify-content-evenly">
                <Col>
                  <Paper>
                    <Typography>{"LCM Compliance "}</Typography>
                  </Paper>
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
                </Col>
                <Col>
                  <Paper>
                    <Typography>{"Asset Compliance "}</Typography>
                  </Paper>
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
                </Col>
              </Row>
            ) : (
              ""
            )}
            <Box sx={{ width: "65%" }}>
              <div className="glance-legend-container">
                <section className="glance-legend-container">
                  <div className="legend-style legend-green"></div>
                  <div
                    style={{
                      color: `${darkMode ? "white" : "black"}`,
                    }}
                  >
                    {" EOM > Today"}
                  </div>
                </section>
                <section className="glance-legend-container">
                  <div className="legend-style legend-amber"></div>
                  <div
                    style={{
                      color: `${darkMode ? "white" : "black"}`,
                    }}
                  >
                    {" EOS > Today > EOM"}
                  </div>
                </section>
                <section className="glance-legend-container">
                  <div className="legend-style legend-red"></div>
                  <div
                    style={{
                      color: `${darkMode ? "white" : "black"}`,
                    }}
                  >
                    {" Today > EOS"}
                  </div>
                </section>
              </div>
            </Box>

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
                    <FormControl variant="standard" fullWidth>
                      <InputLabel id="demo-simple-select-standard-label">
                        Opco
                      </InputLabel>
                      <Select
                        labelId="demo-simple-select-standard-label"
                        id="demo-simple-select-standard"
                        value={selectedOpco}
                        onChange={(e) => setSelectedOpco(e.target.value)}
                        label="Opco"
                        endAdornment={
                          selectedOpco && (
                            <InputAdornment position="end">
                              <IconButton
                                onClick={() => setSelectedOpco("")}
                                edge="end"
                              >
                                <IoClose />
                              </IconButton>
                            </InputAdornment>
                          )
                        }
                        IconComponent={selectedOpco ? () => null : undefined}
                        MenuProps={{ disableScrollLock: true }}
                      >
                        {LCMAtGlanceOpCo?.length &&
                          LCMAtGlanceOpCo?.map((opco) => (
                            <MenuItem key={opco[1].key} value={opco[1].key}>
                              {opco[1].value}
                            </MenuItem>
                          ))}
                      </Select>
                    </FormControl>
                  </Box>
                  <Box sx={{ width: "100%" }}>
                    <FormControl variant="standard" fullWidth>
                      <InputLabel id="demo-simple-select-standard-label">
                        Vertical
                      </InputLabel>
                      <Select
                        labelId="demo-simple-select-standard-label"
                        id="demo-simple-select-standard"
                        value={selectedVertical}
                        onChange={(e) => setSelectedVertical(e.target.value)}
                        label="Vertical"
                        endAdornment={
                          selectedVertical && (
                            <InputAdornment position="end">
                              <IconButton
                                onClick={() => setSelectedVertical("")}
                                edge="end"
                              >
                                <IoClose />
                              </IconButton>
                            </InputAdornment>
                          )
                        }
                        IconComponent={
                          selectedVertical ? () => null : undefined
                        }
                        MenuProps={{ disableScrollLock: true }}
                      >
                        {LCMAtGlanceVertical?.length &&
                          LCMAtGlanceVertical?.map((vertical) => (
                            <MenuItem
                              key={vertical[1].key}
                              value={vertical[1].key}
                            >
                              {vertical[1].value}
                            </MenuItem>
                          ))}
                      </Select>
                    </FormControl>
                  </Box>
                  <Box sx={{ width: "100%" }}>
                    <FormControl variant="standard" fullWidth>
                      <InputLabel id="demo-simple-select-standard-label">
                        Product
                      </InputLabel>
                      <Select
                        labelId="demo-simple-select-standard-label"
                        id="demo-simple-select-standard"
                        value={selectedProduct}
                        onChange={(e) => setSelectedProduct(e.target.value)}
                        label="Product"
                        endAdornment={
                          selectedProduct && (
                            <InputAdornment position="end">
                              <IconButton
                                onClick={() => setSelectedProduct("")}
                                edge="end"
                              >
                                <IoClose />
                              </IconButton>
                            </InputAdornment>
                          )
                        }
                        IconComponent={selectedProduct ? () => null : undefined}
                        MenuProps={{ disableScrollLock: true }}
                      >
                        {LCMAtGlanceProduct?.length &&
                          LCMAtGlanceProduct?.map((product) => (
                            <MenuItem
                              key={product[1].key}
                              value={product[1].key}
                            >
                              {product[1].value}
                            </MenuItem>
                          ))}
                      </Select>
                    </FormControl>
                  </Box>
                  <Box sx={{ width: "100%" }}>
                    <FormControl variant="standard" fullWidth>
                      <InputLabel id="demo-simple-select-standard-label">
                        Support Service
                      </InputLabel>
                      <Select
                        labelId="demo-simple-select-standard-label"
                        id="demo-simple-select-standard"
                        value={selectedSubNetwork}
                        onChange={(e) => setSelectedSubNetwork(e.target.value)}
                        label="SubNetwork"
                        endAdornment={
                          selectedSubNetwork && (
                            <InputAdornment position="end">
                              <IconButton
                                onClick={() => setSelectedSubNetwork("")}
                                edge="end"
                              >
                                <IoClose />
                              </IconButton>
                            </InputAdornment>
                          )
                        }
                        IconComponent={
                          selectedSubNetwork ? () => null : undefined
                        }
                        MenuProps={{ disableScrollLock: true }}
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
                </Stack>
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
                      variant="outlined"
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
                  selectedVertical ? (
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

            {/* <Row
              className={
                LCMAtGlanceGraphData?.items?.length && selectedOpco
                  ? "row-cols-1 mt-5"
                  : "row-cols-5 mt-5"
              }
            >
              {LCMAtGlanceGraphData?.items?.length &&
                LCMAtGlanceGraphData?.items?.map((glance) => (
                  <Col key={glance?.opcoId} className="mb-3">
                    <Gauge
                      red={safeNumber(glance?.redCompatibilityPercentage)}
                      green={safeNumber(glance?.greenCompatibilityPercentage)}
                      yellow={safeNumber(glance?.oragneCompatibilityPercentage)}
                      selectedOpco={selectedOpco}
                    />
                    <Paper>
                      <Typography>{glance?.opCoDescrption}</Typography>
                    </Paper>
                  </Col>
                ))}
            </Row> */}

            <div
              className="container mt-4 mb-3"
              style={{
                position: "relative",
                background: `${darkMode ? "black" : "white"}`,
                border: `${darkMode ? "2px solid white" : "2px solid black"}`,
              }}
            >
              {transformedData?.length ? (
                <>
                  <Paper>
                    <Typography className="px-2 py-2">
                      {"OpCo Compliance "}
                    </Typography>
                  </Paper>
                  <BarChart
                    dataset={transformedData}
                    xAxis={[
                      {
                        scaleType: "band",
                        dataKey: "category",
                      },
                    ]}
                    series={[
                      {
                        dataKey: "planned",
                        color: "#e60000a1",
                        highlightScope: {
                          highlighted: "item",
                          faded: "global",
                        },
                        valueFormatter: (element, data) => {
                          const Index = data?.dataIndex;
                          return `${element} % (Nodes: ${transformedData[Index]?.planned_node_count})`;
                        },
                      },
                      {
                        dataKey: "actual",
                        color: "#FFFF00a1",
                        highlightScope: {
                          highlighted: "item",
                          faded: "global",
                        },
                        valueFormatter: (element, data) => {
                          const Index = data?.dataIndex;
                          return `${element} % (Nodes: ${transformedData[Index]?.actual_node_count})`;
                        },
                      },
                      {
                        dataKey: "target",
                        color: "#00c300a1",
                        highlightScope: {
                          highlighted: "item",
                          faded: "global",
                        },
                        valueFormatter: (element, data) => {
                          const Index = data?.dataIndex;
                          return `${element} % (Nodes: ${transformedData[Index]?.target_node_count})`;
                        },
                      },
                    ]}
                    barLabel={
                      (selectedOpco && transformedData.length < 5) ||
                      (selectedProduct && transformedData.length < 5) ||
                      (selectedSubNetwork && transformedData.length < 5)
                        ? (item, context) =>
                            item.value ? item.value + "%" : undefined
                        : undefined
                    }
                    height={300}
                    width={
                      (selectedOpco && transformedData.length < 5) ||
                      (selectedProduct && transformedData.length < 5) ||
                      (selectedSubNetwork && transformedData.length < 5)
                        ? 600
                        : 1100
                    }
                  />
                </>
              ) : (
                ""
              )}
              <hr
                style={{
                  margin: 0,
                  padding: 0,
                  height: "1px",
                  background: `${darkMode ? "white" : "black"}`,
                }}
              />
              {/* <Stack
                direction="column"
                width="100%"
                textAlign="left"
                spacing={2}
              > */}
              <Box sx={{ width: "100%" }}>
                {productWiseFormattedGraphData?.length ? (
                  <>
                    <Paper>
                      <Typography className="px-2 py-2">
                        Product Compliance
                      </Typography>
                    </Paper>
                    {/* <PieChart
                      series={[
                        {
                          arcLabel: ({ label }) =>
                            label && label?.length > 6
                              ? `${label?.slice(0, 6)}...`
                              : `${label}`,

                          arcLabelMinAngle: 15,
                          arcLabelRadius: "50%",
                          valueFormatter: (item) => `${item["data"]} %`,
                          data: productWiseFormattedGraphData,
                          highlightScope: {
                            faded: "global",
                            highlighted: "item",
                          },
                        },
                      ]}
                      height={500}
                      slotProps={{
                        legend: {
                          hidden: true,
                        },
                      }}
                    /> */}
                    <ProductCompliance
                      data={LCMAtGlanceGraphData?.data?.productWisePercentage}
                    />
                  </>
                ) : (
                  ""
                )}
              </Box>
              {/* <div
                  style={{
                    width: "2px",
                    background: `${darkMode ? "white" : "black"}`,
                  }}
                ></div> */}
              <hr
                style={{
                  margin: 0,
                  padding: 0,
                  height: "1px",
                  background: `${darkMode ? "white" : "black"}`,
                }}
              />
              <Box sx={{ width: "100%" }}>
                {subNetworkWiseFormattedGraphData?.length ? (
                  <>
                    <Paper>
                      <Typography className="px-2 py-2">
                        SubNetwork Compliance
                      </Typography>
                    </Paper>
                    <PieChart
                      series={[
                        {
                          arcLabel: ({ label, value }) => `${value} %`,
                          arcLabelMinAngle: 15,
                          arcLabelRadius: "65%",
                          valueFormatter: (item) => `${item["data"]} %`,
                          data: subNetworkWiseFormattedGraphData,
                          cx: 320,
                          highlightScope: {
                            faded: "global",
                            highlighted: "item",
                          },
                        },
                      ]}
                      height={300}
                      slotProps={{
                        legend: {
                          direction: "column",
                          itemMarkWidth: 24,
                          itemMarkHeight: 20,
                          markGap: 5,
                          itemGap: 5,
                          position: {
                            horizontal: "right",
                            vertical: "middle",
                          },
                          labelStyle: {
                            fontSize: 12,
                            fontWeight: "bolder",
                            // fill: "white",
                          },
                        },
                      }}
                    />
                  </>
                ) : (
                  ""
                )}
              </Box>
              {/* </Stack> */}
            </div>
          </>
        ) : (
          <>
            {LCMAtGlanceGraphData?.data?.opcoWisePercentage &&
            Object.keys(LCMAtGlanceGraphData?.data?.opcoWisePercentage)
              .length === 0 ? (
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
      </div>
      <div className="mt-3 mb-4"></div>
    </ThemeProvider>
  );
};

export default Chart5;
