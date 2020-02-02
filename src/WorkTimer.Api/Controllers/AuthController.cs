using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WorkTimer.Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {
        // GET: api/Auth
        [HttpPost("login")]
        public IActionResult Login() {
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, "some_id"),
                new Claim("granny", "cookie")
            };

            var secretBytes = Encoding.UTF8.GetBytes("adfadfaldjganvut4oauuou9993449997rnaflfhfaljfdlgjs");
            var key = new SymmetricSecurityKey(secretBytes);
            var algorithm = SecurityAlgorithms.HmacSha256;

            var signingCredentials = new SigningCredentials(key, algorithm);

            var token = new JwtSecurityToken(
                    "https://localhost:5661/",
                    "https://localhost:5661/",
                    claims,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials);

            var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { access_token = tokenJson });
        }

        // GET: api/Auth/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id) {
            return "value";
        }

        // POST: api/Auth
        [HttpPost]
        public void Post([FromBody] string value) {
        }

        // PUT: api/Auth/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value) {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id) {
        }
    }
}
