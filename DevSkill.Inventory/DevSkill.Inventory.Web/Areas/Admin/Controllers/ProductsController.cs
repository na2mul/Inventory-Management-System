using AutoMapper;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Application.Exceptions;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Services;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Web;
using DevSkill.Inventory.Infrastructure;
using MediatR;
using DevSkill.Inventory.Application.Features.Products.Commands;
using DevSkill.Inventory.Application.Features.Products.Queries;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public ProductsController(ILogger<ProductsController> logger, IMapper mapper, IMediator mediator)
        {
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Add()
        {
            var model = new ProductAddCommand();
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ProductAddCommand model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Id = IdentityGenerator.NewSequentialGuid();
                    await _mediator.Send(model);
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
        public async Task<JsonResult> GetProductJsonData([FromBody] ProductGetListQuery productQuery)
        {
            try
            {

                var (data, total, totalDisplay) = await _mediator.Send(productQuery);
                var products = new
                {
                    recordstotal = total,
                    recordsFiltred = totalDisplay,
                    data = (from record in data
                            select new string[]
                            {
                                HttpUtility.HtmlEncode(record.Name),
                                record.Price.ToString(),
                                record.StockQuantity.ToString(),
                                HttpUtility.HtmlEncode(record.Description),
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
