import { CREATE_NETWORK_ELEMENT_AS_IS, GET_CREATE_NETWORK_ELEMENT_AS_IS, NetworkElementAsIsCreate } from "../../../Model/NetworkElementAsIs";

const initState: NetworkElementAsIsCreate = {
	ResultDtoCreate: null,
	NetworkElementAsIsDtoCreate: null,
};
//const dispatch = useDispatch();

export const NetworkElementAsIsCreateReducer = (state = initState, action: { type: string; payload: NetworkElementAsIsCreate }) => {
	switch (action.type) {
		case CREATE_NETWORK_ELEMENT_AS_IS: {
			return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
		}
		case GET_CREATE_NETWORK_ELEMENT_AS_IS:
			return { ...state, NetworkElementAsIsDtoCreate: action.payload.NetworkElementAsIsDtoCreate };
		default:
			return state;
	}
};
