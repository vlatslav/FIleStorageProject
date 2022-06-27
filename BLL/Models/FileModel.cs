using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BusinessLogicLayer.Models
{
    public class FileModel
    {
        [Key]
        public long FileId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string ContentType { get; set; }
        public string UserId { get; set; }
        public int CategoryId { get; set; } //mod
    }
}
