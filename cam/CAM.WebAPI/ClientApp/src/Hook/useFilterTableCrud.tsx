import { SetStateAction, useEffect } from "react";
import { useState } from "react";
import { DateFilter, QueryObjectGrid } from "../Model/Common";
import { GetFilterColumDesignAspect } from "../Redux/Action/DesignAspect/DesignAspectGridAction";
import {
  GetFilterColumLcmEngineeringArchived,
  GetFilterColumLcmEngineering,
} from "../Redux/Action/LcmEngineering/LcmEngineeringGridAction";
import {
  GetFilterColumArchivedPlannedActivity,
  GetFilterColumPlannedActivity,
} from "../Redux/Action/PlannedActivity/PlannedActivityGridAction";
import { lowerFirstLetter } from "./Common";

export function useFilterTableCrud<T extends QueryObjectGrid>(
  Filter: (obj: SetStateAction<QueryObjectGrid>) => any,
  functionForFilter?: Function,
  pagination?: T | undefined
) {
  const [isVisibleFiltri, setIsVisibleFiltri] = useState(0);
  const [isVisibleFiltriString, setIsVisibleFiltriString] = useState("");
  const [count, setCount] = useState<string[]>([]);
  const [filtriAttivi, setFiltriAttivi] = useState<T | undefined>(pagination);
  const [isFiltriAttivati, setIsFiltriAttivati] = useState<boolean>(false);
  const [FiltriAttivati, setFiltriAttivati] = useState<string[]>([]);

  //CHIAMATA GET FILTRI
  const getFilters = async (property: string, text: string) => {
    if (
      sessionStorage.getItem("isArchivedMode") &&
      sessionStorage.getItem("archivedType") === "LCM" &&
      !functionForFilter
    ) {
      functionForFilter = GetFilterColumLcmEngineeringArchived;
    } else if (
      !sessionStorage.getItem("isArchivedMode") &&
      sessionStorage.getItem("archivedType") == "LCM" &&
      !functionForFilter
    ) {
      functionForFilter = GetFilterColumLcmEngineering;
    } else if (
      sessionStorage.getItem("archivedType") === "DA" &&
      !functionForFilter
    ) {
      functionForFilter = GetFilterColumDesignAspect;
    } else if (
      !sessionStorage.getItem("isArchivedMode") &&
      sessionStorage.getItem("archivedType") === "PA" &&
      !functionForFilter
    ) {
      functionForFilter = GetFilterColumPlannedActivity;
    } else if (
      sessionStorage.getItem("isArchivedMode") &&
      sessionStorage.getItem("archivedType") === "PA" &&
      !functionForFilter
    ) {
      functionForFilter = GetFilterColumArchivedPlannedActivity;
    }
    if (functionForFilter) {
      const isArchivedPA =
        sessionStorage.getItem("isArchivedMode") === "true" &&
        sessionStorage.getItem("archivedType") === "PA";

      const paOriginFilter = sessionStorage.getItem("paOriginFilter");

      const filterPayload = {
        ...filtriAttivi,
        page: 1,
        ...(isArchivedPA && paOriginFilter === "ServiceLevel"
          ? { forServicePlanLink: ["True"] }
          : {}),
        ...(isArchivedPA && paOriginFilter === "LCM"
          ? { forLcmLink: ["True"] }
          : {}),
        ...(isArchivedPA && paOriginFilter === "DesignAspect"
          ? { forDesignAspectLink: ["True"] }
          : {}),
        ...(isArchivedPA && paOriginFilter === "Asset"
          ? { AddEditAssetFilter: true }
          : {}),
      };

      await functionForFilter(property, text, filterPayload);
    }
  };

  const closeAll = () => {
    setIsVisibleFiltri(0);
    setIsVisibleFiltriString("");
  };

  const setDateToChildren = (data: any, property: string, target: string) => {
    let copy = { ...filtriAttivi } as T;

    switch (target) {
      case "from":
        try {
          copy[lowerFirstLetter(property) + "StartDate"] = data;
        } catch {}
        try {
          if (copy[lowerFirstLetter(property)] === undefined) {
            copy[lowerFirstLetter(property)] = {
              startDate: undefined,
              endDate: undefined,
            } as DateFilter;
          }
          copy[lowerFirstLetter(property)].startDate = data;
        } catch {}
        Filter(copy);
        setFiltriAttivi(copy);
        break;

      case "to":
        try {
          copy[lowerFirstLetter(property) + "EndDate"] = data;
        } catch {}
        try {
          if (copy[lowerFirstLetter(property)] === undefined) {
            copy[lowerFirstLetter(property)] = {
              startDate: undefined,
              endDate: undefined,
            } as DateFilter;
          }
          copy[lowerFirstLetter(property)].endDate = data;
        } catch {}
        Filter(copy);
        setFiltriAttivi(copy);
        break;

      case "both":
        try {
          copy[lowerFirstLetter(property) + "EndDate"] = data?.to;
          copy[lowerFirstLetter(property) + "StartDate"] = data?.from;
        } catch {}
        try {
          if (copy[lowerFirstLetter(property)] === undefined) {
            copy[lowerFirstLetter(property)] = {
              startDate: undefined,
              endDate: undefined,
            } as DateFilter;
          }
          copy[lowerFirstLetter(property)].endDate = data?.to;
          copy[lowerFirstLetter(property)].startDate = data?.from;
        } catch {}
        Filter(copy);
        setFiltriAttivi(copy);
        break;
      default:
        break;
    }
  };

  useEffect(() => {
    setFiltriAttivi(pagination);
  }, [pagination]);

  const getFiltriAttivi = (
    ids: string[],
    property: string,
    override?: string
  ) => {
    let copy = { ...filtriAttivi } as T;
    copy[lowerFirstLetter(override ?? property)] = ids;
    copy = {
      ...copy,
      page: 1,
    };
    Filter(copy);
    setFiltriAttivi(copy);
  };

  const orderBy = (property: string, isAscending: boolean) => {
    let copy = { ...filtriAttivi } as T;
    copy.sortBy = lowerFirstLetter(property);
    copy.isSortAscending = isAscending;
    Filter(copy);
    setFiltriAttivi(copy);
  };

  const updateCount = (property: string) => {
    let copy = [...count];
    if (copy.filter((x) => x == property).length < 1) {
      copy.push(property);
      setCount(copy);
    }
    return copy;
  };

  const resetFilter = (property: string) => {
    let copy = { ...filtriAttivi } as T;
    copy &&
      copy[lowerFirstLetter(property)] &&
      copy[lowerFirstLetter(property)]?.splice(
        0,
        copy[lowerFirstLetter(property)]?.length
      );
    if (copy.sortBy == lowerFirstLetter(property)) {
      copy.sortBy = "";
    }
    let index = FiltriAttivati.findIndex((x) => x == property);
    let copyAttivati = [...FiltriAttivati];
    if (index != undefined && index != -1) {
      copyAttivati.splice(index, 1);
      setFiltriAttivati(copyAttivati);

      if (copyAttivati.length > 0) {
        setIsFiltriAttivati(true);
      } else {
        setIsFiltriAttivati(false);
      }
    }
    Filter(copy);
    setFiltriAttivi(copy);
  };

  const checkFilterinValue = (property: string): boolean => {
    let value = filtriAttivi?.[property] || "";
    if (value.length > 0 || filtriAttivi?.sortBy == property) {
      let copyAttivati = [...FiltriAttivati];
      if (!FiltriAttivati.includes(property)) {
        copyAttivati.push(property);
        setFiltriAttivati(copyAttivati);

        if (copyAttivati.length > 0) {
          setIsFiltriAttivati(true);
        } else {
          setIsFiltriAttivati(false);
        }
      }
      return true;
    }
    let index = FiltriAttivati.findIndex((x) => x == property);
    let copyAttivati = [...FiltriAttivati];
    if (index != undefined && index != -1) {
      copyAttivati.splice(index, 1);
      setFiltriAttivati(copyAttivati);

      if (copyAttivati.length > 0) {
        setIsFiltriAttivati(true);
      } else {
        setIsFiltriAttivati(false);
      }
    }
    return false;
  };

  const checkFilterDateinValue = (property: string): boolean => {
    if (
      filtriAttivi?.[property + "StartDate"] ||
      filtriAttivi?.[property]?.startDate ||
      (filtriAttivi?.[property]?.endDate &&
        filtriAttivi?.[property]?.startDate != undefined) ||
      (filtriAttivi?.[property + "EndDate"] &&
        filtriAttivi?.[property + "StartDate"] != undefined) ||
      filtriAttivi?.[property + "EndDate"] != undefined ||
      filtriAttivi?.sortBy == property
    ) {
      let copyAttivati = [...FiltriAttivati];
      if (!FiltriAttivati.includes(property)) {
        copyAttivati.push(property);
        setFiltriAttivati(copyAttivati);

        if (copyAttivati.length > 0) {
          setIsFiltriAttivati(true);
        } else {
          setIsFiltriAttivati(false);
        }
      }
      return true;
    }
    let index = FiltriAttivati.findIndex((x) => x == property);
    let copyAttivati = [...FiltriAttivati];
    if (index != undefined && index != -1) {
      copyAttivati.splice(index, 1);
      setFiltriAttivati(copyAttivati);

      if (copyAttivati.length > 0) {
        setIsFiltriAttivati(true);
      } else {
        setIsFiltriAttivati(false);
      }
    }
    return false;
  };

  const resetFilterDate = (property: string) => {
    let copy = { ...filtriAttivi } as T;
    try {
      copy[lowerFirstLetter(property) + "StartDate"] = undefined;
      copy[lowerFirstLetter(property) + "EndDate"] = undefined;
    } catch {}
    try {
      copy[lowerFirstLetter(property)] = undefined;
    } catch {}

    if (copy.sortBy == lowerFirstLetter(property)) {
      copy.sortBy = "";
    }

    let index = FiltriAttivati.findIndex(
      (x) => x == lowerFirstLetter(property)
    );
    let copyAttivati = [...FiltriAttivati];
    if (index != undefined && index != -1) {
      copyAttivati.splice(index, 1);
      setFiltriAttivati(copyAttivati);

      if (copyAttivati.length > 0) {
        setIsFiltriAttivati(true);
      } else {
        setIsFiltriAttivati(false);
      }
    }
    Filter(copy);
    setFiltriAttivi(copy);
  };

  return {
    filtriAttivi,
    setFiltriAttivi,
    isVisibleFiltri,
    setIsVisibleFiltri,
    resetFilter,
    closeAll,
    setDateToChildren,
    orderBy,
    resetFilterDate,
    getFilters,
    updateCount,
    getFiltriAttivi,
    count,
    setCount,
    checkFilterinValue,
    checkFilterDateinValue,
    isVisibleFiltriString,
    setIsVisibleFiltriString,
    isFiltriAttivati,
  };
}
