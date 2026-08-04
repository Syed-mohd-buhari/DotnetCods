import React, { useEffect, useState } from "react";
import Tabs from "@mui/material/Tabs";
import Tab from "@mui/material/Tab";
import Box from "@mui/material/Box";

interface ScrollableTabsProps {
  tabs: {
    label: string;
    component: React.ReactNode;
  }[]; // Array of tabs with labels and components
  dataType: number; // Pass the dataType to reset value when it changes
  onTabChange?: (tab: { label: string; component: React.ReactNode }) => void; // Send tab object including dataType
}

const ScrollableTabs: React.FC<ScrollableTabsProps> = ({
  tabs,
  dataType,
  onTabChange,
}) => {
  const [value, setValue] = useState(0);

  useEffect(() => {
    setValue(0); // Reset value to 0 when tabs change
  }, [dataType]);

  const handleChange = (event: React.SyntheticEvent, newValue: number) => {
    if (newValue >= 0 && newValue < tabs.length) {
      setValue(newValue); // Set the new tab value when the user clicks a tab
      onTabChange && onTabChange(tabs[newValue]);
    }
  };

  return (
    <Box sx={{ bgcolor: "background.paper" }}>
      {tabs.length > 0 && (
        <Tabs
          value={value}
          onChange={handleChange}
          variant="scrollable"
          scrollButtons="auto"
          aria-label="scrollable auto tabs example"
          sx={{
            "& .MuiTabs-indicator": { backgroundColor: "red" },
            "& .MuiTab-root": {
              fontWeight: "bold",
              color: "black",
              textAlign: "left !important",
            },
            "& .Mui-selected": {
              color: "red !important",
              fontWeight: "bold",
            },
          }}
        >
          {tabs.map((tab, index) => (
            <Tab
              key={index}
              label={tab.label}
              sx={{
                textTransform: "none",
                border: "none",
              }}
            />
          ))}
        </Tabs>
      )}
      <Box
        sx={{
          mt: 2,
          p: 2,
          border: "1px solid",
          borderColor: "divider",
          borderRadius: 1,
        }}
      >
        {tabs[value]?.component || <div>No content available</div>}{" "}
        {/* Safely render the component */}
      </Box>
    </Box>
  );
};

export default ScrollableTabs;
