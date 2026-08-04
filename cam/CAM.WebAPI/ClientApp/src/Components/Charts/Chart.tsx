import React, { useState, useEffect, useMemo } from "react";
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
import { safeNumber } from "../../Hook/Common";
import PieChartComponent from "../ECharts/CustomEPieChart";
import { useSearchParams } from "react-router-dom";
import { useAuth } from "../../Hook/useAuth";

let plannedActivityQuery: PlannedActivityQueryObjectGrid = {
  opCo: [],
  plannedImplementationYear: [],
  activityStatusId: [],
  planningActivityStatusId: [],
  designComponentId: [],
  originalDesignComponent: [],
  driver: [],
  benefits: [],
  plannedActivityDescription: [],
  activityDetailsText: [],
  budgetAvailability: [],
  deliveryProjectName: [],
  localApproval: [],
  deliveryStatusId: [],
  responsibilityPhaseId: [],
  plannedCompletion: undefined,
  notes: [],
  lcmEngineeringId: [],
  riskEngineeringEvaluation: [],
  riskEngineeringNotes: [],
  riskOperationalEvaluation: [],
  riskOperationalNotes: [],
  sortBy: "",
  isSortAscending: false,
  budgetTrackingId: [],
  budgetValueGrid: [],
  deliveryProjectId: [],
  plannedActivityResourceId: [],
  planningRisk: [],
  relatesToId: [],
  page: 1,
  pageSize: 500,
  lastModified: undefined,
  principalId: undefined,
  deleted: false,
  orphan: false,
  lastModifiedBy: [],
  forLcm: true,
  forNetwork: true,
  forDesignAspect: true,
  projectStatus: [],
  forLcmLink: ["True"],
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

const Chart: React.FC = () => {
  const { readonly, isPermesso } = useAuth();
  const navigate = useNavigate();
  const { darkMode, selectDarkMode } = useTheme();
  const [searchParams] = useSearchParams();
  const [searchQueryParam, setSearchQueryParam] = useState<any>(null);
  const theme = darkMode ? darkTheme : lightTheme;
  const [selectedOpco, setSelectedOpco] = React.useState("");
  const [selectedVertical, setSelectedVertical] = React.useState("");
  const [plannedActivitiesFormatedData, setPlannedActivitiesFormatedData] =
    useState<any>();
  const [
    plannedActivitiesVerticalFormatedData,
    setPlannedActivitiesVerticalFormatedData,
  ] = useState<any>();
  const [
    plannedActivitiesLCMFormatedData,
    setPlannedActivitiesLCMFormatedData,
  ] = useState<any>();
  const [plannedActivitiesOpcoData, setPlannedActivitiesOpcoData] =
    useState<any>();
  const [
    expiredPlannedActivitiesFormatedData,
    setExpiredPlannedActivitiesFormatedData,
  ] = useState<any>();

  useEffect(() => {
    selectDarkMode(true);
    return () => {
      selectDarkMode(false);
    };
  }, []);

  useEffect(() => {
    if (isPermesso) {
      getPaData();
      getPaResourceFilterValues();
      getPaOpcoFilterValues();
      getPaVerticalNameFilterValues();
    }
  }, [isPermesso]);

  const [plannedActivityData, setPlannedActivityData] = useState<any>();
  const getPaData = async () => {
    const paData = await GetPlannedActivityGridForChart(plannedActivityQuery);
    if (paData?.PlannedActivityGridResult) {
      setPlannedActivityData(paData?.PlannedActivityGridResult);
    }
  };

  const [resourceFilterValues, setResourceFilterValues] = useState<any>();
  const getPaResourceFilterValues = async () => {
    const filterValues = await GetFilterColumPlannedActivityForChart(
      "plannedActivityResourceId",
      ""
    );
    if (filterValues?.filter) {
      setResourceFilterValues(filterValues.filter);
    }
  };

  const [opcoFilterValues, setOpcoFilterValues] = useState<any>();
  const getPaOpcoFilterValues = async () => {
    const filterValues = await GetFilterColumPlannedActivityForChart(
      "opCo",
      ""
    );
    if (filterValues?.filter) {
      setOpcoFilterValues(filterValues.filter);
    }
  };

  const [verticalNameFilterValues, setVerticalNameFilterValues] =
    useState<any>();
  const getPaVerticalNameFilterValues = async () => {
    const filterValues = await GetFilterColumPlannedActivityForChart(
      "verticalName",
      ""
    );
    if (filterValues?.filter) {
      setVerticalNameFilterValues(filterValues.filter);
    }
  };

  useEffect(() => {
    if (searchParams && searchParams["size"] !== 0) {
      const paramsObj = Object.fromEntries(searchParams?.entries());
      const transformedObj: any = Object.keys(paramsObj).reduce((acc, key) => {
        const value = paramsObj[key];
        acc[key] = value?.split(",");
        return acc;
      }, {});
      setSearchQueryParam({
        axisValue: transformedObj.axisValue[0],
        dataIndex: safeNumber(transformedObj.dataIndex[0]),
        seriesValues: {
          "auto-generated-id-0": safeNumber(transformedObj.seriesValues[0]),
        },
      });
    }
  }, [searchParams]);

  useEffect(() => {
    const now = new Date();

    if (plannedActivityData?.items && plannedActivityData?.items?.length) {
      const data = plannedActivityData?.items;
      const categorizedDataMap: Map<string, any[]> = data
        .filter((data) => data["forLcmLink"] === true)
        .reduce((acc, item) => {
          const plannedCompletionDate = new Date(item.plannedCompletion);
          const plannedCompletionYear = new Date(
            `${item.plannedCompletion}`
          ).getUTCFullYear();

          if (plannedCompletionDate > now) {
            if (!acc.has(plannedCompletionYear)) {
              acc.set(plannedCompletionYear, []);
            }
            acc.get(plannedCompletionYear).push(item);
          } else {
            if (!acc.has(plannedCompletionYear)) {
              acc.set(plannedCompletionYear, []);
            }
          }
          return acc;
        }, new Map());

      if (Array.from(categorizedDataMap.entries()).length) {
        setPlannedActivitiesFormatedData(
          Array.from(categorizedDataMap.entries())
            .map((categoryData) => {
              return [String(categoryData[0]), categoryData[1].length];
            })
            .sort(([yearA], [yearB]) => {
              const numA = safeNumber(yearA);
              const numB = safeNumber(yearB);
              return numA - numB;
            })
        );
      } else {
        setPlannedActivitiesFormatedData([]);
      }

      const categorizedExpiredPADataMap: Map<string, any[]> = data
        .filter((data) => data["forLcmLink"] === true)
        .reduce((acc, item) => {
          const plannedCompletionDate = new Date(item.plannedCompletion);

          if (plannedCompletionDate < now) {
            let plannedCompletionYear = new Date(
              `${item.plannedCompletion}`
            ).getUTCFullYear();
            if (!acc.has(plannedCompletionYear)) {
              acc.set(plannedCompletionYear, []);
            }
            acc.get(plannedCompletionYear).push(item);
          }
          return acc;
        }, new Map());
      if (Array.from(categorizedExpiredPADataMap.entries()).length) {
        setExpiredPlannedActivitiesFormatedData(
          Array.from(categorizedExpiredPADataMap.entries())
            .map((categoryData) => {
              return [String(categoryData[0]), categoryData[1].length];
            })
            .sort(([yearA], [yearB]) => {
              const numA = safeNumber(yearA);
              const numB = safeNumber(yearB);
              return numA - numB;
            })
        );
      } else {
        setExpiredPlannedActivitiesFormatedData([]);
      }
    }
  }, [plannedActivityData]);

  useEffect(() => {
    if (
      plannedActivityData?.items &&
      plannedActivityData?.items?.length &&
      resourceFilterValues &&
      resourceFilterValues?.length
    ) {
      const data = plannedActivityData?.items;

      const deliveryStatusDataMap: Map<string, any[]> = data
        .filter((data) => data["forLcmLink"] === true)
        .reduce((acc, item) => {
          if (!acc.has(item.plannedActivityResourceId)) {
            acc.set(item.plannedActivityResourceId, []);
          }
          acc.get(item.plannedActivityResourceId).push(item);
          return acc;
        }, new Map());

      const regex = /\([^\)]*\)/g;
      let b = Array.from(deliveryStatusDataMap.entries())
        .filter((data, index) => index < 8)
        .map((data, index) => {
          const filterId = resourceFilterValues?.filter((filterData) => {
            return filterData.text === data[0];
          });
          return {
            id: filterId ? filterId[0].value : index,
            value: data[1].length,
            label: data[0].replace(regex, "").trim(),
            // label: data[0],
            filterId: filterId ? filterId[0].value : index,
          };
        });
      if (b.length) {
        setPlannedActivitiesLCMFormatedData(b);
      } else {
        setPlannedActivitiesLCMFormatedData([]);
      }

      // opco
      const opcoMap: Map<string, any[]> = data
        .filter((data) => data["forLcmLink"] === true)
        .reduce((acc, item) => {
          if (!acc.has(item.opCo)) {
            acc.set(item.opCo, []);
          }
          acc.get(item.opCo).push(item);
          return acc;
        }, new Map());

      setPlannedActivitiesOpcoData(
        Array.from(opcoMap.entries())
          .filter((data) => data[1].length > 10)
          .map((data) => {
            return [String(data[0]), data[1].length];
          })
      );

      //   vertical
      const verticalMap: Map<string, any[]> = data
        .filter((data) => data["forLcmLink"] === true)
        .reduce((acc, item) => {
          const verticalNames = item["verticalName"]
            .split(",")
            .map((name) => name.trim());

          verticalNames.forEach((verticalName) => {
            if (!acc.has(verticalName)) {
              acc.set(verticalName, []);
            }
            acc.get(verticalName)?.push(item);
          });

          return acc;
        }, new Map());

      setPlannedActivitiesVerticalFormatedData(
        Array.from(verticalMap.entries())
          .filter((data) => data[0])
          .map((data) => {
            return [String(data[0]), data[1].length];
          })
      );
      if (
        searchParams &&
        searchParams["size"] !== 0 &&
        isPermesso &&
        searchQueryParam &&
        plannedActivityData?.items?.length
      ) {
        handleChangeOpCo(null, searchQueryParam);
        setHighLightedOpCoItem((prev) => ({
          dataIndex: prev?.dataIndex,
          seriesId:
            prev?.dataIndex === 0 || prev?.dataIndex
              ? "auto-generated-id-0"
              : "",
        }));
      }
    }
  }, [plannedActivityData, resourceFilterValues]);

  // filter
  const handleChangeOpCo = (event, barItemIdentifier) => {
    setHighLightedBarItem({});
    setSelectedOpco(barItemIdentifier.axisValue);
    setSelectedVertical("");
    handleHighLightedOpCoItem(barItemIdentifier.dataIndex);

    if (
      plannedActivityData?.items &&
      plannedActivityData?.items?.length &&
      resourceFilterValues &&
      resourceFilterValues?.length
    ) {
      const data = plannedActivityData?.items;
      const now = new Date();

      // planned completion year
      const categorizedDataMap: Map<string, any[]> = data
        .filter(
          (data) =>
            data["forLcmLink"] === true &&
            data["opCo"] === barItemIdentifier.axisValue
        )
        .reduce((acc, item) => {
          const plannedCompletionDate = new Date(item.plannedCompletion);
          const plannedCompletionYear = new Date(
            `${item.plannedCompletion}`
          ).getUTCFullYear();

          if (plannedCompletionDate > now) {
            if (!acc.has(plannedCompletionYear)) {
              acc.set(plannedCompletionYear, []);
            }
            acc.get(plannedCompletionYear).push(item);
          } else {
            if (!acc.has(plannedCompletionYear)) {
              acc.set(plannedCompletionYear, []);
            }
          }
          return acc;
        }, new Map());

      if (Array.from(categorizedDataMap.entries()).length) {
        setPlannedActivitiesFormatedData(
          Array.from(categorizedDataMap.entries())
            .map((data) => {
              return [String(data[0]), data[1].length];
            })
            .sort(([yearA], [yearB]) => {
              const numA = safeNumber(yearA);
              const numB = safeNumber(yearB);
              return numA - numB;
            })
        );
      } else {
        setPlannedActivitiesFormatedData([]);
      }

      // pie chart
      const deliveryStatusDataMap: Map<string, any[]> = data
        .filter(
          (data) =>
            data["forLcmLink"] === true &&
            data["opCo"] === barItemIdentifier.axisValue
        )
        .reduce((acc, item) => {
          if (!acc.has(item.plannedActivityResourceId)) {
            acc.set(item.plannedActivityResourceId, []);
          }
          acc.get(item.plannedActivityResourceId).push(item);
          return acc;
        }, new Map());

      const regex = /\([^\)]*\)/g;
      let b = Array.from(deliveryStatusDataMap.entries())
        .filter((data, index) => index < 8)
        .map((data, index) => {
          const filterId = resourceFilterValues?.filter((filterData) => {
            return filterData.text === data[0];
          });
          return {
            id: filterId ? filterId[0].value : index,
            value: data[1].length,
            label: data[0].replace(regex, "").trim(),
            // label: data[0],
            filterId: filterId ? filterId[0].value : index,
          };
        });
      if (b.length) {
        setPlannedActivitiesLCMFormatedData(b);
      } else {
        setPlannedActivitiesLCMFormatedData([]);
      }

      //   vertical
      const verticalMap: Map<string, any[]> = data
        .filter(
          (data) =>
            data["forLcmLink"] === true &&
            data["opCo"] === barItemIdentifier.axisValue
        )
        .reduce((acc, item) => {
          const verticalNames = item["verticalName"]
            .split(",")
            .map((name) => name.trim());

          verticalNames.forEach((verticalName) => {
            if (!acc.has(verticalName)) {
              acc.set(verticalName, []);
            }
            acc.get(verticalName)?.push(item);
          });

          return acc;
        }, new Map());

      setPlannedActivitiesVerticalFormatedData(
        Array.from(verticalMap.entries())
          .filter((data) => data[0])
          .map((data) => {
            return [String(data[0]), data[1].length];
          })
      );

      // expired pa
      const categorizedExpiredPADataMap: Map<string, any[]> = data
        .filter(
          (data) =>
            data["forLcmLink"] === true &&
            data["opCo"] === barItemIdentifier.axisValue
        )
        .reduce((acc, item) => {
          const plannedCompletionDate = new Date(item.plannedCompletion);

          if (plannedCompletionDate < now) {
            let plannedCompletionYear = new Date(
              `${item.plannedCompletion}`
            ).getUTCFullYear();
            if (!acc.has(plannedCompletionYear)) {
              acc.set(plannedCompletionYear, []);
            }
            acc.get(plannedCompletionYear).push(item);
          }
          return acc;
        }, new Map());
      if (Array.from(categorizedExpiredPADataMap.entries()).length) {
        setExpiredPlannedActivitiesFormatedData(
          Array.from(categorizedExpiredPADataMap.entries())
            .map((categoryData) => {
              return [String(categoryData[0]), categoryData[1].length];
            })
            .sort(([yearA], [yearB]) => {
              const numA = safeNumber(yearA);
              const numB = safeNumber(yearB);
              return numA - numB;
            })
        );
      } else {
        setExpiredPlannedActivitiesFormatedData([]);
      }
    }
  };

  const handleChangeVertical = (params) => {
    setSelectedVertical(params.name);
    if (
      plannedActivityData?.items &&
      plannedActivityData?.items?.length &&
      resourceFilterValues &&
      resourceFilterValues?.length
    ) {
      const data = plannedActivityData?.items;
      const now = new Date();

      // planned completion year
      const categorizedDataMap: Map<string, any[]> = data
        .filter((data) => {
          const selectedVerticalList = params.name.split(",");
          const verticalList = data["verticalName"].split(",");

          const verticalMatch = verticalList.some((value) =>
            selectedVerticalList.includes(value)
          );
          return selectedOpco
            ? data["forLcmLink"] === true &&
                verticalMatch &&
                data["opCo"] === selectedOpco
            : data["forLcmLink"] === true && verticalMatch;
        })
        .reduce((acc, item) => {
          const plannedCompletionDate = new Date(item.plannedCompletion);
          const plannedCompletionYear = new Date(
            `${item.plannedCompletion}`
          ).getUTCFullYear();

          if (plannedCompletionDate > now) {
            if (!acc.has(plannedCompletionYear)) {
              acc.set(plannedCompletionYear, []);
            }
            acc.get(plannedCompletionYear).push(item);
          } else {
            if (!acc.has(plannedCompletionYear)) {
              acc.set(plannedCompletionYear, []);
            }
          }
          return acc;
        }, new Map());

      if (Array.from(categorizedDataMap.entries()).length) {
        setPlannedActivitiesFormatedData(
          Array.from(categorizedDataMap.entries())
            .map((data) => {
              return [String(data[0]), data[1].length];
            })
            .sort(([yearA], [yearB]) => {
              const numA = safeNumber(yearA);
              const numB = safeNumber(yearB);
              return numA - numB;
            })
        );
      } else {
        setPlannedActivitiesFormatedData([]);
      }

      // pie chart
      const deliveryStatusDataMap: Map<string, any[]> = data
        .filter((data) => {
          const selectedVerticalList = params.name.split(",");
          const verticalList = data["verticalName"].split(",");

          const verticalMatch = verticalList.some((value) =>
            selectedVerticalList.includes(value)
          );
          return selectedOpco
            ? data["forLcmLink"] === true &&
                verticalMatch &&
                data["opCo"] === selectedOpco
            : data["forLcmLink"] === true && verticalMatch;
        })
        .reduce((acc, item) => {
          if (!acc.has(item.plannedActivityResourceId)) {
            acc.set(item.plannedActivityResourceId, []);
          }
          acc.get(item.plannedActivityResourceId).push(item);
          return acc;
        }, new Map());

      const regex = /\([^\)]*\)/g;
      let b = Array.from(deliveryStatusDataMap.entries())
        .filter((data, index) => index < 8)
        .map((data, index) => {
          const filterId = resourceFilterValues?.filter((filterData) => {
            return filterData.text === data[0];
          });
          return {
            id: filterId ? filterId[0].value : index,
            value: data[1].length,
            label: data[0].replace(regex, "").trim(),
            // label: data[0],
            filterId: filterId ? filterId[0].value : index,
          };
        });
      if (b.length) {
        setPlannedActivitiesLCMFormatedData(b);
      } else {
        setPlannedActivitiesLCMFormatedData([]);
      }

      // expired pa
      const categorizedExpiredPADataMap: Map<string, any[]> = data
        .filter((data) => {
          const selectedVerticalList = params.name.split(",");
          const verticalList = data["verticalName"].split(",");

          const verticalMatch = verticalList.some((value) =>
            selectedVerticalList.includes(value)
          );
          return selectedOpco
            ? data["forLcmLink"] === true &&
                verticalMatch &&
                data["opCo"] === selectedOpco
            : data["forLcmLink"] === true && verticalMatch;
        })
        .reduce((acc, item) => {
          const plannedCompletionDate = new Date(item.plannedCompletion);

          if (plannedCompletionDate < now) {
            let plannedCompletionYear = new Date(
              `${item.plannedCompletion}`
            ).getUTCFullYear();
            if (!acc.has(plannedCompletionYear)) {
              acc.set(plannedCompletionYear, []);
            }
            acc.get(plannedCompletionYear).push(item);
          }
          return acc;
        }, new Map());
      if (Array.from(categorizedExpiredPADataMap.entries()).length) {
        setExpiredPlannedActivitiesFormatedData(
          Array.from(categorizedExpiredPADataMap.entries())
            .map((categoryData) => {
              return [String(categoryData[0]), categoryData[1].length];
            })
            .sort(([yearA], [yearB]) => {
              const numA = safeNumber(yearA);
              const numB = safeNumber(yearB);
              return numA - numB;
            })
        );
      } else {
        setExpiredPlannedActivitiesFormatedData([]);
      }
    }
  };

  const verticalFilter = useMemo(
    () => (
      <PieChartComponent
        isDarkMode={darkMode}
        seriesData={plannedActivitiesVerticalFormatedData?.map((vertical) => ({
          value: vertical[1],
          name: vertical[0],
        }))}
        filterCallBack={handleChangeVertical}
      />
    ),
    [plannedActivitiesVerticalFormatedData, darkMode]
  );

  // redirect
  const handleClickCompletionYear = (event, barItemIdentifier) => {
    const today = new Date();
    const year = String(today.getFullYear());

    const filterObj = {
      state: {
        forLcmLink: ["True"],
        // plannedImplementationYear: [`${barItemIdentifier.axisValue}`],
        plannedCompletionValue: {
          endDate: `${barItemIdentifier.axisValue}/12/31`,
          startDate: `${barItemIdentifier.axisValue}/1/1`,
        },
      },
    };
    const paramObj = {
      state: {
        forLcmLink: ["True"],
        // plannedImplementationYear: [`${barItemIdentifier.axisValue}`],
        endDate: `${barItemIdentifier.axisValue}/12/31`,
        startDate: `${barItemIdentifier.axisValue}/1/1`,
      },
    };
    if (selectedOpco) {
      const selectedOpcoValue = opcoFilterValues.filter(
        (opcoVal) => opcoVal.text === selectedOpco
      );
      if (selectedOpcoValue.length) {
        filterObj.state["opCo"] = [String(selectedOpcoValue[0].value)];
        paramObj.state["opCo"] = [String(selectedOpcoValue[0].value)];
      }
    }
    if (selectedVertical) {
      const selectedVerticalList = selectedVertical.split(",");

      const selectedVericalValue = verticalNameFilterValues
        .filter((pair) => selectedVerticalList.includes(pair.text))
        .map((pair) => pair.value);

      filterObj.state["verticalNameId"] = selectedVericalValue;
      paramObj.state["verticalNameId"] = selectedVericalValue;
    }
    if (barItemIdentifier.axisValue !== year) {
      // navigate(`/plannedActivities/LCM`, filterObj);

      // Serialize the state object to query params
      const queryParams = new URLSearchParams();
      for (const key in paramObj.state) {
        if (Array.isArray(paramObj.state[key])) {
          paramObj.state[key].forEach((val) =>
            queryParams.append(`${key}`, val)
          );
        } else {
          queryParams.append(`${key}`, paramObj.state[key]);
        }
      }
      const newTabUrl = `/plannedActivities/LCM?${queryParams.toString()}`;
      window.open(newTabUrl, "_blank");
    }
  };

  const handlePAYearClick = (data, index) => {
    if (index.seriesId) {
      const selectedPAYear = plannedActivitiesFormatedData[index.dataIndex][0];

      const today = new Date();
      const year = String(today.getFullYear());
      const month = today.getMonth() + 1; // Months are 0-indexed
      const date = today.getDate();
      if (index.seriesId === "auto-generated-id-0") {
        // expired pa
        const filterObj = {
          state: {
            forLcmLink: ["True"],
            plannedCompletionValue: {
              endDate: `${selectedPAYear}/12/31`,
              startDate: `${selectedPAYear}/1/1`,
            },
          },
        };
        const paramObj = {
          state: {
            forLcmLink: ["True"],
            endDate: `${selectedPAYear}/12/31`,
            startDate: `${selectedPAYear}/1/1`,
          },
        };

        if (selectedPAYear === year) {
          filterObj.state.plannedCompletionValue[
            "endDate"
          ] = `${year}/${month}/${date}`;
          paramObj.state.endDate = `${year}/${month}/${date}`;
        }
        if (selectedOpco) {
          const selectedOpcoValue = opcoFilterValues.filter(
            (opcoVal) => opcoVal.text === selectedOpco
          );
          if (selectedOpcoValue.length) {
            filterObj.state["opCo"] = [String(selectedOpcoValue[0].value)];
            paramObj.state["opCo"] = [String(selectedOpcoValue[0].value)];
          }
        }
        if (selectedVertical) {
          const selectedVerticalList = selectedVertical.split(",");

          const selectedVericalValue = verticalNameFilterValues
            .filter((pair) => selectedVerticalList.includes(pair.text))
            .map((pair) => pair.value);

          filterObj.state["verticalNameId"] = selectedVericalValue;
          paramObj.state["verticalNameId"] = selectedVericalValue;
        }
        // navigate(`/plannedActivities/LCM`, filterObj);

        // Serialize the state object to query params
        const queryParams = new URLSearchParams();
        for (const key in paramObj.state) {
          if (Array.isArray(paramObj.state[key])) {
            paramObj.state[key].forEach((val) =>
              queryParams.append(`${key}`, val)
            );
          } else {
            queryParams.append(`${key}`, paramObj.state[key]);
          }
        }
        const newTabUrl = `/plannedActivities/LCM?${queryParams.toString()}`;
        window.open(newTabUrl, "_blank");
      }
      if (index.seriesId === "auto-generated-id-1") {
        // completion pa
        const filterObj = {
          state: {
            forLcmLink: ["True"],
            // plannedImplementationYear: [`${barItemIdentifier.axisValue}`],
            plannedCompletionValue: {
              endDate: `${selectedPAYear}/12/31`,
              startDate: `${selectedPAYear}/1/1`,
            },
          },
        };
        const paramObj = {
          state: {
            forLcmLink: ["True"],
            endDate: `${selectedPAYear}/12/31`,
            startDate: `${selectedPAYear}/1/1`,
          },
        };
        if (selectedPAYear === year) {
          filterObj.state.plannedCompletionValue[
            "startDate"
          ] = `${year}/${month}/${date}`;
          paramObj.state.startDate = `${year}/${month}/${date}`;
        }
        if (selectedOpco) {
          const selectedOpcoValue = opcoFilterValues.filter(
            (opcoVal) => opcoVal.text === selectedOpco
          );
          if (selectedOpcoValue.length) {
            filterObj.state["opCo"] = [String(selectedOpcoValue[0].value)];
            paramObj.state["opCo"] = [String(selectedOpcoValue[0].value)];
          }
        }
        if (selectedVertical) {
          const selectedVerticalList = selectedVertical.split(",");

          const selectedVericalValue = verticalNameFilterValues
            .filter((pair) => selectedVerticalList.includes(pair.text))
            .map((pair) => pair.value);

          filterObj.state["verticalNameId"] = selectedVericalValue;
          paramObj.state["verticalNameId"] = selectedVericalValue;
        }
        // navigate(`/plannedActivities/LCM`, filterObj);

        // Serialize the state object to query params
        const queryParams = new URLSearchParams();
        for (const key in paramObj.state) {
          if (Array.isArray(paramObj.state[key])) {
            paramObj.state[key].forEach((val) =>
              queryParams.append(`${key}`, val)
            );
          } else {
            queryParams.append(`${key}`, paramObj.state[key]);
          }
        }
        const newTabUrl = `/plannedActivities/LCM?${queryParams.toString()}`;
        window.open(newTabUrl, "_blank");
      }
    }
    // You can handle the clicked segment here
  };

  const handleClickType = (event, itemIdentifier, item) => {
    const filterObj = {
      state: {
        forLcmLink: ["True"],
        plannedActivityResourceId: [`${item.filterId}`],
      },
    };
    if (selectedOpco) {
      const selectedOpcoValue = opcoFilterValues.filter(
        (opcoVal) => opcoVal.text === selectedOpco
      );
      if (selectedOpcoValue.length) {
        filterObj.state["opCo"] = [String(selectedOpcoValue[0].value)];
      }
    }
    if (selectedVertical) {
      const selectedVerticalList = selectedVertical.split(",");

      const selectedVericalValue = verticalNameFilterValues
        .filter((pair) => selectedVerticalList.includes(pair.text))
        .map((pair) => pair.value);

      filterObj.state["verticalNameId"] = selectedVericalValue;
    }
    // navigate(`/plannedActivities/LCM`, filterObj);

    // Serialize the state object to query params
    const queryParams = new URLSearchParams();
    for (const key in filterObj.state) {
      if (Array.isArray(filterObj.state[key])) {
        filterObj.state[key].forEach((val) =>
          queryParams.append(`${key}`, val)
        );
      } else {
        queryParams.append(`${key}`, filterObj.state[key]);
      }
    }
    const newTabUrl = `/plannedActivities/LCM?${queryParams.toString()}`;
    window.open(newTabUrl, "_blank");
  };

  const handleClickExpired = (event, barItemIdentifier) => {
    const today = new Date();
    const year = String(today.getFullYear());
    const month = today.getMonth() + 1; // Months are 0-indexed
    const date = today.getDate();
    const filterObj = {
      state: {
        forLcmLink: ["True"],
        // plannedImplementationYear: [`${barItemIdentifier.axisValue}`],
        plannedCompletionValue: {
          endDate: `${barItemIdentifier.axisValue}/12/31`,
          startDate: `${barItemIdentifier.axisValue}/1/1`,
        },
      },
    };
    const paramObj = {
      state: {
        forLcmLink: ["True"],
        endDate: `${barItemIdentifier.axisValue}/12/31`,
        startDate: `${barItemIdentifier.axisValue}/1/1`,
      },
    };

    if (barItemIdentifier.axisValue === year) {
      filterObj.state.plannedCompletionValue[
        "endDate"
      ] = `${year}/${month}/${date}`;
      paramObj.state.endDate = `${year}/${month}/${date}`;
    }
    if (selectedOpco) {
      const selectedOpcoValue = opcoFilterValues.filter(
        (opcoVal) => opcoVal.text === selectedOpco
      );
      if (selectedOpcoValue.length) {
        filterObj.state["opCo"] = [String(selectedOpcoValue[0].value)];
      }
    }
    if (selectedVertical) {
      const selectedVerticalList = selectedVertical.split(",");

      const selectedVericalValue = verticalNameFilterValues
        .filter((pair) => selectedVerticalList.includes(pair.text))
        .map((pair) => pair.value);

      filterObj.state["verticalNameId"] = selectedVericalValue;
    }
    // navigate(`/plannedActivities/LCM`, filterObj);

    // Serialize the state object to query params
    const queryParams = new URLSearchParams();
    for (const key in paramObj.state) {
      if (Array.isArray(paramObj.state[key])) {
        paramObj.state[key].forEach((val) => queryParams.append(`${key}`, val));
      } else {
        queryParams.append(`${key}`, paramObj.state[key]);
      }
    }
    const newTabUrl = `/plannedActivities/LCM?${queryParams.toString()}`;
    window.open(newTabUrl, "_blank");
  };

  // hightlight
  const [highlightedBarItem, setHighLightedBarItem] =
    useState<HighlightItemData | null>({});
  const handleHighLightedItem = (val) => {
    setHighLightedBarItem((prev) => ({
      ...prev,
      dataIndex: safeNumber(val),
      seriesId: "auto-generated-id-0",
    }));
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

  // reset

  const handleReset = () => {
    if (selectedOpco) {
      setSelectedOpco("");
      setHighLightedOpCoItem({});
    }
    if (selectedVertical) {
      setSelectedVertical("");
      setHighLightedBarItem({});
    }
    if (
      plannedActivityData?.items &&
      plannedActivityData?.items?.length &&
      resourceFilterValues &&
      resourceFilterValues?.length
    ) {
      const data = plannedActivityData?.items;
      const now = new Date();

      const deliveryStatusDataMap: Map<string, any[]> = data
        .filter((data) => data["forLcmLink"] === true)
        .reduce((acc, item) => {
          if (!acc.has(item.plannedActivityResourceId)) {
            acc.set(item.plannedActivityResourceId, []);
          }
          acc.get(item.plannedActivityResourceId).push(item);
          return acc;
        }, new Map());

      const regex = /\([^\)]*\)/g;
      let b = Array.from(deliveryStatusDataMap.entries())
        .filter((data, index) => index < 8)
        .map((data, index) => {
          const filterId = resourceFilterValues?.filter((filterData) => {
            return filterData.text === data[0];
          });
          return {
            id: filterId ? filterId[0].value : index,
            value: data[1].length,
            label: data[0].replace(regex, "").trim(),
            // label: data[0],
            filterId: filterId ? filterId[0].value : index,
          };
        });
      if (b.length) {
        setPlannedActivitiesLCMFormatedData(b);
      } else {
        setPlannedActivitiesLCMFormatedData([]);
      }

      // opco
      const opcoMap: Map<string, any[]> = data
        .filter((data) => data["forLcmLink"] === true)
        .reduce((acc, item) => {
          if (!acc.has(item.opCo)) {
            acc.set(item.opCo, []);
          }
          acc.get(item.opCo).push(item);
          return acc;
        }, new Map());

      setPlannedActivitiesOpcoData(
        Array.from(opcoMap.entries())
          .filter((data) => data[1].length > 10)
          .map((data) => {
            return [String(data[0]), data[1].length];
          })
      );

      //   vertical
      const verticalMap: Map<string, any[]> = data
        .filter((data) => data["forLcmLink"] === true)
        .reduce((acc, item) => {
          const verticalNames = item["verticalName"]
            .split(",")
            .map((name) => name.trim());

          verticalNames.forEach((verticalName) => {
            if (!acc.has(verticalName)) {
              acc.set(verticalName, []);
            }
            acc.get(verticalName)?.push(item);
          });

          return acc;
        }, new Map());

      setPlannedActivitiesVerticalFormatedData(
        Array.from(verticalMap.entries())
          .filter((data) => data[0])
          .map((data) => {
            return [String(data[0]), data[1].length];
          })
      );

      // planned activity year
      const categorizedDataMap: Map<string, any[]> = data.reduce(
        (acc, item) => {
          const plannedCompletionDate = new Date(item.plannedCompletion);
          const plannedCompletionYear = new Date(
            `${item.plannedCompletion}`
          ).getUTCFullYear();

          if (plannedCompletionDate > now) {
            if (!acc.has(plannedCompletionYear)) {
              acc.set(plannedCompletionYear, []);
            }
            acc.get(plannedCompletionYear).push(item);
          } else {
            if (!acc.has(plannedCompletionYear)) {
              acc.set(plannedCompletionYear, []);
            }
          }
          return acc;
        },
        new Map()
      );

      if (Array.from(categorizedDataMap.entries()).length) {
        setPlannedActivitiesFormatedData(
          Array.from(categorizedDataMap.entries())
            .map((categoryData) => {
              return [String(categoryData[0]), categoryData[1].length];
            })
            .sort(([yearA], [yearB]) => {
              const numA = safeNumber(yearA);
              const numB = safeNumber(yearB);
              return numA - numB;
            })
        );
      } else {
        setPlannedActivitiesFormatedData([]);
      }

      // expired pa
      const categorizedExpiredPADataMap: Map<string, any[]> = data.reduce(
        (acc, item) => {
          const plannedCompletionDate = new Date(item.plannedCompletion);

          if (plannedCompletionDate < now) {
            let plannedCompletionYear = new Date(
              `${item.plannedCompletion}`
            ).getUTCFullYear();
            if (!acc.has(plannedCompletionYear)) {
              acc.set(plannedCompletionYear, []);
            }
            acc.get(plannedCompletionYear).push(item);
          }
          return acc;
        },
        new Map()
      );
      if (Array.from(categorizedExpiredPADataMap.entries()).length) {
        setExpiredPlannedActivitiesFormatedData(
          Array.from(categorizedExpiredPADataMap.entries())
            .map((categoryData) => {
              return [String(categoryData[0]), categoryData[1].length];
            })
            .sort(([yearA], [yearB]) => {
              const numA = safeNumber(yearA);
              const numB = safeNumber(yearB);
              return numA - numB;
            })
        );
      } else {
        setExpiredPlannedActivitiesFormatedData([]);
      }
    }
  };

  return (
    <ThemeProvider theme={theme}>
      <div className="container">
        <div className="headerPage row mx-0 justify-content-between">
          <h3 className="voda-bold fz-28" style={{ textAlign: "left" }}>
            Planned Activity Report
          </h3>

          {selectedOpco || selectedVertical ? (
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
      </div>
      {/* {selectedOpco || selectedVertical ? (
        <Button onClick={handleReset}>Reset Filter</Button>
      ) : (
        ""
      )} */}
      {plannedActivityData ? (
        <div
          className="container mt-3 mb-3"
          style={{
            position: "relative",
            background: `${darkMode ? "black" : "white"}`,
            border: `${darkMode ? "2px solid white" : "2px solid black"}`,
          }}
        >
          {/* {plannedActivityData &&
        plannedActivitiesOpcoData &&
        plannedActivitiesOpcoData?.length ? (
          <FormControl sx={{ m: 1, minWidth: 120 }}>
            <Select
              value={selectedOpco}
              onChange={handleChange}
              displayEmpty
              inputProps={{ "aria-label": "Without label" }}
            >
              <MenuItem value="">
                <em>None</em>
              </MenuItem>
              {plannedActivitiesOpcoData?.map((data) => (
                <MenuItem value={data[0]}>{data[0]}</MenuItem>
              ))}
            </Select>
          </FormControl>
        ) : (
          ""
        )} */}

          <Stack
            direction="row"
            sx={{
              width: "100%",
              textAlign: "left",
            }}
            spacing={2}

            // className={`${darkMode ? "border" : "border border-dark"}`}
            // style={{
            //   border: `${darkMode ? "2px solid white" : "2px solid black"}`,
            // }}
          >
            {plannedActivityData &&
            plannedActivitiesOpcoData &&
            plannedActivitiesOpcoData?.length ? (
              <>
                <Box sx={{ width: "50%" }}>
                  <Paper>
                    <Typography className="px-2 py-2">
                      Planned Activity by Local Market (OpCo)
                    </Typography>
                  </Paper>
                  <BarChart
                    dataset={plannedActivitiesOpcoData}
                    xAxis={[
                      {
                        scaleType: "band",
                        dataKey: `${[0]}`,
                      },
                    ]}
                    series={[
                      {
                        dataKey: `${[1]}`,
                        label: "Planned Activities",
                        highlightScope: {
                          highlighted: "item",
                          faded: "global",
                        } as HighlightScope,
                      },
                    ]}
                    // layout="horizontal"
                    height={400}
                    // onAxisClick={handleChangeOpCo}
                    onAxisClick={(event, barItemIdentifier) => {
                      const newTabUrl = `/plannedactivityreport?axisValue=${barItemIdentifier?.axisValue}&dataIndex=${barItemIdentifier?.dataIndex}&seriesValues=${barItemIdentifier?.seriesValues["auto-generated-id-0"]}                      `;
                      window.open(newTabUrl, "_blank");
                    }}
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
                </Box>
              </>
            ) : (
              ""
            )}

            <div
              style={{
                width: "2px",
                background: `${darkMode ? "white" : "black"}`,
              }}
            ></div>

            {/* <Box width="50%"> */}
            {/* {plannedActivityData &&
              plannedActivitiesVerticalFormatedData &&
              plannedActivitiesVerticalFormatedData?.length ? (
                <>
                  <Paper>
                    <Typography className="px-2 py-2">
                      Planned Activity by Vertical
                    </Typography>
                  </Paper>
                  <BarChart
                    dataset={plannedActivitiesVerticalFormatedData}
                    yAxis={[
                      {
                        scaleType: "band",
                        dataKey: `${[0]}`,
                      },
                    ]}
                    series={[
                      {
                        dataKey: `${[1]}`,
                        label: "Planned Activities",
                        highlightScope: {
                          highlighted: "item",
                          faded: "global",
                        } as HighlightScope,
                      },
                    ]}
                    layout="horizontal"
                    height={400}
                    onAxisClick={handleChangeVertical}
                    barLabel={(item) => item.value?.toString()}
                    highlightedItem={highlightedBarItem}
                    margin={{ left: 120 }}
                    onHighlightChange={() =>
                      setHighLightedBarItem((prev) => ({
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
              )} */}
            {/* </Box> */}

            <Box sx={{ width: "50%" }}>
              {" "}
              {plannedActivityData &&
              plannedActivitiesVerticalFormatedData &&
              plannedActivitiesVerticalFormatedData?.length ? (
                <>
                  <Paper>
                    <Typography className="px-2 py-2">
                      Planned Activity by Vertical
                    </Typography>
                  </Paper>
                  {verticalFilter}
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
            sx={{
              width: "100%",
              textAlign: "left",
            }}
            spacing={2}
            // className={`${darkMode ? "border" : "border border-dark"}`}
            // style={{
            //   border: `${darkMode ? "2px solid white" : "2px solid black"}`,
            // }}
          >
            <Box
              component="div"
              style={{
                width: "50%",
                minWidth: "50%",
              }}
            >
              {plannedActivityData &&
              plannedActivitiesLCMFormatedData &&
              plannedActivitiesLCMFormatedData?.length ? (
                <>
                  <Paper>
                    <Typography className="px-2 py-2">
                      Planned Activities by Types
                    </Typography>
                  </Paper>
                  <PieChart
                    series={[
                      {
                        // arcLabel: (item) => `${item.value}`,
                        data: plannedActivitiesLCMFormatedData,
                        innerRadius: 90,
                        outerRadius: 10,
                        paddingAngle: 5,
                        cornerRadius: 0,
                        startAngle: -180,
                        endAngle: 180,
                        cx: 120,
                        cy: 180,
                        highlightScope: {
                          faded: "global",
                          highlighted: "item",
                        },
                        faded: {
                          innerRadius: 80,
                          additionalRadius: -80,
                          color: "gray",
                        },
                      },
                    ]}
                    onItemClick={handleClickType}
                    height={400}
                    slotProps={{
                      legend: {
                        direction: "column",
                        // itemMarkWidth: 24,
                        // itemMarkHeight: 2,
                        // markGap: 5,
                        // itemGap: 5,
                        position: { horizontal: "right", vertical: "middle" },
                        labelStyle: {
                          fontSize: 12,
                          fontWeight: "bolder",
                          // fill: 'green',
                        },
                      },
                    }}
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
              {plannedActivityData &&
              plannedActivitiesFormatedData &&
              expiredPlannedActivitiesFormatedData &&
              expiredPlannedActivitiesFormatedData?.length &&
              plannedActivitiesFormatedData?.length ? (
                <>
                  <Paper>
                    <Typography className="px-2 py-2">
                      Planned Activities by Year
                    </Typography>
                  </Paper>
                  <BarChart
                    // dataset={plannedActivitiesFormatedData}
                    xAxis={[
                      {
                        scaleType: "band",
                        data: plannedActivitiesFormatedData?.map(
                          (data) => data[0]
                        ),
                        // categoryGapRatio: 0.6,
                        // barGapRatio: 0.5,
                      },
                    ]}
                    series={[
                      {
                        data: expiredPlannedActivitiesFormatedData?.map(
                          (data) => data[1]
                        ),
                        label: "Expired Planned Activities",
                        stack: "Planned Activities",
                        color: "#DD0000",
                      },
                      {
                        data: plannedActivitiesFormatedData?.map((data) =>
                          data[1] ? data[1] : ""
                        ),
                        label: "Planned Completion",
                        stack: "Planned Activities",
                        color: "#02B2AF",
                      },
                    ]}
                    //   layout="vertical"
                    height={400}
                    onAxisClick={handleClickCompletionYear}
                    onItemClick={handlePAYearClick}
                    className="border-end border-dark"
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
          </Stack>

          {/* <hr
            style={{
              margin: 0,
              padding: 0,
              height: "1px",
              background: `${darkMode ? "white" : "black"}`,
            }}
          />

          <Stack
            direction="row"
            width="100%"
            textAlign="left"
            spacing={2}
            // className={`${darkMode ? "border" : "border border-dark"}`}
            // style={{
            //   border: `${darkMode ? "2px solid white" : "2px solid black"}`,
            // }}
          >
            {plannedActivityData &&
            expiredPlannedActivitiesFormatedData &&
            expiredPlannedActivitiesFormatedData?.length ? (
              <>
                <Box width="50%">
                  <Paper>
                    <Typography className="px-2 py-2">Expired PA</Typography>
                  </Paper>
                  <BarChart
                    dataset={expiredPlannedActivitiesFormatedData}
                    xAxis={[
                      {
                        scaleType: "band",
                        dataKey: `${[0]}`,
                        // categoryGapRatio: 0.6,
                        // barGapRatio: 0.5,
                      },
                    ]}
                    series={[
                      {
                        dataKey: `${[1]}`,
                        label: "Planned Activities",
                        color: "#DD0000",
                      },
                    ]}
                    height={400}
                    onAxisClick={handleClickExpired}
                    className="border-end border-dark"
                  />
                </Box>
              </>
            ) : (
              ""
            )}
            <div
              style={{
                width: "2px",
                background: `${darkMode ? "white" : "black"}`,
              }}
            ></div>

            {plannedActivityData &&
            plannedActivitiesLCMFormatedData &&
            plannedActivitiesLCMFormatedData?.length ? (
              <>
                <Box width="50%">
                 
                </Box>
              </>
            ) : (
              ""
            )}
          </Stack> */}
        </div>
      ) : (
        <></>
      )}
    </ThemeProvider>
  );
};

export default Chart;
