using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BNG {

    /// <summary>
    /// This collider will Damage a Damageable object on impact
    /// </summary>
    public class DamageCollider : MonoBehaviour {

        /// <summary>
        /// 데미지 양
        /// </summary>
        public float Damage = 25f;

        /// <summary>
        /// 이 충돌체의 속도를 결정하는 데 사용됩니다.
        /// </summary>
        public Rigidbody ColliderRigidbody;

        /// <summary>
        /// 손상을 입히는 데 필요한 최소한의 힘. relativeVelocity.magnitude로 표현됩니다.
        /// </summary>
        public float MinForce = 0.1f;

        /// <summary>
        /// 이전 프레임의 마지막 상대 속도 값
        /// </summary>
        public float LastRelativeVelocity = 0;

        // 충돌 입력 시 마지막으로 적용된 충격력의 양
        public float LastDamageForce = 0;

        /// <summary>
        /// 이 콜라이더가 무언가와 충돌하면 데미지를 입어야하는지 여부, ex.책상에서 떨어지면 박스가 부서짐
        /// </summary>
        public bool TakeCollisionDamage = false;

        /// <summary>
        /// 빠른 속도로 물체와 충돌할 경우 적용할 피해량
        /// </summary>
        public float CollisionDamage = 5;
        public bool isTouch;

        Damageable thisDamageable;

        private void Start() {
            if (ColliderRigidbody == null) {
                ColliderRigidbody = GetComponent<Rigidbody>();
            }

            thisDamageable = GetComponent<Damageable>();
        }

        private void OnCollisionEnter(Collision collision) {

            if(!this.isActiveAndEnabled) {
                return;
            }

            OnCollisionEvent(collision);
        }

        public virtual void OnCollisionEvent(Collision collision) {
            LastDamageForce = collision.impulse.magnitude;
            LastRelativeVelocity = collision.relativeVelocity.magnitude;

            if (LastDamageForce >= MinForce) {

                // Can we damage what we hit?
                Damageable d = collision.gameObject.GetComponent<Damageable>();
                DamageablePart damagePart = collision.gameObject.GetComponent<DamageablePart>();
                if (d) {
                    d.DealDamage(Damage, collision.GetContact(0).point, collision.GetContact(0).normal, true, gameObject, collision.gameObject);
                }
                else if(damagePart){
                      damagePart.parent.DealDamage(Damage, collision.GetContact(0).point, collision.GetContact(0).normal, true, gameObject, collision.gameObject);
                    Debug.Log($"damage:{Damage}");
                }
                }
                // Otherwise, can we take damage ourselves from this collision?
                else if (TakeCollisionDamage && thisDamageable != null) {
                    thisDamageable.DealDamage(CollisionDamage, collision.GetContact(0).point, collision.GetContact(0).normal, true, gameObject, collision.gameObject);
                }
            }
           
        }
    }
