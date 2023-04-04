using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpargoTest.Interfaces
{
    public interface IConsole<T>
    {
        T Get();
    }
}
