using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using EFT.Interactive;
namespace Hag.Esp
{
    class BaseGrenade : IDisposable
    {
        public BaseGrenade(Throwable grenade)
        {
            Grenade = grenade;
        }
        public Throwable Grenade;
        public Vector3 W2S;
        public int Distance;
        public void Dispose()
        { }
        public void SetOneTimeVars()
        { }
    }
}
