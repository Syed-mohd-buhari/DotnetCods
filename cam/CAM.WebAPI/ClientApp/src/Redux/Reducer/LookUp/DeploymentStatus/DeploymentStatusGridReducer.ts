import { LookUpGridForDeploymentStatus } from "../../../../Model/LookUp/LookUpGenericModel";
// import { initState } from "../ActivityStatus/ActivityStatusGridReducer";
const initState: LookUpGridForDeploymentStatus = {
	LookUpGridResult: null,
	LookUpGridResultAll: null,
	filter: null,
};
// const dispatch = useDispatch();
// const dispatch = useDispatch();

export const DeploymentStatusGridReducer = (state = initState, action: { type: string; payload: LookUpGridForDeploymentStatus }) => {
	switch (action.type) {
		case "GET_GRID_DEPLOYMENT_STATUS": {
			return { ...state, LookUpGridResult: action.payload.LookUpGridResult };
		}
		case "GET_GRID_DEPLOYMENT_STATUS_ALL": {
			return { ...state, LookUpGridResultAll: action.payload.LookUpGridResult };
		}
		case "GET_FILTER_DEPLOYMENT_STATUS":
			return { ...state, filter: action.payload.filter };
		default:
			return state;
	}
};
