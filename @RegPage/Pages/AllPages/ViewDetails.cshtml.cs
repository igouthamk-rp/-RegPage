using _RegPage.Data;
using _RegPage.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClosedXML.Excel;
using System.IO;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Runtime.InteropServices;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;

namespace _RegPage.Pages.AllPages
{
    [BindProperties]
    public class ViewDetailsModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        private readonly IConfiguration _config;

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        [BindProperty]

        public string nameSort { get; set; }

        [BindProperty]

        public string dateSort { get; set; }

        [BindProperty]

        public string genSort { get; set; }

        [BindProperty]

        public string modSort { get; set; }

        public PaginatedList<Credentials> Credentials { get; set; }

        public ViewDetailsModel(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _config = configuration;
        }
        
        public int pn { get; set; }
        public async Task OnGetAsync(string sortOrder, int? pageIndex)
        {

            IQueryable<Credentials> Cred = from s in _db.Credentials select s;
           

            if (!string.IsNullOrEmpty(SearchString))
            {
                Cred = Cred.Where(x => x.FullName.ToLower().Contains(SearchString.ToLower()) || x.Email.ToLower().Contains(SearchString.ToLower()) || x.MobileNumber.Contains(SearchString));
            }
            if (SearchString != null)
            {
                pageIndex = 1;
                pn = 1;

            }

            if(!Cred.Any())
            {
                TempData["ErrorMessage"] = "User not found!";
            }


            nameSort = sortOrder == "name_asec" ? "name_desc" : "name_asec";
            dateSort = sortOrder == "date_asec" ? "date_desc" : "date_asec";
            genSort = sortOrder == "Female" ? "Male" : "Female";
            modSort = sortOrder == "m_asec" ? "m_desc" : "m_asec";
            

            switch (sortOrder)
            {
                case "name_asec":
                    Cred = Cred.OrderBy(s => s.FullName);
                    break;
                case "name_desc":
                    Cred = Cred.OrderByDescending(s => s.FullName);
                    break;
                case "date_desc":
                    Cred = Cred.OrderByDescending(s => s.DOB);
                    break;
                case "date_asec":
                    Cred = Cred.OrderBy(s => s.DOB);
                    break;
                case "Male":
                    Cred = Cred.Where(s => s.Gender.Contains("Male"));
                    break;
                case "Female":
                    Cred = Cred.Where(s => s.Gender.Contains("Female"));
                    break;
                case "m_asec":
                    Cred = Cred.OrderBy(s => s.ModDate);
                    break;
                case "m_desc":
                    Cred = Cred.OrderByDescending(s => s.ModDate);
                    break;
                default:
                    Cred = Cred.OrderBy(s => s.Email);
                    break;
            }
            
            var pageSize = _config.GetValue("PageSize", 4);
            Credentials = await PaginatedList<Credentials>.CreateAsync(Cred, pageIndex ?? 1, pageSize);
            
            
        }

        public FileResult OnPostExport()
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[7] { new DataColumn("Email"), new DataColumn("FullName"),new DataColumn("Mobile Number"),new DataColumn("Salutation"), new DataColumn("Gender"), new DataColumn("DOB"), new DataColumn("Password") });

            var expData = from exp in this._db.Credentials.Take(20) select exp;
           

            foreach(var exp  in expData)
            {
                dt.Rows.Add(exp.Email, exp.FullName, exp.MobileNumber, exp.Salutation, exp.Gender, exp.DOB, exp.Password);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                }
            }
        }

        
    }
}
