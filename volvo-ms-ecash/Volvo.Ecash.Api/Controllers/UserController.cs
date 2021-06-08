using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Volvo.Ecash.Application.Service.Interface;
using Volvo.Ecash.Application.Validator;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/ecash/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Contructor
        /// </summary>        
        /// <param name="userService"></param>        
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        [Authorize("Bearer")]
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _userService.SelectUsersAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpGet("Get")]
        [Authorize("Bearer")]
        public async Task<UserDto> GetUser([FromQuery] string user)
        {
            return await _userService.SelectUserAsync(user);
        }

        /// <summary>
        /// Createuser 
        /// </summary>
        /// <param name="userLogin"></param>        
        /// <returns>object</returns>
        [HttpPost("Create")]
        [Authorize("Bearer")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<object> Createuser([FromBody] UserLogin userLogin)
        {
            var result = await _userService.CreateUser<UserValidator>(userLogin);

            if (result != null)
                return this.StatusCode(StatusCodes.Status201Created, result);
            else
                return this.StatusCode(StatusCodes.Status400BadRequest, result);
        }

        /// <summary>
        /// Updateuser
        /// </summary>
        /// <param name="user"></param>
        /// <param name="iduser"></param>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpPatch("Update/{iduser}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<object> Updateuser([FromBody] UserDto user, int iduser)
        {

            var result = await _userService.UpdateUser(iduser, user);

            if (result != null)
                return this.StatusCode(StatusCodes.Status200OK, result);
            else
                return this.StatusCode(StatusCodes.Status400BadRequest, result);
        }

        /// <summary>
        /// Deleteuser (seta active = false, inativa)
        /// </summary>
        /// <param name="iduser"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{iduser}")]
        [Authorize("Bearer")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<object> Deleteuser(int iduser)
        {

            var result = await _userService.DeleteUser(iduser);

            if (result != null)
                return this.StatusCode(StatusCodes.Status201Created, result);
            else
                return this.StatusCode(StatusCodes.Status400BadRequest, result);

        }

        /// <summary>
        /// updatePassword 
        /// </summary>
        /// <param name="userLogin"></param>        
        /// <returns>object</returns>
        [HttpPut("updatePassword")]
        [Authorize("Bearer")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<object> Put([FromBody] UserLogin userLogin)
        {
            var result = await _userService.UpdatePassword<UserValidator>(userLogin);

            if (result.Success)
                return this.StatusCode(StatusCodes.Status200OK, result);
            else
                return this.StatusCode(StatusCodes.Status400BadRequest, result.Message);
        }
    }
}
