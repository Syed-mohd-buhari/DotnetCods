import React, { useState, useEffect, useCallback } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import Select from "react-select";
import TH from "../../Components/TableCrud/TableCrudTH";
import {
  LcmEngAuditDtoUpdate,
  LcmEngAuditQueryDto,
} from "../../Model/LcmEngAudit";
import { RootState, rootStore } from "../../Redux/Store/rootStore";
import { useSelector } from "react-redux";
import {
  GetFilterColumLcmEngAudit,
  GetLcmEngAuditGrid,
} from "../../Redux/Action/LcmEngAudit/LcmEngAuditGridAction";
import { CustomGridRender } from "../../Model/Common";
import setLoader from "../../Redux/Action/LoaderAction";
import { useResourceTableCrud } from "../../Hook/useResourceTableCrud";
import { useFilterTableCrud } from "../../Hook/useFilterTableCrud";
import { GetFilterColumAudit } from "../../Redux/Action/Audit/AuditGridAction";
import {
  SelectFilterType,
  SelectGridType,
} from "../../Hook/CommonRenderGrid/GridRender";
import {
  DateInputComponent,
  DropdownInputComponent,
  TextAreaInputComponent,
  TextInputComponent,
  ToggleInputComponent,
} from "../../Components/FormField";
import { useFormTableCrud } from "../../Hook/useFormTableCrud";
import { CreatLcmEngAudit } from "../../Redux/Action/LcmEngAudit/LCMEngAuditCreateAction";
import { EditLcmEngAudit } from "../../Redux/Action/LcmEngAudit/LCMEngAuditEditAction";
import { dictionaryToArray } from "../../Hook/Dictionary";
import { Tab, Tabs } from "react-bootstrap";
import { deleteLcmEngAudit } from "../../Redux/Action/LcmEngAudit/LCMEngAuditDeleteAction";
import LabelsDictionary from "../../Constant/LabelsAndDescriptions.json";
import { useAuth } from "../../Hook/useAuth";
import { MdDelete, MdEdit } from "react-icons/md";
import { useTheme } from "../../Context/ThemeContext";

interface Props {
  action: {
    closeModal(changed?: boolean): any;
  };
  lcmId: number;
  isAdd?: boolean;
  opCo?: string;
}
let paginationQuery: LcmEngAuditQueryDto = {
  lcmAncillaryDataId: [],
  lcmEngineeringId: [],
  opCo: [],
  designComponent: [],
  productCode: [],
  handedOverToOperation: [],
  contractRenewalPlan: [],
  reasonForNoPlan: [],
  commentOnProjectStatus: [],
  scopeOfSimplification: [],
  dataSource: [],
  incidentClass: [],
  occurenceProbability: [],
  newopsRiskEvaluation: [],
  //securityRiskPotential: [],
  securityRiskEffective: [],
  securityMitigation: [],
  securityRiskOverall: [],
  includedInSecurityScanning: [],
  raId: [],
  cyberRiskRequestId: [],
  lastPenTestReferenceNumber: [],
  lastScanRefNumber: [],

  requestId: [],
  lastScanDate: undefined,
  lastUpgradeDate: undefined,
  lastPenTestDate: undefined,
  assetOutOfScope: [],
  regulatoryFields: [],
  infrastructureLocation: [],
  eomControl: [],
  engUpdateTracker: [],
  opsUpdateTracker: [],
  //custom2: [],
  //kpiStatusService: [],
  //custom: [],
  //custom1: [],
  //idNew: [],
  originalHwLcmId: [],
  originalSwLcmId: [],
  exNetworks: [],
  //productImportanceHistory2: [],
  //cloudVersion: [],
  labSwRelease: [],
  //certifiedSWRealeseForNfviBundle: [],
  //lcmStatus: [],
  originalLCMID: [],
  creationUser: [],
  creationDate: undefined,
  modificationuser: [],
  modificationDate: undefined,
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 10,
  lastModified: undefined,
  principalId: undefined,
  orphan: false,
  deleted: false,
  lastModifiedBy: [],
  warranty: [],
};

const LcmEngAuditModal: React.FC<Props> = (props) => {
  const { isPermesso, pageSize } = useAuth();
  const [data, setData] = useState<any>();
  const [editFlag, setEditFlag] = useState<boolean>(false);
  const [keyTabs, setKey] = useState<string>("projectDetails");
  const {
    formData,
    setFormData,
    Save,
    changed,
    validation,
    setValidation,
    onChange,
    onChangeDate,
    onChangeSelect,
    setChanged,
    setInputValue,
    confirmForm,
    checkIsExist,
  } = useFormTableCrud<LcmEngAuditDtoUpdate>(CreatLcmEngAudit, EditLcmEngAudit);
  const [isVisibleFiltri, setIsVisibleFiltri] = useState("");
  useEffect(() => {
    // Update paginationQuery with the pageSize from useAuth whenever it changes
    paginationQuery.pageSize = pageSize;
  }, [pageSize]);
  const { query, setQuery, next, back } = useResourceTableCrud(
    {
      ...paginationQuery,
      lcmEngineeringId: [props?.lcmId],
    } as LcmEngAuditQueryDto,
    props.isAdd ? undefined : isPermesso ? GetLcmEngAuditGrid : undefined
  );
  const Grid = (state: RootState) =>
    state.lcmEngAuditGridReducer.LcmEngAuditGridResult;
  let GridDto: any = useSelector(Grid);
  const [renderGridState, setRenderGridState] = useState<
    CustomGridRender | undefined
  >();
  const getFiltersData = (state: RootState) =>
    state.lcmEngAuditGridReducer.filter;
  let filterData = useSelector(getFiltersData);
  const {
    filtriAttivi,
    resetFilter,
    closeAll,
    setDateToChildren,
    orderBy,
    resetFilterDate,
    getFilters,
    updateCount,
    getFiltriAttivi,
    count,
    checkFilterinValue,
    checkFilterDateinValue,
    isVisibleFiltriString,
    setIsVisibleFiltriString,
    isFiltriAttivati,
  } = useFilterTableCrud<LcmEngAuditQueryDto>(
    setQuery,
    GetFilterColumLcmEngAudit,
    query
  );

  const { darkMode } = useTheme();

  const thAction = {
    checkFilter: checkFilterinValue,
    settingVisibility: setIsVisibleFiltriString,
    resetFilter: resetFilter,
  };
  const actionFilterCK = {
    closeAll,
    updateCount,
    getFiltriAttivi,
    orderBy,
    getFilters,
  };
  const actionFilterDate = { closeAll, setDateToChildren, orderBy };
  const thActionDate = {
    checkFilter: checkFilterDateinValue,
    settingVisibility: setIsVisibleFiltriString,
    resetFilter: resetFilterDate,
  };

  let contractRenewalPlan = [
    { key: 0, value: "Renew Partial coverage" },
    { key: 1, value: "Renew Full coverage" },
    { key: 2, value: "No renewal" },
  ];

  let reasonForNoPlan = [
    { key: 0, value: "Budget dependencies" },
    { key: 1, value: "Network dependencies" },
    { key: 2, value: "Business dependencies" },
    { key: 3, value: "Stable platforms" },
  ];

  let scopeOfSimplification = [
    { key: 0, value: "Simplification UK" },
    { key: 1, value: "Simplification DE" },
    { key: 2, value: "Execution Monitoring" },
  ];

  let incidentClass = [
    { key: 0, value: "P0" },
    { key: 1, value: "P1" },
    { key: 2, value: "P2" },
    { key: 3, value: "P3" },
    { key: 4, value: "P4" },
    { key: 5, value: "P5" },
    { key: 6, value: "P6" },
  ];

  let occurenceProbability = [
    { key: 0, value: "Low" },
    { key: 1, value: "Medium" },
    { key: 2, value: "High" },
  ];

  // let securityRiskPotential = [
  //   { key: 0, value: "Very High" },
  //   { key: 1, value: "High" },
  //   { key: 2, value: "Medium" },
  //   { key: 3, value: "Low" },
  // ];

  let securityRiskEffective = [
    { key: 0, value: "Very High" },
    { key: 1, value: "High" },
    { key: 2, value: "Medium" },
    { key: 3, value: "Low" },
  ];

  let vulnerabilityRating = [
    { key: 0, value: "Critical" },
    { key: 1, value: "High" },
    { key: 2, value: "Medium" },
    { key: 3, value: "Low" },
  ];

  let securityMitigation = [
    { key: 0, value: "Under analysis" },
    { key: 1, value: "Action ongoing" },
    { key: 2, value: "Risk acceptance requested" },
    { key: 3, value: "Risk accepted" },
    { key: 4, value: "Planned" },
    { key: 5, value: "Closed" },
  ];

  let engUpdateTracker = [
    { key: 0, value: "To be started" },
    { key: 1, value: "Ongoing" },
    { key: 2, value: "Completed" },
  ];

  let opsUpdateTracker = [
    { key: 0, value: "To be started" },
    { key: 1, value: "Ongoing" },
    { key: 2, value: "Completed" },
  ];

  let assetOutOfScope = [
    { key: 0, value: "Asset not under GN accountability" },
    { key: 1, value: "Data gathering still ongoing" },
    {
      key: 2,
      value: "Asset accountability currently on migration to different unit",
    },
    { key: 3, value: "Historical back-up (remediation action completed)" },
    { key: 4, value: "Asset planned to be inserted in the network" },
    { key: 5, value: "Asset in planned decommission, no replacement" },
    { key: 6, value: "Other" },
    { key: 7, value: "In scope" },
    { key: 8, value: "Asset in planned dismission, no replacement" },
  ];

  let regulatoryFields = [
    { label: "PECN", value: "PECN" },
    { label: "PECS", value: "PECS" },
    {
      label: "SCF",
      value: "SCF",
    },
    { label: "NOF", value: "NOF" },
  ];

  let infrastructureLocation = [
    { key: 0, value: "Customer premises equipment" },
    {
      key: 1,
      value: "Aggregation/access ",
    },
    { key: 2, value: "Provider edge" },
    { key: 3, value: "Optical Core" },
    { key: 4, value: "Mobile Core" },
    { key: 5, value: "Data Core" },
    { key: 6, value: "Fixed Core" },
    { key: 7, value: "Management LAN" },
    { key: 8, value: "Session Access Zone" },
    { key: 9, value: "Development / lab" },
    { key: 10, value: "Not Applicable" },
  ];

  const handleNextTab = () => {
    switch (keyTabs) {
      case "projectDetails":
        setKey("securityAssessmentDetails");
        break;
      case "securityAssessmentDetails":
        setKey("engineeringDetails");
        break;
      case "engineeringDetails":
        setKey("operationalDetails");
        break;
      case "operationalDetails":
        setKey("tsrAttributes");
      default:
        break;
    }
  };

  const handlePreviousTab = () => {
    switch (keyTabs) {
      case "securityAssessmentDetails":
        setKey("projectDetails");
        break;
      case "engineeringDetails":
        setKey("securityAssessmentDetails");
        break;
      case "operationalDetails":
        setKey("engineeringDetails");
        break;
      case "tsrAttributes":
        setKey("operationalDetails");
      default:
        break;
    }
  };
  useEffect(() => {
    const newForm = {
      lcmAncillaryDataId: undefined,
      lcmEngineeringId: props.lcmId,
      productCode: "",
      handedOverToOperation: true,
      contractRenewalPlan: "",
      reasonForNoPlan: "",
      commentOnProjectStatus: "",
      scopeOfSimplification: "",
      dataSource: "",
      incidentClass: "",
      occurenceProbability: "",
      //securityRiskPotential: "",
      securityRiskEffective: "",
      vulnerabilityRating: "",
      securityMitigation: "",
      assetOutOfScope: "",
      regulatoryFields: "",
      infrastructureLocation: "",
      includedInSecurityScanning: false,
      exposedEdgeFlag: false,
      externalFacingFlag: false,
      raId: "",
      cyberRiskRequestId: "",
      lastScanRefNumber: "",
      qId: "",
      riskComment: "",
      lastPenTestReferenceNumber: "",
      requestId: "",
      lastScanDate: undefined,
      lastUpgradeDate: undefined,
      lastPenTestDate: undefined,
      eomControl: "",
      engUpdateTracker: "",
      opsUpdateTracker: "",
      //custom2: "",
      //kpiStatusService: "",
      //custom: "",
      //custom1: "",
      //idNew: "",
      exNetworks: "",
      originalHwLcmId: "",
      originalSwLcmId: "",
      //productImportanceHistory2: "",
      //cloudVersion: "",
      //certifiedSWRealeseForNfviBundle: "",
      //lcmStatus: "",
    } as LcmEngAuditDtoUpdate;
    setFormData(newForm);
  }, []);

  useEffect(() => {
    if (GridDto !== undefined && GridDto !== null) {
      setData(GridDto?.items);
      let copy = { ...GridDto?.gridRender } as CustomGridRender | undefined;
      setRenderGridState(copy);
      setLoader("REMOVE", "GetLcmEngAuditGrid");
    }
  }, [GridDto]);

  const onChangeDropdown = (fieldSet: string, e: any) => {
    const copy = { ...formData } as LcmEngAuditDtoUpdate;
    if (e && e["value"]) {
      copy[fieldSet] = e["value"];
      setFormData(copy);
    }
  };

  const onEditHandle = (item: LcmEngAuditDtoUpdate) => {
    let copy = { ...formData } as LcmEngAuditDtoUpdate;
    item &&
      formData &&
      Object.entries(item).forEach(([key, value]) => {
        if (formData.hasOwnProperty(key)) copy[key] = value;
      });
    setFormData(copy);
    setEditFlag(true);
  };
  const refresh = () => {
    if (!props.isAdd) {
      GetLcmEngAuditGrid({
        ...paginationQuery,
        lcmEngineeringId: [props?.lcmId],
      } as LcmEngAuditQueryDto);
    }
  };
  const onSaveAudit = () => {
    let copy = { ...formData } as LcmEngAuditDtoUpdate;
    copy.engUpdateTracker =
      copy.engUpdateTracker === "" || copy.engUpdateTracker === null
        ? "To be started"
        : copy.engUpdateTracker;
    copy.opsUpdateTracker =
      copy.opsUpdateTracker === "" || copy.opsUpdateTracker === null
        ? "To be started"
        : copy.opsUpdateTracker;
    if (copy.lastScanDate === null) {
      delete copy.lastScanDate;
    }
    if (copy.lastUpgradeDate === null) {
      delete copy.lastUpgradeDate;
    }

    if (copy && copy !== null && copy !== undefined) {
      if (props.isAdd) {
        CreatLcmEngAudit(copy).then((x) => {
          props.action.closeModal();
        });
      } else {
        EditLcmEngAudit(copy).then((x) => {
          setEditFlag(false);
          setKey("projectDetails");
          refresh();
        });
      }
    }
  };

  const onDeleteHandle = (id) => {
    deleteLcmEngAudit(id).then((x) => {
      props.action.closeModal();
    });
  };
  return (
    <div className="col-12">
      {/* <Modal
                show={isAncillaryVisibleModalSetup}
                backdrop="static"
                keyboard={false}
                size="lg"
            >
                <Modal.Header className="d-flex justify-content-center">
                <div className="col-12 px-0">
                    <div className="col-12">
                    <h4 className="mb-0 mt-1">Setup Grid Informations</h4>
                    </div>
                </div>
                </Modal.Header>
                <Modal.Body className="plr-30">
                <SetupColumns
                    renderGrid={renderGridState}
                    action={{ closeModalSetup: () => setIsAncillaryVisibleModalSetup(false) }}
                ></SetupColumns>
                </Modal.Body>
            </Modal>
            <Dropdown className="d-inline more-options">
                <Dropdown.Toggle id="dropdown-autoclose-inside">
                More Options
                </Dropdown.Toggle>

                <Dropdown.Menu>
                <Dropdown.Item onClick={() => setIsAncillaryVisibleModalSetup(true)}>
                    Manage Table Content
                </Dropdown.Item>
                </Dropdown.Menu>
            </Dropdown> */}
      {!props.isAdd && (
        <div className="col-12 mx-0 px-0">
          <div className="mx-0 px-0 py-3 flex-row table-container">
            <table
              className="table-responsive table-thead-sticky"
              style={{ minHeight: "inherit" }}
            >
              <thead>
                <tr className="intestazione">
                  {renderGridState?.render
                    .sort((a, b) => a.order - b.order)
                    .filter((x) => x.show)
                    .map((item, i) =>
                      SelectFilterType(
                        item.propertyName == "requestId"
                          ? "Vuln. Request ID"
                          : item.propertyName,
                        item.type,
                        paginationQuery?.isSortAscending,
                        filtriAttivi,
                        actionFilterDate,
                        paginationQuery?.sortBy,
                        filterData,
                        count,
                        actionFilterCK,
                        thAction,
                        thActionDate,
                        isVisibleFiltriString,
                        undefined,
                        undefined,
                        true
                      )
                    )}
                  {renderGridState?.render && renderGridState?.render.length ? (
                    <th className="customWidth"></th>
                  ) : (
                    ""
                  )}
                </tr>
              </thead>
              <tbody>
                {data?.map((item, index) => (
                  <tr className={`dati`} key={item.lcmAncillaryDataId}>
                    {renderGridState?.render
                      .sort((a, b) => a.order - b.order)
                      .filter((x) => x.show)
                      .map((td, i) =>
                        td.propertyName === "regulatoryFields" ? (
                          <td className="">
                            {Object.keys(item?.regulatoryFields).toString()}
                          </td>
                        ) : (
                          SelectGridType(
                            item[td.propertyName],
                            td.propertyName,
                            td.type
                          )
                        )
                      )}

                    <td className="actions">
                      <div className="d-flex flex-row">
                        <button
                          type="button"
                          title="Edit"
                          className="btn btn-link"
                          onClick={() => onEditHandle(item)}
                        >
                          <MdEdit color={`${darkMode ? "white" : "black"}`} />
                        </button>
                        <button
                          type="button"
                          title="Delete"
                          className="btn btn-link"
                          onClick={() =>
                            onDeleteHandle(item.lcmAncillaryDataId)
                          }
                        >
                          <MdDelete color={`${darkMode ? "white" : "black"}`} />
                        </button>
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      )}
      {(props.isAdd || editFlag) && (
        <>
          <form onChange={() => setChanged(true)}>
            <Tabs
              defaultActiveKey={keyTabs}
              id="uncontrolled-tab-example"
              activeKey={keyTabs}
              onSelect={(x) => setKey(x || "")}
            >
              <Tab eventKey="projectDetails" title="Project Details">
                <div className="col-12 p-0 mt-4 mx-2">
                  <fieldset className="fieldset p-0">
                    {/* <label className="text-bb mb-4">Project Details</label> */}
                    <div className="row">
                      <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <TextInputComponent
                            label={`${
                              LabelsDictionary["productCode"]?.Full ??
                              "productCode"
                            }`}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-2"
                            value={formData?.productCode ?? ""}
                            required={false}
                            onChange={(e: any) => onChange("productCode", e)}
                          />
                        </div>
                      </div>
                      <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <DropdownInputComponent
                            label={`${
                              LabelsDictionary["contractRenewalPlan"]?.Full ??
                              "contractRenewalPlan"
                            }`}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-2"
                            isSearchable={true}
                            isClearable={true}
                            onBlur={() => setInputValue("")}
                            value={
                              formData &&
                              formData?.contractRenewalPlan != undefined
                                ? contractRenewalPlan.find(
                                    (x) =>
                                      x.value.toLowerCase() ===
                                      formData?.contractRenewalPlan?.toLowerCase()
                                  )
                                : null
                            }
                            options={contractRenewalPlan}
                            onChange={(e: any) =>
                              onChangeDropdown("contractRenewalPlan", e)
                            }
                          />
                        </div>
                      </div>
                      <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <DropdownInputComponent
                            label={`${
                              LabelsDictionary["reasonForNoPlan"]?.Full ??
                              "reasonForNoPlan"
                            }`}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-2"
                            isSearchable={true}
                            isClearable={true}
                            value={
                              formData && formData?.reasonForNoPlan != undefined
                                ? reasonForNoPlan.find(
                                    (x) =>
                                      x.value.toLowerCase() ===
                                      formData?.reasonForNoPlan?.toLowerCase()
                                  )
                                : null
                            }
                            options={reasonForNoPlan}
                            onChange={(e: any) =>
                              onChangeDropdown("reasonForNoPlan", e)
                            }
                          />
                        </div>
                      </div>
                      <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <TextInputComponent
                            label={`${
                              LabelsDictionary["commentOnProjectStatus"]
                                ?.Full ?? "commentOnProjectStatus"
                            }`}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-2"
                            value={formData?.commentOnProjectStatus ?? ""}
                            onChange={(e: any) =>
                              onChange("commentOnProjectStatus", e)
                            }
                          />
                        </div>
                      </div>
                      <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <DropdownInputComponent
                            label={`${
                              LabelsDictionary["scopeOfSimplification"]?.Full ??
                              "scopeOfSimplification"
                            }`}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-2"
                            isSearchable={true}
                            isClearable={true}
                            value={
                              formData &&
                              formData?.scopeOfSimplification != undefined
                                ? scopeOfSimplification.find(
                                    (x) =>
                                      x.value.toLowerCase() ===
                                      formData?.scopeOfSimplification?.toLowerCase()
                                  )
                                : null
                            }
                            options={scopeOfSimplification}
                            onChange={(e: any) =>
                              onChangeDropdown("scopeOfSimplification", e)
                            }
                          />
                        </div>
                      </div>
                      <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <ToggleInputComponent
                            label={`${
                              LabelsDictionary["handedOverToOperation"]?.Full ??
                              "handedOverToOperation"
                            }`}
                            value={formData?.handedOverToOperation ?? true}
                            required={false}
                            onChange={(e: any) =>
                              onChange("handedOverToOperation", e)
                            }
                          />
                        </div>
                      </div>
                      <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <TextInputComponent
                            label={`${
                              LabelsDictionary["dataSource"]?.Full ??
                              "dataSource"
                            }`}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-2"
                            value={formData?.dataSource ?? ""}
                            required={false}
                            onChange={(e: any) => onChange("dataSource", e)}
                          />
                        </div>
                      </div>
                    </div>
                  </fieldset>
                </div>
              </Tab>
              <Tab
                eventKey="securityAssessmentDetails"
                title="Security Details"
              >
                <div className="col-12 p-0 mt-4 mx-2">
                  <fieldset className="fieldset p-0">
                    {/* <label className="text-bb mb-4">Security Assessment Details</label> */}
                    <div className="row">
                      {/* <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <DropdownInputComponent
                            label={`${
                              LabelsDictionary["securityRiskPotential"]?.Full ??
                              "securityRiskPotential"
                            }`}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-2"
                            isSearchable={true}
                            isClearable={true}
                            value={
                              formData &&
                              formData?.securityRiskPotential != undefined
                                ? securityRiskPotential.find(
                                    (x) =>
                                      x.value.toLowerCase() ===
                                      formData?.securityRiskPotential?.toLowerCase()
                                  )
                                : null
                            }
                            options={securityRiskPotential}
                            onChange={(e: any) =>
                              onChangeDropdown("securityRiskPotential", e)
                            }
                          />
                        </div>
                      </div> */}
                      <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <DropdownInputComponent
                            label={`${
                              LabelsDictionary["securityRiskEffective"]?.Full ??
                              "securityRiskEffective"
                            }`}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-2"
                            isSearchable={true}
                            isClearable={true}
                            value={
                              formData &&
                              formData?.securityRiskEffective != undefined
                                ? securityRiskEffective.find(
                                    (x) =>
                                      x.value.toLowerCase() ===
                                      formData?.securityRiskEffective?.toLowerCase()
                                  )
                                : null
                            }
                            options={securityRiskEffective}
                            onChange={(e: any) =>
                              onChangeDropdown("securityRiskEffective", e)
                            }
                          />
                        </div>
                      </div>
                      <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <DropdownInputComponent
                            label={`${
                              LabelsDictionary["securityMitigation"]?.Full ??
                              "securityMitigation"
                            }`}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-2"
                            isSearchable={true}
                            isClearable={true}
                            value={
                              formData &&
                              formData?.securityMitigation != undefined
                                ? securityMitigation.find(
                                    (x) =>
                                      x.value.toLowerCase() ===
                                      formData?.securityMitigation?.toLowerCase()
                                  )
                                : null
                            }
                            options={securityMitigation}
                            onChange={(e: any) =>
                              onChangeDropdown("securityMitigation", e)
                            }
                          />
                        </div>
                      </div>
                      <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <TextInputComponent
                            label={`${
                              LabelsDictionary["raId"]?.Full ?? "raId"
                            }`}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-2"
                            value={formData?.raId ?? ""}
                            required={false}
                            onChange={(e: any) => onChange("raId", e)}
                          />
                        </div>
                      </div>
                      <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <ToggleInputComponent
                            label={`${
                              LabelsDictionary["includedInSecurityScanning"]
                                ?.Full ?? "includedInSecurityScanning"
                            }`}
                            value={
                              formData?.includedInSecurityScanning ?? false
                            }
                            required={false}
                            onChange={(e: any) =>
                              onChange("includedInSecurityScanning", e)
                            }
                          />
                        </div>
                      </div>
                      <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <TextInputComponent
                            // label={`${
                            //   LabelsDictionary["requestId"]?.Full ?? "requestId"
                            // }`}
                            label={"Vuln. Request ID"}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-2"
                            value={formData?.requestId ?? ""}
                            required={false}
                            onChange={(e: any) => onChange("requestId", e)}
                          />
                        </div>
                      </div>

                      <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <DateInputComponent
                            label={`${
                              LabelsDictionary["lastScanDate"]?.Full ??
                              "lastScanDate"
                            }`}
                            labelCSS="mb-0"
                            value={
                              formData?.lastScanDate &&
                              new Date(formData?.lastScanDate)
                            }
                            onChange={(e, newDate) => {
                              e.preventDefault();
                              onChangeDate("lastScanDate", newDate);
                            }}
                            dateFormat="dd/MM/yyyy"
                            placeholderText={"NOT SPECIFIED"}
                          />
                        </div>
                      </div>
                      <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <DateInputComponent
                            label={`${
                              LabelsDictionary["lastUpgradeDate"]?.Full ??
                              "lastUpgradeDate"
                            }`}
                            labelCSS="mb-0"
                            value={
                              formData?.lastUpgradeDate &&
                              new Date(formData?.lastUpgradeDate)
                            }
                            onChange={(e, newDate) => {
                              e.preventDefault();
                              onChangeDate("lastUpgradeDate", newDate);
                            }}
                            dateFormat="dd/MM/yyyy"
                            placeholderText={"NOT SPECIFIED"}
                          />
                        </div>
                      </div>
                      <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <DropdownInputComponent
                            label={`${
                              LabelsDictionary["vulnerabilityRating"]?.Full ??
                              "vulnerabilityRating"
                            }`}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-2"
                            isSearchable={true}
                            isClearable={true}
                            value={
                              formData &&
                              formData?.vulnerabilityRating != undefined
                                ? vulnerabilityRating.find(
                                    (x) =>
                                      x.value.toLowerCase() ===
                                      formData?.vulnerabilityRating?.toLowerCase()
                                  )
                                : null
                            }
                            options={vulnerabilityRating}
                            onChange={(e: any) =>
                              onChangeDropdown("vulnerabilityRating", e)
                            }
                          />
                        </div>
                      </div>
                      <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <TextInputComponent
                            label={`${
                              LabelsDictionary["cyberRiskRequestId"]?.Full ??
                              "cyberRiskRequestId"
                            }`}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-2"
                            value={formData?.cyberRiskRequestId ?? ""}
                            required={false}
                            onChange={(e: any) =>
                              onChange("cyberRiskRequestId", e)
                            }
                          />
                        </div>
                      </div>

                      <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <TextInputComponent
                            label={`${
                              LabelsDictionary["lastScanRefNumber"]?.Full ??
                              "lastScanRefNumber"
                            }`}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-2"
                            value={formData?.lastScanRefNumber ?? ""}
                            required={false}
                            onChange={(e: any) =>
                              onChange("lastScanRefNumber", e)
                            }
                          />
                        </div>
                      </div>
                      <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <DateInputComponent
                            label={`${
                              LabelsDictionary["lastPenTestDate"]?.Full ??
                              "lastPenTestDate"
                            }`}
                            labelCSS="mb-0"
                            value={
                              formData?.lastPenTestDate &&
                              new Date(formData?.lastPenTestDate)
                            }
                            onChange={(e, newDate) => {
                              e.preventDefault();
                              onChangeDate("lastPenTestDate", newDate);
                            }}
                            dateFormat="dd/MM/yyyy"
                            placeholderText={"NOT SPECIFIED"}
                          />
                        </div>
                      </div>

                      <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <TextInputComponent
                            label={`${
                              LabelsDictionary["lastPenTestReferenceNumber"]
                                ?.Full ?? "lastPenTestReferenceNumber"
                            }`}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-2"
                            value={formData?.lastPenTestReferenceNumber ?? ""}
                            required={false}
                            onChange={(e: any) =>
                              onChange("lastPenTestReferenceNumber", e)
                            }
                          />
                        </div>
                      </div>

                      <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <TextInputComponent
                            label={`${
                              LabelsDictionary["qId"]?.Full ?? "qId"
                            }(Demand Portal quails Id)`}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-2"
                            value={formData?.qId ?? ""}
                            required={false}
                            onChange={(e: any) => onChange("qId", e)}
                          />
                        </div>
                      </div>
                      <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <TextAreaInputComponent
                            label={`${
                              LabelsDictionary["riskComment"]?.Full ??
                              "riskComment"
                            }`}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-2"
                            areaRow={3}
                            value={formData?.riskComment ?? ""}
                            required={false}
                            onChange={(e: any) => onChange("riskComment", e)}
                          />
                        </div>
                      </div>
                    </div>
                  </fieldset>
                </div>
              </Tab>
              <Tab eventKey="engineeringDetails" title="Engineering Details">
                <div className="col-12 p-0 mt-4 mx-2">
                  <fieldset className="fieldset p-0">
                    {/* <label className="text-bb mb-4">Engineering Details</label> */}
                    <div className="row">
                      <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <DropdownInputComponent
                            label={`${
                              LabelsDictionary["assetOutOfScope"]?.Full ??
                              "assetOutOfScope"
                            }`}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-2"
                            isSearchable={true}
                            isClearable={true}
                            required={false}
                            value={
                              formData && formData?.assetOutOfScope != undefined
                                ? assetOutOfScope.find(
                                    (x) =>
                                      x.value.toLowerCase() ===
                                      formData?.assetOutOfScope?.toLowerCase()
                                  )
                                : null
                            }
                            options={assetOutOfScope}
                            onChange={(e: any) =>
                              onChangeDropdown("assetOutOfScope", e)
                            }
                          />
                        </div>
                      </div>
                      <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <TextInputComponent
                            label={`${
                              LabelsDictionary["eomControl"]?.Full ??
                              "eomControl"
                            }`}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-2"
                            value={formData?.eomControl ?? ""}
                            onChange={(e: any) => onChange("eomControl", e)}
                          />
                        </div>
                      </div>
                      <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <DropdownInputComponent
                            label={`${
                              LabelsDictionary["engUpdateTracker"]?.Full ??
                              "engUpdateTracker"
                            }`}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-2"
                            isSearchable={true}
                            isClearable={true}
                            required={false}
                            defaultValue={{ key: 0, value: "To be started" }}
                            value={
                              formData && formData?.engUpdateTracker
                                ? engUpdateTracker.find(
                                    (x) =>
                                      x.value.toLowerCase() ===
                                      formData?.engUpdateTracker?.toLowerCase()
                                  )
                                : { key: 0, value: "To be started" }
                            }
                            options={engUpdateTracker}
                            onChange={(e: any) =>
                              onChangeDropdown("engUpdateTracker", e)
                            }
                          />
                        </div>
                      </div>
                      <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <DropdownInputComponent
                            label={`${
                              LabelsDictionary["incidentClass"]?.Full ??
                              "incidentClass"
                            }`}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-2"
                            isSearchable={true}
                            isClearable={true}
                            required={false}
                            value={
                              formData && formData?.incidentClass != undefined
                                ? incidentClass.find(
                                    (x) =>
                                      x.value.toLowerCase() ===
                                      formData?.incidentClass?.toLowerCase()
                                  )
                                : null
                            }
                            options={incidentClass}
                            onChange={(e: any) =>
                              onChangeDropdown("incidentClass", e)
                            }
                          />
                        </div>
                      </div>
                      <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <DropdownInputComponent
                            label={`${
                              LabelsDictionary["occurenceProbability"]?.Full ??
                              "occurenceProbability"
                            }`}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-2"
                            isSearchable={true}
                            isClearable={true}
                            required={false}
                            value={
                              formData &&
                              formData?.occurenceProbability != undefined
                                ? occurenceProbability.find(
                                    (x) =>
                                      x.value.toLowerCase() ===
                                      formData?.occurenceProbability?.toLowerCase()
                                  )
                                : null
                            }
                            options={occurenceProbability}
                            onChange={(e: any) =>
                              onChangeDropdown("occurenceProbability", e)
                            }
                          />
                        </div>
                      </div>
                    </div>
                  </fieldset>
                </div>
              </Tab>
              <Tab eventKey="operationalDetails" title="Operational Details">
                <div className="col-12 p-0 mt-4 mx-2">
                  <fieldset className="fieldset p-0">
                    {/* <label className="text-bb mb-4">Operational Details</label> */}
                    <div className="row">
                      <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <DropdownInputComponent
                            label={`${
                              LabelsDictionary["opsUpdateTracker"]?.Full ??
                              "opsUpdateTracker"
                            }`}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-2"
                            isSearchable={true}
                            isClearable={true}
                            required={false}
                            defaultValue={{ key: 0, value: "To be started" }}
                            value={
                              formData && formData?.opsUpdateTracker
                                ? opsUpdateTracker.find(
                                    (x) =>
                                      x.value.toLowerCase() ===
                                      formData?.opsUpdateTracker?.toLowerCase()
                                  )
                                : { key: 0, value: "To be started" }
                            }
                            options={opsUpdateTracker}
                            onChange={(e: any) =>
                              onChangeDropdown("opsUpdateTracker", e)
                            }
                          />
                        </div>
                      </div>
                      {/* <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <TextInputComponent
                            label={`${
                              LabelsDictionary["custom2"]?.Full ?? "custom2"
                            }`}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-2"
                            value={formData?.custom2 ?? ""}
                            onChange={(e: any) => onChange("custom2", e)}
                          />
                        </div>
                      </div> */}
                      {/* <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <TextInputComponent
                            label={`${
                              LabelsDictionary["kpiStatusService"]?.Full ??
                              "kpiStatusService"
                            }`}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-2"
                            value={formData?.kpiStatusService ?? ""}
                            onChange={(e: any) =>
                              onChange("kpiStatusService", e)
                            }
                          />
                        </div>
                      </div> */}
                      {/* <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <TextInputComponent
                            label={`${
                              LabelsDictionary["custom"]?.Full ?? "custom"
                            }`}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-2"
                            value={formData?.custom ?? ""}
                            onChange={(e: any) => onChange("custom", e)}
                          />
                        </div>
                      </div> */}
                      {/* <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <TextInputComponent
                            label={`${
                              LabelsDictionary["custom1"]?.Full ?? "custom1"
                            }`}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-2"
                            value={formData?.custom1 ?? ""}
                            onChange={(e: any) => onChange("custom1", e)}
                          />
                        </div>
                      </div> */}
                      {/* <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <TextInputComponent
                            label={`${
                              LabelsDictionary["idNew"]?.Full ?? "idNew"
                            }`}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-2"
                            value={formData?.idNew ?? ""}
                            onChange={(e: any) => onChange("idNew", e)}
                          />
                        </div>
                      </div> */}
                      <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <TextInputComponent
                            label={`${
                              LabelsDictionary["exNetworks"]?.Full ??
                              "exNetworks"
                            }`}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-2"
                            value={formData?.exNetworks ?? ""}
                            onChange={(e: any) => onChange("exNetworks", e)}
                          />
                        </div>
                      </div>
                      {/* <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <TextInputComponent
                            label={`${
                              LabelsDictionary["productImportanceHistory2"]
                                ?.Full ?? "productImportanceHistory2"
                            }`}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-2"
                            value={formData?.productImportanceHistory2 ?? ""}
                            onChange={(e: any) =>
                              onChange("productImportanceHistory2", e)
                            }
                          />
                        </div>
                      </div> */}
                      {/* <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <TextInputComponent
                            label={`${
                              LabelsDictionary["cloudVersion"]?.Full ??
                              "cloudVersion"
                            }`}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-2"
                            value={formData?.cloudVersion ?? ""}
                            onChange={(e: any) => onChange("cloudVersion", e)}
                          />
                        </div>
                      </div> */}
                      {/* <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <TextInputComponent
                            label={`${
                              LabelsDictionary[
                                "certifiedSWRealeseForNfviBundle"
                              ]?.Full ?? "certifiedSWRealeseForNfviBundle"
                            }`}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-2"
                            value={
                              formData?.certifiedSWRealeseForNfviBundle ?? ""
                            }
                            onChange={(e: any) =>
                              onChange("certifiedSWRealeseForNfviBundle", e)
                            }
                          />
                        </div>
                      </div> */}
                      {/* <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <TextInputComponent
                            label={`${
                              LabelsDictionary["lcmStatus"]?.Full ?? "lcmStatus"
                            }`}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-2"
                            value={formData?.lcmStatus ?? ""}
                            onChange={(e: any) => onChange("lcmStatus", e)}
                          />
                        </div>
                      </div> */}
                      <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <TextInputComponent
                            label={`${
                              LabelsDictionary["originalHwLcmId"]?.Full ??
                              "originalHwLcmId"
                            }`}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-2"
                            value={formData?.originalHwLcmId ?? ""}
                            onChange={(e: any) =>
                              onChange("originalHwLcmId", e)
                            }
                          />
                        </div>
                      </div>
                      <div className="form-group col-4 pl-0">
                        <div className="col-12">
                          <TextInputComponent
                            label={`${
                              LabelsDictionary["originalSwLcmId"]?.Full ??
                              "originalSwLcmId"
                            }`}
                            labelCSS="mb-0"
                            inputCSS="labelForm voda-bold mb-2"
                            value={formData?.originalSwLcmId ?? ""}
                            onChange={(e: any) =>
                              onChange("originalSwLcmId", e)
                            }
                          />
                        </div>
                      </div>
                    </div>
                  </fieldset>
                </div>
              </Tab>
              {(props.opCo === "UK" ||
                props.opCo === "GROUP" ||
                props.opCo === "ZZZ") && (
                <Tab eventKey="tsrAttributes" title="UK TSR Attributes">
                  <div className="col-12 p-0 mt-4 mx-2">
                    <fieldset className="fieldset p-0">
                      {/* <label className="text-bb mb-4">Engineering Details</label> */}
                      <div className="row">
                        <div className="form-group col-4 pl-0">
                          <label className="labelForm voda-bold w-100 mb-0">
                            Regulatory Fields
                            <div className="d-flex">
                              <div className="w-100">
                                <Select
                                  menuPosition={"fixed"}
                                  options={regulatoryFields}
                                  value={
                                    formData && formData.regulatoryFields
                                      ? Object.entries(
                                          formData.regulatoryFields
                                        )
                                          .filter(([key, value]) => value)
                                          .map(([key]) => ({
                                            label: key,
                                            value: key,
                                          }))
                                      : null
                                  }
                                  isMulti
                                  onChange={(value: any) => {
                                    setFormData({
                                      ...formData,
                                      regulatoryFields: value.reduce(
                                        (obj, item) => {
                                          return {
                                            ...obj,
                                            [item.value]: true,
                                          };
                                        },
                                        {}
                                      ),
                                    });
                                  }}
                                />
                              </div>
                            </div>
                          </label>
                        </div>
                        <div className="form-group col-4 pl-0">
                          <div className="col-12">
                            <DropdownInputComponent
                              label={`${
                                LabelsDictionary["infrastructureLocation"]
                                  ?.Full ?? "infrastructureLocation"
                              }`}
                              labelCSS="mb-0"
                              inputCSS="labelForm voda-bold mb-2"
                              isSearchable={true}
                              isClearable={true}
                              required={false}
                              value={
                                formData &&
                                formData?.infrastructureLocation != undefined
                                  ? infrastructureLocation.find(
                                      (x) =>
                                        x.value.toLowerCase() ===
                                        formData?.infrastructureLocation?.toLowerCase()
                                    )
                                  : null
                              }
                              options={infrastructureLocation}
                              onChange={(e: any) =>
                                onChangeDropdown("infrastructureLocation", e)
                              }
                            />
                          </div>
                        </div>

                        <div className="form-group col-4 pl-0">
                          <div className="col-12">
                            <ToggleInputComponent
                              label={`${
                                LabelsDictionary["exposedEdgeFlag"]?.Full ??
                                "exposedEdgeFlag"
                              }`}
                              value={formData?.exposedEdgeFlag ?? false}
                              required={false}
                              onChange={(e: any) =>
                                onChange("exposedEdgeFlag", e)
                              }
                            />
                          </div>
                        </div>
                        <div className="form-group col-4 pl-0">
                          <div className="col-12">
                            <ToggleInputComponent
                              label={`${
                                LabelsDictionary["externalFacingFlag"]?.Full ??
                                "externalFacingFlag"
                              }`}
                              value={formData?.externalFacingFlag ?? false}
                              required={false}
                              onChange={(e: any) =>
                                onChange("externalFacingFlag", e)
                              }
                            />
                          </div>
                        </div>
                      </div>
                    </fieldset>
                  </div>
                </Tab>
              )}
            </Tabs>
          </form>
          <div className="col-12 justify-content-end d-flex footerModal">
            {/* <button
                            className="  voda-bold btn btn-link px-4 btnHeader cancel"
                            onClick={() => {
                                props.action.closeModal(true);
                            }}
                            type="button"
                        >
                            Cancel
                        </button> */}
            {keyTabs !== "projectDetails" && (
              <button
                className="  voda-bold btn btn-link px-4 btnHeader cancel-mr"
                type="button"
                onClick={() => handlePreviousTab()}
              >
                Previous
              </button>
            )}
            {(props.opCo !== "UK" &&
            props.opCo !== "GROUP" &&
            props.opCo !== "ZZZ"
              ? keyTabs !== "operationalDetails"
              : keyTabs !== "tsrAttributes") && (
              <button
                className="  voda-bold btn btn-link px-4 btnHeader cancel-mr"
                type="button"
                onClick={() => handleNextTab()}
              >
                Next
              </button>
            )}
            {(props.opCo !== "UK" &&
            props.opCo !== "GROUP" &&
            props.opCo !== "ZZZ"
              ? keyTabs === "operationalDetails"
              : keyTabs === "tsrAttributes") && (
              <button
                className="  voda-bold btn btn-danger px-4 btnHeader"
                type="button"
                onClick={() => onSaveAudit()}
              >
                Save
              </button>
            )}
          </div>
        </>
      )}
    </div>
  );
};

export default LcmEngAuditModal;
