import React, { useCallback, useEffect, useRef, useState } from "react";

import { useSelector } from "react-redux";

import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { IoClose } from "react-icons/io5";

import Paginate from "../../Components/PaginationComponent";
import { useAuth } from "../../Hook/useAuth";
import { useResourceTableCrud } from "../../Hook/useResourceTableCrud";
import { useOperationTableCrud } from "../../Hook/useOperationTableCrud";
import {
  fetchNewElementNamesDirectly,
  GetAssetsDetailsGrid,
  GetAssetsPlatformMigrationGrid,
} from "../../Redux/Action/AssetsPlatform/AssetsDetailsGridAction";
import { RootState } from "../../Redux/Store/rootStore";
import AssetsDetailsGrid from "../../screen/Lookup/AssetPlatform/AssetsDetailsGrid";
import AssetsDetailsForm from "../../screen/Lookup/AssetPlatform/AssetsDetailsForm";
import {
  AssetMigrationApiResponse,
  DaAssetMigrationDto,
  DaAssetMigrationGrid,
  QueryResultDtoOfDAAssetMigrationDtoGrid,
} from "../../Model/LookUp/AssetMigrationModels";
import { CustomGridRender } from "../../Model/Common";
import { stateConfirm } from "../../Model/Common";
import ModalConfirm from "../../Components/ModalConfirm";

const generateUniqueId = () =>
  `uid-${Date.now()}-${Math.random().toString(36).substr(2, 9)}`;

export const paginationQueryAssets: DaAssetMigrationGrid = {
  daAssetMigrationId: [],
  plannedActivityId: [],
  networkElementAsPlannedId: [],
  newelEmentName: [],
  targetDesignComponenetId: [],
  environmentDesc: [],
  deploymentStatusDesc: [],
  opcoDesc: [],
  opcoId: [],
  locationDesc: [],
  rfoDate: undefined,
  rfsDate: undefined,
  migrationCompletionDate: undefined,
  trafficNodePercentage: [],
  oldAssetName: [],
  currentDesignComponenet: [],
  targetDesignComponenet: [],
  newEnvironment: [],
  newDeploymentStatus: [],
  location: [],
  oldEnvironment: [],
  oldDeploymentType: [],
  oldDeploymentStatus: [],
  currentDcfId: [],
  sortBy: "",
  isSortAscending: false,
  page: 1,
  pageSize: 1000,
  principalId: undefined,
};

interface Props {
  modal?: { isModal: boolean; setIsVisibleModalLookup(value: number): any };
  opcoId?: number;
  dcfId?: number;
  plannedDcfId?: number;
  paId?: number;
  initialAssets?: DaAssetMigrationDto[];
  onAssetsChange?: (assets: DaAssetMigrationDto[]) => void;
  isAssetDetailContainer?: boolean;
  skipInitialApiCall?: boolean;
  decommissionAll?: boolean;
}

interface ExtendedDaAssetMigrationDto extends DaAssetMigrationDto {
  source?: "API" | "LOCAL";
  uniqueId?: string;
}

const AssetsDetailsContainer: React.FC<Props> = (props) => {
  const { isPermesso } = useAuth();
  const pageSize = 1000;
  const [apiDataByPage, setApiDataByPage] = useState<
    Map<number, ExtendedDaAssetMigrationDto[]>
  >(new Map());
  const [localAssets, setLocalAssets] = useState<ExtendedDaAssetMigrationDto[]>(
    []
  );
  const [renderGridState, setRenderGridState] = useState<CustomGridRender>();
  const [selectedAsset, setSelectedAsset] =
    useState<ExtendedDaAssetMigrationDto>();
  const [changedState, setChangedState] = useState(false);
  const [isFiltriAttivati, setIsFiltriAttivati] = useState(false);
  const [isFormLoading, setIsFormLoading] = useState(false);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalItems, setTotalItems] = useState(0);
  const [apiTotalItems, setApiTotalItems] = useState(0);
  const [editedApiAssetUiIds, setEditedApiAssetUiIds] = useState<Set<number>>(
    new Set()
  );
  const [existingNewElementNames, setExistingNewElementNames] = useState<
    string[]
  >([]);
  const [confirm, setConfirm] = useState(stateConfirm);

  const prevIsFiltriAttivatiRef = useRef<boolean>(false);

  const initialQuery: DaAssetMigrationGrid = {
    ...paginationQueryAssets,
    pageSize: 1000,
    page: 1,
    plannedActivityId: props.paId ? [props.paId] : [],
    currentDcfId: props.dcfId ? [props.dcfId] : [],
    opcoId: props.opcoId ? [props.opcoId] : [],
  };

  const GridDto: QueryResultDtoOfDAAssetMigrationDtoGrid | null = useSelector(
    (state: RootState) =>
      state.daassetsMigrationGridReducer.DAAssetsMigrationGridResult
  );

  const gridResult: AssetMigrationApiResponse | null = useSelector(
    (state: RootState) => state.assetsDetailsReducer.AssetsDetailsGridResult
  );

  const { query, setQuery, updatePageSize } = useResourceTableCrud(
    { ...initialQuery, pageSize: 1000 },
    isPermesso && !props.skipInitialApiCall
      ? GetAssetsPlatformMigrationGrid
      : undefined
  );
  useEffect(() => {
    updatePageSize(1000);
  }, []);

  const getAllApiData = useCallback((): ExtendedDaAssetMigrationDto[] => {
    const allData: ExtendedDaAssetMigrationDto[] = [];
    apiDataByPage.forEach((pageData) => {
      allData.push(...pageData);
    });
    return allData;
  }, [apiDataByPage]);

  const getAllData = useCallback((): ExtendedDaAssetMigrationDto[] => {
    const allApiData = getAllApiData();

    const cleanApiData = allApiData.filter(
      (a) => !a.uniqueIdForUi || !editedApiAssetUiIds.has(a.uniqueIdForUi)
    );

    return [...localAssets, ...cleanApiData];
  }, [getAllApiData, localAssets, editedApiAssetUiIds]);

  // const getPaginatedData = useCallback((): ExtendedDaAssetMigrationDto[] => {
  //   const allData = getAllData();

  //   const sorted = [...allData].sort((a, b) => {
  //     if (a.source === "LOCAL" && b.source === "API") return -1;
  //     if (a.source === "API" && b.source === "LOCAL") return 1;
  //     if (a.source === "API" && b.source === "API") {
  //       return (a.daAssetMigrationId || 0) - (b.daAssetMigrationId || 0);
  //     }
  //     return 0;
  //   });

  //   const startIndex = (currentPage - 1) * pageSize;
  //   const endIndex = startIndex + pageSize;
  //   return sorted.slice(startIndex, endIndex);
  // }, [getAllData, currentPage, pageSize]);

  const getSortedData = useCallback((): ExtendedDaAssetMigrationDto[] => {
    const allData = getAllData();

    return [...allData].sort((a, b) => {
      if (a.source === "LOCAL" && b.source === "API") return -1;
      if (a.source === "API" && b.source === "LOCAL") return 1;
      if (a.source === "API" && b.source === "API") {
        return (a.daAssetMigrationId || 0) - (b.daAssetMigrationId || 0);
      }
      return 0;
    });
  }, [getAllData]);

  const fetchExistingNewElementNames = useCallback(async () => {
    try {
      const names = await fetchNewElementNamesDirectly(
        props.paId || 0,
        props.opcoId,
        props.dcfId
      );
      setExistingNewElementNames(names);
    } catch (error) {
      console.error("Error fetching new element names:", error);
      setExistingNewElementNames([]);
    }
  }, [props.paId, props.opcoId, props.dcfId]);

  useEffect(() => {
    setTotalItems(apiTotalItems + localAssets.length);
  }, [apiTotalItems, localAssets.length]);

  useEffect(() => {
    if (props.initialAssets && props.initialAssets.length > 0) {
      const apiAssets: ExtendedDaAssetMigrationDto[] = [];
      const localAssetsTemp: ExtendedDaAssetMigrationDto[] = [];
      const editedUiIds = new Set<number>();

      props.initialAssets.forEach((asset) => {
        const uniqueId = asset.uniqueId || generateUniqueId();
        const source =
          (asset as any)?.source || (asset.uniqueIdForUi ? "API" : "LOCAL");

        const enhancedAsset: ExtendedDaAssetMigrationDto = {
          ...asset,
          uniqueId,
          source,
          isDecommissioned: props.decommissionAll
            ? true
            : asset.isDecommissioned,
        };

        if (enhancedAsset.uniqueIdForUi) {
          if (enhancedAsset.source === "LOCAL") {
            localAssetsTemp.push(enhancedAsset);
            editedUiIds.add(enhancedAsset.uniqueIdForUi);
          } else {
            apiAssets.push(enhancedAsset);
          }
        } else {
          localAssetsTemp.push(enhancedAsset);
        }
      });

      const newMap = new Map();
      if (apiAssets.length > 0) {
        newMap.set(1, apiAssets);
      }
      setApiDataByPage(newMap);
      setLocalAssets(localAssetsTemp);
      setEditedApiAssetUiIds(editedUiIds);
      setCurrentPage(1);
    } else {
      setApiDataByPage(new Map());
      setLocalAssets([]);
      setEditedApiAssetUiIds(new Set());
    }
  }, [props.initialAssets, props.decommissionAll]);

  useEffect(() => {
    if (!GridDto?.items) return;

    const mapped: any[] = GridDto.items
      .filter((item) => {
        const isEdited = item.uniqueIdForUi
          ? editedApiAssetUiIds.has(item.uniqueIdForUi)
          : false;
        return !isEdited;
      })
      .map((item) => ({
        ...item,
        uniqueId: (item as any).uniqueId ?? generateUniqueId(),
        source: "API" as const,
        isDecommissioned: props.decommissionAll ? true : item.isDecommissioned,
      }));

    const newMap = new Map();
    newMap.set(1, mapped);
    setApiDataByPage(newMap);

    setRenderGridState(GridDto.gridRender as CustomGridRender);

    if (GridDto.totalItems !== undefined) {
      setApiTotalItems(GridDto.totalItems);
    }
  }, [GridDto, editedApiAssetUiIds, props.decommissionAll]);

  useEffect(() => {
    if (prevIsFiltriAttivatiRef.current && !isFiltriAttivati) {
      setApiDataByPage(new Map());
      if (props.initialAssets) {
        const apiAssets: ExtendedDaAssetMigrationDto[] = [];
        props.initialAssets.forEach((asset) => {
          if (asset.uniqueIdForUi && (asset as any).source !== "LOCAL") {
            apiAssets.push({
              ...asset,
              uniqueId: asset.uniqueId || generateUniqueId(),
              source: "API",
            });
          }
        });
        const newMap = new Map();
        if (apiAssets.length > 0) {
          newMap.set(1, apiAssets);
        }
        setApiDataByPage(newMap);
      }
    }
    prevIsFiltriAttivatiRef.current = isFiltriAttivati;
  }, [isFiltriAttivati, props.initialAssets]);

  useEffect(() => {
    const hasFilters = Object.keys(query).some((key) => {
      if (
        key === "page" ||
        key === "pageSize" ||
        key === "sortBy" ||
        key === "isSortAscending"
      ) {
        return false;
      }
      const value = query[key as keyof DaAssetMigrationGrid];
      if (Array.isArray(value)) {
        return value.length > 0;
      }
      return value !== undefined && value !== null && value !== "";
    });

    if (!hasFilters && isFiltriAttivati) {
      setIsFiltriAttivati(false);
    }
  }, [query, isFiltriAttivati]);

  useEffect(() => {
    if (query.page !== undefined && query.page !== currentPage) {
      setCurrentPage(query.page);
    }
  }, [currentPage, query.page]);

  const fetchAssetsDetailsGrid = useCallback(
    (daMigrationId: number = 0) =>
      GetAssetsDetailsGrid(
        props.opcoId,
        props.dcfId,
        props.plannedDcfId,
        daMigrationId
      ),
    [props.opcoId, props.dcfId, props.plannedDcfId]
  );

  const { New, isVisibleModal, closeModal } = useOperationTableCrud<
    DaAssetMigrationDto,
    DaAssetMigrationDto
  >(
    async () => Promise.resolve(),
    async () => Promise.resolve(),
    async () => ({
      data: null,
      info: "Delete not implemented",
      warning: false,
    }),
    () => fetchAssetsDetailsGrid(0)
  );

  const handlePageChange = (pageNumber: number) => {
    const combinedTotal = localAssets.length + apiTotalItems;
    const totalPages = Math.ceil(combinedTotal / pageSize);
    pageNumber = Math.max(1, Math.min(pageNumber, totalPages));

    setCurrentPage(pageNumber);

    const itemsNeededUpToPage = pageNumber * pageSize;
    const localItemsCount = localAssets.length;
    const localItemsIncluded = Math.min(localItemsCount, itemsNeededUpToPage);
    const apiItemsNeeded = Math.max(
      0,
      itemsNeededUpToPage - localItemsIncluded
    );
    const apiPageNeeded = Math.ceil(apiItemsNeeded / pageSize) || 1;

    if (apiPageNeeded !== query.page) {
      setQuery((prev) => ({
        ...prev,
        page: apiPageNeeded,
      }));
    }
  };

  const handleAddAsset = async () => {
    setSelectedAsset(undefined);
    setIsFormLoading(true);
    try {
      await fetchAssetsDetailsGrid(0);
      await fetchExistingNewElementNames();
      New();
    } finally {
      setIsFormLoading(false);
    }
  };

  const handleEdit = async (uniqueId: string) => {
    const allData = getAllData();
    const asset = allData.find((x) => x.uniqueId === uniqueId);

    if (!asset) return;

    const editAsset: ExtendedDaAssetMigrationDto = {
      ...asset,
      uniqueId: asset.uniqueId || generateUniqueId(),
    };

    setSelectedAsset(editAsset);
    setIsFormLoading(true);

    try {
      await fetchAssetsDetailsGrid(asset.daAssetMigrationId ?? 0);
      await fetchExistingNewElementNames();
      New();
    } finally {
      setIsFormLoading(false);
    }
  };

  const handleFormSuccess = (asset: DaAssetMigrationDto) => {
    const editUniqueId = selectedAsset?.uniqueId;

    if (editUniqueId) {
      setApiDataByPage((prev) => {
        const newMap = new Map(prev);
        newMap.forEach((pageData, pageNum) => {
          const filtered = pageData.filter(
            (item) => item.uniqueId !== editUniqueId
          );
          newMap.set(pageNum, filtered);
        });
        return newMap;
      });

      const editedAsset: ExtendedDaAssetMigrationDto = {
        ...asset,
        uniqueId: editUniqueId,
        daAssetMigrationId: selectedAsset?.daAssetMigrationId ?? 0,
        uniqueIdForUi: selectedAsset?.uniqueIdForUi,
        plannedActivityId: props.paId ?? 0,
        opcoId: props.opcoId ?? 0,
        source: "LOCAL",
      };

      setLocalAssets((prev) => [
        editedAsset,
        ...prev.filter((item) => item.uniqueId !== editUniqueId),
      ]);

      if (selectedAsset?.uniqueIdForUi) {
        setEditedApiAssetUiIds((prev) =>
          new Set(prev).add(selectedAsset.uniqueIdForUi!)
        );
      }
    } else {
      const newAsset: ExtendedDaAssetMigrationDto = {
        ...asset,
        uniqueId: generateUniqueId(),
        daAssetMigrationId: 0,
        uniqueIdForUi: undefined,
        plannedActivityId: props.paId ?? 0,
        opcoId: props.opcoId ?? 0,
        source: "LOCAL",
      };

      setLocalAssets((prev) => [newAsset, ...prev]);
      setCurrentPage(1);
    }

    setChangedState(true);
    closeModal(false);
  };

  const handleDelete = async (uniqueId: string) => {
    const allItems = getAllData();
    const assetToDelete = allItems.find((x) => x.uniqueId === uniqueId);

    if (!assetToDelete) return;

    const isLocal = assetToDelete.source === "LOCAL";
    const isApi = assetToDelete.source === "API";

    if (isLocal) {
      setLocalAssets((prev) =>
        prev.filter((item) => item.uniqueId !== uniqueId)
      );
    }

    if (isApi) {
      if (assetToDelete.uniqueIdForUi !== undefined) {
        setEditedApiAssetUiIds((prev) =>
          new Set(prev).add(assetToDelete.uniqueIdForUi!)
        );
      }

      setApiDataByPage((prev) => {
        const newMap = new Map(prev);
        newMap.forEach((pageData, pageNum) => {
          const filtered = pageData.filter(
            (item) => item.uniqueId !== uniqueId
          );
          newMap.set(pageNum, filtered);
        });
        return newMap;
      });
    }

    setChangedState(true);

    setTimeout(() => {
      const updatedTotal =
        (isLocal ? localAssets.length - 1 : localAssets.length) +
        (isApi ? apiTotalItems - 1 : apiTotalItems);

      const totalPages = Math.max(1, Math.ceil(updatedTotal / pageSize));

      if (currentPage > totalPages) {
        handlePageChange(totalPages);
      }
    }, 0);
  };

  const removeDuplicateAssets = useCallback(
    (assets: ExtendedDaAssetMigrationDto[]): ExtendedDaAssetMigrationDto[] => {
      const seenUiIds = new Set<number>();
      const result: ExtendedDaAssetMigrationDto[] = [];

      for (const asset of assets) {
        if (asset.uniqueIdForUi) {
          if (!seenUiIds.has(asset.uniqueIdForUi)) {
            seenUiIds.add(asset.uniqueIdForUi);
            result.push(asset);
          }
        } else {
          result.push(asset);
        }
      }

      return result;
    },
    []
  );

  const handleSaveAssets = () => {
    const allAssets = getAllData();
    const uniqueAssets = removeDuplicateAssets(allAssets);
    const invalidAssets = allAssets.filter(
      (a) =>
        (!a.newelEmentName || a.newelEmentName.trim() === "") &&
        (a.isDecommissioned === false || !a.isDecommissioned)
    );

    if (invalidAssets.length > 0) {
      setConfirm({
        title: "Confirm Save",
        message: `One or more assets does not have a plan. Do you want to proceed with saving?`,
        button: "Save",
        item: "",
        isOpen: true,
        actions: {
          confirm: () => {
            props.onAssetsChange?.(uniqueAssets);
            props.modal?.setIsVisibleModalLookup(0);
            setConfirm(stateConfirm);
          },
          cancel: () => setConfirm(stateConfirm),
        },
      });
    } else {
      props.onAssetsChange?.(uniqueAssets);
      props.modal?.setIsVisibleModalLookup(0);
    }
  };

  // const paginatedData = getPaginatedData();
  const sortedData = getSortedData();

  return (
    <div className={props.modal?.isModal ? "container" : "pageContainer"}>
      <ModalConfirm data={confirm} showHyperLink={false} />
      <Dialog
        open={isVisibleModal}
        maxWidth="md"
        fullWidth
        onClose={() => closeModal(false)}
      >
        <DialogTitle>
          {selectedAsset ? "Edit Asset Details" : "Add Asset Details"}
        </DialogTitle>

        <IconButton
          onClick={() => closeModal(false)}
          sx={{ position: "absolute", right: 8, top: 8 }}
        >
          <IoClose size={24} />
        </IconButton>

        <DialogContent>
          {isFormLoading ? (
            <div className="text-center p-4">
              <div className="spinner-border text-primary" role="status">
                <span className="visually-hidden">Loading...</span>
              </div>
              <p className="mt-2">Loading form data...</p>
            </div>
          ) : gridResult ? (
            <AssetsDetailsForm
              existingData={selectedAsset}
              edit={!!selectedAsset}
              paId={props.paId ?? 0}
              opcoId={props.opcoId ?? 0}
              existingAssetResource={gridResult.existingAssetResource ?? []}
              locationResource={gridResult.locationReosurce ?? {}}
              environmentResource={gridResult.environmentReosurce ?? {}}
              targetDesignComponentResource={
                gridResult.targetDesignComponentResource ?? []
              }
              deploymentStatusResource={
                gridResult.deploymentStatusReosurce ?? {}
              }
              existingNewElementNames={existingNewElementNames}
              localAssets={localAssets}
              defaultDecommissioned={props.decommissionAll}
              action={{
                closeModal,
                refresh: () => fetchAssetsDetailsGrid(0),
                setChanged: setChangedState,
                onSubmit: handleFormSuccess,
              }}
            />
          ) : (
            <div className="text-center p-4">
              <p>Unable to load form data. Please try again.</p>
            </div>
          )}
        </DialogContent>
      </Dialog>

      <div className="headerPage row mx-0 justify-content-between">
        <h3 className="voda-bold text-dark">Assets Migration Details</h3>
        {/* <button
          className="voda-bold btn btn-danger px-4"
          onClick={handleAddAsset}
          disabled={isFormLoading}
        >
          {isFormLoading ? "Loading..." : "Add Asset"}
        </button> */}
      </div>

      <AssetsDetailsGrid
        data={sortedData as any}
        pagination={query}
        renderGrid={renderGridState?.render ?? []}
        action={{
          Filter: setQuery,
          setIsFiltriAttivati,
          Edit: handleEdit,
          onDelete: handleDelete,
          isEnable: true,
        }}
      />

      {/* <div className="mt-3">
        <Paginate
          pagination={{
            page: currentPage,
            pageSize: pageSize,
          }}
          totalItems={totalItems}
          actions={{
            next: handlePageChange,
            back: handlePageChange,
          }}
        />
      </div> */}

      {props.modal?.isModal && (
        <div className="col-12 justify-content-end d-flex footerModal">
          <button
            className="voda-bold btn btn-link px-4 btnHeader cancel"
            onClick={() => props.modal?.setIsVisibleModalLookup(0)}
            disabled={isFormLoading}
          >
            Cancel
          </button>
          <button
            className="voda-bold btn btn-danger px-4 btnHeader"
            disabled={!changedState || isFormLoading}
            onClick={handleSaveAssets}
          >
            Save Assets
          </button>
        </div>
      )}
    </div>
  );
};

export default AssetsDetailsContainer;
