using FrameLog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBMS.Models.FrameLog
{

    public class ObjectChange : IObjectChange<ApplicationUser>
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public string ObjectReference { get; set; }
        public virtual ChangeSet ChangeSet { get; set; }
        public virtual List<PropertyChange> PropertyChanges { get; set; }

        IEnumerable<IPropertyChange<ApplicationUser>> IObjectChange<ApplicationUser>.PropertyChanges
        {
            get { return PropertyChanges; }
        }
        void IObjectChange<ApplicationUser>.Add(IPropertyChange<ApplicationUser> propertyChange)
        {
            PropertyChanges.Add((PropertyChange)propertyChange);
        }
        IChangeSet<ApplicationUser> IObjectChange<ApplicationUser>.ChangeSet
        {
            get { return ChangeSet; }
            set { ChangeSet = (ChangeSet)value; }
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", TypeName, ObjectReference);
        }
    }
}