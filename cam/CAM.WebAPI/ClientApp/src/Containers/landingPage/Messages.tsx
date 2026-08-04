import React, { useState, useEffect } from "react";
import { Box, Paper, Typography, Badge, Tooltip } from "@mui/material";
import { useSelector } from "react-redux";
import { RootState } from "../../Redux/Store/rootStore";
import {
  DoingSectionPADetails,
  DoingSectionForEomAndEos,
  AchievementsDetails,
  SignPostPaDetails,
} from "../../Redux/Action/LandingPage/AbstractionLayerAction";
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
import AutoScrollDiv from "./AutoScrollDiv";
import { AiOutlineEdit } from "react-icons/ai";
import { DropdownInputComponent } from "../../Components/FormField";
import { IoCloseCircleOutline } from "react-icons/io5";
import Logo from "./skins/vf_tv_play_clr_logo_white_cmyk_1.png";

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

interface MessageCard {
  id: number;
  title: string;
  subtitle: string;
  description: string;
  backgroundColor?: string;
  count?: number;
  onClick?: () => void;
}

interface Props {
  onMonthRangeChange: (messageData?: any) => void;
  triggerFetch: number;
}

const Messages = ({ onMonthRangeChange, triggerFetch }: Props) => {
  const userInfo = useSelector(
    (state: RootState) => state.autenticazione.aadResponse
  );

  const [paDetailsList, setPaDetailsList] = useState<any>(null);
  const [eomEosList, setEomEosList] = useState<any>(null);
  const [achievementList, setAchievementList] = useState<any>(null);
  const [signPostPaList, setSignPostPaList] = useState<any>(null);
  const [isEditMode, setIsEditMode] = useState(false);
  const [dateRange, setDateRange] = useState<any>(null);
  const [messageCounts, setMessageCounts] = useState({
    doing: 0,
    achievements: 0,
    signposting: 0,
  });

  useEffect(() => {
    fetchMessageCounts();
  }, []);

  useEffect(() => {
    if (triggerFetch > 0) {
      fetchMessageCounts();
    }
  }, [triggerFetch]);

  const fetchMessageCounts = async () => {
    try {
      const data1 = await DoingSectionPADetails(userInfo?.account?.userid);
      setPaDetailsList(data1?.doingSectioinForPA ?? null);
      const monthRange = data1?.messagingDate ?? null;
      setDateRange(
        monthRange
          ? {
              key: monthRange,
              value: `${monthRange} ${monthRange > 1 ? "Months" : "Month"}`,
            }
          : null
      );
      const data2 = await DoingSectionForEomAndEos(userInfo?.account?.userid);
      setEomEosList(data2);
      const data3 = await AchievementsDetails(userInfo?.account?.userid);
      setAchievementList(data3);
      const data4 = await SignPostPaDetails(userInfo?.account?.userid);
      setSignPostPaList(data4);
    } catch (error) {
      console.error("Error fetching message counts:", error);
    }
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
        height: "20.5rem",
        minHeight: "20.5rem",
        maxHeight: "20.5rem",
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
          <div className="main-header" style={{ color: "#e60000" }}>
            <div className="main-quick-title pl-0"> Messages</div>
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
                { key: 2, value: "2 Months" },
                { key: 3, value: "3 Months" },
              ]}
              onChange={(e: any) => {
                setDateRange(e);
                onMonthRangeChange({
                  section: "1",
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
        {((paDetailsList !== null && paDetailsList?.length !== 0) ||
          (eomEosList !== null && eomEosList?.length !== 0)) && (
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
                {"TODO"}
              </Typography>
            </Box>
            <AutoScrollDiv>
              {paDetailsList?.map((product: any, index: number) => (
                <Box
                  key={index}
                  sx={{
                    mb: 2,
                    borderBottom:
                      index < paDetailsList?.length - 1
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
                    <b>•</b> Past planned completion date for the Product{" "}
                    <b>
                      {product.oem}-{product.product}
                    </b>
                    {"– "}
                    <b>update required</b>
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
                            key={`${index}-${verIndex}`}
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
              ))}
              {eomEosList?.map((eomEosItem: any, index: number) => (
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
                    <b>•</b> EOS/EOM for the product{" "}
                    <b> {eomEosItem.product}</b> is in the past,{" "}
                    <b>action required</b>
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
              ))}
            </AutoScrollDiv>
          </Paper>
        )}
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
              {"ACHIEVEMENTS"}
            </Typography>
          </Box>
          <AutoScrollDiv>
            {!achievementList || achievementList.length === 0 ? (
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
              achievementList?.map((product: any, index: number) => (
                <Box
                  key={index}
                  sx={{
                    mb: 2,
                    borderBottom:
                      index < achievementList.length - 1
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
                    <b>•</b> Rollout Complete / FSI Achieved for the Product{" "}
                    <b>
                      {product.oem}-{product.product}
                    </b>{" "}
                    are achieved.
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
                            }}
                          >
                            <Typography
                              variant="body2"
                              sx={{ fontWeight: 500, color: "#000 !important" }}
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
                                      lineHeight: 0, // Removes extra spacing around inline-flex
                                    }}
                                  >
                                    <FlagIcon
                                      title={trimmedCountry}
                                      style={{
                                        height: "1em", // Matches text height exactly
                                        width: "auto", // Maintains natural 3x2 aspect ratio
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
              {"SIGNPOSTING"}
            </Typography>
          </Box>
          <AutoScrollDiv>
            {!signPostPaList || signPostPaList.length === 0 ? (
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
              signPostPaList?.map((product: any, index: number) => (
                <Box
                  key={index}
                  sx={{
                    mb: 2,
                    borderBottom:
                      index < signPostPaList.length - 1
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
                    <b>•</b> Upcoming planned completion date for the Product{" "}
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
                            key={`${index}-${verIndex}`}
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
      </Box>
    </Paper>
  );
};

export default Messages;
