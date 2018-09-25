using DataRepositoryInterfaces;
using RepositoryInterface;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataRepositoriesTest
{
    public class CommentRepository:ICommentRepository
    {
        private IEntityRepository<CommentEntity> commentsStorage;

        public CommentRepository(IEntityRepository<CommentEntity> aRepo)
        {
            commentsStorage = aRepo;
        }
    }
}
