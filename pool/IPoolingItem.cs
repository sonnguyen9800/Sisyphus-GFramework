using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Not completed. WIP
namespace SisyphusFramework
{
    internal interface IPoolingItem<in T>
    {
        public string Name { get; set; }
        public string Setup(T param);
    }
}
