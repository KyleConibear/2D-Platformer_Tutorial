using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KyleConibear
{
    using static Logger;
    public class Parallax : MonoBehaviour
    {
        [SerializeField] private new Transform camera = null;
        [SerializeField] private Vector2 relativeMove = new Vector2(0.3f, 0.1f);
        [SerializeField] private bool isYLocked = false;

        // Start is called before the first frame update
        void Start()
        {
            if(camera == null)
            {
                camera = Camera.main.transform;
                Logger.Log(Type.Warning, "Camera Transform was null.\nMain Camera Transform is assigned instead.");
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (camera == null)
                return;

            if(isYLocked)
            {
                this.transform.position = new Vector2(camera.position.x * relativeMove.x,
                                                  this.transform.position.y);
            }
            else
            {
                this.transform.position = new Vector2(camera.position.x * relativeMove.x,
                                                                  camera.position.y * relativeMove.y);
            }            
        }
    }
}