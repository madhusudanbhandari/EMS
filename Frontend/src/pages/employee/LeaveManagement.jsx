import { useEffect, useState } from "react";
import {
    applyLeave,
    getMyLeaves,
} from "../../api/employeeApi";

function LeaveManagement() {

    const [formData, setFormData] = useState({
        leaveType: "",
        startDate: "",
        endDate: "",
        reason: "",
    });

    const [leaves, setLeaves] = useState([]);

    const [loading, setLoading] = useState(true);
    const [submitting, setSubmitting] = useState(false);

    const [error, setError] = useState("");
    const [success, setSuccess] = useState("");

    useEffect(() => {
        loadLeaves();
    }, []);

    const loadLeaves = async () => {
        try {
            setLoading(true);
            setError("");

            const data = await getMyLeaves();

            setLeaves(data);

        } catch (error) {
            console.error("Error loading leaves:", error);

            setError(
                error.response?.data?.message ||
                error.response?.data ||
                "Unable to load leave history."
            );
        } finally {
            setLoading(false);
        }
    };

    const handleChange = (e) => {
        const { name, value } = e.target;

        setFormData((previous) => ({
            ...previous,
            [name]: value,
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        setError("");
        setSuccess("");

        if (!formData.leaveType) {
            setError("Please select a leave type.");
            return;
        }

        if (!formData.startDate) {
            setError("Please select a start date.");
            return;
        }

        if (!formData.endDate) {
            setError("Please select an end date.");
            return;
        }

        if (formData.startDate > formData.endDate) {
            setError("Start date cannot be after end date.");
            return;
        }

        if (!formData.reason.trim()) {
            setError("Please provide a reason.");
            return;
        }

        try {
            setSubmitting(true);

            const leaveData = {
                leaveType: formData.leaveType,
                startDate: formData.startDate,
                endDate: formData.endDate,
                reason: formData.reason,
            };

            console.log("Submitting leave:", leaveData);

            await applyLeave(leaveData);

            setSuccess("Leave application submitted successfully.");

            setFormData({
                leaveType: "",
                startDate: "",
                endDate: "",
                reason: "",
            });

            await loadLeaves();

        } catch (error) {
            console.error("Apply leave error:", error);

            setError(
                error.response?.data?.message ||
                error.response?.data ||
                "Unable to apply for leave."
            );
        } finally {
            setSubmitting(false);
        }
    };

    const getStatusClass = (status) => {

        if (status === "Approved") {
            return "bg-green-50 text-green-700 border-green-200";
        }

        if (status === "Rejected") {
            return "bg-red-50 text-red-700 border-red-200";
        }

        return "bg-yellow-50 text-yellow-700 border-yellow-200";
    };

    return (
        <div className="min-h-screen bg-slate-100">

            <main className="max-w-7xl mx-auto px-6 py-8">

                <div className="mb-8">
                    <h1 className="text-2xl font-bold text-slate-900">
                        Leave Management
                    </h1>

                    <p className="mt-1 text-slate-500">
                        Apply for leave and track your leave requests.
                    </p>
                </div>

                {error && (
                    <div className="mb-6 rounded-lg bg-red-50 border border-red-200 px-4 py-3 text-sm text-red-700">
                        {error}
                    </div>
                )}

                {success && (
                    <div className="mb-6 rounded-lg bg-green-50 border border-green-200 px-4 py-3 text-sm text-green-700">
                        {success}
                    </div>
                )}

                {/* APPLY LEAVE */}

                <div className="bg-white rounded-2xl border border-slate-200 shadow-sm p-6 mb-8">

                    <div className="mb-6">
                        <h2 className="text-lg font-semibold text-slate-900">
                            Apply for Leave
                        </h2>

                        <p className="text-sm text-slate-500 mt-1">
                            Submit a new leave request.
                        </p>
                    </div>

                    <form onSubmit={handleSubmit} className="space-y-5">

                        <div>
                            <label className="block text-sm font-medium text-slate-700 mb-1.5">
                                Leave Type
                            </label>

                            <select
                                name="leaveType"
                                value={formData.leaveType}
                                onChange={handleChange}
                                className="w-full rounded-lg border border-slate-300 px-3.5 py-2.5 outline-none focus:border-indigo-500 focus:ring-2 focus:ring-indigo-500/20"
                            >
                                <option value="">
                                    Select leave type
                                </option>

                                <option value="Sick">
                                    Sick Leave
                                </option>

                                <option value="Casual">
                                    Casual Leave
                                </option>

                                <option value="Annual">
                                    Annual Leave
                                </option>

                                <option value="Emergency">
                                    Emergency Leave
                                </option>
                            </select>
                        </div>

                        <div className="grid grid-cols-1 md:grid-cols-2 gap-5">

                            <div>
                                <label className="block text-sm font-medium text-slate-700 mb-1.5">
                                    Start Date
                                </label>

                                <input
                                    type="date"
                                    name="startDate"
                                    value={formData.startDate}
                                    onChange={handleChange}
                                    className="w-full rounded-lg border border-slate-300 px-3.5 py-2.5 outline-none focus:border-indigo-500 focus:ring-2 focus:ring-indigo-500/20"
                                />
                            </div>

                            <div>
                                <label className="block text-sm font-medium text-slate-700 mb-1.5">
                                    End Date
                                </label>

                                <input
                                    type="date"
                                    name="endDate"
                                    value={formData.endDate}
                                    onChange={handleChange}
                                    className="w-full rounded-lg border border-slate-300 px-3.5 py-2.5 outline-none focus:border-indigo-500 focus:ring-2 focus:ring-indigo-500/20"
                                />
                            </div>

                        </div>

                        <div>
                            <label className="block text-sm font-medium text-slate-700 mb-1.5">
                                Reason
                            </label>

                            <textarea
                                name="reason"
                                value={formData.reason}
                                onChange={handleChange}
                                rows="4"
                                placeholder="Enter the reason for your leave..."
                                className="w-full rounded-lg border border-slate-300 px-3.5 py-2.5 outline-none focus:border-indigo-500 focus:ring-2 focus:ring-indigo-500/20"
                            />
                        </div>

                        <button
                            type="submit"
                            disabled={submitting}
                            className="px-5 py-2.5 bg-indigo-600 text-white rounded-lg text-sm font-semibold hover:bg-indigo-500 disabled:opacity-50"
                        >
                            {submitting
                                ? "Submitting..."
                                : "Apply for Leave"}
                        </button>

                    </form>
                </div>

                {/* LEAVE HISTORY */}

                <div className="bg-white rounded-2xl border border-slate-200 shadow-sm p-6">

                    <div className="flex items-center justify-between mb-6">

                        <div>
                            <h2 className="text-lg font-semibold text-slate-900">
                                My Leave History
                            </h2>

                            <p className="text-sm text-slate-500 mt-1">
                                Track the status of your leave requests.
                            </p>
                        </div>

                        <button
                            onClick={loadLeaves}
                            className="px-4 py-2 border border-slate-300 rounded-lg text-sm font-medium text-slate-700 hover:bg-slate-50"
                        >
                            Refresh
                        </button>

                    </div>

                    {loading ? (
                        <p className="text-slate-500">
                            Loading leave history...
                        </p>
                    ) : leaves.length === 0 ? (
                        <div className="py-10 text-center">
                            <p className="text-slate-500">
                                You haven't applied for any leave yet.
                            </p>
                        </div>
                    ) : (
                        <div className="overflow-x-auto">

                            <table className="w-full text-sm">

                                <thead>
                                    <tr className="border-b border-slate-200 text-left">
                                        <th className="py-3 pr-4">
                                            Leave Type
                                        </th>

                                        <th className="py-3 pr-4">
                                            Start
                                        </th>

                                        <th className="py-3 pr-4">
                                            End
                                        </th>

                                        <th className="py-3 pr-4">
                                            Reason
                                        </th>

                                        <th className="py-3 pr-4">
                                            Status
                                        </th>

                                        <th className="py-3">
                                            Applied At
                                        </th>
                                    </tr>
                                </thead>

                                <tbody>

                                    {leaves.map((leave) => (

                                        <tr
                                            key={leave.id}
                                            className="border-b border-slate-100"
                                        >

                                            <td className="py-4 pr-4 font-medium text-slate-900">
                                                {leave.leaveType}
                                            </td>

                                            <td className="py-4 pr-4 text-slate-600">
                                                {leave.startDate}
                                            </td>

                                            <td className="py-4 pr-4 text-slate-600">
                                                {leave.endDate}
                                            </td>

                                            <td className="py-4 pr-4 text-slate-600 max-w-xs">
                                                {leave.reason}
                                            </td>

                                            <td className="py-4 pr-4">

                                                <span
                                                    className={`px-3 py-1 rounded-full border text-xs font-medium ${getStatusClass(
                                                        leave.status
                                                    )}`}
                                                >
                                                    {leave.status}
                                                </span>

                                            </td>

                                            <td className="py-4 text-slate-600">
                                                {new Date(
                                                    leave.appliedAt
                                                ).toLocaleDateString()}
                                            </td>

                                        </tr>

                                    ))}

                                </tbody>

                            </table>

                        </div>
                    )}

                </div>

            </main>

        </div>
    );
}

export default LeaveManagement;