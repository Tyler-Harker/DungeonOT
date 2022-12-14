using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOT.Scripting;

namespace SharpOT.Scripting
{
    public interface ICommand
    {
        string GetWords();
        bool CanBeUsedBy(Player player);
        bool Action(Game game, Player player, string args);
    }

    public interface ISpell : ICommand
    {

    }
}

namespace SharpOT
{
    public static class Commands
    {
        private static LinkedList<ICommand> commandList = new LinkedList<ICommand>();

        public static void RegisterCommand(ICommand command)
        {
            if (!(command is ISpell) && command.GetWords().Contains(' '))
                throw new Exception("Command words cannot contain spaces.");

            LinkedListNode<ICommand> lastNode = commandList.First;

            if (lastNode == null)
            {
                commandList.AddFirst(command);
            }
            else
            {
                while (lastNode.Next != null &&
                    lastNode.Value.GetWords().Length > command.GetWords().Length)
                {
                    lastNode = lastNode.Next;
                }
                commandList.AddAfter(lastNode, command);
            }
        }

        public static void UnRegisterCommand(ICommand command)
        {
            commandList.Remove(commandList.Find(command));
        }

        public static bool ExecuteCommand(Game game, Player player, string words)
        {
            foreach (ICommand cmd in commandList)
            {
                if (!cmd.CanBeUsedBy(player))
                    continue;

                if (cmd is ISpell)
                {
                    string[] split = words.Split('"');
                    if (split[0].Trim() == cmd.GetWords())
                    {
                        string args = "";
                        if (split.Length > 1)
                            args = split[1];
                        return cmd.Action(game, player, args);
                    }
                }
                else
                {
                    string[] split = words.Split(' ');
                    if (split[0].Trim() == cmd.GetWords())
                    {
                        string args = words.Substring(split[0].Length).Trim();
                        return cmd.Action(game, player, args);
                    }
                } 
            }
            return true;
        }
    }
}
