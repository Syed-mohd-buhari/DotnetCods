import React, { SetStateAction, useEffect, useState } from "react";
import { Dropdown } from "react-bootstrap";
import { useSelector } from "react-redux";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import {
  SelectFilterType,
  SelectGridType,
} from "../../Hook/CommonRenderGrid/GridRender";
import { useFilterTableCrud } from "../../Hook/useFilterTableCrud";
import {
  CustomGridRender,
  QueryObjectGrid,
  RenderDetail,
} from "../../Model/Common";
import {
  SoftwareConfigurationDtoGrid,
  SoftwareConfigurationQueryObjectGrid,
} from "../../Model/SoftwareConfiguration";
import {
  GetFilterColumSoftwareConfiguration,
  GetSubFuntionFilter,
} from "../../Redux/Action/SoftwareConfiguration/SoftwareConfigurationGridAction";
import { RootState } from "../../Redux/Store/rootStore";
import { useAuth } from "./../../Hook/useAuth";
import ThreeDot from "../../Components/TableCrud/ThreeDot";
import { toggleState } from "../../Hook/Common";
import { useResourceTableCrud } from "../../Hook/useResourceTableCrud";
import SubFunctionGrid from "./SubFunctionGrid";
import Paginate from "../../Components/PaginationComponent";
interface Props {
  action: {
    closeModal();
  };
  paginationQuery: SoftwareConfigurationQueryObjectGrid;
}

const SoftwareConfigurationGrid: React.FC<Props> = (props) => {
  // console.log("Tems SoftwareConfigurationGrid props", props);
  const [data, setData] = useState<any>([]);
  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();

  const [IsFiltriAttivati, setIsFiltriAttivati] = useState<boolean>(false);
  const { readonly, isPermesso } = useAuth();
  const getFiltersData = (state: RootState) =>
    state.subFunctionFilterGridReducer.SubFunctionFilterGridResult;
  let filterData = useSelector(getFiltersData);
  console.log("Tems filterData", filterData);
  const { query, setQuery, next, back } = useResourceTableCrud(
    props?.paginationQuery,
    isPermesso ? GetSubFuntionFilter : undefined
  );

  useEffect(() => {
    if (filterData !== undefined || filterData !== null) {
      setData(filterData?.items);
      let copy = { ...filterData?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
    }
  }, [filterData]);
  return (
    <>
      {data ? (
        <div>
          <SubFunctionGrid
            data={data}
            pagination={query}
            renderGrid={renderGridState?.render ?? []}
            action={{
              Filter: setQuery,
              setIsFiltriAttivati,
            }}
          ></SubFunctionGrid>
          {/* <Paginate
          pagination={{ page: query.page, pageSize: query.pageSize }}
          totalItems={filterData?.totalItems}
          actions={{ next, back }}
        /> */}
        </div>
      ) : (
        ""
      )}
    </>
  );
};

export default SoftwareConfigurationGrid;
