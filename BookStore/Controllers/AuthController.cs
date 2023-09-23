using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Dtos;
using BookStore.IServices;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookStore.Controllers
{
    [Authorize]
    [Route("api/v1/authenticate")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register"), AllowAnonymous]
        public async Task<IActionResult> register([FromBody]Customer request)
        {
            try
            {
                await _authService.CreateUserAsync(request);
                return Ok("Create user successfully!");
            }catch(Exception e)
            {
                return BadRequest($"Error: {e}");
            }
        }

        [HttpPost("login"), AllowAnonymous]
        public async Task<IActionResult> login([FromBody]UserDto request)
        {
            try
            {
                if(request.UserName == null || request.Password == null)
                {
                    return BadRequest("Invalid username or password!");
                }
                string token = await _authService.LoginAsync(request);
                return Ok(token);
            }catch(Exception e)
            {
                return BadRequest($"Error: {e}");
            }
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrent()
        {
            try
            {
                string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;


                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest($"Invalid token!");
                }

                var customer = await _authService.getCurrentUser(token);
                return Ok(customer);
            }catch(Exception e)
            {
                return BadRequest($"Error: {e}");
            }
        }
    }
}

