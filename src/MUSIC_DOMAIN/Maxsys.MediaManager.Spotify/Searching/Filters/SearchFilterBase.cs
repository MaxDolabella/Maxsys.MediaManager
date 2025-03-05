using Maxsys.Core;
using Maxsys.Core.Exceptions;

namespace Maxsys.MediaManager.Spotify.Searching.Filters;

public abstract class SearchFilterBase
{
    private const string PAGINATION_EXCEPTION_MESSAGE = "Pagination is invalid. Index must be positive and Size must be less or equal '50'. Current value is: [Index={0}, Size={1}].";

    private Pagination _pagination;
    private string _term;

    public string Term
    {
        get => _term;
        set
        {
            _term = value.Replace(" ", "+");
        }
    }

    public Pagination Pagination
    {
        get => _pagination;
        set
        {
            if (value.Index < 0 || value.Size > 50)
            {
                throw new DomainException(string.Format(PAGINATION_EXCEPTION_MESSAGE, value.Index, value.Size));
            }

            _pagination = value;
        }
    }
}
