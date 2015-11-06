﻿using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using Settings = Kitelyn.Config.Misc;
using Kitelyn.Modes;
namespace Kitelyn
{
    public static class Program
    {
        // Change this line to the champion you want to make the addon for,
        // watch out for the case being correct!
        public const string ChampName = "Caitlyn";

        public static void Main(string[] args)
        {
            // Wait till the loading screen has passed
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            // Verify the champion we made this addon for
            if (Player.Instance.ChampionName != ChampName)
            {
                // Champion is not the one we made this addon for,
                // therefore we return
                return;
            }

            // Initialize the classes that we need
            Config.Initialize();
            SpellManager.Initialize();
            ModeManager.Initialize();
            SaveMePls.Initialize();
            if (Settings.autolevelskills)
            {
                Player.Instance.Spellbook.LevelSpell(SpellSlot.W);
            }

            // Listen to events we need
            Drawing.OnDraw += OnDraw;
            Player.OnLevelUp += Kitelyn.Modes.PermaActive.autoLevelSkills;
            Gapcloser.OnGapcloser += Gapcloser_OnGapcloser;

            
        }

        private static void Gapcloser_OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (Settings.useWOnGapcloser && sender.IsEnemy)
            {
                SpellManager.W.Cast(e.End);
            }
            else if (Settings.useEOnGapcloser && sender.IsEnemy)
            {
                SpellManager.E.Cast(sender);
            }
        }

        private static void OnDraw(EventArgs args)
        {
            // Draw range circles of our spells
            if(Settings._drawQ.CurrentValue)
                Circle.Draw(Color.Red, SpellManager.Q.Range, Player.Instance.Position);
            if (Settings._drawW.CurrentValue)
                Circle.Draw(Color.Aqua, SpellManager.W.Range, Player.Instance.Position);
            if (Settings._drawE.CurrentValue)
                Circle.Draw(Color.DarkGreen, SpellManager.E.Range, Player.Instance.Position);
            if (Settings._drawR.CurrentValue)
                Circle.Draw(Color.DarkOrange, SpellManager.RRange, Player.Instance.Position);

        }
    }
}
