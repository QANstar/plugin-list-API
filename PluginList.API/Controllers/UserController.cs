using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PluginList.Entity;
using PluginList.Model;
using QANbuy.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PluginList.API.Controllers
{
    [ApiController]
    [Route("api/User/[controller]/[action]")]
    public class UserController : Controller
    {
        PluginList_DBContext Context;
        public UserController(PluginList_DBContext context)
        {
            Context = context;
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        [EnableCors("any")]
        [HttpPost]
        public IActionResult userSginUp(SignUpModel userInfo)
        {
            try
            {
                bool isEmailHave = Context.User.ToList().Exists(x => x.email == userInfo.email);
                bool isNameHave = Context.User.ToList().Exists(x => x.userName == userInfo.userName);
                if (isEmailHave)
                {
                    return BadRequest("邮箱已被注册");
                }
                if (isNameHave)
                {
                    return BadRequest("用户名已被注册");
                }
                User user = new User();
                user.email = userInfo.email;
                user.password = userInfo.password;
                user.userName = userInfo.userName;
                Context.User.Add(user);
                Context.SaveChanges();
                return Ok("注册成功");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [EnableCors("any")]
        [HttpPost]
        public IActionResult Login(LoginModel user)
        {
            Entity.User result = Context.User.FirstOrDefault(x => x.email == user.email && x.password == user.password);
            if (result != null)
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                    new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddMinutes(60*24*7)).ToUnixTimeSeconds()}"),
                    new Claim(ClaimTypes.Sid, result.id.ToString())
                };
                var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("QANstarAndSuoMi1931"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: "QANstar",
                    audience: "QANstar",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(60 * 24 * 7),
                    signingCredentials: creds);

                var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(jwtToken);
            }
            else
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// 显示用户信息
        /// </summary>
        /// <returns></returns>
        [EnableCors("any")]
        [Authorize]
        [HttpGet]
        public IActionResult showUserInfo()
        {
            var auth = HttpContext.AuthenticateAsync();
            int userID = int.Parse(auth.Result.Principal.Claims.First(t => t.Type.Equals(ClaimTypes.Sid))?.Value);
            User user = Context.User.FirstOrDefault(x => x.id == userID);
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// 编辑用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [EnableCors("any")]
        [HttpPost]
        [Authorize]
        public IActionResult editUserInfo(EditUserModel user)
        {
            var auth = HttpContext.AuthenticateAsync();
            int userID = int.Parse(auth.Result.Principal.Claims.First(t => t.Type.Equals(ClaimTypes.Sid))?.Value);
            bool isEmailHave = Context.User.ToList().Exists(x => x.email == user.email && x.id != userID);
            bool isNameHave = Context.User.ToList().Exists(x => x.userName == user.userName && x.id != userID);
            if (isEmailHave)
            {
                return BadRequest("邮箱已被注册");
            }
            if (isNameHave)
            {
                return BadRequest("用户名已被注册");
            }
            Entity.User userT = Context.User.FirstOrDefault(x => x.id == userID);
            if (userT != null)
            {
                userT.userName = user.userName;
                userT.email = user.email;
                userT.password = user.password;
                Context.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
