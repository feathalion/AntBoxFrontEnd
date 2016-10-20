﻿using AntBoxFrontEnd.Entities;
using AntBoxFrontEnd.Services.Boxes;
using AntBoxFrontEnd.Services.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AntBoxFrontEnd.Models
{
    public class BoxModel
    {

        public PaginationBoxesResponse Boxes { get; set; }

        public List<UserResponse> Users { get; set; }

    }
}