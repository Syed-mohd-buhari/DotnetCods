import { QueryObjectGrid } from "../../Model/Common";
import { TSRReportQueryObjectGrid } from "../../Model/TSRReport";
import { useAuth } from "../useAuth";
import { useResourceTableCrud } from "../useResourceTableCrud";

export function useTemsFNTReport(
  paginationQuery: QueryObjectGrid,
  functionForRefillGrid: Function | undefined
) {
  const { isPermesso } = useAuth();
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQuery,
    isPermesso ? functionForRefillGrid : undefined
  );
  const queryTems = query;
  const setQueryTems = setQuery;
  const nextTems = next;
  const backTems = back;

  return { queryTems, setQueryTems, nextTems, backTems };
}
