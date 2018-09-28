using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using RepositoryInterface;
using BusinessLogic;

namespace DataRepositories
{
    public class SportRepository : IRepository<Sport>
    {
        private DatabaseConnection context;

        public SportRepository(DatabaseConnection context)
        {
            this.context = context;
        }

        public void Add(Sport entity)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool Exists(Sport record)
        {
            throw new NotImplementedException();
        }

        public Sport Get(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<Sport> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool IsEmpty()
        {
            throw new NotImplementedException();
        }

        public void Modify(Sport entity)
        {
            throw new NotImplementedException();
        }
    }
}
