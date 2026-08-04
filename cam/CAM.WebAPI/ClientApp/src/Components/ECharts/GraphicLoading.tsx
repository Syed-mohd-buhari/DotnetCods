import React, { useEffect, useMemo, useRef, useState } from "react";
import * as echarts from "echarts/core";
import { GraphicComponent, GraphicComponentOption } from "echarts/components";
import { CanvasRenderer } from "echarts/renderers";
import { useTheme } from "../../Context/ThemeContext";

echarts.use([GraphicComponent, CanvasRenderer]);
type EChartsOption = echarts.ComposeOption<GraphicComponentOption>;

const GraphicLoading = (props) => {
  const chartRef = useRef<HTMLDivElement>(null);
  const { darkMode } = useTheme();

  const option: EChartsOption = {
    graphic: {
      elements: [
        {
          type: "group",
          left: "center",
          top: "center",
          children: new Array(7).fill(0).map((val, i) => ({
            type: "rect",
            x: i * 20,
            shape: {
              x: 0,
              y: -40,
              width: 10,
              height: 60,
            },
            style: {
              fill: "#5470c6",
            },
            keyframeAnimation: {
              duration: 1500,
              delay: i * 300,
              loop: true,
              keyframes: [
                {
                  percent: 0.5,
                  scaleY: 0.3,
                  easing: "cubicIn",
                },
                {
                  percent: 1,
                  scaleY: 1,
                  easing: "cubicOut",
                },
              ],
            },
          })),
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

export default GraphicLoading;
