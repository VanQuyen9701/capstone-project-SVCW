﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SVCW.Models
{
    public partial class FollowJoinAvtivity
    {
        [Key]
        [Column("userId")]
        [StringLength(10)]
        public string UserId { get; set; }
        [Key]
        [Column("activityId")]
        [StringLength(10)]
        public string ActivityId { get; set; }
        [Column("isJoin")]
        [StringLength(50)]
        public string IsJoin { get; set; }
        [Column("isFollow")]
        public bool? IsFollow { get; set; }
        [Column("datetime", TypeName = "datetime")]
        public DateTime Datetime { get; set; }
        [Key]
        [Column("processId")]
        [StringLength(10)]
        public string ProcessId { get; set; }

        [ForeignKey("ActivityId")]
        [InverseProperty("FollowJoinAvtivity")]
        public virtual Activity Activity { get; set; }
        [ForeignKey("ProcessId")]
        [InverseProperty("FollowJoinAvtivity")]
        public virtual Process Process { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("FollowJoinAvtivity")]
        public virtual User User { get; set; }
    }
}