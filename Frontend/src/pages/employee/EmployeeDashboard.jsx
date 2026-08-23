import { useEffect, useState } from "react";
import {
    getMyProfile,
    updateMyProfile,
} from "../../api/employeeApi";

function EmployeeDashboard() {

    const [profile, setProfile] = useState(null);

    const [loading, setLoading] = useState(true);
    const [saving, setSaving] = useState(false);

    const [error, setError] = useState("");
    const [success, setSuccess] = useState("");

    const [editing, setEditing] = useState(false);

    const [formData, setFormData] = useState({
        firstName: "",
        lastName: "",
        email: "",
        salary: "",
        departmentId: "",
    });

    useEffect(() => {
        loadProfile();
    }, []);

    const loadProfile = async () => {

        try {

            setLoading(true);
            setError("");

            const data = await getMyProfile();

            setProfile(data);

            setFormData({
                firstName: data.firstName || "",
                lastName: data.lastName || "",
                email: data.email || "",
                salary: data.salary || "",
                departmentId: data.departmentId || "",
            });

        } catch (error) {

            console.error(error);

            setError(
                error.response?.data?.message ||
                error.response?.data ||
                "Unable to load your profile."
            );

        } finally {

            setLoading(false);

        }
    };

    const handleChange = (e) => {

        setFormData({
            ...formData,
            [e.target.name]: e.target.value,
        });

    };

    const handleUpdate = async (e) => {

        e.preventDefault();

        try {

            setSaving(true);
            setError("");
            setSuccess("");

            await updateMyProfile({
                firstName: formData.firstName,
                lastName: formData.lastName,
                email: formData.email,
                salary: Number(formData.salary),
                departmentId: Number(formData.departmentId),
            });

            setSuccess("Profile updated successfully.");

            await loadProfile();

            setEditing(false);

        } catch (error) {

            console.error(error);

            setError(
                error.response?.data?.message ||
                error.response?.data ||
                "Unable to update profile."
            );

        } finally {

            setSaving(false);

        }
    };

    const handleLogout = () => {

        localStorage.removeItem("token");

        window.location.href = "/login";
    };

    if (loading) {

        return (
            <div className="min-h-screen flex items-center justify-center bg-slate-50">

                <p className="text-slate-500">
                    Loading your profile...
                </p>

            </div>
        );
    }

    if (error && !profile) {

        return (
            <div className="min-h-screen flex items-center justify-center bg-slate-50 px-4">

                <div className="bg-white border border-red-200 rounded-xl p-6">

                    <p className="text-red-600">
                        {error}
                    </p>

                </div>

            </div>
        );
    }

    return (
        <div className="min-h-screen bg-slate-100">

            {/* NAVBAR */}

            <header className="bg-white border-b border-slate-200">

                <div className="max-w-7xl mx-auto px-6 py-4 flex items-center justify-between">

                    <div>

                        <h1 className="text-xl font-bold text-slate-900">
                            Employee Portal
                        </h1>

                        <p className="text-xs text-slate-500">
                            Employee Management System
                        </p>

                    </div>

                    <button
                        onClick={handleLogout}
                        className="px-4 py-2 rounded-lg border border-red-200 text-red-600 text-sm font-medium hover:bg-red-50"
                    >
                        Logout
                    </button>

                </div>

            </header>


            {/* MAIN */}

            <main className="max-w-7xl mx-auto px-6 py-8">

                {/* WELCOME */}

                <div className="mb-8">

                    <h2 className="text-2xl font-bold text-slate-900">

                        Welcome, {profile?.firstName}

                    </h2>

                    <p className="text-slate-500 mt-1">

                        Manage your employee profile and information.

                    </p>

                </div>


                {/* ALERTS */}

                {success && (

                    <div className="mb-6 rounded-lg bg-green-50 border border-green-200 px-4 py-3 text-sm text-green-700">

                        {success}

                    </div>

                )}

                {error && (

                    <div className="mb-6 rounded-lg bg-red-50 border border-red-200 px-4 py-3 text-sm text-red-700">

                        {error}

                    </div>

                )}


                <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">


                    {/* PROFILE CARD */}

                    <div className="bg-white rounded-2xl border border-slate-200 shadow-sm p-6">

                        <div className="flex flex-col items-center text-center">

                            {profile?.profilePicture ? (

                                <img
                                    src={profile.profilePicture}
                                    alt="Profile"
                                    className="w-28 h-28 rounded-full object-cover border-4 border-slate-100"
                                />

                            ) : (

                                <div className="w-28 h-28 rounded-full bg-indigo-100 flex items-center justify-center">

                                    <span className="text-3xl font-bold text-indigo-600">

                                        {profile?.firstName?.charAt(0)}

                                    </span>

                                </div>

                            )}

                            <h3 className="mt-4 text-xl font-semibold text-slate-900">

                                {profile?.firstName} {profile?.lastName}

                            </h3>

                            <p className="text-sm text-slate-500">

                                {profile?.email}

                            </p>

                            <span className="mt-3 px-3 py-1 rounded-full bg-green-50 text-green-700 border border-green-200 text-xs font-medium">

                                Employee

                            </span>

                        </div>

                    </div>


                    {/* INFORMATION */}

                    <div className="lg:col-span-2 bg-white rounded-2xl border border-slate-200 shadow-sm p-6">

                        <div className="flex items-center justify-between mb-6">

                            <div>

                                <h3 className="text-lg font-semibold text-slate-900">

                                    My Profile

                                </h3>

                                <p className="text-sm text-slate-500">

                                    Your employee information

                                </p>

                            </div>

                            {!editing && (

                                <button
                                    onClick={() => setEditing(true)}
                                    className="px-4 py-2 bg-indigo-600 text-white rounded-lg text-sm font-medium hover:bg-indigo-500"
                                >
                                    Edit Profile
                                </button>

                            )}

                        </div>


                        {!editing ? (

                            <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">

                                <InfoItem
                                    label="First Name"
                                    value={profile?.firstName}
                                />

                                <InfoItem
                                    label="Last Name"
                                    value={profile?.lastName}
                                />

                                <InfoItem
                                    label="Email"
                                    value={profile?.email}
                                />

                                <InfoItem
                                    label="Salary"
                                    value={`$${profile?.salary}`}
                                />

                                <InfoItem
                                    label="Department ID"
                                    value={profile?.departmentId}
                                />

                            </div>

                        ) : (

                            <form
                                onSubmit={handleUpdate}
                                className="space-y-5"
                            >

                                <div className="grid grid-cols-1 sm:grid-cols-2 gap-5">

                                    <Input
                                        label="First Name"
                                        name="firstName"
                                        value={formData.firstName}
                                        onChange={handleChange}
                                    />

                                    <Input
                                        label="Last Name"
                                        name="lastName"
                                        value={formData.lastName}
                                        onChange={handleChange}
                                    />

                                </div>


                                <Input
                                    label="Email"
                                    name="email"
                                    type="email"
                                    value={formData.email}
                                    onChange={handleChange}
                                />


                                <div className="grid grid-cols-1 sm:grid-cols-2 gap-5">

                                    <Input
                                        label="Salary"
                                        name="salary"
                                        type="number"
                                        value={formData.salary}
                                        onChange={handleChange}
                                    />

                                    <Input
                                        label="Department ID"
                                        name="departmentId"
                                        type="number"
                                        value={formData.departmentId}
                                        onChange={handleChange}
                                    />

                                </div>


                                <div className="flex gap-3 pt-2">

                                    <button
                                        type="submit"
                                        disabled={saving}
                                        className="px-5 py-2.5 bg-indigo-600 text-white rounded-lg text-sm font-semibold hover:bg-indigo-500 disabled:opacity-50"
                                    >

                                        {saving
                                            ? "Saving..."
                                            : "Save Changes"}

                                    </button>


                                    <button
                                        type="button"
                                        onClick={() => setEditing(false)}
                                        className="px-5 py-2.5 border border-slate-300 text-slate-700 rounded-lg text-sm font-medium hover:bg-slate-50"
                                    >

                                        Cancel

                                    </button>

                                </div>

                            </form>

                        )}

                    </div>

                </div>

            </main>

        </div>
    );
}


function InfoItem({ label, value }) {

    return (

        <div>

            <p className="text-xs font-medium uppercase tracking-wide text-slate-400">
                {label}
            </p>

            <p className="mt-1 text-sm font-medium text-slate-900">
                {value || "Not provided"}
            </p>

        </div>

    );

}


function Input({
    label,
    name,
    type = "text",
    value,
    onChange,
}) {

    return (

        <div>

            <label className="block text-sm font-medium text-slate-700 mb-1.5">

                {label}

            </label>

            <input
                type={type}
                name={name}
                value={value}
                onChange={onChange}
                className="w-full rounded-lg border border-slate-300 px-3.5 py-2.5 outline-none focus:border-indigo-500 focus:ring-2 focus:ring-indigo-500/20"
            />

        </div>

    );

}


export default EmployeeDashboard;