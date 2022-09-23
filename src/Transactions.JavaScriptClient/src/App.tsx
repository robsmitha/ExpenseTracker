import './App.css';
import {
  BrowserRouter,
  Routes,
  Route
} from "react-router-dom";


// Components
import Layout from "./layout/Layout";
import DefaultLayout from "./layout/DefaultLayout";
import Authenticated from "./middleware/AuthenticatedRoute";

// Routes
import Home from "./routes/home";
import SignOut from "./routes/signout";
import Authorize from "./routes/authorize";
import Accounts from "./routes/accounts";
import Account from "./routes/account";
import Budgets from "./routes/budgets";
import Budget from "./routes/budget";


export interface IAppProps {};

export const App: React.FunctionComponent<IAppProps> = (props) => {

  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route path="/" element={<Home />} />
          <Route path="/authorize" element={<Authorize />} />
        </Route>
        <Route path="/" element={<DefaultLayout />}>
          <Route path="/budgets" element={
          <Authenticated>
            <Budgets />
          </Authenticated>
          } />
          <Route path="/sign-out" element={
          <Authenticated>
            <SignOut />
          </Authenticated>
          } />
          <Route path="/accounts" element={
          <Authenticated>
            <Accounts />
          </Authenticated>
          } />
          <Route path="/budget/:budgetId" element={
          <Authenticated>
            <Budget />
          </Authenticated>
          } />
          <Route path="/account/:accountId" element={
          <Authenticated>
            <Account />
          </Authenticated>
          } />
        </Route>
      </Routes>
    </BrowserRouter>
  );
};
export default App;
