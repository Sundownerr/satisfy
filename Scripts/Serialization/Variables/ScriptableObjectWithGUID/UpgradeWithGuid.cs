using System;
using Gongulus.Game;

namespace Satisfy.Data
{
    [Serializable]
    public class UpgradeWithGuid : ScriptableObjectWithGuid<Upgrade>
    {
        public UpgradeWithGuid(Upgrade data, string guid) : base(data, guid)
        {
        }
    }
}