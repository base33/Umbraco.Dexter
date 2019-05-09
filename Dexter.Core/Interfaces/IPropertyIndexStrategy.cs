using Dexter.Core.Models.IndexStrategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexter.Core.Interfaces
{
    public interface IPropertyIndexStrategy
    {
        void Execute(IndexFieldEvent e);
    }
}
