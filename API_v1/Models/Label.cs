using System;
namespace SFF.Models
{
    public class Label
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public string City { get; set; }
        public DateTime Date { get; set; }
    }
}