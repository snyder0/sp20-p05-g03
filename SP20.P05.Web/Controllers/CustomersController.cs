using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SP20.P05.Web.Data;
using SP20.P05.Web.Features.Authentication;

namespace SP20.P05.Web.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly DataContext dataContext;
        private readonly UserManager<User> userManager;

        public CustomersController(UserManager<User> userManager, DataContext dataContext)
        {
            this.userManager = userManager;
            this.dataContext = dataContext;
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(CreateCustomerDto dto)
        {
            var newUser = new User
            {
                UserName = dto.Username
            };

            // wrapping in a transaction means that if part of the transaction fails then everything saved is undone
            using (var transaction = await dataContext.Database.BeginTransactionAsync())
            {

                var identityResult = await userManager.CreateAsync(newUser, dto.Password);
                if (!identityResult.Succeeded)
                {
                    return BadRequest();
                }

                var roleResult = await userManager.AddToRoleAsync(newUser, Roles.Customer);
                if (!roleResult.Succeeded)
                {
                    return BadRequest();
                }

                transaction.Commit(); // this marks our work as done

                return Created(string.Empty, new UserDto
                {
                    Username = newUser.UserName
                });
            }
        }
    }
}