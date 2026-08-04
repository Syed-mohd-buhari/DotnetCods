import { LookUpCreate, LookUpForDeploymentStatusCreate, LookUpGridForDeploymentStatus } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: LookUpForDeploymentStatusCreate = {
	ResultDtoCreate: null,
	LookUpDtoCreate: null,
};
//const dispatch = useDispatch();

export const DeploymentStatusCreateReducer = (state = initState, action: { type: string; payload: LookUpForDeploymentStatusCreate }) => {
	switch (action.type) {
		case "CREATE_DEPLOYMENT_STATUS": {
			return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
		}
		case "GET_CREATE_DEPLOYMENT_STATUS":
			return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate };
		default:
			return state;
	}
};
