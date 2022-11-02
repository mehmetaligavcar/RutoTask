using Microsoft.EntityFrameworkCore;
using System.Net;
using Task.Models.Domain;

namespace Task.Models.ViewModels
{
    public class GetCurrencyViewModel
    {
        public DateTime date1 { get; set; }
        public DateTime date2 { get; set; }
    }
}
