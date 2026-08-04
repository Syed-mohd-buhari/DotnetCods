import React, { useEffect, useRef, useMemo, useState } from "react";
import * as echarts from "echarts/core";
import {
  ToolboxComponent,
  TooltipComponent,
  GridComponent,
  LegendComponent,
  DataZoomComponent,
} from "echarts/components";
import { GaugeChart, GaugeSeriesOption } from "echarts/charts";
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

echarts.use([GaugeChart, CanvasRenderer]);

type EChartsOption = echarts.ComposeOption<GaugeSeriesOption>;

interface EChartProps {
  width?: string;
  height?: string;
  padding?: string;
}

const EChartComponent: React.FC<EChartProps> = ({ width, height, padding }) => {
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
  //   const colors = generateUniqueColors(seriesData.length);

  useEffect(() => {
    if (!chartRef.current) return;
    const chartDom = chartRef.current;
    const myChart = echarts.init(chartDom, darkMode ? "dark" : "auto");
    const option: EChartsOption = {
      series: [
        {
          type: "gauge",
          axisLine: {
            lineStyle: {
              width: 30,
              color: [
                [0.3, "#67e0e3"],
                [0.7, "#37a2da"],
                [1, "#fd666d"],
              ],
            },
          },
          pointer: {
            itemStyle: {
              color: "auto",
            },
          },
          axisTick: {
            distance: -30,
            length: 8,
            lineStyle: {
              color: "#fff",
              width: 2,
            },
          },
          splitLine: {
            distance: -30,
            length: 30,
            lineStyle: {
              color: "#fff",
              width: 4,
            },
          },
          axisLabel: {
            color: "inherit",
            distance: 40,
            fontSize: 20,
          },
          detail: {
            valueAnimation: true,
            formatter: "{value}%",
            color: "inherit",
          },
          data: [
            {
              value: 70,
            },
          ],
        },
      ],
    };
    myChart.setOption(option);
    // Update the chart data every 2 seconds
    const interval = setInterval(() => {
      myChart.setOption<EChartsOption>({
        series: [
          {
            data: [
              {
                value: +(Math.random() * 100).toFixed(2),
              },
            ],
          },
        ],
      });
    }, 2000);

    // Clean up on component unmount
    return () => {
      clearInterval(interval);
      myChart.dispose();
    };
  }, []);

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
