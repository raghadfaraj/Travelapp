using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TravelExpertsData
{
    public partial class Package
    {
        public Package()
        {
            Bookings = new HashSet<Booking>();
            PackagesProductsSuppliers = new HashSet<PackagesProductsSupplier>();
        }

        [Key]
        public int PackageId { get; set; }
        [StringLength(50)]
        [Display(Name = "Package Name")]
        public string PkgName { get; set; } = null!;
        [DisplayFormat(DataFormatString = "{0:MMMM dd, yyyy}")]
        [Display(Name = "Start Date")]
        [Column(TypeName = "datetime")]
        public DateTime? PkgStartDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:MMMM dd, yyyy}")]
        [Display(Name = "End Date")]
        [Column(TypeName = "datetime")]
        public DateTime? PkgEndDate { get; set; }
        [StringLength(50)]
        [Display(Name = "Description")]
        public string? PkgDesc { get; set; }
        [Column(TypeName = "money")]
        [Display(Name = "Package Price")]
        public decimal PkgBasePrice { get; set; }
        [Column(TypeName = "money")]
        [Display(Name = "Agency Commission")]
        public decimal? PkgAgencyCommission { get; set; }

        [InverseProperty("Package")]
        public virtual ICollection<Booking> Bookings { get; set; }
        [InverseProperty("Package")]
        public virtual ICollection<PackagesProductsSupplier> PackagesProductsSuppliers { get; set; }
    }
}
