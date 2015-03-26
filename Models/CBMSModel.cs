using FrameLog.Filter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBMS.Models
{
    public class CBMSModel : IValidatableObject
    {
        /// <summary>
        /// Id stored in database.
        /// </summary>
        [Key]
        [DoNotLog]
        public int Id { get; set; }

        /// <summary>
        /// Object Version in database.
        /// </summary>
        [Timestamp]
        [ScaffoldColumn(false)]
        [DoNotLog]
        public byte[] RowVersion { get; set; }

        /// <summary>
        /// Object Creation Time
        /// </summary>
        [Editable(false)]
        public DateTime? ObjectCreateTime { get; set; }

        /// <summary>
        /// Object Update Time
        /// </summary>
        [Editable(false)]
        [DoNotLog]
        public DateTime? ObjectUpdateTime { get; set; }

        /// <summary>
        /// Object Status in database.
        /// </summary>
        [ScaffoldColumn(false)]
        public string Status { get; set; }

        // overrideable
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ObjectCreateTime.HasValue && ObjectUpdateTime.HasValue && ObjectCreateTime > ObjectUpdateTime)
            {
                // yield is used to pause the code execution for IEnumerable. 
                // the method will continue at this point after next iteration of the IEnumerable.
                yield return new ValidationResult("Object Update Time should not be earlier than Object Create Time!", new[] { "ObjectUpdateTime" });
            }
        }
    }
}
