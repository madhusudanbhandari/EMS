import api from "./axios";

export const registerUser=async(data)=>{
    const response=await api.post("/Auth/register",data);
    return response.data;
}

export const loginUser=async(data)=>{
    const response=await api.post("/Auth/login",data);
    return response.data;
}