using DumpTruckManagementSystem.Application.Dtos.AuthDtos;
using DumpTruckManagementSystem.Application.Services;
using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Shared.Wrappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DumpTruckManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtService _jwtService;

        public AuthController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IJwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtService = jwtService;
        }

        /// <summary>
        /// تسجيل الدخول
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<AuthResponseDto>
                {
                    Succeeded = false,
                    Message = "بيانات غير صحيحة",
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                });
            }

            // البحث عن المستخدم بالاسم أو البريد الإلكتروني
            var user = await _userManager.FindByNameAsync(model.UserNameOrEmail) 
                       ?? await _userManager.FindByEmailAsync(model.UserNameOrEmail);

            if (user == null)
            {
                return Unauthorized(new Response<AuthResponseDto>
                {
                    Succeeded = false,
                    Message = "اسم المستخدم أو كلمة المرور غير صحيحة",
                    HttpStatusCode = HttpStatusCode.Unauthorized
                });
            }

            // التحقق من كلمة المرور
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return Unauthorized(new Response<AuthResponseDto>
                {
                    Succeeded = false,
                    Message = "اسم المستخدم أو كلمة المرور غير صحيحة",
                    HttpStatusCode = HttpStatusCode.Unauthorized
                });
            }

            // جلب أدوار المستخدم
            var roles = await _userManager.GetRolesAsync(user);

            // توليد Token
            var token = _jwtService.GenerateToken(user, roles);

            var response = new AuthResponseDto
            {
                Token = token,
                Expiration = DateTime.Now.AddHours(24),
                UserId = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                FullName = user.FullName ?? string.Empty
            };

            return Ok(new Response<AuthResponseDto>
            {
                Data = response,
                Succeeded = true,
                Message = "تم تسجيل الدخول بنجاح",
                HttpStatusCode = HttpStatusCode.OK
            });
        }

        /// <summary>
        /// إنشاء مستخدم جديد
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<AuthResponseDto>
                {
                    Succeeded = false,
                    Message = "بيانات غير صحيحة",
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                });
            }

            // التحقق من وجود المستخدم
            if (await _userManager.FindByNameAsync(model.UserName) != null)
            {
                return BadRequest(new Response<AuthResponseDto>
                {
                    Succeeded = false,
                    Message = "اسم المستخدم موجود بالفعل",
                    HttpStatusCode = HttpStatusCode.BadRequest
                });
            }

            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                return BadRequest(new Response<AuthResponseDto>
                {
                    Succeeded = false,
                    Message = "البريد الإلكتروني موجود بالفعل",
                    HttpStatusCode = HttpStatusCode.BadRequest
                });
            }

            // إنشاء مستخدم جديد
            var user = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName,
                EmailConfirmed = true // يمكنك تغيير هذا حسب احتياجك
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new Response<AuthResponseDto>
                {
                    Succeeded = false,
                    Message = "فشل إنشاء المستخدم",
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Errors = result.Errors.Select(e => e.Description).ToList()
                });
            }

            // إضافة المستخدم إلى دور "User" (إذا كان موجوداً)
            var userRole = await _roleManager.FindByNameAsync("User");
            if (userRole != null)
            {
                await _userManager.AddToRoleAsync(user, "User");
            }

            // جلب أدوار المستخدم
            var roles = await _userManager.GetRolesAsync(user);

            // توليد Token
            var token = _jwtService.GenerateToken(user, roles);

            var response = new AuthResponseDto
            {
                Token = token,
                Expiration = DateTime.Now.AddHours(24),
                UserId = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                FullName = user.FullName ?? string.Empty
            };

            return Ok(new Response<AuthResponseDto>
            {
                Data = response,
                Succeeded = true,
                Message = "تم إنشاء المستخدم بنجاح",
                HttpStatusCode = HttpStatusCode.Created
            });
        }
    }
}
