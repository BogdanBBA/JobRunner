using CommonCode.Utils;

namespace WebAPI.Controllers.FoodDelivery
{
    public static class FoodDeliveryUtils
    {
        public static string ProcessNameFilter(string filter)
        {
            filter = filter?.Cleanup();
            if (string.IsNullOrEmpty(filter) || filter.Equals("*"))
                return null;
            return filter;
        }
    }
}
