﻿using System.Collections.Generic;
using UnityEngine;

namespace AstroNet.GameElements
{
    public class AssembledObject : MonoBehaviour
    {
        [SerializeField] List<Transform> _faces;
        // Start is called before the first frame update
        public void Attack(FaceType type)
        {
            foreach(var face in _faces)
            {
                if (face.gameObject.activeSelf)
                {
                    var objectFace = face.GetComponent<ObjectFace>();
                    if(objectFace.Type == type)
                    {
                        objectFace.Explode();
                        break;
                    }
                }
            }
        }
    }
}