import { GridDtoBase, QueryObject, RenderDetail } from "../Common";
import { defaultSchema } from '../../Constant/JsonSchema';

/**
 * 
 * @export
 * @interface DeploymentStatusDto
 */
 export interface DeploymentStatusDto {
    /**
     * 
     * @type {number}
     * @memberof DeploymentStatusDto
     */
    deploymentStatusId?: number;
    /**
     * 
     * @type {string}
     * @memberof DeploymentStatusDto
     */
    deploymentStatusDescription?: string;
    /**
     * 
     * @type {number}
     * @memberof DeploymentStatusDto
     */
    rule?: number;
    /**
     * 
     * @type {Array<string>}
     * @memberof DeploymentStatusDto
     */
    plannedActivityResourceAllowed?: Array<string>;
    /**
     * 
     * @type {Array<number>}
     * @memberof DeploymentStatusDto
     */
    plannedActivityResourceAllowedId?: Array<number>;
    /**
     * 
     * @type {boolean}
     * @memberof DeploymentStatusDto
     */
    readOnlyPlannedActivity?: boolean;
    /**
     * 
     * @type {boolean}
     * @memberof DeploymentStatusDto
     */
    checkPlannedActivity?: boolean;
    /**
     * 
     * @type {boolean}
     * @memberof DeploymentStatusDto
     */
    defaultValue?: boolean;
    /**
     * 
     * @type {Date}
     * @memberof DeploymentStatusDto
     */
    lastModified?: Date;
    /**
     * 
     * @type {string}
     * @memberof DeploymentStatusDto
     */
    lastModifiedBy?: string;
    /**
     * 
     * @type {{ [key: string]: string; }}
     * @memberof DeploymentStatusDto
     */
    plannedActivityResources?: { [key: string]: string; };
}
/**
 * 
 * @export
 * @interface DeploymentStatusDtoGrid
 */
export interface DeploymentStatusDtoGrid extends GridDtoBase {
    /**
     * 
     * @type {number}
     * @memberof DeploymentStatusDtoGrid
     */
    deploymentStatusId?: number;
    /**
     * 
     * @type {string}
     * @memberof DeploymentStatusDtoGrid
     */
    deploymentStatusDescription?: string;
    /**
     * 
     * @type {number}
     * @memberof DeploymentStatusDtoGrid
     */
    rule?: number;
    /**
     * 
     * @type {string}
     * @memberof DeploymentStatusDtoGrid
     */
    plannedActivityResourceAllowed?: string;
    /**
     * 
     * @type {Array<number>}
     * @memberof DeploymentStatusDtoGrid
     */
    plannedActivityResourceAllowedId?: Array<number>;
    /**
     * 
     * @type {boolean}
     * @memberof DeploymentStatusDtoGrid
     */
    readOnlyPlannedActivity?: boolean;
    /**
     * 
     * @type {boolean}
     * @memberof DeploymentStatusDtoGrid
     */
    checkPlannedActivity?: boolean;
    /**
     * 
     * @type {boolean}
     * @memberof DeploymentStatusDtoGrid
     */
    defaultValue?: boolean;
    /**
     * 
     * @type {Date}
     * @memberof DeploymentStatusDtoGrid
     */
    lastModified?: Date;
    /**
     * 
     * @type {string}
     * @memberof DeploymentStatusDtoGrid
     */
    lastModifiedBy?: string;
}

/**
 *
 * @export
 * @interface CustomGridRenderOfDeploymentStatusDtoGrid
 */
export interface CustomGridRenderOfDeploymentStatusDtoGrid {
	/**
	 *
	 * @type {string}
	 * @memberof CustomGridRenderOfDeploymentStatusDtoGrid
	 */
	className?: string;
	/**
	 *
	 * @type {Array<RenderDetail>}
	 * @memberof CustomGridRenderOfDeploymentStatusDtoGrid
	 */
	render?: Array<RenderDetail>;
}

/**
 *
 * @export
 * @interface QueryResultDtoOfDeploymentStatusDtoGrid
 */
export interface QueryResultDtoOfDeploymentStatusDtoGrid {
	/**
	 *
	 * @type {number}
	 * @memberof QueryResultDtoOfDeploymentStatusDtoGrid
	 */
	totalItems?: number;
	/**
	 *
	 * @type {Array<DeploymentStatusDtoGrid>}
	 * @memberof QueryResultDtoOfDeploymentStatusDtoGrid
	 */
	items?: Array<DeploymentStatusDtoGrid>;
	/**
	 *
	 * @type {CustomGridRenderOfDeploymentStatusDtoGrid}
	 * @memberof QueryResultDtoOfDeploymentStatusDtoGrid
	 */
	gridRender?: CustomGridRenderOfDeploymentStatusDtoGrid;
}

// ------------Not AutoGen---------

export interface DeploymentStatusQuery extends QueryObject {
	deploymentStatusId?: Array<number>;
	deploymentStatusDescription?: Array<string>;
	rule?: Array<number>;
	plannedActivityResourceAllowed?: Array<string>;
	readOnlyPlannedActivity?: Array<boolean>;
	defaultValue?: Array<boolean>;
	checkPlannedActivity?: Array<boolean>;
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
