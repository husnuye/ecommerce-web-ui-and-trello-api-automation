namespace ApiTests.Models
{
    // Represents a Trello card object
    public class CardModel
    {
        public string Id { get; set; }        // Unique ID of the card
        public string Name { get; set; }      // Card title
        public string Desc { get; set; }      // Card description
        public string IdList { get; set; }    // List ID where the card belongs
    }
}