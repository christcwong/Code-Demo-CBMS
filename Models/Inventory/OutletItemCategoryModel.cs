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
    public class OutletItemCategoryModel : CBMSModel
    {
        #region Attributes

        [Display(Name = "Category Name")]
        public string Name { get; set; }

        #endregion

        #region Foreign Keys

        public int OutletId { get; set; }
        public virtual Roster.OutletModel Outlet { get; set; }

        // Not used
        ////public int ItemCategoryId { get; set; }
        //public virtual ICollection<ItemCategoryModel> ItemCategories { get; set; }

        //public int ItemId { get; set; }
        public virtual IList<ItemModel> Items { get; set; }

        #endregion
    }
}