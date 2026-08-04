import React, { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import { RootState } from "../../Redux/Store/rootStore";
import { CreateTeams } from "../../Redux/Action/TeamManagement/TeamManagementCreateAction";
import { EditTeams } from "../../Redux/Action/TeamManagement/TeamManagementEditAction";
import { MultiSelectComponent } from "../../Components/FormField";
import { dictionaryToArray } from "../../Hook/Dictionary";

interface TeamsFormProps {
  isAdd: boolean;
  teamId?: number | null;
  action: {
    closeModal: () => void;
    refresh: () => void;
  };
}

const TeamsForm: React.FC<TeamsFormProps> = ({ isAdd, teamId, action }) => {
  const [teamName, setTeamName] = useState("");
  const [teamDescription, setTeamDescription] = useState("");
  const [error, setError] = useState("");
  const [saving, setSaving] = useState(false);

  const teamsCreateResource = useSelector(
    (state: RootState) => state.teamManagementCreateReducer.TeamsDtoCreate
  );
  const teamsEditResource = useSelector(
    (state: RootState) => state.teamManagementEditReducer.TeamsDtoEdit
  );

  const activeSource = isAdd ? teamsCreateResource : teamsEditResource;

  const userOptions = activeSource?.userResources
    ? dictionaryToArray(activeSource.userResources).map((item) => ({
        label: item.value,
        value: item.key,
      }))
    : [];

  const [selectedUserIds, setSelectedUserIds] = useState<number[]>([]);

  const onChangeUsers = (selected: any) => {
    setSelectedUserIds(selected ? selected.map((opt: any) => opt.value) : []);
  };

  const removeTeamMember = (userId: number) => {
    setSelectedUserIds((prev) => prev.filter((id) => id !== userId));
  };

  useEffect(() => {
    setTeamName(activeSource?.teamName ?? "");
    setTeamDescription(activeSource?.teamDescription ?? "");
    setSelectedUserIds(activeSource?.userIds ?? []);
  }, [isAdd, teamsCreateResource, teamsEditResource]);

  const teamMemberRows = selectedUserIds.map((id) => ({
    userId: id,
    userName: userOptions.find((opt) => opt.value === id)?.label ?? "",
  }));

  const handleSave = async () => {
    if (!teamName.trim()) {
      setError("Team name is required");
      return;
    }
    setSaving(true);
    try {
      if (isAdd) {
        await CreateTeams({
          teamName,
          teamDescription,
          userIds: selectedUserIds,
        });
      } else {
        await EditTeams({
          teamId: teamId ?? undefined,
          teamName,
          teamDescription,
          userIds: selectedUserIds,
        });
      }
      action.refresh();
      action.closeModal();
    } catch (err) {
      console.error("Team save failed", err);
      setError("Something went wrong while saving the team.");
    } finally {
      setSaving(false);
    }
  };

  return (
    <div className="row">
      <div className="col-6">
        <label className="voda-bold w-100 mt-2">
          Team Name
          <span className="red">*</span>
          <input
            type="text"
            className="inputForm w-100"
            value={teamName}
            onChange={(e) => {
              setTeamName(e.target.value);
              setError("");
            }}
          />
          {error && <span className="validation text-danger">{error}</span>}
        </label>
      </div>
      <div className="col-6">
        <label className="voda-bold w-100 mt-2">
          Team Description
          <textarea
            className="inputForm w-100"
            value={teamDescription}
            placeholder="Enter team description (optional)"
            onChange={(e) => setTeamDescription(e.target.value)}
          />
        </label>
      </div>
      <div className="col-12 mb-4">
        <fieldset className="fieldset">
          <legend className="voda-bold mb-3 fz-18">Team Members</legend>
          <div className="row">
            <div className="form-group col-6 mb-3">
              <MultiSelectComponent
                label={"User"}
                labelCSS={"mb-0"}
                inputCSS="labelForm voda-bold mb-2"
                isSearchable
                isClearable={false}
                required={false}
                value={userOptions.filter((opt) =>
                  selectedUserIds.includes(opt.value)
                )}
                options={userOptions}
                onChange={onChangeUsers}
              />
            </div>
          </div>

          <div className="row mt-3">
            <div className="w-100">
              <table className="w-100" style={{ borderCollapse: "collapse" }}>
                <thead>
                  <tr
                    className="head"
                    style={{ borderBottom: "1px solid #d9d9d9" }}
                  >
                    <th className="pl-2 ptb-12 text-left">Team Member</th>
                    <th className="pl-2 ptb-12 text-left">User Id</th>
                    <th className="pl-2 ptb-12 text-left">User</th>

                    <th className="pl-2 ptb-12"></th>
                  </tr>
                </thead>
                <tbody>
                  {teamMemberRows.length > 0 ? (
                    teamMemberRows.map((item, i) => (
                      <tr
                        className="dati"
                        key={item.userId}
                        style={{ borderBottom: "1px solid #eee" }}
                      >
                        <td className="pl-2 ptb-12">{`Member ${i + 1}`}</td>
                        <td className="pl-2 ptb-12">{item.userId}</td>

                        <td className="pl-2 ptb-12">{item.userName}</td>
                        <td className="pl-2 ptb-12">
                          <img
                            onClick={() => removeTeamMember(item.userId)}
                            className="btnEdit op-55"
                            src={require("../../img/delete.png")}
                            style={{ cursor: "pointer" }}
                          />
                        </td>
                      </tr>
                    ))
                  ) : (
                    <tr>
                      <td colSpan={4} className="text-center py-3">
                        No team members added
                      </td>
                    </tr>
                  )}
                </tbody>
              </table>
            </div>
          </div>
        </fieldset>
      </div>

      <div className="col-12 d-flex justify-content-end gap-2 mt-4">
        <button
          type="button"
          className="voda-bold btn btn-link px-4 btnHeader cancel"
          onClick={() => action.closeModal()}
        >
          Cancel
        </button>
        <button
          type="button"
          className="btn btn-danger"
          disabled={saving}
          onClick={handleSave}
        >
          Save
        </button>
      </div>
    </div>
  );
};

export default TeamsForm;
