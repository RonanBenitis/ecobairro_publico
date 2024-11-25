import { createContext, useContext, useState } from "react";


interface UserContextType {
    user: any | null;
    setUser: (user: any | null) => void;
}
const UserContext = createContext<UserContextType | undefined>(undefined);
export function useUser()  {
    const context = useContext(UserContext);
    if (context === undefined) {
        throw new Error("useUser must be used within a UserProvider");
    }
    return context;
}

interface UserProviderProps {
    children: React.ReactNode;
}
export function UserProvider({ children }: UserProviderProps) {
    const [user, setUser] = useState<any | null>(null);
    return (
        <UserContext.Provider value={{ user, setUser }}>
            {children}
        </UserContext.Provider>
    );
}