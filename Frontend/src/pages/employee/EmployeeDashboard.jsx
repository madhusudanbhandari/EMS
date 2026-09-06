
import { useEffect, useState } from "react";
import { useAuth } from "../../context/AuthContext";

import {
    getMyProfile,
    completeMyProfile,
    updateMyProfile,
} from "../../api/employeeApi";

function EmployeeDashboard() {

    const [profile, setProfile] = useState(null);

    const [loading, setLoading] = useState(true);
    const [saving, setSaving] = useState(false);

    const [error, setError] = useState("");
    const [success, setSuccess] = useState("");

    const [editing, setEditing] = useState(false);
    const [profileCompleted, setProfileCompleted] = useState(false);

    const [formData, setFormData] = useState({
        firstName: "",
        lastName: "",
        phone: "",
        address: "",
        email: "",
        salary: "",
        departmentId: "",
        profilePicture: null,
    });

    useEffect(() => {
        loadProfile();
    }, []);

    // ================================
    // LOAD PROFILE
    // ================================

    const loadProfile = async () => {
        try {
            setLoading(true);
            setError("");

            const data = await getMyProfile();

            setProfile(data);
            setProfileCompleted(true);

            setFormData({
                firstName: data.firstName || "",
                lastName: data.lastName || "",
                phone: data.phone || "",
                address: data.address || "",
                email: data.email || "",
                salary: data.salary || "",
                departmentId: data.departmentId || "",
                profilePicture: null,
            });

        } catch (error) {
            console.error("Profile loading error:", error);

            // New employee doesn't have an employee profile yet
            if (error.response?.status === 404) {
                setProfile(null);
                setProfileCompleted(false);
                setError("");
            } else {
                setError(
                    error.response?.data?.message ||
                    error.response?.data ||
                    "Unable to load your profile."
                );
            }
        } finally {
            setLoading(false);
        }
    };

    // ================================
    // HANDLE INPUT
    // ================================

    const handleChange = (e) => {
        const { name, value } = e.target;

        setFormData((previous) => ({
            ...previous,
            [name]: value,
        }));
    };

    // ================================
    // HANDLE FILE
    // ================================

    const handleFileChange = (e) => {
        setFormData((previous) => ({
            ...previous,
            profilePicture: e.target.files[0] || null,
        }));
    };

    // ================================
    // COMPLETE PROFILE
    // ================================

    const handleCompleteProfile = async (e) => {
        e.preventDefault();

        try {
            setSaving(true);
            setError("");
            setSuccess("");

            if (!formData.firstName.trim()) {
                setError("First name is required.");
                return;
            }

            if (!formData.lastName.trim()) {
                setError("Last name is required.");
                return;
            }

            if (!formData.phone.trim()) {
                setError("Phone number is required.");
                return;
            }

            if (!formData.address.trim()) {
                setError("Address is required.");
                return;
            }

            if (!formData.email.trim()) {
                setError("Email is required.");
                return;
            }

            if (!formData.salary || Number(formData.salary) <= 0) {
                setError("Please enter a valid salary.");
                return;
            }

            if (
                !formData.departmentId ||
                Number(formData.departmentId) <= 0
            ) {
                setError("Please enter a valid department ID.");
                return;
            }

            console.log("Completing profile with:", {
                firstName: formData.firstName,
                lastName: formData.lastName,
                phone: formData.phone,
                address: formData.address,
                email: formData.email,
                salary: Number(formData.salary),
                departmentId: Number(formData.departmentId),
                profilePicture: formData.profilePicture,
            });

            await completeMyProfile({
                firstName: formData.firstName,
                lastName: formData.lastName,
                phone: formData.phone,
                address: formData.address,
                email: formData.email,
                salary: Number(formData.salary),
                departmentId: Number(formData.departmentId),
                profilePicture: formData.profilePicture,
            });

            setSuccess("Profile completed successfully.");

            await loadProfile();

        } catch (error) {
            console.error("Complete profile error:", error);

            setError(
                error.response?.data?.message ||
                error.response?.data ||
                "Unable to complete your profile."
            );
        } finally {
            setSaving(false);
        }
    };

    // ================================
    // UPDATE PROFILE
    // ================================

    const handleUpdate = async (e) => {
        e.preventDefault();

        try {
            setSaving(true);
            setError("");
            setSuccess("");

            if (!formData.firstName.trim()) {
                setError("First name is required.");
                return;
            }

            if (!formData.lastName.trim()) {
                setError("Last name is required.");
                return;
            }

            if (!formData.email.trim()) {
                setError("Email is required.");
                return;
            }

            if (!formData.salary || Number(formData.salary) <= 0) {
                setError("Please enter a valid salary.");
                return;
            }

            if (
                !formData.departmentId ||
                Number(formData.departmentId) <= 0
            ) {
                setError("Please enter a valid department ID.");
                return;
            }

            console.log("Updating profile:", {
                firstName: formData.firstName,
                lastName: formData.lastName,
                email: formData.email,
                salary: Number(formData.salary),
                departmentId: Number(formData.departmentId),
                profilePicture: formData.profilePicture,
            });

            await updateMyProfile({
                firstName: formData.firstName,
                lastName: formData.lastName,
                email: formData.email,
                salary: Number(formData.salary),
                departmentId: Number(formData.departmentId),
                profilePicture: formData.profilePicture,
            });

            setSuccess("Profile updated successfully.");

            setEditing(false);

            await loadProfile();

        } catch (error) {
            console.error("Update profile error:", error);

            setError(
                error.response?.data?.message ||
                error.response?.data ||
                "Unable to update profile."
            );
        } finally {
            setSaving(false);
        }
    };

    // ================================
    // CANCEL EDIT
    // ================================

    const handleCancelEdit = () => {
        setFormData({
            firstName: profile?.firstName || "",
            lastName: profile?.lastName || "",
            phone: profile?.phone || "",
            address: profile?.address || "",
            email: profile?.email || "",
            salary: profile?.salary || "",
            departmentId: profile?.departmentId || "",
            profilePicture: null,
        });

        setEditing(false);
        setError("");
    };

    // ================================
    // LOADING
    // ================================

    if (loading) {
        return (
            <div className="min-h-screen flex items-center justify-center bg-slate-100">
                <p className="text-slate-500">
                    Loading your profile...
                </p>
            </div>
        );
    }

    // ================================
    // COMPLETE PROFILE SCREEN
    // ================================

    if (!profileCompleted) {
        return (
            <div className="min-h-screen bg-slate-100">



                <main className="max-w-3xl mx-auto px-6 py-10">

                    <div className="bg-white rounded-2xl border border-slate-200 shadow-sm">

                        <div className="p-6 border-b border-slate-200">

                            <h2 className="text-2xl font-bold text-slate-900">
                                Complete Your Profile
                            </h2>

                            <p className="mt-1 text-sm text-slate-500">
                                Please provide your employee information to continue.
                            </p>

                        </div>

                        {error && (
                            <div className="mx-6 mt-6 rounded-lg bg-red-50 border border-red-200 px-4 py-3 text-sm text-red-700">
                                {error}
                            </div>
                        )}

                        <form
                            onSubmit={handleCompleteProfile}
                            className="p-6 space-y-6"
                        >

                            <div className="grid grid-cols-1 sm:grid-cols-2 gap-5">

                                <Input
                                    label="First Name"
                                    name="firstName"
                                    value={formData.firstName}
                                    onChange={handleChange}
                                    required
                                />

                                <Input
                                    label="Last Name"
                                    name="lastName"
                                    value={formData.lastName}
                                    onChange={handleChange}
                                    required
                                />

                            </div>

                            <div className="grid grid-cols-1 sm:grid-cols-2 gap-5">

                                <Input
                                    label="Phone"
                                    name="phone"
                                    value={formData.phone}
                                    onChange={handleChange}
                                    required
                                />

                                <Input
                                    label="Email"
                                    name="email"
                                    type="email"
                                    value={formData.email}
                                    onChange={handleChange}
                                    required
                                />

                            </div>

                            <div>

                                <label className="block text-sm font-medium text-slate-700 mb-1.5">
                                    Address
                                </label>

                                <textarea
                                    name="address"
                                    value={formData.address}
                                    onChange={handleChange}
                                    rows="3"
                                    required
                                    className="w-full rounded-lg border border-slate-300 px-3.5 py-2.5 outline-none focus:border-indigo-500 focus:ring-2 focus:ring-indigo-500/20"
                                />

                            </div>

                            <div className="grid grid-cols-1 sm:grid-cols-2 gap-5">

                                <Input
                                    label="Salary"
                                    name="salary"
                                    type="number"
                                    value={formData.salary}
                                    onChange={handleChange}
                                    required
                                />

                                <Input
                                    label="Department ID"
                                    name="departmentId"
                                    type="number"
                                    value={formData.departmentId}
                                    onChange={handleChange}
                                    required
                                />

                            </div>

                            <div>

                                <label className="block text-sm font-medium text-slate-700 mb-1.5">
                                    Profile Picture
                                </label>

                                <input
                                    type="file"
                                    accept="image/*"
                                    onChange={handleFileChange}
                                    className="w-full text-sm text-slate-600"
                                />

                                <p className="mt-1 text-xs text-slate-400">
                                    Optional. Select an image for your profile.
                                </p>

                            </div>

                            <div className="pt-2">

                                <button
                                    type="submit"
                                    disabled={saving}
                                    className="w-full px-5 py-3 bg-indigo-600 text-white rounded-lg text-sm font-semibold hover:bg-indigo-500 disabled:opacity-50"
                                >
                                    {saving
                                        ? "Completing Profile..."
                                        : "Complete Profile"}
                                </button>

                            </div>

                        </form>

                    </div>

                </main>

            </div>
        );
    }

    // ================================
    // NORMAL DASHBOARD
    // ================================

    return (
        <div className="min-h-screen bg-slate-100">

            {/* NAVBAR */}

            <main className="max-w-7xl mx-auto px-6 py-8">

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
                                    label="Phone"
                                    value={profile?.phone}
                                />

                                <InfoItem
                                    label="Address"
                                    value={profile?.address}
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
                                        required
                                    />

                                    <Input
                                        label="Last Name"
                                        name="lastName"
                                        value={formData.lastName}
                                        onChange={handleChange}
                                        required
                                    />

                                </div>

                                <Input
                                    label="Email"
                                    name="email"
                                    type="email"
                                    value={formData.email}
                                    onChange={handleChange}
                                    required
                                />

                                <div className="grid grid-cols-1 sm:grid-cols-2 gap-5">

                                    <Input
                                        label="Salary"
                                        name="salary"
                                        type="number"
                                        value={formData.salary}
                                        onChange={handleChange}
                                        required
                                    />

                                    <Input
                                        label="Department ID"
                                        name="departmentId"
                                        type="number"
                                        value={formData.departmentId}
                                        onChange={handleChange}
                                        required
                                    />

                                </div>

                                <div>

                                    <label className="block text-sm font-medium text-slate-700 mb-1.5">
                                        Profile Picture
                                    </label>

                                    <input
                                        type="file"
                                        accept="image/*"
                                        onChange={handleFileChange}
                                        className="w-full text-sm text-slate-600"
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
                                        onClick={handleCancelEdit}
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

// ========================================
// INFO ITEM
// ========================================

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

// ========================================
// INPUT
// ========================================

function Input({
    label,
    name,
    type = "text",
    value,
    onChange,
    required = false,
}) {
    return (
        <div>

            <label className="block text-sm font-medium text-slate-700 mb-1.5">
                {label}
                {required && (
                    <span className="text-red-500 ml-1">*</span>
                )}
            </label>

            <input
                type={type}
                name={name}
                value={value}
                onChange={onChange}
                required={required}
                className="w-full rounded-lg border border-slate-300 px-3.5 py-2.5 outline-none focus:border-indigo-500 focus:ring-2 focus:ring-indigo-500/20"
            />

        </div>
    );
}

export default EmployeeDashboard;
