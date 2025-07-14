namespace LifeTrack.Core.Models.Contracts.API.Account;

public record ChangePasswordRequest(string OldPassword, string NewPassword, string ConfirmNewPassword);