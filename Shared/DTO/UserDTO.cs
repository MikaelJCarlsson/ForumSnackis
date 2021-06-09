﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumSnackis.Shared.DTO
{
    public class UserDTO
    {
        public string UserName { get; set; }
        public int Posts { get; set; }
        public DateTime JoinDate { get; set; }
        public string ImagePath { get; set; }
    }
}