namespace Names
{
    public class Class1 : Qurre.Plugin
    {
        public static string NN = "";
        public override void Enable()
        {
            NN = Config.GetString("names_otryadi", "<color=red>#Вафля</color>", "Название проекта в отрядах");
            Qurre.Events.Round.Waiting += N;
        }
        public override void Disable()
        {
            Qurre.Events.Round.Waiting -= N;
        }
        private static void N()
        {
            Qurre.Log.Info("Добавляю юниты..");
            Qurre.API.Round.AddUnit(Qurre.API.Objects.TeamUnitType.ChaosInsurgency, "<size=1> </size>");
            Qurre.API.Round.AddUnit(Qurre.API.Objects.TeamUnitType.NineTailedFox, "<size=1> </size>");
            Qurre.API.Round.AddUnit(Qurre.API.Objects.TeamUnitType.ClassD, "<size=1> </size>");
            Qurre.API.Round.AddUnit(Qurre.API.Objects.TeamUnitType.Scientist, "<size=1> </size>");
            Qurre.API.Round.AddUnit(Qurre.API.Objects.TeamUnitType.Scp, "<size=1> </size>");
            Qurre.API.Round.AddUnit(Qurre.API.Objects.TeamUnitType.Tutorial, "<size=1> </size>");

            Qurre.API.Round.AddUnit(Qurre.API.Objects.TeamUnitType.ChaosInsurgency, NN);
            Qurre.API.Round.AddUnit(Qurre.API.Objects.TeamUnitType.NineTailedFox, NN);
            Qurre.API.Round.AddUnit(Qurre.API.Objects.TeamUnitType.ClassD, NN);
            Qurre.API.Round.AddUnit(Qurre.API.Objects.TeamUnitType.Scientist, NN);
            Qurre.API.Round.AddUnit(Qurre.API.Objects.TeamUnitType.Scp, NN);
            Qurre.API.Round.AddUnit(Qurre.API.Objects.TeamUnitType.Tutorial, NN);
        }
    }
}