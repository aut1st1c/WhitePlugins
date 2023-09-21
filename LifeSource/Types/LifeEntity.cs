﻿using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.Utility;
using UnityEngine;

namespace BrokeProtocol.GameSource.Types
{
    public class LifeEntity : EntityEvents
    {
        [Execution(ExecutionMode.Additive)]
        public override bool SecurityTrigger(ShEntity entity, Collider otherCollider)
        {
            if (entity.GetPlace is ApartmentPlace apartmentPlace && otherCollider.TryGetComponent(out ShPlayer player) && 
                player != apartmentPlace.svOwner && LifeManager.pluginPlayers.TryGetValue(player, out var pluginPlayer))
            {
                apartmentPlace.svOwner.svPlayer.SendGameMessage($"{entity.name} detected {player.username} in apartment");

                if (pluginPlayer.ApartmentTrespassing(apartmentPlace.svOwner))
                    pluginPlayer.AddCrime(CrimeIndex.Trespassing, apartmentPlace.svOwner);
            }

            return true;
        }

        [Execution(ExecutionMode.Additive)]
        public override bool Destroy(ShEntity entity)
        {
            if (entity.svEntity.randomSpawn)
            {
                var waypointIndex = (int)entity.svEntity.WaypointProperty;

                // Entity should only be part of 1 job's array but check all just in case
                foreach (var info in BPAPI.Jobs)
                {
                    ((MyJobInfo)info).randomEntities[waypointIndex].Remove(entity);
                }
            }

            return true;
        }

        [Execution(ExecutionMode.Additive)]
        public override bool SameSector(ShEntity e)
        {
            if (e.svEntity.randomSpawn && e.svEntity.spectators.Count == 0 && (!e.Player || !e.Player.svPlayer.currentState.IsBusy) && !e.svEntity.sector.HumanControlled())
            {
                e.svEntity.Deactivate(true);
            }

            return true;
        }
    }
}
