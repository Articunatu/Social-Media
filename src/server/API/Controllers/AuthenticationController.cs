//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Cryptography;

//namespace API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AuthenticationController : ControllerBase
//    {
//        public static UserLogin login = new();

//        [HttpPost("signup")]
//        public async Task<IActionResult> SignUp(UserLoginVM request)
//        {
//            CreatePassword(request., out byte[] passwordHash, out byte[] passwordSalt);

//            login.Account.Tag = request.Tag;
//            login.PasswordHash = passwordHash;
//            login.PasswordSalt = passwordSalt;

//            return Ok(login);
//        }

//        private void CreatePassword(string password, out byte[] passwordHash, out byte[] passwordSalt)
//        {
//            using (var secutiry = new HMACSHA512())
//            {
//                passwordSalt = secutiry.Key;
//                passwordHash = secutiry.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
//            }
//        }
//    }
//}
