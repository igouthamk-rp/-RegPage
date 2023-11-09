using _RegPage.Data;
using _RegPage.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _RegPage.Pages.AllPages
{
    [BindProperties]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public Credentials Credentials { get; set; }

        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public void OnGet(string Email)
        {


            Credentials = _db.Credentials.Find(Email);
        }

        public async Task<IActionResult> OnPost()
        {
             _db.Credentials.Remove(Credentials);
             await _db.SaveChangesAsync();
             return RedirectToPage("ViewDetails");
        }
        
    }
}
