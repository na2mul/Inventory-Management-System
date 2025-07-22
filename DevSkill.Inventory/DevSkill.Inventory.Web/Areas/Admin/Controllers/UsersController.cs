using AutoMapper;
using DevSkill.Inventory.Application.Features.Users.Queries;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Infrastructure.Identity;
using DevSkill.Inventory.Infrastructure.UserService;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using DevSkill.Inventory.Web.Areas.Admin.Models.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly IMediator _mediator;
        private readonly ILogger<UsersController> _logger;
        private readonly ICreateUserService _createUserService;
        public UsersController(UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            IMediator mediator,
            ILogger<UsersController> logger,
            ICreateUserService createUserService)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _mediator = mediator;
            _logger = logger;
            _createUserService = createUserService;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public IActionResult AddUserAsync()
        {
            var model = new AddUserModel();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUserAsync(AddUserModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = _createUserService.CreateUser();


                    await _userStore.SetUserNameAsync(user, model.Email, CancellationToken.None);
                    await _emailStore.SetEmailAsync(user, model.Email, CancellationToken.None);

                    user.RegistrationDate = DateTime.UtcNow;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.Email;
                    user.Status = "Active";
                    user.PhoneNumber = model.PhoneNumber;
                    user.CompanyName = model.CompanyName;

                    var result = await _userManager.CreateAsync(user, model.Password);

                    await _userManager.AddToRoleAsync(user, model.Role);

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "User added",
                        Type = ResponseTypes.Success
                    });
                }
                catch (Exception ex)
                {
                    var message = "User Create Failed";
                    ModelState.AddModelError("UserCreateFailed", message);
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = message,
                        Type = ResponseTypes.Danger
                    });
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<JsonResult> GetUserJsonDataAsync([FromBody] UserGetListQuery query)
        {
            try
            {
                var totalUsers = await _userManager.Users.CountAsync();

                var users = await _userManager.Users
                    .Skip((query.PageIndex - 1) * query.PageSize)
                    .Take(query.PageSize == -1 ? int.MaxValue : query.PageSize)
                    .ToListAsync();
                
                var userList = new List<UserGetListQuery>();

                foreach (var usr in users)
                {
                    var roles = await _userManager.GetRolesAsync(usr);

                    userList.Add(new UserGetListQuery
                    {
                        Id = usr.Id,
                        FirstName = $"{usr.FirstName} {usr.LastName}",
                        CompanyName = usr.CompanyName,
                        Email = usr.Email,
                        PhoneNumber = usr.PhoneNumber,
                        Role = string.Join(", ", roles),
                        Status = usr.Status
                    });
                }
                
                var result = (data: userList, total: totalUsers, totalDisplay: userList.Count);
               
                var user = new
                {
                    recordsTotal = result.total,
                    recordsFiltered = result.totalDisplay,
                    data = (from record in result.data
                            select new string[]
                            {
                                record.Id.ToString(),
                                record.FirstName,
                                HttpUtility.HtmlEncode(record.CompanyName),
                                HttpUtility.HtmlEncode(record.Email),
                                HttpUtility.HtmlEncode(record.PhoneNumber),
                                HttpUtility.HtmlEncode(record.Role),
                                HttpUtility.HtmlEncode(record.Status),
                                record.Id.ToString()
                            }).ToArray()
                };

                return Json(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem in getting Users");
                return Json(DataTables.EmptyResult);
            }
        }
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());

                if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Any())
                    {
                        var roleRemoveResult = await _userManager.RemoveFromRolesAsync(user, roles);
                        if (!roleRemoveResult.Succeeded)
                        {
                            TempData.Put("ResponseMessage", new ResponseModel
                            {
                                Message = "Failed to remove user roles: " + string.Join("; ", roleRemoveResult.Errors.Select(e => e.Description)),
                                Type = ResponseTypes.Danger
                            });
                            return RedirectToAction("Index");
                        }
                    }

                    var deleteResult = await _userManager.DeleteAsync(user);

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = deleteResult.Succeeded ? "User deleted successfully" : "Delete failed: " + string.Join("; ", deleteResult.Errors.Select(e => e.Description)),
                        Type = deleteResult.Succeeded ? ResponseTypes.Success : ResponseTypes.Danger
                    });

                    return RedirectToAction("Index");
                }

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "User not found",
                    Type = ResponseTypes.Danger
                });
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "An error occurred: " + ex.Message,
                    Type = ResponseTypes.Danger
                });
                return RedirectToAction("Index");
            }
        }        

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
