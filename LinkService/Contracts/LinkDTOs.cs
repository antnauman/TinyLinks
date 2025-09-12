namespace LinkService.Contracts;

public record CreateLinkRequestDTO(string TargetUrl, int? Threshold);
public record LinkResponseDTO(Guid id, string Code, string TargetUrl, int? Threshold, string OwnerSub, DateTime CreatedUtc);
