using FrameLog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBMS.Models.FrameLog
{
    public class PropertyChange : IPropertyChange<ApplicationUser>
    {
        public int Id { get; set; }
        public virtual ObjectChange ObjectChange { get; set; }
        public string PropertyName { get; set; }
        public string Value { get; set; }
        public int? ValueAsInt { get; set; }

        IObjectChange<ApplicationUser> IPropertyChange<ApplicationUser>.ObjectChange
        {
            get { return ObjectChange; }
            set { ObjectChange = (ObjectChange)value; }
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", PropertyName, Value);
        }
    }
}