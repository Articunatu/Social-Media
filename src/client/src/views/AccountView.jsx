// import { Table } from "react-bootstrap";
// import React, { useState, useEffect } from 'react';
// import { AccountReceiver } from "../controllers/service/AccountReciever";

// export default function AccountView()
// {
//    const [accounts, setAccounts] = useState([]);

//    useEffect(() => {
//       async function fetchData() {
//         const data = await AccountReceiver();
//         setAccounts(data);
//       }
//       fetchData();
//     }, []);

//    //useEffect(() => {fetchData()} , []) 
  
//    //const accounts = ReadAllAccounts.AccountController();

//    return (
//       <Table striped bordered hover>
//          <thead>
//             <tr>
//                <th>ID</th>
//                <th>First Name</th>
//                <th>Last Name</th>
//                <th>Profile Pic</th>
//                <th>Background Img</th>
//                <th>Verified?</th>
//             </tr>
//          </thead>
//          <tbody>
//             {accounts.map((account) => (
//             <tr key={account.id}>
//                <th scope="row">{account.id}</th>
//                <td>{account.firstName}</td>
//                <td>{account.lastName}</td>
//                <td>{account.profilePicture}</td>
//                <td>
//                   <img src="{account.profilePicture}" alt="PROFILBILD SAKNAS" /> 
//                </td>
//                <td>
//                   <img src="{account.backgroundImage}" alt="BAKGRUNDSBILD SAKNAS" /> 
//                </td>
//                <td>{account.isVerified}</td>
//             </tr>
//             ))}
//          </tbody>
//       </Table>
//    );
// };