using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CBMS.Services.Interfaces.Roster;
using System.Web.Configuration;
using CBMS.Models;
using CBMS.Models.Roster;
using CBMS.Models.Inventory;
using CBMS.Models.ViewModels;
using System.Threading.Tasks;
using CBMS.Utilities;
using CBMS.Services.Interfaces.Inventory;
using System.IO;
using System.Net;
using CBMS.Services.Interfaces.Config;



namespace CBMS.Controllers.Inventory
{
    public class InventoryController : Controller
    {
        string outletServiceName = WebConfigurationManager.AppSettings["CBMS.SERVICES.ROSTER.OUTLET"];
        string brandServiceName = WebConfigurationManager.AppSettings["CBMS.SERVICES.ROSTER.BRAND"];
        string positionServiceName = WebConfigurationManager.AppSettings["CBMS.SERVICES.ROSTER.POSITION"];
        string InventoryServicesName = WebConfigurationManager.AppSettings["CBMS.SERVICES.INVENTORY"];
        string cfgServicesName = WebConfigurationManager.AppSettings["CBMS.SERVICES.CONFIG.CONFIG"];

        IInventoryServices _InventoryServices;
        IBrandServices brandServices;
        IOutletServices outletServices;
        IPositionServices positionServices;
        IConfigurationServices cfgServices;

        private CBMSDbContext db;
        public InventoryController()
        {
            this.db = new CBMSDbContext();
            outletServices = (IOutletServices)Activator.CreateInstance(Type.GetType(outletServiceName), this.ModelState, db);
            brandServices = (IBrandServices)Activator.CreateInstance(Type.GetType(brandServiceName), this.ModelState, db);
            positionServices = (IPositionServices)Activator.CreateInstance(Type.GetType(positionServiceName), this.ModelState, db);
            this._InventoryServices = (IInventoryServices)Activator.CreateInstance(Type.GetType(InventoryServicesName), this.ModelState, db);
            cfgServices = (IConfigurationServices)Activator.CreateInstance(Type.GetType(cfgServicesName), this.ModelState, db);

        }

        // GET: Inventory
        public ActionResult Index()
        {
            return View();
        }


        // test page used for testing autocomplete
        public ActionResult Test()
        {
            return View();
        }

        public ActionResult ListAllItem()
        {
            var itemModelList = _InventoryServices.GetItems().ToList();
            var itemCategoryList = _InventoryServices.GetItemCategories().ToList();
            ItemViewModel vm = new ItemViewModel()
            {
                ItemCategories = itemCategoryList,
                Items = itemModelList,
                NewItem = new ItemModel(),
                NewItemCategory = new ItemCategoryModel(),
            };
            ViewBag.CategoryId = new SelectList(this._InventoryServices.GetItemCategories(), "Id", "Name");
            ViewBag.PackageUnitId = new SelectList(cfgServices.GetConfigUnits(), "Id", "Name");
            return View(vm);
        }

        public async Task<ActionResult> ListOutletItem(int? OutletId)
        {
            InventoryViewModel vm = new InventoryViewModel();

            ViewBag.BrandIdList = new SelectList(brandServices.GetBrands(), "Id", "Name");
            ViewBag.OutletIdList = new SelectList(db.Outlets, "Id", "Name");
            ViewBag.DepartmentIdList = new SelectList(db.Departments, "Id", "Name");
            ViewBag.CopyOutletIdList = new SelectList(outletServices.GetOutlets(), "Id", "Name", "Brand.Name",selectedValue:null);

            if (OutletId.HasValue)
            {
                var outlet = await this.outletServices.GetOutletById(OutletId.Value);
                if (outlet != null)
                {
                    vm.Brand = outlet.Brand;
                    vm.Outlet = outlet;
                    vm.OutletCategoryList = this._InventoryServices.GetOutletItemCategories(outlet);
                    ViewBag.BrandIdList = new SelectList(brandServices.GetBrands(), "Id", "Name", outlet.BrandId);
                    ViewBag.OutletIdList = new SelectList(brandServices.GetOutlets(outlet.Brand), "Id", "Name", outlet.Id);
                    ViewBag.CopyOutletIdList = new SelectList(outletServices.GetOutlets(), "Id", "Name", "Brand.Name",outlet.Id);
                }
            }


            return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult> CopyOutletCategory(OutletModel Outlet, int? sourceOutletId)
        {
            if (Outlet == null)
            {
                return HttpNotFound();
            }
            var targetOutlet = await outletServices.GetOutletById(Outlet.Id);

            if (targetOutlet == null)
            {
                return HttpNotFound();
            }

            if (!sourceOutletId.HasValue)
            {
                return RedirectToAction("ListOutletItem", new { OutletId = targetOutlet.Id });
            }
            var sourceOutlet = await outletServices.GetOutletById(sourceOutletId.Value);
            if (sourceOutlet == null)
            {
                return RedirectToAction("ListOutletItem", new { OutletId = targetOutlet.Id });
            }

            _InventoryServices.CopyOutletItemCategory(sourceOutlet, targetOutlet);
            _InventoryServices.SaveChanges();

            return RedirectToAction("ListOutletItem", new { OutletId = targetOutlet.Id });

        }


        // GET: Item/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ItemModel itemModel = _InventoryServices.GetItemById(id.Value);
            if (itemModel == null)
            {
                return HttpNotFound();
            }
            return View(itemModel);
        }

        // GET: Item/Create
        public ActionResult Create()
        {
            ViewBag.CategoryIdList = new SelectList(this._InventoryServices.GetItemCategories(), "Id", "Name");
            ViewBag.PackageUnitIdList = new SelectList(cfgServices.GetConfigUnits(), "Id", "Name");
            return View();
        }

        // POST: Item/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Serial,Name,ChinName,CategoryId,PackageUnitId")] ItemModel itemModel)
        {
            if (ModelState.IsValid)
            {
                _InventoryServices.InsertItem(itemModel);

                if (ModelState.IsValid)
                {
                    _InventoryServices.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.CategoryIdList = new SelectList(_InventoryServices.GetItemCategories(), "Id", "Name", itemModel.CategoryId);
            ViewBag.PackageUnitIdList = new SelectList(cfgServices.GetConfigUnits(), "Id", "Name", itemModel.PackageUnitId);
            return View(itemModel);
        }

        // GET: Item/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemModel itemModel = _InventoryServices.GetItemById(id.Value);

            if (itemModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(_InventoryServices.GetItemCategories(), "Id", "Name", itemModel.CategoryId);
            ViewBag.PackageUnitId = new SelectList(cfgServices.GetConfigUnits(), "Id", "Name", itemModel.PackageUnitId);
            return View(itemModel);
        }

        // POST: Item/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Serial,Name,ChinName,CategoryId,PackageUnitId")] ItemModel itemModel)
        {
            if (ModelState.IsValid)
            {
                _InventoryServices.UpdateItem(itemModel);

                if (ModelState.IsValid)
                {
                    _InventoryServices.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.CategoryId = new SelectList(_InventoryServices.GetItemCategories(), "Id", "Name", itemModel.CategoryId);
            ViewBag.PackageUnitId = new SelectList(cfgServices.GetConfigUnits(), "Id", "Name", itemModel.PackageUnitId);
            return View(itemModel);
        }

        // GET: Item/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemModel itemModel = _InventoryServices.GetItemById(id.Value);
            if (itemModel == null)
            {
                return HttpNotFound();
            }
            return View(itemModel);
        }

        // POST: Item/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ItemModel itemModel = _InventoryServices.GetItemById(id);
            if (itemModel == null)
            {
                return HttpNotFound();
            }
            _InventoryServices.DeleteItem(itemModel.Id);
            _InventoryServices.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<ActionResult> CreateOutletCategory(InventoryViewModel vm)
        {

            if (vm.Brand == null || vm.Outlet == null)
            {
                return RedirectToAction("ListOutletItem");
            }
            if (vm.NewOutletItemCategory == null)
            {
                return RedirectToAction("ListOutletItem", new { OutletId = vm.Outlet.Id });
            }

            var outlet = await this.outletServices.GetOutletById(vm.Outlet.Id);
            var category = new OutletItemCategoryModel()
            {
                Name = vm.NewOutletItemCategory.Name,
                Outlet = outlet,
                OutletId = outlet.Id,
            };
            this._InventoryServices.InsertOutletItemCategory(category);
            this._InventoryServices.SaveChanges();


            return RedirectToAction("ListOutletItem", new { OutletId = vm.Outlet.Id });
        }

        [HttpPost]
        public ActionResult InsertOutletItem(InventoryViewModel vm)
        {
            //try
            //{
            if (vm.NewItem == null)
            {
                return PartialView();
            }
            if (vm.OutletCategoryList == null || !vm.OutletCategoryList.Any())
            {
                return PartialView();
            }
            var item = this._InventoryServices.GetItemBySerial(vm.NewItem.Serial);
            var outletCategory = this._InventoryServices.GetOutletItemCategoryById(vm.OutletCategoryList.First().Id);

            this._InventoryServices.InsertItemToOutletItemCategory(item, outletCategory);
            this._InventoryServices.SaveChanges();

            ViewBag.outletItemCategoryId = outletCategory.Id;
            return PartialView("_itemTable", outletCategory.Items);
        }


        public ActionResult DeleteOutletItemCategory(int outletItemCategoryId)
        {
            var outletItemCategory = this._InventoryServices.GetOutletItemCategoryById(outletItemCategoryId);
            if (outletItemCategory != null)
            {
                this._InventoryServices.DeleteOutletItemCategory(outletItemCategoryId);
                this._InventoryServices.SaveChanges();
                return RedirectToAction("ListOutletItem", new { OutletId = outletItemCategory.OutletId });
            }
            return RedirectToAction("ListOutletItem");

        }

        public ActionResult DeleteItemFromOutletItemCategory(int itemId, int outletItemCategoryId)
        {
            var item = this._InventoryServices.GetItemById(itemId);
            var outletCategory = this._InventoryServices.GetOutletItemCategoryById(outletItemCategoryId);

            if (item == null || outletCategory == null)
            {
                return PartialView("_itemTable", new List<ItemModel>());
            }

            this._InventoryServices.RemoveItemFromOutletItemCategory(item, outletCategory);
            this._InventoryServices.SaveChanges();

            //return RedirectToAction("ListOutletItem", new { outletId = outletCategory.OutletId });
            ViewBag.outletItemCategoryId = outletCategory.Id;

            return PartialView("_itemTable", outletCategory.Items);
        }



        #region Item
        public ActionResult ListItem()
        {
            return PartialView("_ListItem", _InventoryServices.GetItems());
        }

        [HttpPost]
        public ActionResult CreateItem(ItemViewModel vm)
        {
            _InventoryServices.InsertItem(vm.NewItem);
            _InventoryServices.SaveChanges();
            return PartialView("_ListItem", _InventoryServices.GetItems());
        }

        public async Task<ActionResult> EditItem(int ItemId)
        {
            var item = _InventoryServices.GetItemById(ItemId);
            ViewBag.CategoryIdList = new SelectList(this._InventoryServices.GetItemCategories(), "Id", "Name", item.CategoryId);
            ViewBag.PackageUnitIdList = new SelectList(cfgServices.GetConfigUnits(), "Id", "Name", item.PackageUnitId);
            return PartialView("_EditItem", item);
        }

        [HttpPost]
        public async Task<ActionResult> EditItem(ItemModel ItemModel)
        {
            _InventoryServices.UpdateItem(ItemModel);
            _InventoryServices.SaveChanges();
            return PartialView("_ListItem", _InventoryServices.GetItems());
        }

        // POST: ConfigValue/Delete/5
        [HttpPost]
        public async Task<ActionResult> DeleteItem(int ItemId)
        {
            _InventoryServices.DeleteItem(ItemId);
            _InventoryServices.SaveChanges();
            return PartialView("_ListItem", _InventoryServices.GetItems());
        }
        #endregion

        #region ItemCategory
        public ActionResult ListItemCategory()
        {
            return PartialView("_ListItemCategory", _InventoryServices.GetItemCategories());
        }

        [HttpPost]
        public ActionResult CreateItemCategory(ItemViewModel vm)
        {
            _InventoryServices.InsertItemCategory(vm.NewItemCategory);
            _InventoryServices.SaveChanges();
            return PartialView("_ListItemCategory", _InventoryServices.GetItemCategories());
        }

        public async Task<ActionResult> EditItemCategory(int ItemCategoryId)
        {
            return PartialView("_EditItemCategory", _InventoryServices.GetItemCategoryById(ItemCategoryId));
        }

        [HttpPost]
        public async Task<ActionResult> EditItemCategory(ItemCategoryModel ItemCategoryModel)
        {
            _InventoryServices.UpdateItemCategory(ItemCategoryModel);
            _InventoryServices.SaveChanges();
            return PartialView("_ListItemCategory", _InventoryServices.GetItemCategories());
        }

        // POST: ConfigValue/Delete/5
        [HttpPost]
        public async Task<ActionResult> DeleteItemCategory(int ItemCategoryId)
        {
            _InventoryServices.DeleteItemCategory(ItemCategoryId);
            _InventoryServices.SaveChanges();
            return PartialView("_ListItemCategory", _InventoryServices.GetItemCategories());
        }
        #endregion







        // GET: Inventory/Upload/5
        public ActionResult Upload()
        {
            return View();
        }

        // POST: Inventory/Upload/5
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {

            if (file == null || file.ContentLength == 0)
            {
                ViewBag.StatusMessage += "No file is uploaded. Please try again.";
                return View();
            }
            if (!file.FileName.EndsWith(".xlsx") && !file.FileName.EndsWith(".xls"))
            {
                ViewBag.StatusMessage += "Uploaded file is not a valid excel file. Please try again.";
                return View();
            }

            var directoryPath = Server.MapPath("~/App_Data/uploads");
            Directory.CreateDirectory(directoryPath);
            var fileName = Path.GetFileName(file.FileName);
            var path = Path.Combine(directoryPath, fileName);
            file.SaveAs(path);
            ViewBag.StatusMessage += "File has been uploaded successfully.";
            var affectedRow = this._InventoryServices.ImportItem(path);
            this._InventoryServices.SaveChanges();
            ViewBag.StatusMessage += " " + affectedRow + " Row(s) has(have) been affected.";
            return View();
        }
    }
}
