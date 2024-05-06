using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using WebAopSample.Models;
using WebAopSample.Repositories;
using WebAopSample.Services;

namespace WebAppTestProject;

[TestFixture]
public class UserServiceTests
{
    private UserService userService;

    private Mock<IUserRepository> userRepositoryMock;

    [SetUp]
    public void SetUp() {
        userRepositoryMock = new Mock<IUserRepository>();
        userService = new UserService(userRepositoryMock.Object);
    }

    [Test]
    public void InsertUser_Test()
    {
        UserModel user = new UserModel {
            Name = "ben",
            Password = "password",
            Phone  = "123231231",
            Email = "email",

        };
        userRepositoryMock.Setup(mock => mock.Add(user)).Returns(user);
        var result = userService.Insert(user);

        Assert.That(result, Is.Not.Null);

        userRepositoryMock.Verify(repo => repo.Add(user), Times.Once);
    }
}