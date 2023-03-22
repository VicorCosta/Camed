using Camed.SSC.Core.Enums;

namespace Camed.SSC.Core.Data
{
    public class OrderByInfo
    {
        public string PropertyName { get; set; }
        public SortDirection Direction { get; set; }
        public bool Initial { get; set; }
    }
}
