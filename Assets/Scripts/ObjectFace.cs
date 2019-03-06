using UnityEngine;

namespace AstroNet.GameElements
{
    public enum FaceType {
        Square,
        Triangle,
        Circle
    }
    public class ObjectFace : MonoBehaviour
    {
        [SerializeField] private FaceType _type;

        public FaceType Type => _type;

        public void Explode()
        {
            gameObject.SetActive(false);
        }
    }
}