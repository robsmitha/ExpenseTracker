import { createContext, useContext } from 'react';

export type IUserContext = {
  authenticated: boolean;
  setAuthenticated: (authenticated: boolean, token?: string) => void;
}

export const UserContext = createContext<IUserContext>({
  authenticated: false,
  setAuthenticated: () => {},
});

export const useUserContext = () => useContext(UserContext);