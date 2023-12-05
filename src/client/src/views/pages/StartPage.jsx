import { useNavigate } from "react-router-dom";
import { useContext, useEffect } from "react";
import { AuthProvider } from "../../controllers/AuthenticationController";


export default function StartPage() {
    const navigate = useNavigate();
    const isLoggedIn = useContext(AuthProvider);

    useEffect(() => {
        if (isLoggedIn) {
        navigate("/feed");
        }
    }, [isLoggedIn, navigate]);

    const handleSignupClick = () => {
        navigate("/signup");
    };

    const handleLoginClick = () => {
        navigate("/login");
    };

    return (
        <div>
        <h2>Välkommen till min socialmedia-hemsida!</h2>
        <p>Vill du upptäcka mer får du logga in eller skapa ett konto:</p>
        <button onClick={handleSignupClick}>Registrera konto</button>
        <button onClick={handleLoginClick}>Logga in</button>
        </div>
    );
}
