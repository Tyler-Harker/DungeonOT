using SharpOT;
using SharpOT.Scripting;
using System;

public class CreateItemCommand : ICommand
{
    public string GetWords()
    {
        return "/item";
    }

    public bool CanBeUsedBy(Player player)
    {
        return true;
    }

    public bool Action(Game game, Player player, string args)
    {
        try
        {
            string[] parsed = args.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
            string first = parsed[0].Trim().ToLower();
            ushort itemId = 0;
            if (!ushort.TryParse(first, out itemId))
            {
                // TODO: Create a list of all matching names, display to user
                bool found = false;
                foreach (ItemInfo info in ItemInfo.GetEnumerator())
                {
                    if (info.Name != null && info.Name.ToLower().Equals(first))
                    {
                        itemId = info.Id;
                        found = true;
                        break;
                    }
                }
                if (!found)
                    throw new ArgumentException();
            }

            Item item = Item.Create(itemId);

            game.AddItemToTile(item, game.Map.GetTile(player.Tile.Location));
        }
        catch (Exception)
        {
            player.Connection.SendTextMessage(
                TextMessageType.StatusSmall,
                "Syntax: /item <item id or item name>"
            );
        }

        return false;
    }
}