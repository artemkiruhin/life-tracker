using System.Security.Claims;
using LifeTrack.Core.Interfaces.Services.Entity;
using LifeTrack.Core.Models.Contracts.API.Account;
using LifeTrack.Core.Models.Contracts.Specific;
using LifeTrack.Core.Models.Contracts.Update;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LifeTrack.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetInfo(CancellationToken ct)
        {
            try
            {
                var userGuidClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                if (userGuidClaim == null) return Unauthorized();
                var userGuid = Guid.Parse(userGuidClaim.Value);
                
                var userInfo = await _userService.GetUser(userGuid, ct);
                if (!userInfo.IsSuccess) return BadRequest($"Error during getting info: {userInfo.ErrorMessage}");
                return Ok(new { data = userInfo.Data });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }
        
        [HttpDelete("")]
        public async Task<IActionResult> DeleteAccount(CancellationToken ct)
        {
            try
            {
                var userGuidClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                if (userGuidClaim == null) return Unauthorized();
                var userGuid = Guid.Parse(userGuidClaim.Value);
                
                var deleteResult = await _userService.Delete(userGuid, ct);
                if (!deleteResult.IsSuccess) return BadRequest($"Error during deleting info: {deleteResult.ErrorMessage}");
                
                HttpContext.Response.Cookies.Delete("jwt");
                
                return Ok(new { data = deleteResult.Data });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }

        [HttpPatch("")]
        public async Task<IActionResult> UpdateAccount(AccountUpdateRequest request, CancellationToken ct)
        {
            try
            {
                var userGuidClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                if (userGuidClaim == null) return Unauthorized();
                var userGuid = Guid.Parse(userGuidClaim.Value);

                var updateContract = new UserUpdateContract(userGuid, request.Username, request.Email, userGuid);
                var updateResult = await _userService.Update(updateContract, ct);
                if (!updateResult.IsSuccess) return BadRequest($"Error during updating info: {updateResult.ErrorMessage}");
                
                return Ok(new { data = updateResult.Data });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }
 
        [HttpPatch("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request, CancellationToken ct)
        {
            try
            {
                var userGuidClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                if (userGuidClaim == null) return Unauthorized();
                var userGuid = Guid.Parse(userGuidClaim.Value);

                var resetPasswordContract = new ResetPasswordContract(userGuid, request.OldPassword, request.NewPassword, request.ConfirmNewPassword);
                var resetContract = await _userService.ResetPassword(resetPasswordContract, ct);
                if (!resetContract.IsSuccess) return BadRequest($"Error during resetting password: {resetContract.ErrorMessage}");
                
                return Ok(new { data = resetContract.Data });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }
        
        
        [HttpPatch("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ChangePasswordByPinRequest request, CancellationToken ct)
        {
            try
            {
                var userGuidClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                if (userGuidClaim == null) return Unauthorized();
                var userGuid = Guid.Parse(userGuidClaim.Value);

                var resetPasswordContract = new ResetPasswordContractByPin(userGuid, request.Pin, request.NewPassword, request.ConfirmNewPassword);
                var resetContract = await _userService.ResetPassword(resetPasswordContract, ct);
                if (!resetContract.IsSuccess) return BadRequest($"Error during resetting password: {resetContract.ErrorMessage}");
                
                return Ok(new { data = resetContract.Data });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }
    }
}
