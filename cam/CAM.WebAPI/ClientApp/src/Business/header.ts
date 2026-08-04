import { Configuration } from "./Common/configuration";

export const headerObj = {
  "Strict-Transport-Security": "max-age=31536000; includeSubDomains",
  "X-Content-Type-Options": "nosniff",
  "Cache-control": "no-store,max-age=0",
};

export const requestHeader = {
  ...headerObj,
  //Authorization: config && config.apiKey ? "Bearer" + config.apiKey : "",
};
