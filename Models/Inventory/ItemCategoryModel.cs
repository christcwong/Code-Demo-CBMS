using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CBMS.Models.Inventory
{
    public class ItemCategoryModel : CBMSModel
    {
        #region Attributes

        public string Name { get; set; }

        #endregion

        #region Foreign Keys

        public IList<ItemModel> Items { get; set; }

        #endregion
    }
}