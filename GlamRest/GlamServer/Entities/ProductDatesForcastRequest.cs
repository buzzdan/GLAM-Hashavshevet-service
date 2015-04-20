using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlamServer.Entities
{
    public class ProductDatesForcastRequest
    {
        public int ProductID { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}