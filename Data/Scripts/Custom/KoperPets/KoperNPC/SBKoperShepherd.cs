using System;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.KoperPets
{
    public class SBKoperShepherd : SBInfo
    {
        private readonly List<GenericBuyInfo> m_BuyInfo = new List<GenericBuyInfo>();
        private readonly IShopSellInfo m_SellInfo = new GenericSellInfo();

        public SBKoperShepherd()
        {
            // Add Breeding Item (Placeholder)
            //m_BuyInfo.Add(new GenericBuyInfo(typeof(KoperBreedingItem), 5000, 10, 0xF8D, 0)); // Price 5k, 10 in stock

            // Add Nursery Furniture (Placeholder)
            //m_BuyInfo.Add(new GenericBuyInfo(typeof(KoperNurseryFurniture), 7500, 5, 0xB7C, 0)); // Price 7.5k, 5 in stock
        }

        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
        public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
    }
}
