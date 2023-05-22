using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using System.Collections;
using Photon.Pun.Demo.PunBasics;

namespace Com.MyCompany.MyGame
{
    public class PetLogic : MonoBehaviourPunCallbacks
    {
        public float m_distance = 2.0f;
        private PlayerManager m_owner;
        private bool m_isFollowing = false;

        public void InitPet(PlayerManager _owner)
        {
            this.m_owner = _owner;
        }

        public void Update()
        {
            if (m_isFollowing)
            {

            }
            else
            {
                float dis = Vector3.Distance(this.m_owner.gameObject.transform.position, this.gameObject.transform.position);
                if (dis > m_distance)
                {
                    m_isFollowing = true;
                }
            }
        }
    }
}