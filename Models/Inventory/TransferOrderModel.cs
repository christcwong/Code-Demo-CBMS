using CBMS.Models.Roster;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CBMS.Models.Inventory
{
    public class TransferOrderModel : CBMSModel
    {

        #region Attributes
        [DisplayName("Date Of Issue")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime IssueDate { get; set; }

        [DisplayName("Effective Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EffectiveDate { get; set; }

        public string Note { get; set; }

        public int OrderStatusValue { get; set; }

        [NotMapped]
        public OrderStatusEnum OrderStatus
        {
            get { return (OrderStatusEnum)OrderStatusValue; }
            set { OrderStatusValue = (int)value; }
        }

        #endregion



        #region Foreign Keys

        public int SourceLocationId { get; set; }
        [DisplayName("Source Location")]
        public virtual LocationModel SourceLocation { get; set; }

        public int DestinationLocationId { get; set; }
        [DisplayName("Destination Location")]
        public virtual LocationModel DestinationLocation { get; set; }

        public int IssuerId { get; set; }
        public virtual ApplicationUser Issuer { get; set; }

        public virtual List<TransferOrderDetailModel> Details { get; set; }

        #endregion
    }
    public enum OrderStatusEnum
    {
        DRAFT,
        ACTIVE,
        ACCEPTED,
        REJECTED
    }
}