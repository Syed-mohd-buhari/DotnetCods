export interface GetUsersLoggingLevels {
  deleted?: boolean;
  orphan?: boolean;
  lastModified?: string;
  lastModifiedBy?: string;
  allLoggingLevels?: string;
  currentLogLevel?: string;
}

export interface UserLoggingLevel {
  deleted?: boolean;
  orphan?: boolean;
  lastModified?: string;
  lastModifiedBy?: string;
  aspnetUsers?: Array<User>;
  selectedUser?: number;
  loggingLevel?: string;
}

export interface User {
  id?: number;
  username?: string;
  email?: string;
}

export interface UserLogLevelsBody {
  deleted?: boolean;
  orphan?: boolean;
  lastModified?: string;
  lastModifiedBy?: string;
  allLoggingLevels?: string;
  currentLogLevel?: string;
}
