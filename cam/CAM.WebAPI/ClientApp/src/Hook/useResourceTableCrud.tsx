import { useEffect, useState, useCallback } from "react";
import { QueryObjectGrid } from "../Model/Common";
import setLoader from "../Redux/Action/LoaderAction";
import { useAuth } from "./useAuth";

export function useResourceTableCrud(
  paginationQuery: QueryObjectGrid,
  functionForRefillGrid?: Function | undefined,
  isLoaderRequired: boolean = true
) {
  const { pageSize } = useAuth();

  const [query, setQuery] = useState<QueryObjectGrid>({
    ...paginationQuery,
    pageSize: pageSize,
  });
  const [loading, setLoading] = useState(false);

  const next = useCallback((pageNumber: number) => {
    setQuery((prevQuery) => ({
      ...prevQuery,
      page: pageNumber,
    }));
  }, []);

  const back = useCallback(() => {
    setQuery(
      (prevQuery: any) =>
        ({
          ...prevQuery,
          page: prevQuery.page > 1 ? prevQuery.page - 1 : prevQuery.page,
        } as QueryObjectGrid)
    );
  }, []);

  const updatePageSize = useCallback((pageSize: number) => {
    setQuery(
      (prevQuery: any) =>
        ({
          ...prevQuery,
          page: 1,
          pageSize: pageSize ?? 10,
        } as QueryObjectGrid)
    );
  }, []);

  useEffect(() => {
    if (functionForRefillGrid !== undefined && functionForRefillGrid !== null) {
      if (isLoaderRequired) {
        setLoader("ADD", functionForRefillGrid.name);
      } else {
        setLoading(true);
      }

      functionForRefillGrid(query).then((x: any) => {
        if (isLoaderRequired) {
          setLoader("REMOVE", functionForRefillGrid.name);
        } else {
          setLoading(false);
        }
      });
    }
  }, [functionForRefillGrid, query, isLoaderRequired]);

  return { query, setQuery, next, back, updatePageSize, loading };
}
