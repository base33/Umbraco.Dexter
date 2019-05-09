using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexter.Core.Models.Backoffice
{
    public class Index
    {
        public string Name { get; set; }
        public int DocumentsIndexed { get; set; }
        public int FieldsIndexed { get; set; }
    }
}
