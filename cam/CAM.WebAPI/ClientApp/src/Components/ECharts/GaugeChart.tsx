import React, { useEffect, useMemo, useRef, useState } from "react";
import * as echarts from "echarts/core";
import { useTheme } from "../../Context/ThemeContext";
import { GaugeChart, GaugeSeriesOption } from "echarts/charts";
import { CanvasRenderer } from "echarts/renderers";
import { TitleComponent, TitleComponentOption } from "echarts/components";

echarts.use([TitleComponent, GaugeChart, CanvasRenderer]);

type EChartsOption = echarts.ComposeOption<
  GaugeSeriesOption | TitleComponentOption
>;
const GaugeCharts = (props) => {
  const chartRef = useRef<HTMLDivElement>(null);
  const { darkMode } = useTheme();

  // Normalize colorData to sum up to 1 (100%)
  const total =
    props?.colorData?.reduce((sum, [value]) => sum + value, 0) ?? 100;

  // Generate the cumulative color data with normalization
  let cumulative = 0;
  const normalizedColorData = props?.colorData?.map(([value, color]) => {
    cumulative += value / total; // Accumulate the value
    return [cumulative, color]; // Return the cumulative value and color
  }) ?? [
    [0.3, "#00c300"],
    [0.7, "#FFFF00"],
    [1, "#e60000"],
  ];

  const option: EChartsOption = {
    // title: [{ text: "LCM Compliance", left: "left" }],
    series: [
      {
        type: "gauge",
        radius: "80%",
        axisLine: {
          lineStyle: {
            width: 10,
            color: normalizedColorData,
          },
        },
        pointer: {
          itemStyle: {
            color: "auto",
          },
        },
        axisTick: {
          distance: -20,
          length: 8,
          lineStyle: {
            color: "#fff",
            width: 2,
          },
        },
        splitLine: {
          distance: -26,
          length: 30,
          lineStyle: {
            color: "#fff",
            width: 2,
          },
        },
        axisLabel: {
          color: "inherit",
          distance: 20,
          fontSize: 10,
        },
        detail: {
          valueAnimation: true,
          formatter: "{value}%",
          color: "inherit",
          fontSize: 20,
        },
        data: props?.data ?? [
          {
            value: 70,
          },
        ],
      },
    ],
  };

  useEffect(() => {
    if (!chartRef.current) return;
    const chartDom = chartRef.current;
    const myChart = echarts.init(chartDom, "dark");
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
        height: props?.height === "100%" ? "100vh" : props?.height,
        bottom: 0, // Align the chart at the bottom
      }}
    />
  );
};

export default GaugeCharts;
