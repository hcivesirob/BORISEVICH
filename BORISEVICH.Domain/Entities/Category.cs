﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORISEVICH.Domain.Entities
{
    public class Category:Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
    }
}
