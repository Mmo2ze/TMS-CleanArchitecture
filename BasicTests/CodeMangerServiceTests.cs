using TMS.Application.Common.Enums;
using TMS.Application.Common.Interfaces.Auth;
using TMS.Domain.Common.Errors;
using TMS.Infrastructure.Auth;

namespace BasicTests;

public class CodeMangerServiceTests
{
    private readonly ICodeManger _codeManger = new CodeManger();

    [Fact]
    public async Task VerifyCodeShouldReturnInvalidCredentialsWhenCodeNotFound()
    {
        var result = await _codeManger.VerifyCode("", UserAgent.Admin, "1234");
        Assert.Equal(Errors.Auth.InvalidCredentials, result);
    }

    [Fact]
    public void GenerateCodeShouldReturnCode()
    {
        var phone = "1234567890";
        var userAgent = UserAgent.Admin;
        var result = _codeManger.GenerateCode(phone, userAgent);
        Assert.NotNull(result);
        Assert.Equal(phone, result.Phone);
        Assert.Equal(userAgent, result.Identify);
    }
    
    [Fact]
    public async Task VerifyCodeShouldReturnNullWhenCodeIsCorrect()
    {
        var phone = "1234567890";
        var userAgent = UserAgent.Admin;
        var code = _codeManger.GenerateCode(phone, userAgent);
        var result = await _codeManger.VerifyCode(phone, userAgent, code.Code);
        Assert.Null(result);
    }
    [Fact]
    public async Task VerifyCodeShouldReturnInvalidCodeWhenCodeIsInvalid()
    {
        var phone = "1234567890";
        var userAgent = UserAgent.Admin;
        var code = _codeManger.GenerateCode(phone, userAgent);
        var result = await _codeManger.VerifyCode(phone, userAgent, "bad code");
        Assert.Equal(Errors.Auth.InvalidCode, result);
    }
    
    [Fact]
    public async Task VerifyCodeShouldReturnToManyTryWhenCodeTriedMoreThan5()
    {
        var phone = "1234567890";
        var userAgent = UserAgent.Admin;
        _codeManger.GenerateCode(phone, userAgent);
        var result = await _codeManger.VerifyCode(phone, userAgent, "bad code");
        for (var i = 0; i < 5; i++)
        {
             result = await _codeManger.VerifyCode(phone, userAgent, "bad code");
        } 
        Assert.Equal(Errors.Auth.ToManyTry, result);
    }
    
}