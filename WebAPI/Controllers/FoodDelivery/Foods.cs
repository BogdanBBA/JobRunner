using CommonCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebAPI.Controllers.FoodDelivery
{
    public class Food
    {
        public string Name { get; private set; }
        internal string SearchName { get; private set; }
        public Restaurants Restaurant { get; private set; }
        public List<Ingridients> Ingridients { get; private set; }

        //public Food(string name, Restaurants restaurant, params Ingridients[] ingridients)
        //{
        //    Name = ProcessName(name);
        //    SearchName = Name.Cleanup();
        //    Restaurant = restaurant;
        //    Ingridients = new List<Ingridients>(ingridients);
        //}

        public Food(Restaurants restaurant, string name, string ingridientsList, string separator = ",")
        {
            Name = ProcessName(name);
            SearchName = Name.Cleanup();
            Restaurant = restaurant;
            Ingridients = ParseIngridientsFromString(ingridientsList, separator);
        }

        public FoodSource WorstFoodSource
            => (FoodSource)Ingridients.Select(ingridient => (int)IngridientsOrganizer.Instance.Properties[ingridient].Source).Max();

        private static string ProcessName(string name)
        {
            return name.Trim();
        }

        private static List<Ingridients> ParseIngridientsFromString(string ingridientsList, string separator)
        {
            List<Ingridients> result = new List<Ingridients>();
            string[] ingridientNames = ingridientsList.Cleanup()
                .Replace("dresing", "dressing").Replace("dressing", "")
                .Split(separator, StringSplitOptions.RemoveEmptyEntries);
            foreach (string ingridientName in ingridientNames)
            {
                string name = ProcessName(ingridientName);
                bool found = false;
                foreach (IngridientProperties ingridient in IngridientsOrganizer.Instance.Properties.Values)
                    if (ingridient.AlternativeNames.Contains(name))
                    {
                        result.Add(ingridient.Ingridient);
                        found = true;
                        break;
                    }
                if (!found)
                    throw new FormatException($"unknown ingridient '{name}'");
            }
            return result;
        }
    }

    public class FoodsOrganizer : BaseOrganizer
    {
        private static FoodsOrganizer _instance;
        public static FoodsOrganizer Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new FoodsOrganizer();
                return _instance;
            }
        }

        public List<Food> AllFoods { get; private set; }

        private FoodsOrganizer()
            : base()
        {
        }

        protected override void Initialize()
        {
            AllFoods = new List<Food>();
            AllFoods.AddRange(GetFoodForNapoli());
        }

        private List<Food> GetFoodForNapoli()
        {
            const Restaurants restaurant = Restaurants.Napoli;
            return new List<Food>()
            {
                new Food(restaurant, "PIZZA MARGHERITA", "Sos de roşii, mozzarella"),
                new Food(restaurant, "PIZZA PROSCIUTTO", "Sos de roşii, mozzarella , şuncă Praga"),
                new Food(restaurant, "PIZZA PROSCIUTTO E FUNGHI", "Sos de roşii, mozzarella, şuncă Praga, ciuperci"),
                new Food(restaurant, "PIZZA CAPRICCIOSA", "Sos de roşii, mozzarella, şuncă Praga, ciuperci, masline, ardei gras"),
                new Food(restaurant, "PIZZA QUATTRO STAGIONI", "Sos de roşii, mozzarella, suncă Praga, salam, porumb, ciuperci, masline, ardei gras"),
                new Food(restaurant, "PIZZA CINQUE FORMAGGI", "Mozzarella, ricota, şvaiţer, gorgonzola, parmezan"),
                new Food(restaurant, "PIZZA ALSACIA", "Mozzarella, smântână, bacon, şuncă Praga, praz"),
                new Food(restaurant, "PIZZA DIAVOLA", "Sos de roşii, mozzarella, salam picant, chili"),
                new Food(restaurant, "PIZZA SALAMI", "Sos de roşii, mozzarella, salam, salam picant, ciuperci, măsline"),
                new Food(restaurant, "PIZZA TONNO", "Sos de roşii, mozzarella, ton, ceapă"),
                new Food(restaurant, "PIZZA CALZONI", "Sos de roşii, mozzarella, şuncă Praga, ciuperci"),
                new Food(restaurant, "PIZZA VEGETARIANA", "Sos de roşii, mozzarella, ciuperci, ardei gras, măsline, porumb, roşii"),
                new Food(restaurant, "PIZZA NAPOLI", "Sos de roşii, mozzarella, ciuperci, şuncă Praga, sardine, anchoa, măsline, capere, ceapă"),
                new Food(restaurant, "PIZZA SICILIANA", "Sos de roşii, mozzarella, ciuperci, cabanos, porumb, roşii"),
                new Food(restaurant, "PIZZA DE POST", "Sos de roşii, ciuperci, măsline, ardei gras, porumb, roşii"),
                new Food(restaurant, "PIZZA HAWAII", "Sos de roşii, mozzarella, şuncă Praga, ananas"),
                new Food(restaurant, "PIZZA TROPICALA", "Sos de roşii, mozzarella, ton, fructe asortate"),
                new Food(restaurant, "PIZZA AFUMICATO", "Sos de roşii, mozzarella, cabanos, muşchi file, şuncă Praga, bacon, ou"),
                new Food(restaurant, "PIZZA PALERMO", "Sos de roşii, mozzarella, ciuperci, muşchi file, porumb, măsline, roşii"),
                new Food(restaurant, "PIZZA POLLO", "Sos rosii, mozzarella, ciuperci, piept de pui, porumb, rosii"),
                new Food(restaurant, "PIZZA CASEI", "Mozzarella, piept de pui, cartofi prajiţi, usturoi"),
                new Food(restaurant, "PIZZA CU FRUCTE DE MARE", "Sos de roşii, mozzarella, fructe de mare"),
                new Food(restaurant, "PIZZA URSULET (PENTRU COPII)", "Sos de roşii, mozzarella, salam, măsline, porumb"),
                new Food(restaurant, "PIZZA PROSCIUTTO CRUDO", "Sos rosii, mozzarella, prosciutto crudo, rucolla, rosii cherry, parmezan"),
                new Food(restaurant, "PIZZA NAPOLI GIGANT", "Sos de roşii, mozzarella, şuncă Praga, bacon, cabanos, ciuperci, porumb, măsline, ardei gras"),
                new Food(restaurant, "TORTELLINI PANNA E PROSCIUTTO", "Tortellini, şuncă Praga, smântână, ouă, parmezan"),
                new Food(restaurant, "TAGLIATELLE CU FRUCTE DE MARE", "Tagliatelle, fructe de mare, usturoi, vin alb, ulei de măsline"),
                new Food(restaurant, "TAGLIATELLE PROSCIUTTO E FUNGHI", "Tagliatelle, şuncă Praga, ciuperci, smântână, parmezan"),
                new Food(restaurant, "PENNE CON POLLO", "Penne, piept de pui, ardei, măsline, busuioc"),
                new Food(restaurant, "PENNE QUATTRO FORMAGGI", "Penne, gorgonzola, mozzarella, şvaiţer, smântână"),
                new Food(restaurant, "PENNE POMODORI", "Penne, roşii, ceapă, usturoi, busuioc, parmezan"),
                new Food(restaurant, "PENNE ALL'ARRABBIATA", "Penne, anchoa, roşii, măsline, capere, ceapă, usturoi, tabasco"),
                new Food(restaurant, "SPAGHETTI CU PUI ŞI CIUPERCI", "Spaghete, piept de pui, ciuperci, smântână, parmezan"),
                new Food(restaurant, "SPAGHETTI MILANEZE", "Spaghete, şuncă Praga, roşii, ciuperci, parmezan"),
                new Food(restaurant, "SPAGHETTI CU TON", "Spaghete, ton, sos rosii, parmezan,ceapa"),
                new Food(restaurant, "SPAGHETTI CARBONARA", "Spaghete, bacon, smântână, oua, parmezan"),
                new Food(restaurant, "SALATA DE CRUDITATI", "Mar, morcov, telina"),
                new Food(restaurant, "SALATĂ CAPRESE", "Mozzarella, roşii, busuioc, ulei de măsline"),
                new Food(restaurant, "SALATĂ BULGĂREASCĂ", "Ardei gras, castraveţi, roşii, ouă, măsline, salată verde, telemea, ceapă, dresing iaurt"),
                new Food(restaurant, "SALATĂ GRECEASCĂ", "Ardei gras, castraveţi, roşii, brânză feta, măsline, salată verde, dresing iaurt"),
                new Food(restaurant, "SALATĂ NAPOLI", "Salată verde, şuncă Praga, ardei gras, castraveţi, roşii, ouă, măsline, dresing iaurt"),
                new Food(restaurant, "SALATĂ CU PIEPT DE PUI", "Salată verde, piept de pui, şuncă Praga, ardei gras, castraveţi, roşii, dresing iaurt"),
                new Food(restaurant, "SALATĂ CU TON", "Salată verde, ton, ardei gras, castraveţi, roşii, ceapă, măsline")
            };
        }

        protected override void Check()
        {
            // nothing to do?
        }

        public List<Food> Filter(string restaurantName = null, string foodName = null, FoodSource? worstFoodSource = null, List<Ingridients> mustHave = null, List<Ingridients> mustNotHave = null)
        {
            IEnumerable<Food> result = new List<Food>(AllFoods).Select(food => food);
            Dictionary<Restaurants, RestaurantProperties> restaurants = RestaurantsOrganizer.Instance.Properties;

            if (restaurantName != null)
                result = result.Where(food => restaurants[food.Restaurant].Name.ToLowerInvariant().Contains(restaurantName));

            if (foodName != null)
                result = result.Where(food => food.SearchName.Contains(foodName));

            if (worstFoodSource.HasValue)
                result = result.Where(food => (int)food.WorstFoodSource <= (int)worstFoodSource);

            if (mustHave?.Count > 0)
                result = result.Where(food => food.Ingridients.ContainsAll(mustHave));

            if (mustNotHave?.Count > 0)
                result = result.Where(food => food.Ingridients.ContainsNone(mustNotHave));

            return result.ToList();
        }
    }
}
