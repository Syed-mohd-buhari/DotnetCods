import React, { useEffect, useMemo, useRef, useState } from "react";
import * as echarts from "echarts/core";
import { useTheme } from "../../Context/ThemeContext";
import { GraphicComponent, GraphicComponentOption } from "echarts/components";
import { CanvasRenderer } from "echarts/renderers";

echarts.use([GraphicComponent, CanvasRenderer]);

type EChartsOption = echarts.ComposeOption<GraphicComponentOption>;

const NoDataAnimation = (props) => {
  const chartRef = useRef<HTMLDivElement>(null);
  const { darkMode } = useTheme();

  const option: EChartsOption = {
    graphic: {
      elements: [
        {
          type: "text",
          left: "center",
          top: "center",
          style: {
            text: props?.message ?? "No Data Found",
            fontSize: props?.fontSize ?? 40,
            fontWeight: "bold",
            lineDash: [0, 200],
            lineDashOffset: 0,
            fill: "transparent",
            stroke: darkMode ? "#fff" : "#000",
            lineWidth: 1,
          },
          keyframeAnimation: {
            duration: 3000,
            loop: false,
            keyframes: [
              {
                percent: 0.7,
                style: {
                  fill: "transparent",
                  lineDashOffset: 200,
                  lineDash: [200, 0],
                },
              },
              {
                // Stop for a while.
                percent: 0.8,
                style: {
                  fill: "transparent",
                },
              },
              {
                percent: 1,
                style: {
                  fill: "black",
                },
              },
            ],
          },
        },
      ],
    },
  };

  useEffect(() => {
    if (!chartRef.current) return;
    const chartDom = chartRef.current;
    const myChart = echarts.init(chartDom, props?.isDarkMode ? "dark" : "auto");
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
        padding: props?.padding ?? "",
      }}
    />
  );
};

export default NoDataAnimation;
