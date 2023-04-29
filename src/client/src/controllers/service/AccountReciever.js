// api.js
const api = URL + '/Account/read-all'

export async function AccountReceiver() {
    const res = await fetch(api);
    return res.json();
  }
  

//import axios from "axios";
// import Url from "../Url";
// import { useAxios } from "use-axios-client";

// export const AccountReciever = {
//   async ReadAll() {
//     const { data, error, loading } = useAxios({
//       url: baseURL
//     });

//     if (loading || !data) return "Loading...";
//     if (error) return "Error!";
//     return data;
//   }
// }

// export function AccountController()
// {
//   const api = 'https://localhost:7284/api/Account/read-all'

//   const fetchData = async () => 
//   {
//     try 
//     {
        
//         return json;
//     } 
//     catch (err) 
//     {
//         console.error(err)
//     }
//   }

// }