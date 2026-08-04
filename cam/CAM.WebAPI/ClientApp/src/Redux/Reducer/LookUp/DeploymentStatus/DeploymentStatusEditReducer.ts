import { LookUpEdit, LookUpForDeploymentStatusEdit } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: LookUpForDeploymentStatusEdit = {
	LookUpDtoEdit: null,
	ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const DeploymentStatusEditReducer = (state = initState, action: { type: string; payload: LookUpForDeploymentStatusEdit }) => {
	switch (action.type) {
		case "EDIT_DEPLOYMENT_STATUS": {
			return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
		}
		case "GET_EDIT_DEPLOYMENT_STATUS":
			return { ...state, LookUpDtoEdit: action.payload.LookUpDtoEdit };
		default:
			return state;
	}
};
