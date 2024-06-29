using TMS.Domain.Common.Enums;

namespace TMS.Contracts.Student.Get;

public record StudentDto(
    string Id,
    string Name,
    string? Phone,
    string? Email,
    Gender Gender,
    bool? HasWhatsapp);