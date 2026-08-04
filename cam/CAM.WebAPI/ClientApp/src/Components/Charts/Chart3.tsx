import React, { useState, useEffect } from "react";
import Graph from "react-graph-vis";
import "vis-network/styles/vis-network.css";
import {
  GetNetworkVisualizerGraphApiResource,
  GetNetworkVisualizerGraph,
} from "../../Redux/Action/LookUp/NetworkElementGraph/NetworkVisualizerGraphAction";
import Box from "@mui/material/Box";
import Stack from "@mui/material/Stack";
import InputLabel from "@mui/material/InputLabel";
import MenuItem from "@mui/material/MenuItem";
import FormControl from "@mui/material/FormControl";
import Select, { SelectChangeEvent } from "@mui/material/Select";
import { InputAdornment } from "@mui/material";
import IconButton from "@mui/material/IconButton";
import { IoClose } from "react-icons/io5";
import { createTheme, ThemeProvider } from "@mui/material/styles";
import { useTheme } from "../../Context/ThemeContext";
import { Network } from "vis-network";
import { safeNumber } from "../../Hook/Common";

interface Node {
  id: string;
  label: string;
  shape: string;
  title?: string;
  group?: string;
  x?: number;
  y?: number;
  color?: string;
}

interface Edge {
  from: string;
  to: string;
  smooth: { type: string; roundness: number };
  label: string;
}

interface GraphData {
  nodes: Node[];
  edges: Edge[];
}

const Chart3 = () => {
  const [networkVisualizerGraphData, setNetworkVisualizerGraphData] =
    useState<any>();
  const [networkVisualizerResource, setNetworkVisualizerResource] =
    useState<any>();

  const getNetworkVisualizerGraphApiResource = async () => {
    const networkVisualizerGraphApiResource =
      await GetNetworkVisualizerGraphApiResource();
    if (networkVisualizerGraphApiResource.ResultDtoCreate) {
      const formattedNetworkVisualizerGraphApiResource = Object.entries(
        networkVisualizerGraphApiResource.ResultDtoCreate
      );
      setNetworkVisualizerResource(formattedNetworkVisualizerGraphApiResource);
    }
  };

  const getNetworkVisualizerGraph = async (filters) => {
    const graphData = await GetNetworkVisualizerGraph(filters);
    setNetworkVisualizerGraphData(graphData?.ResultDtoCreate);
  };
  useEffect(() => {
    if (!networkVisualizerGraphData?.items?.length) {
      getNetworkVisualizerGraph({});
    }
    if (!networkVisualizerResource?.length) {
      getNetworkVisualizerGraphApiResource();
    }
  }, []);

  const [graphData, setGraphData] = useState<GraphData>({
    nodes: [],
    edges: [],
  });

  const [groupColors, setGroupColors] = useState(new Map());

  const [selectedOpco, setSelectedOpco] = useState("");
  const [selectedService, setSelectedService] = useState("");

  const handleOpcoChange = (event: SelectChangeEvent) => {
    setSelectedOpco(event.target.value as string);
    const filterObj = {};
    if (selectedService) {
      filterObj["supportedService"] = [safeNumber(selectedService)];
    }
    if (event.target.value) {
      filterObj["opCo"] = [event.target.value];
    }
    getNetworkVisualizerGraph(filterObj);
  };
  const handleServiceChange = (event: SelectChangeEvent) => {
    setSelectedService(event.target.value as string);
    const filterObj = {};
    if (event.target.value) {
      filterObj["supportedService"] = [safeNumber(event.target.value)];
    }
    if (selectedOpco) {
      filterObj["opCo"] = [selectedOpco];
    }
    getNetworkVisualizerGraph(filterObj);
  };

  const handleOpcoClear = () => {
    setSelectedOpco("");
    const filterObj = {};
    if (selectedService) {
      filterObj["supportedService"] = [safeNumber(selectedService)];
    }
    getNetworkVisualizerGraph(filterObj);
  };
  const handleServiceClear = () => {
    setSelectedService("");
    const filterObj = {};
    if (selectedOpco) {
      filterObj["opCo"] = [selectedOpco];
    }
    getNetworkVisualizerGraph(filterObj);
  };

  const options = {
    nodes: {
      shape: "circle",
      size: 10,
      font: {
        size: 10,
        face: "Tahoma",
        color: "#000",
        strokeWidth: 0,
      },
      label: {
        position: "bottom",
        margin: 10,
      },
      scaling: {
        min: 10,
        max: 30,
      },
    },
    edges: {
      width: 0.15,
      color: { inherit: "from" },
      arrows: {
        to: { enabled: false },
        from: { enabled: false },
      },
    },
    physics: {
      enabled: false,
      forceAtlas2Based: {
        gravitationalConstant: -26,
        centralGravity: 0.005,
        springLength: 230,
        springConstant: 0.18,
      },
      maxVelocity: 146,
      solver: "forceAtlas2Based",
      timestep: 0.35,
      stabilization: { iterations: 150 },
    },
    interaction: {
      tooltipDelay: 200,
      hideEdgesOnDrag: false,
      hideEdgesOnZoom: true,
    },
    height: "900px",
  };

  type Item = {
    elementName: string;
    vendor: string;
    product: string;
    vrfName?: string;
    location?: string;
    ipAddress?: string | undefined;
  };
  type TitleEntry = {
    node: string;
    location: string;
    vendor: string;
    vrf: string;
    product: string;
    ipAddress: string | undefined;
  };

  const colors = [
    "#FF5733",
    "#33FF57",
    "#3357FF",
    "#FF33A8",
    "#FFB833",
    "#33FFF7",
    "#C733FF",
    "#FF5733",
    "#C70039",
    "#FFC300",
    "#DAF7A6",
    "#900C3F",
    "#581845",
    "#FF5733",
    "#FF6F61",
    "#6B5B93",
    "#88B04B",
    "#F7CAC9",
    "#92A8D1",
    "#955251",
    "#B9D9B9",
  ];

  const generateUniqueColor = (index: number): string => {
    return colors[index % colors.length];
  };

  const createGraphData = (data: Item[]): GraphData => {
    const nodes: Node[] = [];
    const edges: Edge[] = [];
    const connectionCount = new Map<string, number>();
    const nodeMap = new Map<string, { nodeId: string; title: TitleEntry[] }>();

    const groupCoordinates = new Map<
      string,
      { centerX: number; centerY: number }
    >();
    const groupCounts = new Map<string, number>();
    const nodeSpacing = 70;
    const groupOffset = 300;

    const totalGroups = new Set<string>();
    data.forEach((item) => totalGroups.add(item.product));

    const totalGroupsCount = totalGroups.size;
    const groupColors = new Map<string, string>();

    let groupIndex = 0;
    totalGroups.forEach((groupName) => {
      const centerX =
        (groupIndex % 4) * groupOffset - (totalGroupsCount / 4) * groupOffset;
      const centerY = Math.floor(groupIndex / 4) * groupOffset;
      groupCoordinates.set(groupName, { centerX, centerY });
      groupCounts.set(groupName, 0);
      groupColors.set(groupName, generateUniqueColor(groupIndex));
      groupIndex++;
    });

    data.forEach((item) => {
      const nodeId = item.elementName;
      const vendorName = item.vendor;
      const groupName = item.product;

      const { centerX, centerY } = groupCoordinates.get(groupName)!;
      const count = groupCounts.get(groupName)!;

      const angle = (count % 5) * ((2 * Math.PI) / 5);
      let x = centerX + Math.cos(angle) * nodeSpacing;
      let y = centerY + Math.sin(angle) * nodeSpacing;

      const minDistance = 80;
      nodes.forEach((existingNode) => {
        if (existingNode.x !== undefined && existingNode.y !== undefined) {
          const distance = Math.sqrt(
            Math.pow(existingNode.x - x, 2) + Math.pow(existingNode.y - y, 2)
          );
          if (distance < minDistance) {
            const angleOffset = Math.random() * (2 * Math.PI);
            x += (Math.cos(angleOffset) * (minDistance - distance)) / 2;
            y += (Math.sin(angleOffset) * (minDistance - distance)) / 4;
          }
        }
      });

      if (!nodeMap.has(nodeId)) {
        const productNodeId = `product${nodes.length}`;
        nodeMap.set(nodeId, {
          nodeId: productNodeId,
          title: [
            {
              node: nodeId,
              location: item.location || "",
              vendor: vendorName,
              vrf: item.vrfName || "",
              product: groupName,
              ipAddress: item.ipAddress || "",
            },
          ],
        });
        nodes.push({
          id: productNodeId,
          label: nodeId,
          title: formatTitle(nodeMap.get(nodeId)!.title),
          shape: "circle",
          group: groupName,
          color: groupColors.get(groupName),
          x: x,
          y: y,
        });

        groupCounts.set(groupName, count + 1);
      } else {
        const existingProduct = nodeMap.get(nodeId);
        if (existingProduct) {
          const existingGroup = nodes.find(
            (node) => node.id === existingProduct.nodeId
          )?.group;

          if (existingGroup === groupName) {
            existingProduct.title.push({
              node: nodeId,
              location: item.location || "",
              vendor: vendorName,
              vrf: item.vrfName || "",
              product: groupName,
              ipAddress: item.ipAddress || "",
            });

            const nodeIndex = nodes.findIndex(
              (node) => node.id === existingProduct.nodeId
            );
            if (nodeIndex !== -1) {
              nodes[nodeIndex].title = formatTitle(existingProduct.title);
            }
          } else {
            const productNodeId = `product${nodes.length}`;
            const newEntry = {
              node: nodeId,
              location: item.location || "",
              vendor: vendorName,
              vrf: item.vrfName || "",
              product: groupName,
              ipAddress: item.ipAddress || "",
            };
            nodeMap.set(nodeId + groupName, {
              nodeId: productNodeId,
              title: [newEntry],
            });
            nodes.push({
              id: productNodeId,
              label: nodeId,
              title: formatTitle([newEntry]),
              shape: "circle",
              group: groupName,
              x: x,
              y: y,
              color: groupColors.get(groupName),
            });

            groupCounts.set(groupName, count + 1);
          }
        }
      }
    });

    const vrfGroups = new Map<string, string[]>();
    data.forEach((item) => {
      const vrfId = item.vrfName!;
      const nodeId = item.elementName;

      if (!vrfGroups.has(vrfId)) {
        vrfGroups.set(vrfId, []);
      }
      vrfGroups.get(vrfId)!.push(nodeMap.get(nodeId)!.nodeId);
    });

    vrfGroups.forEach((productIds, vrfId) => {
      for (let i = 0; i < productIds.length; i++) {
        for (let j = i + 1; j < productIds.length; j++) {
          const connectionKey = `${productIds[i]}-${productIds[j]}`;
          const count = (connectionCount.get(connectionKey) || 0) + 1;
          connectionCount.set(connectionKey, count);
          if (vrfId) {
            edges.push({
              from: productIds[i],
              to: productIds[j],
              smooth: { type: "curvedCW", roundness: 0.2 * count },
              label: vrfId,
            });
          }
        }
      }
    });

    setGroupColors(groupColors);
    setGraphData({ nodes, edges });
    return { nodes, edges };
  };

  const formatTitle = (
    entries: {
      node: string;
      location: string;
      vendor: string;
      vrf: string;
      product: string;
      ipAddress: string | undefined;
    }[]
  ) => {
    return entries
      .map((entry, index) => {
        return `{ node: ${entry.node}, location: ${
          entry.location || "-"
        }, vendor: ${entry.vendor}, vrf: ${entry.vrf || "-"}, product: ${
          entry.product || "-"
        }, ipAddress: ${entry.ipAddress || "-"} }${
          index < entries.length - 1 ? "," : ""
        }`;
      })
      .join("\n");
  };

  const [network, setNetwork] = useState<Network | null>(null); // Type the state

  const handleNetwork = (networkInstance: Network) => {
    setNetwork(networkInstance);
  };

  useEffect(() => {
    if (network) {
      // Reset zoom when graphData changes
      network.fit(); // This will fit the graph to the viewport
    }
  }, [graphData, network]);

  useEffect(() => {
    if (networkVisualizerGraphData?.items?.length) {
      createGraphData(networkVisualizerGraphData?.items);
    } else {
      setGraphData({ nodes: [], edges: [] });
    }
  }, [networkVisualizerGraphData]);

  const events = {
    select: function (event) {
      var { nodes, edges } = event;
    },
  };

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

  return (
    <div>
      {networkVisualizerResource?.length ? (
        <div>
          <ThemeProvider theme={theme}>
            <div className="container">
              <div className="headerPage row mx-0 justify-content-between ">
                <h3 className="voda-bold fz-28" style={{ textAlign: "left" }}>
                  Network Visualizer
                </h3>
              </div>
              <div className="mt-5" style={{ overflow: "hidden" }}>
                <Stack
                  direction="row"
                  sx={{
                    width: "100%",
                    textAlign: "left",
                  }}
                  spacing={2}
                >
                  <Box sx={{ width: "40%" }}>
                    <FormControl variant="standard" fullWidth>
                      <InputLabel id="demo-simple-select-standard-label">
                        Opco
                      </InputLabel>
                      <Select
                        labelId="demo-simple-select-standard-label"
                        id="demo-simple-select-standard"
                        value={selectedOpco}
                        onChange={handleOpcoChange}
                        label="Opco"
                        endAdornment={
                          selectedOpco && (
                            <InputAdornment position="end">
                              <IconButton onClick={handleOpcoClear} edge="end">
                                <IoClose />
                              </IconButton>
                            </InputAdornment>
                          )
                        }
                        IconComponent={selectedOpco ? () => null : undefined}
                        MenuProps={{ disableScrollLock: true }}
                      >
                        {/* <MenuItem value="">
                  <em>None</em>
                </MenuItem> */}
                        {networkVisualizerResource?.length &&
                          Object.entries(networkVisualizerResource[0][1])?.map(
                            ([key, value]) => (
                              <MenuItem key={key} value={key}>
                                {value as any}
                              </MenuItem>
                            )
                          )}
                      </Select>
                    </FormControl>
                  </Box>
                  <Box sx={{ width: "40%" }}>
                    <FormControl variant="standard" fullWidth>
                      <InputLabel id="demo-simple-select-standard-label">
                        Support Service
                      </InputLabel>
                      <Select
                        labelId="demo-simple-select-standard-label"
                        id="demo-simple-select-standard"
                        value={selectedService}
                        onChange={handleServiceChange}
                        label="Support Service"
                        endAdornment={
                          selectedService && (
                            <InputAdornment position="end">
                              <IconButton
                                onClick={handleServiceClear}
                                edge="end"
                              >
                                <IoClose />
                              </IconButton>
                            </InputAdornment>
                          )
                        }
                        IconComponent={selectedService ? () => null : undefined}
                        MenuProps={{ disableScrollLock: true }}
                      >
                        {/* <MenuItem value="">
                  <em>None</em>
                </MenuItem> */}
                        {networkVisualizerResource?.length &&
                          Object.entries(networkVisualizerResource[1][1])?.map(
                            ([key, value]) => (
                              <MenuItem key={key} value={key}>
                                {value as any}
                              </MenuItem>
                            )
                          )}
                      </Select>
                    </FormControl>
                  </Box>
                </Stack>
              </div>
              <div className="mt-3">
                <Stack
                  direction="row"
                  sx={{
                    width: "100%",
                    textAlign: "left",
                  }}
                  spacing={2}
                >
                  <Box sx={{ width: "85%" }}>
                    {graphData && (
                      <Graph
                        graph={graphData}
                        options={options}
                        events={events}
                        getNetwork={handleNetwork}
                      />
                    )}
                  </Box>
                  <Box sx={{ width: "15%" }}>
                    {graphData && graphData?.nodes?.length ? (
                      <>
                        <h3 className="voda-bold fz-28 text-center">
                          Products
                        </h3>{" "}
                        {groupColors && (
                          <div
                            className="legend"
                            style={{
                              background: darkMode ? "black" : "white",
                              border: darkMode
                                ? "2px solid white"
                                : "2px solid black",
                            }}
                          >
                            {Array.from(groupColors.entries()).map(
                              ([groupName, color]) => (
                                <li
                                  key={groupName}
                                  style={{
                                    listStyle: "none",
                                    margin: "15px",
                                    color: darkMode ? "white" : "black",
                                  }}
                                >
                                  <span
                                    style={{
                                      backgroundColor: color,
                                      width: "12px",
                                      height: "12px",
                                      display: "inline-block",
                                      marginRight: "8px",
                                    }}
                                  />
                                  {groupName}
                                </li>
                              )
                            )}
                          </div>
                        )}
                      </>
                    ) : (
                      ""
                    )}
                  </Box>
                </Stack>
              </div>
            </div>
            <div className="mt-2"></div>
          </ThemeProvider>
        </div>
      ) : (
        ""
      )}
    </div>
  );
};

export default Chart3;
