import { createContext, useContext, useState } from "react";

const AuthContext = createContext();

export function AuthProvider({ children }) {

    const [user, setUser] = useState(() => {
        

        const token=localStorage.getItem("token");
        const storedUser=localStorage.getItem("user");

        if(!token||!storedUser){
            return null;
        }

        try{
            return Json.parse(storedUser);
        }catch{
            localStorage.removeItem("token");
            localStorage.removeItem("user");
        }

        return null;
    });

    const login = (authResponse) => {

        localStorage.setItem(
            "token",
            authResponse.token
        );

        localStorage.setItem(
            "user",
            JSON.stringify(authResponse)
        );

        setUser(authResponse);
    };

    const logout = () => {

        localStorage.removeItem("token");
        localStorage.removeItem("user");

        setUser(null);
    };

    return (
        <AuthContext.Provider
            value={{
                user,
                login,
                logout,
                isAuthenticated: !!user,
            }}
        >
            {children}
        </AuthContext.Provider>
    );
}

export function useAuth() {
    return useContext(AuthContext);
}