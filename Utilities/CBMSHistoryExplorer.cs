using FrameLog.Contexts;
using FrameLog.Exceptions;
using FrameLog.History;
using FrameLog.History.Binders;
using FrameLog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CBMS.Utilities
{
    public class CBMSHistoryExplorer<TChangeSet, TPrincipal> : HistoryExplorer<TChangeSet, TPrincipal> 
        where TChangeSet : IChangeSet<TPrincipal>
    {
        private IHistoryContext<TChangeSet, TPrincipal> db;
        private IBindManager binder;

        public CBMSHistoryExplorer(IHistoryContext<TChangeSet, TPrincipal> db, IBindManager binder = null)
            : base(db, binder)
        {
            this.db = db;
            this.binder = binder ?? new BindManager(db);
        }

        public IEnumerable<IChange<TValue,TPrincipal>> GetChangesTo<TModel,TValue>(TModel model){
            var changes = changesTo(model);
            throw new NotImplementedException();
        }

        public IEnumerable<IChange<TValue,TPrincipal>> GetChangesTo<TModel,TValue>(TModel model, Expression<Func<TModel,TValue>> property){
            throw new NotImplementedException();
        }



        /// <summary>
        /// Returns all IObjectChanges that are relevant to this object, earliest first
        /// </summary>
        public virtual IOrderedQueryable<IObjectChange<TPrincipal>> changesTo<TModel>(TModel model)
        {
            string typeName = typeof(TModel).Name;
            string reference = db.GetReferenceForObject(model);

            var changes = db.ObjectChanges
                .Where(o => o.TypeName == typeName)
                .Where(o => o.ObjectReference == reference)
                .OrderBy(o => o.ChangeSet.Timestamp);
            return changes;
        }

        /// <summary>
        /// Returns all IObjectChanges that are relevant to this object, earliest first
        /// </summary>
        public virtual IEnumerable<IChangeSet<TPrincipal>> changeSetsTo<TModel>(TModel model)
        {
            string typeName = typeof(TModel).Name;
            string reference = db.GetReferenceForObject(model);

            var changes = db.ObjectChanges
                .Where(o => o.TypeName == typeName)
                .Where(o => o.ObjectReference == reference)
                .OrderBy(o => o.ChangeSet.Timestamp);

            var changeSets = new List<IChangeSet<TPrincipal>>();
            changes.ToList().ForEach(c => { changeSets.Add(c.ChangeSet); });
            return changeSets.Distinct();
        }


        ///// <summary>
        ///// Retrieve the values that a single property has gone through, most recent
        ///// first (descending date order).
        ///// 
        ///// If the property is a complex type then it must have a default constructor
        ///// and implement ICloneable.
        ///// </summary>
        //new public virtual IEnumerable<IChange<TValue, TPrincipal>> ChangesTo<TModel, TValue>(TModel model, Expression<Func<TModel, TValue>> property)
        //     where TModel : new()
        //{
        //    string typeName = typeof(TModel).Name;
        //    string propertyName = ((MemberExpression)property.Body).Member.Name;
        //    string propertyPrefix = propertyName + ".";
        //    string reference = db.GetReferenceForObject(model);
        //    var propertyFunc = property.Compile();

        //    var objectChanges = changesTo(model)
        //        .SelectMany(o => o.PropertyChanges)
        //        .Where(p => p.PropertyName == propertyName || p.PropertyName.StartsWith(propertyPrefix))
        //        .AsEnumerable()
        //        .GroupBy(p => p.ObjectChange)
        //        .Select(g => new FilteredObjectChange<TPrincipal>(g.Key, g));

        //    // If the propert expression refers to a complex type then we will not have any changes
        //    // directly to that property. Instead we will see changes to sub-properties.
        //    // We retrieve the history differently for complex types, so here we distinguish which
        //    // case we are in by looking at the first change.
        //    var sample = objectChanges.SelectMany(o => o.PropertyChanges).FirstOrDefault();
        //    if (sample != null && sample.PropertyName.StartsWith(propertyPrefix))
        //    {
        //        // Construct a "seed" instance of the complex type, and then apply changes to it in order
        //        // to reconstruct the intermediate states.
        //        return applyChangesTo(
        //                HistoryHelpers.Instantiate<TValue>(),
        //                objectChanges,
        //                propertyName
        //                )
        //            .OrderByDescending(c => c.Timestamp);
        //    }
        //    else
        //    {
        //        // Just directly bind the simple property values
        //        return objectChanges
        //            .OrderByDescending(o => o.ChangeSet.Timestamp)
        //            .SelectMany(o => o.PropertyChanges)
        //            .Select(p => Change.FromObjectChange(binder.Bind<TValue>(p.Value), p.ObjectChange));
        //    }
        //}

        ///// <summary>
        ///// Rehydrates versions of the object, one for each logged change to the object,
        ///// most recent first (descending date order).
        ///// </summary>
        //public virtual IEnumerable<IChange<TModel, TPrincipal>> ChangesTo<TModel>(TModel model)
        //    where TModel:new()
        //{
        //    var changes = changesTo(model);
        //    return applyChangesTo(new TModel(), changes)
        //        .OrderByDescending(c => c.Timestamp);
        //}

        //new protected virtual IEnumerable<IChange<TModel, TPrincipal>> applyChangesTo<TModel>(TModel seed, IEnumerable<IObjectChange<TPrincipal>> changes, string prefix = "")
        //     where TModel : new()
        //{
        //    TModel current = seed;
        //    foreach (var change in changes)
        //    {
        //        var c = apply(change, current, prefix);
        //        yield return c;
        //        current = c.Value;
        //    }
        //}


        //protected virtual IChange<TModel, TPrincipal> apply<TModel>(IObjectChange<TPrincipal> change, TModel model, string prefix)
        //     where TModel : new()
        //{
        //    var type = typeof(TModel);
        //    var newVersion = clone(model);
        //    foreach (var propertyChange in change.PropertyChanges.Select(p => new PropertyChangeProcessor<TPrincipal>(p)))
        //    {
        //        propertyChange.ApplyTo(newVersion, binder, prefix);
        //    }
        //    return Change.FromObjectChange(newVersion, change);
        //}

        //private TModel clone<TModel>(TModel model) where TModel:new()
        //{
        //    var TModelClone = AutoMapper.Mapper.CreateMap<TModel, TModel>();
        //    TModel cloned = new TModel();
        //    cloned = AutoMapper.Mapper.Map(model, cloned);
        //    return cloned;
        //}
    }
}