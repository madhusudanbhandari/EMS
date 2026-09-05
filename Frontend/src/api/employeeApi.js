import { data } from "react-router-dom";
import api from "./axios";

export const getMyProfile=async()=>{
    const response=await api.get("/Employees/my-profile");
    return response.data;
}

export const completeMyProfile=async(profileData)=>{
    const response=await api.post(
        "Employees/complete-profile",
        profileData,{
            headers:{"Content-Type":"application/json"}
        }
    );
    return response.data;
};

export const updateMyProfile=async(profilData)=>{
    const formData=new FormData();

    formData.append("FirstName",profilData.firstName);
    formData.append("LastName",profilData.lastName);
    formData.append("Email",profilData.email);
    formData.append("Salary",profilData.salary);
    formData.append("DepartmentId",profilData.departmentId);

    if(profilData.profilePicture){
        formData.append("ProfilePicture",profilData.profilePicture);
    }



    const response=await api.patch(
        "Employees/update-profile",
        formData
    );

    return response.data;
}