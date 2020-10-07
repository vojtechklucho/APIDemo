using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace APIDemo.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public Category Category { get; set; }

        [JsonIgnore]
        public ICollection<QuoteTag> QuoteTags {get;set;}
    }

    public enum Category
    {
        Author,
        Genre,
        Other
    }

}
