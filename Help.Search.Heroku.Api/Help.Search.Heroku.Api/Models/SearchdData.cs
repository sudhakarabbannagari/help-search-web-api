using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Help.Search.Heroku.Api.Models
{

    public class SearchdData
    {
        public string? Name { get; set; }
        [Key]
        public int BroadbandId { get; set; }
        public string? Speed { get; set; }
        public string? Provider { get; set; }
        public string? Price { get; set; }
    }
}
