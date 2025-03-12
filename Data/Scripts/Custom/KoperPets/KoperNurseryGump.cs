using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Custom.KoperPets
{
    public class KoperNurseryGump : Gump
    {
        private PlayerMobile _player;

        public KoperNurseryGump(PlayerMobile player) : base(50, 50)
        {
            _player = player;

            if (player == null)
            {
                return;
            }

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            // Background
            this.AddImage(3, 6, 7034);
            this.AddLabel(190, 20, 1153, "Koper Pet Nursery");

            // Column Titles
            this.AddLabel(50, 50, 1153, "Active Followers");
            this.AddLabel(300, 50, 1153, "Stored Pets");

            // Get Active Followers
            List<BaseCreature> activePets = new List<BaseCreature>();
            foreach (Mobile follower in player.AllFollowers)
            {
                BaseCreature pet = follower as BaseCreature;
                if (pet != null && pet.ControlMaster == player)
                {
                    activePets.Add(pet);
                }
            }

            // Get Stored Pets (Moved OUT of loop)
            List<KoperStoredPet> storedPets = KoperPetNursery.GetStoredPets(player);

            int yOffset = 80;

            // **Active Pets (Left Side)**
            if (activePets.Count > 0)
            {
                for (int i = 0; i < activePets.Count; i++)
                {
                    BaseCreature pet = activePets[i];

                    this.AddLabel(50, yOffset, 1153, pet.Name ?? "Unknown Pet");
                    this.AddButton(200, yOffset, 4005, 4006, 100 + i, GumpButtonType.Reply, 0); // Store Button
                    this.AddLabel(230, yOffset, 1153, "Store");

                    yOffset += 30;
                }
            }
            else
            {
                this.AddLabel(50, yOffset, 1153, "(No Active Pets)");
            }

            yOffset = 80;

            // **Stored Pets (Right Side)**
            if (storedPets.Count > 0)
            {
                for (int i = 0; i < storedPets.Count; i++)
                {
                    KoperStoredPet storedPet = storedPets[i];

                    this.AddLabel(300, yOffset, 1153, storedPet.PetName);
                    this.AddButton(450, yOffset, 4005, 4006, 200 + i, GumpButtonType.Reply, 0); // Retrieve Button
                    this.AddLabel(480, yOffset, 1153, "Retrieve");

                    yOffset += 30;
                }
            }
            else
            {
                this.AddLabel(300, yOffset, 1153, "(No Stored Pets)");
            }

            // **Close Button**
            this.AddButton(220, 350, 4017, 4018, 0, GumpButtonType.Reply, 0);
            this.AddLabel(250, 350, 1153, "Close");
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (_player == null || sender.Mobile != _player)
                return;

            int buttonID = info.ButtonID;

            if (buttonID == 0) // Close button
                return;

            // Store Pet
            if (buttonID >= 100 && buttonID < 200)
            {
                int petIndex = buttonID - 100;
                List<BaseCreature> activePets = new List<BaseCreature>();

                // Loop through player's followers
                foreach (Mobile follower in _player.AllFollowers)
                {
                    BaseCreature pet = follower as BaseCreature;
                    if (pet != null && pet.ControlMaster == _player)
                    {
                        activePets.Add(pet);
                    }
                }

                if (petIndex < activePets.Count)
                {
                    KoperPetNursery.StorePet(_player, activePets[petIndex]);
                    _player.SendGump(new KoperNurseryGump(_player)); // Refresh gump
                }
            }

            // Retrieve Pet
            else if (buttonID >= 200)
            {
                int petIndex = buttonID - 200;
                List<KoperStoredPet> storedPets = KoperPetNursery.GetStoredPets(_player);

                if (petIndex < storedPets.Count)
                {
                    KoperPetNursery.RetrievePet(_player, petIndex);
                    _player.SendGump(new KoperNurseryGump(_player)); // Refresh gump
                }
            }
        }
    }
}
