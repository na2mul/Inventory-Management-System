namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class AddBookModel
    {
        public string Title { get; set; }
        public Guid? AuthorId { get; set; }
        public DateTime? PublishDate { get; set; }
    }
}
