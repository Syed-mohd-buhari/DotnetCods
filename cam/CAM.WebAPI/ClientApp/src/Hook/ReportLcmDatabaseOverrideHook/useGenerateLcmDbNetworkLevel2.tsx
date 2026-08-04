import { useEffect } from "react";
import { QueryObjectGrid } from "../../Model/Common";
import { useAuth } from "../useAuth";
import { useResourceTableCrud } from "../useResourceTableCrud";
import setLoader from "../../Redux/Action/LoaderAction";

export function useGenerateLcmDbNetworkLevel2(
  paginationQuery: QueryObjectGrid,
  functionForRefillGrid: Function | undefined
) {
  const { isPermesso } = useAuth();
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQuery,
    isPermesso ? functionForRefillGrid : undefined
  );
  const queryNetworkLevel2 = query;
  const setQueryNetworkLevel2 = setQuery;
  const nextNetworkLevel2 = next;
  const backNetworkLevel2 = back;

  return {
    queryNetworkLevel2,
    setQueryNetworkLevel2,
    nextNetworkLevel2,
    backNetworkLevel2,
  };
}
