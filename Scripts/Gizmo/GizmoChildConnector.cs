using System.Linq;
using Satisfy.Attributes;
using Sirenix.OdinInspector;
using Unity.Linq;
using UnityEngine;

namespace Gongulus.Game
{
    [HideMonoScript]
    public class GizmoChildConnector : MonoBehaviour
    {
        [SerializeField] [Tweakable] private bool draw = true;
        [SerializeField] [Tweakable] private Color color;
        [SerializeField] [Tweakable] private float pointSize = 0.04f;

        private void OnDrawGizmos()
        {
            if (!draw)
                return;

            Gizmos.color = color;

            gameObject.Children().Aggregate((x, y) =>
            {
                Gizmos.DrawLine(x.transform.position, y.transform.position);
                Gizmos.DrawSphere(y.transform.position, pointSize);
                return y;
            });
        }
    }
}