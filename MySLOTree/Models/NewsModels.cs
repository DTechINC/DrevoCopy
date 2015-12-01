using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace MySLOTree.Models
{
    public class TreeContext : DbContext
    {
        public TreeContext()
            : base("TreesContext")
        {
        }

        public DbSet<Folders> Folders { get; set; }
    }

    [Table("Folders")]
    public class Folders //модель бд
    {
        public Folders()
        {
            IsDeleted = false;
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class FoldersModel//модель ветви дерева
    {
        internal FoldersModel(Folders folders)
        {
            this.Id = folders.Id;
            this.ParentId = folders.ParentId;
            this.Title = folders.Title;
        }

        public int Id {get; set; }
        public int? ParentId {get; set; }
        public string Title {get;set;}
    }

    public class FoldersListModel//модель списка ветвей
    {
        public int? Seed { get; set; }
        public IEnumerable<FoldersModel> Folders { get; set; }
    }

    
}
