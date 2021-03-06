﻿// <autogenerated>
//   This file was generated using ItemCategoryRepository.tt.
//   Any changes made manually will be lost next time the file is regenerated.
// </autogenerated>
using CBMS.Models;
using CBMS.Models.Inventory;
using CBMS.Repositories.Interfaces.Inventory;
using CBMS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CBMS.Repositories.Inventory
{
    public class ItemCategoryRepository : CBMSRepository<ItemCategoryModel>, IItemCategoryRepository
    {
        public ItemCategoryRepository()
            : base()
        {
        }

        public ItemCategoryRepository(CBMSDbContext context)
            : base(context)
        {
        }

        #region Get Context
        public CBMSDbContext GetContext()
        {
            return this.Context;
        }
        #endregion

        #region Create
        public override ItemCategoryModel Create(ItemCategoryModel ItemCategory)
        {
            ItemCategory.Status = ObjectStatus.ACTIVE;
            ItemCategory.ObjectCreateTime = DateTimeWrapper.Now;
            ItemCategory.ObjectUpdateTime = DateTimeWrapper.Now;
            return base.Create(ItemCategory);
        }

        public void InsertItemCategory(ItemCategoryModel ItemCategory)
        {
            this.Create(ItemCategory);
        }
        #endregion

        #region Read
        public IQueryable<ItemCategoryModel> GetItemCategories()
        {
            return base.Filter(m => m.Status != ObjectStatus.DELETED);
        }
        public IEnumerable<ItemCategoryModel> GetItemCategoriesAsList()
        {
            return GetItemCategories().ToList();
        }
        public ItemCategoryModel GetItemCategoryById(int ItemCategoryId)
        {
            return base.Find(ItemCategoryId);
        }
        public async Task<ItemCategoryModel> GetItemCategoryByIdAsync(int ItemCategoryId)
        {
            return await (base.FindAsync(ItemCategoryId));
        }
        #endregion

        #region Update
        public void UpdateItemCategory(ItemCategoryModel ItemCategory)
        {
            if (ItemCategory == null)
            {
                throw new ArgumentException("ItemCategory cannot be null.");
            }
            ItemCategoryModel dbVersionOfItemCategory = GetItemCategoryById(ItemCategory.Id);
            if (dbVersionOfItemCategory == null)
            {
                throw new ArgumentException("ItemCategory to be updated cannot be found in database.");
            }
            // Just put it into the db directly at the moment
            UpdateItemCategoryCore(ItemCategory, dbVersionOfItemCategory);
            //leave the commit to somewhere else.
            return;
        }
        public async Task UpdateItemCategoryAsync(ItemCategoryModel ItemCategory)
        {
            if (ItemCategory == null)
            {
                throw new ArgumentException("ItemCategory cannot be null.");
            }
            Task<ItemCategoryModel> GetDbVersionOfItemCategoryTask = GetItemCategoryByIdAsync(ItemCategory.Id);


            ItemCategoryModel dbVersionOfItemCategory = await (GetDbVersionOfItemCategoryTask);
            if (dbVersionOfItemCategory == null)
            {
                throw new ArgumentException("ItemCategory to be updated cannot be found in database.");
            }

            // Just put it into the db directly at the moment
            UpdateItemCategoryCore(ItemCategory, dbVersionOfItemCategory);
            //leave the commit to somewhere else.
            return;
        }
        private void UpdateItemCategoryCore(ItemCategoryModel Source, ItemCategoryModel Destination)
        {
            var ItemCategoryClone = AutoMapper.Mapper.CreateMap<ItemCategoryModel, ItemCategoryModel>();
            ItemCategoryClone.ForAllMembers(opt => opt.Condition(srs => !srs.IsSourceValueNull));
            ItemCategoryClone.ForMember(dst => dst.Id, opt => opt.Ignore());
            ItemCategoryClone.ForMember(dst => dst.Items, opt => opt.Ignore());
            ItemCategoryClone.ForMember(dst => dst.ObjectCreateTime, opt => opt.Ignore());
            Destination = AutoMapper.Mapper.Map(Source, Destination);
            Destination.ObjectUpdateTime = DateTimeWrapper.Now;
            base.Update(Destination);
        }
        #endregion

        #region Delete
        public void DeleteItemCategory(int ItemCategoryId)
        {
            ItemCategoryModel ItemCategory = GetItemCategoryById(ItemCategoryId);
            if (ItemCategory == null)
            {
                throw new ArgumentException("ItemCategory to be updated cannot be found in database.");
            }
            ItemCategory.Status = ObjectStatus.DELETED;
            ItemCategory.ObjectUpdateTime = DateTimeWrapper.Now;
            base.Update(ItemCategory);
            return;
        }
        public async Task DeleteItemCategoryAsync(int ItemCategoryId)
        {
            ItemCategoryModel ItemCategory = await GetItemCategoryByIdAsync(ItemCategoryId);
            if (ItemCategory == null)
            {
                throw new ArgumentException("ItemCategory to be updated cannot be found in database.");
            }
            ItemCategory.Status = ObjectStatus.DELETED;
            ItemCategory.ObjectUpdateTime = DateTimeWrapper.Now;
            base.Update(ItemCategory);
            return;
        }
        #endregion
    }
}