using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAopSample.Models;
using WebAopSample.Services;

namespace WebAopSample.Controllers;


[ApiController]
[Route("api/[Controller]")]
public class UserController(IUserService userService) : Controller
{
    [HttpPost]
    public ActionResult AddUser(UserModel user) {
        userService.Insert(user);
        return Ok();
    }
}
