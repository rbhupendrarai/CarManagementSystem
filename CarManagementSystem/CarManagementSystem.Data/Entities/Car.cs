﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarManagementSystem.Data.Models
{
    [Table("Car")]
    public class Car
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CR_Id { get; set; }

        [Required]
        [Column(TypeName = "varchar")]
        [StringLength(30)]
        [Display(Name = "Car Name")]
        public string CR_Name { get; set; }


        [Required]

        [Column(TypeName = "varchar(max)")]
        [Display(Name = "Discription")]
        public string CR_Discription { get; set; }

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
        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }


        [Required]
        [Column(TypeName = "varchar")]
        [StringLength(30)]
        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }
        public ICollection<Model> models { get; set; }
        
          

    }
}