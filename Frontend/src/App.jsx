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
import { ROLES } from "./constants/roles";
import MainLayout from "./components/layout/MainLayout";

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
                        <ProtectedRoute role={ROLES.ADMIN}>
                            <MainLayout>
                                <AdminDashboard />
                            </MainLayout>
                        </ProtectedRoute>
                    }
                />

                <Route
                path="/employee"
                element={
                    <ProtectedRoute role={ROLES.EMPLOYEE}>
                        <MainLayout>
                            <EmployeeDashboard/>
                        </MainLayout>
                        
                    </ProtectedRoute>
                }
                />

            </Routes>

        </BrowserRouter>
    );
}

export default App;