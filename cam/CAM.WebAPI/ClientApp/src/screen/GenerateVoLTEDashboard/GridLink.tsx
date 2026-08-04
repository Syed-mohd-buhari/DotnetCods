import React from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";

import { Link } from "react-router-dom";
import { formatTimeLocal } from "../../Hook/Common";

interface Props {
	to: {
		pathname: string;
		search: string;
		state: { id?: number; tab: string; prevPage: string; idDetail?: number };
	};
	title?: string;
	classes?: string;
}

const GridLink: React.FunctionComponent<Props> = (props) => {
	let content:any = {};
	const classes = props.classes ? props.classes : "linkTo";

	if (props.title?.trim()) {
		content = <label style={{ cursor: "pointer" }} dangerouslySetInnerHTML={{ __html: props.title }} />;
	} else {
		content = <label>---</label>;
	}

	return (
		<Link to={{pathname: `/${props.to.pathname}`, search: props.to.search}} state={props.to.state} className={classes}>
			{content}
		</Link>
	);
};

export default GridLink;
