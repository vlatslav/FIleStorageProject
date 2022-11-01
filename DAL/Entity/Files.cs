using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using BAL.Entity.Auth;

namespace BAL.Entity
{
    public class Files
    {
        [Key]
        [Column(TypeName = "bigint")]
        public long FileId { get; set; }
        [Column(TypeName = "varchar(max)")]
        public string FilePath { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Date { get; set; }
        [Column(TypeName = "varchar(max)")]
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; } 
        public string Title { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
