import { useEffect, useState } from "react";
import {
    getPendingUsers,
    approveUser,
    rejectUser
} from "../../api/adminApi";

function Users() {
    const [users, setUsers] = useState([]);

    const [loading, setLoading] = useState(true);
    const [processingId, setProcessingId] = useState(null);

    const [error, setError] = useState("");
    const [success, setSuccess] = useState("");

    // Store selected role for each user
    const [selectedRoles, setSelectedRoles] = useState({});

    // --------------------------------
    // Load pending users
    // --------------------------------

    const loadUsers = async () => {
        try {
            setLoading(true);
            setError("");

            const data = await getPendingUsers();

            console.log("Pending users API response:", data);

            setUsers(data);

            // Give every user Employee role by default
            const defaultRoles = {};

            data.forEach((user) => {
                defaultRoles[user.id] = 1;
            });

            setSelectedRoles(defaultRoles);

        } catch (err) {
            console.error("Failed to load users:", err);

            setError(
                err.response?.data?.message ||
                err.response?.data ||
                "Failed to load pending users"
            );
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        loadUsers();
    }, []);

    // --------------------------------
    // Change role
    // --------------------------------

    const handleRoleChange = (userId, role) => {
        setSelectedRoles((previous) => ({
            ...previous,
            [userId]: Number(role)
        }));
    };

    // --------------------------------
    // Approve user
    // --------------------------------

    const handleApprove = async (userId) => {
        const role = selectedRoles[userId];

        if (!role) {
            setError("Please select a role.");
            return;
        }

        try {
            setProcessingId(userId);
            setError("");
            setSuccess("");

            await approveUser(userId, role);

            setSuccess("User approved successfully.");

            await loadUsers();

        } catch (err) {
            console.error("Failed to approve user:", err);

            setError(
                err.response?.data?.message ||
                err.response?.data ||
                "Failed to approve user"
            );
        } finally {
            setProcessingId(null);
        }
    };

    // --------------------------------
    // Reject user
    // --------------------------------

    const handleReject = async (userId) => {
        const confirmed = window.confirm(
            "Are you sure you want to reject this user?"
        );

        if (!confirmed) {
            return;
        }

        try {
            setProcessingId(userId);
            setError("");
            setSuccess("");

            await rejectUser(userId);

            setSuccess("User rejected successfully.");

            await loadUsers();

        } catch (err) {
            console.error("Failed to reject user:", err);

            setError(
                err.response?.data?.message ||
                err.response?.data ||
                "Failed to reject user"
            );
        } finally {
            setProcessingId(null);
        }
    };

    // --------------------------------
    // Loading
    // --------------------------------

    if (loading) {
        return (
            <div className="p-6">
                <p>Loading pending users...</p>
            </div>
        );
    }

    return (
        <div className="p-6">

            {/* Header */}

            <div className="mb-6">
                <h1 className="text-2xl font-bold">
                    Users
                </h1>

                <p className="text-gray-500 mt-1">
                    Review and manage pending user registrations
                </p>
            </div>

            {/* Error */}

            {error && (
                <div className="mb-4 p-3 bg-red-100 text-red-700 rounded-lg">
                    {error}
                </div>
            )}

            {/* Success */}

            {success && (
                <div className="mb-4 p-3 bg-green-100 text-green-700 rounded-lg">
                    {success}
                </div>
            )}

            {/* Users table */}

            <div className="bg-white rounded-xl shadow overflow-hidden">

                <table className="w-full">

                    <thead className="bg-gray-100">

                        <tr>

                            <th className="px-6 py-3 text-left">
                                ID
                            </th>

                            <th className="px-6 py-3 text-left">
                                Name
                            </th>

                            <th className="px-6 py-3 text-left">
                                Email
                            </th>

                            <th className="px-6 py-3 text-left">
                                Role
                            </th>

                            <th className="px-6 py-3 text-left">
                                Actions
                            </th>

                        </tr>

                    </thead>

                    <tbody>

                        {users.length === 0 ? (

                            <tr>

                                <td
                                    colSpan="5"
                                    className="px-6 py-8 text-center text-gray-500"
                                >
                                    No pending users.
                                </td>

                            </tr>

                        ) : (

                            users.map((user) => (

                                <tr
                                    key={user.id}
                                    className="border-t"
                                >

                                    <td className="px-6 py-4">
                                        {user.id}
                                    </td>

                                    <td className="px-6 py-4 font-medium">
                                        {user.firstName || user.name || "-"}
                                    </td>

                                    <td className="px-6 py-4">
                                        {user.email}
                                    </td>

                                    <td className="px-6 py-4">

                                        <select
                                            value={
                                                selectedRoles[user.id] || 1
                                            }
                                            onChange={(e) =>
                                                handleRoleChange(
                                                    user.id,
                                                    e.target.value
                                                )
                                            }
                                            className="border border-gray-300 rounded-lg px-3 py-2"
                                        >

                                            <option value="1">
                                                Employee
                                            </option>

                                            <option value="2">
                                                HR
                                            </option>

                                            <option value="3">
                                                Admin
                                            </option>

                                            <option value="4">
                                                Manager
                                            </option>

                                        </select>

                                    </td>

                                    <td className="px-6 py-4">

                                        <button
                                            onClick={() =>
                                                handleApprove(user.id)
                                            }
                                            disabled={
                                                processingId === user.id
                                            }
                                            className="text-green-600 mr-4 hover:underline disabled:opacity-50"
                                        >
                                            {processingId === user.id
                                                ? "Processing..."
                                                : "Approve"}
                                        </button>

                                        <button
                                            onClick={() =>
                                                handleReject(user.id)
                                            }
                                            disabled={
                                                processingId === user.id
                                            }
                                            className="text-red-600 hover:underline disabled:opacity-50"
                                        >
                                            Reject
                                        </button>

                                    </td>

                                </tr>

                            ))

                        )}

                    </tbody>

                </table>

            </div>

        </div>
    );
}

export default Users;