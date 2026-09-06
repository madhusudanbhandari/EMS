import api from "./axios";

export const getMyProfile = async () => {
    const response = await api.get("/Employees/my-profile");
    return response.data;
};

export const completeMyProfile = async (profileData) => {
    const formData = new FormData();

    formData.append("FirstName", profileData.firstName);
    formData.append("LastName", profileData.lastName);
    formData.append("Phone", profileData.phone);
    formData.append("Address", profileData.address);
    formData.append("Email", profileData.email);
    formData.append("Salary", profileData.salary);
    formData.append("DepartmentId", profileData.departmentId);

    if (profileData.profilePicture) {
        formData.append(
            "ProfilePicture",
            profileData.profilePicture
        );
    }

    const response = await api.post(
        "/Employees/complete-profile",
        formData
    );

    return response.data;
};

export const updateMyProfile = async (profileData) => {
    const formData = new FormData();

    formData.append("FirstName", profileData.firstName);
    formData.append("LastName", profileData.lastName);
    formData.append("Email", profileData.email);
    formData.append("Salary", profileData.salary);
    formData.append("DepartmentId", profileData.departmentId);

    if (profileData.profilePicture) {
        formData.append(
            "ProfilePicture",
            profileData.profilePicture
        );
    }

    const response = await api.patch(
        "/Employees/update-profile",
        formData
    );

    return response.data;
};

export const getEmployees = async (params = {}) => {
    const response = await api.get("/Employees", {
        params
    });

    return response.data;
};

export const applyLeave=async (leaveData)=>{
    const response=await api.post(
        "/Employees/apply-leave",
        leaveData
    );
}

export const getMyLeaves=async()=>{
    const response=await api.get(
        "/Employees/my-leaves"
    );
    return response.data;
}

export const getMyPayroll=async()=>{
    const response=await api.get("/Employees/my-payroll");
    return response.data;
}

