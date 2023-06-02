using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablesManager : MonoBehaviour
{
    [System.Serializable]
    struct ObjectInfo
    {
        public Transform objInScene;
        public string viewText;
    }
}
