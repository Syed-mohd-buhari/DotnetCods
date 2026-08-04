import React, { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import { useNavigate, useLocation } from "react-router-dom";
import "../Css/App.css";
import "../Css/index.css";
import "../Css/NetworkElement.css";
import { useAuth } from "../Hook/useAuth";
import Paginate from "../Components/PaginationComponent";
import { useOperationTableCrud } from "../Hook/useOperationTableCrud";
import { useResourceTableCrud } from "../Hook/useResourceTableCrud";
import { CustomGridRender, stateConfirm } from "../Model/Common";
import { ResultDto } from "../Model/CommonModels";
import {
  VolteKPIWorklogDto,
  WorklogApprovalQueryObjectGrid,
} from "../Model/VolteKpi/WorklogApproval";
import setLoader from "../Redux/Action/LoaderAction";
import { EditWorklogApproval } from "../Redux/Action/VolteKPI/WorklogApproval/WorklogApprovalEditAction";
import { GetWorklogApprovalGrid } from "../Redux/Action/VolteKPI/WorklogApproval/WorklogApprovalGridAction";
import { RootState } from "../Redux/Store/rootStore";
import WorklogApprovalsGrid from "../screen/VolteKPI/WorklogApproval/WorklogApprovalGrid";

export let paginationQuery: WorklogApprovalQueryObjectGrid = {
  opCo: [],
  kpiIdName: [],
  monthYear: [],
  targetMonthlyValueOld: [],
  targetMonthlyValueProposed: [],
  targetMonthlyValueNew: [],
  eoyTargetOld: [],
  eoyTargetNew: [],
  actualMonthlyValueOld: [],
  actualMonthlyValueNew: [],
  actualNumberOfRegisteredOld: [],
  actualNumberOfRegisteredNew: [],
  actualNumberOfProvisionedOld: [],
  actualNumberOfProvisionedNew: [],
  comments: [],
  approved: [],
  submissionDate: undefined,
  submittedBy: [],
  isStored: [],
  sortBy: "",
  isSortAscending: undefined,
  page: 1,
  pageSize: 10,
  lastModified: undefined,
  principalId: undefined,
  deleted: undefined,
  orphan: undefined,
  lastModifiedBy: [],
};

const WorklogApprovalContainer: React.FC = (props) => {
  //TABS
  const [key, setKey] = useState("structure");
  //STATE CONFIRM
  const [redirect, setRedirect] = useState(false);
  const [filterRedirect, setFilterRedirect] = useState(false);
  //DTO
  const [data, setData] = useState<VolteKPIWorklogDto[]>([]);
  const [enablePendingRequest, setEnablePendingRequest] =
    useState<boolean>(false);

  const GridDto = useSelector(
    (state: RootState) =>
      state.worklogApprovalsGridReducer.worklogApprovalGridResult
  );

  const { isPermesso, pageSize } = useAuth();

  // const renderGrid = GridDto?.gridRender
  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();

  useEffect(() => {
    // Update paginationQuery with the pageSize from useAuth whenever it changes
    paginationQuery.pageSize = pageSize;
  }, [pageSize]);

  const refresh = () => {
    closeModal();
    GetWorklogApprovalGrid(query);
  };

  const reset = () => {
    setEnablePendingRequest(false);
    GetWorklogApprovalGrid(paginationQuery);
    setQuery(paginationQuery);
  };

  //PAGINAZIONE E RISULTATI FILTRAGGIO
  const navigate = useNavigate();
  const location: any = useLocation();
  const { query, setQuery, next, back } = useResourceTableCrud(
    paginationQuery,
    isPermesso ? GetWorklogApprovalGrid : undefined
  );

  const fakeDelete = async (id: number) => {
    let test = (await {
      data: null,
      warning: false,
      info: "",
    }) as ResultDto;

    return test;
  };

  const CustomEdit = (obj: VolteKPIWorklogDto) => {
    EditWorklogApproval(obj, false).then((x) => {
      GetWorklogApprovalGrid(query);
    });
  };

  const getDuplicates = (item: VolteKPIWorklogDto, setConfirm: Function) => {
    let copyQuery = { ...query } as WorklogApprovalQueryObjectGrid;
    if (item != undefined) {
      copyQuery.opCo = [item.opCoId ?? 0];
      copyQuery.kpiIdName = [item.volteKPIType ?? 0];
      copyQuery.monthYear = [
        `${
          (item.month ?? 0).toString().length > 1
            ? (item.month ?? 0).toString()
            : "0" + (item.month ?? 0).toString()
        }/${item.year}`,
      ];
      copyQuery.isStored = [1];
      queryAwaitable(copyQuery).then((x) => {
        GetWorklogApprovalGrid(copyQuery).then((x) => {
          setEnablePendingRequest(true);
          setConfirm(stateConfirm);
        });
      });
    } else {
      setEnablePendingRequest(false);
      setConfirm(stateConfirm);
    }
  };

  const queryAwaitable = async (copyQuery: WorklogApprovalQueryObjectGrid) => {
    setQuery(copyQuery);
    return;
  };

  const { closeModal, setLocalState } = useOperationTableCrud<
    VolteKPIWorklogDto,
    VolteKPIWorklogDto
  >(GetWorklogApprovalGrid, EditWorklogApproval, fakeDelete, refresh);

  //CARICAMENTO INIZIALE CON PAGE PREDEFINITO A 1
  useEffect(() => {
    setLoader("ADD", "GetWorklogApprovalGrid");

    if (location.state != null && location.state != undefined) {
      let localState = location.state as {
        id: number | null;
        tab: string;
        prevPage: string;
      };

      if (localState?.id != null) {
        setLocalState(localState);
        // Edit(localState?.id);
        setRedirect(true);
        setFilterRedirect(true);

        let copy = { ...query } as WorklogApprovalQueryObjectGrid;
        // copy. = [];
        // copy.vnfTransitionId?.push(localState?.id);
        copy.principalId = localState?.id;
        setQuery(copy);
      }
    } else {
      GetWorklogApprovalGrid(paginationQuery).then((x) =>
        setLoader("REMOVE", "GetWorklogApprovalGrid")
      );
    }
    setLoader("REMOVE", "GetWorklogApprovalGrid");
  }, []);

  const resetQuery = () => {
    setQuery(paginationQuery);
    setFilterRedirect(false);
  };

  //UPDATE ON CHANGE DTO
  useEffect(() => {
    if (GridDto !== undefined && GridDto !== null) {
      setData(GridDto?.items ?? []);
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
    }
  }, [GridDto]);

  return (
    <div className="pageContainer">
      <div className="d-flex justify-content-between py-3">
        <h3 className="voda-bold">KPIs Worklog and Approvals</h3>
        {enablePendingRequest ? (
          <button
            className="  voda-bold btn btn-danger px-4 btnHeader grid-main-btn"
            onClick={() => reset()}
            type="button"
          >
            Close Pending Request
          </button>
        ) : (
          <button
            className="px-4 btnHeader clearBtn br-20 voda-bold grid-main-btn"
            onClick={() => GetWorklogApprovalGrid(query)}
            type="button"
          >
            Refresh
          </button>
        )}
      </div>
      <div>
        <WorklogApprovalsGrid
          enablePendingRequest={enablePendingRequest}
          data={data}
          pagination={query}
          renderGrid={renderGridState?.render ?? []}
          action={{
            Edit: CustomEdit,
            Filter: setQuery,
            setEnablePendingRequest,
            getDuplicates,
          }}
        ></WorklogApprovalsGrid>
        <Paginate
          pagination={{ page: query.page, pageSize: query.pageSize }}
          totalItems={GridDto?.totalItems}
          actions={{ next, back }}
        />
      </div>
    </div>
  );
};

export default WorklogApprovalContainer;
