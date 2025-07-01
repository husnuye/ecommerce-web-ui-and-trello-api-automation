namespace ApiTests.Models
{
    // Represents a Trello board object
    public class BoardModel
    {
        public string Id { get; set; }     // Unique ID of the board
        public string Name { get; set; }   // Board title
        public string Url { get; set; }    // Board URL in Trello
    }
}