import React, { createContext, useState } from "react";
import { DataModalConfirm, stateConfirm } from "../Model/Common";

export interface ModalContextType {
  isVisibleModalManage: boolean;
  setIsVisibleModalManage: React.Dispatch<React.SetStateAction<boolean>>;
  isVisibleModalInitializeNewProduct: boolean;
  setIsVisibleModalInitializeNewProduct: React.Dispatch<
    React.SetStateAction<boolean>
  >;
  isVisibleModalProductLifecycle: boolean;
  setIsVisibleModalProductLifecycle: React.Dispatch<
    React.SetStateAction<boolean>
  >;
  isVisibleModalStatus: boolean;
  setIsVisibleModalStatus: React.Dispatch<React.SetStateAction<boolean>>;
  myConfirm: DataModalConfirm;
  onClickLink: (type?: string) => void;
}

const initialContext: ModalContextType = {
  isVisibleModalManage: false,
  setIsVisibleModalManage: () => {},
  isVisibleModalInitializeNewProduct: false,
  setIsVisibleModalInitializeNewProduct: () => {},
  isVisibleModalProductLifecycle: false,
  setIsVisibleModalProductLifecycle: () => {},
  isVisibleModalStatus: false,
  setIsVisibleModalStatus: () => {},
  myConfirm: stateConfirm,
  onClickLink: () => {},
};

export const ModalContext = createContext<ModalContextType>(initialContext);

export const ModalProvider: React.FC = ({ children }) => {
  const [isVisibleModalManage, setIsVisibleModalManage] =
    useState<boolean>(false);
  const [
    isVisibleModalInitializeNewProduct,
    setIsVisibleModalInitializeNewProduct,
  ] = useState<boolean>(false);
  const [isVisibleModalProductLifecycle, setIsVisibleModalProductLifecycle] =
    useState<boolean>(false);
  const [isVisibleModalStatus, setIsVisibleModalStatus] =
    useState<boolean>(false);
  const [myConfirm, setMyConfirm] = useState<DataModalConfirm>(stateConfirm);

  const onClickLink = (type?: string) => {
    switch (type) {
      case "contact":
        setMyConfirm({
          title: "Contact Us",
          item: 0,
          message:
            "For technical enquiries or issues relate to TEMS, please contact the the team via email at temsengineering@vodafone.com. We monitor our emails seven days a week, and one of the team will be back in contact with you as soon as possible.",
          isOpen: true,
          footerAlert: true,
          actions: {
            cancel: () => setMyConfirm(stateConfirm),
            confirm: async () => null,
          },
        });
        break;

      case "about":
        setMyConfirm({
          title: "About Us",
          item: 0,
          message:
            "The TEMS tool development is run as a VCE UK Project delivery on behalf of the wider VCE Community.",
          isOpen: true,
          footerAlert: true,
          actions: {
            cancel: () => setMyConfirm(stateConfirm),
            confirm: async () => null,
          },
        });
        break;
    }
  };

  return (
    <ModalContext.Provider
      value={{
        isVisibleModalManage,
        setIsVisibleModalManage,
        isVisibleModalInitializeNewProduct,
        setIsVisibleModalInitializeNewProduct,
        isVisibleModalProductLifecycle,
        setIsVisibleModalProductLifecycle,
        isVisibleModalStatus,
        setIsVisibleModalStatus,
        myConfirm,
        onClickLink,
      }}
    >
      {children}
    </ModalContext.Provider>
  );
};
