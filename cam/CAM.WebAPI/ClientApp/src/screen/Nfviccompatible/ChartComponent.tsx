import React, { useEffect, useState } from "react";
import { Box, Paper, Stack, Typography } from "@mui/material";
import { BarChart, PieChart } from "@mui/x-charts";
import { GetNFVICompatibilityReport } from "../../Redux/Action/Nfviccompatible/NfvicCompatibilityReportAction";
import {
  NFVICompatibilityReportRequest,
  NFVIStatus,
} from "../../Model/NfvicCompatible";
import { HighlightItemData } from "@mui/x-charts/context";
import LabelsDictionary from "../../Constant/LabelsAndDescriptions.json";

type Props = {
  vmVarId: number;
  selectedMarketId?: number | null;
  selectedVendorId?: number | null;
  selectedVerticalId?: number | null;
  selectedVodafoneId?: number | null;
  darkMode?: boolean | undefined;
};

const NVFChart = ({
  vmVarId,
  selectedMarketId,
  selectedVendorId = null,
  selectedVerticalId = null,
  selectedVodafoneId = null,
  darkMode,
}: Props) => {
  const [opcoData, setOpcoData] = useState<any[]>([]);
  const [vendorData, setVendorData] = useState<any[]>([]);
  const [verticalData, setVerticalData] = useState<any[]>([]);
  const [vfNameData, setVfNameData] = useState<any[]>([]);
  const [vfData, setVfData] = useState<any[]>([]);
  const [plannedCompletionData, setPlannedCompletionData] = useState<any[]>([]);
  const [highlightedOpCoItem, setHighlightedOpCoItem] =
    useState<HighlightItemData | null>(null);
  const openWithFilters = (
    pathname: string,
    newParams: Record<string, string | number | undefined>
  ) => {
    const queryParams = new URLSearchParams(window.location.search);
    Object.entries(newParams).forEach(([key, value]) => {
      if (value !== undefined && value !== null) {
        queryParams.set(key, value.toString());
      }
    });
    window.open(`${pathname}?${queryParams.toString()}`, "_blank");
  };

  useEffect(() => {
    const fetchReport = async () => {
      const request: NFVICompatibilityReportRequest = {
        page: 0,
        pageSize: 0,
        sortBy: "",
        isSortAscending: true,
        deleted: true,
        orphan: true,
        principalId: 0,
        market: selectedMarketId ? [selectedMarketId] : [],
        oemId: selectedVendorId !== null ? [selectedVendorId] : [],
        domain: selectedVerticalId ? [selectedVerticalId] : [],
        vodafoneNameId: selectedVodafoneId !== null ? [selectedVodafoneId] : [],
        application: [],
        designComponent: [],
        currentVNF: [],
        minimumVNF: [],
        plannedVNF: [],
        plannedUpgrade: undefined,
        status: [],
        deleiveryStatus: [],
        eduSpoc: [],
        subDomainSpoc: [],
        lastModified: undefined,
        lastModifiedBy: [],
        lastModifiedValue: undefined,
      };

      const data = await GetNFVICompatibilityReport(vmVarId, request);

      setVendorData(
        (data?.vendorBasedNFVIStatus ?? []).map((d: NFVIStatus) => ({
          component: d.vendorName ?? "Unknown",
          activities: d.overAllStatusCount,
          vendorId: d.vendorId,
          vendorName: d.vendorName,
        }))
      );

      setOpcoData(
        (data?.opcoBasedNFVIStatus ?? []).map((d: NFVIStatus) => ({
          market: d.opco ?? "Unknown",
          activities: d.overAllStatusCount,
          opCoId: d.opCoId,
          opco: d.opco,
        }))
      );

      setVerticalData(
        (data?.verticalBasedNFVIStatus ?? []).map((d: NFVIStatus) => {
          const matchedVertical = d.verticalIdDto?.find(
            (v) => v.text === d.verticalName
          );
          return {
            vertical: d.verticalName ?? "Unknown",
            activities: d.overAllStatusCount,
            verticalId: matchedVertical
              ? Number(matchedVertical.value)
              : d.verticalId,
          };
        })
      );

      setVfNameData(
        (data?.vfBasedNFVIStatus ?? []).map((item: NFVIStatus) => ({
          vertical: item.vodafoneName ?? "Unknown",
          activities: item.overAllStatusCount ?? 0,
          vodafoneId: item.vodafoneNameId,
        }))
      );

      setVfData(convertToPieData(data?.overAllNFVIStatus));
      setPlannedCompletionData(
        convertToPieDataWith(data?.plannedCompletionBasedNFVIStatus)
      );
    };

    if (vmVarId) {
      fetchReport();
    }
  }, [
    vmVarId,
    selectedMarketId,
    selectedVendorId,
    selectedVerticalId,
    selectedVodafoneId,
  ]);

  const convertToPieData = (obj: any) => {
    if (!obj) return [];
    return Object.entries(obj)
      .filter(([k, v]) => typeof v === "number" && v > 0 && !k.includes("id"))
      .map(([key, value]) => ({
        label: LabelsDictionary[key]?.Full ?? key, // Use mapped label if available
        value,
      }));
  };

  const convertToPieDataWith = (obj: any) => {
    if (!obj) return [];
    return Object.entries(obj)
      .filter(
        ([k, v]) =>
          typeof v === "number" && v > 0 && !k.toLowerCase().endsWith("id")
      )
      .map(([label, value]) => ({ label, value }));
  };

  const handleMarketClick = (_e: any, { dataIndex }: any) => {
    const marketId = opcoData[dataIndex]?.opCoId;
    const marketLabel = opcoData[dataIndex]?.opco;
    openWithFilters("/marketdetails", { vmVarId, marketId, marketLabel });
  };

  const handleVendorClick = (_e: any, { dataIndex }: any) => {
    const vendorId = vendorData[dataIndex]?.vendorId;
    const vendorLabel = vendorData[dataIndex]?.vendorName;
    openWithFilters("/vendordetails", { vmVarId, vendorId, vendorLabel });
  };

  const handleVerticalClick = (_e: any, { dataIndex }: any) => {
    const verticalId = verticalData[dataIndex]?.verticalId;
    const verticalLabel = verticalData[dataIndex]?.vertical;
    openWithFilters("/verticaldetails", { vmVarId, verticalId, verticalLabel });
  };

  const handleVodafoneClick = (_e: any, { dataIndex }: any) => {
    const vodafoneId = vfNameData[dataIndex]?.vodafoneId;
    const vodafoneLabel = vfNameData[dataIndex]?.vertical;
    openWithFilters("/vodafonedetails", { vmVarId, vodafoneId, vodafoneLabel });
  };

  return (
    <div
      className="container mt-3 mb-3"
      style={{
        position: "relative",
        background: "black",
        border: `${darkMode ? "2px solid white" : "2px solid black"}`,
      }}
    >
      <Stack spacing={3}>
        <Stack direction="row" spacing={2}>
          <Box sx={{ width:"45%"}}>
            <Paper>
              <Typography className="px-2 py-2">VNF/Vendor</Typography>
            </Paper>
            <BarChart
              dataset={vendorData}
              xAxis={[{ scaleType: "band", dataKey: "component" }]}
              yAxis={[{}]}
              series={[{ dataKey: "activities", label: "VNF/Vendor" }]}
              height={300}
              onAxisClick={handleVendorClick}
            />
          </Box>
          <Box sx={{ width:"45%"}}>
            <Paper>
              <Typography className="px-2 py-2">VNF/Market</Typography>
            </Paper>
            <BarChart
              dataset={opcoData}
              xAxis={[{ scaleType: "band", dataKey: "market" }]}
              series={[
                {
                  dataKey: "activities",
                  label: "VNF/Market",
                  highlightScope: { highlighted: "item", faded: "global" },
                },
              ]}
              height={300}
              onAxisClick={handleMarketClick}
              highlightedItem={highlightedOpCoItem}
              barLabel={(item) => item.value?.toString()}
            />
          </Box>
        </Stack>

        <Stack direction="row" spacing={2}>
          <Box sx={{ width:"45%"}}>
            <Paper>
              <Typography className="px-2 py-2">VNF/Vertical</Typography>
            </Paper>
            <BarChart
              dataset={verticalData}
              yAxis={[{ scaleType: "band", dataKey: "vertical" }]}
              series={[{ dataKey: "activities", label: "VNF/Vertical" }]}
              layout="horizontal"
              height={300}
              margin={{ left: 140 }}
              onItemClick={handleVerticalClick}
            />
          </Box>
          <Box sx={{ width:"45%"}}>
            <Paper>
              <Typography className="px-2 py-2">VNF/Vodafone Name</Typography>
            </Paper>
            <PieChart
              series={[
                {
                  data: vfNameData.map((item) => ({
                    label: item.vertical,
                    value: item.activities,
                  })),
                  innerRadius: 70,
                  outerRadius: 120,
                },
              ]}
              height={300}
              width={600}
              onItemClick={handleVodafoneClick} // PieChart might have different event props, verify if needed
            />
          </Box>
        </Stack>

        <Stack direction="row" spacing={1}>
          {/* VNF Status Pie */}
          <Box sx={{ width: "50%", textAlign: "center" }}>
            <Paper>
              <Typography className="px-2 py-2">VNF Status</Typography>
            </Paper>
            <PieChart
              series={[{ data: vfData, innerRadius: 70, outerRadius: 120 }]}
              height={300}
              width={600}
            />
          </Box>

          {/* Planned Completion Pie */}
          <Box sx={{ width: "45%", textAlign: "center" }}>
            <Paper>
              <Typography className="px-2 py-2">
                Planned Completion Pie
              </Typography>
            </Paper>
            <BarChart
              dataset={plannedCompletionData.map((item) => ({
                label: item.label,
                activities: item.value,
              }))}
              xAxis={[{ scaleType: "band", dataKey: "label" }]}
              yAxis={[{}]}
              series={[{ dataKey: "activities", label: "Planned Completion" }]}
              height={300}
              width={600}
            />
          </Box>
        </Stack>
      </Stack>
    </div>
  );
};

export default NVFChart;
