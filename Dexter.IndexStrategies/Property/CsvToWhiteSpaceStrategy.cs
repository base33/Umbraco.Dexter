using Dexter.Core.Interfaces;
using Dexter.Core.Models.IndexStrategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexter.IndexStrategies.Property
{
    public class CsvToWhiteSpaceStrategy : IPropertyIndexStrategy
    {
        public void Execute(IndexFieldEvent indexField)
        {
            var text = indexField.Value as string;
            indexField.Value = string.Join(" ", text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
