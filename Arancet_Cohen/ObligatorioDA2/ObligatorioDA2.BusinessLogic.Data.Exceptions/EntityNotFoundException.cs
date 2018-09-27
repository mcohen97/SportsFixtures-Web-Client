﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.BusinessLogic.Data.Exceptions
{
    public class EntityNotFoundException:Exception
    {
        public EntityNotFoundException() {
        }

        public EntityNotFoundException(string message) : base(message) {

        }
    }
}