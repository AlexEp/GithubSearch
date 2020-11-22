using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GithubSearch.Shared.Interfaces
{
    public interface ISearchEngine
    {
        Task<IEnumerable<GithubSearch.Shared.DTO.Repository>> SearchAsync(string searchValue, CancellationToken cancellationToken);
    }
}
