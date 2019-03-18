﻿using WildernessCallouts.CalloutFunct;
using WildernessCallouts.Peds;

namespace WildernessCallouts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Drawing;
    using LSPD_First_Response.Mod.API;
    using LSPD_First_Response.Engine.Scripting.Entities;
    using Rage;
    using Rage.Native;
    using System.Diagnostics;
    using System.IO;
    using WildernessCallouts.Callouts;

    internal class Common
    {
        public static void RegisterCallouts()
        {
            if (Settings.Callouts.IsIllegalHuntingEnabled)
                LSPD_First_Response.Mod.API.Functions.RegisterCallout(typeof(IllegalHunting));

            if (Settings.Callouts.IsRocksBlockEnabled)
                LSPD_First_Response.Mod.API.Functions.RegisterCallout(typeof(RocksBlockingRoad));

            if (Settings.Callouts.IsAircraftCrashEnabled)
                LSPD_First_Response.Mod.API.Functions.RegisterCallout(typeof(AircraftCrash));

            if (Settings.Callouts.IsRecklessDriverEnabled)
                LSPD_First_Response.Mod.API.Functions.RegisterCallout(typeof(RecklessDriver));

            if (Settings.Callouts.IsWantedFelonEnabled)
                LSPD_First_Response.Mod.API.Functions.RegisterCallout(typeof(WantedFelonInVehicle));

            if (Settings.Callouts.IsSuicideAttemptEnabled)
                LSPD_First_Response.Mod.API.Functions.RegisterCallout(typeof(SuicideAttempt));

            if (Settings.Callouts.IsMissingPersonEnabled)
                LSPD_First_Response.Mod.API.Functions.RegisterCallout(typeof(MissingPerson));

            if (Settings.Callouts.IsAnimalAttackEnabled)
                LSPD_First_Response.Mod.API.Functions.RegisterCallout(typeof(AnimalAttack));

            if (Settings.Callouts.IsPublicDisturbanceEnabled)
                LSPD_First_Response.Mod.API.Functions.RegisterCallout(typeof(PublicDisturbance));

            if (Settings.Callouts.IsHostageSituationEnabled)
                LSPD_First_Response.Mod.API.Functions.RegisterCallout(typeof(HostageSituation));

            if (Settings.Callouts.IsArsonEnabled)
                LSPD_First_Response.Mod.API.Functions.RegisterCallout(typeof(Arson));

            if (Settings.Callouts.IsOfficerNeedsTransportEnabled)
                LSPD_First_Response.Mod.API.Functions.RegisterCallout(typeof(OfficerNeedsTransport));

            if (Settings.Callouts.IsAttackedPoliceStationEnabled)
                LSPD_First_Response.Mod.API.Functions.RegisterCallout(typeof(AttackedPoliceStation));

            if (Settings.Callouts.IsDemonstrationEnabled)
                LSPD_First_Response.Mod.API.Functions.RegisterCallout(typeof(Demonstration));

            //LSPD_First_Response.Mod.API.Functions.RegisterCallout(typeof(MurderInvestigation));

            //if (Settings.Callouts.IsEscortEnable)
            //    LSPD_First_Response.Mod.API.Functions.RegisterCallout(typeof(Escort));
        }


        public static void PlayHeliAudioWithEntityCardinalDirection(string extraAudio, Entity entity)
        {
            if (entity.GetCardinalDirectionLowDetailed() == "N")
                Functions.PlayScannerAudio(extraAudio + " HELI_VISUAL_HEADING_NORTH_DISPATCH");
            else if (entity.GetCardinalDirectionLowDetailed() == "S")
                Functions.PlayScannerAudio(extraAudio + " HELI_VISUAL_HEADING_SOUTH_DISPATCH");
            else if (entity.GetCardinalDirectionLowDetailed() == "W")
                Functions.PlayScannerAudio(extraAudio + " HELI_VISUAL_HEADING_WEST_DISPATCH");
            else if (entity.GetCardinalDirectionLowDetailed() == "E")
                Functions.PlayScannerAudio(extraAudio + " HELI_VISUAL_HEADING_EAST_DISPATCH");
        }

        private static Dictionary<string, string> _huntingLicensesDict = new Dictionary<string, string>();
        public static void HuntingLicense(Ped ped)
        {
            string[] licenseState = { "~g~valid", "~r~suspended", "~r~expired" };
            Persona persona = LSPD_First_Response.Mod.API.Functions.GetPersonaForPed(ped);
            if (!_huntingLicensesDict.ContainsKey(persona.FullName))
            {
                string license = licenseState.GetRandomElement();
                Game.DisplayNotification("prop_flags", "prop_flag_sanandreas", "San Andreas", "HUNTING LICENSE", "~b~" + persona.FullName + "~n~~w~Birth: " + persona.BirthDay.ToShortDateString() + "~n~~w~Hunting license is " + license);
                _huntingLicensesDict.Add(persona.FullName, license);
            }
            else
            {
                Game.DisplayNotification("prop_flags", "prop_flag_sanandreas", "San Andreas", "HUNTING LICENSE", "~b~" + persona.FullName + "~n~~w~Birth: " + persona.BirthDay.ToShortDateString() + "~n~~w~Hunting license is " + _huntingLicensesDict[persona.FullName]);
            }
        }
        public static Dictionary<string, string> HuntingLicensesDictionary { get { return _huntingLicensesDict; } }

        private static Dictionary<string, string> _fishingLicensesDict = new Dictionary<string, string>();
        public static void FishingLicense(Ped ped)
        {
            string[] licenseState = { "~g~valid", "~r~suspended", "~r~expired" };
            Persona persona = LSPD_First_Response.Mod.API.Functions.GetPersonaForPed(ped);
            if (!_fishingLicensesDict.ContainsKey(persona.FullName))
            {
                string license = licenseState.GetRandomElement();
                Game.DisplayNotification("prop_flags", "prop_flag_sanandreas", "San Andreas", "FISHING LICENSE", "~b~" + persona.FullName + "~n~~w~Birth: " + persona.Birthday.ToShortDateString() + "~n~~w~Fishing license is " + license);
                _fishingLicensesDict.Add(persona.FullName, license);
            }
            else
            {
                Game.DisplayNotification("prop_flags", "prop_flag_sanandreas", "San Andreas", "FISHING LICENSE", "~b~" + persona.FullName + "~n~~w~Birth: " + persona.Birthday.ToShortDateString() + "~n~~w~Fishing license is " + _fishingLicensesDict[persona.FullName]);
            }
        }
        public static Dictionary<string, string> FishingLicensesDictionary { get { return _fishingLicensesDict; } }

        public static void EndMessage(string calloutName)
        {
            GameFiber.StartNew(delegate
            {
                GameFiber.Sleep(1250);
                Functions.PlayScannerAudio("ATTENTION_ALL_UNITS WE_ARE_CODE_4 NO_FURTHER_UNITS_REQUIRED");
                Game.DisplayNotification("~b~" + Settings.General.Name + ":~w~ Dispatch, ~o~" + calloutName.ToLower() + "~w~ call is ~g~code 4");
                GameFiber.Sleep(125);
                Game.DisplayNotification("~b~Dispatch:~w~ 10-4, all units " + calloutName.ToLower() + " call is code 4");
            });
        }

        public static void PlayAIRespondingAudio()
        {
            Functions.PlayScannerAudio("AI_RESPONDING");
        }

        /// <summary>
        /// Returns the specified file version
        /// </summary>
        /// <param name="filePath">The path of the file</param>
        /// <returns>the specified file version</returns>
        public static string GetFileVersion(string filePath)
        {
            try
            {
                var versInfo = FileVersionInfo.GetVersionInfo(filePath);
                string myVers = String.Format("{0}.{1}.{2}.{3}", versInfo.FileMajorPart, versInfo.FileMinorPart, versInfo.FileBuildPart, versInfo.FilePrivatePart);
                return myVers;
            }
            catch (Exception e)
            {
                Logger.LogTrivial("Exception handled: Error Loading Version of " + filePath + ": " + e);
                return "Error Loading Version!";
            }
        }

        /// <summary>
        /// Returns the specified file version
        /// </summary>
        /// <param name="filePath">The path of the file</param>
        /// <returns>the specified file version</returns>
        public static Version GetVersion(string filePath, bool considerRevision = false)
        {
            try
            {
                var versInfo = FileVersionInfo.GetVersionInfo(filePath);
                Version myVers;
                if (considerRevision) myVers = new Version(versInfo.FileMajorPart, versInfo.FileMinorPart, versInfo.FileBuildPart, versInfo.FilePrivatePart);
                else myVers = new Version(versInfo.FileMajorPart, versInfo.FileMinorPart, versInfo.FileBuildPart);
                return myVers;
            }
            catch (Exception e)
            {
                Logger.LogTrivial("Exception handled: Error Loading Version of " + filePath + ": " + e);
                return null;
            }
        }

        //public static string DownloadText(string url)
        //{
        //    System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
        //    System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();

        //    string text;

        //    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
        //        text = reader.ReadToEnd();

        //    return text;
        //}

        /// <summary>
        /// Starts a particle effect on entity
        /// </summary>
        /// <param name="ptfxAsset">Particle asset</param>
        /// <param name="effectName">Particle effect name</param>
        /// <param name="entity">Entity to create the particle on</param>
        /// <param name="offset">Entity offset</param>
        /// <param name="rotation">Particle rotation</param>
        /// <param name="scale">Particle scale</param>
        public static void StartParticleFxNonLoopedOnEntity(string ptfxAsset, string effectName, Entity entity, Vector3 offset, Rotator rotation, float scale)
        {

            NativeFunction.Natives.xb80d8756b4668ab6(ptfxAsset); //RequestNamedPtfxAsset
            while (!NativeFunction.Natives.x8702416e512ec454<bool>(ptfxAsset)) //HasNamedPtfxAssetLoaded
            {
                GameFiber.Sleep(25);
                NativeFunction.Natives.xb80d8756b4668ab6(ptfxAsset); //RequestNamedPtfxAsset
                GameFiber.Yield();
            }

            NativeFunction.Natives.x6c38af3693a69a91(ptfxAsset); //SetPtfxAssetNextCall
            NativeFunction.Natives.START_PARTICLE_FX_NON_LOOPED_ON_ENTITY(effectName, entity, offset.X, offset.Y, offset.Z, rotation.Pitch, rotation.Roll, rotation.Yaw, scale, false, false, false);
        }


        /// <summary>
        /// Returns the closest animal to the position
        /// </summary>
        /// <param name="position">Position</param>
        /// <param name="radius">Radius</param>
        /// <returns>the closest animal to the position</returns>
        public static Ped GetClosestAnimal(Vector3 position, float radius)
        {
            List<Ped> pedList = World.GetAllPeds().ToList();

            Ped animal = (Ped)(from x in pedList where x.IsAnimal() && x.Position.DistanceTo(position) < radius orderby x.Position.DistanceTo(position) select x).FirstOrDefault<Ped>();

            if (animal != null && animal.Exists()) return animal;
            else return null;
        }

        /// <summary>
        /// Returns the closest animal to the player.
        /// </summary>
        /// <param name="radius">Radius</param>
        /// <param name="maximumPedsToEvalute">maximum 16 -- number of peds to evaluate</param>
        /// <returns>the closest animal to the position</returns>
        public static Ped GetClosestAnimal(float radius, int maximumPedsToEvalute)
        {
            List<Ped> pedList = Game.LocalPlayer.Character.GetNearbyPeds(maximumPedsToEvalute).ToList();

            Ped animal = (Ped)(from x in pedList where x.IsAnimal() && x.Position.DistanceTo(Game.LocalPlayer.Character.Position) < radius orderby x.Position.DistanceTo(Game.LocalPlayer.Character.Position) select x).FirstOrDefault<Ped>();

            if (animal != null && animal.Exists()) return animal;
            else return null;
        }


        /// <summary>
        /// Returns a ped who may need rescuing by the air ambulance.
        /// </summary>
        /// <returns></returns>
        public static Ped GetPedToRescue()
        {
            foreach (Ped ped in Game.LocalPlayer.Character.GetNearbyPeds(16).ToList()) {
                Logger.LogDebug($"Evalute {ped}, {ped.Model.Name}, {ped.Handle}, dead:{ped.IsDead}, human:{ped.IsHuman}");
            }

            foreach (Ped ped in World.Get.ToList().Where(x =>
                (x.IsDead || x.Health <= 20 || x == MissingPerson.CurrentMissingPed) && x != Game.LocalPlayer.Character &&
                x.IsHuman && !AirParamedic.RescuedPeds.Contains(x))) {
                Logger.LogDebug($"Evalute {ped}, {ped.Model.Name}, {ped.Handle}");
            }

            Ped pedToRescue = Game.LocalPlayer.Character.GetNearbyPeds(16).ToList().Where(x =>
                    (x.IsDead || x.Health <= 20 || x == MissingPerson.CurrentMissingPed) && x != Game.LocalPlayer.Character &&
                    x.IsHuman && Vector3.Distance(x.Position, Game.LocalPlayer.Character.Position) > 15.0f &&
                    !AirParamedic.RescuedPeds.Contains(x))
                .OrderBy(x => x.Position.DistanceTo(Game.LocalPlayer.Character.Position))
                .FirstOrDefault();
            return pedToRescue;
        }


        /// <summary>
        /// Toggles the night vision
        /// </summary>
        /// <param name="toggle">Toggle the night vision</param>
        public static void SetNightVision(bool toggle)
        {
            NativeFunction.Natives.SET_NIGHTVISION(toggle);
        }

        /// <summary>
        /// Toggles the thermal vision
        /// </summary>
        /// <param name="toggle">Toggle the thermal vision</param>
        public static void SetThermalVision(bool toggle)
        {
            NativeFunction.Natives.SET_SEETHROUGH(toggle);
        }

        /// <summary>
        /// Returns a value indicating wheteher the night vision is active
        /// </summary>
        /// <returns>a value indicating wheteher the night vision is active</returns>
        public static bool IsNightVisionActive()
        {
            return NativeFunction.Natives.x2202a3f42c8e5f79<bool>(); //IsNightVisionActive
        }

        /// <summary>
        /// Returns a value indicating wheteher the thermal vision is active
        /// </summary>
        /// <returns>a value indicating wheteher the thermal vision is active</returns>
        public static bool IsThermalVisionActive()
        {
            return NativeFunction.Natives.x44b80abab9d80bd3<bool>(); //IsSeeThroughActive ??
        }

        public static void DisEnableGameControls(bool enable, params GameControl[] controls)
        {
            foreach (var con in controls)
            {
                if (enable)
                {
                    NativeFunction.Natives.ENABLE_CONTROL_ACTION(0, (int)con);
                    NativeFunction.Natives.ENABLE_CONTROL_ACTION(1, (int)con);
                    NativeFunction.Natives.ENABLE_CONTROL_ACTION(2, (int)con);
                }
                else
                {
                    NativeFunction.Natives.DISABLE_CONTROL_ACTION(0, (int)con);
                    NativeFunction.Natives.DISABLE_CONTROL_ACTION(1, (int)con);
                    NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, (int)con);
                }
            }
        }

        public static void DisEnableAllGameControls(bool enable)
        {
            foreach (var con in Enum.GetValues(typeof(GameControl)))
            {
                if (enable)
                {
                    NativeFunction.Natives.ENABLE_CONTROL_ACTION(0, (int)con);
                    NativeFunction.Natives.ENABLE_CONTROL_ACTION(1, (int)con);
                    NativeFunction.Natives.ENABLE_CONTROL_ACTION(2, (int)con);
                }
                else
                {
                    NativeFunction.Natives.DISABLE_CONTROL_ACTION(0, (int)con);
                    NativeFunction.Natives.DISABLE_CONTROL_ACTION(1, (int)con);
                    NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, (int)con);
                }
            }
            //Controls we want
            // -Frontend
            // -Mouse
            // -Walk/Move
            // -

            if (enable) return;
            var list = new List<GameControl>
            {
                GameControl.FrontendAccept,
                GameControl.FrontendAxisX,
                GameControl.FrontendAxisY,
                GameControl.FrontendDown,
                GameControl.FrontendUp,
                GameControl.FrontendLeft,
                GameControl.FrontendRight,
                GameControl.FrontendCancel,
                GameControl.FrontendSelect,
                GameControl.CursorScrollDown,
                GameControl.CursorScrollUp,
                GameControl.CursorX,
                GameControl.CursorY,
                GameControl.MoveUpDown,
                GameControl.MoveLeftRight,
                GameControl.Sprint,
                GameControl.Jump,
                GameControl.Enter,
                GameControl.VehicleExit,
                GameControl.VehicleAccelerate,
                GameControl.VehicleBrake,
                GameControl.VehicleMoveLeftRight,
                GameControl.VehicleFlyYawLeft,
                GameControl.ScriptedFlyLeftRight,
                GameControl.ScriptedFlyUpDown,
                GameControl.VehicleFlyYawRight,
                GameControl.VehicleHandbrake,
            };

            //if (IsUsingController)
            //{
            //    list.AddRange(new GameControl[]
            //    {
            //        GameControl.LookUpDown,
            //        GameControl.LookLeftRight,
            //        GameControl.Aim,
            //        GameControl.Attack,
            //    });
            //}

            foreach (var control in list)
            {
                NativeFunction.Natives.ENABLE_CONTROL_ACTION(0, (int)control);
            }
        }

        public static void DrawScaleformMovieFullscreen(RAGENativeUI.Elements.Scaleform scaleform, System.Drawing.Color color, int unk1 = 1)
        {
            NativeFunction.Natives.DRAW_SCALEFORM_MOVIE_FULLSCREEN(scaleform.Handle, (int)color.R, (int)color.G, (int)color.B, (int)color.A, unk1);
        }

        public static bool IsUsingController()
        {
            return !NativeFunction.Natives.xa571d46727e2b718<bool>(0, 2);
        }

        public static void DrawLine(Vector3 start, Vector3 end, Color color)
        {
            NativeFunction.Natives.DRAW_LINE(start.X, start.Y, start.Z, end.X, end.Y, end.Z, (int)color.R, (int)color.G, (int)color.B, (int)color.A);
        }

        public static void DrawMarker(EMarkerType type, Vector3 pos, Vector3 dir, Vector3 rot, Vector3 scale, Color color)
        {
            DrawMarker(type, pos, dir, rot, scale, color, false, false, 2, false, null, null, false);
        }
        public static void DrawMarker(EMarkerType type, Vector3 pos, Vector3 dir, Vector3 rot, Vector3 scale, Color color, bool bobUpAndDown, bool faceCamY, int unk2, bool rotateY, string textueDict, string textureName, bool drawOnEnt)
        {
            dynamic dict = 0;
            dynamic name = 0;

            if (textueDict != null && textureName != null)
            {
                if (textueDict.Length > 0 && textureName.Length > 0)
                {
                    dict = textueDict;
                    name = textureName;
                }
            }
            NativeFunction.Natives.DRAW_MARKER((int)type, pos.X, pos.Y, pos.Z, dir.X, dir.Y, dir.Z, rot.X, rot.Y, rot.Z, scale.X, scale.Y, scale.Z, (int)color.R, (int)color.G, (int)color.B, (int)color.A, bobUpAndDown, faceCamY, unk2, rotateY, dict, name, drawOnEnt);
        }

        public static Entity RaycastEntity2(Vector3 pos, Vector3 direction, float distance, params Entity[] toIgnore)
        {
            TraceFlags flags = TraceFlags.IntersectFoliage | TraceFlags.IntersectObjects | TraceFlags.IntersectPeds | TraceFlags.IntersectPedsSimpleCollision | TraceFlags.IntersectVehicles | TraceFlags.IntersectWorld;
            HitResult ray = World.TraceLine(pos, pos + direction * distance, flags, toIgnore);

            return ray.HitEntity; 
        }

        public static Vector3 GetGameplayCameraDirection()
        {
            Vector3 rot = GetGameplayCameraRotation();
            double rotX = rot.X / 57.295779513082320876798154814105;
            double rotZ = rot.Z / 57.295779513082320876798154814105;
            double multXY = System.Math.Abs(System.Math.Cos(rotX));

            return new Vector3((float)(-System.Math.Sin(rotZ) * multXY), (float)(System.Math.Cos(rotZ) * multXY), (float)(System.Math.Sin(rotX)));
        }

        public static Vector3 GetGameplayCameraPosition()
        {
            return NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
        }

        public static Vector3 GetGameplayCameraRotation()
        {
            return NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(2);
        }

        //public static Texture GetTextureFromEmbeddedResource(string resourceName)
        //{
        //    System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
        //    System.IO.Stream stream = assembly.GetManifestResourceStream(resourceName);
        //    string tempPath = System.IO.Path.GetTempFileName();
        //    using (System.IO.FileStream fs = new System.IO.FileStream(tempPath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))
        //    {
        //        fs.SetLength(0);
        //        byte[] buffer = new byte[stream.Length];
        //        stream.Read(buffer, 0, buffer.Length);
        //        fs.Write(buffer, 0, buffer.Length);
        //    }

        //    return Game.CreateTextureFromFile(tempPath);
        //}

        /// <summary>
        /// Get a valid animal for vet pickup, or return null if no animal was found.
        /// </summary>
        /// <returns></returns>
        public static Ped GetValidAnimalForVetPickup()
        {
            Ped animal = WildernessCallouts.Common.GetClosestAnimal(Game.LocalPlayer.Character.Position, 30.0f);

            int timesToLoop = 0;
            while (timesToLoop < 20)
            {
                if (animal.Exists() && animal.IsDead && !Vet.TakenAnimals.Contains(animal))
                    break;
                animal = WildernessCallouts.Common.GetClosestAnimal(Game.LocalPlayer.Character.Position, 35.0f);
                timesToLoop++;
            } // Gets the closest dead animal

            if (!animal.Exists() || animal.IsAlive || Vet.TakenAnimals.Contains(animal))
                return animal;
            return animal;
        }

#if DEBUG
        public static string GetUserInput(EWindowTitle windowTitle, string defaultText, int maxLength)
        {
            NativeFunction.Natives.DISPLAY_ONSCREEN_KEYBOARD(true, windowTitle.ToString(), "", defaultText, "", "", "", maxLength + 1);

            while (NativeFunction.Natives.UPDATE_ONSCREEN_KEYBOARD<int>() == 0)
                GameFiber.Yield();

            return (string)NativeFunction.Natives.GET_ONSCREEN_KEYBOARD_RESULT<string>();
        }
#endif
    }
}
