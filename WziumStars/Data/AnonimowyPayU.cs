﻿using System;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.IO;
using System.Text;
using WziumStars.Models.ViewModels;

namespace WziumStars.Data
{
    public class AnonimowyPayU
    {
        public static async Task<string> GetAccessToken()
        {
            var baseAddress = new Uri("https://secure.snd.payu.com/");

            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {


                using (var content = new StringContent("grant_type=client_credentials&client_id=395609&client_secret=1c1d2e6551384b4e6e9c58c412746a24", System.Text.Encoding.Default, "application/x-www-form-urlencoded"))
                {
                    using (var response = await httpClient.PostAsync("pl/standard/user/oauth/authorize", content))
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        return responseData;
                    }
                }
            }
        }

        public static async Task<string> CreateNewOrder(string accessToken, string tokenType, OrderDetailsAnonimowyKoszyk detailsCard, string ip)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in detailsCard.listCart)
            {
                sb.Append("{\"name\":");
                sb.Append("\"" + item.Produkt.Name + "\",  ");
                sb.Append("\"unitPrice\":");
                sb.Append("\"" + item.Produkt.Price * 100 + "\",  ");
                sb.Append("\"quantity\":");
                sb.Append("\"" + item.Count + "\"  },");
            }
            sb.Length--;
            string products = sb.ToString();

            var baseAddress = new Uri("https://secure.snd.payu.com/");

            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {


                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("authorization", tokenType + " " + accessToken);

                using (var content = new StringContent(
                    "{  \"notifyUrl\": \"https://localhost:44361/Klient/Zamowienie/Potwierdzenie/" + detailsCard.OrderHeader.Id + "\", " +
                    " \"customerIp\": \"" + ip + "\",  " +
                    "\"merchantPosId\": \"395609\",  " +
                    "\"description\": \"Florist shop\",  " +
                    "\"currencyCode\": \"PLN\",  " +
                    "\"totalAmount\": \"" + (detailsCard.OrderHeader.OrderTotal * 100).ToString() + "\",  " +
                    " \"continueUrl\": \"https://localhost:44361/Klient/Zamowienie/Potwierdzenie/" + detailsCard.OrderHeader.Id + "\",  " +
                    "\"buyer\": " +
                    "{" +
                    "\"email\": \"" + detailsCard.OrderHeader.ApplicationUser.Email + "\"," +
                    "\"phone\": \"" + detailsCard.OrderHeader.PhoneNumber + "\"," +
                    "\"firstName\": \"" + detailsCard.OrderHeader.ApplicationUser.FirstName + "\"," +
                    "\"lastName\": \"" + detailsCard.OrderHeader.ApplicationUser.LastName + "\"" +
                    "}," +
                    "\"products\": " +
                    "[" + products + "]}", System.Text.Encoding.Default, "application/json"))
                {
                    using (var response = await httpClient.PostAsync("api/v2_1/orders/", content))
                    {
                        string Url = response.RequestMessage.RequestUri.ToString();
                        return Url;
                    }
                }
            }
        }
    }
}
