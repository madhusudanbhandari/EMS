import api from "./axios";

export const getMyProfile=async()=>{
    const response=await api.get("/Employees/my-profile");
    return response.data;
}

export const completeMyProfile=async(profileData)=>{
    const response=await api.post(
        "Employees/complete-profile",
        profileData
    );
    return response.data;
};

export const updateMyProfile=async(profilData)=>{
    const response=await api.patch(
        "Employees/update-profile",
        profilData
    );

    return response.data;
}