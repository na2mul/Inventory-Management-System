using AutoMapper;
using DevSkill.Inventory.Application.Exceptions;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Infrastructure.Utilities;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using DevSkill.Inventory.Application.Features.Sales.Queries;
using DevSkill.Inventory.Application.Features.Customers.Commands;
using DevSkill.Inventory.Web.Areas.Admin.Models.Customers;
using Newtonsoft.Json;
using DevSkill.Inventory.Application.Features.Products.Queries;
using DevSkill.Inventory.Application.Features.Products.Commands;
using DevSkill.Inventory.Web.Areas.Admin.Models.Products;
using DevSkill.Inventory.Web.Areas.Admin.Models.Sales;
using DevSkill.Inventory.Application.Features.Sales.Commands;
using DevSkill.Inventory.Application.Features.Customers.Queries;
using DevSkill.Inventory.Application.Features.AccountTypes.Queries;
using DevSkill.Inventory.Application.Features.Accounts.Queries;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class SalesController : Controller
    {
        private readonly ILogger<SalesController> _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IImageUtility _imageUtility;

        public SalesController(
            ILogger<SalesController> logger,
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
        public async Task<IActionResult> AddAsync(AddSaleModel model)
        {           

            if (ModelState.IsValid)
            {
                try
                {                  
                    var sale = _mapper.Map<SaleAddCommand>(model);

                    sale.Id = IdentityGenerator.NewSequentialGuid();
                    await _mediator.Send(sale);

                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = "sale Added",
                        Type = ResponseTypes.Success
                    });
                }                
                catch (Exception ex)
                {
                    string message = "Failed to add sale";
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
        public async Task<JsonResult> GetSaleJsonDataAsync([FromBody] SaleGetListQuery model)
        {
            try
            {
                var (data, total, totalDisplay) = await _mediator.Send(model);
                var sales = new
                {
                    recordsTotal = total,
                    recordsFiltered = totalDisplay,
                    data = (from record in data
                            select new string[]
                            {
                                record.Id.ToString(),                                
                                HttpUtility.HtmlEncode(record.InvoiceNo),
                                HttpUtility.HtmlEncode(record.SaleDate),
                                HttpUtility.HtmlEncode(record.CustomerName),
                                HttpUtility.HtmlEncode(record.TotalAmount),
                                HttpUtility.HtmlEncode(record.PaidAmount),
                                HttpUtility.HtmlEncode(record.DueAmount),
                                HttpUtility.HtmlEncode(record.Status),
                                record.Id.ToString()
                            }).ToArray()
                };
                return Json(sales);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "there was a problem getting Sales");
                return Json(DataTables.EmptyResult);
            }
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCustomerAsync(AddCustomerModel model)
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
                    return Json(new
                    {
                        success = true,
                        customerId = customer.Id,
                        customerName = $"{customer.Mobile} ( {customer.Name} )"
                    });
                }
                catch (DuplicateCustomerNameException de)
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
            return Json(new { success = false, message = "Invalid data provided" });
        }

       
        public async Task<IActionResult> GetProductDetailsAsync(Guid id, int saleType)
        {
            try
            {                
                if (id == null || saleType==0)
                {
                    return Content("0"); 
                }
                
                var product = await _mediator.Send(new ProductGetQuery() { Id = id});
                if (product == null)
                {
                    return Content("1");
                }
               
                if (product.Stock <= 0)
                {
                    return Content("2"); 
                }                
                
                if(saleType == 1)
                {
                    var productData = new
                    {
                        Id = product.Id,
                        Barcode = product.Barcode,
                        Name = product.Name,
                        Price = product.MRP,
                        Stock = product.Stock,
                        Quantity = 1,
                        Subtotal = product.MRP
                    };
                    return Content(JsonConvert.SerializeObject(productData));
                }
                else
                {
                    var productData = new
                    {
                        Id = product.Id,
                        Barcode = product.Barcode,
                        Name = product.Name,
                        Price = product.WholesalePrice,
                        product.Stock,
                        Quantity = 1,
                        Subtotal = product.WholesalePrice
                    };
                    return Content(JsonConvert.SerializeObject(productData));
                }                   
            }
            catch (Exception ex)
            {
                string message = "Error in GetProductDetails";
                _logger.LogError(ex, message);
                return Content("3");
            }
        }

        public async Task<IActionResult> GetAccountTypesAsync()
        {
            try
            {
                var accountTypeList = await _mediator.Send(new AccountTypeGetAllQuery());
                return Json(accountTypeList);
            }
            catch (Exception ex)
            {
                string error = "there was a problem getting AccountType";
                _logger.LogError(ex, error);
                return Json(new { success = false, message = error });
            }
        }
        public async Task<IActionResult> GetAccountsByTypeAsync(Guid accountTypeId)
        {
            try
            {
                var accountList = await _mediator.Send(new AccountGetAllQuery() { Id = accountTypeId });
                return Json(accountList);
            }
            catch (Exception ex)
            {
                string error = "there was a problem getting accounts";
                _logger.LogError(ex, error);
                return Json(new { success = false, message = error });
            }
        }
    }
}
