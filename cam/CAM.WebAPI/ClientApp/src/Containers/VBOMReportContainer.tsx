import React, { useEffect, useState } from "react";
import {
  Box,
  Paper,
  Stack,
  List,
  ListItem,
  ListItemText,
  Divider,
  Grid,
  Typography,
} from "@mui/material";
import { styled } from "@mui/material/styles";
import {
  DropdownInputComponent,
  ShowYearInputComponent,
} from "../Components/FormField";
import { resourceArrayRefactor } from "../Hook/Dictionary";
import { Link, useLocation, useNavigate } from "react-router-dom";
import { GoArrowLeft } from "react-icons/go";
import { useTheme } from "../Context/ThemeContext";
import { useAuth } from "../Hook/useAuth";
import { Dropdown } from "react-bootstrap";
import setLoader from "../Redux/Action/LoaderAction";
import {
  GetVBOMReportGetAllResource,
  GetVBOMReportGrid,
} from "../Redux/Action/VBOMReport/VBOMReportGridAction";
import { useResourceTableCrud } from "../Hook/useResourceTableCrud";
import { useSelector } from "react-redux";
import { RootState } from "../Redux/Store/rootStore";
import { RelatedRecordsResultDto } from "../Model/CommonModels";
import { CustomGridRender } from "../Model/Common";
import MUIPaginationComponent from "../Components/MUIPaginationComponent";
import VBOMReportGrid from "../screen/VBOMReport/VBOMReportGrid";

const ItemList = styled(Paper)({
  padding: 0,
  maxWidth: 150,
  border: "1px solid #e60000",
  borderRadius: 0,
  background: "#fff",
  flexGrow: 1,
});

const Item = styled(Paper)(({ theme }) => ({
  ...theme.typography.body2,
  textAlign: "center",
  backgroundColor: "#cccccc00",
  color: theme.palette.text.secondary,
  height: 45,
  lineHeight: "41px",
  marginBottom: "1rem",
  fontFamily: "VodafoneRg",
  fontWeight: "bold",
  fontSize: "15px",
}));

export let paginationQuery = {
  financialYear: [],
  opcoName: [11],
  hardwareType: [],
  vnfName: [],
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  principalId: undefined,
  orphan: false,
  deleted: false,
  lastModifiedBy: [],
};

const VBOMReportContainer = () => {
  const { darkMode } = useTheme();
  const navigate = useNavigate();
  const location: any = useLocation();
  const { readonly, isPermesso, pageSize } = useAuth();
  const [data, setData] = useState<any>([]);
  const [visualData, setVisualData] = useState<any>([]);
  const [prevPage, setPrevPage] = useState<string>();
  const [orphanColor, setOrphanColor] = useState(false);
  const [opcoDropdown, setOpcoDropdown] = useState<any>([]);
  const [hardwareDropdown, setHardwareDropdown] = useState<any>([]);
  const [vnfNameDropdown, setVnfNamesDropdown] = useState<any>([]);
  const [opcoValue, setOpcoValue] = useState<any>(null);
  const [hardwareValue, setHardwareValue] = useState<any>(null);
  const [vnfName, setVnfName] = useState<any>(null);
  const [financialYear, setFinancialYear] = useState<any>(null);

  const [renderGridState, setRenderGridState] = useState<any>();
  let GridDto = useSelector(
    (state: RootState) => state.VBOMReportGridReducer.VBOMReportGridResult
  );
  const { query, setQuery, next, back, updatePageSize } = useResourceTableCrud(
    paginationQuery,
    undefined
  );

  const [isVisibleModalRelated, setIsVisibleModalRelated] = useState(false);
  const [relatedRecord, setRelatedRecord] = useState<RelatedRecordsResultDto>();
  const [show, setShow] = useState(false);
  const [alerStatus, setAlertStatus] = useState({
    message: "",
    class: "light",
  });

  useEffect(() => {
    if (GridDto !== undefined || GridDto !== null) {
      const groupedByText = GridDto?.items?.reduce((result, item) => {
        (result[item.hardwareType] = result[item.hardwareType] || []).push(
          item
        );
        return result;
      }, {}) as any;
      const output = groupedByText
        ? Object.keys(groupedByText).map((key) => ({
            hardwareType: key,
            list: groupedByText[key],
          }))
        : [];
      setVisualData(output);
      setData(GridDto?.items);
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
      setLoader("REMOVE", "GetVBOMReportGrid");
    }
  }, [GridDto]);

  useEffect(() => {
    if (query && isPermesso) {
      setLoader("ADD", "GetVBOMReportGrid");
      const payload = {
        ...query,
        opcoName: opcoValue?.key ? [opcoValue?.key] : [],
        hardwareType:
          hardwareValue?.key && hardwareValue?.key !== "all"
            ? [String(hardwareValue?.key)]
            : [],
        financialYear: financialYear ? [financialYear] : [],
        vnfName: vnfName?.key && vnfName?.key !== "all" ? [vnfName?.key] : [],
      };
      GetVBOMReportGrid(payload).then((x) =>
        setLoader("REMOVE", "GetVBOMReportGrid")
      );
      setLoader("REMOVE", "GetVBOMReportGrid");
    }
  }, [query]);

  useEffect(() => {
    if (isPermesso) {
      callFilterApi();
    }
  }, [isPermesso]);

  const callFilterApi = async () => {
    setLoader("ADD", "GetVBOMReportResource");
    let response: any = await GetVBOMReportGetAllResource();
    if (response) {
      const { allOpcos, hardwareTypes, vnfNames, currentFy } =
        response?.data as any;
      const opcoRes = allOpcos?.map((res) => {
        return { key: res.value, text: res.text };
      });
      const defaultOpco = allOpcos
        ?.filter((res: any) => res?.text?.toLowerCase() === "uk")
        ?.map((res) => {
          return { key: res.value, value: res.text };
        })[0];
      setOpcoDropdown(opcoRes ?? null);
      setHardwareDropdown(hardwareTypes ?? null);
      setVnfNamesDropdown(vnfNames ?? null);
      setFinancialYear(currentFy ?? null);
      setOpcoValue(defaultOpco ?? null);
      setQuery({ ...query, opcoName: defaultOpco?.value } as any);
      setLoader("REMOVE", "GetVBOMReportResource");
    }
    setLoader("REMOVE", "GetVBOMReportResource");
  };

  const getRandomLightColor = () => {
    const randChannel = () => Math.floor(180 + Math.random() * 65);
    const r = randChannel();
    const g = randChannel();
    const b = randChannel();
    return `#${r.toString(16).padStart(2, "0")}${g
      .toString(16)
      .padStart(2, "0")}${b.toString(16).padStart(2, "0")}`;
  };

  const adjustColor = (color, amount) => {
    color = color[0] === "#" ? color.slice(1) : color;
    return (
      "#" +
      color
        .match(/.{2}/g)
        .map((hex) =>
          Math.min(255, Math.max(0, parseInt(hex, 16) + amount))
            .toString(16)
            .padStart(2, "0")
        )
        .join("")
    );
  };

  return (
    <div className="pageContainer mb-0">
      <div className="headerPage row mx-0 justify-content-between">
        <div className="d-flex flex-row align-items-center">
          <h3 className="voda-bold">VBOM Report</h3>
        </div>
        {/* <div className="d-flex">
          <button
            className="download-to-excel mrl-10 grid-main-btn"
            // onClick={() => setDownloadModalFlag(true)}
          >
            Download to Excel
          </button>
        </div> */}
      </div>

      <Box sx={{ flexGrow: 1 }}>
        <Grid
          container
          spacing={4}
          sx={{ marginTop: 0, alignItems: "flex-bettween" }}
        >
          <Grid size={{ xs: 4 }}>
            <Grid container sx={{ direction: "column" }} spacing={4}>
              <Grid
                sx={{
                  width: "inherit",
                  paddingRight: 4,
                  paddingTop: "0px !important",
                }}
              >
                <Paper elevation={6}>
                  <Item
                    key={"Filter Details"}
                    sx={{
                      alignContent: "center",
                    }}
                  >
                    <Typography
                      variant="h6"
                      sx={{
                        fontWeight: "bold",
                        marginBottom: "0px",
                        marginLeft: "1rem",
                        textAlign: "left",
                      }}
                    >
                      {`Filter`}
                    </Typography>
                  </Item>
                  <div className="row mx-1 mt-3">
                    <div className="col-6">
                      <div className="col-12 p-0">
                        <DropdownInputComponent
                          label={"Select Opco"}
                          labelCSS="mb-0 text-left"
                          inputCSS="labelForm voda-bold mb-2"
                          isSearchable={true}
                          isClearable={false}
                          required={false}
                          value={
                            opcoDropdown &&
                            resourceArrayRefactor(opcoDropdown).filter(
                              (x) => x?.key === opcoValue?.key
                            )
                          }
                          options={
                            opcoDropdown && resourceArrayRefactor(opcoDropdown)
                          }
                          onChange={(e: any) => {
                            setOpcoValue(e);
                            setQuery({
                              ...query,
                              opcoName: e !== null ? [e?.value] : [],
                            } as any);
                          }}
                        />
                      </div>
                    </div>
                    <div className="col-6">
                      <div className="col-12 p-0">
                        <DropdownInputComponent
                          label={"VNF Name"}
                          labelCSS="mb-0 text-left"
                          inputCSS="labelForm voda-bold mb-2"
                          isSearchable={true}
                          isClearable={false}
                          required={false}
                          showAllOption={true}
                          value={
                            vnfNameDropdown &&
                            resourceArrayRefactor(vnfNameDropdown).filter(
                              (x) => x?.key === vnfName?.key
                            )
                          }
                          options={
                            vnfNameDropdown &&
                            resourceArrayRefactor(vnfNameDropdown)
                          }
                          onChange={(e: any) => {
                            setVnfName(e);
                            setQuery({
                              ...query,
                              vnfName: e !== null ? [e?.key] : [],
                            } as any);
                          }}
                        />
                      </div>
                    </div>
                    <div className="col-6">
                      <div className="col-12 p-0">
                        <ShowYearInputComponent
                          label={"Financial Year"}
                          labelCSS="mb-0 text-left"
                          value={
                            financialYear && new Date(`${financialYear}/1/1`)
                          }
                          isClearable={false}
                          onChange={(e, newDate) => {
                            e.preventDefault();
                            setFinancialYear(
                              newDate ? `${newDate?.getFullYear()}` : null
                            );
                            setQuery({
                              ...query,
                              financialYear:
                                newDate !== null
                                  ? [`${newDate?.getFullYear()}`]
                                  : [],
                            } as any);
                          }}
                        />
                      </div>
                    </div>
                    <div className="col-6">
                      <div className="col-12 p-0">
                        <DropdownInputComponent
                          label={"Select HW Type"}
                          labelCSS="mb-0 text-left"
                          inputCSS="labelForm voda-bold mb-2"
                          isSearchable={true}
                          isClearable={false}
                          required={false}
                          value={
                            hardwareDropdown &&
                            resourceArrayRefactor(hardwareDropdown).filter(
                              (x) => x?.key === hardwareValue?.key
                            )
                          }
                          options={
                            hardwareDropdown &&
                            resourceArrayRefactor(hardwareDropdown)
                          }
                          onChange={(e: any) => {
                            setHardwareValue(e);
                            setQuery({
                              ...query,
                              hardwareType:
                                e !== null && e?.key !== "all"
                                  ? [e?.value]
                                  : [],
                            } as any);
                          }}
                          showAllOption={true}
                        />
                      </div>
                    </div>
                  </div>
                </Paper>
              </Grid>
              <Grid
                sx={{
                  width: "inherit",
                  paddingRight: 4,
                  paddingTop: "16px !important",
                }}
              >
                <Paper
                  sx={{
                    paddingBottom: "1.5rem",
                    height: "33.3rem !important",
                  }}
                  elevation={6}
                >
                  <Item
                    key={"VNF Level Report for Financial Year 2025"}
                    sx={{
                      alignContent: "center",
                      marginBottom: "0rem !important",
                    }}
                  >
                    <Typography
                      variant="h6"
                      sx={{
                        fontWeight: "bold",
                        marginBottom: "0px",
                        marginLeft: "1rem",
                        textAlign: "left",
                      }}
                    >
                      {`VNF Level Report ${
                        financialYear
                          ? `for Financial Year ${financialYear}`
                          : ""
                      }`}
                    </Typography>
                  </Item>
                  <div
                    className="col-12"
                    style={{ padding: "15px", height: "100%" }}
                  >
                    {data?.length !== 0 ? (
                      <>
                        <VBOMReportGrid
                          data={data}
                          pagination={query}
                          orphanColor={orphanColor}
                          renderGrid={renderGridState?.render ?? []}
                          action={{
                            Filter: setQuery,
                          }}
                        ></VBOMReportGrid>
                      </>
                    ) : (
                      <Typography
                        variant="h6"
                        sx={{
                          marginBottom: "0px",
                          textAlign: "left",
                          paddingLeft: "1rem",
                          height: "100%",
                          justifySelf: "center !important",
                          alignContent: "center",
                          fontWeight: 200,
                          fontFamily:
                            "VodafoneRgBd, Arial, Helvetica, san-serif",
                        }}
                      >
                        Record Not Found
                      </Typography>
                    )}
                  </div>
                </Paper>
              </Grid>
            </Grid>
          </Grid>
          <Grid size={{ xs: 8 }}>
            <Grid container sx={{ direction: "column" }} spacing={4}>
              <Grid
                sx={{
                  width: "inherit",
                  paddingRight: 4,
                  paddingTop: "0rem !important",
                }}
              >
                <Paper elevation={6}>
                  <Item
                    sx={{
                      lineHeight: "24px !important",
                      height: "100% !important",
                      alignContent: "center",
                    }}
                  >
                    <div
                      className="col-12"
                      style={{
                        padding: "15px",
                        height: "47rem",
                        overflowY: "auto",
                        marginTop: "1rem",
                        marginBottom: "1rem",
                        paddingTop: "0.5rem !important",
                      }}
                    >
                      {visualData?.length > 0 ? (
                        visualData?.map((listData, idx) => (
                          <Box
                            sx={{
                              border: "2px solid black",
                              borderColor: "black",
                              backgroundColor: "#ffffff",
                              position: "relative",
                              padding: "24px 14px 14px 14px",
                              borderRadius: 2,
                              paddingTop: "24px",
                              width: "100%",
                              marginBottom: "1.5rem",
                            }}
                          >
                            <Box
                              sx={{
                                position: "absolute",
                                top: -12,
                                left: -2,
                                backgroundColor: "black",
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
                              {listData?.hardwareType ?? "Test"}
                            </Box>
                            <Box sx={{ display: "flex", flexWrap: "wrap" }}>
                              {listData?.list?.map((valRes, subIdx) => {
                                const borderColor = getRandomLightColor();
                                const headerBg = adjustColor(borderColor, -10);
                                const rowBg = adjustColor(borderColor, 35);

                                return valRes?.vbomDisaggregatedViews?.map(
                                  (res, viewIdx) => {
                                    const items = [
                                      {
                                        left: res?.vnfName,
                                        right: res?.vmTypeName,
                                      },
                                      { left: "vCPU", right: res?.vCpu },
                                      { left: "RAM", right: `${res?.ram}GB` },
                                      {
                                        left: "DISK",
                                        right: `${res?.dataDisk}GB`,
                                      },
                                    ];
                                    return (
                                      <Grid
                                        size={{ xs: "auto" }}
                                        key={`card-${idx}-${subIdx}-${viewIdx}`}
                                        sx={{
                                          minWidth: 150,
                                          m: 0.5,
                                          height: "fit-content",
                                          borderRadius: "4px !important",
                                        }}
                                      >
                                        <ItemList
                                          variant="outlined"
                                          sx={{
                                            p: 0,
                                            minWidth: 150,
                                            height: "fit-content",
                                            border: `1.6px solid ${borderColor}`,
                                            borderRadius: "4px !important",
                                          }}
                                        >
                                          <List
                                            sx={{
                                              width: "100%",
                                              maxWidth: 200,
                                              bgcolor: rowBg,
                                              borderRadius: "4px !important",
                                              p: 0,
                                              border: "1px solid lightgray",
                                            }}
                                          >
                                            {items.map((item, itemIdx) =>
                                              itemIdx === 0 ? (
                                                <ListItem
                                                  key={`item-${idx}-${subIdx}-${viewIdx}-${itemIdx}`}
                                                  sx={{
                                                    p: "0px 6px",
                                                    borderBottom:
                                                      itemIdx < items.length - 1
                                                        ? "1px solid rgba(0, 0, 0, 0.2)"
                                                        : "none",
                                                    bgcolor: headerBg,
                                                  }}
                                                >
                                                  <ListItemText
                                                    primary={
                                                      <Box
                                                        sx={{
                                                          display: "flex",
                                                          width: "100%",
                                                          alignItems: "center",
                                                        }}
                                                      >
                                                        <Box
                                                          sx={{
                                                            fontWeight: "bold",
                                                            flex: 1,
                                                            textAlign: "center",
                                                          }}
                                                        >
                                                          {item.left +
                                                            " " +
                                                            item.right}
                                                        </Box>
                                                      </Box>
                                                    }
                                                    sx={{ width: "100%" }}
                                                    slotProps={{
                                                      primary: {
                                                        sx: {
                                                          fontFamily:
                                                            "VodafoneRg, Arial, Helvetica, sans-serif",
                                                          fontWeight: "bold",
                                                        },
                                                      },
                                                    }}
                                                  />
                                                </ListItem>
                                              ) : (
                                                <ListItem
                                                  key={`item-${idx}-${subIdx}-${viewIdx}-${itemIdx}`}
                                                  sx={{
                                                    p: "0px 6px",
                                                    borderBottom:
                                                      itemIdx < items.length - 1
                                                        ? "1px solid rgba(0, 0, 0, 0.2)"
                                                        : "none",
                                                    bgcolor: rowBg,
                                                  }}
                                                >
                                                  <ListItemText
                                                    primary={item.left}
                                                    sx={{ width: "45%" }}
                                                    slotProps={{
                                                      primary: {
                                                        sx: {
                                                          fontFamily:
                                                            "VodafoneRg, Arial, Helvetica, sans-serif",
                                                          fontWeight: 400,
                                                        },
                                                      },
                                                    }}
                                                  />
                                                  <Divider
                                                    orientation="vertical"
                                                    variant="middle"
                                                    flexItem
                                                    sx={{
                                                      mx: 0.5,
                                                    }}
                                                  />
                                                  <ListItemText
                                                    primary={item.right}
                                                    sx={{
                                                      width: "50%",
                                                      textAlign: "right",
                                                    }}
                                                    slotProps={{
                                                      primary: {
                                                        sx: {
                                                          fontFamily:
                                                            "VodafoneRg, Arial, Helvetica, sans-serif",
                                                          fontWeight: 400,
                                                        },
                                                      },
                                                    }}
                                                  />
                                                </ListItem>
                                              )
                                            )}
                                          </List>
                                        </ItemList>
                                      </Grid>
                                    );
                                  }
                                );
                              })}
                            </Box>
                          </Box>
                        ))
                      ) : (
                        <Typography
                          variant="h6"
                          sx={{
                            marginBottom: "0px",
                            textAlign: "left",
                            paddingLeft: "1rem",
                            height: "100%",
                            justifySelf: "center !important",
                            alignContent: "center",
                            fontWeight: 200,
                            fontFamily:
                              "VodafoneRgBd, Arial, Helvetica, san-serif",
                          }}
                        >
                          Record Not Found
                        </Typography>
                      )}
                    </div>
                  </Item>
                </Paper>
              </Grid>
            </Grid>
          </Grid>
        </Grid>
      </Box>
    </div>
  );
};

export default VBOMReportContainer;
