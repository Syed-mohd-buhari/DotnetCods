import { useEffect } from "react";
import { QueryObjectGrid } from "../../Model/Common";
import setLoader from "../../Redux/Action/LoaderAction";
import { useResourceTableCrud } from "../useResourceTableCrud";
import { useAuth } from "../useAuth";

export function useGenerateLcmDbSubBoundSoftware(
  paginationQuery: QueryObjectGrid,
  functionForRefillGrid: Function | undefined
) {
  const { isPermesso } = useAuth();
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQuery,
    isPermesso ? functionForRefillGrid : undefined
  );
  const querySubBoundSoftware = query;
  const setQuerySubBoundSoftware = setQuery;
  const nextSubBoundSoftware = next;
  const backSubBoundSoftware = back;
  return {
    querySubBoundSoftware,
    setQuerySubBoundSoftware,
    nextSubBoundSoftware,
    backSubBoundSoftware,
  };
}
