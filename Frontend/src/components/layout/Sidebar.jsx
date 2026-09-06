import { NavLink } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";
import { ROLES } from "../../constants/roles";

function Sidebar() {

    const { user } = useAuth();

    const navigation = [];

    if (user?.role === ROLES.EMPLOYEE) {
        navigation.push(
            {
                name: "Dashboard",
                path: "/employee",
            },
          
            {
                name: "Leave",
                path: "/employee/leave",
            },
            {
                name: "Payroll",
                path: "/employee/payroll",
            },
            {
                name: "Chat",
                path: "/employee/chat",
            }
        );
    }

    if (user?.role === ROLES.HR) {
        navigation.push(
            {
                name: "Dashboard",
                path: "/hr",
            },
            {
                name: "Employees",
                path: "/hr/employees",
            },
            {
                name: "Leave Requests",
                path: "/hr/leave",
            },
            {
                name: "Departments",
                path: "/hr/departments",
            },
            {
                name: "Payroll",
                path: "/hr/payroll",
            },
            {
                name: "Chat",
                path: "/hr/chat",
            }
        );
    }

    if (user?.role === ROLES.ADMIN) {
        navigation.push(
            {
                name: "Dashboard",
                path: "/admin",
            },
            {
                name: "Users",
                path: "/admin/users",
            },
            {
                name: "Employees",
                path: "/admin/employees",
            },
            {
                name: "Departments",
                path: "/admin/departments",
            },
         
        );
    }

    return (
        <aside className="w-64 min-h-screen bg-gray-900 text-white">

            <div className="p-6 border-b border-gray-700">
                <h1 className="text-xl font-bold">
                    EMS
                </h1>

                <p className="text-sm text-gray-400 mt-1">
                    Employee Management
                </p>
            </div>

            <nav className="p-4 space-y-2">

                {navigation.map((item) => (

                    <NavLink
                        key={item.path}
                        to={item.path}
                        end={item.path === "/employee" || item.path === "/hr" || item.path === "/admin"}
                        className={({ isActive }) =>
                            `block px-4 py-3 rounded-lg transition ${
                                isActive
                                    ? "bg-blue-600 text-white"
                                    : "text-gray-300 hover:bg-gray-800"
                            }`
                        }
                    >
                        {item.name}
                    </NavLink>

                ))}

            </nav>

        </aside>
    );
}

export default Sidebar;