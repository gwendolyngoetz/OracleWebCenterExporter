using System.Collections.Generic;
using System.Linq;
using OracleWebCenterExporter.Extensions;
using OracleWebCenterExporter.Model;

namespace OracleWebCenterExporter.Services
{
    internal class SiteHierarchyTreeBuilder
    {
        internal SiteHierachyNode ToTree(List<SiteHierarchyItem> siteHierarchyItems)
        {
            var siteHierarchyItem = siteHierarchyItems.FirstOrDefault(x => x.Level == 0);

            if (siteHierarchyItem == null)
            {
                return null;
            }

            var rootNode = BuildNode(1, siteHierarchyItems, new SiteHierachyNode
            {
                NodeId = siteHierarchyItem.NodeId,
                Name = siteHierarchyItem.NamePath,
                Level = siteHierarchyItem.Level
            });

            return rootNode;
        }

        private SiteHierachyNode BuildNode(int level, List<SiteHierarchyItem> siteHierarchyItems, SiteHierachyNode parentNode)
        {
            var childNodes = new List<SiteHierachyNode>();

            var order = 1;

            siteHierarchyItems.Where(x => x.Level == level && x.NamePath.StartsWith(parentNode.Name)).Each(rawCell =>
            {
                childNodes.Add(new SiteHierachyNode
                {
                    NodeId = rawCell.NodeId,
                    Name = rawCell.NamePath,
                    Level = rawCell.Level,
                    Order = order
                });

                order++;
            });

            parentNode.Children.AddRange(childNodes);
            childNodes.Each(cell => BuildNode(level + 1, siteHierarchyItems, cell));

            return parentNode;
        }
    }
}