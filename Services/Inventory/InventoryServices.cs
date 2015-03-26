using CBMS.Models;
using CBMS.Models.Config;
using CBMS.Models.Inventory;
using CBMS.Models.Roster;
using CBMS.Repositories.Interfaces.Config;
using CBMS.Repositories.Interfaces.Inventory;
using CBMS.Repositories.Interfaces.Roster;
using CBMS.Services.Interfaces.Inventory;
using CBMS.Utilities;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Data.Entity;
using CBMS.Models.ViewModels;

namespace CBMS.Services.Inventory
{
    public class InventoryServices : CBMSServices, IInventoryServices
    {
        #region ctor

        private string ItemRepositoryName = WebConfigurationManager.AppSettings["CBMS.REPOSITORIES.INVENTORY.ITEM"];
        private string ItemCategoryRepositoryName = WebConfigurationManager.AppSettings["CBMS.REPOSITORIES.INVENTORY.ITEMCATEGORY"];
        private string OutletItemCategoryRepositoryName = WebConfigurationManager.AppSettings["CBMS.REPOSITORIES.INVENTORY.OUTLETITEMCATEGORY"];
        private string TransferOrderDetailRepositoryName = WebConfigurationManager.AppSettings["CBMS.REPOSITORIES.INVENTORY.TRANSFERORDERDETAIL"];
        private string TransferOrderRepositoryName = WebConfigurationManager.AppSettings["CBMS.REPOSITORIES.INVENTORY.TRANSFERORDER"];
        private string ConfigUnitRepositoryName = WebConfigurationManager.AppSettings["CBMS.REPOSITORIES.CONFIG.CONFIGUNIT"];
        private string OutletRepositoryName = WebConfigurationManager.AppSettings["CBMS.REPOSITORIES.ROSTER.OUTLET"];

        private IConfigUnitRepository _ConfigUnitRepository;
        private ITransferOrderRepository _TransferOrderRepository;
        private ITransferOrderDetailRepository _TransferOrderDetailRepository;
        private IOutletItemCategoryRepository _OutletItemCategoryRepository;
        private IItemCategoryRepository _ItemCategoryRepository;
        private IItemRepository _ItemRepository;
        private IOutletRepository _outletRepository;


        public InventoryServices(ModelStateDictionary modelStateDictionary)
            : this(modelStateDictionary, new CBMSDbContext())
        {
        }
        public InventoryServices(ModelStateDictionary modelStateDictionary, CBMSDbContext dbContext)
        {
            this._modelStateDictionary = modelStateDictionary;
            this.dbContext = dbContext;
            this._ItemRepository = (IItemRepository)Activator.CreateInstance(Type.GetType(ItemRepositoryName), dbContext);
            this._ItemCategoryRepository = (IItemCategoryRepository)Activator.CreateInstance(Type.GetType(ItemCategoryRepositoryName), dbContext);
            this._OutletItemCategoryRepository = (IOutletItemCategoryRepository)Activator.CreateInstance(Type.GetType(OutletItemCategoryRepositoryName), dbContext);
            this._TransferOrderDetailRepository = (ITransferOrderDetailRepository)Activator.CreateInstance(Type.GetType(TransferOrderDetailRepositoryName), dbContext);
            this._TransferOrderRepository = (ITransferOrderRepository)Activator.CreateInstance(Type.GetType(TransferOrderRepositoryName), dbContext);
            this._ConfigUnitRepository = (IConfigUnitRepository)Activator.CreateInstance(Type.GetType(ConfigUnitRepositoryName), dbContext);
            this._outletRepository = (IOutletRepository)Activator.CreateInstance(Type.GetType(OutletRepositoryName), dbContext);
        }
        #endregion

        #region Functional Services

        /// <summary>
        /// Import a list of Item from a XLS file from MYOB.
        /// Please refer to the sample file "invctst1" at ClosedXMLSample\ClosedXMLSample\bin\Debug
        /// </summary>
        /// <param name="filePath">File path for uploaded XLS</param>
        /// <returns>Number of items inserted</returns>
        public int ImportItem(string filePath)
        {
            // TO DO:
            // 1) read in items from file
            var wb = new XLWorkbook(filePath);
            if (wb == null)
            {
                throw new ArgumentException("Path name invalid.", "filePath");
            }
            // 2) get data
            DataSet ds = new DataSet();
            foreach (var ws in wb.Worksheets)
            {
                DataTable dt = new DataTable(ws.Name);
                var rows = ws.RowsUsed().Where(row => row.CellsUsed().Count() >= 4);
                var firstRow = rows.First();
                foreach (var cell in firstRow.CellsUsed())
                {
                    dt.Columns.Add(cell.GetString());
                }

                foreach (var row in rows.Skip(1))
                {
                    var cells = row.CellsUsed();
                    var dr = dt.NewRow();
                    dr.ItemArray = cells.Select(cell => cell.RichText.Text.Normalize()).ToArray();
                    //var dr = new DataRow();
                    dt.Rows.Add(dr);
                }
                // data massage
                foreach (var row in dt.Rows)
                {
                    // i) 0_x000D_
                    var cell = ((DataRow)row)["On Hand"];
                    if (cell.ToString() == "0_x000D_")
                    {
                        ((DataRow)row)["On Hand"] = 0;
                    }
                    // ii) Units. To Upper
                    var cell2 = ((DataRow)row)["Units"];
                    ((DataRow)row)["Units"] = ((DataRow)row)["Units"].ToString().ToUpper();
                }
                //dt.AcceptChanges();
                ds.Tables.Add(dt);
                ds.AcceptChanges();
            }

            //throw new NotImplementedException();

            var dbItems = this.GetItems();



            // 2a) exists : update it
            int updateCount = 0;


            foreach (DataTable tbl in ds.Tables)
            {
                var ItemRequiredUpdate = dbItems.Join(tbl.AsEnumerable(),
                    dbItem => dbItem.Serial,
                    tblRow => tblRow.Field<string>("Item No."),
                    (dbItem, tblRow) =>
                    new { dbItem = dbItem, dsItem = tblRow }
                    ).ToList();

                updateCount += ItemRequiredUpdate.Count();
                foreach (var updatingItem in ItemRequiredUpdate)
                {
                    var dbItem = updatingItem.dbItem;
                    var NameSplited = updatingItem.dsItem.Field<string>("Item Name").Split('-');
                    dbItem.Name = NameSplited.Count() > 0 ? NameSplited[0].Trim() : "";
                    dbItem.ChinName = NameSplited.Count() > 1 ? NameSplited[1].Trim() : "";
                    this.UpdateItemPackagingUnit(dbItem, updatingItem.dsItem.Field<string>("Units"));
                    this.UpdateItem(dbItem);
                }
            }

            // 2b) not exists : create new item
            int addCount = 0;

            foreach (DataTable tbl in ds.Tables)
            {
                var dbSerials = dbItems.Select(i => i.Serial).ToList();
                var ItemRequiredInsert =
                    tbl.AsEnumerable().Where(dr => !dbSerials.Contains(dr.Field<string>("Item No.")));

                addCount += ItemRequiredInsert.Count();
                foreach (var insertingItem in ItemRequiredInsert)
                {

                    var NameSplited = insertingItem.Field<string>("Item Name").Split('-');
                    var dbItem = new ItemModel()
                    {
                        Name = NameSplited.Count() > 0 ? NameSplited[0].Trim() : "",
                        ChinName = NameSplited.Count() > 1 ? NameSplited[1].Trim() : "",
                        Serial = insertingItem.Field<string>("Item No.")
                    };
                    this.UpdateItemPackagingUnit(dbItem, insertingItem.Field<string>("Units"));
                    this.InsertItem(dbItem);
                }
            }
            // 2c) mark serial no longer there as deleted.
            int deleteCount = 0;

            // ignoring this delete unless confirmed necessary.

            var resultCount = updateCount + addCount + deleteCount;

            return resultCount;
        }


        /// <summary>
        /// insert an item into a category
        /// </summary>
        /// <param name="item">item to be inserted</param>
        /// <param name="itemCategory">target category</param>
        /// <returns>item inserted</returns>
        public ItemModel InsertItemToCategory(ItemModel item, ItemCategoryModel itemCategory)
        {
            if (itemCategory.Items.Contains(item))
            {
                return item;
            }
            //insert into category if not already inside
            item.CategoryId = itemCategory.Id;
            this._ItemRepository.UpdateItem(item);
            this._ItemCategoryRepository.UpdateItemCategory(itemCategory);
            return item;
        }

        /// <summary>
        /// Remove an item from category
        /// </summary>
        /// <param name="item">Item to be removed</param>
        /// <param name="itemCategory">target Category</param>
        /// <returns>Item Removed</returns>
        public ItemModel RemoveItemFromCategory(ItemModel item, ItemCategoryModel itemCategory)
        {
            if (!itemCategory.Items.Contains(item))
            {
                return item;
            }
            //insert into category if not already inside
            itemCategory.Items.Remove(item);
            this._ItemCategoryRepository.UpdateItemCategory(itemCategory);
            this._ItemRepository.UpdateItem(item);
            return item;
        }

        /// <summary>
        /// insert an item into a OutletItems
        /// </summary>
        /// <param name="item">item to be inserted</param>
        /// <param name="OutletItemCategory">target OutletItems</param>
        /// <returns>item inserted</returns>
        public ItemModel InsertItemToOutletItemCategory(ItemModel item, OutletItemCategoryModel OutletItemCategory)
        {
            if (OutletItemCategory.Items.Contains(item))
            {
                return item;
            }
            //insert into OutletItems if not already inside
            OutletItemCategory.Items.Add(item);
            item.OutletItemCategories.Add(OutletItemCategory);
            this._OutletItemCategoryRepository.UpdateOutletItemCategory(OutletItemCategory);
            // seems for M to N, only one side require the update.
            // extra update will remove the item instead...
            //this._ItemRepository.UpdateItem(item);
            return item;
        }

        /// <summary>
        /// Remove an item from OutletItems
        /// </summary>
        /// <param name="item">Item to be removed</param>
        /// <param name="OutletItemCategory">target OutletItems</param>
        /// <returns>Item Removed</returns>
        public ItemModel RemoveItemFromOutletItemCategory(ItemModel item, OutletItemCategoryModel OutletItemCategory)
        {
            if (!OutletItemCategory.Items.Contains(item))
            {
                return item;
            }
            //insert into OutletItems if not already inside
            OutletItemCategory.Items.Remove(item);
            item.OutletItemCategories.Remove(OutletItemCategory);
            this._OutletItemCategoryRepository.UpdateOutletItemCategory(OutletItemCategory);
            //this._ItemRepository.UpdateItem(item);
            return item;
        }


        /// <summary>
        /// Prepare Transfer Order for specific outlet
        /// </summary>
        /// <param name="outlet"></param>
        /// <returns></returns>
        public TransferOrderModel PrepareTransferOrder(OutletModel outlet)
        {
            if (outlet == null)
            {
                return new TransferOrderModel();
            }

            var result = new TransferOrderModel()
                {
                    // These two field are actually same as destination field.
                    SourceLocationId = outlet.LocationId.Value,
                    SourceLocation = outlet.Location,
                    DestinationLocationId = outlet.LocationId.Value,
                    DestinationLocation = outlet.Location,
                    Details = new List<TransferOrderDetailModel>(),
                    OrderStatus = OrderStatusEnum.ACTIVE,
                    OrderStatusValue = (int)OrderStatusEnum.ACTIVE,
                    IssueDate = DateTimeWrapper.Now,
                    EffectiveDate = DateTimeWrapper.Now,
                };
            // look for outletitems
            var outletItemCategories = this.GetOutletItemCategories(outlet);
            if (!outletItemCategories.Any())
            {
                // pour all items if not configurated
                foreach (var item in this.GetItems())
                {
                    result.Details.Add(new TransferOrderDetailModel()
                     {
                         Item = item,
                         ItemId = item.Id,
                         ParentOrder = result,
                         QuantityRequested = 0,
                         QuantityChange = 0,
                         OrderDetailStatus = OrderDetailStatusEnum.PENDING,
                         OrderDetailStatusValue = (int)OrderDetailStatusEnum.PENDING,
                     });
                }
                return result;
            }

            foreach (var category in outletItemCategories)
            {
                foreach (var item in category.Items)
                {
                    result.Details.Add(new TransferOrderDetailModel()
                    {
                        Item = item,
                        ItemId = item.Id,
                        ParentOrder = result,
                        QuantityRequested = 0,
                        QuantityChange = 0,
                        OrderDetailStatus = OrderDetailStatusEnum.PENDING,
                        OrderDetailStatusValue = (int)OrderDetailStatusEnum.PENDING,
                    });
                }
            }

            return result;


        }

        /// <summary>
        /// Prepare Transfer Order for specific outlet
        /// </summary>
        /// <param name="outlet"></param>
        /// <returns></returns>
        public TransferOrderViewModel PrepareTransferOrderViewModel(OutletModel outlet)
        {
            TransferOrderModel order = PrepareTransferOrder(outlet);
            var outletItemCategorys = this.GetOutletItemCategories(outlet);
            //var detailsForCategory = new Dictionary<OutletItemCategoryModel, List<TransferOrderDetailModel>>();
            //foreach (var cat in outletItemCategorys)
            //{
            //    var categoryItemIds = cat.Items.Select(i => i.Id).Distinct();
            //    detailsForCategory.Add(cat,
            //        order.Details.Where(
            //            d =>
            //                categoryItemIds.Contains(d.ItemId)
            //        ).ToList()
            //    );
            //}

            //Linq method:
            var detailsForCategory = outletItemCategorys
                // it is just a table join in memory space
                .Select(cat =>
                    new
                    {
                        category = cat,
                        details = order.Details.Where(d => cat.Items.Select(i => i.Id).Contains(d.ItemId)).ToList()
                    })
                .ToDictionary(x => x.category, x => x.details);

            var recentHistories = this.GetTransferOrders(outlet).OrderByDescending(o => o.IssueDate).Take(3).ToList();

            TransferOrderViewModel vm = new TransferOrderViewModel()
            {
                TransferOrder = order,
                Outlet = outlet,
                OutletId = outlet.Id,
                OutletItemCategories = outletItemCategorys,
                detailsForCategory = detailsForCategory,
                RecentTransferOrders = recentHistories,
            };
            return vm;
        }


        /// <summary>
        /// Prepare Transfer Order for specific outlet based on existing transferOrder
        /// </summary>
        /// <param name="outlet"></param>
        /// <returns></returns>
        public TransferOrderViewModel PrepareTransferOrderViewModel(OutletModel outlet, TransferOrderModel transferOrder)
        {

            TransferOrderViewModel vm = PrepareTransferOrderViewModel(outlet);

            // modify the quantity
            foreach (var detail in transferOrder.Details)
            {
                if (detail.Item != null)
                {
                    var detailsInVm = vm.TransferOrder.Details.Where(d => d.Item.Id == detail.Item.Id);
                    if (detailsInVm.Count() > 0)
                    {
                        foreach (var detailInVm in detailsInVm)
                        {
                            detailInVm.QuantityRequested = detail.QuantityRequested;
                        }
                    }
                }
            }
            return vm;
        }


        /// <summary>
        /// Submit TransferOrder
        /// </summary>
        /// <param name="transferOrder">prepared transfer order to be submitted</param>
        /// <returns>Transfer Order Submitted</returns>
        public TransferOrderModel SubmitTransferOrder(TransferOrderModel transferOrder)
        {
            if (transferOrder.Details != null)
            {
                transferOrder.Details = transferOrder.Details.Where(d => d.QuantityChange != 0).ToList();
                foreach (var detail in transferOrder.Details)
                {
                    this.InsertTransferOrderDetail(detail);
                }
            }
            return this.InsertTransferOrder(transferOrder);
        }


        /// <summary>
        /// Copy outlet item category from outlet <b>source</b> to outlet <b>destination</b>.
        /// </summary>
        /// <param name="source">outlet to copy from</param>
        /// <param name="destination">outlet to copy to</param>
        /// <returns>List of Outlet Item Categories for destination outlet after copying</returns>
        public List<OutletItemCategoryModel> CopyOutletItemCategory(OutletModel source, OutletModel destination)
        {
            var sourceCategories = GetOutletItemCategories(source);
            var destinationCategories = GetOutletItemCategories(destination);
            // two cases : 
            // 1) src category does not exists in  dst
            //   --> just copy whole categories and items
            // 2) src category exists in dst
            //   --> copy items that does not exists in dst


            var case1Cat = sourceCategories
                .Where(
                    scat =>
                        !destinationCategories.Select(dcat => dcat.Name)
                        .Contains(scat.Name)
                ).ToList();
            var case2Cat = sourceCategories.Except(case1Cat);

            // case 1:
            foreach (var cat in case1Cat)
            {
                InsertOutletItemCategory(new OutletItemCategoryModel()
                {
                    Items = cat.Items,
                    Name = cat.Name,
                    Outlet = destination,
                    OutletId = destination.Id,
                });
            }

            // case 2:
            foreach (var cat in case2Cat)
            {
                var matchedCat = destinationCategories.Where(dcat => dcat.Name == cat.Name);
                foreach (var dcat in matchedCat)
                {
                    foreach (var item in cat.Items)
                    {
                        InsertItemToOutletItemCategory(item, dcat);
                    }
                }
            }

            // finally return updated categories.
            var result = GetOutletItemCategories(destination);
            return result;

        }



        #endregion


        #region CRUD Services

        #region Items
        public ItemModel InsertItem(ItemModel ItemModel)
        {
            // should have some validation here...

            // Finally put to repository
            try
            {
                _ItemRepository.Create(ItemModel);
                return ItemModel;
            }
            catch (Exception e)
            {
                _modelStateDictionary.AddModelError("Insert Exception", e.Message);
                return null;
            }
        }

        public List<ItemModel> GetItems()
        {
            var result = this._ItemRepository.GetItems();
            result = result.Include(i => i.OutletItemCategories).OrderBy(i => i.Serial);
            return result.ToList();
        }

        public ItemModel GetItemById(int ItemId)
        {
            var result = this._ItemRepository.GetItemById(ItemId);
            return result;
        }

        public ItemModel GetItemBySerial(string serial)
        {
            var result = this._ItemRepository.Find(i => i.Serial == serial);
            return result;
        }


        public ItemModel UpdateItem(ItemModel Item)
        {
            this._ItemRepository.UpdateItem(Item);
            return Item;
        }

        public ItemModel DeleteItem(int ItemId)
        {
            var item = this._ItemRepository.GetItemById(ItemId);
            return this.DeleteItem(item);
        }
        public ItemModel DeleteItem(ItemModel item)
        {
            this._ItemRepository.DeleteItem(item.Id);
            return item;
        }

        protected ItemModel UpdateItemPackagingUnit(ItemModel item, string unitName)
        {
            var pUnits = this._ConfigUnitRepository.GetConfigUnits().Where(u => u.TypeValue == (int)UnitType.Package).ToList();
            // yes here we need to violate the repository model again....
            var pUnitsLocal = this.dbContext.ConfigUnits.Local.ToList();
            // and yes we need to materialize the units from db first.
            var pUnit = pUnits.Union(pUnitsLocal).Where(u => u.Name == unitName).ToList();
            if (pUnit.Any())
            {
                item.PackageUnit = pUnit.First();
                item.PackageUnitId = pUnit.First().Id;
            }
            else
            {
                var unit = this._ConfigUnitRepository.Create(new ConfigUnitModel()
                {
                    Name = unitName,
                    unitType = UnitType.Package,
                });
                item.PackageUnit = unit;
            }
            return item;
        }

        #endregion


        #region ItemCategory

        public ItemCategoryModel InsertItemCategory(ItemCategoryModel ItemCategoryModel)
        {
            // should have some validation here...

            // Finally put to repository
            try
            {
                _ItemCategoryRepository.Create(ItemCategoryModel);
                return ItemCategoryModel;
            }
            catch (Exception e)
            {
                _modelStateDictionary.AddModelError("Insert Exception", e.Message);
                return null;
            }
        }

        public List<ItemCategoryModel> GetItemCategories()
        {
            return this._ItemCategoryRepository.GetItemCategories().OrderBy(c => c.Name).ToList();
        }

        public ItemCategoryModel GetItemCategoryById(int ItemCategoryId)
        {
            return this._ItemCategoryRepository.GetItemCategoryById(ItemCategoryId);
        }

        public ItemCategoryModel UpdateItemCategory(ItemCategoryModel ItemCategory)
        {
            this._ItemCategoryRepository.UpdateItemCategory(ItemCategory);
            return ItemCategory;
        }

        public ItemCategoryModel DeleteItemCategory(int ItemCategoryId)
        {
            var result = this.GetItemCategoryById(ItemCategoryId);
            return this.DeleteItemCategory(result);
        }

        public ItemCategoryModel DeleteItemCategory(ItemCategoryModel ItemCategory)
        {
            this._ItemCategoryRepository.DeleteItemCategory(ItemCategory.Id);
            return ItemCategory;
        }

        #endregion

        #region OutletItemCategorys

        public OutletItemCategoryModel InsertOutletItemCategory(OutletItemCategoryModel OutletItemCategorysModel)
        {
            // should have some validation here...

            // Finally put to repository
            try
            {
                _OutletItemCategoryRepository.Create(OutletItemCategorysModel);
                return OutletItemCategorysModel;
            }
            catch (Exception e)
            {
                _modelStateDictionary.AddModelError("Insert Exception", e.Message);
                return null;
            }
        }

        public List<OutletItemCategoryModel> GetOutletItemCategories()
        {
            return this._OutletItemCategoryRepository.GetOutletItemCategories().ToList();
        }

        public List<OutletItemCategoryModel> GetOutletItemCategories(OutletModel outlet)
        {
            return this._OutletItemCategoryRepository.GetOutletItemCategories().Where(o => o.OutletId == outlet.Id).ToList();
        }


        public OutletItemCategoryModel GetOutletItemCategoryById(int OutletItemCategoryId)
        {
            return this._OutletItemCategoryRepository.GetOutletItemCategoryById(OutletItemCategoryId);
        }

        public OutletItemCategoryModel UpdateOutletItemCategory(OutletItemCategoryModel OutletItemCategory)
        {
            this._OutletItemCategoryRepository.UpdateOutletItemCategory(OutletItemCategory);
            return OutletItemCategory;
        }

        public OutletItemCategoryModel DeleteOutletItemCategory(int OutletItemCategoryId)
        {
            var result = this.GetOutletItemCategoryById(OutletItemCategoryId);
            return this.DeleteOutletItemCategory(result);
        }

        public OutletItemCategoryModel DeleteOutletItemCategory(OutletItemCategoryModel OutletItemCategoryModel)
        {
            this._OutletItemCategoryRepository.DeleteOutletItemCategory(OutletItemCategoryModel.Id);
            return OutletItemCategoryModel;
        }

        #endregion

        #region TransferOrderDetail

        public TransferOrderDetailModel InsertTransferOrderDetail(TransferOrderDetailModel TransferOrderDetailModel)
        {
            // should have some validation here...

            // Finally put to repository
            try
            {
                _TransferOrderDetailRepository.Create(TransferOrderDetailModel);
                return TransferOrderDetailModel;
            }
            catch (Exception e)
            {
                _modelStateDictionary.AddModelError("Insert Exception", e.Message);
                return null;
            }
        }

        public List<TransferOrderDetailModel> GetTransferOrderDetails()
        {
            return this._TransferOrderDetailRepository.GetTransferOrderDetails().ToList();
        }

        public List<TransferOrderDetailModel> GetTransferOrderDetails(TransferOrderModel TransferOrder)
        {
            return this._TransferOrderDetailRepository.GetTransferOrderDetails()
                .Where(detail => detail.ParentOrderID == TransferOrder.Id)
                .ToList();
        }

        public TransferOrderDetailModel GetTransferOrderDetailById(int TransferOrderDetailId)
        {
            return this._TransferOrderDetailRepository.GetTransferOrderDetailById(TransferOrderDetailId);
        }

        public TransferOrderDetailModel UpdateTransferOrderDetail(TransferOrderDetailModel TransferOrderDetail)
        {
            this._TransferOrderDetailRepository.UpdateTransferOrderDetail(TransferOrderDetail);
            return TransferOrderDetail;
        }

        public TransferOrderDetailModel DeleteTransferOrderDetail(int TransferOrderDetailId)
        {
            var result = this.GetTransferOrderDetailById(TransferOrderDetailId);
            return this.DeleteTransferOrderDetail(result);
        }
        public TransferOrderDetailModel DeleteTransferOrderDetail(TransferOrderDetailModel TransferOrderDetail)
        {
            this._TransferOrderDetailRepository.DeleteTransferOrderDetail(TransferOrderDetail.Id);
            return TransferOrderDetail;
        }

        #endregion

        #region TransferOrder

        public TransferOrderModel InsertTransferOrder(TransferOrderModel TransferOrderModel)
        {
            // should have some validation here...

            // Finally put to repository
            try
            {
                _TransferOrderRepository.Create(TransferOrderModel);
                return TransferOrderModel;
            }
            catch (Exception e)
            {
                _modelStateDictionary.AddModelError("Insert Exception", e.Message);
                return null;
            }
        }

        public List<TransferOrderModel> GetTransferOrders()
        {
            return this._TransferOrderRepository.GetTransferOrders().ToList();
        }

        public List<TransferOrderModel> GetTransferOrders(OutletModel outlet)
        {
            var result = this._TransferOrderRepository.GetTransferOrders();
            // this is tricky : the transfer order store the location and location does not correspond to an outlet
            result = result.Where(t => t.DestinationLocationId == outlet.LocationId);
            return result.ToList();
        }


        public TransferOrderModel GetTransferOrderById(int TransferOrderId)
        {
            return this._TransferOrderRepository.GetTransferOrderById(TransferOrderId);
        }

        public TransferOrderModel UpdateTransferOrder(TransferOrderModel TransferOrder)
        {
            this._TransferOrderRepository.UpdateTransferOrder(TransferOrder);
            return TransferOrder;
        }

        public TransferOrderModel DeleteTransferOrder(int TransferOrderId)
        {
            var result = this.GetTransferOrderById(TransferOrderId);
            return this.DeleteTransferOrder(result);
        }

        public TransferOrderModel DeleteTransferOrder(TransferOrderModel TransferOrder)
        {
            this._TransferOrderRepository.DeleteTransferOrder(TransferOrder.Id);
            return TransferOrder;
        }

        #endregion

        #endregion


    }
}