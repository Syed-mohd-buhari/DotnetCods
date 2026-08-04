import { GenericViewReportQueryObjectGrid } from "../../Model/GenericReport";
import { useAuth } from "../useAuth";
import { useResourceTableCrud } from "../useResourceTableCrud";

export function useGenericDisAggregatedReport(
  paginationQuery: GenericViewReportQueryObjectGrid,
  functionForRefillGrid: Function
) {
  const { isPermesso } = useAuth();
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQuery,
    isPermesso ? functionForRefillGrid : undefined
  );
  const queryDisAggregated = query;
  const setQueryDisAggregated = setQuery;
  const nextDisAggregated = next;
  const backDisAggregated = back;

  return { queryDisAggregated, setQueryDisAggregated, nextDisAggregated, backDisAggregated };
}