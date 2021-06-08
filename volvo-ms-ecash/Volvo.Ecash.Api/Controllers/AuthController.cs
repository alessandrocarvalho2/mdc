using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
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
    public class AuthController : ControllerBase
    {

        private readonly IUserService _userService;

        /// <summary>
        /// Contructor
        /// </summary>        
        /// <param name="userService"></param>        
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// GetToken 
        /// </summary>
        /// <param name="userLogin"></param>        
        /// <returns>object</returns>
        [HttpPost("token", Name = "token")]
        [AllowAnonymous]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<object> Post([FromBody] UserLogin userLogin)
        {
            var result = await _userService.GetByUser<UserValidator>(userLogin);

            if (result.Success)
                return this.StatusCode(StatusCodes.Status200OK, result);
            else
                return this.StatusCode(StatusCodes.Status400BadRequest, result.Message);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="refreshTokenDto"></param>
        /// <param name="getToken"></param>
        /// <returns></returns>
        [HttpPost("refreshToken", Name = "refreshToken")]
        [AllowAnonymous]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<object> Post([FromBody] RefreshTokenDto refreshTokenDto, bool getToken = false)
        {

            var result = await _userService.RefreshToken(refreshTokenDto, getToken);

            if (result.Success)
                return this.StatusCode(StatusCodes.Status200OK, result);
            else
                return this.StatusCode(StatusCodes.Status400BadRequest, result.Message);
        }

        /// <summary>
        /// RefreshToken 
        /// </summary>
        /// <param name="userDto"></param>        
        /// <returns>object</returns>
        [HttpPost("logout/", Name = "logout")]
        [AllowAnonymous]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<object> LogOut([FromBody] UserDto userDto)
        {

            var result = await _userService.InvalidateRefreshToken(userDto);

            if (result.Success)
                return this.StatusCode(StatusCodes.Status200OK, result);
            else
                return this.StatusCode(StatusCodes.Status400BadRequest, result.Message);

        }
    }
}
