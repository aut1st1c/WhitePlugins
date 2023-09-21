﻿using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.Managers;
using BrokeProtocol.Utility;
using System.Collections;
using UnityEngine;

namespace BrokeProtocol.GameSource
{
    public class Events : IScript
    {
        [CustomTarget]
        public void GetItemValue(ShPlayer player, ShItem item)
        {
            var other = player.otherEntity;

            if (other && !other.IsDead && other.Shop)
            {
                player.svPlayer.SendGameMessage('$' + other.GetMyItemValue(item, false).ToString());
            }
        }

        [CustomTarget]
        public void ThrowableTarget(Serialized trigger, ShPhysical physical)
        {
            if (physical is ShThrown thrown)
            {
                var thrower = thrown.svEntity.instigator;
                if (thrower)
                {
                    thrower.TransferMoney(
                        DeltaInv.AddToMe,
                        Mathf.Clamp(Mathf.CeilToInt(0.5f * thrower.DistanceSqr(trigger.mainT.position)), 10, 1000),
                        true);
                }
                thrown.Destroy();
            }
        }
        [CustomTarget]
        public void Rearm(Serialized trigger, ShPhysical physical)
        {
            if (physical is ShTransport transport)
            {
                transport.StartCoroutine(RearmCoroutine(trigger, transport));
            }
        }

        private IEnumerator RearmCoroutine(Serialized trigger, ShTransport transport)
        {
            var delay = new WaitForSeconds(2f);
            while(transport.svMovable.currentTriggers.Contains(trigger))
            {
                transport.Rearm(0.1f); // Rearm 10% of ammo capacity
                yield return delay;
            }
        }

        [CustomTarget]
        public void Repair(Serialized trigger, ShPhysical physical)
        {
            if (physical is ShTransport transport)
            {
                transport.StartCoroutine(RepairCoroutine(trigger, transport));
            }
        }

        private IEnumerator RepairCoroutine(Serialized trigger, ShTransport transport)
        {
            var delay = new WaitForSeconds(2f);
            while (transport.svMovable.currentTriggers.Contains(trigger))
            {
                transport.svTransport.Heal(transport.maxStat * 0.1f); // Heal 10% of maxStat
                yield return delay;
            }
        }

        // Return value isn't used anywhere, just for testing API support
        [CustomTarget]
        public bool ButtonPush(ShEntity target, ShPlayer caller)
        {
            const int cost = 500;

            if (caller.MyMoneyCount < cost)
            {
                caller.svPlayer.SendGameMessage("Not enough cash");
                return false;
            }

            if (!StartSlowMotion(caller))
            {
                caller.svPlayer.SendGameMessage("Do not challenge the void");
                return false;
            }

            caller.TransferMoney(DeltaInv.RemoveFromMe, cost);
            return true;
        }

        private static bool slowMotion;

        public static bool StartSlowMotion(ShPlayer caller, float duration = 5f)
        {
            if (slowMotion || duration <= 0f) return false;
            SceneManager.Instance.StartCoroutine(SlowMotion(duration));
            return true;
        }

        private static IEnumerator SlowMotion(float duration)
        {
            slowMotion = true;

            var delay = new WaitForSecondsRealtime(0.1f);

            var startTime = Time.time;

            var originalTimeScale = Time.timeScale;
            var targetTimeScale = 0.25f;

            var defaultEnvironment = SceneManager.Instance.defaultEnvironment;

            var originalSky = SceneManager.Instance.skyColor;
            var originalCloud = SceneManager.Instance.cloudColor;
            var originalWater = SceneManager.Instance.waterColor;

            var targetSky = Color.red;
            var targetCloud = Color.black;
            var targetWater = Color.cyan;

            var normalizedClip = 0.25f;

            while (Time.time < startTime + duration)
            {
                yield return delay;

                var normalizedTime = (Time.time - startTime) / duration;

                float lerp;

                if (normalizedTime < normalizedClip)
                {
                    lerp = normalizedTime / normalizedClip;
                }
                else if (normalizedTime >= 1f - normalizedClip)
                {
                    lerp = (1f - normalizedTime) / normalizedClip;
                }
                else
                {
                    lerp = 1f;
                }

                SvManager.Instance.SvSetTimeScale(Mathf.Lerp(originalTimeScale, targetTimeScale, lerp));
                SvManager.Instance.SvSetSkyColor(Color.LerpUnclamped(originalSky, targetSky, lerp));
                SvManager.Instance.SvSetCloudColor(Color.LerpUnclamped(originalCloud, targetCloud, lerp));
                SvManager.Instance.SvSetWaterColor(Color.LerpUnclamped(originalWater, targetWater, lerp));
            }

            SvManager.Instance.SvSetTimeScale(originalTimeScale);

            if (defaultEnvironment)
            {
                SvManager.Instance.SvSetDefaultEnvironment();
            }
            else
            {
                SvManager.Instance.SvSetSkyColor(originalSky);
                SvManager.Instance.SvSetCloudColor(originalCloud);
                SvManager.Instance.SvSetWaterColor(originalWater);
            }

            slowMotion = false;
        }
    }
}
