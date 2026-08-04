import React, { useCallback, useEffect, useState } from "react";
import "../Css/App.css";
import "../Css/index.css";
import "../Css/NetworkElement.css";
import { useDispatch, useSelector } from "react-redux";
import { useNavigate, useLocation } from "react-router-dom";

import { useResourceTableCrud } from "../Hook/useResourceTableCrud";
import setLoader from "../Redux/Action/LoaderAction";

import { RootState } from "../Redux/Store/rootStore";
import {
  NFVICCompatibilityDtoGrid,
  NFVICCompatibilityQueryDto,
} from "../Model/NfvicCompatible";
import { GetNFVICCompatibleGrid } from "../Redux/Action/Nfviccompatible/NfvicCompatibelGrid";

import { useAuth } from "../Hook/useAuth";
import Paginate from "../Components/PaginationComponent";
import NFVICCompatibilityGrid from "../screen/Nfviccompatible/NFVICCompatibilityGrid";
import { CustomGridRender, QueryObjectGrid } from "../Model/Common";
import Box from "@mui/material/Box";
import InputLabel from "@mui/material/InputLabel";
import MenuItem from "@mui/material/MenuItem";
import FormControl from "@mui/material/FormControl";
import Select from "@mui/material/Select";
import { createTheme, ThemeProvider } from "@mui/material/styles";

import { GetVmWareDropdown } from "../Redux/Action/Nfviccompatible/NfvicCompatibelGrid";
import NVFChart from "../screen/Nfviccompatible/ChartComponent";
import { Divider, Stack } from "@mui/material";
import { useTheme } from "../Context/ThemeContext";
import { GetNFVICompatibleReport } from "../Redux/Action/Nfviccompatible/NfvicCompatibleDownloadAction";

const paginationQuery: NFVICCompatibilityQueryDto = {
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  lastModified: undefined,
  lastModifiedValue: undefined,
  principalId: undefined,
  deleted: false,
  orphan: false,
  lastModifiedBy: [],
  market: [],
  application: [],
  domain: [],
  designComponent: [],
  currentVNF: [],
  minimumVNF: [],
  plannedVNF: [],
  status: [],
  deleiveryStatus: [],
  eduSpoc: [],
  subDomainSpoc: [],
  plannedUpgrade: undefined,
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

const NFVICContainer = () => {
  const [data, setData] = useState<NFVICCompatibilityDtoGrid[] | undefined>([]);
  const [renderGridState, setRenderGridState] = useState<any>();
  const [selectedVmVarId, setSelectedVmVarId] = useState<string>("");
  const { darkMode, selectDarkMode } = useTheme();
  const dispatch = useDispatch();
  const { pageSize, isPermesso } = useAuth();
  const theme = darkMode ? darkTheme : lightTheme;
  const GridDto = useSelector(
    (state: RootState) =>
      state.nFVICCompatibleReducer.NFVICCompatibilityGridResult
  );

  const dropdowns = useSelector(
    (state: RootState) => state.nFVICCompatibleReducer.dropdowns
  );

  const vmwareMswPlatformOptions = dropdowns?.vmwareMswPlaftform ?? {};

  useEffect(() => {
    paginationQuery.pageSize = pageSize;
  }, [pageSize]);

  const fetchGridWithVmVarId = useCallback(
    (queryParam: NFVICCompatibilityQueryDto) => {
      if (!selectedVmVarId) return Promise.resolve();
      return GetNFVICCompatibleGrid(Number(selectedVmVarId), queryParam);
    },
    [selectedVmVarId]
  );

  const { query, next, back } = useResourceTableCrud(
    paginationQuery,
    selectedVmVarId ? fetchGridWithVmVarId : undefined
  );

  const InvocheDownload = async () => {
    let result = await GetNFVICompatibleReport(Number(selectedVmVarId), query);
    if (result !== undefined) {
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };

  useEffect(() => {
    if (GridDto) {
      setData(GridDto?.items);
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
      setLoader("REMOVE", "GetNFVICCompatibleGrid");
    }
  }, [GridDto]);

  useEffect(() => {
    selectDarkMode(true);
    return () => {
      selectDarkMode(false);
    };
  }, []);

  useEffect(() => {
    if (isPermesso) {
      dispatch(GetVmWareDropdown() as any);
    }
  }, [dispatch, isPermesso]);

  useEffect(() => {
    const firstKey = Object.keys(vmwareMswPlatformOptions)?.[0];
    if (!selectedVmVarId && firstKey) {
      setSelectedVmVarId(firstKey);
    }
  }, [vmwareMswPlatformOptions]);

  return (
    <ThemeProvider theme={theme}>
      <div className="pageContainer">
        <div className="headerPage row mx-0 justify-content-between align-items-center">
          <h3 className="voda-bold">Compatibility @Glance</h3>
          <FormControl variant="standard" sx={{ minWidth: 350 }}>
            <InputLabel id="vmware-select-label" className="voda-bold">
              Select VNF
            </InputLabel>
            <Select
              labelId="vmware-select-label"
              id="vmware-select"
              value={selectedVmVarId}
              onChange={(e) => setSelectedVmVarId(e.target.value)}
              MenuProps={{ disableScrollLock: true }}
            >
              {Object.entries(vmwareMswPlatformOptions).map(([key, value]) => (
                <MenuItem key={key} value={key}>
                  {value}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
        </div>

        <Box sx={{ mb: 4 }}>
          <Stack
            sx={{
              display: "flex",
              flexDirection: "row",
              spacing: 2,
              width: "100%",
            }}
          >
            <Box sx={{ width: "100%" }}>
              {!isNaN(Number(selectedVmVarId)) && (
                <NVFChart
                  vmVarId={Number(selectedVmVarId)}
                  darkMode={darkMode}
                />
              )}
            </Box>
          </Stack>
        </Box>
        <div className="headerPage row mx-0 justify-content-end">
          <div className="d-flex" style={{ marginRight: "80px" }}>
            <button
              className="download-to-excel mrl-10 grid-main-btn
            "
              onClick={() => InvocheDownload()}
            >
              {/* <img src={require("../img/excel.png")} /> */}
              Download to Excel
            </button>
          </div>
        </div>
        <NFVICCompatibilityGrid
          data={data}
          renderGrid={renderGridState?.render ?? []}
        />

        <Paginate
          pagination={{ page: query.page, pageSize: query.pageSize }}
          totalItems={GridDto?.totalItems}
          actions={{ next, back }}
        />
      </div>
    </ThemeProvider>
  );
};

export default NFVICContainer;
