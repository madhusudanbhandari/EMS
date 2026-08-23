import api from "./axios";

export const getPendingUsers=async()=>{
    const response=await api.get("/Admin/pending-users");

    return response.data;
}

export const approveUser=async(userId,role)=>{
    const response=await api.post("/Admin/approve-user",{
        userId,
        role,
    });
    return response.data;
};

export const rejectUser=async(userId)=>{
    const response=await api.post("/Admin/reject-user",{
        userId,
    });
    return response.data;
}