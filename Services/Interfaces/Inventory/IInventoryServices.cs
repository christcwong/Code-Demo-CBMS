using CBMS.Models.Inventory;
using CBMS.Models.Roster;
using CBMS.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBMS.Services.Interfaces.Inventory
{
    public interface IInventoryServices : IServices
    {

        #region Functional Services

        /// <summary>
        /// Import a list of Item from a XLS file from MYOB.
        /// Please refer to the sample file "invctst1" at ClosedXMLSample\ClosedXMLSample\bin\Debug
        /// </summary>
        /// <param name="filePath">File path for uploaded XLS</param>
        /// <returns>Number of items inserted</returns>
        int ImportItem(string filePath);

        /// <summary>
        /// insert an item into a category. If item already at another category, it will be relocated to target category.
        /// </summary>
        /// <param name="item">item to be inserted</param>
        /// <param name="itemCategory">target category</param>
        /// <returns>item inserted</returns>
        ItemModel InsertItemToCategory(ItemModel item, ItemCategoryModel itemCategory);

        /// <summary>
        /// Remove an item from category
        /// </summary>
        /// <param name="item">Item to be removed</param>
        /// <param name="itemCategory">target Category</param>
        /// <returns>Item Removed</returns>
        ItemModel RemoveItemFromCategory(ItemModel item, ItemCategoryModel itemCategory);


        /// <summary>
        /// insert an item into a OutletItems. If item already at another category, it will be at both category at the same time.
        /// </summary>
        /// <param name="item">item to be inserted</param>
        /// <param name="OutletItemCategory">target OutletItemCategory</param>
        /// <returns>item inserted</returns>
        ItemModel InsertItemToOutletItemCategory(ItemModel item, OutletItemCategoryModel OutletItemCategory);

        /// <summary>
        /// Remove an item from OutletItems
        /// </summary>
        /// <param name="item">Item to be removed</param>
        /// <param name="OutletItemCategory">target OutletItemCategory</param>
        /// <returns>Item Removed</returns>
        ItemModel RemoveItemFromOutletItemCategory(ItemModel item, OutletItemCategoryModel OutletItemCategory);


        /// <summary>
        /// Prepare Transfer Order for specific outlet
        /// </summary>
        /// <param name="outlet"></param>
        /// <returns></returns>
        TransferOrderModel PrepareTransferOrder(OutletModel outlet);

        /// <summary>
        /// Prepare Transfer Order for specific outlet
        /// </summary>
        /// <param name="outlet"></param>
        /// <returns></returns>
        TransferOrderViewModel PrepareTransferOrderViewModel(OutletModel outlet);

         /// <summary>
        /// Prepare Transfer Order for specific outlet based on existing transferOrder
        /// </summary>
        /// <param name="outlet"></param>
        /// <returns></returns>
        TransferOrderViewModel PrepareTransferOrderViewModel(OutletModel outlet, TransferOrderModel transferOrder);

        /// <summary>
        /// Submit TransferOrder. Order Detail with zero request amount will be ignored.
        /// </summary>
        /// <param name="transferOrder">prepared transfer order to be submitted</param>
        /// <returns>Transfer Order Submitted</returns>
        TransferOrderModel SubmitTransferOrder(TransferOrderModel transferOrder);

        /// <summary>
        /// Copy outlet item category from outlet <b>source</b> to outlet <b>destination</b>.
        /// </summary>
        /// <param name="source">outlet to copy from</param>
        /// <param name="destination">outlet to copy to</param>
        /// <returns>List of Outlet Item Categories for destination outlet after copying</returns>
        List<OutletItemCategoryModel> CopyOutletItemCategory(OutletModel source, OutletModel destination);

        #endregion

        #region CRUD Services
        ItemModel InsertItem(ItemModel Item);
        List<ItemModel> GetItems();
        ItemModel GetItemById(int ItemId);
        ItemModel GetItemBySerial(string serial);
        ItemModel UpdateItem(ItemModel Item);
        ItemModel DeleteItem(int ItemId);
        ItemModel DeleteItem(ItemModel Item);

        ItemCategoryModel InsertItemCategory(ItemCategoryModel ItemCategoryModel);
        List<ItemCategoryModel> GetItemCategories();
        ItemCategoryModel GetItemCategoryById(int ItemCategoryId);
        ItemCategoryModel UpdateItemCategory(ItemCategoryModel ItemCategory);
        ItemCategoryModel DeleteItemCategory(int ItemCategoryId);
        ItemCategoryModel DeleteItemCategory(ItemCategoryModel ItemCategory);

        OutletItemCategoryModel InsertOutletItemCategory(OutletItemCategoryModel OutletItemCategory);
        List<OutletItemCategoryModel> GetOutletItemCategories();
        List<OutletItemCategoryModel> GetOutletItemCategories(OutletModel outlet);
        OutletItemCategoryModel GetOutletItemCategoryById(int OutletItemCategoryId);
        OutletItemCategoryModel UpdateOutletItemCategory(OutletItemCategoryModel OutletItemCategory);
        OutletItemCategoryModel DeleteOutletItemCategory(int OutletItemCategoryId);
        OutletItemCategoryModel DeleteOutletItemCategory(OutletItemCategoryModel OutletItemCategory);

        TransferOrderDetailModel InsertTransferOrderDetail(TransferOrderDetailModel TransferOrderDetailModel);
        List<TransferOrderDetailModel> GetTransferOrderDetails();
        List<TransferOrderDetailModel> GetTransferOrderDetails(TransferOrderModel TransferOrder);
        TransferOrderDetailModel GetTransferOrderDetailById(int TransferOrderDetailId);
        TransferOrderDetailModel UpdateTransferOrderDetail(TransferOrderDetailModel TransferOrderDetail);
        TransferOrderDetailModel DeleteTransferOrderDetail(int TransferOrderDetailId);
        TransferOrderDetailModel DeleteTransferOrderDetail(TransferOrderDetailModel TransferOrderDetail);

        TransferOrderModel InsertTransferOrder(TransferOrderModel TransferOrderModel);
        List<TransferOrderModel> GetTransferOrders();
        List<TransferOrderModel> GetTransferOrders(OutletModel outlet);
        TransferOrderModel GetTransferOrderById(int TransferOrderId);
        TransferOrderModel UpdateTransferOrder(TransferOrderModel TransferOrder);
        TransferOrderModel DeleteTransferOrder(int TransferOrderId);
        TransferOrderModel DeleteTransferOrder(TransferOrderModel TransferOrder);
        #endregion
    }
}
