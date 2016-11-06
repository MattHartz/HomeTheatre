namespace HomeTheatre.DataAccess
{
    using Dto.Models;
    using System.Data.Entity;

    public class RoomContext : DbContext
    {
        // Your context has been configured to use a 'RoomContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'HomeTheatre.DAL.RoomContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'RoomContext' 
        // connection string in the application configuration file.
        public RoomContext()
            : base("name=RoomContext")
        {
        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Connection> Connections { get; set; }        

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}