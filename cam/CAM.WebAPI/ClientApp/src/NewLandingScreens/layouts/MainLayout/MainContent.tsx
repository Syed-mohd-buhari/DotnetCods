import React, { useEffect, useRef, useState } from "react";
import { Box, Typography } from "@mui/material";
import Skeleton from "@mui/material/Skeleton";
import { styled } from "@mui/material/styles";
import {
  FiBookOpen,
  FiServer,
  FiAlertCircle,
  FiMap,
  FiGrid,
  FiChevronDown,
  FiChevronRight,
} from "react-icons/fi";
import * as echarts from "echarts";
import OpenActionsModal from "./OpenActionsModal";
import { useAuth } from "../../../Hook/useAuth";
import {
  GetAchivementAndSignPostRecord,
  GetComplainceGraphData,
  GetExpiredEomEosActions,
  GetUpcomingEomEosActions,
  GetOpenLinkRecordCount,
  GetUpcomingLinkRecordCount,
} from "../../../Redux/Action/HomePage/HomePageAction";
import { RootState } from "../../../Redux/Store/rootStore";
import ClockIcon from "../../../img/new_uim_clock.png";
import { useSelector } from "react-redux";
import PushIcon from "../../../img/PushIcon";
import WarningTriangleFlat from "../../../img/WarningTriangleFlat";
import UpcomingActionsModal1 from "./UpcomingActionsModal1";
import AchivementModal from "./AchivementModal";
import { SaveCustomGridRender } from "../../../Redux/Action/Grid/SaveGridCustom";
import { QueryObject } from "../../../Model/Common";

const MainWrapper = styled(Box)({
  height: "calc(100vh - 92px)",
  overflowY: "auto",
  flex: 1,
  display: "flex",
  flexDirection: "column",
  gap: "24px",
  padding: "0 24px",
  "&::-webkit-scrollbar": { width: "6px" },
  "&::-webkit-scrollbar-track": { background: "transparent" },
  "&::-webkit-scrollbar-thumb": {
    background: "rgba(0,0,0,0.18)",
    borderRadius: "4px",
  },
});

const HeroSection = styled(Box)({
  display: "flex",
  flexDirection: "column",
  justifyContent: "center",
  alignItems: "center",
  padding: "24px 24px",
  gap: "24px",
  width: "100%",
  background: "#F0F0F3",
  borderRadius: "0 0 10px 10px",
  flexShrink: 0,
});

const AlertCard = styled(Box)(() => ({
  display: "flex",
  flexDirection: "row",
  alignItems: "center",
  padding: "8px 16px",
  gap: "16px",
  background: "#FFFFFF",
  boxShadow:
    "0px 0px 4.1px rgba(12, 26, 75, 0.01), 0px 2.09732px 9.6px -0.699108px rgba(50, 50, 71, 0.05)",
  borderRadius: "8px",
  cursor: "pointer",
  flex: 1,
  minWidth: 0,
  justifyContent: "space-between",
  height: "52px",
  position: "relative",
  overflow: "hidden",

  "&::before": {
    content: '""',
    position: "absolute",
    top: 0,
    left: 0,
    width: "0%",
    height: "100%",
    background: "#E60000",
    transition: "width 0.2s ease",
    zIndex: 0,
  },

  "&:hover::before": {
    width: "100%",
  },

  "& > *": {
    position: "relative",
    zIndex: 1,
  },

  "&:hover .alert-label": {
    color: "#FFFFFF",
  },

  "&:hover .alert-count": {
    color: "#FFFFFF",
  },

  "&:hover .alert-icon": {
    color: "#FFFFFF",
  },
}));
const BadgeCount = styled(Box)<{ isActive?: boolean }>(({ isActive }) => ({
  display: "flex",
  justifyContent: "center",
  alignItems: "center",
  padding: "3.5px 6.5px",
  background: isActive ? "rgba(255,255,255,0.25)" : "rgba(230, 0, 0, 0.1)",
  borderRadius: "10px",
  minWidth: "24px",
  height: "24px",
  flexShrink: 0,
}));

const SectionDivider = styled(Box)({
  width: "100%",
  height: "1px",
  background: "rgba(0, 0, 0, 0.1)",
});

const CustomIcon = styled("img")({
  width: "28px",
  height: "28px",
  objectFit: "contain",
  flexShrink: 0,
});

const quickLinks = [
  { icon: FiBookOpen, label: "Open Software Product", page: "softwareProduct" },
  // { icon: FiServer, label: "Manage software updates" },
  // { icon: FiAlertCircle, label: "Set / update vulnerability" },
  // { icon: FiMap, label: "View role guide" },
  // { icon: FiGrid, label: "Open design library" },
];

type LoadingStates = {
  achievement: boolean;
  openActions: boolean;
  upcomingActions: boolean;
  compliance: boolean;
};

const ACHIEVEMENTS_DEFAULT_PAGE_SIZE = 4;
const ACHIEVEMENTS_VIEW_ALL_PAGE_SIZE = 0;

const ROLE_ID_MAP: Record<string, number> = {
  "sw product owner": 1,
  "hw product owner": 2,
};

const getRoleId = (role?: string | null) =>
  ROLE_ID_MAP[role?.toLowerCase() ?? ""] ?? 0;

const ComplianceGauge = ({ percentage }) => {
  const ref = useRef<HTMLDivElement>(null);
  const convertNumber = (value: any): number => {
    if (value === null || value === undefined) return 0;

    const num = Number(String(value).replace("%", "").replace(",", ".").trim());

    return Number.isFinite(num) ? num : 0;
  };
  useEffect(() => {
    if (!ref.current) return;
    const chart = echarts.init(ref.current);
    chart.setOption({
      series: [
        {
          type: "gauge",
          startAngle: 180,
          endAngle: 0,
          min: 0,
          max: 100,
          radius: "100%",
          center: ["50%", "80%"],
          pointer: { show: false },
          progress: {
            show: true,
            overlap: false,
            roundCap: false,
            clip: false,
            itemStyle: {
              color: {
                type: "linear",
                x: 0,
                y: 0,
                x2: 1,
                y2: 0,
                colorStops: [
                  { offset: 0, color: "#800000" },
                  { offset: 1, color: "#E60000" },
                ],
              },
            },
          },
          axisLine: {
            lineStyle: {
              width: 20,
              color: [[1, "rgba(230,0,0,0.15)"]],
            },
          },
          splitLine: { show: false },
          axisTick: { show: false },
          axisLabel: { show: false },
          data: [{ value: convertNumber(percentage), name: "" }],
          detail: {
            show: true,
            offsetCenter: [0, "-5%"],
            formatter: (value: number) => `${value}%`,
            fontSize: 22,
            fontWeight: 700,

            color: "#27272E",
          },
          title: { show: false },
        },
      ],
    });
    return () => chart.dispose();
  }, []);

  return <div ref={ref} style={{ width: "100%", height: "110px" }} />;
};

const ComplianceChart = ({ complianceList }) => {
  const chartRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (!chartRef.current) return;

    const chart = echarts.init(chartRef.current);

    const categories = complianceList?.map((d) => d.name).reverse();
    const compliantVals = complianceList?.map((d) => d.compliant).reverse();
    const nonCompliantVals = complianceList
      ?.map((d) => d.nonCompliant)
      ?.reverse();
    const totalCounts = complianceList?.map((d) => d.totalCount);

    const maxValue = Math.max(...totalCounts);
    const maxAxisValue = Math.ceil(maxValue);

    chart.on("mouseover", (params) => {
      if (params.componentType === "yAxis") {
        chart.dispatchAction({
          type: "showTip",
          seriesIndex: 0,
          dataIndex: params.value,
        });
      }
    });

    chart.setOption({
      grid: {
        top: 8,
        bottom: 80,
        left: 80,
        right: 140,
      },

      legend: {
        bottom: 25,
        itemWidth: 8,
        itemHeight: 8,
        itemGap: 20,
        textStyle: {
          fontSize: 11,
        },
        data: ["Non-compliant", "Compliant"],
      },

      tooltip: {
        trigger: "axis",
        axisPointer: {
          type: "shadow",
        },
        backgroundColor: "#fff",
        borderColor: "#E1E4EA",
        borderWidth: 1,
        textAlign: "left",
      },

      xAxis: {
        type: "value",
        min: 0,
        max: Math.ceil(maxValue),

        interval: 1,
        minInterval: 1,
        splitNumber: maxAxisValue,
        axisLine: {
          show: false,
        },
        axisTick: {
          show: false,
        },
        axisLabel: {
          fontSize: 12,
          color: "#000",
          interval: 0,
          formatter: (value: number) => Math.round(value),
        },
        splitLine: {
          lineStyle: {
            color: "rgba(0,0,0,0.3)",
            width: 0.5,
          },
        },
      },

      yAxis: [
        {
          type: "category",
          inverse: true,
          data: categories,
          triggerEvent: true,
          axisLine: {
            show: false,
          },
          axisTick: {
            show: false,
          },
          axisLabel: {
            fontSize: 13,
            color: "#000",
            width: 80,
            overflow: "truncate",
            formatter: (value: string) => value,
            ellipsis: "...",
          },
        },
        {
          type: "category",
          inverse: true,
          position: "right",
          data: nonCompliantVals.map((v) => `${v} non-compliant`),
          axisLine: {
            show: false,
          },
          axisTick: {
            show: false,
          },
          axisLabel: {
            color: "#AE0101",
            fontSize: 11,
            backgroundColor: "#FFEAEA",
            borderRadius: 20,
            padding: [4, 10],
          },
        },
      ],

      series: [
        {
          name: "Compliant",
          type: "bar",
          data: compliantVals,
          barWidth: 10,
          silent: true,
          z: 1,
          itemStyle: {
            color: "#C9CCC9",
            borderRadius: 51.42,
          },
        },

        {
          name: "Non-compliant",
          type: "bar",
          data: nonCompliantVals,
          barGap: "-100%",
          barWidth: 10,
          z: 2,

          itemStyle: {
            color: new echarts.graphic.LinearGradient(1, 0, 0, 0, [
              {
                offset: 0,
                color: "#800000",
              },
              {
                offset: 1,
                color: "#E60000",
              },
            ]),
            borderRadius: 51.42,
          },

          emphasis: {
            itemStyle: {
              color: "#E60000",
              borderRadius: 51.42,
            },
          },
        },
      ],

      dataZoom: [
        {
          type: "slider",
          xAxisIndex: 0,
          bottom: 0,
          height: 12,
          start: 0,
          end: 20,
          showDetail: false,
          showDataShadow: false,
          brushSelect: false,
          fillerColor: "#D9D9D9",
          backgroundColor: "#F5F5F5",
          borderColor: "#E6E6E6",
          handleSize: 14,
          handleStyle: {
            color: "#C4C4C4",
            borderColor: "#B0B0B0",
          },
          moveHandleSize: 0,
        },

        {
          type: "inside",
          xAxisIndex: 0,
          start: 0,
          end: 100,
        },

        {
          type: "slider",
          yAxisIndex: [0, 1],
          right: 0,
          height: "80%",
          width: 12,

          start: 0,
          end:
            complianceList.length > 8 ? (8 / complianceList.length) * 100 : 100,

          showDetail: false,
          showDataShadow: false,
          brushSelect: false,

          fillerColor: "#D9D9D9",
          backgroundColor: "#F5F5F5",
          borderColor: "#E6E6E6",

          handleSize: 14,

          handleStyle: {
            color: "#C4C4C4",
            borderColor: "#B0B0B0",
          },

          moveHandleSize: 0,
        },

        {
          type: "inside",
          yAxisIndex: [0, 1],
          start: 0,
          end:
            complianceList.length > 8 ? (8 / complianceList.length) * 100 : 100,
          filterMode: "empty",
        },
      ],
    });

    const handleResize = () => chart.resize();

    window.addEventListener("resize", handleResize);

    return () => {
      window.removeEventListener("resize", handleResize);
      chart.dispose();
    };
  }, [complianceList]);

  return <div ref={chartRef} style={{ width: "100%", height: "400px" }} />;
};

export interface achievementDto extends QueryObject {
  userId: number;
  portalRoleId: number;
}

const achievementPagination = {
  userId: null,
  portalRoleId: null,
  pageSize: 4,
  page: 0,
};
const MainContent = (props: any) => {
  const [openActionsOpen, setOpenActionsOpen] = useState(false);
  const [monthSelected, setMonthSelected] = useState(1);
  const [achievementOpen, setAchievementOpen] = useState(false);
  const [upcomingActionsOpen, setUpcomingActionsOpen] = useState(false);
  const [achievementList, setAchievementList] = useState<any>(null);
  const [openActionList, setOpenActionList] = useState<any>(null);
  const [upcomingActionList, setUpcomingActionList] = useState<any>(null);
  const [complianceList, setComplianceList] = useState<any>(null);
  const [errorMessage, setErrorMessage] = useState("");
  const [openActionCount, setOpenActionCount] = useState<number>(0);
  const [upcomingActionCount, setUpcomingActionCount] = useState<number>(0);
  const openActionListLoaded = useRef(false);
  const upcomingActionListLoaded = useRef(false);

  const [overallComplianceDetail, setOverallComplianceDetail] =
    useState<any>(null);
  const [loadingStates, setLoadingStates] = useState<LoadingStates>({
    achievement: true,
    openActions: true,
    upcomingActions: true,
    compliance: true,
  });

  const setLoading = (key: keyof LoadingStates, value: boolean) => {
    setLoadingStates((prev) => ({ ...prev, [key]: value }));
  };

  const { isPermesso } = useAuth();
  const activeRole = props?.role;
  const activePage = props?.activePage;
  const userInfo = useSelector(
    (state: RootState) => state.autenticazione.aadResponse
  );
  const userId = userInfo?.account?.userid;

  const fetchedForRole = useRef<string | null>(null);

  const fetchAchievements = (pageSize: number, roleForCall?: string) => {
    const roleId = getRoleId(roleForCall ?? activeRole);
    setLoading("achievement", true);
    GetAchivementAndSignPostRecord({
      ...achievementPagination,
      userId,
      portalRoleId: roleId,
      pageSize,
    })
      .then(({ data }) => {
        setMonthSelected(data?.messagingDate);

        const achievementItems = data?.achivementRecords?.items ?? [];

        const achievements = achievementItems.flatMap((item) =>
          (item.plannedAction ?? []).map((action) => {
            const [country, date, , type] = (action.actionDetail ?? "")
              .split("|")
              .map((part) => part.trim());

            return {
              paId: action.paId,
              lcmEngineeringId: action.lcmengineeringid,
              type,
              date,
              product: `${item.oem} ${item.product}`,
              description: `Completed for ${country}`,
              currentDcName: action.currentDcName,
              plannedDcName: action.plannedDcName,
              plannedCompletion: action.plannedcompletion,
            };
          })
        );

        setAchievementList(achievements);
      })
      .catch((err) => {
        console.error("Achievements ApiCall error", err);
        fetchedForRole.current = null;
      })
      .finally(() => setLoading("achievement", false));
  };
  const fetchOpenActionsCount = (roleForCall?: string) => {
    const roleId = getRoleId(roleForCall ?? activeRole);
    setLoading("openActions", true);
    GetOpenLinkRecordCount(userId, roleId)
      .then((res) => {
        setOpenActionCount(res?.data?.openActionsResource ?? 0);
      })
      .catch((err) => {
        console.error("Open actions count ApiCall error", err);
      })
      .finally(() => setLoading("openActions", false));
  };
  const fetchUpcomingActionsCount = (roleForCall?: string) => {
    const roleId = getRoleId(roleForCall ?? activeRole);
    setLoading("upcomingActions", true);
    GetUpcomingLinkRecordCount(userId, roleId)
      .then((res) => {
        setUpcomingActionCount(res?.data?.upcomingActionsResource ?? 0);
      })
      .catch((err) => {
        console.error("Upcoming actions count ApiCall error", err);
      })
      .finally(() => setLoading("upcomingActions", false));
  };
  const fetchOpenActionsList = (roleForCall?: string, force = false) => {
    if (openActionListLoaded.current && !force) return;
    const roleId = getRoleId(roleForCall ?? activeRole);
    setLoading("openActions", true);
    GetExpiredEomEosActions(userId, roleId)
      .then((openActionData) => {
        setOpenActionList(
          openActionData?.data?.openActionsResource?.flatMap((item) =>
            item.versions.map((version) => ({
              id: version.majorId,
              label: "EOS/EOM for the product ",
              productName: item.productAndPlatfrom,
              description: " is in the past, action required",
              eom: version.eomDate || "NA",
              eos: version.eosDate || "NA",
              country: version.opCo,
              majorId: version.majorId,
            }))
          )
        );
        openActionListLoaded.current = true;
      })
      .catch((err) => {
        console.error("Open actions ApiCall error", err);
        fetchedForRole.current = null;
      })
      .finally(() => setLoading("openActions", false));
  };
  const fetchUpcomingActionsList = (roleForCall?: string, force = false) => {
    if (upcomingActionListLoaded.current && !force) return;
    const roleId = getRoleId(roleForCall ?? activeRole);
    setLoading("upcomingActions", true);
    GetUpcomingEomEosActions(userId, roleId)
      .then((upcomingActionData) => {
        setUpcomingActionList(
          upcomingActionData?.data?.upcomingActionsResource?.flatMap((item) =>
            item.versions.map((version) => ({
              id: version.majorId,
              productName: item.productAndPlatfrom,
              productUnderlined: true,
              description: " will reach its EOM/EOS milestone soon",
              eom: version.eomDate || "NA",
              eos: version.eosDate || "NA",
              country: version.opCo,
              majorId: version.majorId,
            }))
          )
        );
        upcomingActionListLoaded.current = true;
      })
      .catch((err) => {
        console.error("Upcoming actions ApiCall error", err);
        fetchedForRole.current = null;
      })
      .finally(() => setLoading("upcomingActions", false));
  };
  const fetchCompliance = (roleForCall?: string) => {
    const roleId = getRoleId(roleForCall ?? activeRole);
    setLoading("compliance", true);
    GetComplainceGraphData(userId, roleId)
      .then((complianceData) => {
        const { products, ...res } =
          complianceData?.data?.productWiseCompliance || {};

        setComplianceList(
          products?.map((item) => ({
            name: item?.productName,
            compliant: item?.compliantProductCount,
            nonCompliant: item?.nonCompliantProductCount,
            totalCount: item?.totalProductCount,
          })) || []
        );

        setOverallComplianceDetail(res);
      })
      .catch((err) => {
        console.error("Compliance ApiCall error", err);
        fetchedForRole.current = null;
      })
      .finally(() => setLoading("compliance", false));
  };

  useEffect(() => {
    if (!isPermesso || !userId || !activeRole) return;
    if (fetchedForRole.current === activeRole) return;
    fetchedForRole.current = activeRole;

    ApiCall(undefined, activeRole);
  }, [isPermesso, userId, activeRole]);
  const ApiCall = (Apicallfrom?: string, roleForCall?: string) => {
    const currentRole = roleForCall ?? activeRole;

    fetchAchievements(
      achievementOpen
        ? ACHIEVEMENTS_VIEW_ALL_PAGE_SIZE
        : ACHIEVEMENTS_DEFAULT_PAGE_SIZE,
      currentRole
    );

    fetchOpenActionsCount(currentRole);
    fetchUpcomingActionsCount(currentRole);

    if (openActionListLoaded.current) {
      fetchOpenActionsList(currentRole, true);
    }
    if (upcomingActionListLoaded.current) {
      fetchUpcomingActionsList(currentRole, true);
    }
    if (Apicallfrom !== "modal") {
      fetchCompliance(currentRole);
    }
  };

  const alertCards = [
    {
      icon: <WarningTriangleFlat />,
      label: "Open actions",
      count: openActionCount,
      loading: loadingStates.openActions,
    },
    {
      icon: <img src={ClockIcon} alt="" />,
      label: "Upcoming actions",
      count: upcomingActionCount,
      loading: loadingStates.upcomingActions,
    },
    {
      icon: (
        <PushIcon
          className="alert-icon"
          sx={{
            color: "#E60000",
            ".alert-card:hover &": {
              color: "#FFFFFF",
            },
          }}
        />
      ),
      label: "Nudges",
      count: 0,
      loading: false,
    },
  ];
  const handleSelectedMonth = async (month) => {
    try {
      const response: any = await SaveCustomGridRender({
        messagingDate: month?.value,
      } as any);

      if (response?.ResultDtoCreate?.warning === false) {
        ApiCall("modal", activeRole);
      } else {
        setErrorMessage("Failed to save changes. Please try again.");
        setTimeout(() => setErrorMessage(""), 5000);
      }
    } catch (error) {
      setErrorMessage("Error saving changes. Please try again.");
      setTimeout(() => setErrorMessage(""), 5000);
    }
  };
  return (
    <>
      <OpenActionsModal
        open={openActionsOpen}
        openActionList={openActionList}
        onClose={() => setOpenActionsOpen(false)}
        monthSelected={monthSelected}
        handleSelectedMonth={handleSelectedMonth}
        handleRedirect={(id) => {
          props?.handleSoftwareRedirect && props?.handleSoftwareRedirect(id);
          setOpenActionsOpen(false);
        }}
      />
      <UpcomingActionsModal1
        open={upcomingActionsOpen}
        upcomingActionList={upcomingActionList}
        onClose={() => setUpcomingActionsOpen(false)}
        monthSelected={monthSelected}
        handleSelectedMonth={handleSelectedMonth}
        handleRedirect={(id) => {
          props?.handleHardwareRedirect && props?.handleHardwareRedirect(id);
          setUpcomingActionsOpen(false);
        }}
      />
      <AchivementModal
        open={achievementOpen}
        achievementList={achievementList}
        onClose={() => setAchievementOpen(false)}
        monthSelected={monthSelected}
        handleSelectedMonth={handleSelectedMonth}
      />
      {/* <UpcomingActionsModal
        open={upcomingActionsOpen}
        onClose={() => setUpcomingActionsOpen(false)}
      /> */}
      <MainWrapper>
        <HeroSection>
          <Typography
            sx={{
              fontWeight: 400,
              fontSize: "40px",
              lineHeight: "48px",
              color: "#000000",
              textAlign: "center",
            }}
          >
            Here's what is happening today,{" "}
            {userInfo?.account.name
              .split("@")[0]
              .trim()
              .replace(/\./g, " ")
              .replace(/(^\w|\.\s*\w)/g, function (char) {
                return char.toUpperCase();
              })}
          </Typography>
          <Box
            sx={{
              display: "flex",
              flexDirection: "row",
              gap: "16px",
              width: "100%",
            }}
          >
            {alertCards.map((alert, idx) => {
              const isDisabled = alert.label === "Nudges";
              const isLoading = alert.loading;
              const isActiveByDefault = idx === 0 && !isDisabled && !isLoading;

              const handleClick = () => {
                if (isDisabled || isLoading) return;

                if (idx === 0) {
                  if (openActionCount > 0) {
                    setOpenActionsOpen(true);
                    fetchOpenActionsList();
                  }
                } else if (idx === 1) {
                  if (upcomingActionCount > 0) {
                    setUpcomingActionsOpen(true);
                    fetchUpcomingActionsList();
                  }
                }
              };

              return (
                <AlertCard
                  key={idx}
                  onClick={handleClick}
                  sx={{
                    cursor: isDisabled || isLoading ? "not-allowed" : "pointer",
                    color: "#E60000",
                    "&::before": {
                      width: isActiveByDefault ? "100%" : "0%",
                    },

                    "& .alert-label": {
                      color: isActiveByDefault ? "#FFFFFF" : "#0D0D0D",
                    },

                    "& .alert-count": {
                      color: isActiveByDefault ? "#FFFFFF" : "#E60000",
                    },

                    "& .alert-icon": {
                      color: isActiveByDefault ? "#FFFFFF" : "inherit",
                    },

                    "&:hover::before": {
                      width: isDisabled || isLoading ? "0%" : "100%",
                    },

                    "&:hover .alert-label": {
                      color: isDisabled || isLoading ? "#0D0D0D" : "#FFFFFF",
                    },

                    "&:hover .alert-count": {
                      color: isDisabled || isLoading ? "#0D0D0D" : "#FFFFFF",
                    },
                    "&:hover .alert-icon": {
                      color: isDisabled || isLoading ? "inherit" : "#FFFFFF",
                    },
                    opacity: isDisabled ? 0.6 : 1,
                  }}
                >
                  <Box
                    sx={{
                      display: "flex",
                      alignItems: "center",
                      gap: "16px",
                      flex: 1,
                      minWidth: 0,
                    }}
                  >
                    <Box
                      className="alert-icon"
                      sx={{
                        width: "36px",
                        height: "36px",
                        display: "flex",
                        alignItems: "center",
                        justifyContent: "center",
                        borderRadius: "50%",
                        flexShrink: 0,
                      }}
                    >
                      {isLoading ? (
                        <Skeleton
                          animation="wave"
                          variant="circular"
                          width={28}
                          height={28}
                        />
                      ) : (
                        alert.icon
                      )}
                    </Box>

                    {isLoading ? (
                      <Skeleton
                        animation="wave"
                        variant="text"
                        width="70%"
                        height={28}
                      />
                    ) : (
                      <Typography
                        className="alert-label"
                        sx={{
                          fontWeight: 400,
                          fontSize: "16px",
                          lineHeight: "28px",
                          color: "#0D0D0D",
                          transition: "color 0.3s ease",
                          whiteSpace: "nowrap",
                          minWidth: 0,
                          overflow: "hidden",
                          textOverflow: "ellipsis",
                        }}
                      >
                        {alert.label}
                      </Typography>
                    )}
                  </Box>

                  <BadgeCount>
                    {isLoading ? (
                      <Skeleton
                        animation="wave"
                        variant="text"
                        width={16}
                        height={16}
                      />
                    ) : (
                      <Typography
                        className="alert-count"
                        sx={{
                          fontWeight: 700,
                          fontSize: "12px",
                          color: "#E60000",
                          transition: "color 0.3s ease",
                        }}
                      >
                        {alert.count}
                      </Typography>
                    )}
                  </BadgeCount>
                </AlertCard>
              );
            })}
          </Box>
        </HeroSection>
        <Box
          sx={{
            display: "flex",
            flexDirection: "column",
            gap: "16px",
            background: "#FFFFFF",
            borderRadius: "10px",
            boxShadow:
              "0px 0px 0.83px rgba(12,26,75,0.24), 0px 2.49px 6.64px -0.83px rgba(50,50,71,0.05)",
          }}
        >
          <Box
            sx={{
              display: "flex",
              justifyContent: "space-between",
              alignItems: "center",
              padding: "24px 24px 0",
            }}
          >
            <Typography
              sx={{
                fontWeight: 700,
                fontSize: "18px",
                lineHeight: "28px",
                color: "#0D0D0D",
              }}
            >
              Quick links
            </Typography>
            {/* <Box
              sx={{
                display: "flex",
                alignItems: "center",
                gap: "6px",
                cursor: "pointer",
                padding: "8px 16px",
                background: "#F5F7FA",
                borderRadius: "10px",
              }}
            >
              <Typography
                sx={{
                  fontSize: "16px",
                  lineHeight: "28px",
                  color: "rgba(0,0,0,0.9)",
                }}
              >
                More links
              </Typography>
              <FiChevronDown
                style={{ fontSize: "14px", color: "rgba(0,0,0,0.6)" }}
              />
            </Box> */}
          </Box>

          <SectionDivider />

          <Box
            sx={{
              display: "flex",
              flexDirection: "row",
              alignItems: "center",
              gap: "48px",
              flexWrap: "wrap",
              padding: "0 24px 24px",
            }}
          >
            {quickLinks.map((link, idx) => {
              const Icon = link.icon;
              const isActive = link.page && activePage === link.page;
              return (
                <Box
                  key={idx}
                  onClick={() => {
                    if (link.page) {
                      props?.onNavigate && props.onNavigate(link.page);
                    }
                  }}
                  sx={{
                    display: "flex",
                    alignItems: "center",
                    gap: "8px",
                    cursor: link.page ? "pointer" : "default",
                    padding: "4px 0",
                    "&:hover": { opacity: link.page ? 0.7 : 1 },
                  }}
                >
                  <Icon style={{ fontSize: "20px", color: "#0D0D0D" }} />
                  <Typography
                    sx={{
                      fontSize: "16px",
                      lineHeight: "28px",
                      color: "#0D0D0D",
                      borderBottom: isActive ? "1.4px solid #0D0D0D" : "none",
                    }}
                  >
                    {link.label}
                  </Typography>
                </Box>
              );
            })}
          </Box>
        </Box>
        <Box
          sx={{
            display: "flex",
            flexDirection: { xs: "column", md: "column", lg: "row" },
            gap: "22px",
            alignItems: "flex-start",
            paddingBottom: "10px",
            width: "100%",
          }}
        >
          {" "}
          <Box
            sx={{
              display: "flex",
              flexDirection: "column",
              gap: "16px",
              background: "#FFFFFF",
              boxShadow:
                "0px 0px 0.83px rgba(12,26,75,0.24), 0px 2.49px 6.64px -0.83px rgba(50,50,71,0.05)",
              borderRadius: "12px",
              flex: 1,
              minWidth: 0,
              width: "100%",
            }}
          >
            <Box
              sx={{
                borderBottom: "1px solid rgba(0,0,0,0.2)",
                textAlign: "left",
                padding: "20px 20px 8px",
              }}
            >
              <Typography
                sx={{
                  fontWeight: 700,
                  fontSize: "16px",
                  lineHeight: "18px",
                  color: "#000000",
                }}
              >
                Compliance by products
              </Typography>
              <Typography
                sx={{
                  fontSize: "14px",
                  lineHeight: "28px",
                  color: "#757575",
                  marginTop: "2px",
                }}
              >
                {activeRole === "SW Product Owner" ? "Software" : "Hardware"}{" "}
                compliance ownership across products
              </Typography>
            </Box>
            <Box
              sx={{
                padding: "0px 20px 20px",
              }}
            >
              {loadingStates.compliance ? (
                <Skeleton
                  animation="wave"
                  variant="rectangular"
                  sx={{ width: "100%", height: "400px", borderRadius: "8px" }}
                />
              ) : (
                <ComplianceChart complianceList={complianceList ?? []} />
              )}
            </Box>
          </Box>
          <Box
            sx={{
              display: "flex",
              flexDirection: {
                xs: "column",
                sm: "column",
                md: "row",
                lg: "column",
              },
              gap: "22px",
              width: { xs: "100%", md: "100%", lg: "345px" },
              flexShrink: { lg: 0 },
              alignItems: "flex-start",
            }}
          >
            <Box
              sx={{
                display: "flex",
                flexDirection: "row",
                alignItems: "center",
                padding: "16.78px",
                gap: "8px",
                background: "#FFFFFF",
                boxShadow:
                  "0px 0px 0.699px rgba(12,26,75,0.24), 0px 2.097px 5.593px -0.699px rgba(50,50,71,0.05)",
                borderRadius: "11.19px",
                flex: {
                  xs: "1 1 100%",
                  sm: "1 1 100%",
                  md: "1 1 calc(50% - 11px)",
                  lg: "1 1 auto",
                },
                width: { xs: "100%", sm: "100%", md: "auto", lg: "100%" },
                minWidth: 0,
              }}
            >
              <Box
                sx={{
                  display: "flex",
                  flexDirection: "column",
                  padding: "8.39px 8.39px 8.39px 0",
                  gap: "16.78px",
                  flex: 1,
                  minWidth: 0,
                  textAlign: "left",
                }}
              >
                {loadingStates.compliance ? (
                  <>
                    <Skeleton
                      animation="wave"
                      variant="text"
                      width="70%"
                      height={22}
                    />
                    <Box>
                      <Skeleton
                        animation="wave"
                        variant="text"
                        width="60%"
                        height={22}
                      />
                      <Box
                        sx={{
                          width: "100%",
                          height: "1px",
                          background: "#E4ECF7",
                          margin: "4px 0",
                        }}
                      />
                      <Skeleton
                        animation="wave"
                        variant="text"
                        width="50%"
                        height={18}
                      />
                    </Box>
                  </>
                ) : (
                  <>
                    <Typography
                      sx={{
                        fontWeight: 700,
                        fontSize: "16px",
                        lineHeight: "18px",
                        letterSpacing: "-0.005em",
                        color: "#000000",
                      }}
                    >
                      Overall compliance
                    </Typography>
                    <Box>
                      <Typography
                        sx={{
                          fontWeight: 700,
                          fontSize: "16px",
                          lineHeight: "136.52%",
                          color: "#000000",
                        }}
                      >
                        {overallComplianceDetail?.overallComplaintProductCount}{" "}
                        <span style={{ fontWeight: 400 }}>Compliant</span>
                      </Typography>
                      <Box
                        sx={{
                          width: "100%",
                          height: "1px",
                          background: "#E4ECF7",
                          margin: "4px 0",
                        }}
                      />
                      <Typography
                        sx={{
                          fontSize: "14px",
                          lineHeight: "16px",
                          color: "#718096",
                        }}
                      >
                        {`of ${overallComplianceDetail?.overallProductCount} services`}
                      </Typography>
                    </Box>
                  </>
                )}
              </Box>
              <Box
                sx={{
                  width: { xs: "160px", md: "180px", lg: "160px" },
                  flexShrink: 0,
                }}
              >
                {loadingStates.compliance ? (
                  <Skeleton
                    animation="wave"
                    variant="rectangular"
                    sx={{ width: "100%", height: "110px", borderRadius: "8px" }}
                  />
                ) : (
                  overallComplianceDetail && (
                    <ComplianceGauge
                      percentage={
                        overallComplianceDetail?.overallGreenPercentage
                      }
                    />
                  )
                )}
              </Box>
            </Box>
            <Box
              sx={{
                display: "flex",
                flexDirection: "column",
                padding: "0 0 16px",
                gap: "0",
                background: "#FFFFFF",
                boxShadow:
                  "0px 0px 4.1px rgba(12,26,75,0.01), 0px 2.097px 9.6px -0.699px rgba(50,50,71,0.05)",
                borderRadius: "10px",
                flex: {
                  xs: "1 1 100%",
                  sm: "1 1 100%",
                  md: "1 1 calc(50% - 11px)",
                  lg: "1 1 auto",
                },
                width: { xs: "100%", sm: "100%", md: "auto", lg: "100%" },
                minWidth: 0,
              }}
            >
              <Box
                sx={{
                  padding: "16px 18px 8px",
                  borderBottom: "1px solid #E5E5E5",
                }}
              >
                <Box
                  sx={{
                    display: "flex",
                    justifyContent: "space-between",
                    alignItems: "center",
                    marginBottom: "8px",
                  }}
                >
                  <Typography
                    sx={{
                      fontWeight: 700,
                      fontSize: "18px",
                      lineHeight: "28px",
                      color: "#0D0D0D",
                    }}
                  >
                    Achievements
                  </Typography>
                  <Box
                    sx={{
                      display: "flex",
                      alignItems: "center",
                      gap: "4px",
                      cursor: "pointer",
                      paddingRight: "6px",
                    }}
                    onClick={() => {
                      setAchievementOpen(true);
                      fetchAchievements(ACHIEVEMENTS_VIEW_ALL_PAGE_SIZE);
                    }}
                  >
                    <Typography
                      sx={{
                        fontSize: "14px",
                        lineHeight: "28px",
                        color: "#757575",
                        textDecoration: "underline",
                      }}
                    >
                      View all
                    </Typography>
                    <FiChevronRight
                      style={{ fontSize: "14px", color: "#757575" }}
                    />
                  </Box>
                </Box>
              </Box>

              <Box
                sx={{
                  display: "flex",
                  flexDirection: "column",
                  gap: "8px",
                  padding: "8px 8px 0",
                  justifyContent:
                    !loadingStates.achievement && achievementList?.length === 0
                      ? "center"
                      : "flex-start",
                  height: "16.5rem",
                  overflowY: "scroll",
                  "&::-webkit-scrollbar": { width: "6px" },
                  "&::-webkit-scrollbar-track": { background: "transparent" },
                  "&::-webkit-scrollbar-thumb": {
                    background: "rgba(0,0,0,0.18)",
                    borderRadius: "4px",
                  },
                }}
              >
                {loadingStates.achievement ? (
                  Array.from({ length: 4 }).map((_, index) => (
                    <Box
                      key={index}
                      sx={{
                        display: "flex",
                        flexDirection: "column",
                        padding: "8px",
                        gap: "8px",
                        background: "#F4F6F9",
                        borderRadius: "5px",
                      }}
                    >
                      <Box
                        sx={{
                          display: "flex",
                          justifyContent: "space-between",
                          alignItems: "center",
                          gap: "8px",
                        }}
                      >
                        <Skeleton
                          animation="wave"
                          variant="text"
                          width="30%"
                          height={20}
                        />
                        <Skeleton
                          animation="wave"
                          variant="text"
                          width="20%"
                          height={20}
                        />
                      </Box>
                      <Skeleton
                        animation="wave"
                        variant="text"
                        width="90%"
                        height={20}
                      />
                    </Box>
                  ))
                ) : achievementList?.length > 0 ? (
                  achievementList?.map((achievement, index) => (
                    <Box
                      key={index}
                      sx={{
                        display: "flex",
                        flexDirection: "row",
                        alignItems: "flex-start",
                        padding: "8px",
                        gap: "16px",
                        background: "#F4F6F9",
                        borderRadius: "5px",
                      }}
                    >
                      <Box
                        sx={{
                          display: "flex",
                          flexDirection: "column",
                          gap: "8px",
                          flex: 1,
                          minWidth: 0,
                        }}
                      >
                        <Box
                          sx={{
                            display: "flex",
                            justifyContent: "space-between",
                            alignItems: "center",
                            gap: "8px",
                            flexWrap: "wrap",
                          }}
                        >
                          <Typography
                            sx={{
                              fontSize: "14px",
                              lineHeight: "16px",
                              color: "#0D0D0D",
                            }}
                          >
                            {achievement.type}
                          </Typography>
                          <Box
                            sx={{
                              padding: "4px 6px",
                              background: "#E5E5E5",
                              borderRadius: "2px",
                              flexShrink: 0,
                            }}
                          >
                            <Typography
                              sx={{
                                fontSize: "12px",
                                lineHeight: "16px",
                                color: "#757575",
                                whiteSpace: "nowrap",
                              }}
                            >
                              {achievement.date}
                            </Typography>
                          </Box>
                        </Box>
                        <Typography
                          sx={{
                            fontSize: "14px",
                            lineHeight: "26px",
                            color: "#0D0D0D",
                            wordBreak: "break-word",
                            textAlign: "left",
                          }}
                        >
                          <>
                            <span
                              style={{
                                textDecoration: "underline",
                                textUnderlineOffset: "3px",
                                color: "rgba(0, 0, 0, 0.5)",
                              }}
                            >
                              {achievement.product}
                            </span>{" "}
                            {achievement.description}
                          </>
                        </Typography>
                      </Box>
                    </Box>
                  ))
                ) : (
                  <Box
                    sx={{
                      display: "flex",
                      justifyContent: "center",
                      alignItems: "center",
                      minHeight: "120px",
                    }}
                  >
                    <Typography
                      sx={{
                        fontSize: "16px",
                        color: "#757575",
                      }}
                    >
                      No Data Found
                    </Typography>
                  </Box>
                )}
              </Box>
            </Box>
          </Box>
        </Box>
      </MainWrapper>
    </>
  );
};

export default MainContent;
