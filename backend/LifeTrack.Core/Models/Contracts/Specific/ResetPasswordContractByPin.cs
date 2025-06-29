namespace LifeTrack.Core.Models.Contracts.Specific;

public record ResetPasswordContractByPin(Guid UserId, string Pin, string NewPassword, string ConfirmNewPassword);