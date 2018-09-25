using DataRepositoryInterfaces;
using RepositoryInterface;
using ObligatorioDA2.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic;

namespace DataRepositoriesTest
{
    public class CommentRepository:ICommentRepository
    {
        private IEntityRepository<CommentEntity> commentsStorage;

        public CommentRepository(IEntityRepository<CommentEntity> aRepo)
        {
            commentsStorage = aRepo;
        }

        public void AddComment(Commentary comment)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool IsEmpty()
        {
            throw new NotImplementedException();
        }
    }
}
