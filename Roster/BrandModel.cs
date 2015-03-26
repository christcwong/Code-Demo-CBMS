using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CBMS.Models.Roster
{
    public class BrandModel : CBMSModel
    {
        #region Attributes

        /// <summary>
        /// Brand Name
        /// </summary>
        [Required]
        public string Name { get; set; }

        #endregion

        #region Foreign Keys

        // Removed
        //public virtual ICollection<PositionModel> Positions { get; set; }

        public virtual ICollection<StaffProfileModel> Staff { get; set; }

        public virtual ICollection<OutletModel> Outlets { get; set; }

        #endregion


    }
}