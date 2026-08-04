import React, { useCallback, useState } from "react";
import "./TreeViewMenu.css";
import WhatsGoingOn from "../../Containers/landingPage/WhatsGoingOn";
import QuickLinkNew from "../../Containers/landingPage/QuickLinkNew";
import { Box } from "@mui/material";
import Messages from "../../Containers/landingPage/Messages";
import { useAuth } from "../../Hook/useAuth";

const TreeView: React.FC = () => {
  const [triggerMessageFetch, setTriggerMessageFetch] = useState<number>(0);
  const [triggerWhatsGoingFetch, setTriggerWhatsGoingFetch] =
    useState<number>(0);
  const [section, setSection] = useState<any>(null);

  const handleMonthRangeChange = useCallback((data: any) => {
    setSection(data?.section);
    setMonthRangeData(
      data?.section === "1"
        ? { messagingDate: data?.dateRange ?? 0 }
        : { whatsGoingOnDate: data?.dateRange ?? 0 }
    );
  }, []);

  const [monthRangeData, setMonthRangeData] = useState<any>(null);

  const handleApiSuccess = () => {
    section === "1" && setTriggerMessageFetch((prev) => prev + 1);
    section === "2" && setTriggerWhatsGoingFetch((prev) => prev + 1);
  };

  return (
    <>
      <div>
        <Box
          sx={{
            minHeight: "70vh",
            fontFamily: "VodafoneRg !important",
            marginLeft: "65px",
            marginRight: "10px",
            marginTop: "30px",
            marginBottom: "20px",
          }}
        >
          <Box
            sx={{
              display: "flex",
              gap: 2,
              alignItems: "flex-start",
            }}
          >
            <QuickLinkNew
              monthRangeData={monthRangeData}
              onApiSuccess={handleApiSuccess}
            />
            <Box
              sx={{
                flex: 1,
                display: "flex",
                flexDirection: "column",
                gap: 2,
              }}
            >
              <Messages
                onMonthRangeChange={handleMonthRangeChange}
                triggerFetch={triggerMessageFetch}
              />
              <WhatsGoingOn
                onMonthRangeChange={handleMonthRangeChange}
                triggerFetch={triggerWhatsGoingFetch}
              />
            </Box>
          </Box>
        </Box>
      </div>
    </>
  );
};

export default TreeView;
