using CommonCode;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WebAPI.Controllers.FoodDelivery;

namespace WebAPI.Controllers
{
    public class FoodDeliveryControllerParameterWrapper
    {
        public List<string> Restaurants { get; set; }
    }

    [Route("api/food-delivery")]
    public class FoodDeliveryController : Controller
    {
        [Route("list-restaurants")]
        [HttpGet]
        public APIResult<Dictionary<int, string>> ListRestaurants()
        {
            try
            {
                Dictionary<int, string> result = new Dictionary<int, string>();
                Dictionary<Restaurants, RestaurantProperties> properties = RestaurantsOrganizer.Instance.Properties;
                properties.Keys.ToList().ForEach(key => result.Add((int)key, $"{properties[key].Name} ({properties[key].City})"));
                return new APIResult<Dictionary<int, string>>(result);
            }
            catch (Exception e)
            {
                return new APIResult<Dictionary<int, string>>(null, e.ToString());
            }
        }

        [Route("list-ingridients")]
        [HttpGet]
        public APIResult<Dictionary<int, string>> ListIngridients()
        {
            try
            {
                Dictionary<int, string> result = new Dictionary<int, string>();
                Dictionary<Ingridients, IngridientProperties> properties = IngridientsOrganizer.Instance.Properties;
                properties.Keys.ToList().ForEach(key => result.Add((int)key, $"{properties[key].StandardName} ({properties[key].Source})"));
                return new APIResult<Dictionary<int, string>>(result);
            }
            catch (Exception e)
            {
                return new APIResult<Dictionary<int, string>>(null, e.ToString());
            }
        }

        [Route("list-food-sources")]
        [HttpGet]
        public APIResult<Dictionary<int, string>> ListFoodSources()
        {
            try
            {
                Dictionary<int, string> result = new Dictionary<int, string>();
                foreach (FoodSource item in Enum.GetValues(typeof(FoodSource)))
                    result.Add((int)item, item.ToString());
                return new APIResult<Dictionary<int, string>>(result);
            }
            catch (Exception e)
            {
                return new APIResult<Dictionary<int, string>>(null, e.ToString());
            }
        }

        [Route("filter-food/restaurant-name={restaurantName}&food-name={foodName}&worst-food-source={worstFoodSourceValue}&must-have={mustHaveList}&must-not-have={mustNotHaveList}")]
        [HttpGet]
        public APIResult<List<Food>> FilterFood(string restaurantName, string foodName, int worstFoodSourceValue, string mustHaveList, string mustNotHaveList)
        {
            try
            {
                FoodSource foodSource = (FoodSource)worstFoodSourceValue;
                List<Ingridients> mustHave = ParseIngridients(mustHaveList);
                List<Ingridients> mustNotHave = ParseIngridients(mustNotHaveList);

                List<Food> result = FoodsOrganizer.Instance.Filter(
                    FoodDeliveryUtils.ProcessNameFilter(restaurantName),
                    FoodDeliveryUtils.ProcessNameFilter(foodName),
                    foodSource,
                    mustHave,
                    mustNotHave
                    );

                return new APIResult<List<Food>>(result);
            }
            catch (Exception e)
            {
                return new APIResult<List<Food>>(null, e.ToString());
            }
        }

        private static List<Ingridients> ParseIngridients(string list)
        {
            if (FoodDeliveryUtils.ProcessNameFilter(list) == null)
                return null;
            return new List<Ingridients>(
                list.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                    .Select(p => (Ingridients)int.Parse(p)));
        }
    }
}