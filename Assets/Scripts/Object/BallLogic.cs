using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using System.Collections;
using Photon.Pun.Demo.PunBasics;

namespace Com.MyCompany.MyGame
{
    public class BallLogic : MonoBehaviourPunCallbacks
    {
        public float m_duration = 10.0f;
        private float m_elapsed = float.MaxValue;
        private bool m_destroyed = false;

        public override void OnEnable()
        {
            base.OnEnable();
            m_destroyed = false;
            m_elapsed = float.MaxValue;
        }

        public void FixedUpdate()
        {
            if (m_destroyed)
            {
                return;
            }

            m_elapsed -= Time.deltaTime;
            if (m_elapsed <= 0)
            {
                PhotonNetwork.Destroy(this.gameObject);
                m_destroyed = true;
            }
        }

        public void ThrowAway(Vector3 force)
        {
            m_destroyed = false;
            m_elapsed = m_duration;
            Rigidbody rbody = this.gameObject.GetComponent<Rigidbody>();
            if (rbody != null)
            {
                rbody.AddForce(force, ForceMode.Impulse);
            }
        }
    }
}