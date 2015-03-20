﻿using System;
using System.Security.Permissions;
namespace DniproFuture.Models
{
    public class NewsOutputModel
    {
        public string Title { get; set; }
        public string ShortText { get; set; }
        public string Photo { get; set; }
        public DateTime Date { get; set; }
    }
}