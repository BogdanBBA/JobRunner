using Jobs.WebstoreProducts.Parsing.Parsers;
using System;
using System.Text.RegularExpressions;

namespace Jobs.WebstoreProducts.Parsing
{
    public static class ParserFactory
    {
        public static Parser GetParser(string url)
        {
            string domain = Regex.Match(url.Trim().ToLowerInvariant(), @"^(?:https?:\/\/)?(?:[^@\n]+@)?(?:www\.)?([^:\/\n?]+)").Groups[1].Value;
            switch (domain)
            {
                case "jysk.ro":
                    return new Jysk();
                case "emag.ro":
                    return new EMag();
                case "mediagalaxy.ro":
                    return new MediaGalaxy();
                case "pcgarage.ro":
                    return new PcGarage();
                case "microsoft.com":
                    return new MicrosoftStore();
                default:
                    throw new ApplicationException($"ParserFactory.GetParser() ERROR: unknown domain in URL {url}");
            }
        }
    }
}
