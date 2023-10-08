using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace WebApiLib.Model
{
    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class SoapEnvelope
    {
        [XmlElement(ElementName = "Body")]
        public SoapBody Body { get; set; }
    }

    public class SoapBody
    {
        [XmlElement(ElementName = "ExecuteSQLCommandResponse", Namespace = "http://www.comarch.pl/cdn/Products/CDN XL/")]
        public ExecuteSQLCommandResponse ExecuteSQLCommandResponse { get; set; }
    }

    public class ExecuteSQLCommandResponse
    {
        [XmlElement(ElementName = "ExecuteSQLCommandResult")]
        public string ExecuteSQLCommandResult { get; set; }
    }
}
