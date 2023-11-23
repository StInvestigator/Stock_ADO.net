using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_ADO.net
{
    public class Good
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Provider { get; set; }
        public int Count { get; set; } 
        public SqlMoney Price { get; set; }
        public DateTime Date { get; set; }
        public Good(string name, string type, string provider, int count, SqlMoney price, DateTime date) 
        {
            Name = name;
            Type = type;
            Provider = provider;
            Count = count;
            Price = price;
            Date = date;
        }

    }
}
