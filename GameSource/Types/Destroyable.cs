﻿using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.Managers;
using BrokeProtocol.Required;
using BrokeProtocol.Utility;
using UnityEngine;

namespace BrokeProtocol.GameSource.Types
{
    public class Destroyable : DestroyableEvents
    {
        [Execution(ExecutionMode.Additive)]
        public override bool Death(ShDestroyable destroyable, ShPlayer attacker)
        {
            if (attacker && attacker != destroyable)
            {
                attacker.svPlayer.job.OnDestroyEntity(destroyable);
            }

            return true;
        }

        [Execution(ExecutionMode.Additive)]
        public override bool Damage(ShDamageable damageable, DamageIndex damageIndex, float amount, ShPlayer attacker, Collider collider, Vector3 hitPoint, Vector3 hitNormal)
        {
            if (damageable.IsDead) return false;

            var destroyable = damageable as ShDestroyable;

            var player = damageable.Player;

            if (player)
            {
                if (player.svPlayer.godMode || player.IsShielded(damageIndex, collider))
                    return false;

                if (damageIndex != DamageIndex.Null)
                {
                    BodyEffect effect;
                    var random = Random.value;

                    if (random < 0.6f)
                        effect = BodyEffect.Null;
                    else if (random < 0.8f)
                        effect = BodyEffect.Pain;
                    else if (random < 0.925f)
                        effect = BodyEffect.Bloodloss;
                    else
                        effect = BodyEffect.Fracture;

                    BodyPart part;

                    var capsuleHeight = player.capsule.direction == 1 ? player.capsule.height : player.capsule.radius * 2f;

                    var hitY = player.GetLocalY(hitPoint);

                    if (damageIndex == DamageIndex.Random)
                    {
                        part = (BodyPart)Random.Range(0, (int)BodyPart.Count);
                    }
                    else if (damageIndex == DamageIndex.Melee && player.IsBlocking(damageIndex))
                    {
                        part = BodyPart.Arms;
                        amount *= 0.3f;
                    }
                    else if (collider == player.headCollider) // Headshot
                    {
                        part = BodyPart.Head;
                        amount *= 2f;
                    }
                    else if (hitY >= capsuleHeight * 0.75f)
                    {
                        part = Random.value < 0.5f ? BodyPart.Arms : BodyPart.Chest;
                    }
                    else if (hitY >= capsuleHeight * 0.5f)
                    {
                        part = BodyPart.Abdomen;
                        amount *= 0.8f;
                    }
                    else
                    {
                        part = BodyPart.Legs;
                        amount *= 0.5f;
                    }

                    // Only do amount reductions for non-Null types of damage
                    amount -= amount * (player.armorLevel / 200f);
                    if (!player.isHuman)
                    {
                        amount /= SvManager.Instance.settings.difficulty;
                    }

                    if (effect != BodyEffect.Null)
                    {
                        player.svPlayer.SvAddInjury(part, effect, (byte)Random.Range(10, 50));
                    }
                }
            }

            destroyable.health -= amount;

            if (destroyable.health <= 0f)
            {
                destroyable.Die(attacker);
            }
            else if (attacker && attacker != damageable)
            {
                var controller = damageable.controller;

                if (controller && controller != damageable && !controller.isHuman && !controller.svPlayer.currentState.IsBusy)
                {
                    controller.GamePlayer().SetAttackState(attacker);
                }
                
                attacker.svPlayer.job.OnDamageEntity(damageable);
            }

            var damageSourceType = Util.DamageSourceMap[(int)damageIndex];

            Vector3 source;

            if (damageSourceType == Util.DamageSource.HitPoint)
                source = hitPoint;
            else if (damageSourceType == Util.DamageSource.Attacker && attacker)
                source = attacker.GetOrigin;
            else
                source = default;

            destroyable.svDestroyable.UpdateHealth(source);

            return true;
        }

        [Execution(ExecutionMode.Additive)]
        public override bool DestroySelf(ShDestroyable destroyable)
        {
            var player = destroyable.Player;
            if(!player || !player.isHuman || !player.IsRestrained || !player.IsUp)
                destroyable.svDestroyable.Damage(DamageIndex.Null, destroyable.health);
            return true;
        }

        [Execution(ExecutionMode.Additive)]
        public override bool Heal(ShDestroyable destroyable, float amount, ShPlayer healer)
        {
            if (destroyable.CanHeal)
            {
                if (healer) healer.svPlayer.job.OnHealEntity(destroyable);

                destroyable.health = Mathf.Min(destroyable.health + amount, destroyable.maxStat);
                // Must send to local because health is required info
                destroyable.svDestroyable.UpdateHealth();
                return true;
            }

            return false;
        }
    }
}