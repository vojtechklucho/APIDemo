using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace APIDemo.Models
{
    public class Quote
    {
        [Key]
        public int Id { get; set; }

        public string Text { get; set; }

        [JsonIgnore]
        public ICollection<QuoteTag> QuoteTags { get; set; }

    }
}
