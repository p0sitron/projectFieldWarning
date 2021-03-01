/**
 * Copyright (c) 2017-present, PFW Contributors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in
 * compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software distributed under the License is
 * distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See
 * the License for the specific language governing permissions and limitations under the License.
 */

using UnityEngine;

namespace PFW.UI.Ingame
{
    /// <summary>
    /// A billboard is a 2d texture that is always facing the camera.
    /// </summary>
    public class BillboardBehavior : MonoBehaviour
    {
        [SerializeField]
        private float ALTITUDE;
        [SerializeField]
        private float SIZE = 0.1f;

        private void OnEnable()
        {
            // HACK I don't know why, but setting this on declaration simply wasn't working, so I moved it here.
            // Nothing else touches this, so I can only assume it is either a Unity bug or a magical network issue.
            ALTITUDE = 10f * Constants.MAP_SCALE;
        }

        private void Update()
        {
            transform.localPosition = ALTITUDE * Camera.main.transform.up;
            FaceCamera();
        }

        private void FaceCamera()
        {
            transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
            float distance = (Camera.main.transform.position - transform.position).magnitude;
            transform.localScale = SIZE * distance * Vector3.one;
        }
    }
}
