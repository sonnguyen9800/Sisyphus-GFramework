using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Not completed. WIP

namespace SisyphusFramework
{
    
    public abstract partial class APool<T> : Node 
    {
        IPoolingItem<T> pool;
    }
}
