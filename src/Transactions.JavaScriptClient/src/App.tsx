import './App.css';
import {
  BrowserRouter,
  Routes,
  Route
} from "react-router-dom";


// Components
import Layout from "./components/Layout";
import RequireAuth from "./components/RequireAuth";

// Routes
import Home from "./routes/home";
import SignOut from "./routes/signout";
import Authorize from "./routes/authorize";
import Accounts from "./routes/accounts";
import LinkAccount from "./routes/linkaccount";


export interface IAppProps {};

export const App: React.FunctionComponent<IAppProps> = (props) => {

  return (
    <BrowserRouter>
      <Routes>
          <Route path="/" element={<Layout />}>
            <Route path="/" element={<Home />} />
            <Route path="/authorize" element={<Authorize />} />
            <Route path="/sign-out" element={
            <RequireAuth>
              <SignOut />
            </RequireAuth>
            } />
            <Route path="/accounts" element={
            <RequireAuth>
              <Accounts />
            </RequireAuth>
            } />
            <Route path="/link-account" element={
            <RequireAuth>
              <LinkAccount />
            </RequireAuth>
            } />
          </Route>
      </Routes>
    </BrowserRouter>
  );
};
export default App;
