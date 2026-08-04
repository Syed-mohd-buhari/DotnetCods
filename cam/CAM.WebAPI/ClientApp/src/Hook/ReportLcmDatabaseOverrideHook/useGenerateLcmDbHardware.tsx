import { useEffect } from "react";
import { QueryObjectGrid } from "../../Model/Common";
import { useAuth } from "../useAuth";
import { useResourceTableCrud } from "../useResourceTableCrud";
import setLoader from "../../Redux/Action/LoaderAction";

export function useGenerateLcmDbHardware(
  paginationQuery: QueryObjectGrid,
  functionForRefillGrid: Function | undefined
) {
  const { isPermesso } = useAuth();
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQuery,
    isPermesso ? functionForRefillGrid : undefined
  );
  const queryHardware = query;
  const setQueryHardware = setQuery;
  const nextHardware = next;
  const backHardware = back;

  return { queryHardware, setQueryHardware, nextHardware, backHardware };
}
