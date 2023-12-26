import React from 'react';
import App from './App';
import { createBrowserRouter } from "react-router-dom";
import StartPage from './views/pages/StartPage';
import Feed from './views/pages/Feed';
import ProfileInfo from './views/pages/ProfileInfo';
import Explore from './views/pages/Explore';
import Settings from './views/pages/Settings';
import DirectMessages from './views/pages/DirectMessages';
import SignUp from './views/pages/authentication/SignUp';
import Login from './views/pages/authentication/Login';

export const router = createBrowserRouter ([
  {
    path: "/",
    element: <App />,
    children: [
      { path: "", element: <StartPage /> },
      { path: "signup", element: <SignUp /> },
      { path: "login", element: <Login /> },
      { path: "feed", element: <Feed /> },
      { path: "profile/id", element: <ProfileInfo /> },
      { path: "explore", element: <Explore /> },
      { path: "settings", element: <Settings /> },
      { path: "direct_messages", element: <DirectMessages /> },
      // {
      //   path: "company/:ticker",
      //   element: <CompanyPage />,
      //   children: [
      //     { path: "historical-dividend", element: <HistoricalDividend /> },
      //   ],
      // },
    ],
  },
]);

// export default function RouterManager() {
//   return (
//     <Router>
//       <AuthProvider>
//         <div>
//           <NavMenu />
//           <Routes>
//             <Route path="/" element={<StartPage />} />
//             <Route path="" element={<SignUp />} />
//             <Route path="/login" element={<Login />} />
//             <Route path="/feed" element={<Feed />} />
//             <Route path="/profile/id" element={<ProfileInfo />} />
//             <Route path="/explore" element={<Explore />} />
//             <Route path="/settings" element={<Settings />} />
//             <Route path="/direct_messages" element={<DirectMessages />} />  
//           </Routes>
//         </div>
//       </AuthProvider>
//     </Router>
//   );
// }