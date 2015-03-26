using CBMS.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBMS.Models.ViewModels
{
    public class ItemViewModel
    {
        public List<ItemModel> Items { get; set; }
        public List<ItemCategoryModel> ItemCategories { get; set; }

        public ItemModel NewItem { get; set; }
        public ItemCategoryModel NewItemCategory { get; set; }
    }
}