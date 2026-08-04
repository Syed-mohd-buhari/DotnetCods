import React, { SetStateAction, useEffect, useState } from "react";
import { Dropdown, Modal } from "react-bootstrap";
import { useSelector } from "react-redux";
import "../Css/App.css";
import "../Css/index.css";
import "../Css/NetworkElement.css";
import { useAuth } from "../Hook/useAuth";
import { useResourceTableCrud } from "../Hook/useResourceTableCrud";
import { CustomGridRender } from "../Model/Common";
import { SoftwareConfigurationQueryObjectGrid } from "../Model/SoftwareConfiguration";
import { GetSubFuntionFilter } from "../Redux/Action/SoftwareConfiguration/SoftwareConfigurationGridAction";
import { RootState } from "../Redux/Store/rootStore";
import SubFunctionGrid from "../screen/SoftwareConfiguration/SubFunctionGrid";
import Paginate from "../Components/PaginationComponent";
import ModalConfirm from "../Components/ModalConfirm";
import SetupColumns from "../screen/Shared/SetupColumns";

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
  const [isVisibleModalSetup, setIsVisibleModalSetup] = useState(false);
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

  const closeModalSetup = (changed: boolean) => {
    setIsVisibleModalSetup(false);
  };

  return (
    <>
      <div className="">
        <div className="">
          {/* <div className="row">
            <div className="col-12">
              <div>
                <div className="mt-30 fr d-flex">
                  <Dropdown className="d-inline more-options">
                    <Dropdown.Toggle id="dropdown-autoclose-inside">
                      More Options
                    </Dropdown.Toggle>

                    <Dropdown.Menu>
                      <Dropdown.Item
                        onClick={() => setIsVisibleModalSetup(true)}
                      >
                        Manage Table Content
                      </Dropdown.Item>
                    </Dropdown.Menu>
                  </Dropdown>
                </div>
              </div>
            </div>
          </div> */}
          <Modal
            show={isVisibleModalSetup}
            backdrop="static"
            keyboard={false}
            size="lg"
          >
            <Modal.Header className="d-flex justify-content-center">
              <div className="col-12 px-0">
                <div className="col-12">
                  <h4 className="mb-0 mt-1">Setup Grid Informations</h4>
                </div>
                {/* <ErrorNotification OnModal={true} /> */}
              </div>
            </Modal.Header>
            <Modal.Body className="plr-30">
              <SetupColumns
                renderGrid={renderGridState}
                action={{ closeModalSetup }}
                tab={""}
              ></SetupColumns>
            </Modal.Body>
          </Modal>
          <div className="mt-4">
            <SubFunctionGrid
              data={data}
              pagination={query}
              renderGrid={renderGridState?.render ?? []}
              action={{
                Filter: setQuery,
                setIsFiltriAttivati,
              }}
            ></SubFunctionGrid>
            <Paginate
              pagination={{ page: query.page, pageSize: query.pageSize }}
              totalItems={filterData?.totalItems}
              actions={{ next, back }}
            />
          </div>
        </div>
      </div>
    </>
  );
};

export default SoftwareConfigurationGrid;
