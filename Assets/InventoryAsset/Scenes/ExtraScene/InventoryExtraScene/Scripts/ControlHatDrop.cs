using System.Collections.Generic;
using UnityEngine;

namespace InventorySampleScene
{
    public class ControlHatDrop : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField]
        private List<GameObject> hats;

        [SerializeField]
        private Transform pos1;
        [SerializeField]
        private Transform pos2;
        private GameObject curObject;

        // Update is called once per frame
        void Update()
        {
            if (curObject == null)
            {
                curObject = Instantiate(hats[Random.Range(0, hats.Count)],
                    new Vector3(Random.Range(pos1.position.x, pos2.position.x), pos1.position.y, pos1.position.z), Quaternion.identity);
            }
        }
    }
}
