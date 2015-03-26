using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CBMS.Models.Roster;
using CBMS.Models.Inventory;

namespace CBMS.Models.ViewModels
{
    public class InventoryViewModel
    {
        public BrandModel Brand { get; set; }
        public OutletModel Outlet { get; set; }
        public IList<BrandModel> Brands { get; set; }
        public IList<OutletModel> Outlets { get; set; }

        public OutletItemCategoryModel NewOutletItemCategory { get; set; }
        public IList<OutletItemCategoryModel> OutletCategoryList { get; set; }
        public ItemModel NewItem { get; set; }
        public IList<ItemModel> ItemsOfOutlet { get; set; }
    }
}