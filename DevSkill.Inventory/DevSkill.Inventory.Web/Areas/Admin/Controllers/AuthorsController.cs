using Demo.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Services;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Web;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthorsController : Controller
    {
        private readonly ILogger<AuthorsController> _logger;
        private readonly IAuthorService _authorService;
        public AuthorsController(ILogger<AuthorsController> logger, IAuthorService authorService)
        {
            _logger = logger;
            _authorService = authorService;
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
                _authorService.AddAuthor(new Author { Name = model.Name, Biography = model.Biography, Rating = model.Rating});
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult GetAuthorJsonData([FromBody] AuthorListModel model)
        {
            try
            {
                var (data, total, totalDisplay) = _authorService.GetAuthors(model.PageIndex, model.PageSize, model.FormatSortExpression("Name","Biography","Rating", "Id"), model.Search);
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
