using _RegPage.Data;
using _RegPage.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace _RegPage.Pages.AllPages
{
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public Credentials Credentials{ get; set; }

        public EditModel(ApplicationDbContext db)
        {
            _db = db;
        }
       
        public void OnGet(string Email)
        {
            

            Credentials = _db.Credentials.Find(Email);
        }

        public async Task<IActionResult> OnPost()
        {
            if(Credentials.Password != Credentials.ConfPassword)
            {
                ModelState.AddModelError("Credentials.ConfPassword", "Confirm password should be same as password");
            }



            if (ModelState.IsValid)
            {
                _db.Credentials.Update(Credentials);
                await _db.SaveChangesAsync();
                return RedirectToPage("ViewDetails");
            }
            return Page();
        }
    }
}
