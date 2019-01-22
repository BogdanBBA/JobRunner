using CommonCode.DataLayer;
using System;
using System.Data.SQLite;
using System.Text;

namespace Jobs.WebstoreProducts.DTOs
{
    public class Last2ProductPricesDTO : BaseDTO
    {
        public ProductDTO Product { get; set; }
        public PriceDTO PriceA { get; set; }
        public PriceDTO PriceB { get; set; }

        public bool SomethingChanged
        {
            get
            {
                // case where (!A and B) shouldn't exist, only considering (!A and !B), (A and !B) and (A and B)
                if (PriceB == null) // !A and !B => there are no records => don't notify
                    return false;
                if (PriceA == null) // A and !B => there's only one record, probably means that it's a new product => notify
                    return true;
                // A and B => the 2 most recent prices => notify if the prices don't match
                return PriceA.PreviousPrice != PriceB.PreviousPrice || PriceA.CurrentPrice != PriceB.CurrentPrice;
            }
        }

        public bool HasCurrentPriceChange
            => CurrentPriceChangePercentage != 0.0;

        public bool HasPreviousPriceChange
            => PreviousPriceChangePercentage != 0.0;

        public double CurrentPriceChangePercentage
            => PriceA == null || PriceB == null ? 0.0 : (PriceB.CurrentPrice - PriceA.CurrentPrice) / PriceA.CurrentPrice;

        public double PreviousPriceChangePercentage
            => PriceA == null || PriceB == null ? 0.0 : (PriceB.PreviousPrice - PriceA.PreviousPrice) / PriceA.PreviousPrice;

        public string FormatOldPrice
            => FormatPriceAsHTML(PriceA);

        public string FormatNewPrice
            => FormatPriceAsHTML(PriceB);

        private static string FormatPriceAsHTML(PriceDTO price)
        {
            if (price == null)
                return "unknown";
            StringBuilder sb = new StringBuilder();
            sb.Append($"<b>{price.CurrentPrice:N2} {price.Currency}</b>");
            if (price.PreviousPrice == price.CurrentPrice)
                sb.Append(" (no reduction)");
            else if (price.PriceReduction != 0.0)
                sb.Append($" (reduced {-price.PriceReduction:N2} {price.Currency} / {-price.PriceReductionPercentage:P1} from {price.PreviousPrice:N2} {price.Currency})");
            return sb.ToString();
        }

        public string FormatPriceChange(string greenSpanClass, string redSpanClass)
        {
            if (PriceB == null)
                return "<b>unknown</b> (no prices)";
            if (PriceA == null)
                return "<b>none</b> (first price recording)";

            StringBuilder sb = new StringBuilder("<b>");
            sb.Append(HasCurrentPriceChange ? FormatPriceChangePercentage(CurrentPriceChangePercentage, greenSpanClass, redSpanClass) : "none");
            sb.Append("</b> (non-reduced: ");
            sb.Append(HasPreviousPriceChange ? FormatPriceChangePercentage(PreviousPriceChangePercentage, greenSpanClass, redSpanClass) : "none");
            return sb.Append(")").ToString();
        }

        private static string FormatPriceChangePercentage(double percentage, string greenSpanClass, string redSpanClass)
        {
            string text = percentage.ToString("P1");
            if (percentage < 0.0)
                return $"<span class=\"{greenSpanClass}\">{text}</span>";
            if (percentage > 0.0)
                return $"<span class=\"{redSpanClass}\">+{text}</span>";
            return text;
        }

        public static Last2ProductPricesDTO ParseSingleDTOFromList(ProductDTO product, SQLiteDataReader reader)
        {
            PriceDTO priceB = reader.Read() ? PriceDTO.ParseCurrent(reader) : null;
            PriceDTO priceA = reader.Read() ? PriceDTO.ParseCurrent(reader) : null;
            return new Last2ProductPricesDTO()
            {
                Product = product,
                PriceA = priceA,
                PriceB = priceB
            };
        }

        public override string ToSQL
            => throw new NotImplementedException("Last2ProductPricesDTO.ToSQL should not be called");
    }
}
