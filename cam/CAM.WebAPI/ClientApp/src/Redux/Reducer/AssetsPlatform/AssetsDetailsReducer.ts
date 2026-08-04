// AssetsDetailsReducer.ts

import {
  AssetsDetailsGrid,
  GET_GRID_ASSETS_DETAILS,
  GET_GRID_ASSETS_RESULT,
  GET_FILTER_ASSETS_GRID_RESULT,
  QueryResultDtoOfDAAssetMigrationDtoGrid,
} from "../../../Model/LookUp/AssetMigrationModels";
import { ResultDto } from "../../../Model/CommonModels";
import { CREATE_ASSET_MIGRATION } from "../../../Model/NetworkElementAsPlanned";

interface ExtendedAssetsDetailsGrid extends AssetsDetailsGrid {
  ResultDtoCreate: ResultDto | null;
  daAssetMigrationDtoCreate: any | null; // replace `any` with correct type if you have it
}

const initialState: ExtendedAssetsDetailsGrid = {
  AssetsDetailsGridResult: null,
  filter: null,
  ResultDtoCreate: null,
  daAssetMigrationDtoCreate: null,
};

export const AssetsDetailsReducer = (
  state = initialState,
  action: any
): ExtendedAssetsDetailsGrid => {
  switch (action.type) {
    case GET_GRID_ASSETS_DETAILS:
      return {
        ...state,
        AssetsDetailsGridResult: action.payload.AssetsDetailsGridResult,
        filter: action.payload.filter,
      };

    case CREATE_ASSET_MIGRATION:
      return {
        ...state,
        ResultDtoCreate: action.payload.ResultDtoCreate,
        daAssetMigrationDtoCreate: action.payload.daAssetMigrationDtoCreate,
      };

    default:
      return state;
  }
};

export interface DAAssetsMigrationGrid {
  DAAssetsMigrationGridResult: QueryResultDtoOfDAAssetMigrationDtoGrid | null;
  filter: null;
}

const initState: DAAssetsMigrationGrid = {
  DAAssetsMigrationGridResult: null,
  filter: null,
};

export const DAAssetsMigrationGridReducer = (
  state = initState,
  action: {
    type: string;
    payload: DAAssetsMigrationGrid;
  }
) => {
  switch (action.type) {
    case GET_GRID_ASSETS_RESULT: {
      return {
        ...state,
        DAAssetsMigrationGridResult: action.payload.DAAssetsMigrationGridResult,
      };
    }
    case GET_FILTER_ASSETS_GRID_RESULT:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
