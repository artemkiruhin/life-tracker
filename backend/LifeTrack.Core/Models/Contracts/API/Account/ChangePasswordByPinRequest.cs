namespace LifeTrack.Core.Models.Contracts.API.Account;

public record ChangePasswordByPinRequest(string Pin, string NewPassword, string ConfirmNewPassword);