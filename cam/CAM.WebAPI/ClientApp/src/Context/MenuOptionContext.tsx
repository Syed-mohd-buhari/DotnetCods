import React, { createContext, useState, ReactNode, FC } from "react";

interface OptionContextType {
  selectedOption: string;
  setSelectedOption: (option: string) => void;
}

const initialOptionContext: OptionContextType = {
  selectedOption: "",
  setSelectedOption: () => {},
};

const OptionContext = createContext<OptionContextType>(initialOptionContext);

interface OptionProviderProps {
  children: ReactNode;
}

const OptionProvider: FC<OptionProviderProps> = ({ children }) => {
  const [selectedOption, setSelectedOption] = useState<string>("");

  return (
    <OptionContext.Provider value={{ selectedOption, setSelectedOption }}>
      {children}
    </OptionContext.Provider>
  );
};

export { OptionProvider, OptionContext };
