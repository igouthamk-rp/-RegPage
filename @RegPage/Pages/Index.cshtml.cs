using _RegPage.Data;
using _RegPage.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace _RegPage.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }


        [BindProperty]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public Credentials Credentials { get; set; }



        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {

            String findEmail = _db.Credentials.FirstOrDefault(x => x.Email == Email)?.Email ?? "";
            String findPassword = _db.Credentials.Where(x => x.Email == findEmail).FirstOrDefault()?.Password ?? "";
            if (findEmail == "" || findPassword == "")
            {

                ModelState.AddModelError(findEmail, "Email not found, Create new account");

            }
            else if (Email != findEmail || Password != findPassword)
            {
                ModelState.AddModelError(findPassword, "Wrong Credentials");

            }
            if (!ModelState.IsValid)
            {
                return Page();
            }
            TempData["UserName"] = _db.Credentials.FirstOrDefault(x => x.Email == Email)?.FullName ?? "";

            return RedirectToPage("/AllPages/Content");
        }
    }
}