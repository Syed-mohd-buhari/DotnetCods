const ENV_TOKEN = process.env.REACT_APP_ENV_TOKEN;

const periodKey = `${ENV_TOKEN}_period`;
export const checkTokenExpiration = (): boolean => {
  const tokenPeriod: number = +localStorage.getItem(periodKey)!;

  if (tokenPeriod * 60000 < Date.now()) {
    return false;
  }
  return true;
};

interface Account {
  aadResponse: {
    jwtIdToken?: string;
    account: {
      name?: string;
      userid?: any;
      userName?: string;
      idToken?: string;
    };
  } | null;
  JWTBearer: string | null;
  permesso?: boolean | null;
  role: Array<string> | null;
  mode?: string;
  userPagePrefrenceDetails?: any;
  pagesize: number;
}
const initialState: Account = {
  aadResponse: null,
  JWTBearer: null,
  permesso: null,
  role: null,
  pagesize: 10,
  userPagePrefrenceDetails: null,
};

export const userReducer = (state = initialState, action: any) => {
  switch (action.type) {
    case "AAD_LOGIN_SUCCESS":
      return { ...state, aadResponse: action.payload };
    case "ACCESS_TOKEN_ASSIGN":
      return { ...state, JWTBearer: action.payload };
    case "AAD_LOGOUT_SUCCESS":
      return { ...state, aadResponse: null };
    case "AUTH_NOT_REGISTERED":
      return { ...state, permesso: false, role: null };
    case "AUTH_REGISTERED":
      return { ...state, permesso: true };
    case "AUTH_ROLE":
      return { ...state, role: action.payload };
    case "AUTH_USER_PREFERENCE":
      return { ...state, userPagePrefrenceDetails: action.payload };
    case "ADD_OPERATING_MODE":
      return { ...state, mode: action.payload };
    case "UPDATE_PAGE_SIZE":
      return { ...state, pagesize: action.payload };
    default:
      return state;
  }
};
