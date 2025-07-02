using NUnit.Framework;
using ApiTests.Core;
using ApiTests.Models;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace ApiTests.Tests
{
    [TestFixture]
    public class TrelloBoardTests
    {
        private ApiClient _client;
        private string _boardId;
        private string _listId;
        private string _cardId1;
        private string _cardId2;

        [SetUp]
        public void Setup()
        {
            _client = new ApiClient();
        }

        [Test]
        public async Task FullBoardCardFlow()
        {
            // 1. Create board
            var createBoardResponse = await _client.PostAsync("boards", new { name = "Test Board From API" });
            Console.WriteLine("📥 createBoardResponse.Content:");
            Console.WriteLine(createBoardResponse.Content); // gelen JSON'u terminale yazdır
            var board = JsonConvert.DeserializeObject<BoardModel>(createBoardResponse.Content);
            _boardId = board.Id;
            TestContext.WriteLine($"Board created: {_boardId}");

            // 2. Get default list
            var listResponse = await _client.GetAsync($"boards/{_boardId}/lists");
            Console.WriteLine("📥 listResponse.Content:");
            Console.WriteLine(listResponse.Content);
            var lists = JsonConvert.DeserializeObject<dynamic>(listResponse.Content);
            _listId = lists[0].id;
            TestContext.WriteLine($"List retrieved: {_listId}");

            // 3. Create 2 cards
            var card1Response = await _client.PostAsync("cards", new { name = "Card One", idList = _listId });
            var card2Response = await _client.PostAsync("cards", new { name = "Card Two", idList = _listId });
            Console.WriteLine("📥 card1Response.Content:");
            Console.WriteLine(card1Response.Content);
            var card1 = JsonConvert.DeserializeObject<CardModel>(card1Response.Content);
            var card2 = JsonConvert.DeserializeObject<CardModel>(card2Response.Content);
            _cardId1 = card1.Id;
            _cardId2 = card2.Id;
            TestContext.WriteLine($"Cards created: {_cardId1}, {_cardId2}");

            // 4. Update one randomly
            var cardToUpdate = new Random().Next(0, 2) == 0 ? _cardId1 : _cardId2;
            await _client.PutAsync($"cards/{cardToUpdate}", new { name = "Updated Card" });
            TestContext.WriteLine($"Card updated: {cardToUpdate}");

            // 5. Delete both cards
            await _client.DeleteAsync($"cards/{_cardId1}");
            await _client.DeleteAsync($"cards/{_cardId2}");
            TestContext.WriteLine("Cards deleted.");

            // 6. Delete board
            await _client.DeleteAsync($"boards/{_boardId}");
            TestContext.WriteLine("Board deleted.");
        }
    }
}