import {
    BrowserRouter,
    Routes,
    Route
} from "react-router-dom";

import Register from "./pages/auth/Register";
import Login from "./pages/auth/login";
import Unauthorized from "./pages/auth/Unauthorized";

import AdminDashboard from "./pages/admin/AdminDashboard";

import ProtectedRoute from "./components/ProtectedRoute";
import EmployeeDashboard from "./pages/employee/EmployeeDashboard";

function App() {

    return (
        <BrowserRouter>

            <Routes>

                <Route
                    path="/register"
                    element={<Register />}
                />

                <Route
                    path="/login"
                    element={<Login />}
                />

                {/* <Route
                    path="/pending-approval"
                    element={<PendingApproval />}
                /> */}

                <Route
                    path="/unauthorized"
                    element={<Unauthorized />}
                />

                <Route
                    path="/admin"
                    element={
                        <ProtectedRoute role={3}>
                            <AdminDashboard />
                        </ProtectedRoute>
                    }
                />

                <Route
                    path="/employee"
                    element={<EmployeeDashboard />}
                />

            </Routes>

        </BrowserRouter>
    );
}

export default App;