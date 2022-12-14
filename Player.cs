using System;
using System.Collections.Generic;
using SharpOT.Scripting;

namespace SharpOT
{
    public class Player : Creature
    {
        public Connection Connection { get; set; }
        public Inventory Inventory { get; private set; }
        public Gender Gender { get; set; }
        public Vocation Vocation { get; set; }
        public ushort Level { get; set; }
        public byte MagicLevel { get; set; }
        public uint Experience { get; set; }
        public uint Capacity { get; set; }
        public Location SavedLocation { get; set; }
        public List<Channel> ChannelList { get; set; }
        public List<Channel> OpenedChannelList { get; set; }
        public FightMode FightMode { get; set; }
        public bool ChaseMode { get; set; }
        public bool SafeMode { get; set; }
        public Dictionary<uint, Vip> VipList { get; set; }
        public int LastYellTime { get; set; }
        public DateTime LastLogin { get; set; }
        public PlayerFlags Flags { get; set; }
        
        public Player()
        {
            Inventory = new Inventory();
            Gender = Gender.Male;
            Vocation = Vocation.None;
            ChannelList = new List<Channel>();
            OpenedChannelList = new List<Channel>();
            ChannelList.Add(new Channel((ushort)ChatChannel.Game, "Game-Chat", 0));
            ChannelList.Add(new Channel((ushort)ChatChannel.RealLife, "RL-Chat", 0));
            ChannelList.Add(new Channel((ushort)ChatChannel.Help, "Help", 0));
            VipList = new Dictionary<uint, Vip>(100);
        }

        public override Item GetCorpse()
        {
            if (Gender == Gender.Female)
                return Item.Create(Constants.Items.CorpseFemale);
            else
                return Item.Create(Constants.Items.CorpseMale);
        }
    }
}