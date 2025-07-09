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
using DevSkill.Inventory.Application.Features.MeasurementUnits.Queries;
using DevSkill.Inventory.Infrastructure.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IImageUtility _imageUtility;

        public ProductsController(
            ILogger<ProductsController> logger,
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
        public async Task<IActionResult> AddAsync(AddProductModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var product = _mapper.Map<ProductAddCommand>(model);
                    product.ImageUrl = await _imageUtility.UploadImage(model.Image, model.ImageUrl);
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
                    string message = "Failed to add product";
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

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAsync(UpdateProductModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var product = _mapper.Map<ProductUpdateCommand>(model);
                    product.ImageUrl = await _imageUtility.UploadImage(model.Image, model.ImageUrl);
                    await _mediator.Send(product);
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
                    string message = "Failed to update product";
                    _logger.LogError(ex, message);
                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = message,
                        Type = ResponseTypes.Danger

                    });
                }
            }
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> StoreAsync(StoreProductModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var product = _mapper.Map<ProductStoreCommand>(model);                    
                    await _mediator.Send(product);
                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = "product Stored",
                        Type = ResponseTypes.Success
                    });
                    return RedirectToAction("Index");
                }                
                catch (Exception ex)
                {
                    string message = "Failed to store product";
                    _logger.LogError(ex, message);
                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = message,
                        Type = ResponseTypes.Danger
                    });
                }
            }
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DamageAsync(DamageProductModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var product = _mapper.Map<ProductDamageCommand>(model);
                    await _mediator.Send(product);
                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = "damage product Stored",
                        Type = ResponseTypes.Success
                    });
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    string message = "Failed to store damage product";
                    _logger.LogError(ex, message);
                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = message,
                        Type = ResponseTypes.Danger
                    });
                }
            }
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAsync(Guid id)
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
        public  async Task<JsonResult> GetProductJsonDataAsync([FromBody] ProductGetListQuery model)
        {
            try
            {                
                var (data, total, totalDisplay) = await _mediator.Send(model);
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
        public async Task<JsonResult> GetProductForUpdateAsync(Guid id)
        {
            try
            {
                var product = await _mediator.Send(new ProductGetQuery() { Id = id });

                if (product == null)
                {
                    return Json(new { success = false, message = "Product not found" });
                }
                var model = _mapper.Map<UpdateProductModel>(product);

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        id = model.Id,
                        name = model.Name,
                        barcode = model.Barcode,
                        categoryId = model.CategoryId,
                        measurementUnitId = model.MeasurementUnitId,
                        purchasePrice = model.PurchasePrice,
                        mrp = model.MRP,
                        wholesalePrice = model.WholesalePrice,
                        stock = model.Stock,
                        lowStock = model.LowStock,
                        damageStock = model.DamageStock,
                        description = model.Description,
                        imageUrl = model.ImageUrl
                    }
                });
            }
            catch (Exception ex)
            {
                string error = "there was a problem getting products";
                _logger.LogError(ex, error);
                return Json(new { success = false,message = error});
            }
        }
        public async Task<IActionResult> GetCategoriesAsync()
        {
            try
            {
                var categoryList = await _mediator.Send(new CategoryGetQuery());
                return Json(categoryList);
            }
            catch (Exception ex)
            {
                string error = "there was a problem getting Categories";
                _logger.LogError(ex, error);
                return Json(new { success = false, message = error });
            }
        }
        public async Task<IActionResult> GetMeasurementUnitsAsync()
        {
            try
            {
                var MeasurementUnitList = await _mediator.Send(new MeasurementUnitGetQuery());
                return Json(MeasurementUnitList);
            }
            catch (Exception ex)
            {
                string error = "there was a problem getting Measurement units";
                _logger.LogError(ex, error);
                return Json(new { success = false, message = error });
            }
        }
        public async Task<IActionResult> GetProductsAsync()
        {
            try
            {
                var ProductsList = await _mediator.Send(new ProductGetAllQuery());
                return Json(ProductsList);
            }
            catch (Exception ex)
            {
                string error = "there was a problem getting products";
                _logger.LogError(ex, error);
                return Json(new { success = false, message = error });
            }
        }
    }
}      
