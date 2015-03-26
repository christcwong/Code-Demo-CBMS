using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CBMS.Models.Roster
{
    public class StaffProfileModel : CBMSModel
    {
        #region Attributes


        [Display(Name = "Staff ID")]
        public string StaffId { get; set; }

        [Display(Name = "Roster Name")]
        public string RosterName { get; set; }

        /// <summary>
        /// Prefered Shift for staff is openeing
        /// </summary>
        public bool PreferOpeneingShift { get; set; }
        /// <summary>
        /// Prefered Shift for staff is Mid
        /// </summary>
        public bool PreferMidShift { get; set; }
        /// <summary>
        /// Prefered Shift for staff is Closing
        /// </summary>
        public bool PreferClosingShift { get; set; }

        // It is already at admin status!
        ///// <summary>
        ///// Date of Commence of Staff
        ///// </summary>
        //[DataType(DataType.Date)]
        //[Display(Name = "Start Date")]
        //public DateTime StartDate { get; set; }


        #endregion

        #region Foreign Keys

        public int? DepartmentId { get; set; }
        public virtual DepartmentModel Department { get; set; }

        public int? PositionId { get; set; }
        public virtual PositionModel Position { get; set; }

        public int? PayCodeId { get; set; }
        public virtual Config.ConfigPayCodeValueModel PayCode { get; set; }

        public int? BrandId { get; set; }
        public virtual BrandModel Brand { get; set; }

        //[ForeignKey("LeaveProfile")]
        public int? LeaveProfileId { get; set; }
        //[Required]
        [Display(Name = "Leave Profile")]
        public virtual StaffProfileLeaveProfileModel LeaveProfile { get; set; }

        public int? AdminStatusId { get; set; }
        [Display(Name = "Office Use")]
        public virtual StaffProfileAdminStatusModel AdminStatus { get; set; }

        public int? PaymentDetailId { get; set; }
        [Display(Name = "Payment Detail")]
        public virtual StaffProfilePaymentDetailModel PaymentDetail { get; set; }

        public int? StaffInfoId { get; set; }
        [Display(Name = "Personal Information")]
        public virtual StaffInfoModel StaffInfo { get; set; }



        #endregion


    }
}