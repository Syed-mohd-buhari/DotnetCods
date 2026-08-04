import { ResultDto } from "../CommonModels";
import { TipologicaQueryDtoForVirtualized } from "./LookUpGenericModel";
import { TipologicaGridDtoForVirtualized } from "./PlannedActivityResource";

export interface ActivityDetailsDtoGrid {
	forVirtualized: Array<boolean>;
	id?: Array<number>;
	description?: Array<string>;
	sortBy?: string;
	isSortAscending?: boolean;
	page?: number;
	pageSize?: number;
	lastModifiedStartDate?: Date;
	lastModifiedEndDate?: Date;
	principalId?: number;
	deleted?: boolean;
	orphan?: boolean;
	lastModifiedBy?: Array<string>;
	options?: any;
}

export interface ActivityDetailsFilterDto {
	propertyName?: string;
	propertyFilter?: string;
	forVirtualized?: Array<boolean>;
	id?: Array<number>;
	description?: Array<string>;
	sortBy?: string;
	isSortAscending?: boolean;
	page?: number;
	pageSize?: number;
	lastModifiedStartDate?: Date;
	lastModifiedEndDate?: Date;
	principalId?: number;
	deleted?: boolean;
	orphan?: boolean;
	lastModifiedBy?: Array<string>;
	options?: any;
}

export interface LookUpEditActivityDetails {
	LookUpDtoEdit: TipologicaGridDtoForVirtualized | null;
	ResultDtoEdit: ResultDto | null;
}
export interface LookUpCreateActivityDetails {
	LookUpDtoCreate: TipologicaGridDtoForVirtualized | null;
	ResultDtoCreate: ResultDto | null;
}
