﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;


namespace wcf_chat
{
    public class ServerUser
    {
        public string Name { get; set; }

        public int ID { get; set; }

        public OperationContext operationContext { get; set; }
    }
}
