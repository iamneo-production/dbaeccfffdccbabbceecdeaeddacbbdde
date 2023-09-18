// Models/Player.cs
using System.ComponentModel.DataAnnotations;

namespace Week3MVC.Models
{

    public class Player
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Bidding amount is required.")]
        [Range(0.00, double.MaxValue, ErrorMessage = "Bidding amount must be greater than 0.")]
        public decimal BiddingAmount { get; set; }
    }
}
