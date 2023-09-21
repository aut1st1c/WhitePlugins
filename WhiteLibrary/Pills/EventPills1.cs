using BrokeProtocol.API;
using BrokeProtocol.Entities;

namespace WhiteLibrary.Pills
{
    public class PlayerEventsGuide : PlayerEvents
    {
        [Execution(ExecutionMode.Additive)]
        public override bool Consume(ShPlayer player, ShConsumable consumable, ShPlayer healer)
        {
            if (consumable.name == "PillsRegen")
            {
                WhiteLibrary.Effects.Regen.Regeneration(player);
            }
            else
            {
                if (consumable.name == "PillsSpeed")
                {
                    WhiteLibrary.Effects.Speed.speed(player);
                }
                else
                {
                    if (consumable.name == "PillsHealth")
                    {
                        WhiteLibrary.Effects.Health.health(player);
                    }
                }
            }
            return true;
        }
    }
}
