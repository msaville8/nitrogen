﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nitrogen.IO;
using System.IO;
using Nitrogen.Blf;
using Nitrogen.Games.Halo4;
using Nitrogen.ContentData;
using Nitrogen.Games.Halo4.ContentData;
using Nitrogen.ContentData.Localization;

namespace Nitrogen.Test
{
    [TestClass]
    public class VariantTests
    {
        [TestMethod]
        public void CreateHalo4MapVariant()
        {
            using (var file = File.Create("C:/Users/Aaron/Desktop/test.map"))
            using (var s = new EndianStream(file, StreamState.Write, ByteOrder.BigEndian))
            {
                var mvar = new Nitrogen.Games.Halo4.Halo4MapVariantBlob();
                mvar.Serialize(s);
            }
        }

        [TestMethod]
        public void ReadAndModifyHalo4MapVariant()
        {
            Halo4MapVariantBlob map;

            using (var buffer = new MemoryStream(File.ReadAllBytes("C:/Users/Matt/Desktop/test objects.map")))
                map = Blob.Load<Halo4MapVariantBlob>(buffer);

            /*using (var output = File.Create("C:/Users/matt/Desktop/test.map"))
                Blob.Save<Halo4MapVariantBlob>(output, map);*/
        }

        [TestMethod]
        public void ReadHalo4MultiplayerVariant()
        {
            Halo4MultiplayerVariantBlob mpvr;

            using (var buffer = new MemoryStream(File.ReadAllBytes("C:/Users/Matt/Desktop/h4_rumble_tu.game")))
                mpvr = Blob.Load<Halo4MultiplayerVariantBlob>(buffer);
        }

        [TestMethod]
        public void ReadHalo4MapInfo()
        {
            /*Halo4MapInfoBlob map;

            using (var buffer = new MemoryStream(File.ReadAllBytes("C:/Users/Matt/Desktop/m40_invasion.mapinfo")))
            {
                map = Blob.Load<Halo4MapInfoBlob>(buffer);
            }

            using (var output = File.Create("C:/Users/matt/Desktop/m10_crash_resave.mapinfo"))
            {
                //Blob.Save<Halo4MapInfoBlob>(output, map);
            }*/
        }

        [TestMethod]
        public void CreateHalo4MapInfo()
        {
            /*using (var stream = File.Create("C:/Users/Matt/Desktop/lel.mapinfo"))
            {
                var mapInfo = Blob.Create<Halo4MapInfoBlob>();

                mapInfo.Level.MapId = 10261;
                mapInfo.Level.DefaultMapVariantAuthor = "i made dis!";
                mapInfo.Level.Name.Set("meltdown");
                mapInfo.Level.MapFileName = "ca_canyon";
                mapInfo.Level.Description.Set("testy test");
                mapInfo.Level.MapImageFileName = "wargames_ca_canyon";
                mapInfo.Level.Flags = LevelFlags.IsMultiplayer | LevelFlags.Visible | LevelFlags.GeneratesFilm;

                Blob.Save(stream, mapInfo);
            }*/
        }
    }
}
