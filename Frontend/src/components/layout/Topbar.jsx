import { useNavigate } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";

function Topbar() {

    const { user, logout } = useAuth();
    const navigate = useNavigate();

    const handleLogout = () => {

        logout();

        navigate("/login");
    };

    return (
        <header className="h-16 bg-white border-b flex items-center justify-between px-6">

            <div>
                <h2 className="text-lg font-semibold text-gray-800">
                    Welcome back, {user?.name}
                </h2>
            </div>

            <div className="flex items-center gap-4">

                <div className="text-right">

                    <p className="text-sm font-medium text-gray-800">
                        {user?.name}
                    </p>

                    <p className="text-xs text-gray-500">
                        {user?.email}
                    </p>

                </div>

                <button
                    onClick={handleLogout}
                    className="px-4 py-2 bg-red-500 text-white rounded-lg hover:bg-red-600 transition"
                >
                    Logout
                </button>

            </div>

        </header>
    );
}

export default Topbar;