using System.Xml.Serialization;

namespace App2.View
{
    [XmlType("Table")]
    public  class FiltrSQLs
    {
        public string FiltrSQL{ get; set; }
    }
}