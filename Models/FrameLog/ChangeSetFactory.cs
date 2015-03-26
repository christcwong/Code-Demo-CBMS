using FrameLog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBMS.Models.FrameLog
{
    public class ChangeSetFactory : IChangeSetFactory<ChangeSet, ApplicationUser>
    {
        public ChangeSet ChangeSet()
        {
            var set = new ChangeSet();
            set.ObjectChanges = new List<ObjectChange>();
            return set;
        }

        public IObjectChange<ApplicationUser> ObjectChange()
        {
            var o = new ObjectChange();
            o.PropertyChanges = new List<PropertyChange>();
            return o;
        }

        public IPropertyChange<ApplicationUser> PropertyChange()
        {
            return new PropertyChange();
        }
    }
}