import React, { useState } from 'react';

export default function Login() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  const handleLogin = async (event) => {
    event.preventDefault();

    const response = await fetch('/api/auth/login', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        email,
        password
      })
    });

    const data = await response.json();

    if (response.ok) {
      localStorage.setItem('accessToken', data.accessToken);
    }
  };

  return (
    <form onSubmit={handleLogin}>
      <label>
        Email:
        <input type="email" value={email} onChange={e => setEmail(e.target.value)} />
      </label>
      <label>
        Password:
        <input type="password" value={password} onChange={e => setPassword(e.target.value)} />
      </label>
      <button type="submit">Login</button>
    </form>
  );
}

      
///ASP.NET Identity
// <div class="row">
// <div class="col-md-4">
//     <section>
//         <form id="account" method="post">
//             <h2>Use a local account to log in.</h2>
//             <hr />
//             <div asp-validation-summary="ModelOnly" class="text-danger"></div>
//             <div class="form-floating">
//                 <input asp-for="Input.Email" class="form-control" autocomplete="username" aria-required="true" />
//                 <label asp-for="Input.Email" class="form-label"></label>
//                 <span asp-validation-for="Input.Email" class="text-danger"></span>
//             </div>
//             <div class="form-floating">
//                 <input asp-for="Input.Password" class="form-control" autocomplete="current-password" aria-required="true" />
//                 <label asp-for="Input.Password" class="form-label"></label>
//                 <span asp-validation-for="Input.Password" class="text-danger"></span>
//             </div>
//             <div>
//                 <div class="checkbox" style="color:white;>
//                     <label asp-for="Input.RememberMe" class="form-label">
//                         <input class="form-check-input" asp-for="Input.RememberMe" />
//                         @Html.DisplayNameFor(m => m.Input.RememberMe) 
//                     </label>
//                 </div>
//             </div>
//             <div>
//                 <button id="login-submit" type="submit" class="btn btn-dark">Log in</button>
//             </div>
//             <div>
//                 <p>
//                     <a style="color:white; id="forgot-password" asp-page="./ForgotPassword">Forgot your password?</a>
//                 </p>
//                 <p>
//                     <a style="color:white; asp-page=" . /Register" asp-route-returnUrl="@Model.ReturnUrl">Register as a new user</a>
//                 </p>
//                 <p>
//                     <a style="color:white; id=" resend-confirmation" asp-page="./ResendEmailConfirmation">Resend email confirmation</a>
//                 </p>
//             </div>
//         </form>
//     </section>
// </div>

// </div>