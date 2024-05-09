using NSubstitute;
using NSubstitute.ReturnsExtensions;
using TMS.Application.Common.Services;
using TMS.Application.Common.Variables;
using TMS.Infrastructure.Services;

namespace BasicTests;

public class TeacherHelperTests
{
    private readonly ITeacherHelper _teacherHelper;
    private readonly IClaimsReader _claimsReader = Substitute.For<IClaimsReader>();

    public TeacherHelperTests()
    {
        _teacherHelper = new TeacherHelper(_claimsReader);
    }
    
    [Fact]
    public void IsTeacherShouldReturnTrueWhenTeacherRoleExists()
    {
        _claimsReader.GetRoles().Returns(["Teacher"]);
        var result = _teacherHelper.IsTeacher();
        Assert.True(result);
    }
    
    [Fact]
    public void IsTeacherShouldReturnFalseWhenTeacherRoleDoesNotExist()
    {
        _claimsReader.GetRoles().Returns(["Assistant"]);
        var result = _teacherHelper.IsTeacher();
        _claimsReader.GetRoles().Returns([]);
        var result2 = _teacherHelper.IsTeacher();
        Assert.False(result);
        Assert.False(result2);
    }
    
    [Fact]
    public void IsAssistantShouldReturnTrueWhenAssistantRoleExists()
    {
        _claimsReader.GetRoles().Returns(["Assistant"]);
        var result = _teacherHelper.IsAssistant();
        Assert.True(result);
    }
    
    [Fact]
    public void IsAssistantShouldReturnFalseWhenAssistantRoleDoesNotExist()
    {
        _claimsReader.GetRoles().Returns(["Teacher"]);
        var result = _teacherHelper.IsAssistant();
        _claimsReader.GetRoles().Returns([]);
        var result2 = _teacherHelper.IsAssistant();
        Assert.False(result);
        Assert.False(result2);
    }
    
    [Fact]
    public void GetTeacherIdShouldReturnTeacherIdWhenClaimExists()
    {
        var teacherId = "te_id";
        _claimsReader.GetByClaimType(CustomClaimTypes.TeacherId).Returns(teacherId);
        var result = _teacherHelper.GetTeacherId();
        Assert.NotNull(result);
        Assert.Equal(teacherId, result.Value);
    }
    
    [Fact]
    public void GetTeacherIdShouldReturnNullWhenClaimDoesNotExist()
    {
        _claimsReader.GetByClaimType(CustomClaimTypes.TeacherId).ReturnsNull();
        var result = _teacherHelper.GetTeacherId();
        Assert.Null(result);
    }
    
    [Fact]
    public void GetTeacherIdShouldReturnNullWhenClaimIsInvalid()
    {
        var teacherId = "invalid_id";
        _claimsReader.GetByClaimType(CustomClaimTypes.TeacherId).Returns(teacherId);
        var result = _teacherHelper.GetTeacherId();
        Assert.Null(result);
    }
    
    [Fact]
    public void GetAssistantIdShouldReturnAssistantIdWhenClaimExists()
    {
        var assistantId = "as_id";
        _claimsReader.GetByClaimType(CustomClaimTypes.Id).Returns(assistantId);
        var result = _teacherHelper.GetAssistantId();
        Assert.NotNull(result);
        Assert.Equal(assistantId, result.Value);
    }
    
    [Fact]
    public void GetAssistantIdShouldReturnNullWhenClaimDoesNotExist()
    {
        _claimsReader.GetByClaimType(CustomClaimTypes.Id).ReturnsNull();
        var result = _teacherHelper.GetAssistantId();
        Assert.Null(result);
    }
    
    [Fact]
    public void GetAssistantIdShouldReturnNullWhenClaimIsInvalid()
    {
        var assistantId = "invalid_id";
        _claimsReader.GetByClaimType(CustomClaimTypes.Id).Returns(assistantId);
        var result = _teacherHelper.GetAssistantId();
        Assert.Null(result);
    }
    
    
    
    
    
}