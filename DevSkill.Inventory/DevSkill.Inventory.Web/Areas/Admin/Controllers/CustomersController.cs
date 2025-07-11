using AutoMapper;
using DevSkill.Inventory.Application.Features.Categories.Queries;
using DevSkill.Inventory.Application.Features.MeasurementUnits.Queries;
using DevSkill.Inventory.Application.Features.Products.Commands;
using DevSkill.Inventory.Application.Features.Products.Queries;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Infrastructure.Utilities;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using DevSkill.Inventory.Application.Features.Customers.Queries;
using DevSkill.Inventory.Web.Areas.Admin.Models.Products;
using DevSkill.Inventory.Web.Areas.Admin.Models.Customers;
using DevSkill.Inventory.Application.Features.Customers.Commands;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomersController : Controller
    {
        private readonly ILogger<CustomersController> _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IImageUtility _imageUtility;

        public CustomersController(
            ILogger<CustomersController> logger,
            IMapper mapper,
            IMediator mediator,
            IImageUtility imageUtility)
        {
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
            _imageUtility = imageUtility;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAsync(AddCustomerModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {                    
                    var customer = _mapper.Map<CustomerAddCommand>(model);

                    customer.ImageUrl = await _imageUtility.UploadImage(model.Image, model.ImageUrl);
                    customer.Id = IdentityGenerator.NewSequentialGuid();
                    await _mediator.Send(customer);

                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = "customer Added",
                        Type = ResponseTypes.Success
                    });
                }
                catch (Application.Exceptions.DuplicateNameException de)
                {
                    ModelState.AddModelError("DuplicateCustomer", de.Message);
                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = de.Message,
                        Type = ResponseTypes.Danger
                    });
                }
                catch (Exception ex)
                {
                    string message = "Failed to add customer";
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
        public async Task<JsonResult> GetCustomerJsonDataAsync([FromBody] CustomerGetListQuery model)
        {
            try
            {
                var (data, total, totalDisplay) = await _mediator.Send(model);
                var customers = new
                {
                    recordsTotal = total,
                    recordsFiltred = totalDisplay,
                    data = (from record in data
                            select new string[]
                            {
                                record.Id.ToString(),
                                $"<img src='{"/" + HttpUtility.HtmlDecode(record.ImageUrl ?? string.Empty)}' alt='Image' width='80' height='70'/>",
                                HttpUtility.HtmlEncode(record.CustomerId),
                                HttpUtility.HtmlEncode(record.Name),
                                HttpUtility.HtmlEncode(record.Mobile),
                                HttpUtility.HtmlEncode(record.Address),
                                HttpUtility.HtmlEncode(record.Email),
                                HttpUtility.HtmlEncode(record.Balance),
                                HttpUtility.HtmlEncode(record.Status.Equals(true) ? "Active" : "Inactive"),
                                record.Id.ToString()
                            }).ToArray()
                };
                return Json(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "there was a problem getting customers");
                return Json(DataTables.EmptyResult);
            }
        }        
    }
}
