using Azure;
using BiblioManager.API.Dtos.Auth;
using BiblioManager.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;

namespace BiblioManager.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthsService _authService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IPasswordResetService _passwordResetService;
        public AuthController(IAuthsService authService, IRefreshTokenService refreshTokenService, IPasswordResetService passwordResetService)
        {
            _authService = authService;
            _refreshTokenService = refreshTokenService;
            _passwordResetService = passwordResetService;
        }

        /// <summary>
        /// User authentication Method
        /// </summary>
        /// <param name="request">Login request param</param>
        /// <returns>JWT Token</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);
            Response.Cookies.Append(
                "refreshToken",
                response.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = response.RefreshTokenExpiration
                });
            response.RefreshToken = string.Empty;
            return Ok(response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var result = await _refreshTokenService.RefreshTokenAsync(refreshToken);
            Response.Cookies.Append(
                "refreshToken", result.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = result.RefreshTokenExpiration
                });

            result.RefreshToken = string.Empty;
            return Ok(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (!string.IsNullOrEmpty(refreshToken))
            {
                await _authService.LogoutAsync(refreshToken);
            }
            Response.Cookies.Delete("refreshToken");
            return Ok(new
            {
                message = "Déconnexion réussie"
            });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _passwordResetService.RequestPasswordResetAsync(request.Email);
            return Ok(new
            {
                message = "Si un compte associé à cette adresse existe, " +
            "un email de réinitialisation a été envoyé."
            });
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _passwordResetService
                .ResetPasswordAsync(
                    request.Token,
                    request.NewPassword);

            return Ok(new
            {
                message = "Mot de passe réinitialisé avec succès."
            });
        }
    }
}
