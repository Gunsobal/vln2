﻿using CodeKingdom.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeKingdom.Models.ViewModels
{
    public class ProjectViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public List<Collaborator> Collaborators { get; set; }
    }
}