using System;
using UnityEngine;

namespace UnityStandardAssets.Effects
{
    public class WaterHoseParticles : MonoBehaviour
    {
        public static float lastSoundTime;
        public float force = 1;


        private ParticleCollisionEvent[] m_CollisionEvents = new ParticleCollisionEvent[16];
        private ParticleSystem m_ParticleSystem;


        private void Start()
        {
            m_ParticleSystem = GetComponent<ParticleSystem>();
        }

        [Obsolete]
        private void OnParticleCollision(GameObject other)
        {
            ParticleSystem m_ParticleSystem1 = m_ParticleSystem;
            int safeLength = m_ParticleSystem1.GetSafeCollisionEventSize();

            if (m_CollisionEvents.Length < safeLength)
            {
                m_CollisionEvents = new ParticleCollisionEvent[safeLength];
            }

            int numCollisionEvents = m_ParticleSystem1.GetCollisionEvents(other, m_CollisionEvents);
            int i = 0;

            while (i < numCollisionEvents)
            {
                if (Time.time > lastSoundTime + 0.2f)
                {
                    lastSoundTime = Time.time;
                }

                var col = m_CollisionEvents[i].colliderComponent;

                // Ensure col is of type Collider before accessing attachedRigidbody
                Collider collider = col as Collider;
                if (collider != null && collider.attachedRigidbody != null)
                {
                    Vector3 vel = m_CollisionEvents[i].velocity;
                    collider.attachedRigidbody.AddForce(vel * force, ForceMode.Impulse);
                }

                //if (col.attachedRigidbody != null)
                //{
                //    Vector3 vel = m_CollisionEvents[i].velocity;
                //    col.attachedRigidbody?.AddForce(vel * force, ForceMode.Impulse);
                //}

                other.SendMessage("Extinguish", SendMessageOptions.DontRequireReceiver);
                i++;

            }
        }
    }
}
