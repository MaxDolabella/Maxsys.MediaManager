using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;

namespace Maxsys.MediaManager.CoreDomain.Common;

// TODO: Mover para Core?
public class QueryParamsBuilder
{
    private List<QueryParamItem> _items = [];

    public QueryParamsBuilder Add(string name, object value, bool encode = true)
    {
        _items.Add(new QueryParamItem(name, value.ToString()!, encode));

        return this;
    }

    public override string ToString()
    {
        if (_items.Count == 0) return string.Empty;

        var items = _items.Select(x => x.ToQueryParam());

        return $"?{string.Join('&', items)}";
    }

    private class QueryParamItem(string name, string value, bool encode)
    {
        public string Name { get; } = name;
        public string Value { get; } = encode ? UrlEncoder.Default.Encode(value) : value;

        public string ToQueryParam() => string.Format("{0}={1}", Name, Value);
    }
}