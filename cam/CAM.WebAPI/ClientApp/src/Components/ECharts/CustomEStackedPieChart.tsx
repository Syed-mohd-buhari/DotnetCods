import React, { useEffect, useRef } from "react";
import * as echarts from "echarts/core";
import {
  DataZoomComponentOption,
  GridComponentOption,
  LegendComponentOption,
  TitleComponent,
  TitleComponentOption,
  ToolboxComponentOption,
  TooltipComponentOption,
} from "echarts/components";
import { SunburstChart, SunburstSeriesOption } from "echarts/charts";
import { CanvasRenderer } from "echarts/renderers";
import { useTheme } from "../../Context/ThemeContext";

// Register required ECharts components
echarts.use([TitleComponent, SunburstChart, CanvasRenderer]);

// Type for ECharts option combining the necessary components
type EChartsOption = echarts.ComposeOption<
  TitleComponentOption | SunburstSeriesOption
>;

interface SunburstChartProps {
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
}

const SunburstChartComponent = ({
  seriesData,
  legendData,
  height,
  width,
  padding,
  isDarkMode,
}: {
  seriesData: any;
  legendData?: any;
  height?: string;
  width?: string;
  padding?: string;
  isDarkMode?: boolean;
}) => {
  const chartRef = useRef<HTMLDivElement>(null);
  const { darkMode } = useTheme();
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
  const option: EChartsOption = {
    title: {
      textStyle: {
        fontSize: 14,
        align: "center",
      },
      subtextStyle: {
        align: "center",
      },
    },
    series: {
      type: "sunburst",
      data: seriesData,
      radius: [0, "90%"],
      sort: undefined,
      emphasis: {
        focus: "ancestor",
      },
      levels: [
        {},
        {
          r0: "15%",
          r: "35%",
          itemStyle: {
            borderWidth: 2,
          },
          label: {
            show: true,
            position: "inside",
            padding: 9,
            silent: false,
          },
        },
        {
          r0: "35%",
          r: "60%",
          label: {
            show: true,
            position: "outside",
            padding: 3,
            silent: false,
          },
        },
      ],
    },
  };

  useEffect(() => {
    const chartDom = chartRef.current;
    if (!chartRef.current) return;
    const myChart = echarts.init(chartDom, isDarkMode ? "dark" : "auto");
    myChart.setOption(option);

    // Cleanup function to dispose of the chart instance
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

export default SunburstChartComponent;
