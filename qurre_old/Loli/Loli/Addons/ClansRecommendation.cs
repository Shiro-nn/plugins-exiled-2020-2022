using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using MEC;
using Newtonsoft.Json;
using Qurre.API;
namespace Loli.Addons
{
	static internal class ClansRecommendation
	{
		static internal ClanInfo Clan { get; private set; }
		static internal string GetText()
		{
			if (Clan is null) return "";
			if (Clan.FormatedDesc == string.Empty)
			{
				List<string> descList = new();
				int words = 0;
				string dodesc = "";
				foreach (var dd in Clan.Desc.Split(' '))
				{
					dodesc += dd + " ";
					words++;
					if (words == 5)
					{
						words = 0;
						descList.Add(dodesc);
						dodesc = "";
					}
				}
				if (dodesc != string.Empty)
					descList.Add(dodesc);
				string desc = "";
				double dl = 0;
				foreach (var dsc in descList)
				{
					desc += $"<voffset={-4 - dl}em><align=right><pos=50%><b><size=70%><color=#6f6f6f>{dsc}</color></size></b></pos></align></voffset>\n";
					dl += 0.7;
				}
				desc += $"<voffset={-5 - dl}em><align=right><pos=70%><b><size=70%><color=#f47fff>Вступить в клан можно</color></size></b></pos></align></voffset>\n";
				desc += $"<voffset={-6 - dl}em><align=right><pos=70%><b><size=70%><color=#f47fff>🌐 на сайте</color> <color=#0089c7>scpsl<color=red>.</color>store</color></size></b></pos></align></voffset>\n";
				Clan.FormatedDesc = desc;
			}
			return $"<line-height=0%>\n<voffset={2}em><align=right><pos=50%><b><size=150%><color=#ff2222>Рекомендации</color></size></b></pos></align></voffset>\n\n" +
				$"<line-height=0%>\n<voffset={0.5}em><align=right><pos=50%><b><size=150%><color=#ff2222>кланов:</color></size></b></pos></align></voffset>\n\n" +
				$"<voffset={-1}em><align=right><pos=50%><b><size=70%><color={Clan.Color}>{Clan.Name}</color></size></b></pos></align></voffset>\n" +
				$"<voffset={-2}em><align=right><pos=50%><b><size=70%><color=#c9c602>{Clan.Money} 💰 | {Clan.Tag} | 💳 {Clan.Balance}</color></size></b></pos></align></voffset>\n" +
				$"<voffset={-3}em><align=right><pos=50%><b><size=70%><color=#ffae00>{Clan.Boosts} бустов 🚀</color></size></b></pos></align></voffset>\n" +
				$"{Clan.FormatedDesc}\n";
		}
		static internal void Init()
		{
			Timing.RunCoroutine(UpdateClans(), "UpdateClans");
			static IEnumerator<float> UpdateClans()
			{
				yield return Timing.WaitForSeconds(30f);
				for (; ; )
				{
					try { Update(); } catch { }
					yield return Timing.WaitForSeconds(300f);
				}
				void Update()
				{
					var tags = DataBase.Modules.Data.Clans.Select(x => x.Key).ToList();
					SelectUpdate();
					void SelectUpdate()
					{
						try { AgainUpdate(tags[UnityEngine.Random.Range(0, tags.Count)]); }
						catch { SelectUpdate(); }
					}
				}
				void AgainUpdate(string tag)
				{
					var url = $"{Plugin.APIUrl}/clan?tag={tag}&type=info";
					var request = WebRequest.Create(url);
					request.Method = "POST";
					using var webResponse = request.GetResponse();
					using var webStream = webResponse.GetResponseStream();
					using var reader = new StreamReader(webStream);
					var data = reader.ReadToEnd();
					Clan = JsonConvert.DeserializeObject<ClanInfo>(data);
				}
			}
		}
		internal class ClanInfo
		{
			[JsonProperty("name")]
			public string Name { get; set; }

			[JsonProperty("tag")]
			public string Tag { get; set; }

			[JsonProperty("color")]
			public string Color { get; set; }

			[JsonProperty("public")]
			public bool Public { get; set; }

			[JsonProperty("desc")]
			public string Desc { get; set; }

			[JsonProperty("boosts")]
			public int Boosts { get; set; }

			[JsonProperty("money")]
			public int Money { get; set; }

			[JsonProperty("balance")]
			public int Balance { get; set; }

			[JsonIgnore]
			public string FormatedDesc { get; set; } = "";
		}
	}
}