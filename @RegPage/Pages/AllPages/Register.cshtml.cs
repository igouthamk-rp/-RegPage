 using _RegPage.Data;
using _RegPage.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _RegPage.Pages.AllPages
{
    [BindProperties]
    public class RegisterModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public Credentials Credentials { get; set; }

        public RegisterModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public void OnGet()
        {


        }
        public async Task<IActionResult> OnPost(Credentials Credentials)
        {
            if (Credentials.IsChecked == false)
            {
                ModelState.AddModelError("Credentials.IsChecked", "Mark this feild");

            }

            string isFound = _db.Credentials.FirstOrDefault(x=> x.Email == Credentials.Email)?.Email??"";

            if (isFound != "") {
                ModelState.AddModelError("Credentials.Email", "User already exists, Login to your account");
            }

            if(Credentials.Password != Credentials.ConfPassword)
            {
                ModelState.AddModelError("Credentials.ConfPassword", "Confirm password should be same as Password");
            }
            if (ModelState.IsValid)
            {
                await _db.Credentials.AddAsync(Credentials);
                await _db.SaveChangesAsync();
                return RedirectToPage("/AllPages/Login");
            }
            else
            {
                return Page();
            }
        }
    }
}
