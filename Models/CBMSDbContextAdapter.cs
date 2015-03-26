using CBMS.Models.FrameLog;
using FrameLog.Contexts;
using FrameLog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBMS.Models
{
    public class CBMSDbContextAdapter : DbContextAdapter<ChangeSet, ApplicationUser>
    {
        private CBMSDbContext context;

        public CBMSDbContextAdapter(CBMSDbContext context)
            : base(context)
        {
            this.context = context;
        }

        public override IQueryable<IChangeSet<ApplicationUser>> ChangeSets
        {
            get { return context.ChangeSets; }
        }
        public override IQueryable<IObjectChange<ApplicationUser>> ObjectChanges
        {
            get { return context.ObjectChanges; }
        }
        public override IQueryable<IPropertyChange<ApplicationUser>> PropertyChanges
        {
            get { return context.PropertyChanges; }
        }
        public override void AddChangeSet(ChangeSet changeSet)
        {
            context.ChangeSets.Add(changeSet);
        }

        public override Type UnderlyingContextType
        {
            get { return typeof(CBMSDbContext); }
        }
    }
}