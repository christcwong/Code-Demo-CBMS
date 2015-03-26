using CBMS.Models.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CBMS.Models.Inventory
{
    public class ItemModel : CBMSModel
    {
        #region Attributes
        public string Serial { get; set; }
        public string Name { get; set; }

        [DisplayName("中文名稱")]
        public string ChinName { get; set; }

        //public string Specification { get; set; }

        //public double? VolumeH { get; set; }
        //public double? VolumeW { get; set; }
        //public double? VolumeL { get; set; }

        //[DisplayName("Gross Weight")]
        //public double? GrossWeight { get; set; }

        //public double? CBM { get; set; }
        #endregion

        #region Foreign Keys

        /// <summary>
        /// Each Item can only belong to one category
        /// </summary>
        public int? CategoryId { get; set; }
        public virtual ItemCategoryModel Category { get; set; }

        /// <summary>
        /// This field is used to M to N relationship for item to multiple outlets (outletItems)
        /// </summary>
        public virtual IList<OutletItemCategoryModel> OutletItemCategories { get; set; }

        [Display(Name = "Package Unit")]
        public int? PackageUnitId { get; set; }
        public virtual ConfigUnitModel PackageUnit { get; set; }

        //[Display(Name = "Volume Unit")]
        //public int? VolumeUnitId { get; set; }
        //public virtual ConfigUnitModel VolumeUnit { get; set; }

        //[Display(Name = "Gross Weight Unit")]
        //public int? GrossWeightUnitId { get; set; }
        //public virtual ConfigUnitModel GrossWeightUnit { get; set; }


        #endregion

        #region Getters
        [NotMapped]
        public string Label { get { return this.Serial + " " + this.Name; } }

        [NotMapped]
        public string MYOBLabel { get { return this.Name + " - " + this.ChinName; } }

        #endregion
    }
}