import {
  PublicClientApplication,
  Configuration,
  LogLevel,
  AccountInfo,
  RedirectRequest,
  SilentRequest,
  InteractionRequiredAuthError,
} from "@azure/msal-browser";

const authority = process.env.REACT_APP_TENANT_ID as string;
const clientId = process.env.REACT_APP_CLIENT_ID as string;
const redirectUri = process.env.REACT_APP_REDIRECT_URL as string;

const msalConfig: Configuration = {
  auth: {
    clientId,
    authority,
    knownAuthorities: [],
    redirectUri,
    postLogoutRedirectUri: redirectUri,
  },
  cache: {
    cacheLocation: "sessionStorage",
  },
  system: {
    loggerOptions: {
      logLevel: LogLevel.Error,
      piiLoggingEnabled: false,
      loggerCallback: () => {},
    },
  },
};

export const loginRequest: RedirectRequest = { scopes: ["user.read"] };

const apiScopes = [
  "https://userscam.onmicrosoft.com/api/read",
  "https://userscam.onmicrosoft.com/api/write",
];

export const msalInstance = new PublicClientApplication(msalConfig);
export const msalReady = msalInstance.initialize();

function getActiveAccount(): AccountInfo | null {
  return (
    msalInstance.getActiveAccount() ?? msalInstance.getAllAccounts()[0] ?? null
  );
}

async function acquireToken(scopes: string[]) {
  const account = getActiveAccount();
  if (!account) {
    await msalInstance.loginRedirect({ ...loginRequest, scopes });
    throw new Error("No active account; redirecting to login.");
  }
  const request: SilentRequest = { scopes, account };
  try {
    return await msalInstance.acquireTokenSilent(request);
  } catch (e) {
    if (e instanceof InteractionRequiredAuthError) {
      await msalInstance.acquireTokenRedirect(request);
    }
    throw e;
  }
}

export const authProvider = {
  getInstance: () => msalInstance,
  login: () => msalInstance.loginRedirect(loginRequest),
  logout: () => msalInstance.logoutRedirect(),
  acquireTokenSilent: (request: { scopes: string[] }) =>
    acquireToken(request.scopes),
  getAccessToken: async () => {
    const r = await acquireToken(apiScopes);
    return { accessToken: r.accessToken };
  },
  getIdToken: async () => {
    const r = await acquireToken(loginRequest.scopes);
    return { idToken: { rawIdToken: r.idToken } };
  },
  getAccount: () => getActiveAccount(),
};
