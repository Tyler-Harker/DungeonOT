﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOT.Scripting;

namespace SharpOT.Scripting
{
    public interface IActionItem
    {
        ushort GetItemId();
        bool Use(Location fromLocation, byte fromStackPosition, byte index, Item item);
        bool UseOnItem(Location fromLocation, byte fromStackPosition, byte index, Item item, Location toLocation, Item onItem);
        bool UseOnTile(Location fromLocation, byte fromStackPosition, byte index, Item item, Tile tile);
        bool UseOnCreature(Location fromLocation, byte fromStackPosition, byte index, Item item, Creature creature);
    }
}

namespace SharpOT
{
    public static class ActionItems
    {
        static Dictionary<ushort, IActionItem> actions = new Dictionary<ushort, IActionItem>();

        public static void RegisterAction(IActionItem action)
        {
            if (actions.ContainsKey(action.GetItemId()))
                throw new Exception(String.Format("Action {0} has a duplicate ItemId {1}.", action.GetType().Name, action.GetItemId()));

            actions.Add(action.GetItemId(), action);
        }

        public static void UnRegisterAction(ushort itemId)
        {
            actions.Remove(itemId);
        }

        public static bool ExecuteUse(Game game, Player player, Location fromLocation, byte fromStackPosition, byte index, Item item)
        {
            if (actions.ContainsKey(item.Id))
                return actions[item.Id].Use(fromLocation, fromStackPosition, index, item);
            return true;
        }

        public static bool ExecuteUseOnItem(Game game, Player player, Location fromLocation, byte fromStackPosition, byte index, Item item, Location toLocation, Item onItem)
        {
            if (actions.ContainsKey(item.Id))
                return actions[item.Id].UseOnItem(fromLocation, fromStackPosition, index, item, toLocation, onItem);
            return true;
        }

        public static bool ExecuteUseOnTile(Game game, Player player, Location fromLocation, byte fromStackPosition, byte index, Item item, Tile tile)
        {
            if (actions.ContainsKey(item.Id))
                return actions[item.Id].UseOnTile(fromLocation, fromStackPosition, index, item, tile);
            return true;
        }

        public static bool ExecuteUseOnCreature(Game game, Player player, Location fromLocation, byte fromStackPosition, byte index, Item item, Creature creature)
        {
            if (actions.ContainsKey(item.Id))
                return actions[item.Id].UseOnCreature(fromLocation, fromStackPosition, index, item, creature);
            return true;
        }
    }
}