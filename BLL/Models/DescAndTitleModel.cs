using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace BusinessLogicLayer.Models
{
    public class DescAndTitleModel
    {
        public int FileId { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
    }
}
