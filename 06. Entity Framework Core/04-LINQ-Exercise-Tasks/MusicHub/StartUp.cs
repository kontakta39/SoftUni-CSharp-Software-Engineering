namespace MusicHub;

using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Data;
using Initializer;

public class StartUp
{
    public static void Main()
    {
        MusicHubDbContext context =
            new MusicHubDbContext();

        DbInitializer.ResetDatabase(context);
        //int producerId = int.Parse(Console.ReadLine());
        int duration = int.Parse(Console.ReadLine());

        //Console.WriteLine(ExportAlbumsInfo(context, producerId));
        Console.WriteLine(ExportSongsAboveDuration(context, duration));
    }

    public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
    {
        var albumInfo = context.Producers
            .FirstOrDefault(a => a.Id == producerId)
            .Albums.Select(a => new 
            {
              a.Name,
              ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
              ProducerName = a.Producer.Name,
              AlbumSongs = a.Songs.Select(a => new 
              {
              a.Name,
              a.Price,
              WriterName = a.Writer.Name,
              }).OrderByDescending(a => a.Name).ThenBy(a => a.WriterName),
              TotalPrice = a.Price
            }).OrderByDescending(a => a.TotalPrice).ToList();

        StringBuilder sb = new();

        foreach (var album in albumInfo)
        {
            sb.AppendLine($"-AlbumName: {album.Name}");
            sb.AppendLine($"-ReleaseDate: {album.ReleaseDate}");
            sb.AppendLine($"-ProducerName: {album.ProducerName}");
            sb.AppendLine("-Songs:");

            int count = 1;

            foreach (var song in album.AlbumSongs)
            {
                sb.AppendLine($"---#{count++}");
                sb.AppendLine($"---SongName: {song.Name}");
                sb.AppendLine($"---Price: {song.Price:f2}");
                sb.AppendLine($"---Writer: {song.WriterName}");
            }

            sb.AppendLine($"-AlbumPrice: {album.TotalPrice:f2}");
        }

        return sb.ToString().TrimEnd();
    }

    public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
    {
        var songs = context.Songs
            .Where(s => s.Duration > TimeSpan.FromSeconds(duration))
            .Select(s => new
            {
                s.Name,
                PerformersName = s.SongPerformers.Select(s => new
                {
                    PerformerFirstName = s.Performer.FirstName,
                    PerformerLastName = s.Performer.LastName
                })
                .OrderBy(sp => sp.PerformerFirstName)
                .ThenBy(sp => sp.PerformerLastName).ToList(),
                WriterName = s.Writer.Name,
                ProducerName = s.Album.Producer.Name,
                Duration = s.Duration.ToString("c")
            }).OrderBy(s => s.Name).ThenBy(s => s.WriterName).ToList();

        StringBuilder sb = new();
        int count = 1;

        foreach (var song in songs)
        {
            sb.AppendLine($"-Song #{count++}");
            sb.AppendLine($"---SongName: {song.Name}");
            sb.AppendLine($"---Writer: {song.WriterName}");

            foreach (var performer in song.PerformersName)
            {
                sb.AppendLine($"---Performer: {performer.PerformerFirstName} {performer.PerformerLastName}");
            }
            
            sb.AppendLine($"---AlbumProducer: {song.ProducerName}");
            sb.AppendLine($"---Duration: {song.Duration}");
        }

        return sb.ToString().TrimEnd();
    }
}