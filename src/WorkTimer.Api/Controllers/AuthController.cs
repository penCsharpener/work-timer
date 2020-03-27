using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WorkTimer.Api.Contracts;
using WorkTimer.Api.Models;

namespace WorkTimer.Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {
        private readonly IWebTokenBuilder _webTokenBuilder;
        private readonly IAuthProvider _authProvider;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IWebTokenBuilder webTokenBuilder,
                              IAuthProvider authProvider,
                              ILogger<AuthController> logger) {
            _webTokenBuilder = webTokenBuilder;
            _authProvider = authProvider;
            _logger = logger;
        }

        // GET: api/Auth
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel) {
            if (!ModelState.IsValid) {
                _logger.LogError($"Invalid model for user: ${loginModel?.Email} and password with length ${loginModel?.Password?.Length ?? 0}");
                return BadRequest(ModelState);
            }

            var user = await _authProvider.Login(loginModel);

            if (user == null) {
                _logger.LogError($"User not found for user: ${loginModel?.Email} and password with length ${loginModel?.Password?.Length ?? 0}");
                return BadRequest();
            }

            var token = _webTokenBuilder.GenerateToken(user);
            return Ok(new { user.FirstName, user.LastName, Token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel) {
            if (!ModelState.IsValid) {
                _logger.LogError($"Invalid model for user: ${registerModel?.Email} and password with length ${registerModel?.Password?.Length ?? 0}");
                return BadRequest(ModelState);
            }

            if (registerModel.Password != registerModel.PasswordConfirm) {
                _logger.LogError($"Password inputs don't match for {registerModel?.Email}");
                return BadRequest("Password inputs don't match.");
            }

            if (await _authProvider.UserExists(registerModel)) {
                return BadRequest("User already registered.");
            }

            var user = await _authProvider.Register(registerModel);

            if (user == null) {
                _logger.LogError($"User could not be registered: ${registerModel?.Email}");
                return BadRequest("User could not be registered");
            }

            var token = _webTokenBuilder.GenerateToken(user);
            return Ok(new { user.FirstName, user.LastName, Token = token });
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
