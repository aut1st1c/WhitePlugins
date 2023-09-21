using BrokeProtocol.API;
using System;

namespace WhiteTax.Base
{
    public class SaveJson : ManagerEvents
    {
        [Execution(ExecutionMode.Additive)]
        public override bool Save()
        {
            WhiteTax.Base.JsonParser.SaveJson();
            Console.WriteLine("[WHITE] Succefuly saved JSON");
            return true;
        }
    }
}
