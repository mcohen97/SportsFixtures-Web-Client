﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.DataAccess.Entities
{
    public class CommentEntity : BaseEntity
    {
        public UserEntity Maker { get; set; }
        public string Text { get; set; }
    }
}