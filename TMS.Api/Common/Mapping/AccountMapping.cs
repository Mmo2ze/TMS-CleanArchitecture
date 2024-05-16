using Mapster;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.Accounts.Commands.Create;
using TMS.Contracts.Account.Create;
using TMS.Contracts.Teacher.Create;
using TMS.Domain.Groups;
using TMS.Domain.Students;

namespace TMS.Api.Common.Mapping;

public class AccountMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateAccountRequest, CreateAccountCommand>()
            .Map(dest => dest.GroupId, src => GroupId.Create(src.GroupId))
            .Map(dest => dest.StudentId, src => StudentId.Create(src.StudentId));
        
    }
}