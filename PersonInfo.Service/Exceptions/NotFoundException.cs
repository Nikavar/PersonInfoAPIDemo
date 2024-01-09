﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PersonInfo.Service
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string name, object key)
            : base($"Entity: {name} - ({key}) არ მოიძებნა.")
        {
        }
    }
}
