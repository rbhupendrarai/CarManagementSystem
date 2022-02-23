using CarManagementSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarManagementSystem.Data.Models
{
    [Table("Images")]
    public class Images
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Img_Id { get; set; }


        [Required]   
        [Display(Name = "Img")]
        public byte[] Img { get; set; }

       


        [Required]
        [Column(TypeName = "DateTime")]
        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        [Required]
        [Column(TypeName = "varchar")]
        [StringLength(30)]
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Required]
        [Column(TypeName = "DateTime")]
        [StringLength(30)]
        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [Required]
        [Column(TypeName = "varchar")]
        [StringLength(30)]
        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }


        public Guid MO_Id { get; set; }
        [ForeignKey("MO_Id")]
        public virtual Model Model{ get; set; }


    }
}