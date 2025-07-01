using AutoMapper;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Application.Exceptions;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Web;
using DevSkill.Inventory.Infrastructure;
using MediatR;
using DevSkill.Inventory.Application.Features.Products.Commands;
using DevSkill.Inventory.Application.Features.Products.Queries;
using DevSkill.Inventory.Application.Features.Categories.Queries;
using DevSkill.Inventory.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using DevSkill.Inventory.Application.Features.MeasurementUnits.Queries;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(ILogger<ProductsController> logger, IMapper mapper,
            IMediator mediator, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Add()
        {
            var model = new AddProductModel();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categoryList = await _mediator.Send(new CategoryGetQuery());
            return Json(categoryList);
        }

        [HttpGet]
        public async Task<IActionResult> GetMeasurementUnits()
        {
            var MeasurementUnitList = await _mediator.Send(new MeasurementUnitGetQuery());
            return Json(MeasurementUnitList);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddProductModel model)
        {
            if (ModelState.IsValid)
            {
                var product = _mapper.Map<ProductAddCommand>(model);
                if (model.Image != null)
                {
                    string folder = "Images/Products/";
                    folder += Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                    model.ImageUrl = folder;
                    string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);

                    await model.Image.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                    product.ImageUrl = model.ImageUrl;
                }
                try
                {
                    product.Id = IdentityGenerator.NewSequentialGuid();
                    await _mediator.Send(product);
                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = "product Added",
                        Type = ResponseTypes.Success
                    });
                }
                catch (DuplicateProductNameException de)
                {
                    ModelState.AddModelError("DuplicateProduct", de.Message);
                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = de.Message,
                        Type = ResponseTypes.Danger
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to add product");
                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = "Failed to add product",
                        Type = ResponseTypes.Danger
                    });
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(Guid id)
        {
            var model = new ProductUpdateCommand();
            var product = await _mediator.Send(new ProductGetQuery() { Id = id });
            _mapper.Map(product, model);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ProductUpdateCommand model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _mediator.Send(model);
                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = "product Updated",
                        Type = ResponseTypes.Success

                    });
                    return RedirectToAction("Index");
                }
                catch (DuplicateProductNameException de)
                {
                    ModelState.AddModelError("DuplicateProduct", de.Message);
                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = de.Message,
                        Type = ResponseTypes.Danger

                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to update product");
                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = "Failed to update product",
                        Type = ResponseTypes.Danger

                    });
                }
            }
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _mediator.Send(new ProductDeleteCommand() { Id = id});
                TempData.Put("ResponseMessage", new ResponseModel()
                {
                    Message = "Product Deleted",
                    Type = ResponseTypes.Success
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete product");
                TempData.Put("ResponseMessage", new ResponseModel()
                {
                    Message = "Failed to delete product",
                    Type = ResponseTypes.Danger
                });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public  async Task<JsonResult> GetProductJsonData([FromBody] ProductListModel model)
        {
            try
            {
                var productQuery = _mapper.Map<ProductGetListQuery>(model);
                var (data, total, totalDisplay) = await _mediator.Send(productQuery);
                var products = new
                {
                    recordsTotal = total,
                    recordsFiltred = totalDisplay,
                    data = (from record in data
                            select new string[]
                            {
                                record.Id.ToString(),
                                $"<img src='{"/" + HttpUtility.HtmlDecode(record.ImageUrl ?? string.Empty)}' alt='Image' width='80' height='70'/>",
                                HttpUtility.HtmlEncode(record.Name),
                                HttpUtility.HtmlEncode(record.Barcode),                                
                                HttpUtility.HtmlEncode(record.CategoryName),
                                HttpUtility.HtmlEncode(record.PurchasePrice),
                                HttpUtility.HtmlEncode(record.MRP),
                                HttpUtility.HtmlEncode(record.WholesalePrice),
                                HttpUtility.HtmlEncode(record.Stock),
                                HttpUtility.HtmlEncode(record.LowStock),
                                HttpUtility.HtmlEncode(record.DamageStock),
                                record.Id.ToString()
                            }).ToArray()
                };
                return Json(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "there was a problem getting products");
                return Json(DataTables.EmptyResult);
            }
        }
    }
}
