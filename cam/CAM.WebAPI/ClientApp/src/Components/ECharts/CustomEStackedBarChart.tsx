import React, { useEffect, useRef, useMemo, useState } from "react";
import * as echarts from "echarts/core";
import {
  ToolboxComponent,
  TooltipComponent,
  GridComponent,
  LegendComponent,
  DataZoomComponent,
} from "echarts/components";
import { BarChart, BarSeriesOption } from "echarts/charts";
import { CanvasRenderer } from "echarts/renderers";
import type {
  ToolboxComponentOption,
  TooltipComponentOption,
  GridComponentOption,
  LegendComponentOption,
  DataZoomComponentOption,
} from "echarts/components";
import { XAXisOption, YAXisOption } from "echarts/types/dist/shared";
import { useTheme } from "../../Context/ThemeContext";
import { safeNumber } from "../../Hook/Common";

echarts.use([
  ToolboxComponent,
  TooltipComponent,
  GridComponent,
  LegendComponent,
  DataZoomComponent,
  BarChart,
  CanvasRenderer,
]);

type EChartsOption = echarts.ComposeOption<
  | ToolboxComponentOption
  | TooltipComponentOption
  | GridComponentOption
  | LegendComponentOption
  | DataZoomComponentOption
  | BarSeriesOption
>;

interface EChartProps {
  namesList?: string[];
  seriesData: any;
  width?: string;
  height?: string;
  padding?: string;
  tooltip?: TooltipComponentOption;
  yAXis?: any;
  xAXis?: any;
  toolbox?: ToolboxComponentOption;
  legend?: LegendComponentOption;
  grid?: GridComponentOption;
  dataZoom?: DataZoomComponentOption[];
  isDarkMode?: boolean;
  colorMap?: any;
  rawData: any;
}

const EChartComponent: React.FC<EChartProps> = ({
  namesList,
  seriesData,
  xAXis,
  yAXis,
  width,
  height,
  padding,
  tooltip,
  toolbox,
  legend,
  grid,
  dataZoom,
  isDarkMode,
  colorMap,
  rawData,
}) => {
  const [rowData, setRowData] = useState(rawData);
  const { darkMode } = useTheme();

  useEffect(() => {
    if (rawData) {
      setRowData(rawData);
    }
  }, [rawData]);

  // console.log("colorMap", colorMap, rawData);

  const chartRef = useRef<HTMLDivElement>(null);
  const totalData: number[] = [];
  for (let i = 0; i < rowData[0]?.length; ++i) {
    let sum = 0;
    for (let j = 0; j < rowData?.length; ++j) {
      sum += rowData[j][i];
    }
    totalData.push(sum);
  }

  const getColor = (name) => colorMap[name.toLowerCase()] || null;

  const series: any[] =
    legend?.data?.map((name, sid) => {
      return {
        name,
        type: "bar",
        cursor: "default",
        stack: "total",
        barWidth: "60%",
        color: getColor(name),
        label: {
          show: true,
          formatter: (params: any) =>
            params.value !== 0 ? `${params.value}%` : "",
        },
        data: rawData?.[sid],
      };
    }) ?? [];

  const option: EChartsOption = useMemo(
    () => ({
      darkMode: true,
      tooltip: tooltip || {},
      toolbox: toolbox || {
        show: true,
        feature: {
          mark: { show: true },
          saveAsImage: { show: true },
        },
        right: "6%",
        top: "2%",
      },
      legend: legend,
      grid: grid || {
        top: "12%",
        left: "1%",
        right: "10%",
        containLabel: true,
      },
      xAxis: xAXis || [
        {
          type: "category",
        },
      ],
      yAxis: yAXis || [
        {
          type: "value",
        },
      ],
      dataZoom: dataZoom || [
        { show: true, start: 0, end: 6 },
        {
          show: false,
          type: "inside",
          start: 0,
          end: 4,
        },
        {
          show: true,
          yAxisIndex: 0,
          filterMode: "empty",
          width: 30,
          height: "80%",
          showDataShadow: false,
          left: "93%",
        },
      ],
      series: series,
    }),
    [namesList, seriesData, tooltip, toolbox, legend, grid, dataZoom]
  );

  useEffect(() => {
    const chartDom = chartRef.current;
    if (!chartRef.current) return;

    const myChart = echarts.init(chartDom, isDarkMode ? "dark" : "auto");
    myChart.setOption(option);
    return () => {
      myChart.dispose();
    };
  }, [option]);

  return (
    <div
      ref={chartRef}
      style={{
        width: "auto",
        height: height === "100%" ? "100vh" : height,
        padding,
      }}
    />
  );
};

export default EChartComponent;
