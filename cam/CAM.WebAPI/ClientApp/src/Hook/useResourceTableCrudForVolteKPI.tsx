import { useEffect, useState } from "react";
import { QueryObjectGrid } from "../Model/Common";
import { VolteKPIQueryObjectGrid } from "../Model/VolteKpi/VolteKPI";
import setLoader from "../Redux/Action/LoaderAction";

export function useResourceTableCrudForVolteKPI(paginationQuery: VolteKPIQueryObjectGrid, functionForRefillGrid: Function) {
	const [query, setQuery] = useState<VolteKPIQueryObjectGrid>(paginationQuery);
	// const next = () => {
	// 	const copy = { ...query };
	// 	if (copy.page !== undefined) {
	// 		copy.page = copy.page + 1;
	// 	}
	// 	setQuery(copy);
	// };

	// const back = () => {
	// 	const copy = { ...query };
	// 	if (copy.page !== undefined) {
	// 		copy.page = copy.page - 1;
	// 	}
	// 	setQuery(copy);
	// };

	useEffect(() => {
		setLoader("ADD", functionForRefillGrid.name);

		functionForRefillGrid(query).then((x) => setLoader("REMOVE", functionForRefillGrid.name));
	}, [functionForRefillGrid, query]);

	return { query, setQuery };
}

export function useResourceTableCrudForApprovalVolteKPI(functionForRefillGrid: Function) {
	const [query, setQuery] = useState<VolteKPIQueryObjectGrid>();

	useEffect(() => {
		setLoader("ADD", functionForRefillGrid.name);
		functionForRefillGrid(query).then((x) => setLoader("REMOVE", functionForRefillGrid.name));
	}, [functionForRefillGrid, query]);

	return { query, setQuery };
}
