using CBMS.Models.Inventory;
using CBMS.Models.Roster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBMS.Models.ViewModels
{
    public class TransferOrderViewModel
    {
        public OutletModel Outlet { get; set; }
        public TransferOrderModel TransferOrder { get; set; }
        public List<OutletItemCategoryModel> OutletItemCategories { get; set; }
        public Dictionary<OutletItemCategoryModel, List<TransferOrderDetailModel>> detailsForCategory { get; set; }

        public List<TransferOrderModel> RecentTransferOrders { get; set; }


        public int OutletId { get; set; }
    }
}