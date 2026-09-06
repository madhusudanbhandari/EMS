import api from "./axios";

export const getDepartments = async () => {
    const response = await api.get("/Departments");
    return response.data;
};

export const getDepartmentById = async (id) => {
    const response = await api.get(`/Departments/${id}`);
    return response.data;
};

export const createDepartment = async (departmentData) => {
    const response = await api.post("/Departments", departmentData);
    return response.data;
};

export const updateDepartment = async (id, departmentData) => {
    const response = await api.put(
        `/Departments/${id}`,
        departmentData
    );

    return response.data;
};

export const deleteDepartment = async (id) => {
    const response = await api.delete(`/Departments/${id}`);
    return response.data;
};