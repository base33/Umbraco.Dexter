using Dexter.Core.Interfaces;
using Dexter.Core.Models.IndexStrategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexter.IndexStrategies.Content
{
    public class HideFromIndexStrategy : IContentIndexStrategy
    {
        public void Execute(IndexContentEvent e)
        {
            if (e.Content.GetValue<bool>("hideFromIndex"))
                e.Cancel = true;
        }
    }
}
