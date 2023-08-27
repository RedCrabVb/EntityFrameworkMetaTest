using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFrameworkMetaTest.Model
{
    [Table("meta_table")]
    public class MetaTableModel
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("extid")]
        public string ExtId { get; set; }
        [Column("json_text")]
        public string JsonText { get; set; }
    }
}
