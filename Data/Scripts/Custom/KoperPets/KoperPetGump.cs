//TODO: make the gump show a picture of the pet, move the pet name to top of the gump, move breeding info to a second page 
//FIXME: add caps for resistance, at 90 for each
//TODO: Fix dmg other stat scalling with stat increase, 1 str should give +2 hits, +min/max dmg.

using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Custom.KoperPets;

namespace Server.Custom.KoperPets
{
    public class KoperPetGump : Gump
    {
        private PlayerMobile m_Player;
        private BaseCreature m_Pet;
        private KoperPetData m_PetData;
        private bool canSpendPoints;
        int yOffset = 162; // Start position

        public static int GetCenteredX(string text, int gumpWidth)
        {
            int estimatedTextWidth = text.Length * 6; // Approx. 6px per character
            return (gumpWidth / 2) - (estimatedTextWidth / 2);
        }

        public KoperPetGump(PlayerMobile player, BaseCreature pet) : base(50, 50)
        {
            if (player == null || pet == null || !pet.Controlled || pet.ControlMaster != player)
            {
                player.SendMessage("Invalid pet selection.");
                return;
            }

            m_Pet = pet;
            m_PetData = KoperPetManager.GetPetData(pet);
            m_Player = player;
            canSpendPoints = (m_PetData.Traits > 0);
            string adjectiveText = KoperPetNaming.GetAdjectiveDescription(m_PetData.Adjective);
            List<string> splitText = KoperPetNaming.SplitToLines(adjectiveText, 30);
            int gumpWidth = 550;
            int leftColumn = 130;
            string pedigreeText = KoperPetNaming.GetPedigreeName(m_PetData.Pedigree) + " - " +
                                  KoperPetManager.GetPedigree(m_Pet).ToString();
            string petGender = KoperPetManager.GetGender(m_PetData);
            string petLevel = "Level: " + m_PetData.Level.ToString();
            string petXP = string.Format("Exp: {0}/{1}", m_PetData.Experience, KoperPetManager.GetXPNeeded(m_PetData));
            string petHits = string.Format("{0}/{1}", m_Pet.Hits, m_Pet.HitsMax);
            string petStam = string.Format("{0}/{1}", m_Pet.Stam, m_Pet.StamMax);
            string petMana = string.Format("{0}/{1}", m_Pet.Mana, m_Pet.ManaMax);

            TimeSpan cooldownRemaining = KoperBreeding.GetRemainingBreedingCooldown(m_Pet);
            string cooldownText = cooldownRemaining.TotalSeconds > 0
                ? string.Format("{0}h {1}m", (int)cooldownRemaining.TotalHours, cooldownRemaining.Minutes)
                : "Ready to Breed!";


            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;
            this.AddPage(0);
            this.AddImage(3, 6, 7034, Server.Misc.PlayerSettings.GetGumpHue( m_Player ));
            this.AddLabel(GetCenteredX("Lineage Tracking", gumpWidth), 17, 1153, @"Lineage Tracking"); //220
            this.AddLabel(GetCenteredX(m_Pet.Name, gumpWidth), 40, 1153, m_Pet.Name);
            this.AddImage(173, 20, 57);
            this.AddImage(346, 20, 59);
            this.AddLabel(GetCenteredX(petLevel, gumpWidth), 77, 1153, petLevel);
            this.AddLabel(409, 35, 1153, string.Format("Lineage Points: {0}", m_PetData.Traits));
            this.AddLabel(GetCenteredX(pedigreeText, gumpWidth), 98, 1153, pedigreeText);
            this.AddImage(225, 40, 93);
            //this.AddLabel(25, 35, 1153, @"Exp:");
            //this.AddLabel(60, 35, 1153,
                //string.Format("{0}/{1}", m_PetData.Experience, KoperPetManager.GetXPNeeded(m_PetData)));
            this.AddLabel(GetCenteredX(petXP, leftColumn), 35, 1153, petXP); // FIXME move all these down so we can double stack xp string and ints
            this.AddLabel(GetCenteredX("Hit Points", leftColumn), 65, 1153, @"Hit Points");
            this.AddLabel(GetCenteredX(petHits, leftColumn), 85, 1153, petHits);
            this.AddLabel(GetCenteredX("Stamina", leftColumn), 125, 1153, @"Stamina");
            this.AddLabel(GetCenteredX(petStam, leftColumn), 145, 1153, petStam);
            this.AddLabel(GetCenteredX("Mana", leftColumn), 185, 1153, @"Mana");
            this.AddLabel(GetCenteredX(petMana, leftColumn), 205, 1153, petMana);
            this.AddLabel(GetCenteredX(KoperPetNaming.GetAdjectiveName(m_PetData), gumpWidth), 119, 1153,
                KoperPetNaming.GetAdjectiveName(m_PetData));
            foreach (string line in splitText)
            {
                this.AddLabel(GetCenteredX(line, gumpWidth), yOffset, 1153, line);
                yOffset += 20; // Move down for next line
            }

            if (canSpendPoints)
            {
                // -- Attributes --
                // The attribute labels are at (25,y) and the current values at (105,y).
                // Add a button to the left (for example at x=5) for each attribute.
                this.AddButton(5, 255, 5601, 5605, 100, GumpButtonType.Reply, 0); // Increase Strength
                this.AddButton(5, 280, 5601, 5605, 101, GumpButtonType.Reply, 0); // Increase Dexterity
                this.AddButton(5, 305, 5601, 5605, 102, GumpButtonType.Reply, 0); // Increase Intelligence

                // -- Resistances --
                // The resistance labels start at (415,y). Place the increase buttons to the left.
                // Adjust the X coordinate so they line up nicely with the labels.
                this.AddButton(380, 152, 5601, 5605, 103, GumpButtonType.Reply, 0); // Increase Physical Resistance
                this.AddButton(380, 176, 5601, 5605, 104, GumpButtonType.Reply, 0); // Increase Fire Resistance
                this.AddButton(380, 199, 5601, 5605, 105, GumpButtonType.Reply, 0); // Increase Cold Resistance
                this.AddButton(380, 221, 5601, 5605, 106, GumpButtonType.Reply, 0); // Increase Poison Resistance
                this.AddButton(380, 245, 5601, 5605, 107, GumpButtonType.Reply, 0); // Increase Energy Resistance
            }

            this.AddLabel(25, 255, 1153, @"Strength");
            this.AddLabel(25, 280, 1153, @"Dexterity");
            this.AddLabel(25, 305, 1153, @"Intelligence");
            this.AddLabel(105, 255, 1153, m_Pet.RawStr.ToString());
            this.AddLabel(105, 280, 1153, m_Pet.RawDex.ToString());
            this.AddLabel(105, 305, 1153, m_Pet.RawInt.ToString());
            this.AddButton(249, 356, 27, 27, 1, GumpButtonType.Page, 0);
            this.AddButton(453, 375, 247, 248, 2, GumpButtonType.Reply, 0);
            this.AddLabel(433, 65, 1153, @"Damage");
            this.AddLabel(437, 88, 1153, string.Format("{0}-{1}", m_Pet.DamageMin, m_Pet.DamageMax));
            this.AddLabel(423, 125, 1153, @"Resistances");
            this.AddLabel(252, 331, 1153, @"Breed");
            this.AddLabel(298, 378, 1153, @"Cooldown:");
            this.AddLabel(368, 379, 1153, cooldownText);
            this.AddLabel(415, 152, 1153, string.Format("Physical:  {0}", m_Pet.PhysicalResistance));
            this.AddLabel(415, 176, 1153, string.Format("Fire:     {0}", m_Pet.FireResistance));
            this.AddLabel(415, 199, 1153, string.Format("Cold:     {0}", m_Pet.ColdResistance));
            this.AddLabel(415, 221, 1153, string.Format("Poison:   {0}", m_Pet.PoisonResistance));
            this.AddLabel(415, 245, 1153, string.Format("Energy:   {0}", m_Pet.EnergyResistance));
            this.AddLabel(GetCenteredX(petGender, gumpWidth), 138, 1153, petGender);
            this.AddLabel(26, 338, 1153, string.Format("MaxLevel: {0}", m_PetData.MaxLevel));
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == 1)
                //KoperPetCommands.BreedPet_OnCommand(sender, info);
                return;
            else if (info.ButtonID == 2)
                sender.Mobile.CloseGump(typeof(KoperPetGump));
            else if (info.ButtonID >= 100 && info.ButtonID <= 107)
            {
                switch (info.ButtonID)
                {
                    case 100: // Increase Strength
                        // Your logic to increase m_Pet.RawStr if allowed
                        break;
                    case 101: // Increase Dexterity
                        // Your logic to increase m_Pet.RawDex if allowed
                        break;
                    case 102: // Increase Intelligence
                        // Your logic to increase m_Pet.RawInt if allowed
                        break;
                    case 103: // Increase Physical Resistance
                        // Increase m_Pet.PhysicalResistance logic
                        break;
                    case 104: // Increase Fire Resistance
                        // Increase m_Pet.FireResistance logic
                        break;
                    case 105: // Increase Cold Resistance
                        // Increase m_Pet.ColdResistance logic
                        break;
                    case 106: // Increase Poison Resistance
                        // Increase m_Pet.PoisonResistance logic
                        break;
                    case 107: // Increase Energy Resistance
                        // Increase m_Pet.EnergyResistance logic
                        break;
                    default:
                        // Handle other buttons or do nothing
                        break;

                }
                sender.Mobile.SendGump(new KoperPetGump(m_Player, m_Pet));
            }
        }
    }
}