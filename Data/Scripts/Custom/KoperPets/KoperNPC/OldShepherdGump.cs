using System;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Custom.KoperPets
{
    public class OldShepherdGump : Gump
    {
        private PlayerMobile _player;

        public OldShepherdGump(PlayerMobile player) : base(100, 100)
        {
            _player = player;

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);
            AddBackground(0, 0, 400, 300, 9270); // Gump background

            AddLabel(150, 20, 1153, "The Old Shepherd");

            // Dialogue
            AddHtml(20, 50, 360, 80, 
                "<BASEFONT COLOR=#FFFFFF>Ah, I see you're interested in the ways of raising strong companions! " +
                "What would you like to know?</BASEFONT>", false, false);

            // Buttons for more info
            AddButton(40, 140, 4005, 4006, 1, GumpButtonType.Reply, 0);
            AddLabel(75, 140, 1153, "Tell me about breeding");

            AddButton(40, 170, 4005, 4006, 2, GumpButtonType.Reply, 0);
            AddLabel(75, 170, 1153, "Tell me about the nursery");

            AddButton(40, 200, 4005, 4006, 3, GumpButtonType.Reply, 0);
            AddLabel(75, 200, 1153, "Explain adjectives");

            // Close button
            AddButton(150, 250, 4017, 4018, 0, GumpButtonType.Reply, 0);
            AddLabel(180, 250, 1153, "Close");
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (_player == null || sender.Mobile != _player)
                return;

            switch (info.ButtonID)
            {
                case 1:
                    _player.SendMessage("Breeding allows two compatible pets to produce offspring with inherited traits and strengths.");
                    break;
                case 2:
                    _player.SendMessage("The nursery is where you can safely store your pets. You can buy special furniture to access it from anywhere.");
                    break;
                case 3:
                    _player.SendMessage("Adjectives define your pet's strengths. Some improve power, others increase speed, and rare ones grant mythical abilities!");
                    break;
                default:
                    return;
            }

            _player.SendGump(new OldShepherdGump(_player)); // Keep the menu open
        }
    }
}
