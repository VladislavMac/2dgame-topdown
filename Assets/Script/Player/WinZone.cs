using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class WinZone : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<PlayerController>())
            {
                collision.GetComponent<PlayerController>().SetWinPlane();
            }
        }
    }
}
