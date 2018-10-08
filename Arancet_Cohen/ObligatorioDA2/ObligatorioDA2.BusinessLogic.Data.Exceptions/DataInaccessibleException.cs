﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.BusinessLogic.Data.Exceptions
{
    public class DataInaccessibleException: Exception
    {
        public DataInaccessibleException() : base("Can't access data") {}
    }
}
