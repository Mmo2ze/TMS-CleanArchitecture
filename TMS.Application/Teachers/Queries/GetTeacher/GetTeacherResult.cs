﻿using TMS.Domain.Teachers;

namespace TMS.Application.Teachers.Queries.GetTeacher;

public record GetTeacherResult(
    TeacherId Id,
    string Name,
    string Phone,
    string? Email,
    int StudentsCount,
    bool IsActive,
    Subject Subject,
    DateOnly EndOfSubscription);
    