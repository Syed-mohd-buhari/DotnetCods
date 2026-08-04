import React, { useEffect, useState } from "react";
import { Box, Paper, Tooltip, Typography } from "@mui/material";
import { useSelector } from "react-redux";
import { RootState } from "../../Redux/Store/rootStore";
import Logo from "./skins/vf_tv_play_clr_logo_white_cmyk_1.png";
import "../../Containers/landingPage/sidebar.css";
import {
  GetAllPaSWUpgrade,
  GetEosAndEomMileStones,
} from "../../Redux/Action/LandingPage/AbstractionLayerAction";
import AutoScrollDiv from "./AutoScrollDiv";
import "./NewLandingPage.css";
import {
  AL,
  CZ,
  DE,
  GB,
  GR,
  IT,
  PT,
  RO,
  IE,
} from "country-flag-icons/react/3x2";
import { AiOutlineEdit } from "react-icons/ai";
import { DropdownInputComponent } from "../../Components/FormField";
import { IoCloseCircleOutline } from "react-icons/io5";
import { useAuth } from "../../Hook/useAuth";

const countryFlagMap: Record<string, React.ElementType> = {
  AL: AL, // Albania
  CZ: CZ, // Czech Republic
  DE: DE, // Germany
  GR: GR, // Greece
  IT: IT, // Italy
  PT: PT, // Portugal
  RO: RO, // Romania
  IE: IE, // Ireland
  UK: GB, // United Kingdom → mapped to GB
};

interface Props {
  onMonthRangeChange: (messageData?: any) => void;
  triggerFetch: number;
}
const WhatsGoingOn = ({ onMonthRangeChange, triggerFetch }: Props) => {
  const [productList, setProductList] = useState<any>(null);
  const [eomEosList, setEomEosList] = useState<any>(null);
  const [isEditMode, setIsEditMode] = useState(false);
  const [dateRange, setDateRange] = useState<any>(null);
  const userInfo = useSelector(
    (state: RootState) => state.autenticazione.aadResponse
  );

  useEffect(() => {
    getData();
  }, []);

  useEffect(() => {
    if (triggerFetch > 0) {
      getData();
    }
  }, [triggerFetch]);

  const getData = async () => {
    const data1 = await GetAllPaSWUpgrade(userInfo?.account?.userid);
    const data2 = await GetEosAndEomMileStones(userInfo?.account?.userid);
    setProductList(data1?.planndActivitySoftwareUpgradDetailsDtoGrid ?? null);
    const monthRange = data1?.whatsGoingOnDate ?? null;
    setDateRange(
      monthRange
        ? {
            key: monthRange,
            value: `${monthRange} ${monthRange > 1 ? "Months" : "Month"}`,
          }
        : null
    );
    setEomEosList(data2 ?? null);
  };
  const formatDate = (dateStr: string) => {
    if (!dateStr) return "";
    const date = new Date(dateStr);
    const day = String(date.getDate()).padStart(2, "0");
    const month = String(date.getMonth() + 1).padStart(2, "0");
    const year = date.getFullYear();
    return `${day}/${month}/${year}`;
  };

  const stripHtmlTags = (html: string) => {
    if (!html) return "";
    return html
      .replace(/<[^>]*>/g, " ")
      .replace(/\s+/g, " ")
      .trim();
  };
  return (
    <Paper
      elevation={0}
      sx={{
        flex: 1,
        backgroundColor: "#c4c4c4",
        borderRadius: "20px",
        p: 2,
        display: "flex",
        flexDirection: "column",
        height: "21rem",
        minHeight: "21rem",
        maxHeight: "21rem",
        overflow: "hidden",
        gap: 1,
      }}
    >
      <header className="main-header">
        <Box
          sx={{
            display: "flex",
            justifyContent: "space-between",
            alignItems: "center",
            width: "100%",
          }}
        >
          <div className="main-header">
            <div className="main-quick-title pl-0">What's Going On</div>
          </div>
          <Box sx={{ display: "flex", gap: 1, width: "22rem" }}>
            <DropdownInputComponent
              label={""}
              placeholderText="Select Range"
              labelCSS="mb-0 text-left"
              inputCSS="labelForm voda-bold mb-2"
              isSearchable={true}
              isClearable={false}
              value={dateRange}
              options={[
                { key: 1, value: "1 Month" },
                { key: 3, value: "3 Months" },
                { key: 6, value: "6 Months" },
                { key: 9, value: "9 Months" },
                { key: 12, value: "12 Months" },
              ]}
              onChange={(e: any) => {
                setDateRange(e);
                onMonthRangeChange({
                  section: "2",
                  dateRange: e?.key,
                });
                setIsEditMode(false);
              }}
            />
          </Box>
        </Box>
      </header>
      <Box
        sx={{
          display: "flex",
          gap: 2,
          flex: 1,
          overflow: "hidden",
        }}
      >
        <Paper
          elevation={0}
          sx={{
            flex: 1,
            backgroundColor: "white",
            borderRadius: "8px",
            display: "flex",
            flexDirection: "column",
            overflow: "hidden",
          }}
        >
          <Box
            sx={{
              backgroundColor: "#4a4d4e08 !important",
              p: "8px",
              borderBottom: "1px solid rgba(255,255,255,0.2)",
            }}
          >
            <Typography
              sx={{
                color: "black",
                fontWeight: "bold",
                fontSize: "15px",
                textAlign: "center",
              }}
            >
              Product Upgrades
            </Typography>
          </Box>
          <AutoScrollDiv>
            {!productList || productList.length === 0 ? (
              <Box
                sx={{
                  display: "flex",
                  justifyContent: "center",
                  alignItems: "center",
                  height: "100%",
                  fontWeight: "bold",

                  mb: 2,
                  minHeight: "80px",
                  color: "text.primary",
                  fontSize: "1rem",
                }}
              >
                No Records Found
              </Box>
            ) : (
              productList &&
              productList?.map((product: any, index: number) => (
                <Box
                  key={`${index}+${product.oem}-${product.product}`}
                  sx={{
                    mb: 2,
                    borderBottom:
                      index < productList.length - 1
                        ? "1px solid rgba(0, 0, 0, 0.2)"
                        : "none",
                    transition: "background-color 0.2s",
                    "&:hover": {
                      backgroundColor: "#eeeeeec7",
                      p: 1.5,
                      borderRadius: "10px",
                      mb: 1,
                    },
                    paddingBottom: "20px",
                  }}
                >
                  <Typography
                    variant="subtitle1"
                    sx={{
                      color: "text.primary",
                      mb: 2,
                      fontSize: "1rem",
                      textAlign: "left",
                    }}
                  >
                    <b>•</b> Planned upgrade for the Product{" "}
                    <b>
                      {product.oem}-{product.product}
                    </b>{" "}
                    to mitigate the risk of Non Compliance to LCM Policy
                  </Typography>

                  <Box
                    sx={{
                      display: "flex",
                      flexWrap: "wrap",
                      gap: 1.5,
                      marginLeft: "10px",
                    }}
                  >
                    {product.plannedAction?.map(
                      (action: any, verIndex: number) => {
                        const arr = action.actionDetail?.split("|");
                        const trimmedCountry = action?.actionDetail
                          ?.split("|")[0]
                          ?.trim()
                          ?.toUpperCase();
                        const FlagIcon = countryFlagMap[trimmedCountry];
                        return (
                          <Tooltip
                            title={
                              <Box sx={{ p: 1 }}>
                                <Typography variant="body2" sx={{ mb: 1 }}>
                                  <strong>Current DC:</strong>{" "}
                                  {stripHtmlTags(action.currentDcName || "")}
                                </Typography>
                                {action.plannedDcName && (
                                  <Typography variant="body2" sx={{ mb: 1 }}>
                                    <strong>Planned DC:</strong>{" "}
                                    {stripHtmlTags(action.plannedDcName)}
                                  </Typography>
                                )}
                                {action.plannedcompletion && (
                                  <Typography variant="body2">
                                    <strong>Planned Completion:</strong>{" "}
                                    {formatDate(action.plannedcompletion)}
                                  </Typography>
                                )}
                              </Box>
                            }
                            placement="right"
                            arrow
                          >
                            <Paper
                              key={`${index}-${verIndex}`}
                              elevation={1}
                              sx={{
                                p: 1.5,
                                minWidth: 200,
                                flex: "0 0 auto",
                                borderRadius: 1.5,
                                backgroundColor: "primary.100",
                                width: "100% !important",
                                textAlign: "left",
                                borderColor: "primary.200",
                                cursor: "pointer",
                              }}
                              onClick={() => {
                                const newTabUrl = `/plannedActivities/LCM?paId=${action?.paId}&lcmId=${action?.lcmengineeringid}`;
                                window.open(newTabUrl, "_blank");
                              }}
                            >
                              <Typography
                                variant="body2"
                                sx={{ fontWeight: 500, color: "primary.main" }}
                              >
                                <span
                                  key={trimmedCountry}
                                  style={{
                                    display: "inline-flex",
                                    alignItems: "center",
                                    verticalAlign: "middle",
                                    gap: "4px",
                                    marginRight:
                                      index < arr.length - 1 ? "6px" : "0",
                                  }}
                                >
                                  {trimmedCountry === "GROUP" && (
                                    <div
                                      style={{
                                        height: "1.3rem",
                                        width: "1.3rem",
                                        position: "relative",
                                      }}
                                    >
                                      <img
                                        src={Logo}
                                        className="menu_items_image"
                                      />
                                    </div>
                                  )}
                                  {FlagIcon && trimmedCountry !== "GROUP" && (
                                    <span
                                      style={{
                                        display: "inline-flex",
                                        alignItems: "center",
                                        lineHeight: 0,
                                      }}
                                    >
                                      <FlagIcon
                                        title={trimmedCountry}
                                        style={{
                                          height: "1em",
                                          width: "auto",
                                          display: "block",
                                          verticalAlign: "middle",
                                        }}
                                      />
                                    </span>
                                  )}
                                  <span>
                                    {trimmedCountry +
                                      " | " +
                                      arr
                                        .slice(1)
                                        .map((item) => item.trim())
                                        .join(" | ")}
                                  </span>
                                </span>
                              </Typography>
                            </Paper>
                          </Tooltip>
                        );
                      }
                    )}
                  </Box>
                </Box>
              ))
            )}
          </AutoScrollDiv>
        </Paper>

        <Paper
          elevation={0}
          sx={{
            flex: 1,
            backgroundColor: "white",
            borderRadius: "8px",
            display: "flex",
            flexDirection: "column",
            overflow: "hidden",
          }}
        >
          <Box
            sx={{
              backgroundColor: "#4a4d4e08 !important",

              p: "8px",
              borderBottom: "1px solid rgba(255,255,255,0.2)",
            }}
          >
            <Typography
              sx={{
                color: "black",
                fontWeight: "bold",
                fontSize: "15px",
                textAlign: "center",
              }}
            >
              EOM/EOS Milestones
            </Typography>
          </Box>
          <AutoScrollDiv>
            {!eomEosList || eomEosList.length === 0 ? (
              <Box
                sx={{
                  display: "flex",
                  justifyContent: "center",
                  alignItems: "center",
                  height: "100%",
                  mb: 2,
                  fontWeight: "bold",
                  minHeight: "80px",
                  color: "text.primary",
                  fontSize: "1rem",
                }}
              >
                No Records Found
              </Box>
            ) : (
              eomEosList?.map((eomEosItem: any, index: number) => (
                <Box
                  key={index}
                  sx={{
                    mb: 2,
                    borderBottom:
                      index < eomEosList.length - 1
                        ? "1px solid rgba(0, 0, 0, 0.2)"
                        : "none",
                    transition: "background-color 0.2s",
                    "&:hover": {
                      backgroundColor: "#eeeeeec7",
                      p: 1.5,
                      borderRadius: "10px",
                      mb: 1,
                    },
                    paddingBottom: "20px",
                  }}
                >
                  <Typography
                    variant="subtitle1"
                    sx={{
                      color: "text.primary",
                      mb: 2,
                      fontSize: "1rem",
                      textAlign: "left",
                    }}
                  >
                    <b>• {eomEosItem.product}</b> will reach its EOM/EOS
                    milestone soon
                  </Typography>

                  <Box
                    sx={{
                      display: "flex",
                      flexWrap: "wrap",
                      gap: 1.5,
                      marginLeft: "10px",
                    }}
                  >
                    {eomEosItem.versions?.map(
                      (version: any, verIndex: number) => {
                        const opcoList = version?.opCo?.split(",");
                        const trimmedCountry = opcoList?.[0]
                          ?.trim()
                          ?.toUpperCase();
                        const FlagIcon = countryFlagMap[trimmedCountry];
                        return (
                          <Paper
                            key={`${index}-${verIndex}`}
                            elevation={1}
                            sx={{
                              p: 1.5,
                              minWidth: 200,
                              flex: "0 0 auto",
                              borderRadius: 1.5,
                              backgroundColor: "primary.100",
                              width: "100% !important",
                              textAlign: "left",
                              borderColor: "primary.200",
                              cursor: "pointer",
                            }}
                            onClick={() => {
                              const newTabUrl = `/majorsoftware?majorSoftwareBuildId=${version?.majorSwId}`;
                              window.open(newTabUrl, "_blank");
                            }}
                          >
                            <Typography
                              variant="body2"
                              sx={{ fontWeight: 500, color: "primary.main" }}
                            >
                              {`${eomEosItem.product}-${
                                version.version
                              } | EOM: ${version.eomDate ?? "NA"} | EOS: ${
                                version.eosDate ?? "NA"
                              }`}
                            </Typography>
                            <Typography
                              variant="body2"
                              sx={{ fontWeight: 500, color: "primary.main" }}
                            >
                              {opcoList.length === 1 ? (
                                <span
                                  key={trimmedCountry}
                                  style={{
                                    display: "inline-flex",
                                    alignItems: "center",
                                    verticalAlign: "middle",
                                    gap: "4px",
                                    marginRight:
                                      index < opcoList.length - 1 ? "6px" : "0",
                                  }}
                                >
                                  {"Impacted OpCo's:  "}
                                  {trimmedCountry === "GROUP" && (
                                    <div
                                      style={{
                                        height: "1.3rem",
                                        width: "1.3rem",
                                        position: "relative",
                                      }}
                                    >
                                      <img
                                        src={Logo}
                                        className="menu_items_image"
                                      />
                                    </div>
                                  )}
                                  {FlagIcon && trimmedCountry !== "GROUP" && (
                                    <span
                                      style={{
                                        display: "inline-flex",
                                        alignItems: "center",
                                        lineHeight: 0,
                                      }}
                                    >
                                      <FlagIcon
                                        title={trimmedCountry}
                                        style={{
                                          height: "1em",
                                          width: "auto",
                                          display: "block",
                                          verticalAlign: "middle",
                                        }}
                                      />
                                    </span>
                                  )}
                                  <span>{trimmedCountry}</span>
                                </span>
                              ) : (
                                <span>{`Impacted OpCo's: ${version.opCo}`}</span>
                              )}
                            </Typography>
                          </Paper>
                        );
                      }
                    )}
                  </Box>
                </Box>
              ))
            )}
          </AutoScrollDiv>
        </Paper>
      </Box>
    </Paper>
  );
};

export default WhatsGoingOn;
