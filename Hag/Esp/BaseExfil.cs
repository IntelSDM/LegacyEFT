using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using EFT.Interactive;
namespace Hag.Esp
{
    class BaseExfil : IDisposable
    {
        public BaseExfil(ExfiltrationPoint exfil)
        {
            Exfil = exfil;
        }
        public ExfiltrationPoint Exfil;
        public Vector3 W2S;
        public int Distance;
        public string Name;
        public Color32 Colour;
        public void Dispose()
        { }
        public void SetOneTimeVars()
        { }
    }
}
