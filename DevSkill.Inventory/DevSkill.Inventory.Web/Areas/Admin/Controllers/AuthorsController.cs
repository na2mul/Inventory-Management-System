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

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthorsController : Controller
    {
        private readonly ILogger<AuthorsController> _logger;
        private readonly IAuthorService _authorService;
        private readonly IMapper _mapper;
        public AuthorsController(ILogger<AuthorsController> logger, IAuthorService authorService, IMapper mapper)
        {
            _logger = logger;
            _authorService = authorService;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Add()
        {
            var model = new AddAuthorModel();
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Add(AddAuthorModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var author = _mapper.Map<Author>(model);
                    author.Id = IdentityGenerator.NewSequentialGuid();
                    _authorService.AddAuthor(author);
                    TempData.Put("ResponseMessage", new ResponseModel()
                    {
                        Message = "Author Added",
                        Type = ResponseTypes.Success
                    });
                }
                catch(DuplicateAuthorNameException de)
                {
                    ModelState.AddModelError("DuplicateAuthor", de.Message);
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "Failed to add author");
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult GetAuthorJsonData([FromBody] AuthorListModel model)
        {
            try
            {
                var (data, total, totalDisplay) = _authorService.GetAuthors(model.PageIndex, model.PageSize, 
                    model.FormatSortExpression("Name","Biography","Rating", "Id"), model.Search);
                var authors = new
                {
                    recordstotal = total,
                    recordsFiltred = totalDisplay,
                    data = (from record in data
                            select new string[]
                            {
                                HttpUtility.HtmlEncode(record.Name),
                                HttpUtility.HtmlEncode(record.Biography),
                                record.Rating.ToString(),
                                record.Id.ToString()
                            }).ToArray()
                };
                return Json(authors);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "there was a problem getting authors");
                return Json(DataTables.EmptyResult);
            }

        }        
    }
}
