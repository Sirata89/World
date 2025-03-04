using System;
using Server.Network;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class DeathsMask : BoneHelm
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		public override int BaseColdResistance{ get{ return 11; } } 
		public override int BaseEnergyResistance{ get{ return 3; } } 
		public override int BasePhysicalResistance{ get{ return 15; } } 
		public override int BasePoisonResistance{ get{ return 12; } } 
		public override int BaseFireResistance{ get{ return 12; } } 
      
      [Constructable]
		public DeathsMask()
		{
          Name = "Mask of Death";
          Hue = 2518;
		  ArmorAttributes.MageArmor = 1;
		  Attributes.BonusInt = 5;
		  Attributes.DefendChance = 10;
		  Attributes.LowerManaCost = 10;
		  Attributes.NightSight = 1;
		  Attributes.SpellDamage = 15;
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			list.Add( 1070722, "Artefact");
        }

		public DeathsMask( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		private void Cleanup( object state ){ Item item = new Artifact_DeathsMask(); Server.Misc.Cleanup.DoCleanup( (Item)state, item ); }

public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader ); Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerStateCallback( Cleanup ), this );

			int version = reader.ReadInt();
		}
	}
}
