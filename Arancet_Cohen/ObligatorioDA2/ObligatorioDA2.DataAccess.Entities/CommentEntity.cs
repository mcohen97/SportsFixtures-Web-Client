using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.DataAccess.Entities
{
    public class CommentEntity 
    {
        public int Id { get; set; }
        public UserEntity Maker { get; set; }
        public string Text { get; set; }
    }
}
