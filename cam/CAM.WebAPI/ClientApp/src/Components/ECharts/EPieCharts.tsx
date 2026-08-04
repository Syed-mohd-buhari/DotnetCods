import React, { useEffect, useRef } from "react";
import * as echarts from "echarts/core";
import {
  TitleComponent,
  TooltipComponent,
  LegendComponent,
} from "echarts/components";
import { PieChart } from "echarts/charts";
import { LabelLayout } from "echarts/features";
import { CanvasRenderer } from "echarts/renderers";
import "../Charts/chartStyle.css";

echarts.use([
  TitleComponent,
  TooltipComponent,
  LegendComponent,
  PieChart,
  CanvasRenderer,
  LabelLayout,
]);

const MyPieChart = ({
  seriesData,
  legendData,
}: {
  seriesData: any;
  legendData: any;
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
  // Generate a list of unique colors for the series data
  const generateUniqueColors = (count: number) => {
    const colors = new Set<string>();
    while (colors.size < count) {
      colors.add(generateRandomColor());
    }
    return Array.from(colors);
  };

  useEffect(() => {
    const chartDom = chartRef.current;
    const myChart = echarts.init(chartDom as HTMLDivElement);
    const colors = generateUniqueColors(seriesData.length);

    const option = {
      // title: {
      //   text: "Name Count Statistics",
      //   subtext: "Purely Fictional",
      //   left: "center",
      // },
      tooltip: {
        trigger: "item",
        formatter: "{b} : {c}%",
      },
      legend: {
        type: "scroll",
        orient: "vertical",
        right: 20,
        top: 30,
        bottom: 20,
        data: legendData,
      },
      series: [
        {
          name: "",
          type: "pie",
          radius: "55%",
          center: ["40%", "50%"],
          data: seriesData.map((item: any, index: number) => ({
            ...item,
            itemStyle: { color: colors[index] }, // Assign each item a unique color
          })),

          emphasis: {
            itemStyle: {
              shadowBlur: 10,
              shadowOffsetX: 0,
              shadowColor: "rgba(0, 0, 0, 0.5)",
            },
          },
        },
      ],
    };
    myChart.setOption(option);

    return () => {
      myChart.dispose();
    };
  }, []);

  return (
    <div
      ref={chartRef}
      style={{
        width: "1457px",
        height: "480px",
        padding: "0 3rem",
        justifySelf: "center",
        inset: "center",
      }}
    />
  );
};

export default MyPieChart;
