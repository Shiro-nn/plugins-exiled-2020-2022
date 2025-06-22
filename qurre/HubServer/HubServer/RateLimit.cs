using HarmonyLib;
using LiteNetLib;
using LiteNetLib.Utils;
using MEC;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Emit;

namespace HubServer
{
    [HarmonyPatch(typeof(CustomLiteNetLib4MirrorTransport), nameof(CustomLiteNetLib4MirrorTransport.ProcessConnectionRequest))]
    static class RateLimit
    {
        static readonly List<long[]> IpList = new();

        static uint requests = 0;

        /*
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> Call(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            Label retLabel = generator.DefineLabel();
            instructions.ElementAt(instructions.Count() - 1).labels.Add(retLabel);

            List<CodeInstruction> list = new(instructions);

            int index = list.FindLastIndex(ins => ins.opcode == OpCodes.Ldstr &&
                (ins.operand as string).StartsWith("Requested challenge for incoming connection from endpoint"));

            list.InsertRange(index, new CodeInstruction[]
            {
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(RateLimit), nameof(RateLimit.AddReq))).MoveLabelsFrom(list[index]),
                new CodeInstruction(OpCodes.Nop),
            });

            return list.AsEnumerable();
        }
        */

        static void AddReq()
        {
            if (requests > 140)
                return;

            requests++;
            Timing.CallDelayed(5f, () => requests--);
        }

        [EventMethod(RoundEvents.Waiting)]
        static void Waiting()
        {
            requests = 0;
        }

        static internal void Enable()
        {
            if (IpList.Count == 0)
            {
                string dir = Path.Combine(Pathes.Plugins, "IpDB");
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                string file = Path.Combine(dir, "db.csv");
                if (!File.Exists(file))
                    File.Create(file).Close();

                var _arr = File.ReadAllText(file).Split('\n');
                foreach (var str in _arr)
                {
                    try
                    {
                        string[] data = str.Split(',');
                        long byte1 = BitConverter.ToInt32(getBytes(IPAddress.Parse(data[0])), 0);
                        long byte2 = BitConverter.ToInt32(getBytes(IPAddress.Parse(data[1])), 0);
                        IpList.Add(new long[] { byte1, byte2 });
                    }
                    catch { }

                    static byte[] getBytes(IPAddress address)
                    {
                        var bytes = address.GetAddressBytes();
                        Array.Reverse(bytes);
                        return bytes;
                    }
                }
            }
        }

        //[HarmonyPrefix]
        static bool Call(ConnectionRequest request)
        {
            AddReq();
            try
            {
                if (requests < 100)
                    return true;

                if (!IsBlocked(request.RemoteEndPoint.Address))
                    return true;

                try { request.Reject(Generate()); } catch { }

                return false;

                static NetDataWriter Generate()
                {
                    try
                    {
                        NetDataWriter netDataWriter = new();
                        netDataWriter.Put((byte)RejectionReason.Custom);

                        netDataWriter.Put("\n" +
                            "Сервер находится под DDoS атакой Layer7\n" +
                            "Ваше подключение было замечено как подозрительное, поэтому было отклонено\n" +
                            "The server is under Layer7 DDoS attack\n" +
                            "Your connection was seen as suspicious, so it was rejected");

                        return netDataWriter;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return true;
            }
        }

        static bool IsBlocked(IPAddress address)
        {
            var ipBytes = address.GetAddressBytes();
            Array.Reverse(ipBytes);
            long ip = BitConverter.ToInt32(ipBytes, 0);

            foreach (long[] bytes in IpList)
            {
                try
                {
                    if (InRange(bytes[0], bytes[1], ip))
                        return false;
                }
                catch { }
            }

            return true;
        }

        static bool InRange(long ipStart, long ipEnd, long ip)
        {
            return ip >= ipStart && ip <= ipEnd;
        }
    }
}