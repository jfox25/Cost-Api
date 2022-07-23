using System;

namespace Api.Dto
{
    public class AddBusinessDto 
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}