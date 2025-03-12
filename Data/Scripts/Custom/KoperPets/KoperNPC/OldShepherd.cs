using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Multis;
using Server.Network;

namespace Server.Custom.KoperPets
{
    public class OldShepherd : BaseVendor
    {
        public override bool IsInvulnerable
        {
            get { return true; }
        } // Makes NPC unkillable

        private static readonly List<SBInfo> _SBInfos = new List<SBInfo>(); // Vendor inventory placeholder

        public override string TalkGumpTitle
        {
            get { return "Better tames for you and me"; }
        }

        public override string TalkGumpSubject
        {
            get { return "Breeder"; }
        }

        protected override List<SBInfo> SBInfos
        {
            get { return _SBInfos; }
        } // Define vendor inventory

        [Constructable]
        public OldShepherd() : base("Old Shepherd")
        {
            Name = "Elias"; // Gives the NPC a name
            Body = 0x190; // Male human
            Hue = Utility.RandomSkinHue(); // Random skin tone

            // Set a classic shepherd outfit
            //AddItem(new LongBeard(Utility.RandomHairHue()));
            //AddItem(new FloppyHat(Utility.RandomNeutralHue()));
            //AddItem(new Shirt(Utility.RandomNeutralHue()));
            //AddItem(new LongPants(Utility.RandomNeutralHue()));
            //AddItem(new Sandals());

            // Holds a crook like a shepherd
            //AddItem(new ShepherdsCrook());

            // Make NPC stand still and not wander
            CantWalk = true;
            this.Hidden = false;
            //HairItemID = 0x203C;   // The ItemID of the hair you want
            //HairHue = 1175;
        }

        public override void InitSBInfo(Mobile m)
        {
            m_Merchant = m;
            _SBInfos.Add(new SBKoperShepherd()); // Attach vendor items (defined later)
        }

        public class SBKoperShepherd : SBInfo
        {
            private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
            private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

            public SBKoperShepherd()
            {
                // Add Breeding Item (Placeholder)
                //m_BuyInfo.Add(new GenericBuyInfo(typeof(KoperBreedingItem), 5000, 10, 0xF8D, 0)); // Price 5k, 10 in stock

                // Add Nursery Furniture (Placeholder)
                //m_BuyInfo.Add(new GenericBuyInfo(typeof(KoperNurseryFurniture), 7500, 5, 0xB7C, 0)); // Price 7.5k, 5 in stock
            }

            public override IShopSellInfo SellInfo
            {
                get { return m_SellInfo; }
            }

            public override List<GenericBuyInfo> BuyInfo
            {
                get { return m_BuyInfo; }
            }

            public class InternalBuyInfo : List<GenericBuyInfo>
            {
                public InternalBuyInfo()
                {
                   
                }
            }

            public class InternalSellInfo : GenericSellInfo
            {
                public InternalSellInfo()
                {
                }
            }
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            PlayerMobile player = e.Mobile as PlayerMobile;
            if (player != null && e.Speech.ToLower().Contains("hello"))
            {
                SayTo(player,
                    "Ah, you're interested in raising strong and noble beasts? Ask me about 'breeding', 'nursery', or 'adjectives'!");
            }

            base.OnSpeech(e);
        }

        public override bool HandlesOnSpeech(Mobile from)
        {
            return true; // Allows the NPC to respond to player speech
        }

        public override void OnDoubleClick(Mobile from)
        {
            PlayerMobile player = from as PlayerMobile;
            if (player != null)
            {
                player.SendGump(new OldShepherdGump(player));
            }
        }

        public static void SpawnShepherd()
        {
            Map map = Map.Sosaria; // Change to desired map
            Point3D location = new Point3D(2947, 1040, 0); // Change to desired coordinates

            OldShepherd existing = GetExistingShepherd();
            if (existing == null)
            {
                OldShepherd shepherd = new OldShepherd();
                shepherd.MoveToWorld(location, map);
                Console.WriteLine("[KoperPets] Old Shepherd spawned at {0}, {1}.", location.X, location.Y);
            }
            else
            {
                Console.WriteLine("[KoperPets] Old Shepherd already exists.");
            }
        }

        public static OldShepherd GetExistingShepherd()
        {
            foreach (Mobile m in World.Mobiles.Values)
            {
                if (m is OldShepherd)
                {
                    //m.Remove();
                    return (OldShepherd)m;
                }
            }

            return null;
        }

        public OldShepherd(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}