using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GithubSearch.DAL.Repository
{
    public class GithubRepositoriesRepository : IGithubRepositoriesRepository
    {
        public Task<bool> DeleteAsync(Shared.DTO.Repository entityToDelete)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Shared.DTO.Repository> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task InsertAsync(Shared.DTO.Repository entity)
        {
            throw new NotImplementedException();
        }

        public Task InsertAsync(IEnumerable<Shared.DTO.Repository> entities)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Shared.DTO.Repository entity)
        {
            throw new NotImplementedException();
        }
    }
}
