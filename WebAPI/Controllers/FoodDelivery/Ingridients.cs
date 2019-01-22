using System;
using System.Collections.Generic;

namespace WebAPI.Controllers.FoodDelivery
{
    public enum FoodSource
    {
        Vegan = 1,
        AnimalNonLethal = 2,
        AnimalLethal = 3
    }

    public enum Ingridients
    {
        Aluat,
        Paste,
        Ulei,
        Oua,

        ArdeiGras,
        ArdeiIute,
        Capere,
        CartofiPrajiti,
        Castraveti,
        Ceapa,
        Ciuperci,
        Masline,
        Porumb,
        Praz,
        Rosii,
        RosiiCherry,
        SalataVerde,
        SosDeRosii,
        Telina,
        Usturoi,
        MixFructe,

        Ananas,
        Mere,
        Morcovi,

        Busuioc,
        Rucola,
        Tabasco,

        VinAlb,
        VinRosu,

        Feta,
        Gorgonzola,
        Iaurt,
        Mozzarella,
        ParmigianoReggiano,
        Ricotta,
        Schweitzer,
        Smantana,
        Telemea,

        Bacon,
        Cabanos,
        MuschiFile,
        PieptDePui,
        Prosciutto,
        Salam,
        SalamPicant,
        SuncaPraga,

        FructeDeMare,
        Hamsii,
        Sardine,
        Ton
    }

    public class IngridientProperties
    {
        public Ingridients Ingridient { get; private set; }
        public string StandardName { get; private set; }
        public List<string> AlternativeNames { get; private set; }
        public FoodSource Source { get; private set; }

        public IngridientProperties(Ingridients ingridient, string standardName, FoodSource source, params string[] alternativeNames)
        {
            Ingridient = ingridient;
            StandardName = standardName;
            AlternativeNames = new List<string>(alternativeNames);
            Source = source;
        }
    }

    public class IngridientsOrganizer : BaseOrganizer
    {
        private static IngridientsOrganizer _instance;
        public static IngridientsOrganizer Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new IngridientsOrganizer();
                return _instance;
            }
        }

        private IngridientsOrganizer()
            : base()
        {
        }

        public Dictionary<Ingridients, IngridientProperties> Properties { get; private set; }

        protected override void Initialize()
        {
            Properties = new Dictionary<Ingridients, IngridientProperties>();
            foreach (IngridientProperties item in new IngridientProperties[]
            {
                new IngridientProperties(Ingridients.Aluat, "Aluat", FoodSource.AnimalNonLethal, "aluat"),
                new IngridientProperties(Ingridients.Paste, "Paste", FoodSource.AnimalNonLethal, "paste", "spaghetti", "spaghete", "tortellini", "tortelini", "penne", "pene", "tagliatelle", "tagliatele"),
                new IngridientProperties(Ingridients.Ulei, "Ulei", FoodSource.Vegan, "ulei", "ulei masline", "ulei de masline"),
                new IngridientProperties(Ingridients.Oua, "Oua", FoodSource.AnimalNonLethal, "ou", "oua"),

                new IngridientProperties(Ingridients.ArdeiGras, "Ardei gras", FoodSource.Vegan, "ardei gras", "ardei"),
                new IngridientProperties(Ingridients.ArdeiIute, "Ardei iute", FoodSource.Vegan, "ardei iute", "chili", "chilli", "paprika"),
                new IngridientProperties(Ingridients.Capere, "Capere", FoodSource.Vegan, "capere"),
                new IngridientProperties(Ingridients.CartofiPrajiti, "Cartofi prajiti", FoodSource.Vegan, "cartofi prajiti"),
                new IngridientProperties(Ingridients.Castraveti, "Castraveti", FoodSource.Vegan, "castraveti"),
                new IngridientProperties(Ingridients.Ceapa, "Ceapa", FoodSource.Vegan, "ceapa"),
                new IngridientProperties(Ingridients.Ciuperci, "Ciuperci", FoodSource.Vegan, "ciuperci"),
                new IngridientProperties(Ingridients.Masline, "Masline", FoodSource.Vegan, "masline"),
                new IngridientProperties(Ingridients.Porumb, "Porumb", FoodSource.Vegan, "porumb"),
                new IngridientProperties(Ingridients.Praz, "Praz", FoodSource.Vegan, "praz"),
                new IngridientProperties(Ingridients.Rosii, "Rosii", FoodSource.Vegan, "rosii"),
                new IngridientProperties(Ingridients.RosiiCherry, "Rosii cherry", FoodSource.Vegan, "rosii cherry"),
                new IngridientProperties(Ingridients.SalataVerde, "Salata verde", FoodSource.Vegan, "salata", "salata verde"),
                new IngridientProperties(Ingridients.SosDeRosii, "Sos de rosii", FoodSource.Vegan, "sos de rosii", "sos rosii", "sos de tomate", "sos tomate"),
                new IngridientProperties(Ingridients.Telina, "Telina", FoodSource.Vegan, "telina"),
                new IngridientProperties(Ingridients.Usturoi, "Usturoi", FoodSource.Vegan, "usturoi"),
                new IngridientProperties(Ingridients.MixFructe, "Mix de fructe", FoodSource.Vegan, "mix fructe", "fructe asortate"),

                new IngridientProperties(Ingridients.Ananas, "Ananas", FoodSource.Vegan, "ananas"),
                new IngridientProperties(Ingridients.Mere, "Mere", FoodSource.Vegan, "mere", "mar"),
                new IngridientProperties(Ingridients.Morcovi, "Morcovi", FoodSource.Vegan, "morcovi", "morcov"),

                new IngridientProperties(Ingridients.Busuioc, "Busuioc", FoodSource.Vegan, "busuioc"),
                new IngridientProperties(Ingridients.Rucola, "Rucola", FoodSource.Vegan, "rucola", "rucolla"),
                new IngridientProperties(Ingridients.Tabasco, "Tabasco", FoodSource.Vegan, "tabasco"),

                new IngridientProperties(Ingridients.VinAlb, "Vin alb", FoodSource.Vegan, "vin alb"),
                new IngridientProperties(Ingridients.VinRosu, "Vin rosu", FoodSource.Vegan, "vin rosu", "vin"),

                new IngridientProperties(Ingridients.Feta, "Feta", FoodSource.AnimalNonLethal, "feta", "branza feta"),
                new IngridientProperties(Ingridients.Gorgonzola, "Gorgonzola", FoodSource.AnimalNonLethal, "gorgonzola", "gorgonzolla", "gorgonzzola", "gorgonzzolla"),
                new IngridientProperties(Ingridients.Iaurt, "Iaurt", FoodSource.AnimalNonLethal, "iaurt", "dresing iaurt"),
                new IngridientProperties(Ingridients.Mozzarella, "Mozzarella", FoodSource.AnimalNonLethal, "mozarela", "mozarella", "mozzarela", "mozzarella"),
                new IngridientProperties(Ingridients.ParmigianoReggiano, "ParmigianoReggiano", FoodSource.AnimalNonLethal, "parmigiano-reggiano", "parmezan"),
                new IngridientProperties(Ingridients.Ricotta, "Ricotta", FoodSource.AnimalNonLethal, "ricotta", "ricota"),
                new IngridientProperties(Ingridients.Schweitzer, "Schweitzer", FoodSource.AnimalNonLethal, "schweitzer", "svaiter"),
                new IngridientProperties(Ingridients.Smantana, "Smantana", FoodSource.AnimalNonLethal, "smantana"),
                new IngridientProperties(Ingridients.Telemea, "Telemea", FoodSource.AnimalNonLethal, "telemea"),

                new IngridientProperties(Ingridients.Bacon, "Bacon", FoodSource.AnimalLethal, "bacon"),
                new IngridientProperties(Ingridients.Cabanos, "Cabanos", FoodSource.AnimalLethal, "cabanos"),
                new IngridientProperties(Ingridients.MuschiFile, "Muschi filé", FoodSource.AnimalLethal, "muschi file", "muschi"),
                new IngridientProperties(Ingridients.PieptDePui, "Piept de pui", FoodSource.AnimalLethal, "piept de pui", "pui piept", "piept pui"),
                new IngridientProperties(Ingridients.Prosciutto, "Prosciutto", FoodSource.AnimalLethal, "prosciutto", "prosciutto crudo", "prosciuto"),
                new IngridientProperties(Ingridients.Salam, "Salam", FoodSource.AnimalLethal, "salam"),
                new IngridientProperties(Ingridients.SalamPicant, "Salam picant", FoodSource.AnimalLethal, "salam picant"),
                new IngridientProperties(Ingridients.SuncaPraga, "Sunca Praga", FoodSource.AnimalLethal, "sunca praga", "sunca de praga", "sunca"),

                new IngridientProperties(Ingridients.FructeDeMare, "Fructe de mare", FoodSource.AnimalLethal, "fructe de mare", "fructe mare"),
                new IngridientProperties(Ingridients.Hamsii, "Hamsii", FoodSource.AnimalLethal, "hamsii", "anchoa", "ansoa", "anchovy", "anchovies"),
                new IngridientProperties(Ingridients.Sardine, "Sardine", FoodSource.AnimalLethal, "sardine"),
                new IngridientProperties(Ingridients.Ton, "Ton", FoodSource.AnimalLethal, "ton", "salata ton", "salata de ton")
            })
            {
                Properties.Add(item.Ingridient, item);
            }
        }

        protected override void Check()
        {
            if (Properties == null)
                throw new NotImplementedException("Ingridient properties dictionary is not initialized");
            foreach (Ingridients value in Enum.GetValues(typeof(Ingridients)))
                if (!Properties.ContainsKey(value))
                    throw new NotImplementedException($"Ingridient value '{value}' is not initialized with properties");
        }
    }
}
