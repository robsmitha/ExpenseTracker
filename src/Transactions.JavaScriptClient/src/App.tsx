import './App.css';
import {
  BrowserRouter,
  Routes,
  Route
} from "react-router-dom";
import Home from "./routes/home";
import SignOut from "./routes/signout";
import Authorize from "./routes/authorize";
import Layout from "./components/Layout";

export interface IAppProps {};

export const App: React.FunctionComponent<IAppProps> = (props) => {

  return (
    <BrowserRouter>
      <Routes>
          <Route path="/" element={<Layout />}>
            <Route path="/" element={<Home />} />
            <Route path="/authorize" element={<Authorize />} />
            <Route path="/sign-out" element={<SignOut />} />
          </Route>
      </Routes>
    </BrowserRouter>
  );
};
export default App;
