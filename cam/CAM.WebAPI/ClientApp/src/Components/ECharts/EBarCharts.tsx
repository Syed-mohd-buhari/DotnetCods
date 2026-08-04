import React, { useEffect, useRef, useState } from "react";
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

const EChartComponent = (chartData: any) => {
  const chartRef = useRef<HTMLDivElement>(null);
  const [isReady, setIsReady] = useState(false);

  useEffect(() => {
    if (!chartRef.current) return;

    const resizeObserver = new ResizeObserver(() => {
      setIsReady(true); // Trigger chart initialization when size is available
    });

    resizeObserver.observe(chartRef.current);

    return () => {
      resizeObserver.disconnect();
    };
  }, []);

  useEffect(() => {
    if (!chartRef.current || !isReady) return;

    const chartDom = chartRef.current;
    const myChart = echarts.init(chartDom);
    const option: EChartsOption = {
      tooltip: {
        trigger: "axis",
        axisPointer: {
          type: "shadow",
          label: {
            show: true,
          },
        },
      },
      toolbox: {
        show: true,
        feature: {
          mark: { show: true },
          saveAsImage: { show: true },
        },
      },
      legend: {
        data: ["Planned", "Actual", "Target"],
        itemGap: 5,
      },
      grid: {
        top: "12%",
        left: "1%",
        right: "10%",
        containLabel: true,
      },
      xAxis: [
        {
          type: "category",
          data: chartData?.namesList,
        },
      ],
      yAxis: [
        {
          type: "value",
          min: 0,
          max: 100,
        },
      ],
      dataZoom: [
        {
          show: true,
          start: 0,
          end: 10,
          showDetail: false,
        },
        {
          show: false,
          type: "inside",
          start: 0,
          end: 4,
        },
        {
          show: true,
          yAxisIndex: 0,
          start: 0,
          end: 6,
          filterMode: "empty",
          width: 30,
          height: "80%",
          showDetail: false,
          showDataShadow: false,
          left: "93%",
        },
      ],
      series: chartData?.seriesData,
    };

    myChart.setOption(option);
    return () => {
      myChart.dispose();
    };
  }, []);

  return (
    <div
      ref={chartRef}
      style={{ width: "auto", height: "606px", padding: "0 3rem" }}
    />
  );
};

export default EChartComponent;
