import React, { useEffect, useRef } from "react";
import * as echarts from "echarts/core";
import {
  TitleComponent,
  TitleComponentOption,
  TooltipComponentOption,
} from "echarts/components";
import { PieChart, PieSeriesOption } from "echarts/charts";
import { CanvasRenderer } from "echarts/renderers";
import { useTheme } from "../../Context/ThemeContext";

// Register required ECharts components
echarts.use([TitleComponent, PieChart, CanvasRenderer]);

type EChartsOption = echarts.ComposeOption<
  TitleComponentOption | PieSeriesOption
>;

const PieChartComponent = ({
  seriesData,
  height,
  width,
  padding,
  isDarkMode,
  filterCallBack,
}: {
  seriesData?: any;
  height?: string;
  width?: string;
  padding?: string;
  isDarkMode?: boolean;
  filterCallBack?: any;
}) => {
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

  // Generate unique colors for the series
  const generateUniqueColors = (count: number) => {
    const colors = new Set<string>();
    while (colors.size < count) {
      colors.add(generateRandomColor());
    }
    return Array.from(colors);
  };

  const colors = generateUniqueColors(seriesData?.length || 0);

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
    backgroundColor: isDarkMode ? "#000000" : "transparent",
    series: {
      type: "pie",
      data: seriesData || [],
      radius: "50%",
      emphasis: {
        focus: "self",
      },
      selectedMode: "single",
      label: {
        show: true,
        position: "outside",
        formatter: "{b}: {c} ({d}%)",
      },
      itemStyle: {
        borderWidth: 2,
      },
      color: colors,
    },
    tooltip: {
      trigger: "item",
      formatter: "{b}: {c} ({d}%)",
    },
  };

  useEffect(() => {
    const chartDom = chartRef.current;
    if (!chartDom) return;

    const myChart = echarts.init(chartDom, isDarkMode ? "dark" : "auto");
    myChart.setOption(option);

    myChart.on("click", (params) => {
      if (params.componentType === "series") {
        filterCallBack(params);
      }
    });

    return () => {
      myChart.dispose();
    };
  }, [option, isDarkMode]);

  return (
    <div
      ref={chartRef}
      style={{
        width: width || "100%",
        height: height || "400px",
        padding,
      }}
    />
  );
};

export default PieChartComponent;
