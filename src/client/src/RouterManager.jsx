import React from 'react';
import NavMenu from './views/components/menu/NavMenu';
import { Routes, Route, BrowserRouter as Router} from 'react-router-dom';
import StartPage from './views/pages/StartPage';
import Feed from './views/pages/Feed';
import ProfileInfo from './views/pages/ProfileInfo';
import Explore from './views/pages/Explore';
import Settings from './views/pages/Settings';
import DirectMessages from './views/pages/DirectMessages';
import SignUp from './views/pages/authentication/SignUp';
import Login from './views/pages/authentication/Login';
import { AuthProvider } from "../src/controllers/AuthenticationController";

export default function RouterManager() {
  return (
    <Router>
      <AuthProvider>
        <div>
          <NavMenu />
          <Routes>
            <Route path="/" element={<StartPage />} />
            <Route path="/signup" element={<SignUp />} />
            <Route path="/login" element={<Login />} />
            <Route path="/feed" element={<Feed />} />
            <Route path="/profile/id" element={<ProfileInfo />} />
            <Route path="/explore" element={<Explore />} />
            <Route path="/settings" element={<Settings />} />
            <Route path="/direct_messages" element={<DirectMessages />} />  
          </Routes>
        </div>
      </AuthProvider>
    </Router>
  );
}

// App Extra
// import Nav from "./components/nav";
// import Header from "./components/header";
// import Feed from "./components/feed";
// import Popup from "./components/popup";
// import {useState, useEffect } from "react";
// import WriteIcon from "./components/writeIcon";

// const App = () => {
//   const [ user, setUser] = useState(null)
//   const [ threads, setThreads] = useState([])
//   const [ viewThreadsFeed, setViewThreadsFeed] = useState(null)
//   const [ filteredThreads, setFilteredThreads ] = useState(null)
//   const [ openPopup, setOpenPopup ] = useState(null)
//   const [ interactingThread, setInteractingThread] = useState(null)
//   const [ popupFeedThread, setPopupFeedThread] = useState(null)
//   const [ text, setText] = useState(null)

//   const userId = "eac6778b-ff71-442f-9365-e762c655c66f"

//   const getUser = async () => {
//     try {
//       const response = await fetch(`http://localhost:3000/users?user_uuid=${userId}`)
//       const data = await response.json()
//       setUser(data[0])
//     } 
//     catch (error) {
//       console.log(error);
//     }
//   }

//   const getThreads = async () => {
//     try {
//       const response = await fetch (`http://localhost:3000/threads?thread_from=${userId}`)
//       const data = await response.json()
//       setThreads(data[0])
//     }
//     catch(error) {
//       console.log(error)
//     }
//   }

//   const getThreadsFeed = () => {
//     if (viewThreadsFeed) {
//       const standAloneThreads = threads?.filter((thread) => thread.reply_to === null) || [];
//       setFilteredThreads(standAloneThreads);
//     }
//     if (!viewThreadsFeed) {
//       const replyThreads = threads?.filter((thread) => thread.reply_to !== null) || [];
//       setFilteredThreads(replyThreads);
//     }
//   };
  

//   const getReplies = async () => {
//     try{
//       const response = await fetch(`http://localhost:3000/threads?reply_to=${interactingThread.id}`)
//       const data = await response.json()
//       setPopupFeedThread(data)
//     }
//     catch (error) {
//       console.log(error)
//     }
//   }

//   const postThread = async () => {
//     const thread = {
//       "timestamp": new Date(),
//       "thread_from": user.user_uuid,
//       "thread_to": user.user_uuid || null,
//       "reply_to": interactingThread?.id || null,
//       "text": text,
//       "likes": []
//     }

//     try {
//       const response = await fetch('http://localhost:3000/threads', {
//         method: "POST",
//         headers: {
//           "Content-Type": "application/json"
//         },
//         body: JSON.stringify()
//       })
//       const result = await response.json()
//       getThreads()
//       getReplies()
//       setText("")
//     } 
//     catch (error) {
//       console.log(error);
//     }
//   }

//   useEffect(() => {
//     getReplies()
//   }, [interactingThread])

//   useEffect(() => {
//     getUser()
//     getThreads()
//   }, [])

//   useEffect(() => {
//     getThreadsFeed()
//   }, [user, threads, viewThreadsFeed])

//   const handleClick = () => {
//     setPopupFeedThread(null)
//     setInteractingThread(null)
//     setOpenPopup(true)
//   }

//   console.log(user)
  
//   return (
//     <>
//       {user && <div className="app">
//         <Nav url={user.instagram_url}/>
//         <Header
//           user ={user}
//           viewThreadsFeed={viewThreadsFeed}
//           setViewThreadsFeed={setViewThreadsFeed}
//         />
//         <Feed
//           user={user}
//           setOpenPopup={setOpenPopup}
//           filteredThreads={filteredThreads}
//           getThreads={getThreads}
//           setInteractingThread={setInteractingThread}
//         />
//         {openPopup && 
//         <Popup
//           user={user}
//           setOpenPopup={setOpenPopup}
//           popupFeedThread={popupFeedThread}
//           text={text}
//           setText={setText}
//           postThread={postThread}
//         /> }
//         <div onClick={handleClick}>
//           <WriteIcon/>
//         </div>
//       </div>}
//     </>
//   );
// }

// export default App;