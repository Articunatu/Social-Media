import { BrowserRouter, Routes, Route } from "react-router-dom";''
import Feed from "../../pages/Feed";
import Profile from "../../pages/ProfileToBeDeleted";
import Explore from "../../pages/Explore";
import Settings from "../../pages/Settings"

export default function AppRoutes()
{
    <>
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Layout />}>
                <Route index element={<Feed />} />
                <Route path="profile" element={<Profile />} />
                <Route path="explore" element={<Exmplore />} />
                <Route path="settings" element={<Settings />} />
                </Route>
            </Routes>
        </BrowserRouter>
        <Switch location={location}>
            <Route exact path="/feed" component={Feed} />
            <Route path="/profile/:id" component={Profile} />
        </Switch>
    </>
}