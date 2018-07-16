﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinphoneXamarin.components
{

    public class navBarMenuItem
    {
        public navBarMenuItem()
        {
            TargetType = typeof(navBarDetail);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}