using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace QuoteWall.Models
{
    public class User: IdentityUser
    {
        public int? QuoteId { get; set; }
        public Quote? Quotes { get; set; }
    }
}
