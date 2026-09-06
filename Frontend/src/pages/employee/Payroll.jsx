import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { getMyPayroll } from "../../api/employeeApi";

const Payroll = () => {
    const navigate = useNavigate();

    const [payrolls, setPayrolls] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        loadPayroll();
    }, []);

    const loadPayroll = async () => {
        try {
            setLoading(true);
            setError("");

            const data = await getMyPayroll();

            setPayrolls(data);
        } catch (error) {
            console.error("Failed to load payroll:", error);

            if (error.response?.status === 401) {
                setError("You are not authorized to view your payroll.");
            } else if (error.response?.status === 404) {
                setError("No payroll records found.");
            } else {
                setError("Failed to load payroll information.");
            }
        } finally {
            setLoading(false);
        }
    };

    const formatCurrency = (amount) => {
        return new Intl.NumberFormat("en-NP", {
            style: "currency",
            currency: "NPR",
            maximumFractionDigits: 2,
        }).format(amount ?? 0);
    };

    const formatDate = (date) => {
        if (!date) {
            return "N/A";
        }

        return new Date(date).toLocaleDateString("en-US", {
            year: "numeric",
            month: "short",
            day: "numeric",
        });
    };

    const getStatusClass = (status) => {
        const normalizedStatus = String(status).toLowerCase();

        if (normalizedStatus === "paid") {
            return "bg-green-100 text-green-700";
        }

        if (normalizedStatus === "processed") {
            return "bg-blue-100 text-blue-700";
        }

        if (normalizedStatus === "pending") {
            return "bg-yellow-100 text-yellow-700";
        }

        return "bg-gray-100 text-gray-700";
    };

    if (loading) {
        return (
            <div className="min-h-screen bg-gray-50 p-8">
                <div className="max-w-7xl mx-auto">
                    <p className="text-gray-600">
                        Loading payroll...
                    </p>
                </div>
            </div>
        );
    }

    return (
        <div className="min-h-screen bg-gray-50 p-8">

            <div className="max-w-7xl mx-auto">

                {/* Header */}

                <div className="flex items-center justify-between mb-8">

                    <div>
                        <h1 className="text-3xl font-bold text-gray-900">
                            My Payroll
                        </h1>

                        <p className="text-gray-600 mt-1">
                            View your salary and payroll history
                        </p>
                    </div>

                    <button
                        onClick={() => navigate("/employee")}
                        className="px-4 py-2 bg-gray-200 text-gray-700 rounded-lg hover:bg-gray-300"
                    >
                        Back to Dashboard
                    </button>

                </div>

                {/* Error */}

                {error && (
                    <div className="mb-6 p-4 bg-red-100 text-red-700 rounded-lg">
                        {error}
                    </div>
                )}

                {/* No payroll */}

                {!error && payrolls.length === 0 && (
                    <div className="bg-white rounded-xl shadow p-8 text-center">
                        <h2 className="text-xl font-semibold text-gray-800">
                            No Payroll Records
                        </h2>

                        <p className="text-gray-500 mt-2">
                            Your payroll information has not been generated yet.
                        </p>
                    </div>
                )}

                {/* Payroll */}

                {payrolls.length > 0 && (
                    <>
                        {/* Latest Payroll */}

                        <div className="mb-8">

                            <h2 className="text-xl font-semibold text-gray-800 mb-4">
                                Latest Payroll
                            </h2>

                            {(() => {
                                const latestPayroll = payrolls[0];

                                return (
                                    <div className="bg-white rounded-xl shadow p-6">

                                        <div className="flex items-center justify-between mb-6">

                                            <div>
                                                <p className="text-sm text-gray-500">
                                                    Payroll Period
                                                </p>

                                                <p className="text-lg font-semibold text-gray-900">
                                                    {latestPayroll.payrollPeriod}
                                                </p>
                                            </div>

                                            <span
                                                className={`px-3 py-1 rounded-full text-sm font-medium ${getStatusClass(
                                                    latestPayroll.status
                                                )}`}
                                            >
                                                {String(latestPayroll.status)}
                                            </span>

                                        </div>

                                        {/* Salary Cards */}

                                        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">

                                            <div className="bg-gray-50 rounded-lg p-4">
                                                <p className="text-sm text-gray-500">
                                                    Base Salary
                                                </p>

                                                <p className="text-xl font-bold text-gray-900 mt-1">
                                                    {formatCurrency(
                                                        latestPayroll.baseSalary
                                                    )}
                                                </p>
                                            </div>

                                            <div className="bg-gray-50 rounded-lg p-4">
                                                <p className="text-sm text-gray-500">
                                                    Overtime
                                                </p>

                                                <p className="text-xl font-bold text-gray-900 mt-1">
                                                    {formatCurrency(
                                                        latestPayroll.overtime
                                                    )}
                                                </p>
                                            </div>

                                            <div className="bg-gray-50 rounded-lg p-4">
                                                <p className="text-sm text-gray-500">
                                                    Bonus
                                                </p>

                                                <p className="text-xl font-bold text-gray-900 mt-1">
                                                    {formatCurrency(
                                                        latestPayroll.bonus
                                                    )}
                                                </p>
                                            </div>

                                            <div className="bg-gray-50 rounded-lg p-4">
                                                <p className="text-sm text-gray-500">
                                                    Gross Salary
                                                </p>

                                                <p className="text-xl font-bold text-gray-900 mt-1">
                                                    {formatCurrency(
                                                        latestPayroll.grossSalary
                                                    )}
                                                </p>
                                            </div>

                                        </div>

                                        {/* Net Salary */}

                                        <div className="mt-6 grid grid-cols-1 md:grid-cols-2 gap-4">

                                            <div className="border rounded-lg p-5">

                                                <p className="text-sm text-gray-500">
                                                    Total Deductions
                                                </p>

                                                <p className="text-2xl font-bold text-red-600 mt-1">
                                                    {formatCurrency(
                                                        latestPayroll.totalDeductions
                                                    )}
                                                </p>

                                            </div>

                                            <div className="border rounded-lg p-5">

                                                <p className="text-sm text-gray-500">
                                                    Net Salary
                                                </p>

                                                <p className="text-2xl font-bold text-green-600 mt-1">
                                                    {formatCurrency(
                                                        latestPayroll.netSalary
                                                    )}
                                                </p>

                                            </div>

                                        </div>

                                        {/* Processed */}

                                        <div className="mt-6 text-sm text-gray-500">

                                            Processed on{" "}
                                            <span className="font-medium text-gray-700">
                                                {formatDate(
                                                    latestPayroll.processedAt
                                                )}
                                            </span>

                                        </div>

                                    </div>
                                );
                            })()}

                        </div>

                        {/* Payroll History */}

                        <div>

                            <h2 className="text-xl font-semibold text-gray-800 mb-4">
                                Payroll History
                            </h2>

                            <div className="bg-white rounded-xl shadow overflow-hidden">

                                <div className="overflow-x-auto">

                                    <table className="w-full">

                                        <thead className="bg-gray-50 border-b">

                                            <tr>

                                                <th className="px-6 py-4 text-left text-sm font-semibold text-gray-600">
                                                    Period
                                                </th>

                                                <th className="px-6 py-4 text-left text-sm font-semibold text-gray-600">
                                                    Base Salary
                                                </th>

                                                <th className="px-6 py-4 text-left text-sm font-semibold text-gray-600">
                                                    Overtime
                                                </th>

                                                <th className="px-6 py-4 text-left text-sm font-semibold text-gray-600">
                                                    Bonus
                                                </th>

                                                <th className="px-6 py-4 text-left text-sm font-semibold text-gray-600">
                                                    Gross
                                                </th>

                                                <th className="px-6 py-4 text-left text-sm font-semibold text-gray-600">
                                                    Deductions
                                                </th>

                                                <th className="px-6 py-4 text-left text-sm font-semibold text-gray-600">
                                                    Net Salary
                                                </th>

                                                <th className="px-6 py-4 text-left text-sm font-semibold text-gray-600">
                                                    Status
                                                </th>

                                            </tr>

                                        </thead>

                                        <tbody className="divide-y">

                                            {payrolls.map((payroll) => (

                                                <tr
                                                    key={payroll.id}
                                                    className="hover:bg-gray-50"
                                                >

                                                    <td className="px-6 py-4 text-sm text-gray-700">
                                                        {payroll.payrollPeriod}
                                                    </td>

                                                    <td className="px-6 py-4 text-sm">
                                                        {formatCurrency(
                                                            payroll.baseSalary
                                                        )}
                                                    </td>

                                                    <td className="px-6 py-4 text-sm">
                                                        {formatCurrency(
                                                            payroll.overtime
                                                        )}
                                                    </td>

                                                    <td className="px-6 py-4 text-sm">
                                                        {formatCurrency(
                                                            payroll.bonus
                                                        )}
                                                    </td>

                                                    <td className="px-6 py-4 text-sm font-medium">
                                                        {formatCurrency(
                                                            payroll.grossSalary
                                                        )}
                                                    </td>

                                                    <td className="px-6 py-4 text-sm text-red-600">
                                                        {formatCurrency(
                                                            payroll.totalDeductions
                                                        )}
                                                    </td>

                                                    <td className="px-6 py-4 text-sm font-bold text-green-600">
                                                        {formatCurrency(
                                                            payroll.netSalary
                                                        )}
                                                    </td>

                                                    <td className="px-6 py-4">

                                                        <span
                                                            className={`px-3 py-1 rounded-full text-xs font-medium ${getStatusClass(
                                                                payroll.status
                                                            )}`}
                                                        >
                                                            {String(
                                                                payroll.status
                                                            )}
                                                        </span>

                                                    </td>

                                                </tr>

                                            ))}

                                        </tbody>

                                    </table>

                                </div>

                            </div>

                        </div>
                    </>
                )}

            </div>

        </div>
    );
};

export default Payroll;