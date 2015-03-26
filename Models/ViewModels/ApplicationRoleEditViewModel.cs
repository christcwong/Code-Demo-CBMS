using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBMS.Models.ViewModels
{
    public class ApplicationRoleEditViewModel
    {
        public ApplicationRole Role { get; set; }
        public IList<Accessiblity> AvailableAccessiblities { get; set; }
        public PostedAccessibilities PostedAccessiblities { get; set; }

    }

    public class PostedAccessibilities
    {
        public int[] AccessibilityIds { get; set; }
    }
}