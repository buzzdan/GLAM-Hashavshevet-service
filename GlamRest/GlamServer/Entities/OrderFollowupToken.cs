using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyRestServer.Entities
{
    public class OrderFollowupToken
    {
        public int OrderID { get; set; }

        public bool OrderInsertedSuccessfully { get; set; }

        public string Error { get; set; }
    }
}