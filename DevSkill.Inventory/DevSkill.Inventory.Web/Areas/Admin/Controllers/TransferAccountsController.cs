using AutoMapper;
using DevSkill.Inventory.Application.Exceptions;
using DevSkill.Inventory.Application.Features.Products.Commands;
using DevSkill.Inventory.Application.Features.TransferAccounts.Queries;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Web.Areas.Admin.Models.Products;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using DevSkill.Inventory.Web.Areas.Admin.Models.TransferAccounts;
using DevSkill.Inventory.Application.Features.TransferAccounts.Commands;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TransferAccountsController : Controller
    {
        private readonly ILogger<TransferAccountsController> _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public TransferAccountsController(
            ILogger<TransferAccountsController> logger,
            IMapper mapper,
            IMediator mediator)
        {
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAsync(AddTransferAccountModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {                    
                    var transferAccount = _mapper.Map<TransferAccountAddCommand>(model);                    
                    transferAccount.Id = IdentityGenerator.NewSequentialGuid();
                    await _mediator.Send(transferAccount);

                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = "transferAccount Added",
                        Type = ResponseTypes.Success
                    });
                }                
                catch (Exception ex)
                {
                    string message = "Failed to add transferAccount";
                    _logger.LogError(ex, message);
                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = message,
                        Type = ResponseTypes.Danger
                    });
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<JsonResult> GetTransferAccountJsonDataAsync([FromBody] TransferAccountGetListQuery query)
        {
            try
            {
                var (data, total, totalDisplay) = await _mediator.Send(query);

                var transferAccount = new
                {
                    recordsTotal = total,
                    recordsFiltered = totalDisplay,
                    data = (from record in data
                            select new string[]
                            {
                                record.Id.ToString(),                                
                                record.TransferDate,
                                HttpUtility.HtmlEncode(record.FromAccountDisplay),
                                HttpUtility.HtmlEncode(record.ToAccountDisplay),
                                record.TransferAmount.ToString(),
                                HttpUtility.HtmlEncode(record.Note),
                                record.Id.ToString()
                            }).ToArray()
                };

                return Json(transferAccount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem in getting books");
                return Json(DataTables.EmptyResult);
            }
        }
    }
}
