import { useEffect, useState } from "react";
import { getEmployees } from "../../api/employeeApi";

function Employees() {

    const [employees, setEmployees] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        const loadEmployees = async () => {

            try {
                setLoading(true);
                setError("");

                const data = await getEmployees();

                console.log("Employees API response:", data);

                // Temporary handling
                if (Array.isArray(data)) {
                    setEmployees(data);
                } else if (data.items) {
                    setEmployees(data.items);
                } else {
                    setEmployees([]);
                }

            } catch (err) {

                console.error("Failed to load employees:", err);

                setError(
                    err.response?.data?.message ||
                    "Failed to load employees"
                );

            } finally {
                setLoading(false);
            }
        };

        loadEmployees();

    }, []);

    if (loading) {
        return (
            <div className="p-6">
                <p>Loading employees...</p>
            </div>
        );
    }

    if (error) {
        return (
            <div className="p-6">
                <p className="text-red-500">
                    {error}
                </p>
            </div>
        );
    }

    return (
        <div className="p-6">

            {/* Header */}
            <div className="flex items-center justify-between mb-6">

                <div>
                    <h1 className="text-2xl font-bold">
                        Employees
                    </h1>

                    <p className="text-gray-500">
                        Manage all employees
                    </p>
                </div>

                <button className="px-4 py-2 bg-blue-600 text-white rounded-lg">
                    Add Employee
                </button>

            </div>

            {/* Employee Table */}
            <div className="bg-white rounded-xl shadow overflow-hidden">

                <table className="w-full">

                    <thead className="bg-gray-100">

                        <tr>

                            <th className="px-6 py-3 text-left">
                                ID
                            </th>

                            <th className="px-6 py-3 text-left">
                                Employee
                            </th>

                            <th className="px-6 py-3 text-left">
                                Email
                            </th>

                            <th className="px-6 py-3 text-left">
                                Department
                            </th>

                            <th className="px-6 py-3 text-left">
                                Salary
                            </th>

                            <th className="px-6 py-3 text-left">
                                Actions
                            </th>

                        </tr>

                    </thead>

                    <tbody>

                        {employees.length === 0 ? (

                            <tr>
                                <td
                                    colSpan="6"
                                    className="px-6 py-8 text-center text-gray-500"
                                >
                                    No employees found.
                                </td>
                            </tr>

                        ) : (

                            employees.map((employee) => (

                                <tr
                                    key={employee.id}
                                    className="border-t"
                                >

                                    <td className="px-6 py-4">
                                        {employee.id}
                                    </td>

                                    <td className="px-6 py-4 font-medium">
                                        {employee.firstName}{" "}
                                        {employee.lastName}
                                    </td>

                                    <td className="px-6 py-4">
                                        {employee.email}
                                    </td>

                                    <td className="px-6 py-4">
                                        {employee.departmentName}
                                    </td>

                                    <td className="px-6 py-4">
                                        Rs. {employee.salary}
                                    </td>

                                    <td className="px-6 py-4">

                                        <button className="text-blue-600 mr-3">
                                            View
                                        </button>

                                        <button className="text-green-600 mr-3">
                                            Edit
                                        </button>

                                        <button className="text-red-600">
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

export default Employees;