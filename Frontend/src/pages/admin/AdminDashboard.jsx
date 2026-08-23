import { useEffect, useState } from "react";
import {
    getPendingUsers,
    approveUser,
    rejectUser,
} from "../../api/adminApi";

function AdminDashboard() {

    const [users, setUsers] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    const loadPendingUsers = async () => {
        try {
            setLoading(true);

            const data = await getPendingUsers();

            setUsers(data);
        } catch (error) {
            console.error(error);

            setError(
                error.response?.data?.message ||
                "Unable to load pending users."
            );
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        loadPendingUsers();
    }, []);

    const handleApprove = async (userId, role) => {

        if (!role) {
            alert("Please select a role.");
            return;
        }

        try {

            await approveUser(userId, Number(role));

            setUsers((previousUsers) =>
                previousUsers.filter(
                    (user) => user.id !== userId
                )
            );

        } catch (error) {

            console.error(error);

            alert(
                error.response?.data?.message ||
                "Unable to approve user."
            );
        }
    };

    const handleReject = async (userId) => {

        try {

            await rejectUser(userId);

            setUsers((previousUsers) =>
                previousUsers.filter(
                    (user) => user.id !== userId
                )
            );

        } catch (error) {

            console.error(error);

            alert(
                error.response?.data?.message ||
                "Unable to reject user."
            );
        }
    };

    if (loading) {
        return (
            <div className="min-h-screen flex items-center justify-center bg-slate-50">
                <p className="text-sm text-slate-500">Loading pending users...</p>
            </div>
        );
    }

    if (error) {
        return (
            <div className="min-h-screen flex items-center justify-center bg-slate-50 px-4">
                <p className="rounded-lg bg-red-50 border border-red-200 px-4 py-3 text-sm text-red-700">
                    {error}
                </p>
            </div>
        );
    }

    return (
        <div className="min-h-screen bg-slate-50 px-4 py-12">
            <div className="mx-auto max-w-3xl">

                <div className="mb-8">
                    <h1 className="text-2xl font-bold text-slate-900 tracking-tight">
                        Admin Dashboard
                    </h1>
                    <h2 className="mt-1 text-sm text-slate-500">
                        Pending Registrations
                    </h2>
                </div>

                {users.length === 0 ? (
                    <div className="bg-white rounded-2xl shadow-sm border border-slate-100 p-8 text-center">
                        <p className="text-sm text-slate-500">No pending registrations.</p>
                    </div>
                ) : (
                    <div className="space-y-4">
                        {users.map((user) => (
                            <UserApprovalCard
                                key={user.id}
                                user={user}
                                onApprove={handleApprove}
                                onReject={handleReject}
                            />
                        ))}
                    </div>
                )}

            </div>
        </div>
    );
}

function UserApprovalCard({
    user,
    onApprove,
    onReject,
}) {

    const [role, setRole] = useState("");

    return (
        <div className="bg-white rounded-2xl shadow-sm border border-slate-100 p-6 flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">

            <div>
                <h3 className="text-base font-semibold text-slate-900">
                    {user.name}
                </h3>
                <p className="text-sm text-slate-500">{user.email}</p>
                <span className="inline-block mt-2 text-xs font-medium text-amber-700 bg-amber-50 border border-amber-200 rounded-full px-2.5 py-0.5">
                    Pending Approval
                </span>
            </div>

            <div className="flex flex-wrap items-center gap-2">

                <select
                    value={role}
                    onChange={(e) => setRole(e.target.value)}
                    className="rounded-lg border border-slate-300 px-3 py-2 text-sm text-slate-900 shadow-sm outline-none transition focus:border-indigo-500 focus:ring-2 focus:ring-indigo-500/20"
                >
                    <option value="">Select Role</option>
                    <option value="1">Employee</option>
                    <option value="2">HR</option>
                    <option value="4">Manager</option>
                </select>

                <button
                    onClick={() => onApprove(user.id, role)}
                    className="rounded-lg bg-indigo-600 px-4 py-2 text-sm font-semibold text-white shadow-sm transition hover:bg-indigo-500 focus:outline-none focus:ring-2 focus:ring-indigo-500/40"
                >
                    Approve
                </button>

                <button
                    onClick={() => onReject(user.id)}
                    className="rounded-lg bg-white px-4 py-2 text-sm font-semibold text-red-600 border border-red-200 shadow-sm transition hover:bg-red-50 focus:outline-none focus:ring-2 focus:ring-red-500/30"
                >
                    Reject
                </button>

            </div>

        </div>
    );
}

export default AdminDashboard;