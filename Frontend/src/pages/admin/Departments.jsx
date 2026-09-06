import { useEffect, useState } from "react";
import {
    getDepartments,
    createDepartment,
    updateDepartment,
    deleteDepartment
} from "../../api/departmentApi";

function Departments() {
    const [departments, setDepartments] = useState([]);

    const [name, setName] = useState("");

    const [editingId, setEditingId] = useState(null);

    const [loading, setLoading] = useState(true);
    const [saving, setSaving] = useState(false);

    const [error, setError] = useState("");
    const [success, setSuccess] = useState("");

    // --------------------------------
    // Load departments
    // --------------------------------

    const loadDepartments = async () => {
        try {
            setLoading(true);
            setError("");

            const data = await getDepartments();

            console.log("Departments API response:", data);

            setDepartments(data);
        } catch (err) {
            console.error("Failed to load departments:", err);

            setError(
                err.response?.data?.message ||
                err.response?.data ||
                "Failed to load departments"
            );
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        loadDepartments();
    }, []);

    // --------------------------------
    // Submit create/update
    // --------------------------------

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!name.trim()) {
            setError("Department name is required.");
            return;
        }

        try {
            setSaving(true);
            setError("");
            setSuccess("");

            const departmentData = {
                name: name.trim()
            };

            if (editingId) {
                await updateDepartment(editingId, departmentData);

                setSuccess("Department updated successfully.");
            } else {
                await createDepartment(departmentData);

                setSuccess("Department created successfully.");
            }

            setName("");
            setEditingId(null);

            await loadDepartments();

        } catch (err) {
            console.error("Failed to save department:", err);

            setError(
                err.response?.data?.message ||
                err.response?.data ||
                "Failed to save department"
            );
        } finally {
            setSaving(false);
        }
    };

    // --------------------------------
    // Edit
    // --------------------------------

    const handleEdit = (department) => {
        setEditingId(department.id);
        setName(department.name);

        setError("");
        setSuccess("");
    };

    // --------------------------------
    // Cancel edit
    // --------------------------------

    const handleCancelEdit = () => {
        setEditingId(null);
        setName("");

        setError("");
        setSuccess("");
    };

    // --------------------------------
    // Delete
    // --------------------------------

    const handleDelete = async (id) => {
        const confirmed = window.confirm(
            "Are you sure you want to delete this department?"
        );

        if (!confirmed) {
            return;
        }

        try {
            setError("");
            setSuccess("");

            await deleteDepartment(id);

            setSuccess("Department deleted successfully.");

            await loadDepartments();

        } catch (err) {
            console.error("Failed to delete department:", err);

            setError(
                err.response?.data?.message ||
                err.response?.data ||
                "Failed to delete department"
            );
        }
    };

    // --------------------------------
    // Loading
    // --------------------------------

    if (loading) {
        return (
            <div className="p-6">
                <p>Loading departments...</p>
            </div>
        );
    }

    return (
        <div className="p-6">

            {/* Header */}

            <div className="mb-6">
                <h1 className="text-2xl font-bold">
                    Departments
                </h1>

                <p className="text-gray-500 mt-1">
                    Manage employee departments
                </p>
            </div>

            {/* Messages */}

            {error && (
                <div className="mb-4 p-3 bg-red-100 text-red-700 rounded-lg">
                    {error}
                </div>
            )}

            {success && (
                <div className="mb-4 p-3 bg-green-100 text-green-700 rounded-lg">
                    {success}
                </div>
            )}

            {/* Add / Edit form */}

            <div className="bg-white rounded-xl shadow p-6 mb-6">

                <h2 className="text-lg font-semibold mb-4">
                    {editingId
                        ? "Edit Department"
                        : "Add Department"}
                </h2>

                <form
                    onSubmit={handleSubmit}
                    className="flex gap-3"
                >

                    <input
                        type="text"
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                        placeholder="Enter department name"
                        className="flex-1 border border-gray-300 rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
                    />

                    <button
                        type="submit"
                        disabled={saving}
                        className="px-5 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 disabled:opacity-50"
                    >
                        {saving
                            ? "Saving..."
                            : editingId
                                ? "Update"
                                : "Add"}
                    </button>

                    {editingId && (
                        <button
                            type="button"
                            onClick={handleCancelEdit}
                            className="px-5 py-2 bg-gray-500 text-white rounded-lg hover:bg-gray-600"
                        >
                            Cancel
                        </button>
                    )}

                </form>
            </div>

            {/* Department table */}

            <div className="bg-white rounded-xl shadow overflow-hidden">

                <table className="w-full">

                    <thead className="bg-gray-100">

                        <tr>

                            <th className="px-6 py-3 text-left">
                                ID
                            </th>

                            <th className="px-6 py-3 text-left">
                                Department Name
                            </th>

                            <th className="px-6 py-3 text-left">
                                Actions
                            </th>

                        </tr>

                    </thead>

                    <tbody>

                        {departments.length === 0 ? (

                            <tr>

                                <td
                                    colSpan="3"
                                    className="px-6 py-8 text-center text-gray-500"
                                >
                                    No departments found.
                                </td>

                            </tr>

                        ) : (

                            departments.map((department) => (

                                <tr
                                    key={department.id}
                                    className="border-t"
                                >

                                    <td className="px-6 py-4">
                                        {department.id}
                                    </td>

                                    <td className="px-6 py-4 font-medium">
                                        {department.name}
                                    </td>

                                    <td className="px-6 py-4">

                                        <button
                                            onClick={() =>
                                                handleEdit(department)
                                            }
                                            className="text-blue-600 mr-4 hover:underline"
                                        >
                                            Edit
                                        </button>

                                        <button
                                            onClick={() =>
                                                handleDelete(department.id)
                                            }
                                            className="text-red-600 hover:underline"
                                        >
                                            Delete
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

export default Departments;