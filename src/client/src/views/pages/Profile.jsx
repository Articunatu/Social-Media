// import ProfileBase from '../../views/components/ProfileBase'
// import React, { useState, useEffect } from 'react';
// import AccountReceiver from '../../controllers/service/AccountService';

// export default function Profile() {

//     const [profile, setProfile] = useState([]);

//     useEffect(() => {
//       async function fetchData() {
//         const data = await AccountReceiver();
//         setProfile(data);
//       }
//       fetchData();
//     }, []);
//     useEffect(() => {fetchData()} , []) 
    
//     return(
//         <div className='profile-container'>
//             <div className='profile-header'>
            
//             <div class="bg-gray-200 h-5">
//                 <div class="h-32 w-32 bg-gradient-to-r from-yellow-400 to-blue-100">
//                     <p>TESTER</p>
//                 </div>

//              </div>
//                 <ProfileBase/>
//                 {/* <Followers/>
//                 <FollowingList/> */}
//             </div>
//             {/* Map all posts of profile */}
//         </div> 
//     )
// }