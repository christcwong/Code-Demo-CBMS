using FrameLog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBMS.Models.FrameLog
{
    public class ChangeSet : IChangeSet<ApplicationUser>
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public virtual ApplicationUser Author { get; set; }
        public virtual List<ObjectChange> ObjectChanges { get; set; }

        IEnumerable<IObjectChange<ApplicationUser>> IChangeSet<ApplicationUser>.ObjectChanges
        {
            get { return ObjectChanges; }
        }

        void IChangeSet<ApplicationUser>.Add(IObjectChange<ApplicationUser> objectChange)
        {
            ObjectChanges.Add((ObjectChange)objectChange);
        }

        public override string ToString()
        {
            return string.Format("By {0} on {1}, with {2} ObjectChanges",
                Author, Timestamp, ObjectChanges.Count);
        }
    }
}