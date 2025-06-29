namespace LifeTrack.Core.Models.Contracts.Specific;

public record ResetPasswordContract(Guid UserId, string OldPassword, string NewPassword, string ConfirmNewPassword);