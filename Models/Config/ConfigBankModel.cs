using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBMS.Models.Config
{
    public class ConfigBankModel : CBMSModel
    {      
        public string BankName { get; set; }

    }
}