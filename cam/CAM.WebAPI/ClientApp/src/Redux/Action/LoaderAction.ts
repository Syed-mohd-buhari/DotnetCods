import { rootStore } from "../Store/rootStore";

export default function setLoader(type: string, payload: String) {
	if (type) {
		rootStore.dispatch({ type, payload });
	}
}
