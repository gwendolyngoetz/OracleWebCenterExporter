using System.Collections.Generic;
using System.Linq;
using OracleWebCenterExporter.Extensions;

namespace OracleWebCenterExporter.Model
{
    public class SiteHierachyNode
    {
        public int NodeId { get; set; }
        public int Level { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public List<SiteHierachyNode> Children { get; set; } = new List<SiteHierachyNode>();

        public SiteHierachyNode Search(string name)
        {
            return Name.EqualsInsensitive(name) ? this : Search(this, name);
        }

        private SiteHierachyNode Search(SiteHierachyNode parent, string name)
        {
            var match = parent.Children.FirstOrDefault(x => x.Name.EqualsInsensitive(name));

            if (match != null)
            {
                return match;
            }

            foreach (var child in parent.Children)
            {
                var result = Search(child, name);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
    }
}