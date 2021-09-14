using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using Xunit;

namespace ProductManager
{
    public class UnitTest1
    {
        [Fact]
        public void ifBroadband1Equal10()
        {
            var priceManager = new PriceManager();
            priceManager.AddBasket("Broadband 1");
            var priceBasket = priceManager.giveTotalPrice();

            Assert.Equal(10, priceBasket);
        }

        [Fact]
        public void ifBroadband1AndBroadband2Equal30()
        {
            var priceManager = new PriceManager();
            priceManager.AddBasket("Broadband 1");
            priceManager.AddBasket("Broadband 2");
            var priceBasket = priceManager.giveTotalPrice();

            Assert.Equal(30, priceBasket);
        }

        //If you order Broadband and Line Rental together. You get a £2 discoun
        [Fact]
        public void ifBroadband1AndLineRental1Discount2()
        {
            var priceManager = new PriceManager();
            priceManager.AddBasket("Broadband 1");
            priceManager.AddBasket("Line Rental 1");
            var priceBasket = priceManager.giveTotalPrice();

            Assert.Equal(13, priceBasket);
        }

        [Fact]
        public void ifBroadband2AndLineRental2Discount2()
        {
            var priceManager = new PriceManager();
            priceManager.AddBasket("Broadband 2");
            priceManager.AddBasket("Line Rental 2");
            var priceBasket = priceManager.giveTotalPrice();

            Assert.Equal(25, priceBasket);
        }

        [Fact]
        public void ifBroadband2AndLineRental2AndRouterDiscount50()
        {
            var priceManager = new PriceManager();
            priceManager.AddBasket("Broadband 2");
            priceManager.AddBasket("Line Rental 2");
            priceManager.AddBasket("Router");
            var priceBasket = priceManager.giveTotalPrice();

            Assert.Equal(25, priceBasket);
        }

        [Fact]
        public void ifBroadband1AndBroadband1AndDiscount1()
        {
            var priceManager = new PriceManager();
            priceManager.AddBasket("Broadband 1");
            priceManager.AddBasket("Broadband 1");
            var priceBasket = priceManager.giveTotalPrice();

            Assert.Equal(19, priceBasket);
        }
        [Fact]
        public void ifBroadband1AndBroadband1AndDiscount1X2()
        {
            var priceManager = new PriceManager();
            priceManager.AddBasket("Broadband 1");
            priceManager.AddBasket("Broadband 1");
            priceManager.AddBasket("Broadband 1");
            priceManager.AddBasket("Broadband 1");
            var priceBasket = priceManager.giveTotalPrice();

            Assert.Equal(38, priceBasket);
        }
        [Fact]
        public void ifBroadband2AndBroadband2AndDiscount4()
        {
            var priceManager = new PriceManager();
            priceManager.AddBasket("Broadband 2");
            priceManager.AddBasket("Broadband 2");
            var priceBasket = priceManager.giveTotalPrice();

            Assert.Equal(36, priceBasket);
        }
        [Fact]
        public void ifBroadband2AndBroadband2AndDiscount4x2()
        {
            var priceManager = new PriceManager();
            priceManager.AddBasket("Broadband 2");
            priceManager.AddBasket("Broadband 2");
            priceManager.AddBasket("Broadband 2");
            priceManager.AddBasket("Broadband 2");
            var priceBasket = priceManager.giveTotalPrice();

            Assert.Equal(72, priceBasket);
        }
        [Fact]
        public void ifBroadband1RouterLineRental1Broadband1RouterLineRental1Router()
        {
            var priceManager = new PriceManager();
            priceManager.AddBasket("Broadband 1");
            priceManager.AddBasket("Router");
            priceManager.AddBasket("Line Rental 1");
            priceManager.AddBasket("Broadband 1");
            priceManager.AddBasket("Router");
            priceManager.AddBasket("Line Rental 1");
            priceManager.AddBasket("Router");
            var priceBasket = priceManager.giveTotalPrice();

            Assert.Equal(75, priceBasket);
        }
        public class PriceManager
        {
            private static Dictionary<string, int> bookDirectory = new Dictionary<string, int>()
            {
                {"Broadband 1", 10},
                {"Broadband 2", 20},
                {"Line Rental 1", 5},
                {"Line Rental 2", 7},
                {"Router", 50}
            };

           List<string> basket = new List<string>();


            public void AddBasket(string product)
            {
                basket.Add(product);

            }

            public int giveTotalPrice()
            {
                int price = 0;
                foreach (var item in basket)
                {
                    int itemBasketPrice = translate(item);
                    price += itemBasketPrice;
                }
                int broadband = basket.Count(str => str.Contains("Broadband 1" ) || str.Contains("Broadband 2"));
                int lineRental = basket.Count(str => str.Contains("Line Rental 1") || str.Contains("Line Rental 2"));
                int router = basket.Count(str => str.Contains("Router"));

                //If you order Broadband and Line Rental with a Router. The Router is free.

                List<int> elementList = new List<int>();
                elementList.Add(broadband);
                elementList.Add(lineRental);
                elementList.Add(router);
                
                if (broadband > 0 && lineRental > 0 && router > 0)
                {
                    var minValue = elementList.Min();
                    price -= bookDirectory["Router"] * minValue;
                }

                //If you order Broadband and Line Rental together. You get a £2 discount.

                if (broadband > 0 && lineRental > 0)
                {
                    price -= Math.Min(broadband, lineRental)*2;
                }

                //For every 2 Broadband 1's you order. You get a discount of £1.
              
                int countBroadband1 = basket.Count(str => str.Contains("Broadband 1"));
                if (countBroadband1 > 1)
                {
                    double countBroadband1Divided2 = countBroadband1 / 2;
                    
                    price -= Convert.ToInt32(Math.Truncate(countBroadband1Divided2)); 
                }

                //For every 2 Broadband 2's you order. You get a discount of £4.

                int countBroadband2 = basket.Count(x => x.Contains("Broadband 2"));
                if (countBroadband2 > 1)
                {
                    double countBroadband2Divided2 = countBroadband2 / 2;

                    price -= Convert.ToInt32(Math.Truncate(countBroadband2Divided2))*4;
                }

                return price;

            }

            public int translate(string value)
            {
                return bookDirectory[$"{value}"];
            }



        }

    }


}
