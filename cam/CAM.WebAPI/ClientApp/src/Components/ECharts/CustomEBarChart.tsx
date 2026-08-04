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
  seriesData: any[];
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
  onBarClick?: (data: { name: string; value: number; color: string }) => void; // Callback for bar click
  onLegendClick?: (data: {
    name: string;
    isSelected: boolean;
    selected: any[];
  }) => void; // Callback for legend interaction
  onAxisLabelClick?: (index: string, label: string) => void; // Callback for xAxis label click
  setXAxisWidth?: (any) => void;
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
  onBarClick,
  onLegendClick,
  onAxisLabelClick,
  setXAxisWidth,
}) => {
  const { darkMode } = useTheme();
  const chartRef = useRef<HTMLDivElement>(null);
  // Function to generate random hex colors
  const generateRandomColor = () => {
    const letters = "0123456789ABCDEF";
    let color = "#";
    for (let i = 0; i < 6; i++) {
      color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
  };
  // Generate a list of unique colors for the series data
  const generateUniqueColors = (count: number) => {
    const colors = new Set<string>();
    while (colors.size < count) {
      colors.add(generateRandomColor());
    }
    return Array.from(colors);
  };
  const colors = generateUniqueColors(seriesData.length);
  const option: EChartsOption = useMemo(
    () => ({
      darkMode: true,
      tooltip: tooltip || {
        trigger: "axis",
        axisPointer: {
          type: "shadow",
          label: { show: true },
        },
      },
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
        { show: true, start: 0, end: 50 },
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
      series: seriesData.map((data) => {
        return {
          ...data,
          label: {
            ...data.label,
            formatter: (params: any) =>
              params.value !== 0 ? `${params.value}%` : "",
          },
          itemStyle: {
            ...data.itemStyle,
            opacity: 0.8,
            borderRadius: [6, 6, 0, 0],
          },
        };
      }),
    }),
    [namesList, seriesData, tooltip, toolbox, legend, grid, dataZoom]
  );

  useEffect(() => {
    if (!chartRef.current) return;

    // Initialize the chart
    const chart = echarts.init(chartRef.current, isDarkMode ? "dark" : "auto");
    chart.setOption(option);

    const categories = xAXis?.[0]?.data || [];
    if (categories.length) {
      const startPixel = chart.convertToPixel({ xAxisIndex: 0 }, 0);
      const endPixel = chart.convertToPixel(
        { xAxisIndex: 0 },
        categories.length
      );
      const xAxisWidth = endPixel - startPixel;
      if (setXAxisWidth) setXAxisWidth(xAxisWidth);
    }

    // Handle xAxis label click
    if (onAxisLabelClick) {
      // Add a click event listener
      chart.on(
        "click",
        (params: any) => {
          if (
            params.componentType === "xAxis" &&
            params.targetType === "axisLabel"
          ) {
            const formattedValue = `${
              xAXis[0]?.data?.[safeNumber(params.value) - 1]
            }`;
            // console.log(
            //   "Clicked xAxis label:",
            //   xAXis?.[0].data,
            //   formattedValue,
            //   safeNumber(params.value) - 1
            // );
            // Pass the label value to a callback or handle it here
            if (onAxisLabelClick) {
              onAxisLabelClick(params.value, formattedValue); // Call the user-defined function with the label
            }
          }
        },
        { passive: true }
      );
    }
    // Add click event listener for bars
    if (onBarClick) {
      chart.on(
        "click",
        (params: echarts.ECElementEvent) => {
          if (params && params.name && params.value !== undefined) {
            onBarClick({
              name: params.name as string,
              value: params.value as number,
              color: params.color as string, // Retrieve the color of the clicked bar
            });
          }
        },
        { passive: true }
      );
    }
    // Add legend interaction listener
    if (onLegendClick) {
      chart.on(
        "legendselectchanged",
        (params: any) => {
          const option = chart.getOption();
          const selected = option.legend?.[0]?.selected || {};
          const selectedList = Object.keys(params.selected).filter(
            (key) => selected[key]
          );
          onLegendClick({
            name: params.name,
            isSelected: selected[params.name],
            selected: selected,
          });
        },
        { passive: true }
      );
    }

    // Cleanup the chart and event listeners
    return () => {
      chart.off("click");
      chart.off("legendselectchanged");
      chart.dispose();
    };
  }, [option, onBarClick, onLegendClick, isDarkMode, onAxisLabelClick]);

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
