import { GenericViewReportQueryObjectGrid } from "../../Model/GenericReport";
import { useAuth } from "../useAuth";
import { useResourceTableCrud } from "../useResourceTableCrud";

export function useGenericAggregatedReport(
  paginationQuery: GenericViewReportQueryObjectGrid,
  functionForRefillGrid: Function
) {
  const { isPermesso } = useAuth();
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQuery,
    isPermesso ? functionForRefillGrid : undefined
  );
  const queryAggregated = query;
  const setQueryAggregated = setQuery;
  const nextAggregated = next;
  const backAggregated = back;

  return { queryAggregated, setQueryAggregated, nextAggregated, backAggregated };
}
