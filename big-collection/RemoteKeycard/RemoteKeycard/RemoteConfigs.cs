using EXILED;
using System.Collections.Generic;

namespace RemoteKeycard
{
	public class ConfigManagers
	{
		public static bool RKCDebug { get; private set; }

		private static ConfigManagers singleton;
		public static ConfigManagers Manager
		{
			get
			{
				if (singleton == null)
				{
					singleton = new ConfigManagers();
				}
				return singleton;
			}
		}

		// Readonly  
#pragma warning disable IDE0052 // the unused pragma lol will be used in the future
		private readonly List<string> DoorPerms = new List<string>()
		{
			"CONT_LVL_1",
			"CONT_LVL_2",
			"CONT_LVL_3",
			"ARMORY_LVL_1",
			"ARMORY_LVL_2",
			"ARMORY_LVL_3",
			"CHCKPOINT_ACC",
			"EXIT_ACC",
			"INCOM_ACC"
		};

		private readonly List<string> NameDoors = new List<string>()
		{
			"HCZ_ARMORY",
			"914",
			"012_BOTTOM",
			"106_BOTTOM",
			"LCZ_ARMORY",
			"GATE_A",
			"106_SECONDARY",
			"GATE_B",
			"012",
			"079_SECOND",
			"106_PRIMARY",
			"049_ARMORY",
			"NUKE_SURFACE",
			"NUKE_ARMORY",
			"CHECKPOINT_ENT",
			"CHECKPOINT_LCZ_B",
			"HID_RIGHT",
			"173_ARMORY",
			"CHECKPOINT_LCZ_A",
			"173",
			"ESCAPE",
			"HID_LEFT",
			"HID",
			"096",
			"372",
			"ESCAPE_INNER",
			"SURFACE_GATE",
			"INTERCOM",
			"079_FIRST"
		};

#pragma warning restore IDE0052
		internal readonly Dictionary<int, PList> DefaultCardAccess = new Dictionary<int, PList>()
		{
			{ 0,    new PList(new List<string>(new string[] { "CONT_LVL_1", "CONT_LVL_2", "CONT_LVL_3", "ARMORY_LVL_1", "ARMORY_LVL_2", "ARMORY_LVL_3", "CHCKPOINT_ACC", "EXIT_ACC", "INCOM_ACC" }))    },
			{ 1,    new PList(new List<string>(new string[] { "CONT_LVL_1", "CONT_LVL_2", "ARMORY_LVL_1", "ARMORY_LVL_2", "ARMORY_LVL_3", "CHCKPOINT_ACC", "EXIT_ACC", "INCOM_ACC" }))                  },
			{ 2,    new PList(new List<string>(new string[] { "CONT_LVL_1", "CONT_LVL_2", "CONT_LVL_3", "CHCKPOINT_ACC", "EXIT_ACC", "INCOM_ACC" } ))                                                   },
			{ 3,    new PList(new List<string>(new string[] { "CONT_LVL_1", "CONT_LVL_2", "ARMORY_LVL_1", "ARMORY_LVL_2", "ARMORY_LVL_3", "CHCKPOINT_ACC", "EXIT_ACC", "INCOM_ACC" }))                  },
			{ 4,    new PList(new List<string>(new string[] { "CONT_LVL_1", "CONT_LVL_2", "ARMORY_LVL_1", "ARMORY_LVL_2", "CHCKPOINT_ACC", "EXIT_ACC" }))                                               },
			{ 5,    new PList(new List<string>(new string[] { "CONT_LVL_1", "CONT_LVL_2", "CONT_LVL_3", "CHCKPOINT_ACC", "INCOM_ACC" }))                                                                },
			{ 6,    new PList(new List<string>(new string[] { "CONT_LVL_1", "CONT_LVL_2", "ARMORY_LVL_1", "ARMORY_LVL_2", "CHCKPOINT_ACC" }))                                                           },
			{ 7,    new PList(new List<string>(new string[] { "CONT_LVL_1", "ARMORY_LVL_1", "CHCKPOINT_ACC" }))                                                                                         },
			{ 8,    new PList(new List<string>(new string[] { "CHCKPOINT_ACC" }))                                                                                                                       },
			{ 9,    new PList(new List<string>(new string[] { "CONT_LVL_1", "CONT_LVL_2", "CHCKPOINT_ACC" }))                                                                                           },
			{ 10,   new PList(new List<string>(new string[] { "CONT_LVL_1", "CONT_LVL_2" }))                                                                                                            },
			{ 11,   new PList(new List<string>(new string[] { "CONT_LVL_1" }))                                                                                                                          },
		};
		internal readonly Dictionary<string, string> DefaultDoorAccess = new Dictionary<string, string>()
		{
			{ "HCZ_ARMORY",                     "ARMORY_LVL_1"  },
			{ "106_SECONDARY",                  "CONT_LVL_3"    },
			{ "914",                            "CONT_LVL_1"    },
			{ "LCZ_ARMORY",                     "ARMORY_LVL_1"  },
			{ "079_SECOND",                     "CONT_LVL_3"    },
			{ "GATE_A",                         "EXIT_ACC"      },
			{ "GATE_B",                         "EXIT_ACC"      },
			{ "106_BOTTOM",                     "CONT_LVL_3"    },
			{ "106_PRIMARY",                    "CONT_LVL_3"    },
			{ "NUKE_ARMORY",                    "ARMORY_LVL_2"  },
			{ "012",                            "CONT_LVL_2"    },
			{ "049_ARMORY",                     "ARMORY_LVL_2"  },
			{ "CHECKPOINT_ENT",                 "CHCKPOINT_ACC" },
			{ "NUKE_SURFACE",                   "CONT_LVL_3"    },
			{ "CHECKPOINT_LCZ_A",               "CHCKPOINT_ACC" },
			{ "CHECKPOINT_LCZ_B",               "CHCKPOINT_ACC" },
			{ "HID",                            "ARMORY_LVL_3"  },
			{ "079_FIRST",                      "CONT_LVL_3"    },
			{ "096",                            "CONT_LVL_2"    },
			{ "INTERCOM",                       "INCOM_ACC"     }
		};
		internal readonly Dictionary<string, CList> DefaultDoorList = new Dictionary<string, CList>()
		{
			{ "HCZ_ARMORY",                     new CList(new List<int>(new int[] { 0,1,3,4,6,7 }))                 },
			{ "106_SECONDARY",                  new CList(new List<int>(new int[] { 0,2,5 }))                       },
			{ "914",                            new CList(new List<int>(new int[] { 0,1,2,3,4,5,6,7,8,9,10,11 }))   },
			{ "LCZ_ARMORY",                     new CList(new List<int>(new int[] { 0,1,3,4,6,7 }))                 },
			{ "079_SECOND",                     new CList(new List<int>(new int[] { 0,2,5 }))                       },
			{ "GATE_A",                         new CList(new List<int>(new int[] { 0,1,2,3,4 }))                   },
			{ "GATE_B",                         new CList(new List<int>(new int[] { 0,1,2,3,4 }))                   },
			{ "106_BOTTOM",                     new CList(new List<int>(new int[] { 0,2,5 }))                       },
			{ "106_PRIMARY",                    new CList(new List<int>(new int[] { 0,2,5 }))                       },
			{ "NUKE_ARMORY",                    new CList(new List<int>(new int[] { 0,1,3,4,6 }))                   },
			{ "012",                            new CList(new List<int>(new int[] { 0,1,2,3,4,5,6,9,10 }))          },
			{ "049_ARMORY",                     new CList(new List<int>(new int[] { 0,1,3,4,6 }))                   },
			{ "CHECKPOINT_ENT",                 new CList(new List<int>(new int[] { 0,1,2,3,4,5,6,7,8,9 }))         },
			{ "NUKE_SURFACE",                   new CList(new List<int>(new int[] { 0,2,5 }))                       },
			{ "CHECKPOINT_LCZ_A",               new CList(new List<int>(new int[] { 0,1,2,3,4,5,6,7,8,9 }))         },
			{ "CHECKPOINT_LCZ_B",               new CList(new List<int>(new int[] { 0,1,2,3,4,5,6,7,8,9 }))         },
			{ "HID",                            new CList(new List<int>(new int[] { 0,1,3 }))                       },
			{ "079_FIRST",                      new CList(new List<int>(new int[] { 0,2 }))                         },
			{ "096",                            new CList(new List<int>(new int[] { 0,1,2,3,4,5,6,9,10 }))          },
			{ "INTERCOM",                       new CList(new List<int>(new int[] { 0,1,2,3,5 }))                   }
		};

		// Customs
		internal readonly List<int> CardsList = new List<int>();
		internal readonly Dictionary<int, PList> CustomCardAccess = new Dictionary<int, PList>();
		internal readonly Dictionary<string, string> CustomDoorAccess = new Dictionary<string, string>();
		internal readonly Dictionary<string, CList> CustomDoorList = new Dictionary<string, CList>();

		internal void ReloadConfig()
		{
			RKCDebug = Plugin.Config.GetBool("rkc_info", false);
		}

		internal class PList
		{
			public List<string> perms;
			public PList(List<string> perms)
			{
				this.perms = perms;
			}
		}

		internal class CList
		{
			public List<int> ints;
			public CList(List<int> ints)
			{
				this.ints = ints;
			}
		}
	}
}
