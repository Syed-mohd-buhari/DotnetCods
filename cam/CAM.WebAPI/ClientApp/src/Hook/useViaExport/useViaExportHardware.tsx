import { useEffect } from "react";
import { QueryObject, QueryObjectGrid } from "../../Model/Common";
import {
  QueryResultDtoVai,
  SkipManager,
  ViaExportQuery,
} from "../../Model/ViaExport/ViaExport";
import setLoader from "../../Redux/Action/LoaderAction";
import { useResourceTableCrud } from "../useResourceTableCrud";
import { useAuth } from "../useAuth";

export function useViaExportHardware(
  paginationQuery: ViaExportQuery,
  functionForRefillGrid: Function | undefined,
  skipAmount?: SkipManager[]
) {
  const { isPermesso } = useAuth();
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQuery,
    isPermesso ? functionForRefillGrid : undefined
  );
  const fakeQuery = (queryHardware: ViaExportQuery) => {
    queryHardware.skipManager = skipAmount;
    setQuery(queryHardware);
  };
  const queryHardware: ViaExportQuery = query;
  const setQueryHardware = fakeQuery;
  const nextHardware = next;
  const backHardware = back;
  return {
    queryHardware,
    setQueryHardware,
    nextHardware,
    backHardware,
  };
}
