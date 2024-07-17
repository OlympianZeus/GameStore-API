namespace GameStore.Api.Entities;

public class Game
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public int GenreId { get; set; }// 1 this code below is to ensure to 1 to 1 relationship between the Genre and the Game
    public Genre? Genre { get; set; } // 2

    public decimal Price { get; set; }
    public DateOnly ReleaseDate { get; set; }


}
//Note: Our game project is made up of 2 entities: Genre and Game