﻿using CarManagementSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarManagementSystem.Data.Models
{
    [Table("Model")]
    public class Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public Guid MO_Id { get; set; }

        [Required]
        [Column(TypeName = "varchar")]
        [StringLength(30)]
        [Display(Name = "Model Name")]
        public string MO_Name { get; set; }
        [Required]
        [Column(TypeName = "varchar(max)")]

        [Display(Name = "Discription")]
        public string MO_Discription { get; set; }

        [Required]
        [Column(TypeName = "varchar(max)")]

        [Display(Name = "Feature")]
        public string MO_Feature { get; set; }
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

        public Guid CR_Id { get; set; }
        [ForeignKey("CR_Id")]   
        public virtual Car Cars { get; set; }
        public ICollection<SubModel> subModels { get; set; }
        public ICollection<Images> Images { get; set; }





    }
}
