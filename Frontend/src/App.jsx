import {
    BrowserRouter,
    Routes,
    Route
} from "react-router-dom";

import Register from "./pages/auth/Register";
import Login from "./pages/auth/login";
import Unauthorized from "./pages/auth/Unauthorized";


import ProtectedRoute from "./components/ProtectedRoute";
import EmployeeDashboard from "./pages/employee/EmployeeDashboard";
import { ROLES } from "./constants/roles";
import MainLayout from "./components/layout/MainLayout";
import Employees from "./pages/admin/Employees";

import Dashboard from "./pages/admin/Dashboard";
import Departments from "./pages/admin/Departments";
import Users from "./pages/admin/Users";
import LeaveManagement from "./pages/employee/LeaveManagement";
import Payroll from "./pages/employee/Payroll";

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
                                <Dashboard />
                            </MainLayout>
                        </ProtectedRoute>
                    }
                />
                 <Route
                    path="/admin/employees"
                    element={
                        <ProtectedRoute role={ROLES.ADMIN}>
                            <MainLayout>
                                <Employees />
                            </MainLayout>
                        </ProtectedRoute>
                    }
                />

                <Route
                    path="/admin/departments"
                    element={
                        <ProtectedRoute role={ROLES.ADMIN}>
                            <MainLayout>
                                <Departments/>
                            </MainLayout>
                        </ProtectedRoute>
                    }
                />

                <Route
                path="/admin/users"
                element={
                    <ProtectedRoute role={ROLES.ADMIN}>
                        <MainLayout>
                            <Users />
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

                <Route
                path="/employee/leave"
                element={
                    <ProtectedRoute role={ROLES.EMPLOYEE}>
                        <LeaveManagement/>
                    </ProtectedRoute>
                }
                />

                <Route
                path="/employee/payroll"
                element={
                    <ProtectedRoute role={ROLES.EMPLOYEE}>
                        <Payroll></Payroll>
                    </ProtectedRoute>
                }
                />

            </Routes>

        </BrowserRouter>
    );
}

export default App;