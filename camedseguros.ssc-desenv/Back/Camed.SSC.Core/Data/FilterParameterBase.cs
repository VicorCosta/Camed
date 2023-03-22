namespace Camed.SSC.Core.Data
{
    public abstract class FilterParameterBase
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = AppConstants.PageSize;
    }
}
