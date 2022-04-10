using System;

namespace Api.Data.ServiceModels
{
    public class GetForexRequest
    {
        public string CurrencyCode { get; set; }
        public DateTime ReferenceDate { get; set; }
        public string ReferenceType { get; set; }
    }

}
