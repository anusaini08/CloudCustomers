using CloudCustomers.Services;
using Microsoft.AspNetCore.Mvc;

namespace CloudCustomers.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
           _usersService = usersService;
        }

        [HttpGet("/GetUsers")]
        public async Task<IActionResult> Get()
        {
            var users = await _usersService.GetAllUsers();
            if(users.Any()) 
                {
                    return Ok(users); 
                }
            return NotFound();
        }
    }
}
