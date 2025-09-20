using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceInventory.Application.DTOs.Category
{
    public class CategoryUpdateDto
    {
        public string Name { get; set; } = "";
        public string? Description { get; set; }
    }

}
