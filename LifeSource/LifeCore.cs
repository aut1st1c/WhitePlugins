﻿using BrokeProtocol.API;
using BrokeProtocol.Required;
using BrokeProtocol.Utility;
using BrokeProtocol.Utility.AI;
using BrokeProtocol.Utility.Jobs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace BrokeProtocol.GameSource
{
    public class LifeCore : Plugin
    {
        public static State Rob = new RobState();
        public static State PullOver = new PullOverState();

        public static int prisonerIndex = -1;
        public static int policeIndex = -1;

        public LifeCore()
        {
            Info = new PluginInfo(
                "LifeSource",
                "life",
                "Roleplay source used by BP. May be modified.",
                "https://github.com/broke-protocol/broke-protocol");

            var jobsFilename = Info.Name + " Jobs.json";

            if (!File.Exists(jobsFilename))
            {
                File.WriteAllText(jobsFilename, JsonConvert.SerializeObject(GetJobs, Formatting.Indented));
            }

            // Use JobsAdditive if you're adding to Default jobs and not replacing them
            JobsOverride = new List<JobInfo>();
            var myJobs = JsonConvert.DeserializeObject<List<MyJobInfo>>(File.ReadAllText(jobsFilename));
            foreach (var job in myJobs)
            {
                JobsOverride.Add(job);
            }

            var index = 0;
            foreach (var info in myJobs)
            {
                if (prisonerIndex < 0 && info.groupIndex == GroupIndex.Prisoner)
                {
                    prisonerIndex = index;
                }
                else if (policeIndex < 0 && info.groupIndex == GroupIndex.LawEnforcement)
                {
                    policeIndex = index;
                }
                index++;
            }

            StatesAdditive = new List<State>
            {
                Rob,
                PullOver,
            };
        }

        private List<MyJobInfo> GetJobs => new()
        {
            /*
            new MyJobInfo(
                typeof(Citizen), "Job Name",
                "Job Description",
                CharacterType.All, 0, GroupIndex.Citizen, new ColorStruct(0.75f, 0.75f, 0.75f), 1f, 28,
                new Transports[] {
                    new Transports(new string[] {
                        "Car1",
                        "Car2" }),
                    new Transports(new string[] {
                        "Aircraft1",
                        "Aircraft2" }),
                    new Transports(new string[] {
                        "Boat1",
                        "Boat2" }),
                    new Transports(new string[] {
                        "Train1",
                        "Train2" }),
                    new Transports(new string[] {
                        "Towable1",
                        "Towable2" }),
                },
                new Upgrades[] {
                    new Upgrades(10,
                        new InventoryStruct[] {
                            new InventoryStruct("Item1", 1),
                            new InventoryStruct("Item2", 1)}),
                    new Upgrades(20,
                        new InventoryStruct[] {
                            new InventoryStruct("Item1", 1),
                            new InventoryStruct("Item2", 1)}),
                    new Upgrades(20,
                        new InventoryStruct[] {
                            new InventoryStruct("Item1", 1),
                            new InventoryStruct("Item2", 1)})
                }),
            */

            new MyJobInfo(
                typeof(Citizen), "Citizen",
                "Get money by robbing, looting, and trading with NPCs and players or get a job by visiting map icons",
                CharacterType.All, 0, GroupIndex.Citizen, new ColorStruct(0.75f, 0.75f, 0.75f), 1f, 28,
                new Transports[] {
                    new Transports(new string[] {
                        "Car1",
                        "Car2",
                        "Car3",
                        "CarPizza",
                        "Flatbed1",
                        "Flatbed2",
                        "Flatbed3",
                        "Hatchback1",
                        "Hatchback2",
                        "Hatchback3",
                        "Pickup1",
                        "Pickup2",
                        "Pickup3",
                        "Semi1",
                        "Semi2",
                        "Semi3",
                        "SportsCar1",
                        "SportsCar2",
                        "SportsCar3",
                        "SUV1",
                        "SUV2",
                        "SUV3",
                        "Taxi",
                        "TowTruck1",
                        "TowTruck2",
                        "TowTruck3",
                        "Ute1",
                        "Ute2",
                        "Ute3",
                        "Van1",
                        "Van2",
                        "Van3",
                        "Motorbike1",
                        "Motorbike2",
                        "Motorbike3"}),
                    new Transports(new string[] {
                        "Cessna1",
                        "Cessna2",
                        "Cessna3",
                        "Cessna4",
                        "SmallHelo1",
                        "SmallHelo2",
                        "SmallHelo3",
                        "Biplane1",
                        "Biplane2"}),
                    new Transports(new string[] {
                        "Boat1",
                        "Boat2",
                        "Boat3"}),
                    new Transports(new string[] {}),
                    new Transports(new string[] {
                        "Car1",
                        "Car2",
                        "Car3",
                        "Trailer",
                        "TrailerContainer1",
                        "TrailerContainer2",
                        "TrailerContainer3",
                        "TrailerContainer4",
                    })
                },
                new Upgrades[0]),
            new MyJobInfo(
                typeof(Prisoner), "Prisoner",
                "The prison door can be bombed and the guard might have a key",
                CharacterType.Humanoid, 0, GroupIndex.Prisoner, new ColorStruct(1f, 0.5f, 0f), 0f, 0,
                new Transports[] {
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                },
                new Upgrades[] {
                    new Upgrades(0,
                        new InventoryStruct[] {
                            new InventoryStruct("TopPrisoner", 1),
                            new InventoryStruct("PantsPrisoner", 1)})}),
            new MyJobInfo(
                typeof(Hitman), "Hitman",
                "Assasinate designated targets to earn bounty rewards",
                CharacterType.Humanoid, 0, GroupIndex.Criminal, new ColorStruct(0f, 0f, 0f), 0.01f, 3,
                new Transports[] {
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                },
                new Upgrades[] {
                    new Upgrades(20,
                        new InventoryStruct[] {
                            new InventoryStruct("TopStriped", 1),
                            new InventoryStruct("SkiMaskDark", 1),
                            new InventoryStruct("GlovesFingerlessDark", 1),
                            new InventoryStruct("Knife", 1)}),
                    new Upgrades(25,
                        new InventoryStruct[] {
                            new InventoryStruct("SkiMaskLight", 1),
                            new InventoryStruct("Winchester", 1),
                            new InventoryStruct("AmmoRifle", 30)})
                    ,
                    new Upgrades(30,
                        new InventoryStruct[] {
                            new InventoryStruct("SkiMaskPattern", 1),
                            new InventoryStruct("LaserRed", 1),
                            new InventoryStruct("Silencer", 1)})
                }),
            new MyJobInfo(
                typeof(Police), "Police",
                "Search others for illegal items, arrest criminals, put the them in your car, and bring to jail for cash rewards",
                CharacterType.Humanoid, 0, GroupIndex.LawEnforcement, new ColorStruct(0f, 1f, 1f), 0.03f, 10,
                new Transports[] {
                    new Transports(new string[] { "CarPolice" }),
                    new Transports(new string[] { }),
                    new Transports(new string[] { "PoliceBoat" }),
                    new Transports(new string[] { }),
                    new Transports(new string[] { }),
                },
                new Upgrades[] {
                    new Upgrades(20,
                        new InventoryStruct[] {
                            new InventoryStruct("TopPolice", 1),
                            new InventoryStruct("PantsPolice", 1),
                            new InventoryStruct("CapPolice", 1),
                            new InventoryStruct("Taser", 1),
                            new InventoryStruct("AmmoTaser", 12),
                            new InventoryStruct("Handcuffs", 5),
                            new InventoryStruct("Muzzle", 2),
                            new InventoryStruct("LicenseGun", 1),
                            new InventoryStruct("ShoesBrown", 1)}),
                    new Upgrades(25,
                        new InventoryStruct[] {
                            new InventoryStruct("Glock", 1),
                            new InventoryStruct("AmmoPistol", 68),
                            new InventoryStruct("Baton", 1),
                            new InventoryStruct("LicenseDrivers", 1)}),
                    new Upgrades(30,
                        new InventoryStruct[] {
                            new InventoryStruct("TopSheriff", 1),
                            new InventoryStruct("PantsSheriff", 1),
                            new InventoryStruct("CapSheriff", 1),
                            new InventoryStruct("Colt", 1),
                            new InventoryStruct("GlovesDark", 1),
                            new InventoryStruct("KeyPrison", 1)})
                }),
            new MyJobInfo(
                typeof(Paramedic), "Paramedic",
                "Use map to find hurt and knocked out players to heal and revive",
                CharacterType.Humanoid, 0, GroupIndex.Citizen, new ColorStruct(1f, 0.75f, 0.75f), 0.02f, 5,
                new Transports[] {
                    new Transports(new string[] {"Ambulance"}),
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                },
                new Upgrades[] {
                    new Upgrades(20,
                        new InventoryStruct[] {
                            new InventoryStruct("TopParamedic", 1),
                            new InventoryStruct("PantsParamedic", 1),
                            new InventoryStruct("GlovesMedical", 1),
                            new InventoryStruct("HealthPack", 8)}),
                    new Upgrades(25,
                        new InventoryStruct[] {
                            new InventoryStruct("Splint", 8),
                            new InventoryStruct("Defibrillator", 1),
                            new InventoryStruct("LicenseDrivers", 1)}),
                    new Upgrades(30,
                        new InventoryStruct[] {
                            new InventoryStruct("TopDoctor", 1),
                            new InventoryStruct("PantsDoctor", 1),
                            new InventoryStruct("Morphine", 6),
                            new InventoryStruct("Bandage", 6)})
                }),
            new MyJobInfo(
                typeof(Firefighter), "Firefighter",
                "Use map to find fires to extinguish",
                CharacterType.Humanoid, 0, GroupIndex.Citizen, new ColorStruct(1f, 1f, 0f), 0.01f, 3,
                new Transports[] {
                    new Transports(new string[] {"FireTruck"}),
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                },
                new Upgrades[] {
                    new Upgrades(20,
                        new InventoryStruct[] {
                            new InventoryStruct("JacketFire", 1),
                            new InventoryStruct("PantsFire", 1),
                            new InventoryStruct("HatHazard", 1),
                            new InventoryStruct("FireExtinguisher", 1)}),
                    new Upgrades(25,
                        new InventoryStruct[] {
                            new InventoryStruct("HatFire", 1),
                            new InventoryStruct("GlovesMedium", 1),
                            new InventoryStruct("LicenseDrivers", 1)}),
                    new Upgrades(30,
                        new InventoryStruct[] {
                            new InventoryStruct("JacketFireBlack", 1),
                            new InventoryStruct("PantsFireBlack", 1),
                            new InventoryStruct("FireHose", 1)})
                }),
            new MyJobInfo(
                typeof(Gangster), "Rojo Loco",
                "Kill enemy gangs to start a turf war and defeat enemy waves to capture territory",
                CharacterType.Humanoid, 0, GroupIndex.Criminal, new ColorStruct(1f, 0f, 0f), 0.1f, 8,
                new Transports[] {
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                },
                new Upgrades[] {
                    new Upgrades(20,
                        new InventoryStruct[] {
                            new InventoryStruct("GangJacketRed", 1),
                            new InventoryStruct("PantsLightBlue", 1),
                            new InventoryStruct("BackwardsCapRed", 1),
                            new InventoryStruct("Machete", 1)}),
                    new Upgrades(25,
                        new InventoryStruct[] {
                            new InventoryStruct("Sig", 1),
                            new InventoryStruct("AmmoPistol", 48)}),
                    new Upgrades(30,
                        new InventoryStruct[] {
                            new InventoryStruct("Mac", 1),
                            new InventoryStruct("AmmoSMG", 90)})
                }),
            new MyJobInfo(
                typeof(Gangster), "Green St. Fam",
                "Kill enemy gangs to start a turf war and defeat enemy waves to capture territory",
                CharacterType.Humanoid, 0, GroupIndex.Criminal, new ColorStruct(0f, 1f, 0f), 0.1f, 8,
                new Transports[] {
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                },
                new Upgrades[] {
                    new Upgrades(20,
                        new InventoryStruct[] {
                            new InventoryStruct("GangJacketGreen", 1),
                            new InventoryStruct("PantsGreen", 1),
                            new InventoryStruct("FaceScarfDark", 1),
                            new InventoryStruct("BatMetal", 1)}),
                    new Upgrades(25,
                        new InventoryStruct[] {
                            new InventoryStruct("Glock", 1),
                            new InventoryStruct("AmmoPistol", 68)}),
                    new Upgrades(30,
                        new InventoryStruct[] {
                            new InventoryStruct("MP5SD", 1),
                            new InventoryStruct("AmmoSMG", 60)})
                }),
            new MyJobInfo(
                typeof(Gangster), "Borgata Blu",
                "Kill enemy gangs to start a turf war and defeat enemy waves to capture territory",
                CharacterType.Humanoid, 0, GroupIndex.Criminal, new ColorStruct(0f, 0f, 1f), 0.1f, 8,
                new Transports[] {
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                },
                new Upgrades[] {
                    new Upgrades(20,
                        new InventoryStruct[] {
                            new InventoryStruct("JacketBusinessBlack", 1),
                            new InventoryStruct("SlacksGray", 1),
                            new InventoryStruct("HatFedora", 1),
                            new InventoryStruct("Crowbar", 1),
                            new InventoryStruct("GlovesFingerlessDark", 1),
                            new InventoryStruct("ShoesBlack", 1)}),
                    new Upgrades(25,
                        new InventoryStruct[] {
                            new InventoryStruct("PPK", 1),
                            new InventoryStruct("AmmoPistol", 49)}),
                    new Upgrades(30,
                        new InventoryStruct[] {
                            new InventoryStruct("Shotgun", 1),
                            new InventoryStruct("AmmoShotgun", 32)})
                }),
            new MyJobInfo(
                typeof(Mayor), "Mayor",
                "You're the Mayor: Accept or reject license requests",
                CharacterType.Humanoid, 1, GroupIndex.Citizen, new ColorStruct(1f, 0f, 1f), 0f, 0,
                new Transports[] {
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                },
                new Upgrades[] {
                    new Upgrades(0,
                        new InventoryStruct[] {
                            new InventoryStruct("JacketBusinessRed", 1),
                            new InventoryStruct("SlacksGray", 1),
                            new InventoryStruct("ShoesGray", 1),
                            new InventoryStruct("HatBoonieDark", 1),
                            new InventoryStruct("LicenseDrivers", 1),
                            new InventoryStruct("LicenseBoating", 1),
                            new InventoryStruct("LicensePilots", 1),
                            new InventoryStruct("LicenseGun", 1)})
                }),
            new MyJobInfo(
                typeof(DeliveryMan), "Delivery Man",
                "Deliver food to hungry players and NPCs on your map (M) for rewards",
                CharacterType.Humanoid, 0, GroupIndex.Citizen, new ColorStruct(0.5f, 0.25f, 0f), 0f, 0,
                new Transports[] {
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                },
                new Upgrades[] {
                    new Upgrades(20,
                        new InventoryStruct[] {
                            new InventoryStruct("TeePizza", 1),
                            new InventoryStruct("CapPizza", 1)}),
                    new Upgrades(25,
                        new InventoryStruct[] {
                            new InventoryStruct("LicenseDrivers", 1)}),
                    new Upgrades(30,
                        new InventoryStruct[] {
                            new InventoryStruct("JacketRacerRed", 1),
                            new InventoryStruct("PantsRacerRed", 1),
                            new InventoryStruct("CapRacerRed", 1),
                            new InventoryStruct("GlovesFingerlessWhite", 1)})
                }),
            new MyJobInfo(
                typeof(TaxiDriver), "Taxi Driver",
                "Bring NPCs to destinations on your map (M) for rewards",
                CharacterType.Humanoid, 0, GroupIndex.Citizen, new ColorStruct(0f, 0f, 0.5f), 0f, 0,
                new Transports[] {
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                },
                new Upgrades[] {
                    new Upgrades(20,
                        new InventoryStruct[] {
                            new InventoryStruct("CapFlat", 1),
                            new InventoryStruct("SuspendersBrown", 1),
                            new InventoryStruct("LicenseDrivers", 1)}),
                    new Upgrades(25,
                        new InventoryStruct[] {
                            new InventoryStruct("GlovesWhite", 1),
                            new InventoryStruct("LicensePilots", 1)}),
                    new Upgrades(30,
                        new InventoryStruct[] {
                            new InventoryStruct("TopChauffeur", 1),
                            new InventoryStruct("PantsChauffeur", 1),
                            new InventoryStruct("CapChauffeur", 1)})
                }),
            new MyJobInfo(
                typeof(SpecOps), "SpecOps",
                "Hunt down the most wanted players on the server for rewards",
                CharacterType.Humanoid, 0, GroupIndex.LawEnforcement, new ColorStruct(0.75f, 0.75f, 0.25f), 0.015f, 8,
                new Transports[] {
                    new Transports(new string[] {
                        "TroopCar1",
                        "TroopCar2",
                        "TroopCar3",
                        "ArmyFuel1",
                        "ArmyFuel2",
                        "ArmyFuel3",
                        "ArmoredCar1",
                        "ArmoredCar2",
                        "ArmoredCar3" }),
                     new Transports(new string[] {
                        "Apache1",
                        "Apache2",
                        "Apache3",
                        "Fighter1",
                        "Fighter2",
                        "FighterBig1",
                        "FighterBig2"}),
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                },
                new Upgrades[] {
                    new Upgrades(20,
                        new InventoryStruct[] {
                            new InventoryStruct("TopCombat", 1),
                            new InventoryStruct("PantsCombat", 1),
                            new InventoryStruct("HelmetCombat", 1),
                            new InventoryStruct("Sig", 1),
                            new InventoryStruct("AmmoPistol", 49),
                            new InventoryStruct("Smoke", 1),
                            new InventoryStruct("LicenseGun", 1)}),
                    new Upgrades(25,
                        new InventoryStruct[] {
                            new InventoryStruct("Kabar", 1),
                            new InventoryStruct("GlovesFingerlessDark", 1),
                            new InventoryStruct("LicenseDrivers", 1)}),
                    new Upgrades(30,
                        new InventoryStruct[] {
                            new InventoryStruct("M4", 1),
                            new InventoryStruct("AmmoRifle", 60)})
                }),
            new MyJobInfo(
                typeof(Retriever), "Retriever",
                "Return lost or dropped items to their rightful owner in time for rewards",
                CharacterType.Mob, 0, GroupIndex.Citizen, new ColorStruct(0.25f, 0.75f, 0.25f), 0f, 0,
                new Transports[] {
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                    new Transports(new string[0]),
                },
                new Upgrades[] {
                    new Upgrades(20,
                        new InventoryStruct[] {
                            new InventoryStruct("CapMob", 1)}),
                    new Upgrades(25,
                        new InventoryStruct[] { }),
                    new Upgrades(30,
                        new InventoryStruct[] { })
                }),
            };
    }
}
