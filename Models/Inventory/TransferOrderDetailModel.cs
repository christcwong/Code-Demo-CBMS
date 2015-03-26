using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CBMS.Models.Inventory
{
    public class TransferOrderDetailModel : CBMSModel
    {

        #region Attributes


        [Display(Name = "Quantity Requested")]
        public int QuantityRequested { get; set; }

        [Display(Name = "Quantity Dispatched")]
        public int QuantityChange { get; set; }


        public int OrderDetailStatusValue { get; set; }
        [NotMapped]
        public OrderDetailStatusEnum OrderDetailStatus
        {
            get { return (OrderDetailStatusEnum)OrderDetailStatusValue; }
            set { OrderDetailStatusValue = (int)value; }
        }
        public string Note { get; set; }

        #endregion

        #region Foreign Keys

        public int ParentOrderID { get; set; }
        public virtual TransferOrderModel ParentOrder { get; set; }

        public int ItemId { get; set; }
        /// <summary>
        /// Item to be included in this detail
        /// </summary>
        public virtual ItemModel Item { get; set; }

        #endregion

    }
    public enum OrderDetailStatusEnum
    {
        PENDING,
        ACCEPTED,
        REJECTED
    }
}