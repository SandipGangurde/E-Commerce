﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCarrier.ViewModels
{
    public class CategoriesVM
    {
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public bool IsActive { get; set; }
    }
}
