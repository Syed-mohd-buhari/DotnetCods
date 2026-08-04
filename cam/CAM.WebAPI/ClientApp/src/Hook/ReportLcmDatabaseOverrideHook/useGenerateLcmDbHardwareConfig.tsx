import { useEffect } from "react";
import { QueryObjectGrid } from "../../Model/Common";
import { useAuth } from "../useAuth";
import { useResourceTableCrud } from "../useResourceTableCrud";
import setLoader from "../../Redux/Action/LoaderAction";

export function useGenerateLcmDbHardwareConfig(
  paginationQuery: QueryObjectGrid,
  functionForRefillGrid: Function | undefined
) {
  const { isPermesso } = useAuth();
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQuery,
    isPermesso ? functionForRefillGrid : undefined
  );
  const queryHardwareConfig = query;
  const setQueryHardwareConfig = setQuery;
  const nextHardwareConfig = next;
  const backHardwareConfig = back;

  return {
    queryHardwareConfig,
    setQueryHardwareConfig,
    nextHardwareConfig,
    backHardwareConfig,
  };
}
