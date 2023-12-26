import Navbar from "./views/components/menu/NavMenu";
import { Outlet } from "react-router";
import { AuthProvider } from "../src/controllers/AuthenticationController";

function App() {
    return (
        <AuthProvider>
            <Navbar />
            <Outlet />
        </AuthProvider>
    );
}

export default App;
