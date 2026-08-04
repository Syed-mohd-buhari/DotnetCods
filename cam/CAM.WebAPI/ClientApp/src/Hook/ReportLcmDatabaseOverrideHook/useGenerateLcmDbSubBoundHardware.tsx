import { useEffect } from "react";
import { QueryObjectGrid } from "../../Model/Common";
import { useAuth } from "../useAuth";
import { useResourceTableCrud } from "../useResourceTableCrud";
import setLoader from "../../Redux/Action/LoaderAction";

export function useGenerateLcmDbSubBoundHardware(
  paginationQuery: QueryObjectGrid,
  functionForRefillGrid: Function | undefined
) {
  const { isPermesso } = useAuth();
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQuery,
    isPermesso ? functionForRefillGrid : undefined
  );
  const querySubBoundHardware = query;
  const setQuerySubBoundHardware = setQuery;
  const nextSubBoundHardware = next;
  const backSubBoundHardware = back;

  return {
    querySubBoundHardware,
    setQuerySubBoundHardware,
    nextSubBoundHardware,
    backSubBoundHardware,
  };
}
