using System.ComponentModel.DataAnnotations;

namespace WordsToNumbers.Web.Models
{
    public class WordsAndNumbers
    {
        [Required]
        public string Words { get; set; }

        public string Numbers { get; set; }
    }
}
