using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Task.Models.ViewModels;
using System.Diagnostics;
using Task.Models.Domain;
using Task.Data;
using Task.Controllers;

namespace Task.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public List<Currency> currencies = new List<Currency>();
        private readonly TaskDbContext dbContext;

        [BindProperty]
        public GetCurrencyViewModel getCurrencyViewModel { get; set; }
        private GetCurrencyController getCurrencyController { get; set; }
        public IndexModel(ILogger<IndexModel> logger, TaskDbContext dbContext)
        {
            _logger = logger;
            this.dbContext = dbContext;
            this.getCurrencyController = new GetCurrencyController(dbContext);
        }
        public void OnGet()
        {
        }
        public void OnPost()
        {
            if(getCurrencyViewModel.date1.ToString("dd/MM/yyyy") != "01.01.0001" &&
                getCurrencyViewModel.date2.ToString("dd/MM/yyyy") != "01.01.0001")
            {
                if (getCurrencyViewModel.date1 > getCurrencyViewModel.date2 ||
                    getCurrencyViewModel.date2 > DateTime.Now)
                    return;
                currencies = getCurrencyController.getCurrencies(getCurrencyViewModel);
            }
        }
    }
}