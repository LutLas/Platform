using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform.Services
{
    public static class TypeBroker
    {
        private static IResponseFormatter textFormatter = new TextResponseFormatter();
        public static IResponseFormatter TextFormatter => textFormatter;

        private static IResponseFormatter htmlFormatter = new HtmlResponseFormatter();
        public static IResponseFormatter HtmlFormatter => htmlFormatter;
    }
}
