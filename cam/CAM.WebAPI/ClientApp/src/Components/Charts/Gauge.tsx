import { Box } from "@mui/material";
import React from "react";
import GaugeComponent from "react-gauge-component";

const Gauge = ({
  red,
  green,
  yellow,
  selectedOpco,
  colorData,
}: {
  red: any;
  green: any;
  yellow?: any;
  selectedOpco: any;
  colorData?: any;
}) => {
  // console.log("colorData", colorData);
  const subArcs: any = [];
  let previousLimit = 0;

  // Iterate through colorData to construct subArcs
  colorData?.forEach(([value, color], index) => {
    const limit = previousLimit + value;

    // Validate if the limit is within bounds
    if (value > 0 && limit > previousLimit && limit <= 100) {
      subArcs.push({
        limit,
        color,
        showTick: false,
        tooltip: {
          text: `${[green, yellow, red][index] || ""}%`, // Dynamically map the percentage to the tooltip
        },
      });
      previousLimit = limit; // Update previousLimit
    }
  });

  return (
    <Box sx={{ width: "100%", padding: 0, margin: 0 }}>
      <GaugeComponent
        style={{
          width: "100%",
          height: `${selectedOpco ? "500px" : ""}`,
          padding: "0",
          margin: "0",
        }}
        type="semicircle"
        arc={{
          width: 0.2,
          padding: 0.005,
          cornerRadius: 1,
          // gradient: true,
          subArcs: subArcs,
        }}
        pointer={{
          color: "#345243",
          length: 0.8,
          width: 10,
          // elastic: true,
        }}
        labels={{
          valueLabel: { formatTextValue: (value) => value + "%" },
          tickLabels: {
            type: "outer",
            defaultTickValueConfig: {
              formatTextValue: (value: any) => value + "%",
              // style: { fontSize: 8 },
            },
            defaultTickLineConfig: {
              distanceFromArc: 1,
              length: 3,
            },
          },
        }}
        value={green}
        minValue={0}
        maxValue={100}
      />
    </Box>
  );
};

export default Gauge;
