using System;
using Satisfy.Data;
using UnityEngine;

namespace Satisfy.Variables.Custom
{
    [Serializable, CreateAssetMenu(fileName = "Skin Data", menuName = "Lists/Skin Data")]
    public class SkinDataList : ListSO<SkinData>
    {

    }
}