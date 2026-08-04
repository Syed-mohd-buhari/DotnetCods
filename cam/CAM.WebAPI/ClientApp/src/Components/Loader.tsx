import React from "react";
import { useSelector } from "react-redux";
import "../Css/App.css";
import { RootState, useAppSelector } from "../Redux/Store/rootStore";
import { createSelector } from "@reduxjs/toolkit";
import loader from "../img/loader.gif";

interface Props {
  show?: boolean | null;
  isFullScreen?: boolean;
}

const Loader: React.FC<Props> = ({ show, isFullScreen = true }) => {
  const getLoader = useAppSelector((state) => state.loaderReducer.show);
  let showLoader = show == null || show === undefined ? getLoader : show;

  return !showLoader ? (
    <></>
  ) : (
    <div
      className={`${
        isFullScreen ? "loaderContainer " : ""
      } justify-content-center align-items-center getLoader true fullWidthAbsolute`}
    >
      <img className="loaderContainer" alt="loader" src={loader} />
    </div>
  );
};
export default Loader;
