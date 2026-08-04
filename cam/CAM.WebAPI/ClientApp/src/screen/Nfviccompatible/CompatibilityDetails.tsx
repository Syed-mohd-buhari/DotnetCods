import React, { useEffect, useState, useCallback } from "react";
import { useSearchParams } from "react-router-dom";
import NFVICCompatibilityGrid from "./NFVICCompatibilityGrid";
import NVFChart from "./ChartComponent";
import { createTheme, ThemeProvider } from "@mui/material/styles";
import { useTheme } from "../../Context/ThemeContext";
import { NFVICCompatibilityQueryDto } from "../../Model/NfvicCompatible";
import { GetNFVICCompatibleGrid } from "../../Redux/Action/Nfviccompatible/NfvicCompatibelGrid";
import { useResourceTableCrud } from "../../Hook/useResourceTableCrud";
import Paginate from "../../Components/PaginationComponent";
import { useSelector } from "react-redux";
import { RootState } from "../../Redux/Store/rootStore";
import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Chip,
  FormLabel,
  Typography,
} from "@mui/material";
import { IoMdArrowUp } from "react-icons/io";
import { useAuth } from "../../Hook/useAuth";
import { GetNFVICompatibleReport } from "../../Redux/Action/Nfviccompatible/NfvicCompatibleDownloadAction";

const lightTheme = createTheme({
  palette: {
    mode: "light",
    primary: { main: "#1976d2" },
    background: { default: "#ffffff" },
  },
});

const darkTheme = createTheme({
  palette: {
    mode: "dark",
    primary: { main: "#90caf9" },
    background: { default: "#121212" },
  },
});

interface CompatibilityDetailsProps {
  primaryFilterKey: "vendorId" | "marketId" | "verticalId" | "vodafoneId";
  primaryFilterLabel: string;
}

const CompatibilityDetails: React.FC<CompatibilityDetailsProps> = ({
  primaryFilterKey,
  primaryFilterLabel,
}) => {
  const [searchParams] = useSearchParams();
  const { darkMode, selectDarkMode } = useTheme();

  const vmVarIdParam = searchParams.get("vmVarId");
  const vmVarId = vmVarIdParam ? Number(vmVarIdParam) : 0;

  // Extract all filter params (numbers or undefined)
  const vendorId = searchParams.get("vendorId")
    ? Number(searchParams.get("vendorId"))
    : undefined;
  const marketId = searchParams.get("marketId")
    ? Number(searchParams.get("marketId"))
    : undefined;
  const verticalId = searchParams.get("verticalId")
    ? Number(searchParams.get("verticalId"))
    : undefined;
  const vodafoneId = searchParams.get("vodafoneId")
    ? Number(searchParams.get("vodafoneId"))
    : undefined;

  // Extract all label params (strings or null)
  const vendorLabel = searchParams.get("vendorLabel");
  const marketLabel = searchParams.get("marketLabel");
  const verticalLabel = searchParams.get("verticalLabel");
  const vodafoneLabel = searchParams.get("vodafoneLabel");

  // Build filter display data for chips
  const displayHeading = [
    { name: "Vendor", list: vendorLabel ? [vendorLabel] : [] },
    { name: "Market", list: marketLabel ? [marketLabel] : [] },
    { name: "Vertical", list: verticalLabel ? [verticalLabel] : [] },
    { name: "Vodafone Name", list: vodafoneLabel ? [vodafoneLabel] : [] },
  ].filter(({ list }) => list.length > 0);

  const isQueryParams = displayHeading.length > 0;
  const theme = darkMode ? darkTheme : lightTheme;

  // Get the primary filter's value dynamically
  const primaryFilterValue = (() => {
    switch (primaryFilterKey) {
      case "vendorId":
        return vendorId;
      case "marketId":
        return marketId;
      case "verticalId":
        return verticalId;
      case "vodafoneId":
        return vodafoneId;
      default:
        return undefined;
    }
  })();

  // Compose base query DTO
  const baseQuery: NFVICCompatibilityQueryDto = {
    page: 1,
    pageSize: 10,
    deleted: false,
    market: marketId !== undefined ? [marketId] : [],
    oemId: vendorId !== undefined ? [vendorId] : [],
    domain: verticalId !== undefined ? [verticalId] : [],
    vodafoneNameId: vodafoneId !== undefined ? [vodafoneId] : [],
    sortBy: "",
    isSortAscending: true,
    orphan: true,
    principalId: 0,
    application: [],
    designComponent: [],
    currentVNF: [],
    minimumVNF: [],
    plannedVNF: [],
    plannedUpgrade: undefined,
    status: [],
    deleiveryStatus: [],
    eduSpoc: [],
    subDomainSpoc: [],
    lastModified: undefined,
    lastModifiedBy: [],
    lastModifiedValue: undefined,
  };

  const { pageSize, isPermesso } = useAuth();
  // Fetch grid data callback
  const fetchGrid = useCallback(
    async (queryParam: NFVICCompatibilityQueryDto) => {
      if (!vmVarId || !isPermesso) return;
      console.log(
        "Fetching grid for vmVarId:",
        vmVarId,
        "with query:",
        queryParam
      );
      await GetNFVICCompatibleGrid(vmVarId, queryParam, false);
    },
    [vmVarId, isPermesso]
  );

  // Hook for pagination and query management
  const { query, next, back } = useResourceTableCrud(baseQuery, fetchGrid);

  // Select data from redux store
  const GridDto = useSelector(
    (state: RootState) =>
      state.nFVICCompatibleReducer.NFVICCompatibilityGridResult
  );

  const [renderGridState, setRenderGridState] = useState<any>();

  useEffect(() => {
    if (GridDto) {
      setRenderGridState(GridDto.gridRender?.render ?? []);
    }
  }, [GridDto]);

  // Enable dark mode on mount, disable on unmount
  useEffect(() => {
    selectDarkMode(true);
    return () => selectDarkMode(false);
  }, [selectDarkMode]);

  if (!primaryFilterValue) return <div>Missing {primaryFilterLabel}</div>;

  const handleDownloadExcel = async () => {
    const query: any = {
      page: 0,
      pageSize: 0,
      sortBy: "",
      isSortAscending: false,
      deleted: false,
      orphan: false,
      principalId: 0,
      application: [],
      domain: [],
      designComponent: [],
      currentVNF: [],
      minimumVNF: [],
      plannedVNF: [],
      plannedUpgrade: undefined,
      status: [],
      deleiveryStatus: [],
      eduSpoc: [],
      subDomainSpoc: [],
      lastModified: undefined,
      lastModifiedBy: [],
      lastModifiedValue: undefined,
    };

    switch (primaryFilterKey) {
      case "marketId":
        query.market = [primaryFilterValue];
        break;
      case "vendorId":
        query.oemId = [primaryFilterValue];
        break;
      case "verticalId":
        query.domain = [primaryFilterValue];
        break;
      case "vodafoneId":
        query.vodafoneNameId = [primaryFilterValue];
        break;
      default:
        break;
    }

    const result = await GetNFVICompatibleReport(vmVarId, query);
    if (result !== undefined) {
      const url = window.URL.createObjectURL(result.file);
      const a = document.createElement("a");
      a.href = url;
      a.download = result.fileName;
      a.click();
    }
  };

  return (
    <ThemeProvider theme={theme}>
      <div className="pageContainer" style={{ marginTop: "15px" }}>
        <div
          className="headerPage row mx-0 justify-content-between align-items-center"
          style={{ marginTop: "15px" }}
        >
          <h3 className="voda-bold">Compatibility @Glance</h3>
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
              {displayHeading.map(({ name, list }) => (
                <div key={name} className="d-flex align-items-start mb-3">
                  <div style={{ minWidth: 150 }}>
                    <FormLabel
                      component="legend"
                      sx={{
                        fontWeight: "bold !important",
                        fontSize: "16px !important",
                        marginTop: "12px",
                        marginRight: "16px",
                      }}
                    >
                      {name}
                    </FormLabel>
                  </div>
                  <div
                    style={{ display: "flex", flexWrap: "wrap", gap: "8px" }}
                  >
                    {list.map((val, i) => (
                      <Chip
                        key={`${name}-${i}`}
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
                  </div>
                </div>
              ))}
            </AccordionDetails>
          </Accordion>
        )}

        {isPermesso && (
          <NVFChart
            vmVarId={vmVarId}
            selectedVendorId={vendorId}
            selectedMarketId={marketId}
            selectedVerticalId={verticalId}
            selectedVodafoneId={vodafoneId}
            darkMode={darkMode}
          />
        )}
        <div className="headerPage row mx-0 justify-content-end align-items-center">
          <div className="d-flex" style={{ marginRight: "80px" }}>
            <button
              className="download-to-excel mrl-10 grid-main-btn"
              onClick={handleDownloadExcel}
            >
              Download to Excel
            </button>
          </div>
        </div>

        <NFVICCompatibilityGrid
          data={GridDto?.items ?? []}
          renderGrid={renderGridState ?? []}
        />

        <Paginate
          pagination={{ page: query.page, pageSize: query.pageSize }}
          totalItems={GridDto?.totalItems ?? 0}
          actions={{ next, back }}
        />
      </div>
    </ThemeProvider>
  );
};

export default CompatibilityDetails;
