// import NavMenu
import DirectMessages from "./DirectMessages";
import Feed from "./Feed";
import ProfileInfo from "./ProfileInfo";
import NavMenu from "../components/menu/NavMenu";
import { Route, Routes } from "react-router-dom"

export default function StartPage() {    
    return(
    <>
        <ProfileInfo></ProfileInfo>
        {/* <NavMenu />
        <div className="container">
            <Routes>
                <Route path="/" element={<Feed />} />
                <Route path="/pricing" element={<DirectMessages />} />
                <Route path="/about" element={<ProfileInfo />} />
            </Routes>
        </div> */}
    </>
    );
}