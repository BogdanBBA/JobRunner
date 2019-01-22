using System;
using System.Collections.Generic;

namespace WebAPI.Controllers.FoodDelivery
{
    public enum Restaurants
    {
        Napoli = 1
    }

    public class RestaurantProperties
    {
        public Restaurants Restaurant { get; private set; }
        public string Name { get; private set; }
        public string City { get; private set; }

        public RestaurantProperties(Restaurants restaurant, string name, string city)
        {
            Restaurant = restaurant;
            Name = name;
            City = city;
        }
    }

    public class RestaurantsOrganizer : BaseOrganizer
    {
        private static RestaurantsOrganizer _instance;
        public static RestaurantsOrganizer Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new RestaurantsOrganizer();
                return _instance;
            }
        }

        public Dictionary<Restaurants, RestaurantProperties> Properties { get; private set; }

        private RestaurantsOrganizer()
            : base()
        {
        }

        protected override void Initialize()
        {
            Properties = new Dictionary<Restaurants, RestaurantProperties>();
            foreach (RestaurantProperties item in new RestaurantProperties[]
            {
                new RestaurantProperties(Restaurants.Napoli, "Pizza Napoli", "Brașov, RO")
            })
            {
                Properties.Add(item.Restaurant, item);
            }
        }

        protected override void Check()
        {
            if (Properties == null)
                throw new NotImplementedException("Restaurant properties dictionary is not initialized");
            foreach (Restaurants value in Enum.GetValues(typeof(Restaurants)))
                if (!Properties.ContainsKey(value))
                    throw new NotImplementedException($"Restaurant value '{value}' is not initialized with properties");
        }
    }
}
