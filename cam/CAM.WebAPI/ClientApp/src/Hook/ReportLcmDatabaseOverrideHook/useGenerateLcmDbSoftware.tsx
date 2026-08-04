import { useEffect } from "react";
import { QueryObjectGrid } from "../../Model/Common";
import setLoader from "../../Redux/Action/LoaderAction";
import { useResourceTableCrud } from "../useResourceTableCrud";
import { useAuth } from "../useAuth";

export function useGenerateLcmDbSoftware(
  paginationQuery: QueryObjectGrid,
  functionForRefillGrid: Function | undefined
) {
  const { isPermesso } = useAuth();
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQuery,
    isPermesso ? functionForRefillGrid : undefined
  );
  const querySoftware = query;
  const setQuerySoftware = setQuery;
  const nextSoftware = next;
  const backSoftware = back;
  return {
    querySoftware,
    setQuerySoftware,
    nextSoftware,
    backSoftware,
  };
}
