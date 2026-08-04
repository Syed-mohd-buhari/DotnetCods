import React, { useEffect, useState } from "react";
import Select from "react-select";
import { UserLogLevelsBody } from "../../Model/UsersLoggingLevels";
import { setNotification } from "../../Redux/Action/NotificationAction";
import {
  GetCreatePageForUsersLoggingLevels,
  UserDownloadLogs,
  UserLoggingLevelCreate,
} from "../../Redux/Action/UserLoggingLevel/userLoggingLevelActions";
import { NotifyType } from "../../Redux/Reducer/NotificationReducer";
import { rootStore } from "../../Redux/Store/rootStore";
import { useAuth } from "./../../Hook/useAuth";
import "./usersLogLevel.css";

const UsersLogLevels = () => {
  const [loggingLevels, setLoggingLevels] = useState<Array<string>>([]);
  const [selectedLoggingLevels, setSelectedLoggingLevels] = useState<any>("");
  const [submitted, setSubmitted] = useState<boolean>(false);
  const [loggingValue, setLoggingValue] = useState<string>();
  const { readonly, isPermesso } = useAuth();

  const onDownloadLogFile = async () => {
    let result = await UserDownloadLogs();
    if (result !== undefined) {
      const redirectUri: any = process.env.REACT_APP_REDIRECT_URL;
      let url = window.URL.createObjectURL(result.file);
      let a = document.createElement("a");
      a.href = url;
      a.download = "TEMSLogs.log";
      a.click();
    }
  };

  const onChangeRolles = (e) => {
    if (e && e.value) {
      setSelectedLoggingLevels(e.value);
      setLoggingValue(e.value);
    } else {
      setSelectedLoggingLevels("");
      setLoggingValue("");
    }
  };

  useEffect(() => {
    GetCreatePageForUsersLoggingLevels().then((res) => {
      if (res?.allLoggingLevels) {
        setLoggingLevels(res.allLoggingLevels.split(","));
      }
      setLoggingValue(res.currentLogLevel);
    });
  }, []);

  const createLogLevel = () => {
    setSubmitted(true);
    const data = {
      currentLogLevel: selectedLoggingLevels,
    } as UserLogLevelsBody;

    if (data.currentLogLevel) {
      UserLoggingLevelCreate(data)
        .then((res) => {
          rootStore.dispatch(
            setNotification({
              message: `${selectedLoggingLevels} Log level created successfully`,
              notifyType: NotifyType.success,
            })
          );
        })
        .catch((err) => {
          setNotification({
            message: "Error Occurs !",
            notifyType: NotifyType.error,
          });
        });
    } else {
    }
  };

  return (
    <div className="pageContainer pt-4">
      <div className="row">
        <div className="col-6">
          <label className="themeText voda-bold w-100 text-left">
            Log Levels<span className="red">*</span>
            <Select
              menuPosition={"fixed"}
              options={
                loggingLevels &&
                loggingLevels.map((item) => ({
                  value: item,
                  id: item,
                }))
              }
              value={
                loggingLevels &&
                loggingLevels
                  .map((item) => ({
                    value: item,
                    id: item,
                  }))
                  .filter((item) => item.value === loggingValue)
              }
              onChange={(e) => onChangeRolles(e)}
              isSearchable
              isClearable
              getOptionLabel={(option) => option.value}
              getOptionValue={(option) => option.id}
            />
          </label>
        </div>

        {!readonly && (
          <div className="col-6 mt-1">
            <div className="justify-content-start mt-4 d-flex footerModal">
              <button
                className="voda-bold btn btn-danger px-4 btnHeader"
                type="button"
                onClick={createLogLevel}
              >
                Apply
              </button>
              <button
                onClick={onDownloadLogFile}
                className="download-to-excel mrl-10"
              >
                Download Log Files
              </button>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default UsersLogLevels;
