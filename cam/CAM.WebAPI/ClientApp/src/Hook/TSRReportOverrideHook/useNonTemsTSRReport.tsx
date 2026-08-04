import { QueryObjectGrid } from "../../Model/Common";
import { TSRReportQueryObjectGrid } from "../../Model/TSRReport";
import { useAuth } from "../useAuth";
import { useResourceTableCrud } from "../useResourceTableCrud";

export function useNonTemsTSRReport(
  paginationQuery: QueryObjectGrid,
  functionForRefillGrid: Function | undefined
) {
  const { isPermesso } = useAuth();
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQuery,
    isPermesso ? functionForRefillGrid : undefined
  );
  const queryNonTems = query;
  const setQueryNonTems = setQuery;
  const nextNonTems = next;
  const backNonTems = back;

  return { queryNonTems, setQueryNonTems, nextNonTems, backNonTems };
}
