import React, { useState, useEffect } from "react";
import { BarChart, BarChartProps } from "@mui/x-charts/BarChart";
import { PieChart } from "@mui/x-charts/PieChart";
import { PlannedActivityQueryObjectGrid } from "../../Model/PlannedActivity";
import {
  GetPlannedActivityGridForChart,
  GetFilterColumPlannedActivityForChart,
} from "../../Redux/Action/PlannedActivity/PlannedActivityGridAction";
import { RootState, rootStore } from "../../Redux/Store/rootStore";
import { useSelector } from "react-redux";
import { createTheme, ThemeProvider } from "@mui/material/styles";
import { useTheme } from "../../Context/ThemeContext";
import { useNavigate } from "react-router";
import InputLabel from "@mui/material/InputLabel";
import MenuItem from "@mui/material/MenuItem";
import FormHelperText from "@mui/material/FormHelperText";
import FormControl from "@mui/material/FormControl";
import Select, { SelectChangeEvent } from "@mui/material/Select";
import Box from "@mui/material/Box";
import Stack from "@mui/material/Stack";
import Typography from "@mui/material/Typography";
import { HighlightItemData, HighlightScope } from "@mui/x-charts/context";
import { Button, Paper } from "@mui/material";
import { GetNetworkElementAsPlannedGrid } from "../../Redux/Action/NetworkElementAsPlanned/NetworkElementAsPlannedGridAction";
import setLoader from "../../Redux/Action/LoaderAction";
import { NetworkElementAsPlannedQueryObjectGrid } from "../../Model/NetworkElementAsPlanned";
import { GET_GRID_NETWORK_ELEMENT_AS_PLANNED } from "../../Model/NetworkElementAsPlanned";
import {
  GetNetworkElementGraphAllOpco,
  GetNetworkElementGraph,
} from "../../Redux/Action/LookUp/NetworkElementGraph/NetworkElementGraphAction";
import { safeNumber } from "../../Hook/Common";
import { useAuth } from "../../Hook/useAuth";

const paginationQuery: NetworkElementAsPlannedQueryObjectGrid = {
  nodeIndex: [],
  opCo: [],
  designComponent: [],
  elementName: [],
  capacityPlanReference: [],
  additionalInformation1: [],
  additionalInformation2: [],
  automatedFeedback: undefined,
  designComponentIndex: [],
  plannedAction: undefined,
  networkConstruct: [],
  environment: [],
  deploymentStatus: [],
  deploymentType: [],
  location: [],
  nfviBundleID: [],
  subDomainSpoc: [],
  eduspoc: [],
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 100,
  lastModified: undefined,
  principalId: undefined,
  deleted: undefined,
  orphan: undefined,
  lastModifiedBy: [],
  hwResourceKey: [],
  previousHWResourceKey: [],
  swResourceKey: [],
  previousSWResourceKey: [],
};

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
  subDomainResponseCeFunctionId: [],
  subDomainResponseCeFunction: [],
  oemVendorId: [],
  oemVendor: [],
  productNameNeInstances: [],
  networkElementsAsPlannedId: [],
  networkElementsPlannedName: [],
};
const Chart4 = () => {
  const { darkMode } = useTheme();
  const { readonly, isPermesso, pageSize } = useAuth();

  useEffect(() => {
    // Update paginationQuery with the pageSize from useAuth whenever it changes
    paginationQuery.pageSize = pageSize;
  }, [pageSize]);
  useEffect(() => {
    rootStore.dispatch({
      type: GET_GRID_NETWORK_ELEMENT_AS_PLANNED,
      payload: {},
    });

    const assetApiCall = GetNetworkElementAsPlannedGrid(paginationQuery);
    const networkApiCall = getNetworkElementGraph();
    setLoader("ADD", "");
    Promise.all([assetApiCall, networkApiCall]).then((data) => {
      setLoader("REMOVE", "");
    });
  }, []);
  const Grid = (state: RootState) =>
    state.networkElementAsPlannedGridReducer.NetworkElementAsPlannedGridResult;
  let GridDto = useSelector(Grid);

  const [assetFormatedData, setAssetFormatedData] = useState<any>();
  const [assetOpcoFormatedData, setAssetOpcoFormatedData] = useState<any>();
  const [assetVfNameFormatedData, setAssetVfNameFormatedData] = useState<any>();

  useEffect(() => {
    setAssetOpcoFormatedData([]);
    setAssetVfNameFormatedData([]);
    setAssetFormatedData(GridDto?.items);
    const data = GridDto?.items;
    if (data?.length) {
      // opco
      const opcoMap: Map<string, any[]> = data.reduce((acc, item) => {
        if (!acc.has(item.opCo)) {
          acc.set(item.opCo, []);
        }
        acc.get(item.opCo).push(item);
        return acc;
      }, new Map());

      setAssetOpcoFormatedData(
        Array.from(opcoMap.entries()).map((data) => {
          return [String(data[0]), data[1].length];
        })
      );
      // vf name

      const vfNameMap: Map<string, any[]> = data.reduce((acc, item) => {
        if (!acc.has(item["vodafoneName"])) {
          acc.set(item["vodafoneName"], []);
        }
        acc.get(item["vodafoneName"]).push(item);
        return acc;
      }, new Map());

      setAssetVfNameFormatedData(
        Array.from(vfNameMap.entries()).map((data) => {
          return [String(data[0]), data[1].length];
        })
      );
    }
  }, [GridDto]);

  const [networkElementGraphData, setNetworkElementGraphData] = useState<any>();
  const [
    formattednetworkElementGraphData,
    setFormattedNetworkElementGraphData,
  ] = useState<any>();
  const getNetworkElementGraph = async () => {
    const graphData = await GetNetworkElementGraph({});
    setNetworkElementGraphData(graphData?.ResultDtoCreate);
    return graphData;
  };

  function groupAndAggregateData(data) {
    const result = {};

    // Iterate over each item in the data array
    data.forEach((item) => {
      const { oemVendor, opcoId, opCoDescrption, networkElementCount } = item;

      // Initialize the vendor group if it doesn't exist
      if (!result[oemVendor]) {
        result[oemVendor] = {
          data: [],
          total: 0,
        };
      }

      // Add the data and update the total count
      result[oemVendor].data.push({
        opcoId,
        opCoDescrption,
        networkElementCount,
      });

      result[oemVendor].total += networkElementCount;
    });

    // Convert the result object into the desired array format
    setFormattedNetworkElementGraphData(
      Object.keys(result).map((vendor) => [vendor, result[vendor].total])
    );
    // return Object.keys(result).map((vendor) => [vendor, result[vendor].total]);
  }

  useEffect(() => {
    if (networkElementGraphData?.items?.length) {
      groupAndAggregateData(networkElementGraphData?.items);
    }
  }, [networkElementGraphData]);

  const [selectedOpco, setSelectedOpco] = useState("");
  const [selectedVf, setSelectedVf] = useState("");
  const [selectedVendor, setSelectedVendor] = useState("");

  const handleChangeOpCo = (event, barItemIdentifier) => {
    console.log("barItemIdentifier", barItemIdentifier);
    setSelectedOpco(barItemIdentifier.axisValue);
    handleHighLightedOpCoItem(barItemIdentifier.dataIndex);

    const data = assetFormatedData;
    // update vf
    if (data?.length && !selectedVf) {
      const vfNameMap: Map<string, any[]> = data
        .filter((data) => data["opCo"] === barItemIdentifier.axisValue)
        .reduce((acc, item) => {
          if (!acc.has(item["vodafoneName"])) {
            acc.set(item["vodafoneName"], []);
          }
          acc.get(item["vodafoneName"]).push(item);
          return acc;
        }, new Map());

      setAssetVfNameFormatedData(
        Array.from(vfNameMap.entries()).map((data) => {
          return [String(data[0]), data[1].length];
        })
      );
    }

    // update vendor

    const networkData = networkElementGraphData?.items;
    if (networkData.length) {
      groupAndAggregateData(
        networkData?.filter(
          (data) => data.opCoDescrption === barItemIdentifier.axisValue
        )
      );
    }
  };

  const [highlightedOpCoItem, setHighLightedOpCoItem] =
    useState<HighlightItemData | null>({});
  const handleHighLightedOpCoItem = (val) => {
    setHighLightedOpCoItem((prev) => ({
      ...prev,
      dataIndex: safeNumber(val),
      seriesId: "auto-generated-id-0",
    }));
  };

  const [highlightedVfItem, setHighLightedVfItem] =
    useState<HighlightItemData | null>({});
  const handleHighLightedVfItem = (val) => {
    setHighLightedVfItem((prev) => ({
      ...prev,
      dataIndex: safeNumber(val),
      seriesId: "auto-generated-id-0",
    }));
  };
  const handleChangeVf = (event, barItemIdentifier) => {
    setSelectedVf(barItemIdentifier.axisValue);
    handleHighLightedVfItem(barItemIdentifier.dataIndex);

    const data = assetFormatedData;
    // update opco
    if (data?.length && !selectedOpco) {
      const opcoMap: Map<string, any[]> = data
        .filter((data) => data["vodafoneName"] === barItemIdentifier.axisValue)
        .reduce((acc, item) => {
          if (!acc.has(item.opCo)) {
            acc.set(item.opCo, []);
          }
          acc.get(item.opCo).push(item);
          return acc;
        }, new Map());

      setAssetOpcoFormatedData(
        Array.from(opcoMap.entries()).map((data) => {
          return [String(data[0]), data[1].length];
        })
      );
    }
  };

  const [highlightedVendorItem, setHighLightedVendorItem] =
    useState<HighlightItemData | null>({});
  const handleHighLightedVendorItem = (val) => {
    setHighLightedVendorItem((prev) => ({
      ...prev,
      dataIndex: safeNumber(val),
      seriesId: "auto-generated-id-0",
    }));
  };
  const handleChangeVendor = (event, barItemIdentifier) => {
    setSelectedVendor(barItemIdentifier.axisValue);
    handleHighLightedVendorItem(barItemIdentifier.dataIndex);
  };

  const handleReset = () => {
    if (selectedOpco) {
      setSelectedOpco("");
      setHighLightedOpCoItem({});
    }

    if (selectedVf) {
      setSelectedVf("");
      setHighLightedVfItem({});
    }
    if (selectedVendor) {
      setSelectedVendor("");
      setHighLightedVendorItem({});
    }
    setAssetOpcoFormatedData([]);
    setAssetVfNameFormatedData([]);
    setFormattedNetworkElementGraphData([]);

    const data = assetFormatedData;
    if (data?.length) {
      // opco
      const opcoMap: Map<string, any[]> = data.reduce((acc, item) => {
        if (!acc.has(item.opCo)) {
          acc.set(item.opCo, []);
        }
        acc.get(item.opCo).push(item);
        return acc;
      }, new Map());

      setAssetOpcoFormatedData(
        Array.from(opcoMap.entries()).map((data) => {
          return [String(data[0]), data[1].length];
        })
      );

      // vf name
      const vfNameMap: Map<string, any[]> = data.reduce((acc, item) => {
        if (!acc.has(item["vodafoneName"])) {
          acc.set(item["vodafoneName"], []);
        }
        acc.get(item["vodafoneName"]).push(item);
        return acc;
      }, new Map());

      setAssetVfNameFormatedData(
        Array.from(vfNameMap.entries()).map((data) => {
          return [String(data[0]), data[1].length];
        })
      );
    }

    if (networkElementGraphData?.items?.length) {
      groupAndAggregateData(networkElementGraphData?.items);
    }
  };

  console.log("assetOpcoFormatedData", assetOpcoFormatedData);
  return (
    <div className="container">
      {" "}
      <div className="headerPage row mx-0 justify-content-between">
        <h3 className="voda-bold fz-28" style={{ textAlign: "left" }}>
          NFVI Report
        </h3>
        {selectedOpco || selectedVf || selectedVendor ? (
          <button
            className="download-to-excel mrl-10 grid-main-btn"
            onClick={handleReset}
          >
            Reset Filter
          </button>
        ) : (
          ""
        )}
      </div>
      {assetFormatedData && networkElementGraphData?.items ? (
        <div
          className="container mt-3 mb-3"
          style={{
            position: "relative",
            background: `${darkMode ? "black" : "white"}`,
            border: `${darkMode ? "2px solid white" : "2px solid black"}`,
          }}
        >
          <Stack
            direction="row"
            sx={{ width: "100%", textAlign: "left" }}
            spacing={2}
          >
            <Box sx={{ width: "50%" }}>
              {networkElementGraphData?.items &&
              networkElementGraphData?.items?.length &&
              formattednetworkElementGraphData &&
              formattednetworkElementGraphData?.length ? (
                <>
                  <Paper>
                    <Typography className="px-2 py-2">VNF / Vendor</Typography>
                  </Paper>
                  <BarChart
                    dataset={formattednetworkElementGraphData}
                    xAxis={[
                      {
                        scaleType: "band",
                        dataKey: `${[0]}`,
                      },
                    ]}
                    series={[
                      {
                        dataKey: `${[1]}`,
                        label: "Asset",
                        highlightScope: {
                          highlighted: "item",
                          faded: "global",
                        } as HighlightScope,
                        color: "#02B2AF",
                      },
                    ]}
                    // layout="horizontal"
                    height={400}
                    onAxisClick={handleChangeVendor}
                    barLabel={(item) => item.value?.toString()}
                    highlightedItem={highlightedVendorItem}
                    onHighlightChange={() =>
                      setHighLightedVendorItem((prev) => ({
                        dataIndex: prev?.dataIndex,
                        seriesId:
                          prev?.dataIndex === 0 || prev?.dataIndex
                            ? "auto-generated-id-0"
                            : "",
                      }))
                    }
                  />
                </>
              ) : (
                ""
              )}
            </Box>
            <div
              style={{
                width: "2px",
                background: `${darkMode ? "white" : "black"}`,
              }}
            ></div>
            <Box sx={{ width: "50%" }}>
              {assetFormatedData &&
              assetOpcoFormatedData &&
              assetOpcoFormatedData?.length ? (
                <>
                  <Paper>
                    <Typography className="px-2 py-2">VNF / OPCO</Typography>
                  </Paper>
                  <BarChart
                    dataset={assetOpcoFormatedData}
                    xAxis={[
                      {
                        scaleType: "band",
                        dataKey: `${[0]}`,
                      },
                    ]}
                    series={[
                      {
                        dataKey: `${[1]}`,
                        label: "Asset",
                        highlightScope: {
                          highlighted: "item",
                          faded: "global",
                        } as HighlightScope,
                      },
                    ]}
                    // layout="horizontal"
                    height={400}
                    onAxisClick={handleChangeOpCo}
                    barLabel={(item) => item.value?.toString()}
                    highlightedItem={highlightedOpCoItem}
                    onHighlightChange={() =>
                      setHighLightedOpCoItem((prev) => ({
                        dataIndex: prev?.dataIndex,
                        seriesId:
                          prev?.dataIndex === 0 || prev?.dataIndex
                            ? "auto-generated-id-0"
                            : "",
                      }))
                    }
                  />
                </>
              ) : (
                ""
              )}
            </Box>
          </Stack>

          <hr
            style={{
              margin: 0,
              padding: 0,
              height: "1px",
              background: `${darkMode ? "white" : "black"}`,
            }}
          />

          <Stack
            direction="row"
            sx={{ width: "100%", textAlign: "left" }}
            spacing={2}
          >
            <Box sx={{ width: "50%" }}>
              {assetVfNameFormatedData && assetVfNameFormatedData?.length ? (
                <>
                  <Paper>
                    <Typography className="px-2 py-2">
                      VNF / Vodafone Name
                    </Typography>
                  </Paper>
                  <BarChart
                    dataset={assetVfNameFormatedData}
                    yAxis={[
                      {
                        scaleType: "band",
                        dataKey: `${[0]}`,
                      },
                    ]}
                    series={[
                      {
                        dataKey: `${[1]}`,
                        label: "Asset",
                        highlightScope: {
                          highlighted: "item",
                          faded: "global",
                        } as HighlightScope,
                      },
                    ]}
                    layout="horizontal"
                    height={400}
                    onAxisClick={handleChangeVf}
                    barLabel={(item) => item.value?.toString()}
                    highlightedItem={highlightedVfItem}
                    margin={{ left: 180 }}
                    onHighlightChange={() =>
                      setHighLightedVfItem((prev) => ({
                        dataIndex: prev?.dataIndex,
                        seriesId:
                          prev?.dataIndex === 0 || prev?.dataIndex
                            ? "auto-generated-id-0"
                            : "",
                      }))
                    }
                  />
                </>
              ) : (
                ""
              )}
            </Box>
            <div
              style={{
                width: "2px",
                background: `${darkMode ? "white" : "black"}`,
              }}
            ></div>
            <Box sx={{ width: "50%" }}></Box>
          </Stack>
        </div>
      ) : (
        ""
      )}
    </div>
  );
};

export default Chart4;
