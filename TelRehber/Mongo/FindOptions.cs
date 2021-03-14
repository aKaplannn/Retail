using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TelRehber.Mongo
{
    public class SortDefinitionItem
    {
        public string FieldName { get; set; }

        public bool Descending { get; set; } = false;
    }


    public class FindOptions
    {
        public int Skip { get; set; } = 0;
        public int Limit { get; set; } = 0;
        public SortDefinitionItem[] SortDefinition { get; set; } = null;
    }
}
