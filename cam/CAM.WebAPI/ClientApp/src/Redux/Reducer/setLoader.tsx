import { useSelector } from "react-redux";
import { RootState } from "../Store/rootStore";

const initialState = {
	show: false,
};

export const LoaderReducer = (state = initialState, action: { type: string; payload: boolean }) => {
	const GridDto = useSelector((state: RootState) => state.loaderReducer.listOfCall);
	if (GridDto.length > 0) {
		return { show: true };
	} else {
		return { show: false };
	}
};
