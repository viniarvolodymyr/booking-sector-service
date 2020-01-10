using Microsoft.AspNetCore.Mvc;
using SoftServe.BookingSectors.WebAPI.BLL.DTO;
using SoftServe.BookingSectors.WebAPI.BLL.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoftServe.BookingSectors.WebAPI.BLL.Filters;
using Microsoft.AspNetCore.Authorization;
using System.Web.Helpers;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;

namespace SoftServe.BookingSectors.WebAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IRegistrationService registrationService;

        public UserController(IUserService userService, IRegistrationService registrationService)
        {
            this.userService = userService;
            this.registrationService = registrationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> Get()
        {
            var dtos = await userService.GetAllUsersAsync();
            if (!dtos.Any())
            {
                return NotFound();
            }
            return Ok(dtos);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<UserDTO>> GetById([FromRoute]int id)
        {
            var dto = await userService.GetUserByIdAsync(id);
            if (dto == null)
            {
                return NotFound();
            }
            return Ok(dto);
        }

        [HttpGet]
        [Route("phone/{phone}")]
        public async Task<ActionResult<UserDTO>> GetByPhone([FromRoute]string phone)
        {
            var dto = await userService.GetUserByPhoneAsync(phone);
            if (dto == null)
            {
                return NotFound();
            }
            return Ok(dto);
        }
        [HttpGet]
        [Route("UserPhoto/{id}")]
        public async Task<string> GetPhotoById([FromRoute]int id)
        {
            var file = await userService.GetUserPhotoById(id);
         
            return file;
        }

        [HttpGet]
        [Route("{id}/{password}")]
        public async Task<bool> PasswordCheck([FromRoute]string password, [FromRoute]int id)
        {
            bool result = await userService.CheckPasswords(password, id);
            return result;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidateModelState))]
        public async Task<IActionResult> Post([FromBody] RegistrationDTO userDTO)
        {
            var dto = await registrationService.InsertUserAsync(userDTO);

            if (dto == null)
            {
                return BadRequest();
            }
            else
            {
                return Created($"api/users/{dto.Id}", dto);
            }
        }

        [HttpPut]
        [Route("{id}")]       
        public async Task<IActionResult> UpdateUser([FromRoute]int id, [FromBody]UserDTO userDTO)
        {
            var user = await userService.UpdateUserById(id, userDTO);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(user);
            }
        }

        [HttpPut]
        [Route("pass/{id}")]
        public async Task<IActionResult> UpdateUserPass([FromRoute]int id, [FromBody]UserDTO userDTO)
        {
            var user = await userService.UpdateUserPassById(id, userDTO);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(user);
            }
        }
        [HttpPut]
        [Route("photo/{id}")]
        public async Task<IActionResult> UpdateUserPhoto([FromRoute]int id, [FromForm] IFormFile file)
        {
           
            var user = await userService.UpdateUserPhotoById(id, file);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(user);
            }
        }
        [HttpPut]
        [Route("email/{email}")]
        public async Task<IActionResult> SendEmail([FromRoute]string email)
        {

            var user = await userService.SendEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(user);
            }
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            var user = await userService.DeleteUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(user);
            }
        }
    }
}

