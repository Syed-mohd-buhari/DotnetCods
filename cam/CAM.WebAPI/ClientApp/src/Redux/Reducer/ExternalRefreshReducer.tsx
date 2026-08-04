import { useSelector } from "react-redux";
import { RootState } from "../Store/rootStore";

const initialState = {
	refresh: false,
};

export const ExternalRefreshReducer = (state = initialState, action: { type: string, payload: boolean }) => {
	switch (action.type) {
		case 'REFRESH':
			return { refresh: action.payload };
		default:
			return state;
	}
};
