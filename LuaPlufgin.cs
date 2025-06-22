[EventMethod(PlayerEvents.Join)]
static void Waiting(JoinEvent ev)
{
    Log.Info("Waiting for players...");
    DateTime now = DateTime.Now;
    ExecuteLuaCode(@"
                sendLog(""hello"")
                sendLogObj(System_DateTime)
                sendLogObj(API_Server)
                sendLogObj(API_Server.Port)
                sendLogObj(System_DateTime.Now)
                API_AudioExtensions.PlayInIntercom(System_Path.Combine(API_Paths.Plugins, ""Audio/RoundStart/cutscene.raw""))
                result = player.UserInformation.UserId
            ", ev.Player);
    Log.Debug("-----");
    Log.Debug((DateTime.Now - now).Ticks);
    Log.Debug((DateTime.Now - now).TotalMilliseconds);
}

static void SendLog(string message)
{
    Log.Info("Log from Lua: " + message);
}
static void SendLogObj(object message)
{
    Log.Info("Log object from Lua: " + message);
}

static void ExecuteLuaCode(string code, Player player)
{
    try
    {
        DateTime now = DateTime.Now;
        Script luaScript = new();
        Log.Debug((DateTime.Now - now).TotalMilliseconds);
        UserData.RegisterType<Player>();
        Log.Debug((DateTime.Now - now).TotalMilliseconds);
        luaScript.Globals["player"] = player;
        luaScript.Globals["sendLog"] = (string message) => SendLog(message);
        luaScript.Globals["sendLogObj"] = (object)SendLogObj;
        Lua.Internal.PrepareTable(luaScript.Globals);
        Log.Debug((DateTime.Now - now).TotalMilliseconds);
        luaScript.DoString(code);
        Log.Debug((DateTime.Now - now).TotalMilliseconds);
        // Retrieve the result if needed
        DynValue result = luaScript.Globals.Get("result");
        Log.Debug((DateTime.Now - now).TotalMilliseconds);
        Log.Debug("Lua Result: " + result.String);
    }
    catch (SyntaxErrorException e)
    {
        Log.Error("Lua Syntax Error: " + e.Message);
    }
    catch (Exception e)
    {
        Log.Error("Lua Error: " + e.Message);
    }
}