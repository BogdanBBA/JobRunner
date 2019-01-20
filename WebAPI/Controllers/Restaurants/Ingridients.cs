using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers.Restaurants
{
    public class IngridientProperties
    {

    }

    public static class IngridientOrganizer
    {
        public static Dictionary<Ingridients, IngridientProperties> Properties { get; private set; }

        static IngridientOrganizer()
        {
            Initialize();
            Check();
        }

        private static void Initialize()
        {

        }

        private static void Check()
        {

        }
    }
}
