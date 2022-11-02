using System.ComponentModel.DataAnnotations;

namespace Task.Models.Domain
{
    public class Currency
    {
        [Key]
        public DateTime date { get; set; }
        public Double value { get; set; }
        public Currency(DateTime date, double value)
        {
            this.date = date;
            this.value = value;
        }
    }
}
