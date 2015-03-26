using CBMS.Models.Roster;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CBMS.Models.Payroll
{
    public class PayrollProfileModel : CBMSModel
    {
        #region Attributes


        [Display(Name = "Lower Bound for Salary")]
        public double SalaryLowerBound { get; set; }

        [Display(Name = "Upper Bound for Salary")]
        public double SalaryUpperBound { get; set; }

        [Display(Name="Valid Until")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ValidUntil { get; set; }

        #endregion

        #region Foreign Keys


        /// <summary>
        /// Mandatory attribute : Outlet for this record
        /// </summary>
        public int OutletId { get; set; }
        public virtual OutletModel Outlet { get; set; }

        /// <summary>
        /// Optional attribute : Department for this record
        /// </summary>
        public int? DepartmentId { get; set; }
        public virtual DepartmentModel Department { get; set; }


        /// <summary>
        /// Optional attribute : Brand for this record
        /// </summary>
        public int? BrandId { get; set; }
        public virtual BrandModel Brand { get; set; }

        #endregion
    }
}